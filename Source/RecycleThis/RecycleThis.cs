using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
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
}