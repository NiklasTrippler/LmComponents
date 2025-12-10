using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ScreenshotTool;

/// <summary>
/// Interface for component screenshot capture
/// </summary>
public interface IComponentScreenshotter
{
    /// <summary>
    /// The name of the component (e.g., "LmButton", "LmCounter")
    /// </summary>
    string ComponentName { get; }

    /// <summary>
    /// Capture all screenshots for this component
    /// </summary>
    /// <param name="page">The Playwright page instance</param>
    /// <param name="baseUrl">Base URL of the Notebook application</param>
    /// <param name="outputDir">Root output directory for screenshots</param>
    Task CaptureScreenshotsAsync(IPage page, string baseUrl, string outputDir);
}
