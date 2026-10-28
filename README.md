# RhythmSword-Prototype

A VR rhythm game prototype built with Unity 6 where the player slices incoming note cubes with dual-wielded swords synchronized to music. Notes approach the player along a track, each marked with a direction (left, right, up, down) and a color (red or blue). The player must swing the correct sword in the correct direction to score points. The project includes a custom in-editor chart authoring tool for creating and editing beatmaps.

## About the Project

RhythmSword-Prototype addresses the challenge of combining rhythm-based gameplay mechanics with immersive VR interaction. Inspired by games such as Beat Saber, this prototype explores how note spawning, directional slash detection, real-time mesh slicing, and music synchronization can be integrated within the Unity XR Interaction Toolkit ecosystem.

The core gameplay loop works as follows: a JSON-based chart file defines note timings, directions, colors, and positions for a given song. At runtime, the `ChartPlayer` system reads the chart, synchronizes with the audio playback, and spawns note cubes ahead of time so they travel toward the player. Each note cube carries directional colliders (LEFT, RIGHT, UP, DOWN) and is color-coded red or blue. The player holds two VR swords — a red sword in the left hand and a blue sword in the right hand — and must slash the note from the correct direction with the matching color. Successful hits trigger score accumulation, hit sound effects, particle effects, and haptic feedback on the controller. Incorrect color or direction results in a miss sound.

## Features

- **Directional Note Slicing** — Each note cube has four directional sub-colliders. The player must swing the sword from the direction indicated on the note to score points. Wrong direction or wrong color triggers a penalty feedback.
- **Dual-Color Sword System** — Two swords are mapped to VR controllers: the left controller wields a red sword, the right controller wields a blue sword. Notes are color-coded to require the correct hand.
- **JSON-Based Chart System** — Songs are mapped using `.chart.json` files that store song name, BPM, audio clip path, and an array of note entries with timing, direction, color, position, and type data.
- **Custom Chart Editor (Unity Editor Window)** — A full-featured in-editor beatmap authoring tool accessible via `Tools > VR Rhythm > Chart Editor`. It includes a visual timeline with four direction lanes, audio playback with variable speed (0.25x to 2x), BPM-aware snap modes (whole, half, quarter, eighth, sixteenth), note placement by clicking on lanes, drag-to-reposition, copy/paste, keyboard shortcuts (Space for play/pause, Delete for note removal, Ctrl+S to save), and zoom/scroll controls.
- **Real-Time Mesh Slicing** — Uses the EzySlice library to dynamically slice note cubes into two physics-driven halves along a computed cutting plane when the sword interacts with the note. The slicing accounts for the hand's entry direction and uses cross product calculations to determine the cut plane normal.
- **Haptic Feedback** — On each successful interaction with a note, the VR controller receives a haptic impulse (0.4 amplitude, 0.1s duration) through the XR Interaction Toolkit.
- **Swing Detection System** — The `RaySwingDetector` tracks controller velocity per frame, determines the dominant swing axis using dot product comparisons against the camera-relative coordinate system, and applies a minimum speed threshold (1.8 m/s) and cooldown (0.35s) to filter noise.
- **Dynamic Environment Effects** — The `AtmosphereManager` script drives time-based environmental changes synchronized to the song, including directional light (sun) rotation speed changes, particle effects (blood rain, white dots) triggered at specific timestamps, creating atmosphere shifts during gameplay.
- **Destructible Wall System** — A brick wall in the background is partitioned into four sections using X-axis normalization with randomized boundary jitter. Sections break apart sequentially at predefined timestamps using physics forces and optional dust particle effects, creating a cinematic backdrop synchronized to the music.
- **Grid-Based Note Positioning** — Notes spawn on a predefined grid (4 columns, 2 rows) with configurable X and Y positions. The chart system supports both custom and default positioning.
- **Multiple Song Support** — The project includes six pre-charted songs: "Ride", "City Walls", "The Contract", "Solace (TXMY)", "Aklimda Bir Kordugum", and "Sadece Sevilmek Istedim", each with its own chart file and dedicated scene or shared scene configuration.
- **Score System** — A singleton `PlayerManager` tracks the cumulative score, displays it via TextMeshPro UI, and provides audio-visual feedback with randomized hit sound pitch (0.9–1.2) and instantiated particle effects at the hit point.

## Technologies Used

