# Contributing to LmComponents

This guide is designed for both human developers and AI assistants (LLMs) working on this project.

## CRITICAL: Documentation Synchronization Rules

**Every code change MUST be accompanied by corresponding documentation updates. This is not optional.**

### Why This Matters

The documentation serves as the source of truth for how this project currently works. When an LLM reads the documentation, it must reflect the actual current state of the code, architecture, and design patterns. Outdated documentation is worse than no documentation because it misleads future developers (human or AI) about how the system works.

### Mandatory Update Rules

When making ANY changes to this project, you MUST update documentation as follows:

#### 1. Component Code Changes

If you modify a component's code (add/remove parameters, change behavior, add features, fix bugs):
- **Update** `docs/<namespace>/ComponentName.Md` with the changes
- **Update** `Components.Md` if the summary or dependencies changed
- **Update** the Storybook demo page if the usage changed
- **Update** tests to reflect new behavior

#### 2. New Components

If you add a new component:
- **Create** `docs/<namespace>/ComponentName.Md` with full documentation
- **Add** entry to `Components.Md` with summary and dependencies
- **Create** demo page in Storybook showing usage
- **Create** tests for the new component
- **Update** README.md if it affects project overview

#### 3. Architectural Changes

If you change how the project is structured or organized (folder structure, naming conventions, build process, dependencies):
- **Update** this `CONTRIBUTING.md` file to reflect the new architecture
- **Update** `README.md` if it affects setup or usage
- **Update** any affected component documentation
- The documentation must clearly state "this is the current way of doing things"

#### 4. Removing Components or Features

If you remove a component or feature:
- **Remove** the component's documentation file
- **Remove** entry from `Components.Md`
- **Remove** Storybook demo page
- **Remove** tests
- **Update** any components that depended on the removed component

#### 5. Dependency Changes

If you change which components depend on other components:
- **Update** both `Components.Md` and the detailed docs with new dependencies
- **Update** any architectural documentation that describes component relationships

#### 6. Screenshot Requirements

If you add a new component, modify component appearance, or add visual features:
- **Capture screenshots** using the automated screenshot tool (see below)
- **Add overview screenshot** to `Components.Md` for quick visual reference
- **Add feature screenshots** to detailed component documentation
- **Update screenshots** when visual changes are made to components
- **Follow naming conventions**: `overview.png` for Components.Md, descriptive names for features

**Screenshot Storage Convention**: `docs/screenshots/<ComponentName>/`

**Screenshot Naming Convention**:
- `overview.png` - Used in Components.Md for quick reference
- Descriptive feature names - `basic.png`, `click-handler.png`, `custom-style.png`, etc.

**Screenshot Format**: PNG format, reasonable resolution (1280x720 viewport recommended)

**How to Capture Screenshots**:
The project includes an automated screenshot tool at `tools/ScreenshotTool` that uses Playwright to capture component screenshots.

**Step 1: Start the Storybook Application** (in a separate terminal/window)

On Windows:
```powershell
# Open a new PowerShell window and run:
dotnet run --project src/LmComponents.Storybook
```

On Linux/macOS:
```bash
# Run in background
dotnet run --project src/LmComponents.Storybook &
```

**Step 2: Run the Screenshot Tool** (in another terminal)

Capture all components:
```bash
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots
```

Capture specific component(s):
```bash
# Just one component
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton

# Multiple components
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton LmCounter
```

**For New Components**:
Create a new screenshotter class in `tools/ScreenshotTool/Screenshotters/` implementing `IComponentScreenshotter`, then register it in `Program.cs`. See the tool's README for details.

### Verification Checklist

Before completing any task, verify:
- [ ] All modified components have updated documentation
- [ ] Components.Md is in sync with all components
- [ ] Screenshots are captured and referenced in documentation
- [ ] Overview screenshots are included in Components.Md
- [ ] Feature screenshots are included in detailed component docs
- [ ] Storybook demos reflect current component usage
- [ ] Architectural changes are reflected in CONTRIBUTING.md
- [ ] No documentation references removed/renamed components

