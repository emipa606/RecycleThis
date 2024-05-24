using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RecycleThis;

public class WorkGiver_RecycleThis : WorkGiver_Scanner
{
    protected JobDef RecycleThing => JobDefOf.RecycleThisRecycle;

    protected DesignationDef Recycle => DesignationDefOf.RecycleThisRecycle;

    public override PathEndMode PathEndMode => PathEndMode.Touch;

    public override bool ShouldSkip(Pawn pawn, bool forced = false)
    {
        return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(Recycle);
    }

    public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
    {
        foreach (var item in pawn.Map.designationManager.SpawnedDesignationsOfDef(Recycle))
        {
            yield return item.target.Thing;
        }
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        if (!pawn.Map.designationManager.AllDesignationsOn(t).Any(x => x.def == DesignationDefOf.RecycleThisRecycle))
        {
            return null;
        }

        if (!pawn.CanReserve(t, 1, 1, null, forced) || t.IsBurning() || t.IsForbidden(pawn))
        {
            JobFailReason.Is("RecycleThisItemIsUnavailable".Translate(t));
            return null;
        }

        var thing = !t.Smeltable
            ? RecycleThisUtility.ClosestSuitableWorkbenchScrap(t, pawn, forced)
            : RecycleThisUtility.ClosestSuitableWorkbenchSmelt(t, pawn, forced);
        if (thing == null)
        {
            JobFailReason.Is("RecycleThisNoAvailableWorkbenches".Translate());
            return null;
        }

        var compRefuelable = thing.TryGetComp<CompRefuelable>();
        var obj = compRefuelable != null ? new bool?(!compRefuelable.HasFuel) : null;

        if (obj.GetValueOrDefault())
        {
            if (RefuelWorkGiverUtility.CanRefuel(pawn, thing, forced))
            {
                return RefuelWorkGiverUtility.RefuelJob(pawn, thing, forced);
            }

            JobFailReason.Is("RecycleThisCannotRefuel".Translate(thing));
            return null;
        }

        var job = JobMaker.MakeJob(RecycleThing, t, thing, thing.InteractionCell);
        job.count = 1;
        return job;
    }
}