- **Unity 6 (6000.4.5f1)** — Game engine, using the URP Blank template (v17.0.14)
- **Universal Render Pipeline (URP) 17.4.0** — Rendering pipeline for optimized visuals and shader graph support
- **XR Interaction Toolkit 3.4.1** — VR interaction framework handling controller input, interactables, hover events, and haptic feedback
- **OpenXR 1.16.1** — Cross-platform VR runtime standard for headset and controller communication
- **XR Management 4.5.4** — XR plugin lifecycle and loader management
- **Unity Input System 1.19.0** — New input system for VR controller mapping
- **TextMeshPro (via Unity UGUI 2.0.0)** — High-quality text rendering for score display, debug overlays, and direction labels on notes
- **EzySlice (integrated source)** — Open-source mesh slicing library for real-time procedural mesh cutting with proper UV, normal, and tangent interpolation
- **Unity Timeline 1.8.12** — Timeline package (available in the project for potential sequencing use)
- **Unity Splines 2.8.4** — Spline package (available in the project)
- **AI Navigation 2.0.12** — Navigation mesh package (available in the project)
- **Shader Graph (via URP)** — Used for custom neon glow shaders on note cubes (Neon_Red.shadergraph, Neon_Blue.shadergraph)
- **JsonUtility (Unity built-in)** — JSON serialization and deserialization for chart data

## Project Structure

```text
Assets/
├── Charts/                          # Pre-authored beatmap JSON files
│   ├── Ride.chart.json
│   ├── City Walls.chart.json
│   ├── The_Contract.chart.json
│   ├── TXMY.chart.json
│   ├── Aklimda_Bir_Kordugum.chart.json
│   └── Sadece_Sevilmek_istedim.chart.json
├── Editor/
│   └── ChartEditor/                 # Custom chart authoring tool (8 scripts)
│       ├── ChartEditorWindow.cs     # Main EditorWindow entry point
│       ├── ChartEditorState.cs      # Shared editor state (selection, playback, zoom)
│       ├── ChartEditorTimeline.cs   # Visual timeline with lane rendering and note interaction
│       ├── ChartEditorPlayback.cs   # Audio preview playback via reflection-based AudioUtil
│       ├── ChartEditorToolbar.cs    # Song metadata, file ops, direction/color/snap controls
│       ├── ChartEditorInspector.cs  # Selected note property inspector
│       ├── ChartEditorInput.cs      # Keyboard shortcut handling
│       └── SnapMode.cs             # BPM-aware snap interval calculations
├── Prefabs/
│   ├── Notes/
│   │   ├── Cube.prefab             # Main note cube with directional sub-colliders
│   │   ├── Cube_Broken.prefab      # Pre-sliced broken cube variant
│   │   ├── Direction/              # Direction arrow textures (PNG) and materials
│   │   └── fbx/                    # Custom 3D note cube models
│   ├── Player/
│   │   ├── XR Origin (XR Rig).prefab  # VR player rig
│   │   ├── RedSword.prefab         # Left-hand red sword
│   │   └── BlueSword.prefab        # Right-hand blue sword
│   ├── Background/
│   │   ├── Walls.prefab            # Destructible brick wall (672 pieces)
│   │   ├── Ways.prefab             # Track/path geometry
│   │   └── AtmosphereManager.prefab # Environment effects controller
│   ├── Gameplay.prefab             # Core gameplay systems bundle
│   ├── Canvas.prefab               # UI canvas (score display)
│   └── SoundSystem.prefab          # Audio source configuration
├── Scripts/
│   ├── Chart/                       # Chart data model and runtime playback
│   │   ├── ChartData.cs            # Song metadata + note list container
│   │   ├── ChartNoteData.cs        # Individual note definition (time, dir, color, pos)
│   │   ├── ChartPlayer.cs          # Runtime chart-to-note spawning synchronization
│   │   └── ChartSerializer.cs      # JSON save/load utility
│   ├── Notes/                       # Note behavior and interaction
│   │   ├── Note.cs                 # Note initialization, color/direction setup, scoring
│   │   ├── NoteData.cs             # Runtime note data + DIRECTION/COLOR enums
│   │   ├── NoteMovement.cs         # Note forward movement with speed zone trigger
│   │   └── OptimizerCubeSlicer.cs  # EzySlice-based directional mesh slicing
│   ├── Player/                      # Player systems
│   │   ├── PlayerManager.cs        # Singleton score manager, hit/miss audio, effects
│   │   ├── SwordRayInteractor.cs   # XR hover-based sword-note interaction logic
│   │   ├── RaySwingDetector.cs     # Controller velocity-based swing direction detection
│   │   ├── KılıcKesici.cs          # Trigger-based sword slicing (alternative approach)
│   │   └── MissPoint.cs            # Missed note detection trigger zone
│   ├── Spawning/                    # Note lifecycle
│   │   ├── CubeSpawner.cs          # Legacy time-based note spawner
│   │   └── DestroyPoint.cs         # End-of-track note cleanup trigger
│   ├── Background/                  # Environment systems
│   │   ├── AtmosphereManager.cs    # Time-synced sun rotation + particle triggers
│   │   ├── TimedWallBreaker.cs     # Sequential wall section destruction via coroutines
│   │   ├── WallPartition.cs        # X-axis based wall piece grouping
│   │   ├── WallSystem.cs           # Random periodic brick ejection
│   │   ├── BackgroundSpinner.cs    # Continuous background object rotation
│   │   ├── SunRotation.cs          # Song-duration-proportional sun rotation
│   │   └── QuadOpacitySettings.cs  # Time-based opacity fade for background quads
│   ├── EzySlice/                    # Mesh slicing library (integrated source)
│   │   ├── Slicer.cs               # Core slicing algorithm
│   │   ├── SlicedHull.cs           # Upper/lower hull mesh generation
│   │   ├── SlicerExtensions.cs     # GameObject extension methods for slicing
│   │   └── Framework/              # Geometric primitives (Plane, Triangle, Line, etc.)
│   └── HapticFeedbackController.cs # Static haptic impulse utility
├── Scenes/
│   ├── SampleScene.unity           # Default/test scene
│   ├── TXMY.unity                  # TXMY song scene
│   ├── The Contract.unity          # The Contract song scene
│   └── CityWalls.unity             # City Walls song scene
├── Sounds/                          # Audio assets (MP3/WAV) and sound effect prefabs
└── Settings/                        # URP and rendering pipeline settings
```

