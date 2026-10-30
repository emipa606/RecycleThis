using RimWorld;
using Verse;

namespace RecycleThis;

[DefOf]
public static class RecipeDefOf
{
    public static RecipeDef SmeltWeapon;

    public static RecipeDef SmeltApparel;

    public static RecipeDef BurnApparel;

    static RecipeDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.RecipeDefOf));
    }
}