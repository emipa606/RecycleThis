using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RecycleThis;

[StaticConstructorOnStartup]
public static class RecycleThis
{
    static RecycleThis()
    {
        new Harmony("RecycleThis.patch").PatchAll(Assembly.GetExecutingAssembly());
        RuntimeHelpers.RunClassConstructor(typeof(RecycleThisUtility).TypeHandle);
    }


    public static IEnumerable<Thing> SmeltProducts(Thing thing, float efficiency)
    {
        var def = thing.def;
        var costListAdj = def.CostListAdjusted(thing.Stuff);
        foreach (var defCountClass in costListAdj)
        {
            if (!RecycleThisMod.Instance.Settings.GiveComponents && defCountClass.thingDef.intricate)
            {
                continue;
            }

            var num = GenMath.RoundRandom(defCountClass.count * 0.25f * efficiency);
            if (num <= 0)
            {
                continue;
            }

            var products = ThingMaker.MakeThing(defCountClass.thingDef);
            products.stackCount = num;
            yield return products;
        }

        if (def.smeltProducts == null)
        {
            yield break;
        }

        foreach (var thingDefCountClass in def.smeltProducts)
        {
            var products = ThingMaker.MakeThing(thingDefCountClass.thingDef);
            products.stackCount = thingDefCountClass.count;
            yield return products;
        }
    }
}