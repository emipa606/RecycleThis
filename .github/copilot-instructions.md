# Copilot Instructions for RimWorld Modding Project: RecycleThis

## Mod Overview and Purpose

"RecycleThis" is a RimWorld mod that introduces enhanced functionalities for recycling and destroying various items within the game. The mod aims to provide players with more efficient and immersive ways to manage their inventory and recycle resources, thus enhancing the overall gameplay experience.

## Key Features and Systems

- **Recycling System**: Players can efficiently recycle items into their base components using the new 'Recycle' designator.
- **Destruction System**: Allows for the removal of unwanted items with the 'Destroy' designator.
- **Integrated with Job System**: New jobs are created for pawns to either recycle or destroy items.
- **Customization Options**: The mod includes settings to adjust the recycling and destruction processes according to player preferences.

## Coding Patterns and Conventions

- **Class Naming**: Classes follow PascalCase convention and are generally grouped by functionality (e.g., `Designator_RecycleThing`, `JobDriver_RecycleThing`).
- **Static Classes for Definitions**: Definitions like designations and jobs are stored in static classes ending with `DefOf` (e.g., `DesignationDefOf`, `JobDefOf`).
- **Mod Extension**: Use of `DefModExtension` with `RecycleThisModExtension` to extend definitions conveniently.

## XML Integration

The mod interacts with RimWorld using XML for defining game content like jobs, designations, and recipes. Ensure all necessary definitions have matching XML entries to prevent any null reference errors at runtime. Review the XML files for proper elements like `<ThingDef>`, `<RecipeDef>`, and `<JobDef>` with appropriate attributes.

## Harmony Patching

**Harmony** library is used for modifying existing game methods, allowing the mod to introduce custom logic without altering the original game files. If new functionality needs to be added, ensure that Harmony patches are well-documented and targeted only at the necessary game methods to maintain performance and compatibility. Create patches in an intuitive manner using pre and post-fix methods.

## Suggestions for Copilot

1. **Auto-completion for Designation Logic**: Implement typical logic structures for designators used in RimWorld when suggesting completions in `Designator_DestroyThing` and `Designator_RecycleThing`.
   
2. **Job System Integration**: Provide suggestions for setting up new jobs adhering to the RimWorld pawn job structure in `JobDriver_DestroyThing` and `JobDriver_RecycleThing`.

3. **Efficient ForEach Loops**: Suggest efficient patterns for processing collections, as seen in methods containing `foreach`, to ensure performance remains optimal.

4. **XML Tag Consistency**: When creating new XML content, provide suggestions to maintain tag consistency and correct structure to avoid validation errors.

5. **Harmony Patch Creation**: Make informed suggestions for creating Harmony patches, focusing on methods typical of managing mod behaviour (e.g., `ReverseDesignatorDatabase_InitDesignators`).

By following these structured guidelines, `Copilot` can effectively aid in developing and extending the RimWorld RecycleThis mod, ensuring clean, maintainable, and efficient code development practices.
