using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ScreenshotTool.Screenshotters;

/// <summary>
/// Screenshot capture for LmCounter component
/// </summary>
public class LmCounterScreenshotter : IComponentScreenshotter
{
    public string ComponentName => "LmCounter";

    public async Task CaptureScreenshotsAsync(IPage page, string baseUrl, string outputDir)
    {
        await page.GotoAsync($"{baseUrl}/components/counter");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Wait for Blazor to fully initialize
        await Task.Delay(1000);

        var componentDir = Path.Combine(outputDir, ComponentName);
        Directory.CreateDirectory(componentDir);

        // Overview screenshot - just the basic counter
        var basicSection = page.Locator(".demo-section").First;
        await basicSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "overview.png")
        });
        Console.WriteLine("  ✓ overview.png");

        // Basic counter - click increment a couple times
        var incrementButton = basicSection.Locator("button").First;
        await incrementButton.ClickAsync();
        await incrementButton.ClickAsync();
        await Task.Delay(300);
        await basicSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "basic.png")
        });
        Console.WriteLine("  ✓ basic.png");

        // Custom initial count
        var customInitialSection = page.Locator(".demo-section").Nth(1);
        await customInitialSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "custom-initial.png")
        });
        Console.WriteLine("  ✓ custom-initial.png");

        // Custom increment
        var customIncrementSection = page.Locator(".demo-section").Nth(2);
        await customIncrementSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "custom-increment.png")
        });
        Console.WriteLine("  ✓ custom-increment.png");

        // Countdown counter
        var countdownSection = page.Locator(".demo-section").Nth(4);
        await countdownSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "countdown.png")
        });
        Console.WriteLine("  ✓ countdown.png");

        // Multiple instances - click each a few times to show independence
        var multipleSection = page.Locator(".demo-section").Nth(5);
        var counters = multipleSection.Locator(".lm-counter");
        
        // Click first counter twice
        await counters.Nth(0).Locator("button").First.ClickAsync();
        await counters.Nth(0).Locator("button").First.ClickAsync();
        
        // Click second counter once
        await counters.Nth(1).Locator("button").First.ClickAsync();
        
        // Click third counter three times (should go down)
        await counters.Nth(2).Locator("button").First.ClickAsync();
        await counters.Nth(2).Locator("button").First.ClickAsync();
        await counters.Nth(2).Locator("button").First.ClickAsync();
        
        await Task.Delay(300);
        await multipleSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "multiple-instances.png")
        });
        Console.WriteLine("  ✓ multiple-instances.png");
    }
}
