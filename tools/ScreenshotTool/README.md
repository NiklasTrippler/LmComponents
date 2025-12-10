# LmComponents Screenshot Tool

An automated screenshot capture tool for LmComponents documentation using Playwright.

## Purpose

This tool automatically captures screenshots of components in the LmComponents library by:
1. Launching a headless Chromium browser
2. Navigating to each component demo page
3. Interacting with components to show different states
4. Capturing screenshots at specified viewport size (1280x720)
5. Saving screenshots to the appropriate documentation directories

## Architecture

The tool uses a modular architecture to keep the codebase maintainable:

- **`IComponentScreenshotter`**: Interface defining the contract for component screenshotters
- **`Screenshotters/`**: Folder containing one class per component
  - `LmButtonScreenshotter.cs`: Handles LmButton screenshots
  - `LmCounterScreenshotter.cs`: Handles LmCounter screenshots
  - *(Add new component screenshotters here)*
- **`Program.cs`**: Main entry point that orchestrates the screenshot capture

This architecture ensures that `Program.cs` stays clean and each component's screenshot logic is isolated in its own file.

## Prerequisites

1. Playwright CLI tool installed globally:
   ```bash
   dotnet tool update --global Microsoft.Playwright.CLI
   ```

2. Chromium browser installed for Playwright:
   ```bash
   playwright install chromium
   ```

## Usage

### Step 1: Start the Storybook Application

The Storybook application must be running before you can capture screenshots.

**Important**: Start the Storybook in a separate terminal/process to keep it running in the background.

**On Windows (PowerShell):**
```powershell
# Open a new PowerShell window and run:
dotnet run --project src/LmComponents.Storybook

# Keep this window open while running the screenshot tool
```

**On Linux/macOS (Bash):**
```bash
# Run in background using &
dotnet run --project src/LmComponents.Storybook &

# Or use a terminal multiplexer like tmux/screen
```

**Cross-Platform Alternative:**
Use your IDE's terminal or open a dedicated terminal window to run the Storybook, then use another terminal for the screenshot tool.

### Step 2: Run the Screenshot Tool

**Capture all components:**
```bash
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots
```

**Capture specific component(s):**
```bash
# Just LmButton
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton

# Multiple components
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton LmCounter
```

**Windows (shorter path):**
```powershell
dotnet tools\ScreenshotTool\bin\Debug\net10.0\ScreenshotTool.dll http://localhost:5285 docs\screenshots LmButton
```

### Command-Line Arguments

```
ScreenshotTool <base-url> <output-directory> [component1] [component2] ...
```

**Arguments:**
- `base-url`: URL where the Storybook application is running (e.g., `http://localhost:5285`)
- `output-directory`: Directory where screenshots will be saved (e.g., `docs/screenshots`)
- `component(s)`: (Optional) Specific component name(s) to capture. If omitted, all components will be captured.

**Available components:** LmButton, LmCounter (and any future components you add)

## Adding Screenshots for New Components

When adding a new component, create a new screenshotter class:

### Step 1: Create the Screenshotter Class

Create a new file in `Screenshotters/` folder:

**File**: `Screenshotters/LmYourComponentScreenshotter.cs`

```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ScreenshotTool.Screenshotters;

/// <summary>
/// Screenshot capture for LmYourComponent component
/// </summary>
public class LmYourComponentScreenshotter : IComponentScreenshotter
{
    public string ComponentName => "LmYourComponent";

    public async Task CaptureScreenshotsAsync(IPage page, string baseUrl, string outputDir)
    {
        await page.GotoAsync($"{baseUrl}/components/yourcomponent");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Task.Delay(1000); // Wait for Blazor initialization

        var componentDir = Path.Combine(outputDir, ComponentName);
        Directory.CreateDirectory(componentDir);

        // Overview screenshot
        var basicSection = page.Locator(".demo-section").First;
        await basicSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "overview.png")
        });
        Console.WriteLine("  ✓ overview.png");

        // Feature screenshots with interactions
        var button = page.Locator("button").First;
        await button.ClickAsync();
        await Task.Delay(300);
        
        await basicSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "feature.png")
        });
        Console.WriteLine("  ✓ feature.png");
    }
}
```

### Step 2: Register the Screenshotter

Add your screenshotter to the `Screenshotters` dictionary in `Program.cs`:

```csharp
private static readonly Dictionary<string, IComponentScreenshotter> Screenshotters = new()
{
    { "LmButton", new LmButtonScreenshotter() },
    { "LmCounter", new LmCounterScreenshotter() },
    { "LmYourComponent", new LmYourComponentScreenshotter() } // Add this line
};
```

### Step 3: Rebuild and Run

```bash
# Rebuild the tool
dotnet build tools/ScreenshotTool

# Capture screenshots for your new component
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmYourComponent
```

That's it! The modular architecture keeps each component's screenshot logic separate and maintainable.

## Screenshot Naming Convention

- `overview.png` - Used in Components.Md for quick visual reference
- Descriptive feature names:
  - `basic.png` - Basic component appearance
  - `click-handler.png` - Component responding to interaction
  - `custom-style.png` - Custom styling applied
  - `async-operation.png` - Async operation in progress/completed
  - etc.

## Output

Screenshots are saved to:
```
docs/screenshots/
├── LmButton/
│   ├── overview.png
│   ├── basic.png
│   ├── custom-text.png
│   ├── click-handler.png
│   ├── custom-style.png
│   └── async-operation.png
├── LmCounter/
│   ├── overview.png
│   ├── basic.png
│   ├── custom-initial.png
│   ├── custom-increment.png
│   ├── countdown.png
│   └── multiple-instances.png
└── [YourComponent]/
    └── ...
```

## Technical Details

- **Browser**: Chromium (headless)
- **Viewport**: 1280x720 pixels
- **Format**: PNG
- **Library**: Microsoft.Playwright 1.49.0
- **Target Framework**: .NET 10.0

## Troubleshooting

**Error: "net::ERR_CONNECTION_REFUSED"**
- Ensure the Storybook application is running on the specified port
- Check that the URL matches the Storybook's running URL

**Error: "Playwright browser not found"**
- Run `playwright install chromium` to install the browser

**Screenshots are blank or incomplete**
- Increase the `Task.Delay()` time to allow Blazor to fully render
- Check that the demo page structure matches the locators in the code

## Integration with Documentation Workflow

This tool is part of the mandatory documentation synchronization process. When adding or modifying components:

1. ✅ Create/update component code
2. ✅ Create/update component documentation
3. ✅ Create/update Storybook demo
4. ✅ **Capture screenshots using this tool**
5. ✅ Reference screenshots in documentation
6. ✅ Update Components.Md with overview screenshot

See `CONTRIBUTING.md` for complete documentation requirements.