## Technical Details

### Chart System Architecture

The chart system uses a two-layer data model. `ChartData` is the top-level container holding song metadata (name, BPM, audio path) and a list of `ChartNoteData` entries. Each `ChartNoteData` stores exact hit time in seconds, direction as a string enum ("LEFT", "RIGHT", "UP", "DOWN"), color ("RED", "BLUE"), note type, and optional grid position coordinates. This string-based serialization was chosen for human readability in JSON files and compatibility with `JsonUtility`.

At runtime, `ChartPlayer` drives note spawning by comparing the audio source's current playback time against each note's target time minus a configurable travel time offset. This offset-based approach ensures notes are spawned early enough to physically travel from the spawn point to the player at the correct moment. Notes are sorted by time at load, and a sequential index counter (`nextNoteIndex`) avoids repeated list scans.

### VR Interaction Model

The project uses `XRSimpleInteractable` from the XR Interaction Toolkit rather than grab or socket interactors. Each note cube prefab contains four child colliders named "LEFT", "RIGHT", "UP", and "DOWN". The `SwordRayInteractor` component on these colliders listens for `OnHoverEntered` events. When a sword hovers over a directional collider, the script checks:
1. Whether the interactor name contains "Left" or "Right" (to determine which hand is swinging)
2. Whether the hand matches the note's color (left hand = RED, right hand = BLUE)
3. Whether the collider's name matches the note's required direction

This three-check system enables directional and color validation without raycasting, relying instead on physical proximity through the XR hover system.

### Mesh Slicing Pipeline

The `OptimizerCubeSlicer` uses a geometry-aware slicing approach:
1. The cutting point is shifted from the hand's contact position toward the cube's center using a configurable `centerShiftRatio` (default 0.5) to prevent edge-case micro-slices.
2. The cut plane normal is computed via cross product between the hand-to-cube-center direction and the hand's forward vector, ensuring the slice plane is perpendicular to the swing trajectory.
3. EzySlice's `Slice()` method divides the mesh along this plane, producing upper and lower hull GameObjects with correct vertex positions, UVs, normals, and tangents.
4. Each hull receives a MeshCollider (convex) and Rigidbody, with opposite impulse forces applied to separate the halves visually. Pieces are offset by 0.02 units to prevent immediate re-collision.
5. Sliced fragments are automatically destroyed after 2 seconds to prevent performance degradation.

### Chart Editor Tool

The custom chart editor (`Tools > VR Rhythm > Chart Editor`) is implemented as a modular Unity EditorWindow split across eight scripts following a separation-of-concerns pattern:
- **State**: Centralized in `ChartEditorState`, holding chart data, selection, playback state, zoom, scroll, snap mode, and dirty flag.
- **Timeline**: Renders a horizontal scrollable timeline with four lanes (one per direction), BPM grid lines, note blocks color-coded by note color, and a playback cursor. Supports left-click to add notes, right-click context menu, and drag to reposition.
- **Playback**: Uses reflection to access Unity's internal `AudioUtil.PlayPreviewClip` API, enabling audio preview directly in the editor without entering Play Mode.
- **Snap System**: `SnapUtility` calculates snap intervals from BPM and the selected subdivision (whole through sixteenth notes), then quantizes note times to the nearest grid point.

### Dynamic Environment System

The `AtmosphereManager` implements a hardcoded timeline of environmental state changes for a specific song, controlling directional light rotation speed and particle effect activation at specific timestamps (e.g., particle effects at 126s and 165s, rapid sun rotation at 112s). This creates a song-specific visual narrative that shifts the environment's mood in sync with musical sections.

