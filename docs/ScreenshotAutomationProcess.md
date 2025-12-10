# Screenshot Automation Process Documentation

## Overview

This document describes the automated screenshot capture process implemented for the LmComponents project. This process was created to solve the problem of maintaining up-to-date visual documentation without manual intervention.

## Problem Statement

The LmComponents project needed a way to:
1. Provide visual documentation for all components
2. Capture screenshots automatically without manual browser interaction
3. Ensure consistency in screenshot quality and format
4. Make it easy for LLMs and developers to see what components look like
5. Update screenshots easily when components change

## Solution: Automated Screenshot Tool

### Technology Choice: Playwright

**Why Playwright?**
- **Browser Automation**: Can programmatically control a real browser (Chromium, Firefox, or WebKit)
- **Headless Mode**: Runs without displaying a browser window
- **Screenshot API**: Built-in support for capturing page and element screenshots
- **Interaction Capabilities**: Can click buttons, fill forms, and interact with components
- **Cross-Platform**: Works on Windows, macOS, and Linux
- **.NET Integration**: First-class support for .NET via Microsoft.Playwright NuGet package

**Alternatives Considered:**
- Manual screenshots: Too time-consuming and error-prone
- Browser DevTools Protocol directly: More complex, Playwright provides better abstraction
- Puppeteer: JavaScript-based, Playwright is the .NET-native successor

### Implementation Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Screenshot Tool Flow                      │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  1. Launch Headless Chromium Browser                        │
│     └─ Viewport: 1280x720 pixels                           │
│                                                              │
│  2. For Each Component (LmButton, LmCounter, etc.):         │
│     ├─ Navigate to component demo page                     │
│     ├─ Wait for page load and Blazor initialization        │
│     ├─ Locate specific demo sections using CSS selectors   │
│     ├─ Interact with component (click, type, etc.)         │
│     ├─ Wait for state changes to complete                  │
│     └─ Capture screenshot of specific element              │
│                                                              │
│  3. Save Screenshots                                         │
│     └─ docs/screenshots/<ComponentName>/<feature>.png      │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Key Technical Decisions

#### 1. Element-Level Screenshots vs Full-Page
**Decision**: Use element-level screenshots (`.LocatorScreenshotAsync()`)
**Rationale**: 
- Captures only the relevant demo section, not the entire page
- Reduces image size and focuses on the component
- Eliminates navigation bars and other UI noise

#### 2. Headless vs Headed Browser
**Decision**: Headless mode (no visible browser window)
**Rationale**:
- Faster execution
- Can run in CI/CD pipelines
- Doesn't require display/GUI on server environments
- More reliable and consistent

#### 3. Viewport Size: 1280x720
**Rationale**:
- Widely supported resolution
- Good balance between detail and file size
- Matches common laptop/monitor aspect ratio
- Ensures components are shown at realistic size

#### 4. Wait Strategies
**Implementation**: Combination of `NetworkIdle` and explicit delays
**Rationale**:
- Blazor Server requires time to establish SignalR connection
- Components may have animations or async initialization
- `NetworkIdle` ensures all resources loaded
- Additional `Task.Delay()` ensures Blazor rendering completes

### Code Structure

**New Modular Architecture** (Refactored for maintainability):

```
tools/ScreenshotTool/
├── ScreenshotTool.csproj          # Project file with Playwright dependency
├── Program.cs                      # Main entry point, orchestrates capture
├── IComponentScreenshotter.cs      # Interface for component screenshotters
├── Screenshotters/                 # One file per component
│   ├── LmButtonScreenshotter.cs   # LmButton screenshot logic
│   └── LmCounterScreenshotter.cs  # LmCounter screenshot logic
├── README.md                       # Usage documentation
└── bin/Debug/net10.0/             # Compiled output
    └── ScreenshotTool.dll
```

**Benefits of Modular Architecture:**
- `Program.cs` stays clean and focused on orchestration
- Each component's screenshot logic is isolated in its own file
- Easy to add new components without bloating main file
- Better separation of concerns
- Reduces context needed when working with specific components

**Program.cs Structure:**
- `Main()`: Entry point, handles arguments, manages browser lifecycle
- `Screenshotters` Dictionary: Registry of all available component screenshotters
- `PrintUsage()`: Help text showing available components and usage