### For Future LLMs Working on This Project

When you read the documentation in this project, trust that it represents the current state. If you find documentation that contradicts the code, this is a bug that must be fixed immediately - either the code or the documentation is wrong, and they must be synchronized.

Always remember: **The documentation IS the specification of how things currently work. Keep it accurate, or the project becomes unmaintainable.**

## Project Architecture

### Solution Organization

#### LmComponents.Components
- **Type**: Class Library (Blazor Razor Class Library)
- **Purpose**: Contains all reusable Blazor components
- **Naming Convention**: Components prefixed with "Lm" (e.g., LmButton, LmCounter)
- **Organization**: Each component in its own folder
- **Location**: `src/LmComponents.Components/`

Component folder structure:
```
ComponentName/
├── LmComponentName.razor       # Component markup and code
└── LmComponentName.razor.css   # Scoped component styles
```

#### LmComponents.Storybook
- **Type**: Blazor Web App (Server)
- **Purpose**: Interactive application for browsing, testing, and documenting components
- **Key Features**:
  - Simple navigation for MCP browser tools
  - Live component demonstrations
  - Clear component isolation (one component per demo page)
  - Accessible via standard web browser without complex interactions
- **Location**: `src/LmComponents.Storybook/`

Demo page structure:
```
Components/Pages/Components/
├── ComponentNameDemo.razor       # Demo page
└── ComponentNameDemo.razor.css   # Demo page styles
```

#### LmComponents.Tests
- **Type**: Test Project (xUnit with bUnit)
- **Purpose**: Automated tests for component behavior
- **Coverage**: Unit tests for component logic, rendering tests for UI
- **Location**: `src/LmComponents.Tests/`

Test file structure:
```
ComponentTests/
└── LmComponentNameTests.cs    # All tests for one component
```

## LLM-Friendly Design Principles

This project follows specific design principles to make it easy for AI assistants to work with:

1. **Clear Documentation**: Every component has both quick reference and detailed docs
2. **Simple Navigation**: Storybook uses standard HTML links, no SPA complexity
3. **Isolated Examples**: Each component demo on separate page
4. **Explicit Dependencies**: Component.Md files clearly list what each component uses
5. **Consistent Structure**: All components follow same file/folder patterns
6. **Minimal Context**: Components.Md provides overview without loading full docs
7. **Self-Documenting Code**: Clear naming, comments where necessary
8. **Testability**: Each component designed to be easily tested

## Component Design Standards

### Using FluentUI as a Base

**IMPORTANT**: When creating new components for LmComponents, use **Microsoft FluentUI Blazor Components** as a base whenever appropriate. This provides:

- **Consistent Design System**: FluentUI components follow Microsoft's Fluent Design System with built-in accessibility, theming, and best practices
- **Reduced Custom Code**: Leverage existing, well-tested components rather than building everything from scratch
- **Better Accessibility**: FluentUI components are built with ARIA standards and keyboard navigation
- **Professional Appearance**: Modern, polished UI that works well across different contexts

**When to Use FluentUI Components**:
- Prefer wrapping or extending FluentUI components when they provide the base functionality you need
- Use FluentUI design tokens for colors, spacing, typography when creating custom components
- Follow FluentUI patterns for interaction and behavior even in custom components

**When to Build Custom**:
- When no suitable FluentUI component exists for your use case
- When wrapping would add unnecessary complexity
- When the component needs to be completely different from standard UI patterns

**Package**: `Microsoft.FluentUI.AspNetCore.Components`

**Example**: Instead of building a basic button from scratch, consider extending `FluentButton` or using it as a reference for behavior and styling.

## Adding a New Component

Follow these steps when creating a new component:

### 1. Create Component Files

```bash
mkdir src/LmComponents.Components/ComponentName
```

Create `LmComponentName.razor`:
```razor
@namespace LmComponents.Components

<div class="lm-componentname">
    <!-- Component markup -->
</div>

@code {
    // Component code with XML doc comments
}
```

Create `LmComponentName.razor.css`:
```css
.lm-componentname {
    /* Component styles */
}
```

