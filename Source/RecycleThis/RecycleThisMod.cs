using Mlie;
using UnityEngine;
using Verse;

namespace RecycleThis;

[StaticConstructorOnStartup]
internal class RecycleThisMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static RecycleThisMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public RecycleThisMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<RecycleThisSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal RecycleThisSettings Settings { get; }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Recycle This";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.CheckboxLabeled("RecycleThisShowGizmo".Translate(), ref Settings.ShowGizmo);
        listingStandard.CheckboxLabeled("RecycleThisGiveComponents".Translate(), ref Settings.GiveComponents);
        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("RecycleThisCurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        if (Current.Game == null)
        {
            return;
        }

        try
        {
            Find.MapUI?.reverseDesignatorDatabase?.Reinit();
        }
        catch
        {
            //
            // 
        }
    }
}