**Component Screenshotter Classes:**
- Implement `IComponentScreenshotter` interface
- Contain all screenshot logic for one specific component
- Handle navigation, interaction, and capture for that component
- Self-contained and independently testable

### Screenshot Naming Strategy

**Convention:**
- `overview.png`: Simplest form of component, used in Components.Md
- `<feature>.png`: Descriptive name showing specific feature
  - Examples: `basic.png`, `click-handler.png`, `custom-style.png`

**Why This Convention:**
- Self-documenting filenames
- Easy to understand what each screenshot shows
- Consistent pattern across all components

## Integration with Documentation Workflow

### Updated Documentation Files

1. **Components.Md**
   - Added "Visual Quick Reference" section
   - Added screenshot policy
   - Embedded overview screenshots for each component

2. **Component-Specific Docs** (Button.Md, Counter.Md)
   - Added "Visual Preview" section after Dependencies
   - Included all feature screenshots with captions
   - Updated "Last Updated" dates

3. **CONTRIBUTING.md**
   - Added "6. Screenshot Requirements" section
   - Documented storage and naming conventions
   - Added screenshot capture to "Adding a New Component" workflow
   - Updated verification checklist

4. **README.md**
   - Added screenshot tool to project structure
   - Mentioned visual documentation in "Why LLM-Friendly?" section
   - Added "Capture Screenshots" quick start section

### Mandatory Requirements for New Components

When adding a new component, developers/LLMs must:

1. ✅ Create component code
2. ✅ Create Notebook demo page
3. ✅ Update screenshot tool to capture new component
4. ✅ Run screenshot tool to generate images
5. ✅ Add overview screenshot to Components.Md
6. ✅ Add feature screenshots to detailed documentation
7. ✅ Update all three documentation files

## Running the Screenshot Tool

### Prerequisites

```bash
# Install Playwright CLI globally
dotnet tool update --global Microsoft.Playwright.CLI

# Install Chromium browser
playwright install chromium
```

### Execution Steps

**Important**: The Notebook application and screenshot tool must run in separate terminals/processes.

**On Windows:**
```powershell
# Terminal 1: Start the Notebook (keep this running)
dotnet run --project src/LmComponents.Notebook

# Terminal 2: Run screenshot tool
# Capture all components:
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots

# Capture specific component(s):
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton LmCounter
```

**On Linux/macOS:**
```bash
# Option 1: Background process
dotnet run --project src/LmComponents.Notebook &

# Then run screenshot tool
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton

# Option 2: Use terminal multiplexer (tmux/screen)
# Start Notebook in one pane, screenshot tool in another
```

**Cross-Platform Alternative:**
Use your IDE's built-in terminal feature to open multiple terminal instances.

### Output Example

```
LmComponents Screenshot Tool
============================

Taking screenshots for LmButton component...
  ✓ overview.png
  ✓ basic.png
  ✓ custom-text.png
  ✓ click-handler.png
  ✓ custom-style.png
  ✓ async-operation.png

Taking screenshots for LmCounter component...
  ✓ overview.png
  ✓ basic.png
  ✓ custom-initial.png
  ✓ custom-increment.png
  ✓ countdown.png
  ✓ multiple-instances.png

✓ All screenshots captured successfully!
```

## Extending the Tool for New Components

### Step-by-Step Guide

**Step 1: Create a Screenshotter Class**

Create a new file in `Screenshotters/` folder:

**File**: `tools/ScreenshotTool/Screenshotters/LmYourComponentScreenshotter.cs`

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
        var section = page.Locator(".demo-section").First;
        await section.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "overview.png")
        });
        Console.WriteLine("  ✓ overview.png");

        // Feature screenshots with interactions
        var button = page.Locator("button").First;
        await button.ClickAsync();
        await Task.Delay(300);
        
        await section.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "feature.png")
        });
        Console.WriteLine("  ✓ feature.png");
    }
}
```

**Step 2: Register in Program.cs**

Add your screenshotter to the `Screenshotters` dictionary:

```csharp
private static readonly Dictionary<string, IComponentScreenshotter> Screenshotters = new()
{
    { "LmButton", new LmButtonScreenshotter() },
    { "LmCounter", new LmCounterScreenshotter() },
    { "LmYourComponent", new LmYourComponentScreenshotter() } // Add this
};
```

**Step 3: Rebuild and Run**

```bash
# Rebuild the tool
dotnet build tools/ScreenshotTool