The `TimedWallBreaker` partitions a 672-piece brick wall into four sections using X-axis position normalization with a small random jitter (+-0.04) at boundaries to create irregular break lines. Sections are released sequentially via coroutine at predefined timestamps, with physics forces applied to each brick for a cascading destruction effect.

## Setup and Running

### Prerequisites

- **Unity 6** (version 6000.4.5f1 or compatible)
- A VR headset and controllers compatible with OpenXR (e.g., Meta Quest, Valve Index, HTC Vive)
- [If testing without a headset, the project includes an XR Device Simulator prefab]

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/[REPOSITORY_URL_HERE]/RhythmSword-Prototype.git
   ```

2. Open the project in Unity Hub by selecting the cloned folder. Unity will import assets and resolve packages automatically.

3. Ensure your VR headset is connected and recognized by the system's OpenXR runtime.

4. Open one of the song scenes from `Assets/Scenes/` (e.g., `The Contract.unity`, `CityWalls.unity`, or `TXMY.unity`).

5. Enter Play Mode to start the game.

### Using the Chart Editor

1. Open the chart editor: `Tools > VR Rhythm > Chart Editor`
2. Set the song name, assign an audio clip, and configure the BPM.
3. Click on a lane in the timeline to place notes at the clicked time position.
4. Use the snap mode dropdown to align notes to beat subdivisions.
5. Drag notes to reposition them, right-click for context menu options.
6. Use the playback controls to preview audio while editing.
7. Save the chart as a `.chart.json` file via `Save As`.

## Usage

1. **Start a Song Scene**: Open one of the configured scenes and enter Play Mode. The song begins automatically and notes start approaching.
2. **Slash Notes**: Using VR controllers, swing the left sword (red) at red notes and the right sword (blue) at blue notes, matching the arrow direction on each note.
3. **Score Points**: Correct direction + correct color = 10 points per note. The score is displayed on the in-game UI canvas.
4. **Watch the Environment**: The background environment changes dynamically with the music — walls break, lighting shifts, and particle effects activate at key musical moments.

## Screenshots and Demo

[Project Demo Video](BURAYA_VIDEO_LINKI)

<!-- Add screenshots here -->
<!-- ![Gameplay Screenshot](screenshots/gameplay.png) -->
<!-- ![Chart Editor Screenshot](screenshots/chart_editor.png) -->

## Development Process

This project was developed as a prototype to explore VR rhythm game mechanics within the Unity ecosystem. Key development decisions include:

- **XR Interaction Toolkit over custom VR input**: The project uses Unity's official XR Interaction Toolkit (v3.4.1) and OpenXR rather than platform-specific SDKs, enabling cross-headset compatibility.
- **Hover-based interaction over physics collision**: Note hit detection uses XR hover events on directional sub-colliders rather than continuous collision detection, providing more reliable interaction at VR interaction speeds.
- **Integrated EzySlice source over package dependency**: The EzySlice mesh slicing library is included directly as source code rather than as a package reference, allowing direct modification and ensuring version stability.
- **JSON charts over binary or ScriptableObject formats**: Chart data uses JSON serialization for human readability, easy external editing, and portability. The custom editor tool generates and consumes these JSON files directly.
- **Separate scenes per song**: Each song has a dedicated scene allowing unique environmental setups (terrain, lighting, wall configurations, atmosphere timelines), though this trades off against a shared-scene architecture with dynamic loading.
- **Editor-time audio preview via reflection**: The chart editor accesses Unity's internal `AudioUtil` API through reflection to enable audio playback in the editor without requiring Play Mode, improving the chart authoring workflow.

AI-assisted tools were utilized during the development process for code suggestions, bug analysis, technical research, and accelerating development. Project architecture, feature design, integrations, testing, and final technical decisions were evaluated and implemented by the developer.

## Future Improvements

- Implementing a song selection menu to switch between songs without manually changing scenes
- Adding combo and multiplier mechanics to the scoring system for consecutive correct hits
- Introducing obstacle notes that the player must dodge rather than slash
- Supporting runtime chart loading from external files for community-created content
- Adding end-of-song results screen with accuracy percentage, max combo, and grade
- Implementing note approach speed variation based on song difficulty levels
- Creating a unified scene architecture with dynamic environment loading per song
- Adding visual feedback on the sword blade (trail renderer, glow on hit)
- Supporting two-handed simultaneous notes (both swords required at the same beat)
- Integrating a leaderboard or local high score persistence system
- Generalizing the `AtmosphereManager` to read environment events from data files rather than hardcoded timestamps

## Developer

- GitHub: [Add link here]
- LinkedIn: [Add link here]
- Portfolio: [Add link here]