using Bunit;
using LmComponents.Components;
using Xunit;

namespace LmComponents.Tests.ComponentTests;

public class LmButtonTests : TestContext
{
    [Fact]
    public void LmButton_Renders_WithDefaultText()
    {
        // Arrange & Act
        var cut = Render<LmButton>();

        // Assert
        var button = cut.Find("button");
        Assert.Equal("Button", button.TextContent);
    }

    [Fact]
    public void LmButton_Renders_WithCustomText()
    {
        // Arrange & Act
        var cut = Render<LmButton>(parameters => parameters
            .Add(p => p.Text, "Click Me"));

        // Assert
        var button = cut.Find("button");
        Assert.Equal("Click Me", button.TextContent);
    }

    [Fact]
    public void LmButton_AppliesBaseClass()
    {
        // Arrange & Act
        var cut = Render<LmButton>();

        // Assert
        var button = cut.Find("button");
        Assert.Contains("lm-button", button.ClassName);
    }

    [Fact]
    public void LmButton_AppliesCustomCssClass()
    {
        // Arrange & Act
        var cut = Render<LmButton>(parameters => parameters
            .Add(p => p.CssClass, "custom-class"));

        // Assert
        var button = cut.Find("button");
        Assert.Contains("lm-button", button.ClassName);
        Assert.Contains("custom-class", button.ClassName);
    }

    [Fact]
    public void LmButton_InvokesOnClickCallback()
    {
        // Arrange
        var clicked = false;
        var cut = Render<LmButton>(parameters => parameters
            .Add(p => p.OnClick, () => clicked = true));

        // Act
        var button = cut.Find("button");
        button.Click();

        // Assert
        Assert.True(clicked);
    }

    [Fact]
    public void LmButton_InvokesOnClickCallback_MultipleClicks()
    {
        // Arrange
        var clickCount = 0;
        var cut = Render<LmButton>(parameters => parameters
            .Add(p => p.OnClick, () => clickCount++));

        // Act
        var button = cut.Find("button");
        button.Click();
        button.Click();
        button.Click();

        // Assert
        Assert.Equal(3, clickCount);
    }

    [Fact]
    public async Task LmButton_InvokesAsyncOnClickCallback()
    {
        // Arrange
        var clicked = false;
        var cut = Render<LmButton>(parameters => parameters
            .Add(p => p.OnClick, async () =>
            {
                await Task.Delay(10);
                clicked = true;
            }));

        // Act
        var button = cut.Find("button");
        await button.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // Assert
        Assert.True(clicked);
    }
}
