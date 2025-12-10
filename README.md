# LmComponents - Language Model Components

A Blazor component library specifically designed to be easily developed and maintained by AI code assistants like Claude and Gemini.

## Purpose

LmComponents (Language Model Components) is built with a unique focus: making it effortless for Large Language Models (LLMs) to understand, navigate, and contribute to the codebase. This design philosophy influences every aspect of the project, from documentation structure to component organization.

## Why LLM-Friendly?

Modern AI assistants can significantly accelerate development when given clear, well-structured information. LmComponents achieves this through:

- **Hierarchical Documentation**: Quick-reference summaries in `Components.Md` with links to detailed docs
- **Visual Documentation**: Screenshots showing component appearance and behavior
- **Minimal Context Loading**: LLMs can grasp the entire component library structure without reading full documentation
- **Clear Dependencies**: Every component explicitly lists its dependencies
- **Simple Navigation**: The Storybook demo app uses standard HTML links compatible with MCP browser tools
- **Consistent Structure**: Predictable patterns across all components and documentation
- **Automated Tooling**: Screenshot capture tool for maintaining visual documentation

## Project Structure

```
LmComponents/
├── README.md                        # This file
├── Components.Md                    # Quick reference for all components
├── CONTRIBUTING.md                  # How to contribute (for humans and AIs)
├── src/
│   ├── LmComponents.Components/     # Component library
│   ├── LmComponents.Storybook/      # Interactive demo/test application
│   └── LmComponents.Tests/          # Component tests
├── docs/                            # Detailed component documentation
│   ├── screenshots/                 # Component screenshots
│   └── LmComponents.Components/
└── tools/
    └── ScreenshotTool/              # Automated screenshot capture tool
```

## Quick Start

### Build the Solution
```bash
dotnet build
```

### Run the Storybook
```bash
dotnet run --project src/LmComponents.Storybook
```

The Storybook application provides an interactive environment where you can browse and test all available components.

### Run Tests
```bash
dotnet test
```

### Capture Screenshots

**Start the Storybook** (in a separate terminal/window):
```bash
dotnet run --project src/LmComponents.Storybook
```

**Run the Screenshot Tool** (in another terminal):
```bash
# Capture all components
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots

# Or capture specific component(s)
dotnet tools/ScreenshotTool/bin/Debug/net10.0/ScreenshotTool.dll http://localhost:5285 docs/screenshots LmButton
```

See `tools/ScreenshotTool/README.md` for detailed information about the screenshot automation tool.

## Documentation Structure

### Components.Md
Quick reference file with:
- Component name and purpose
- Visual preview (screenshot)
- Dependencies on other components
- Link to full documentation

Read this first to get an overview of all available components.

### docs/\<namespace\>/ComponentName.Md
Detailed documentation for each component:
- Full component overview
- Visual preview section with feature screenshots
- All parameters and their types
- Events and callbacks
- Usage examples
- Integration notes

### CONTRIBUTING.md
Guidelines for contributing to the project:
- **CRITICAL**: Documentation synchronization rules
- Project architecture and organization
- LLM-friendly design principles
- Step-by-step guide for adding new components
- Testing and documentation standards

## Contributing

**Please read [CONTRIBUTING.md](CONTRIBUTING.md) before making any changes to this project.**

### Quick Guidelines

For both human developers and AI assistants:

1. **Always read `Components.Md` first** to understand the component landscape
2. **Read `CONTRIBUTING.md`** for architectural decisions and mandatory documentation rules
3. **Update documentation synchronously** with code changes - this is not optional
4. Follow the established patterns for component structure
5. Ensure Storybook demos remain simple and MCP-browser-compatible

The most critical rule: **Every code change MUST be accompanied by documentation updates.** See CONTRIBUTING.md for detailed rules.

## Components

See `Components.Md` for the complete list of available components.

## Technology Stack

- **.NET 10.0**: Latest .NET framework
- **Blazor**: Modern web UI framework
- **bUnit**: Blazor component testing
- **xUnit**: Test framework

## License

[Add your license here]

## Support

[Add support information here]
