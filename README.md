# Trebuchet

A lightweight WPF launcher that runs a sequence of installers in order. Configure one or more installer sequences in a JSON file, select one from the list, and Trebuchet walks through each installer automatically — prompting for confirmation between steps before continuing.

Created to help simplify the installation of a sequence of tools off of a flash key for my parents.

## How It Works

1. On launch, Trebuchet reads `LauncherConfig.json` from the same directory as the executable.
2. Each **entry** in the config appears as a button in the window.
3. Clicking an entry runs its assigned chain of **steps** in sequence.
4. For each step, Trebuchet:
   - Launches the installer at the configured path.
   - Shows a dialog reminding the user to follow the installer instructions.
   - After the installer closes, asks whether the installation was successful.
   - If yes, proceeds to the next step. If no, stops with a warning.

## Configuration

Copy `LauncherConfig.template.json` to `LauncherConfig.json` and fill in your installer paths and messages.

```json
{
  "$schema": "LauncherConfig.schema.json",
  "entries": [
    {
      "title": "Installer Sequence A",
      "startStep": 0
    }
  ],
  "steps": [
    {
      "stepMessage": "Installing A1",
      "path": "SequenceA\\setup_A1.exe",
      "startDelay": 0,
      "nextStep": 1
    },
    {
      "stepMessage": "Installing A2",
      "path": "SequenceA\\setup_A2.exe",
      "startDelay": 0
    }
  ]
}
```

### Fields

#### `entries`
| Field | Required | Description |
|---|---|---|
| `title` | ✅ | Label shown on the button in the launcher window. |
| `startStep` | ✅ | Zero-based index of the first step in the `steps` array to run for this entry. |

#### `steps`
| Field | Required | Description |
|---|---|---|
| `stepMessage` | ✅ | Message displayed while the installer is running. |
| `path` | ✅ | Relative or absolute path to the installer executable. Relative paths resolve from the launcher directory. |
| `startDelay` | ❌ | Milliseconds to wait before launching the installer. Defaults to `0`. |
| `nextStep` | ❌ | Zero-based index of the next step to run after this one completes. Omit on the final step of a sequence. |

A schema file (`LauncherConfig.schema.json`) is included for IntelliSense and validation in editors that support JSON Schema.

### Debug Config

Place a `LauncherConfig.debug.json` alongside `LauncherConfig.json` to override configuration in debug builds. This file is ignored by Git.

## Building

Requires [.NET 10 SDK](https://dotnet.microsoft.com/download) and Windows x64.

**Debug** — standard build, reads `LauncherConfig.debug.json`:
```powershell
dotnet build -c Debug
```

**Release** — produces a single self-contained `Trebuchet.exe` with no dependencies in the `Publish/` folder at the solution root:
```powershell
dotnet build -c Release
```

## License

MIT — see [LICENSE.md](LICENSE.md).