# Capture screenshots for your new component only
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmYourComponent
```

**Benefits of this Architecture:**
- Each component's logic is in its own file
- `Program.cs` stays clean and maintainable
- Easy to understand and modify individual components
- No need to touch existing component code when adding new ones
{
    await page.GotoAsync($"{baseUrl}/components/yourcomponent");
    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    await Task.Delay(1000); // Blazor initialization

    // Overview screenshot
    var section = page.Locator(".demo-section").First;
    await section.ScreenshotAsync(new LocatorScreenshotOptions
    {
        Path = Path.Combine(outputDir, "YourComponent", "overview.png")
    });
    Console.WriteLine("  ✓ overview.png");

    // Additional feature screenshots...
}
```

2. **Call Method from Main():**

```csharp
Console.WriteLine("Taking screenshots for YourComponent...");
await TakeYourComponentScreenshots(page, baseUrl, outputDir);
```

3. **Create Output Directory:**
```csharp
Directory.CreateDirectory(Path.Combine(outputDir, "YourComponent"));
```

## Benefits for LLM-Friendly Development

### Why This Matters for AI Assistants

1. **Visual Context Without Execution**: LLMs can see what components look like without running code
2. **Faster Understanding**: Screenshots provide immediate visual comprehension
3. **Reduced Token Usage**: One image worth many words of description
4. **Documentation Synchronization**: Automated process ensures screenshots stay current
5. **Pattern Recognition**: Consistent screenshot structure helps LLMs understand component patterns

### Example LLM Workflow

When an LLM (like Claude or Gemini) works on this project:

1. Reads `Components.Md` → Sees overview screenshots → Instantly understands component appearance
2. Reads detailed docs → Sees feature screenshots → Understands different states/configurations
3. Modifies component code
4. Runs screenshot tool → Updates screenshots automatically
5. Updates documentation → Screenshots stay synchronized

## Troubleshooting

### Common Issues and Solutions

**Issue**: "net::ERR_CONNECTION_REFUSED"
- **Cause**: Notebook application not running
- **Solution**: Start `dotnet run --project src/LmComponents.Notebook`

**Issue**: Screenshots are blank/white
- **Cause**: Blazor not fully initialized
- **Solution**: Increase `Task.Delay()` time (e.g., from 1000ms to 2000ms)

**Issue**: Wrong element captured
- **Cause**: CSS selector matches wrong element
- **Solution**: Update locator to be more specific (e.g., `.demo-section.First` → `.demo-section[data-demo="button-basic"]`)

**Issue**: Playwright browser not found
- **Cause**: Chromium not installed for Playwright
- **Solution**: Run `playwright install chromium`

## Future Enhancements

Potential improvements to consider:

1. **Comparison Mode**: Detect visual regressions by comparing new screenshots to baseline
2. **Configurable Viewport**: Support different screen sizes (mobile, tablet, desktop)
3. **Theme Support**: Capture screenshots in light and dark themes
4. **Annotation**: Add text annotations to screenshots highlighting specific features
5. **Video Recording**: Capture component interactions as video/GIF
6. **CI/CD Integration**: Run automatically on PR creation to verify visual changes
7. **Interactive Elements**: Capture hover states, focus states, etc.

## Conclusion

The automated screenshot tool solves the critical problem of maintaining synchronized visual documentation. By using Playwright for headless browser automation, the tool:

- ✅ Captures consistent, high-quality screenshots
- ✅ Requires no manual intervention
- ✅ Integrates seamlessly into the development workflow
- ✅ Supports the project's LLM-friendly philosophy
- ✅ Ensures documentation stays current with code changes

This approach makes LmComponents truly AI-assistant-friendly by providing visual context that LLMs can quickly understand and reference.

---

**Document Version**: 1.0  
**Last Updated**: 2025-12-10  
**Author**: GitHub Copilot (AI Assistant)