### 2. Write Documentation

Create `docs/LmComponents.Components/ComponentName.Md`:
```markdown
# LmComponentName Component

**Namespace**: `LmComponents.Components`
**File**: `src/LmComponents.Components/ComponentName/LmComponentName.razor`

## Overview
[Component description]

## Dependencies
[List of components this depends on, or "None"]

## Parameters
[Parameter documentation]

## Usage Examples
[Code examples]
```

### 3. Update Components.Md

Add entry in alphabetical order:
```markdown
### LmComponentName
- **Purpose**: [Brief description]
- **Dependencies**: [Component](#component) or "None"
- **Documentation**: [Full Documentation](docs/LmComponents.Components/ComponentName.Md)
```

### 4. Create Storybook Demo

Create `src/LmComponents.Storybook/Components/Pages/Components/ComponentNameDemo.razor`:
```razor
@page "/components/componentname"
@rendermode InteractiveServer

<PageTitle>LmComponentName Demo</PageTitle>

<h1>LmComponentName Component</h1>

<!-- Demo sections -->
```

Update `src/LmComponents.Storybook/Components/Layout/NavMenu.razor` to add navigation link.

### 5. Create Tests

Create `src/LmComponents.Tests/ComponentTests/LmComponentNameTests.cs`:
```csharp
using Bunit;
using LmComponents.Components;
using Xunit;

namespace LmComponents.Tests.ComponentTests;

public class LmComponentNameTests : TestContext
{
    [Fact]
    public void ComponentName_RendersCorrectly()
    {
        // Test implementation
    }
}
```

### 6. Capture Screenshots

**Step 1**: Create a screenshotter class in `tools/ScreenshotTool/Screenshotters/LmComponentNameScreenshotter.cs`:
```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ScreenshotTool.Screenshotters;

public class LmComponentNameScreenshotter : IComponentScreenshotter
{
    public string ComponentName => "LmComponentName";

    public async Task CaptureScreenshotsAsync(IPage page, string baseUrl, string outputDir)
    {
        // Implement screenshot logic
    }
}
```

**Step 2**: Register it in `tools/ScreenshotTool/Program.cs`:
```csharp
private static readonly Dictionary<string, IComponentScreenshotter> Screenshotters = new()
{
    { "LmButton", new LmButtonScreenshotter() },
    { "LmCounter", new LmCounterScreenshotter() },
    { "LmComponentName", new LmComponentNameScreenshotter() } // Add this
};
```

**Step 3**: Run the screenshot tool (with Storybook running in another terminal):
```bash
# Capture just your new component
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmComponentName
```

**Step 4**: Add screenshots to documentation:
- Add overview screenshot to `Components.Md`
- Add feature screenshots to `docs/LmComponents.Components/ComponentName.Md`

### 7. Build and Test

```bash
dotnet build
dotnet test
dotnet run --project src/LmComponents.Storybook
```

Verify everything works and documentation is complete.

## Documentation Standards

### Components.Md Format

Use this exact format for consistency:
```markdown
### ComponentName
- **Purpose**: Brief one-line description
- **Dependencies**: [OtherComponent](#othercomponent) or "None"
- **Documentation**: [Full Documentation](docs/namespace/ComponentName.Md)
```

### Component Documentation Structure

Each component doc must include:
1. Component name and metadata (namespace, file location)
2. Overview and purpose
3. Dependencies (with links to other component docs)
4. Parameters (name, type, default, required, description)
5. Events/callbacks
6. Styling information (CSS classes)
7. Usage examples (basic and advanced)
8. Implementation notes
9. Testing considerations

### Code Documentation

- Use XML doc comments on all public parameters and properties
- Component code should be self-documenting with clear variable names
- Add comments only where logic is not immediately obvious

## Testing Standards

- Use bUnit for component rendering tests
- Test all component parameters
- Test event callbacks
- Test component state changes
- Test component composition (components using other components)
- Each component should have its own test file
- Test file naming: `LmComponentNameTests.cs`

## Questions or Issues?

For questions about this project structure or contributing guidelines, please open an issue.
