# GitHub Copilot Instructions for Recycle This (Continued)

## Mod Overview and Purpose

Recycle This (Continued) is a RimWorld mod that allows players to recycle and destroy weapons and apparel through order icons, eliminating the need to create annoying bills. It uses vanilla values to calculate refunds (around 25% of the recipe) and job speed, making it almost identical to vanilla bills (Smelt or Burn weapons/apparel).

The mod adds two orders to the menu and affected items:
1. Destroy item - selected weapons or apparel will be destroyed on the closest campfire or smelter.
2. Recycle item - selected weapons or apparel will be either smelted or scrapped at the closest workbench, yielding part of the recipe resources back.

## Key Features and Systems

### Core Features:
- **Recycle and Destroy Orders**: Players can recycle and destroy weapons and apparel using order icons.
- **Settings Options**: Players have the option to not show extra gizmos on items and not return components when recycling.
- **Compatibility with Modded Workbenches**: The mod works with any modded workbenches that have the necessary recipes to burn apparel and smelt weapons.

### Systems:
- **Job System**: New jobs associated with the added features exist in the Crafting worktype.
- **Designation System**: The mod adds new designations for recycling and destroying items.
- **Harmony Patching**: The mod uses Harmony to patch vanilla behavior and integrate new features.

## Coding Patterns and Conventions

### C# Coding Patterns:
- **Modding Framework**: The mod follows the RimWorld modding framework, using classes like `Mod`, `Def`, and `Harmony` to extend vanilla functionality.
- **Settings Management**: Settings are managed using the `ModSettings` class, with options exposed in the settings window.
- **Job and Designation Definitions**: Jobs and designations are defined using `Def` classes, which are loaded from XML files.

### XML Integration:
- **Defs Files**: The mod defines new job types, designation types, and workgivers in XML files located in the `Defs` directory.
- **Patches Files**: Harmony patches are defined in XML files located in the `Patches` directory, modifying vanilla behavior.

## Harmony Patching

Harmony is used to patch vanilla behavior and integrate new features into the game. Patches are defined in XML files and are used to:
- Modify job definitions to include new recycling and destroying jobs.
- Add new designation categories and types.
- Adjust work type definitions to include the new recycling and destroying jobs.

## Suggestions for Copilot

### General Suggestions:
- **Modding Framework**: Copilot should be familiar with the RimWorld modding framework, including the use of `Mod`, `Def`, and `Harmony` classes.
- **Settings Management**: Copilot should be able to generate code for managing settings using the `ModSettings` class and exposing options in the settings window.
- **Job and Designation Definitions**: Copilot should be able to generate code for defining new job types, designation types, and workgivers in XML files.

### XML Integration:
- **Defs Files**: Copilot should be able to generate XML files for defining new job types, designation types, and workgivers.
- **Patches Files**: Copilot should be able to generate XML files for defining Harmony patches to modify vanilla behavior.

### Harmony Patching:
- **Patch Definition**: Copilot should be able to generate Harmony patch definitions in XML files to modify vanilla behavior.
- **Patch Implementation**: Copilot should be able to generate the C# code for implementing Harmony patches.

### Example Code Snippets

#### C# Example: Defining a New Job
csharp
public class JobDefOf
{
    public static JobDef RecycleItem;
    public static JobDef DestroyItem;
}

public static class RecycleThis
{
    static RecycleThis()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
    }
}


#### XML Example: Defining a New Job in XML
xml
<Defs>
    <JobDef>
        <defName>RecycleItem</defName>
        <jobTitle>Recycle Item</jobTitle>
        <workType>Crafting</workType>
        <targetCount>1</targetCount>
    </JobDef>
    <JobDef>
        <defName>DestroyItem</defName>
        <jobTitle>Destroy Item</jobTitle>
        <workType>Crafting</workType>
        <targetCount>1</targetCount>
    </JobDef>
</Defs>


#### Harmony Patch Example: Modifying Vanilla Behavior
csharp
public static class RecycleThisHarmony
{
    [HarmonyPatch(typeof(JobDriver_Smelt), nameof(JobDriver_Smelt.StartJobAction))]
    public static class Patch_JobDriver_Smelt_StartJobAction
    {
        public static void Prefix(ref Job __job)
        {
            // Custom logic for recycling items
        }
    }
}


#### XML Example: Defining a Harmony Patch in XML
xml
<Patches>
    <Patch>
        <patchClass>RecycleThisHarmony</patchClass>
        <targetClass>JobDriver_Smelt</targetClass>
        <targetMethod>StartJobAction</targetMethod>
        <prefix>Prefix</prefix>
    </Patch>
</Patches>


By following these guidelines and using the provided code snippets, Copilot can effectively assist with developing the Recycle This (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

