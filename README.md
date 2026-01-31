# Unity Clean LiveOps Implementation

A Unity-based Live Operations (LiveOps) system demonstrating event scheduling, asset management, and modular feature architecture.

## Overview

This project implements a LiveOps system for a mobile game skeleton. It fetches active events from a backend, filters them based on player level, displays them with countdown timers, and handles asset loading for themed event popups.

## Architecture

The project follows a modular, service-oriented architecture with dependency injection (VContainer).

### Layered Structure

**1. Infrastructure Layer** - Application bootstrap and DI configuration
- `BootEntryPoint` - Initializes all services, restores state, schedules events, loads lobby scene
- `RootScope` - Registers all singleton services in VContainer

**2. Service Layer** - Core services (singletons, shared across features)

| Service | Responsibility |
|---------|----------------|
| `LiveOpsService` | Orchestrates calendar loading and event scheduling |
| `LiveOpsCalendarHandler` | Fetches, caches, and persists the event calendar |
| `LiveOpsEventScheduler` | Calculates event timing using NCrontab, starts/schedules features |
| `TimeService` (`ITimeService`) | Provides a unified time source used for timers/expiration |
| `UserStateService` | Manages player level and state |
| `AddressableAssetProvider` (`IAssetProvider`) | Wraps Unity Addressables for async asset and scene loading |
| `SceneLoaderService` (`ISceneLoaderService`) | Handles scene loading/unloading via Addressables |
| `FeatureService` | Creates and manages child DI scopes for features |
| `ViewStack` (`IViewStack`) | UI view stack management |
| `ViewFactory` (`IViewFactory`) | Creates/instantiates views |
| `PersistentStorage` (`IPersistentStorage`) | File-based persistence (JSON) under `Application.persistentDataPath` |

**3. Feature Layer** - Self-contained modules (scoped, created on demand)

Each LiveOp feature is a complete vertical slice:

```
ClickerLiveOp/
├── ClickerLiveOpInstaller.cs      # DI registration for this feature
├── ClickerLiveOpEntryPoint.cs     # Feature startup (data lifecycle + icon registration)
├── Controllers/
│   └── ClickerLiveOpPopupController.cs
├── Model/
│   ├── ClickerLiveOpData.cs       # Data model (Progress, EventStartTime)
│   └── ClickerLiveOpRepository.cs # Persistence (JSON file via PersistentStorage)
├── Services/
│   ├── IClickerLiveOpUIHandler.cs
│   └── ClickerLiveOpUIHandler.cs  # UI flow (open popup, update progress, expire handling)
└── Views/
    ├── ClickerLiveOpIconView.cs
    └── ClickerLiveOpPopup.cs
```

### UI Architecture (MVC-style)

The UI side of the project follows an MVC-style separation (pragmatic rather than “pure MVC”, since Unity is prefab-driven and favors composition).

- **Models**
  - LiveOps domain models: `LiveOpEvent`, `LiveOpsCalendar`, `LiveOpState`
  - Feature state models: `ClickerLiveOpData`, `KeyCollectLiveOpData`, `PlayGamesLiveOpData`
  - Persistence: `IRepository<T>` + feature repositories (store/restore across sessions)

- **Views**
  - Lobby/UI prefabs and scenes loaded via Addressables (e.g. `LobbyView`, icon and popup prefabs)
  - Views are created/stacked via `ViewStack` / `ViewFactory`
  - LiveOp theming is driven by per-event config assets loaded from `{FeatureType}/Config`

- **Controllers**
  - Plain C# controllers (e.g. `*PopupController`) injected via VContainer to orchestrate view interactions and update model state
  - Controllers are started through `IControllerService` (scoped per LiveOp feature) and typically instantiate/own the view lifecycle
  - Feature “UI handlers” act as coordinators between view + model persistence (open popup, update counters, close, etc.)

### Event Scheduling Flow

