using HarmonyLib;
using Verse;

namespace RecycleThis;

[HarmonyPatch(typeof(ReverseDesignatorDatabase), "InitDesignators")]
public static class ReverseDesignatorDatabase_InitDesignators
{
    public static void Postfix(ref ReverseDesignatorDatabase __instance)
    {
        if (!RecycleThisMod.Instance.Settings.ShowGizmo)
        {
            return;
        }

        __instance.AllDesignators.Add(new Designator_DestroyThing());
        __instance.AllDesignators.Add(new Designator_RecycleThing());
    }
}