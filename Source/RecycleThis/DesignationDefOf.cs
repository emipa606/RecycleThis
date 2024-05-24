using RimWorld;
using Verse;

namespace RecycleThis;

[DefOf]
public static class DesignationDefOf
{
    public static DesignationDef RecycleThisDestroy;

    public static DesignationDef RecycleThisRecycle;

    static DesignationDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DesignationDefOf));
    }
}