1. **Fetch Calendar** - `LiveOpsApiService` requests events from server (or mock)
2. **Parse & Cache** - `LiveOpsCalendarHandler` deserializes response, stores locally
3. **For each event**, the `LiveOpsEventScheduler`:
   - Checks eligibility: `PlayerLevel >= Event.EntryLevel` - skip if not met
   - Calculates next occurrence using NCrontab cron expression
   - If currently active → start immediately
   - If future → schedule async start with `UniTask.Delay`
4. **Start Feature** - Creates child DI scope, runs `EntryPoint.StartAsync()`
5. **Restore/Validate Data** - `LiveOpDataLifecycle<T>` restores persisted data and resets it when event repetition is detected (`EventStartTime != LiveOpState.StartTime`)
6. **Load Assets** - `AssetScope` loads config from `{EventType}/Config` address
7. **Register Icon** - Icon appears in lobby with countdown timer
8. **On Expiration** - Icon shows expired state, stays visible until clicked
9. **On Click** - Feature disposes, `AssetScope` releases all loaded assets

### Dependency Injection Scopes

```
RootScope (Singleton)
│
├── Core Services (`ITimeService`, `IAssetProvider`, `ISceneLoaderService`, `IViewStack`, etc.)
├── LiveOps Services (`ILiveOpsCalendarHandler`, `ILiveOpsEventScheduler`, `ILiveOpsService`, etc.)
│
└── Child Scopes (created per active LiveOp)
    ├── ClickerLiveOp Scope
    │   └── EntryPoint, Repository, UIHandler, Controllers
    ├── KeyCollectLiveOp Scope
    │   └── ...
    └── PlayGamesLiveOp Scope
        └── ...
```

Child scopes inherit parent services and are disposed when the event ends.

### Key Architectural Patterns

| Pattern | Implementation |
|---------|---------------|
| **Dependency Injection** | VContainer - Root scope for singletons, child scopes per feature |
| **Repository Pattern** | `IRepository<T>` for data persistence with JSON serialization |
| **Service Layer** | Core services (Assets, Scenes, Views) + Feature services (LiveOps, UserState) |
| **Factory Pattern** | `ILiveOpInstallerFactory` creates DI installers per event type |
| **Scope-based Asset Management** | `AssetScope` tracks and releases assets per feature lifecycle |

## Why NCrontab?

NCrontab is a standard for creating schedules and is used across all sorts of systems. Also I've decompiled Paper.io 2 and discovered that it's also used there.

## Live Ops Event Types

The system implements three distinct event types:

| Event Type | Data Tracked | Description |
|------------|-------------|-------------|
| **ClickerLiveOp** | `Progress` (click count) | Counts popup icon clicks |
| **KeyCollectLiveOp** | `KeysCollected` | Tracks collected keys |
| **PlayGamesLiveOp** | `GamesPlayed` | Counts gameplay sessions |

Each event has:
- **EntryLevel**: Minimum player level required
- **Schedule**: NCrontab cron expression
- **Duration**: How long the event stays active
- **Themed Assets**: Unique icon and popup prefabs (the icon prefab supports both active and expired states - expired state is applied by the common icon view)

## Addressables / Asset Bundles Layout

Assets are organized into logical groups that can be updated independently.

### Asset Groups

| Group | Contents |
|-------|----------|
| **Lobby** | Lobby scene, ParallaxView, LobbyView |
| **Gameplay** | Gameplay scene, HUDView |
| **UI Generic** | Fonts, shaders, shared UI prefabs (incl. base event icon) |
| **ClickerLiveOp** | Config asset (icon + popup prefabs) |
| **KeyCollectLiveOp** | Config asset (icon + popup prefabs) |
| **PlayGamesLiveOp** | Config asset (icon + popup prefabs) |

### Address Naming Convention

LiveOp configs follow the pattern `{FeatureType}/Config`:
- `ClickerLiveOp/Config`
- `KeyCollectLiveOp/Config`
- `PlayGamesLiveOp/Config`

This allows loading by event type dynamically:
```csharp
var address = $"{eventState.Type}/Config";
var config = await assetScope.LoadAssetAsync<ILiveOpConfig>(address, cancellationToken);
```

### Asset Loading Pattern

