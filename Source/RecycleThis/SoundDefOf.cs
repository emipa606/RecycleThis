using RimWorld;
using Verse;

namespace RecycleThis;

[DefOf]
public static class SoundDefOf
{
    public static SoundDef Recipe_Cremate;

    public static SoundDef Recipe_Smelt;

    public static SoundDef Recipe_Tailor;

    static SoundDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.SoundDefOf));
    }
}