using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RecycleThis;

internal class JobDriver_DestroyThing : JobDriver
{
    private const TargetIndex ThingToDestroyInd = TargetIndex.A;

    private const TargetIndex WorkBenchInd = TargetIndex.B;

    private const TargetIndex InteractionCellInd = TargetIndex.C;

    private float totalNeededWork;
    private float workLeft;

    private Thing Target => job.targetA.Thing;

    private Thing Workbench => job.targetB.Thing;

    private static DesignationDef Designation => DesignationDefOf.RecycleThisDestroy;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(job.GetTarget(WorkBenchInd), job, 1, 1) &&
               pawn.Reserve(job.GetTarget(ThingToDestroyInd), job, 1, 1);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnThingMissingDesignation(ThingToDestroyInd, Designation);
        this.FailOnForbidden(ThingToDestroyInd);
        this.FailOnUnpoweredWorkbench(Workbench);
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
            var destroySpeed = JobDefOf.RecycleThisDestroy?.GetModExtension<RecycleThisModExtension>()?.DestroySpeed;
            if (destroySpeed != null)
            {
                totalNeededWork = destroySpeed.Value;
            }

            workLeft = totalNeededWork;
        };
        doWork.tickIntervalAction = delegate(int delta)
        {
            var actor = doWork.actor;
            _ = actor.jobs.curJob;
            workLeft -= delta;
            actor.GainComfortFromCellIfPossible(delta, true);
            if (workLeft <= 0f)
            {
                doWork.actor.jobs.curDriver.ReadyForNextToil();
            }
        };
        doWork.defaultCompleteMode = ToilCompleteMode.Never;
        doWork.WithProgressBar(ThingToDestroyInd, () => 1f - (workLeft / totalNeededWork));
        doWork.WithEffect(() => EffecterDefOf.Cremate, WorkBenchInd);
        doWork.PlaySustainerOrSound(() => SoundDefOf.Recipe_Cremate);
        yield return doWork;
        yield return new Toil
        {
            initAction = delegate
            {
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