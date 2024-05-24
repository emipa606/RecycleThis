using RimWorld;
using Verse;

namespace RecycleThis;

[DefOf]
public static class JobDefOf
{
    public static JobDef RecycleThisDestroy;

    public static JobDef RecycleThisRecycle;

    static JobDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
    }
}