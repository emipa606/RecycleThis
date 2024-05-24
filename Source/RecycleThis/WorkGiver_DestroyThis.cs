using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RecycleThis;

public class WorkGiver_DestroyThis : WorkGiver_Scanner
{
    protected JobDef DestroyThing => JobDefOf.RecycleThisDestroy;

    protected DesignationDef Destroy => DesignationDefOf.RecycleThisDestroy;

    public override PathEndMode PathEndMode => PathEndMode.Touch;

    public override bool ShouldSkip(Pawn pawn, bool forced = false)
    {
        return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(Destroy);
    }

    public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
    {
        foreach (var item in pawn.Map.designationManager.SpawnedDesignationsOfDef(Destroy))
        {
            yield return item.target.Thing;
        }
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        if (!pawn.Map.designationManager.AllDesignationsOn(t).Any(x => x.def == DesignationDefOf.RecycleThisDestroy))
        {
            return null;
        }

        if (!pawn.CanReserve(t, 1, 1, null, forced) || t.IsBurning() || t.IsForbidden(pawn))
        {
            JobFailReason.Is("RecycleThisItemIsUnavailable".Translate(t));
            return null;
        }

        var thing = RecycleThisUtility.ClosestSuitableWorkBenchDestroy(t, pawn, forced);
        if (thing == null)
        {
            JobFailReason.Is("RecycleThisNoAvailableWorkbenches".Translate());
            return null;
        }

        var compRefuelable = thing.TryGetComp<CompRefuelable>();
        if (compRefuelable is { HasFuel: false })
        {
            if (RefuelWorkGiverUtility.CanRefuel(pawn, thing, forced))
            {
                return RefuelWorkGiverUtility.RefuelJob(pawn, thing, forced);
            }

            JobFailReason.Is("RecycleThisCannotRefuel".Translate(thing));
            return null;
        }

        var job = JobMaker.MakeJob(DestroyThing, t, thing, thing.InteractionCell);
        job.count = 1;
        return job;
    }
}