# PshikRun
This is a demo infinite runner project made in Unity. Please note that as of right now, it's very unfinished and is quite buggy.

<img height="256" alt="image" src="https://github.com/user-attachments/assets/d0f23067-8ef2-47db-b5f3-25dd38e84fea" />


## How it works
The game selects random chunks from the *inactive chunk pool* and moves them ahead of the player. When the player leaves the chunk, it gets transfered back to the pool. This optimization technique is called pooling and it saves a lot on CPU usage, as no new chunks get spawned at runtime.

<img height="300" alt="image" src="https://github.com/user-attachments/assets/c9c2dc29-d8ce-4a6a-8bc8-14f14495fb54" />

In the `GameScene`, there exists two main sections:
- **Game**
  Objects in this section do all the physics calculations and collision detection, but don't actually get rendered. This section does not get distorted by the bezier curve path and is perfectly straight along the Z axis.

- **World**
  Objects in this section are being rendered and distorted by the bezier curve path, but don't actually have colliders or do any physics calculations.

Having these two systems separated ensures that the collision and physics are *independent* of the bezier path's twists and turns.

## Setup guide
To run the project:
- Clone the repo
- Open it in Unity 6.0 or above
- Wait for the libraries and objects to generate
- Navigate to `Assets/Scenes` open the `GameScene`
- Press play

Control the player with arrow keys or by moving the mouse (be careful, as the mouse/touch controls are currently very wonky).

## Project structure
Here's how the project's Assets folder is structured:
- `/Animation`
  - `/Controllers` - animation controllers
- `/AssetPacks` - assets from the Unity Asset Store
- `/Audio`
  - `/Music`
  - `/SFX`
- `/Lib` - third-party libraries used
- `/Materials`
- `/Models` - FBX models, made in Blender
- `/Prefabs`
  - `/Chunks` - chunk pools, containing every chunk used by the game
  - `/Locations` - visual locations that are placed in the World section
  - `/Objects` - objects on the track, such as obstacles, collectables, etc.
- `/Resources`
  - `/InputSystem` - Unity Input System configuration
- `/Scenes`
- `/Scripts`
  - `/Behaviors`
  - `/Controllers`
  - `/Managers`
  - `/StateMachine`
- `/Settings` - URP settings
- `/Shaders`
- `/Textures`
