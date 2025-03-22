using Verse;

namespace RecycleThis;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class RecycleThisSettings : ModSettings
{
    public bool GiveComponents = true;
    public bool ShowGizmo = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowGizmo, "ShowGizmo", true);
        Scribe_Values.Look(ref GiveComponents, "GiveComponents", true);
    }
}