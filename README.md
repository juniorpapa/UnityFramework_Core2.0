# UnityFramework_Core2.0 v1.0

> This project is intended for learning and exchange only, and is not recommended for use.

## Classes

| Class | Purpose |
| --- | --- |
| `EventManager` | Stores and manages in-game events. |
| `ResourceManager` | Stores and manages in-game resources. |
| `ScriptManager` | Manages C#/sandbox scripts. |
| `Environment` | Stores scripts in the game that do not inherit from Mono. |
| `GameLog` | Game log. |
| `ConsoleWindow` | Console window. |
| `Initialize` | Initializes the game. |
| `OldKeyInput` | Keybinding system based on the old input system. |
| `Storage` | Stores data locally. |
| `GameComponent` | All in-game object properties, etc. are composed of Unity-like components, and all components are under `GameComponent`. |
| `HotFixManager` | Manages game hot updates. (Not implemented) |
| `Demo` | Stores game replays, tick-based. (Not implemented) |
| `NewKeyInput` | Keybinding system based on the new input system. (Not implemented) |

## Naming Conventions

| Item | Example |
| --- | --- |
| Member variable | `water` |
| Private variable | `_water_` |
| Static variable | `s_water` |
| Read-only variable | `r_water` |
| Local variable | `_water` |
| Method | `SayHello` |
| Interface | `ISayHello` |
| Class | `Console` |
