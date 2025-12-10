using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using ScreenshotTool.Screenshotters;

namespace ScreenshotTool;

class Program
{
    private static readonly Dictionary<string, IComponentScreenshotter> Screenshotters = new()
    {
        { "LmButton", new LmButtonScreenshotter() },
        { "LmCounter", new LmCounterScreenshotter() }
    };

    static async Task Main(string[] args)
    {
        Console.WriteLine("LmComponents Screenshot Tool");
        Console.WriteLine("============================\n");

        if (args.Length < 2)
        {
            PrintUsage();
            return;
        }

        string baseUrl = args[0];
        string outputDir = args[1];
        
        // Get component names from args (if provided), otherwise capture all
        List<string> componentsToCapture = args.Length > 2 
            ? args.Skip(2).ToList() 
            : Screenshotters.Keys.ToList();

        // Validate component names
        var invalidComponents = componentsToCapture
            .Where(c => !Screenshotters.ContainsKey(c))
            .ToList();

        if (invalidComponents.Any())
        {
            Console.WriteLine($"Error: Unknown component(s): {string.Join(", ", invalidComponents)}");
            Console.WriteLine($"\nAvailable components: {string.Join(", ", Screenshotters.Keys)}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDir);

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
        });

        // Capture screenshots for selected components
        foreach (var componentName in componentsToCapture)
        {
            var screenshotter = Screenshotters[componentName];
            Console.WriteLine($"Taking screenshots for {componentName} component...");
            
            try
            {
                await screenshotter.CaptureScreenshotsAsync(page, baseUrl, outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error capturing screenshots for {componentName}: {ex.Message}");
            }
            
            Console.WriteLine();
        }

        Console.WriteLine("✓ Screenshot capture complete!");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: ScreenshotTool <base-url> <output-directory> [component1] [component2] ...");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  base-url          URL where the Storybook application is running");
        Console.WriteLine("  output-directory  Directory where screenshots will be saved");
        Console.WriteLine("  component(s)      Optional. Specific component(s) to capture.");
        Console.WriteLine("                    If omitted, all components will be captured.");
        Console.WriteLine();
        Console.WriteLine("Available components:");
        foreach (var component in Screenshotters.Keys)
        {
            Console.WriteLine($"  - {component}");
        }
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  # Capture all components:");
        Console.WriteLine("  ScreenshotTool http://localhost:5285 docs/screenshots");
        Console.WriteLine();
        Console.WriteLine("  # Capture only LmButton:");
        Console.WriteLine("  ScreenshotTool http://localhost:5285 docs/screenshots LmButton");
        Console.WriteLine();
        Console.WriteLine("  # Capture LmButton and LmCounter:");
        Console.WriteLine("  ScreenshotTool http://localhost:5285 docs/screenshots LmButton LmCounter");
    }
}
