# Unity Basic Scripts

<!-- ai-created -->

This folder contains a collection of utility scripts for Unity projects (Total number: 28):

## 🟧🟧 Animation & Visual Effects 🟧🟧
- **AnimatorSimple.cs** - Cycles through sprite animations with support for constant or custom timing, works with both UI Images and SpriteRenderers.
- **Billboard.cs** - Empty script template for billboard functionality.
- **FlashWhite.cs** - Creates a white flash effect on UI elements with customizable duration and loop options.
- **MaterialScroller.cs** - Animates material properties over time with various repetition modes and timing controls.
- **MeshCycler.cs** - Cycles through a list of mesh GameObjects with customizable timing controls.
- **UVOffset.cs** - Scrolls texture UVs continuously or in discrete steps with custom property support.

## 🟧🟧 Camera & Movement 🟧🟧
- **CameraFollow2D.cs** - 2D camera follow script with optional smooth lag effect.
- **TopDownMovement2D.cs** - Handles 2D top-down character movement.
- **FollowTarget.cs** - Makes an object move towards a target at a specified speed.

## 🟧🟧 Input Handling 🟧🟧
- **LookAtMouse2D.cs** - Rotates an object to face the mouse cursor in 2D space.
- **PlaceAtMouseDirection.cs** - Places an object in the direction of the mouse cursor with distance limit.
- **PlaceOnMouse.cs** - Makes an object follow the mouse cursor position in world space.
- **SpawnWithKey.cs** - Spawns a prefab at a constant or random offset when a key is pressed.
- **ToggleWithKey.cs** - Toggles a GameObject's active state when a specified key is pressed.

## 🟧🟧 Component Management 🟧🟧
- **DestroyAfterDelay.cs** - Destroys a GameObject after a specified delay, with automatic or manual trigger modes.
- **EnableComponentOnDistance.cs** - Enables a component when another transform comes within specified distance, with optional loop behavior.
- **ToggleAfterDelay.cs** - Toggles a GameObject on and off after specified delays.
- **ToggleChildrenOff.cs** - Disables child GameObjects either all at once or sequentially.
- **ToggleComponentOnOff.cs** - Enables then disables a component after specified delays.
- **ToggleMultipleTimes.cs** - Toggles a component's enabled state multiple times with configurable count and timing.

## 🟧🟧 Rotation 🟧🟧
- **RotateConstantly.cs** - Continuously rotates an object at a specified speed around custom axes.
- **RotatePauseRotate.cs** - Rotates an object for a duration, pauses, then continues in a cycle.
- **RotatorStepwise.cs** - Rotates an object by fixed or random angles at regular intervals.

## 🟧🟧 UI 🟧🟧
- **VerticalMenu.cs** - Implements a keyboard-navigable vertical menu system with hover states and selection events.

## 🟧🟧 Shaders 🟧🟧
- **AlphaMaskedGleam.shader** - UI shader that applies a moving gleam effect with alpha masking and padding controls.
- **HSLMod.shader** - Modifies colors using HSL (Hue, Saturation, Luminance) with customizable tint and shifts.
- **LuminanceAdd.shader** - Adds to pixel brightness based on luminance calculation.
- **LuminanceScale.shader** - Scales pixel brightness based on luminance calculation.
