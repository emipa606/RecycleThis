using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace RecycleThis;

public static class RecycleThisUtility
{
    public static readonly List<ThingDef> destroyBenches;

    public static readonly List<ThingDef> smeltBenches;

    public static readonly List<ThingDef> scrapBenches;

    private static readonly ThingRequest thingRequest;

    private static readonly FieldInfo allRecipesCachedInfo = AccessTools.Field(typeof(ThingDef), "allRecipesCached");

    static RecycleThisUtility()
    {
        thingRequest = ThingRequest.ForGroup(ThingRequestGroup.Undefined);
        smeltBenches = [];
        scrapBenches = [];
        destroyBenches = [];
        foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
        {
            if (thingDef.AllRecipes.Any(x => x == RecipeDefOf.SmeltWeapon || x == RecipeDefOf.SmeltApparel))
            {
                smeltBenches.Add(thingDef);
            }

            if (thingDef.AllRecipes.Any(x =>
                    x.defName is "Make_Apparel_BasicShirt" or "Make_Apparel_TribalA"))
            {
                scrapBenches.Add(thingDef);
                _ = thingDef.AllRecipes;
                var currentRecipes = (List<RecipeDef>)allRecipesCachedInfo.GetValue(thingDef);
                currentRecipes.Insert(0, RecipeDefOf.RecycleThis);
                allRecipesCachedInfo.SetValue(thingDef, currentRecipes);
            }

            if (thingDef.AllRecipes.Any(x => x == RecipeDefOf.SmeltWeapon || x == RecipeDefOf.BurnApparel))
            {
                destroyBenches.Add(thingDef);
            }
        }

        Log.Message(
            $"[RecycleThis]: Added {destroyBenches.Count} things to destroy-benches, {scrapBenches.Count} things to scrap-benches and {smeltBenches.Count} things to smelt-benches");
    }

    public static void CreateLists()
    {
        foreach (var allDef in DefDatabase<RecipeDef>.AllDefs)
        {
            if (allDef.defName == "RecycleThis")
            {
                allDef.recipeUsers.AddRange(scrapBenches);
            }
        }
    }

    public static void FailOnUnpoweredWorkbench<T>(this T f, Thing x) where T : IJobEndable
    {
        f.AddEndCondition(() =>
            ((IBillGiver)x).CurrentlyUsableForBills()
                ? JobCondition.Ongoing
                : JobCondition.Ongoing | JobCondition.Succeeded);
    }

    private static TraverseParms TraverseParms(Pawn pawn)
    {
        var result = default(TraverseParms);
        result.pawn = pawn;
        result.mode = TraverseMode.ByPawn;
        result.maxDanger = Danger.Unspecified;
        result.fenceBlocked = false;
        result.canBashFences = false;
        result.canBashDoors = false;
        return result;
    }

    public static Thing ClosestSuitableWorkbench(Thing thing, Pawn pawn, List<ThingDef> benches, bool forced = false)
    {
        var list = new List<Thing>();
        foreach (var bench in benches)
        {
            list.AddRange(pawn.Map.listerThings.ThingsOfDef(bench));
        }

        return GenClosest.ClosestThingReachable(thing.Position, thing.Map, thingRequest, PathEndMode.InteractionCell,
            TraverseParms(pawn), 9999f, val, list);

        bool val(Thing bench)
        {
            return pawn.CanReserve(bench, 1, -1, null, forced) && ((IBillGiver)bench).UsableForBillsAfterFueling();
        }
    }

    public static Thing ClosestSuitableWorkBenchDestroy(Thing thing, Pawn pawn, bool forced = false)
    {
        return ClosestSuitableWorkbench(thing, pawn, destroyBenches, forced);
    }

    public static Thing ClosestSuitableWorkbenchSmelt(Thing thing, Pawn pawn, bool forced = false)
    {
        return ClosestSuitableWorkbench(thing, pawn, smeltBenches, forced);
    }

    public static Thing ClosestSuitableWorkbenchScrap(Thing thing, Pawn pawn, bool forced = false)
    {
        return ClosestSuitableWorkbench(thing, pawn, scrapBenches, forced);
    }
}