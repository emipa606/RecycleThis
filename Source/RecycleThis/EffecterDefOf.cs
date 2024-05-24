using RimWorld;
using Verse;

namespace RecycleThis;

[DefOf]
public static class EffecterDefOf
{
    public static EffecterDef Cremate;

    public static EffecterDef Smelt;

    static EffecterDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.EffecterDefOf));
    }
}