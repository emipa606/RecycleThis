using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RecycleThis;

public class Designator_RecycleThing : Designator
{
    public Designator_RecycleThing()
    {
        defaultLabel = "DesignatorRecycleThing".Translate();
        defaultDesc = "DesignatorRecycleThingDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("RecycleThisGizmo");
        useMouseIcon = true;
        soundSucceeded = RimWorld.SoundDefOf.Designate_Haul;
        hotKey = KeyBindingDefOf.Misc3;
    }

    public override int DraggableDimensions => 2;

    protected override DesignationDef Designation => DesignationDefOf.RecycleThisRecycle;

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        if (!c.InBounds(Map) || c.Fogged(Map))
        {
            return false;
        }

        foreach (var thing in c.GetThingList(Map))
        {
            if (thing != null && CanDesignateThing(thing))
            {
                return true;
            }
        }

        return false;
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

    private bool SmeltingIsUseful(Thing t)
    {
        if (t.def.smeltProducts is { Count: > 0 })
        {
            return true;
        }

        var list = t.def.CostListAdjusted(t.Stuff);
        foreach (var defCountClass in list)
        {
            if (!defCountClass.thingDef.intricate && Math.Round(defCountClass.count * 0.25f) > 0)
            {
                return true;
            }
        }

        return false;
    }

    public override AcceptanceReport CanDesignateThing(Thing t)
    {
        if (Map.designationManager.DesignationOn(t, Designation) != null)
        {
            return false;
        }

        if (!SmeltingIsUseful(t))
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