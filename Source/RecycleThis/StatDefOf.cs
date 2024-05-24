using RimWorld;

namespace RecycleThis;

[DefOf]
public static class StatDefOf
{
    public static StatDef SmeltingSpeed;

    public static StatDef WorkTableWorkSpeedFactor;

    static StatDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.StatDefOf));
    }
}