using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ScreenshotTool.Screenshotters;

/// <summary>
/// Screenshot capture for LmButton component
/// </summary>
public class LmButtonScreenshotter : IComponentScreenshotter
{
    public string ComponentName => "LmButton";

    public async Task CaptureScreenshotsAsync(IPage page, string baseUrl, string outputDir)
    {
        await page.GotoAsync($"{baseUrl}/components/button");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Wait for Blazor to fully initialize
        await Task.Delay(1000);

        var componentDir = Path.Combine(outputDir, ComponentName);
        Directory.CreateDirectory(componentDir);

        // Overview screenshot - just the basic button
        var basicSection = page.Locator(".demo-section").First;
        await basicSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "overview.png")
        });
        Console.WriteLine("  ✓ overview.png");

        // Basic button
        await basicSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "basic.png")
        });
        Console.WriteLine("  ✓ basic.png");

        // Custom text button
        var customTextSection = page.Locator(".demo-section").Nth(1);
        await customTextSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "custom-text.png")
        });
        Console.WriteLine("  ✓ custom-text.png");

        // Click handler - click the button a few times first
        var clickHandlerSection = page.Locator(".demo-section").Nth(2);
        var incrementButton = clickHandlerSection.Locator("button");
        await incrementButton.ClickAsync();
        await incrementButton.ClickAsync();
        await incrementButton.ClickAsync();
        await Task.Delay(300);
        await clickHandlerSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "click-handler.png")
        });
        Console.WriteLine("  ✓ click-handler.png");

        // Custom style
        var customStyleSection = page.Locator(".demo-section").Nth(3);
        await customStyleSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "custom-style.png")
        });
        Console.WriteLine("  ✓ custom-style.png");

        // Async operation - click the save button first
        var asyncSection = page.Locator(".demo-section").Nth(4);
        var saveButton = asyncSection.Locator("button");
        await saveButton.ClickAsync();
        await Task.Delay(1500); // Wait for async operation to complete
        await asyncSection.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = Path.Combine(componentDir, "async-operation.png")
        });
        Console.WriteLine("  ✓ async-operation.png");
    }
}
