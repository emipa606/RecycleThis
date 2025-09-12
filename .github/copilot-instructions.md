# Copilot Instructions for Recycle This (Continued) Mod

## Mod Overview and Purpose
**Recycle This (Continued)** is a RimWorld mod aimed at simplifying the process of disposing of unwanted weapons and apparel after raids or other events. It eliminates the cumbersome task of setting bills on workbenches by allowing users to recycle or destroy items directly through order icons. The mod is designed to be as seamless as possible, integrating smoothly with vanilla and modded RimWorld experiences while keeping gameplay balanced.

## Key Features and Systems
- **Order Gizmos:** Adds two new order icons:
  - **Destroy Item:** Sends selected items to the nearest building capable of destruction (e.g., campfires, cremators).
  - **Recycle Item:** Directs items to the closest workbench where they can be recycled, yielding part of their resources based on vanilla calculations.
- **Customization Options:** Players can toggle options to hide additional gizmos or prevent the return of complex components during recycling.
- **Job Integration:** Introduces a new crafting-related job visible in Fluffy's Work Tab, merging additional functionality smoothly into existing RimWorld mechanics.

## Coding Patterns and Conventions
- **Static Classes for Definitions**: Usage of `public static class NameOfDef` for organizing definition constants like `DesignationDefOf`, `EffecterDefOf`, `JobDefOf`, and more.
- **Designator Classes**: Classes such as `Designator_DestroyThing` and `Designator_RecycleThing` manage the assignment and execution of destruction and recycling actions respectively.
- **JobDriver Classes**: `JobDriver_DestroyThing` and `JobDriver_RecycleThing` encapsulate the logic for handling the respective jobs, ensuring they execute under appropriate conditions.

## XML Integration
- **DefModExtension**: Utilized in `RecycleThisModExtension` for mod configuration and settings integration.
- **Configurable Settings**: `RecycleThisSettings` class allows for easy toggling of gizmo visibility and component return functionality.

## Harmony Patching
- The mod likely leverages Harmony for runtime patches to integrate seamlessly with existing game mechanics, enhancing compatibility with both vanilla and other modded assets. When modifying core behaviors, ensure Harmony is used to create non-destructive patches.

## Suggestions for Copilot
- **Contextual Code Generation**: Use Copilot to generate code snippets for new features like adding more customizable options based on the `RecycleThisSettings` structure.
- **Integration Scripts**: Assist with Harmony patch creation, especially if new functionality requires altering or extending vanilla methods.
- **Efficient Loops and Checks**: Optimize iteration through item lists and job queues, making use of efficient C# practices such as LINQ where applicable.
- **Error Handling**: Generate code that gracefully handles edge cases, such as missing buildings for destruction or recycling, improving user experience and reducing errors.
- **Documentation Generation**: Help with inline comments and XML documentation within the code to facilitate maintenance and further mod development.

By adhering to these details and utilizing GitHub Copilot effectively, developers can maintain and enhance the *Recycle This (Continued)* mod, ensuring its longevity and compatibility in the ever-evolving modding landscape of RimWorld.
