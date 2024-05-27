using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RecycleThis;

internal class JobDriver_RecycleThing : JobDriver
{
    private const TargetIndex ThingToDestroyInd = TargetIndex.A;

    private const TargetIndex WorkBenchInd = TargetIndex.B;

    private const TargetIndex InteractionCellInd = TargetIndex.C;

    private float totalNeededWork;
    private float workLeft;

    protected Thing Target => job.targetA.Thing;

    protected Thing Workbench => job.targetB.Thing;

    protected DesignationDef Designation => DesignationDefOf.RecycleThisRecycle;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(job.GetTarget(WorkBenchInd), job, 1, 1) &&
               pawn.Reserve(job.GetTarget(ThingToDestroyInd), job, 1, 1);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnThingMissingDesignation(ThingToDestroyInd, Designation);
        this.FailOnDestroyedNullOrForbidden(ThingToDestroyInd);
        if (job.targetA.Thing.Smeltable)
        {
            this.FailOnUnpoweredWorkbench(Workbench);
        }

        yield return Toils_Reserve.Reserve(ThingToDestroyInd);
        yield return Toils_Goto.GotoThing(ThingToDestroyInd, PathEndMode.ClosestTouch)
            .FailOnSomeonePhysicallyInteracting(ThingToDestroyInd);
        yield return Toils_Haul.StartCarryThing(ThingToDestroyInd);
        yield return Toils_Goto.GotoThing(WorkBenchInd, PathEndMode.InteractionCell)
            .FailOnSomeonePhysicallyInteracting(WorkBenchInd);
        yield return Toils_JobTransforms.SetTargetToIngredientPlaceCell(WorkBenchInd, ThingToDestroyInd,
            InteractionCellInd);
        yield return Toils_Haul.PlaceHauledThingInCell(WorkBenchInd, null, false);
        yield return Toils_Reserve.Reserve(ThingToDestroyInd);
        var doWork = new Toil().FailOnForbidden(ThingToDestroyInd);
        doWork.initAction = delegate
        {
            var recycleSpeed = JobDefOf.RecycleThisRecycle.GetModExtension<RecycleThisModExtension>()?.RecycleSpeed;
            if (recycleSpeed != null)
            {
                totalNeededWork = recycleSpeed.Value;
            }

            workLeft = totalNeededWork;
        };
        doWork.tickAction = delegate
        {
            var actor = doWork.actor;
            _ = actor.jobs.curJob;
            float statValue;
            if (Target.def?.recipeMaker?.workSpeedStat != null &&
                Workbench.GetStatValue(StatDefOf.WorkTableWorkSpeedFactor) > 0.01)
            {
                statValue = actor.GetStatValue(Target.def.recipeMaker.workSpeedStat);
                statValue *= Workbench.GetStatValue(StatDefOf.WorkTableWorkSpeedFactor);
            }
            else
            {
                statValue = 1f;
            }

            workLeft -= statValue;
            actor.GainComfortFromCellIfPossible(true);
            if (workLeft <= 0f)
            {
                doWork.actor.jobs.curDriver.ReadyForNextToil();
            }
        };
        doWork.defaultCompleteMode = ToilCompleteMode.Never;
        doWork.WithProgressBar(ThingToDestroyInd, () => 1f - (workLeft / totalNeededWork));
        if (job.targetA.Thing.Smeltable)
        {
            doWork.WithEffect(() => EffecterDefOf.Smelt, WorkBenchInd);
            doWork.PlaySustainerOrSound(() => SoundDefOf.Recipe_Smelt);
        }
        else
        {
            doWork.WithEffect(() => RimWorld.EffecterDefOf.Sow, WorkBenchInd);
            doWork.PlaySustainerOrSound(() => SoundDefOf.Recipe_Tailor);
        }

        yield return doWork;
        yield return new Toil
        {
            initAction = delegate
            {
                var efficiency = Target.def.recipeMaker.efficiencyStat != null
                    ? Target.GetStatValue(Target.def.recipeMaker.efficiencyStat)
                    : 1f;
                var items = RecycleThis.SmeltProducts(Target, efficiency);
                foreach (var item in items)
                {
                    GenSpawn.Spawn(item, pawn.Position, Map);
                }

                Target.Destroy();
                Map.designationManager.RemoveAllDesignationsOn(Target);
            }
        };
        yield return Toils_Reserve.Release(WorkBenchInd);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref workLeft, "workLeft");
        Scribe_Values.Look(ref totalNeededWork, "totalNeededWork");
    }
}