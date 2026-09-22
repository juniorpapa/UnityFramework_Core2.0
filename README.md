# UnityFramework_Core2.0 v1.1

> This project is for learning and communication purposes only. It is not recommended for use.

## Classes and Their Purposes

| Class | Purpose |
| --- | --- |
| `EventManager` | Stores and manages in-game events. |
| `ResourceManager` | Stores and manages in-game resources. |
| `ScriptManager` | Manages C#/sandbox scripts. |
| `Environment` | Stores scripts in the game that do not inherit from Mono. |
| `GameLog` | Game log. |
| `ConsoleWindow` | Console window. |
| `Initialize` | Initializes the game. |
| `OldKeyInput` | Key binding system based on the legacy input system. |
| `Storage` | Stores data locally. |
| `GameComponent` | All in-game object properties, etc. are composed of Unity-like components, and all components are under `GameComponent`. |
| `HotFixActuator` | Performs game hot updates. |
| `NewKeyInput` | Key binding system based on the new input system. (Not implemented.) |

## Naming Conventions

| Type | Example |
| --- | --- |
| Member variable | `water` |
| Private variable | `_water_` |
| Static variable | `s_water` |
| Read-only variable | `r_water` |
| Local variable | `_water` |
| Method | `SayHello` |
| Interface | `ISayHello` |
| Class | `Console` |
