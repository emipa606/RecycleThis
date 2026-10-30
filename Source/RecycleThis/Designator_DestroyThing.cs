using RimWorld;
using UnityEngine;
using Verse;

namespace RecycleThis;

public class Designator_DestroyThing : Designator
{
    public Designator_DestroyThing()
    {
        defaultLabel = "DesignatorDestroyThing".Translate();
        defaultDesc = "DesignatorDestroyThingDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("DestroyThisGizmo");
        useMouseIcon = true;
        soundSucceeded = RimWorld.SoundDefOf.Designate_Haul;
        hotKey = KeyBindingDefOf.Misc4;
    }

    public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Orders;

    protected override DesignationDef Designation => DesignationDefOf.RecycleThisDestroy;

    public override AcceptanceReport CanDesignateCell(IntVec3 loc)
    {
        if (!loc.InBounds(Map) || loc.Fogged(Map))
        {
            return false;
        }

        return loc.GetThingList(Map).Any(thing => thing != null && CanDesignateThing(thing));
    }

    public override void DesignateSingleCell(IntVec3 c)
    {
        var list = Map.thingGrid.ThingsListAt(c);
        foreach (var thing in list)
        {
            if (thing.def.stackLimit < 2 && (thing.def.IsApparel || thing.def.IsWeapon))
            {
                DesignateThing(thing);
            }
        }
    }

    public override AcceptanceReport CanDesignateThing(Thing t)
    {
        if (Map.designationManager.DesignationOn(t, Designation) != null)
        {
            return false;
        }

        return t.def.stackLimit < 2 && (t.def.IsApparel || t.def.IsWeapon);
    }

    public override void DesignateThing(Thing t)
    {
        Map.designationManager.RemoveAllDesignationsOn(t);
        Map.designationManager.AddDesignation(new Designation(t, Designation));
    }
}