```csharp
// Each feature creates its own AssetScope
_assetScope = new AssetScope(_assetProvider);

// Load config using convention-based naming
var config = await _assetScope.LoadAssetAsync<ILiveOpConfig>(
    $"{featureType}/Config", 
    cancellationToken
);

// Config contains references to themed prefabs
config.IconPrefab   // Icon prefab (supports active + expired states)
config.PopupPrefab  // Themed popup with animations
```

Benefits of this organization:
- **Independent Updates**: Update a single event without redownloading everything
- **Lazy Loading**: Assets loaded only when event becomes active
- **Automatic Cleanup**: `AssetScope.Dispose()` releases all tracked assets

## Project Structure

```
LiveOpsVoodoo/
├── LiveOpsClient/                    # Unity Project
│   └── Assets/
│       ├── _Core/
│       │   └── Scripts/
│       │       ├── Runtime/
│       │       │   ├── Features/
│       │       │   │   ├── LiveOps/          # Core LiveOps system
│       │       │   │   │   ├── Services/
│       │       │   │   │   │   ├── Scheduler/  # Event scheduling
│       │       │   │   │   │   ├── Calendar/   # Calendar management
│       │       │   │   │   │   └── Api/        # Backend communication
│       │       │   │   │   └── Models/         # Data models
│       │       │   │   │
│       │       │   │   ├── ClickerLiveOp/    # Clicker event feature
│       │       │   │   ├── KeyCollectLiveOp/ # Key collect event feature
│       │       │   │   ├── PlayGamesLiveOp/  # Play games event feature
│       │       │   │   │
│       │       │   │   ├── UserState/        # Player level management
│       │       │   │   └── Gameplay/         # Core gameplay
│       │       │   │
│       │       │   ├── Services/             # Core services
│       │       │   │   ├── AssetManagement/  # Addressables wrapper
│       │       │   │   ├── SceneLoader/      # Scene management
│       │       │   │   └── ViewStack/        # UI view management
│       │       │   │
│       │       │   └── Infrastructure/       # Boot & DI setup
│       │       │
│       │       └── Tests/                    # Unit tests
│       │
│       └── AddressableAssetsData/            # Addressables configuration
│
└── LiveOpsServer/                    # .NET Backend (Mock)
    ├── LiveOpsModels/                # Shared DTOs
    └── LiveOpsServer/                # ASP.NET Core API
```

## Design Decision: Feature-Specific Logic

> **Note**: A significant portion of LiveOps logic was intentionally kept within individual features rather than extracted into generic components.

From my experience, attempting to create a fully generic setup for LiveOps events often leads to:

1. **Over-abstraction**: Generic systems try to accommodate every possible use case, resulting in complex configuration that's harder to understand than feature-specific code

2. **Leaky Abstractions**: Each event type inevitably has unique requirements, often times they're event mini-games (different data models, UI behaviors, validation rules) that break the generic pattern

3. **Maintenance Burden**: When a specific event needs a unique behavior, you either: (a) add another extension point to the generic system, bloating it further, or (b) work around it with hacks

4. **Slower Development**: Developers spend time figuring out how to fit their needs into the generic framework instead of just implementing what's needed

**The current approach**:
- Common infrastructure (scheduling, asset loading, icon handling) is shared
- Feature-specific logic (data models, UI, controllers) lives in each feature
- Adding a new event type is straightforward: copy an existing feature and modify

This trades some code duplication for clarity, maintainability, and development speed.

## Technologies Used

- **VContainer** - Dependency Injection
- **UniTask** - Async/await for Unity
- **NCrontab** - Cron expression parsing
- **ZLinq** - Zero-allocation LINQ operations
- **ASP.NET Core** - Backend mock server

## Getting Started

1. Open `LiveOpsClient/` in Unity
2. Open `Boot` scene (`Assets/_Core/Scenes/Boot.unity`)
3. Run the mock server in `LiveOpsServer/`
4. Enter Play Mode

You can also use bundles mode by turning it on in `Tools/Addressables/Use Bundles Mode`

