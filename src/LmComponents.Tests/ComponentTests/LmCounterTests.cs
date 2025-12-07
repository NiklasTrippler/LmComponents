using Bunit;
using LmComponents.Components;
using Xunit;

namespace LmComponents.Tests.ComponentTests;

public class LmCounterTests : TestContext
{
    [Fact]
    public void LmCounter_RendersWithDefaultInitialCount()
    {
        // Arrange & Act
        var cut = Render<LmCounter>();

        // Assert
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("0", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_RendersWithCustomInitialCount()
    {
        // Arrange & Act
        var cut = Render<LmCounter>(parameters => parameters
            .Add(p => p.InitialCount, 10));

        // Assert
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("10", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_IncrementsBy1_WhenDefaultIncrementAmount()
    {
        // Arrange
        var cut = Render<LmCounter>();
        var buttons = cut.FindAll("button");
        var incrementButton = buttons[0]; // First button is increment

        // Act
        incrementButton.Click();

        // Assert
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("1", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_IncrementsByCustomAmount()
    {
        // Arrange
        var cut = Render<LmCounter>(parameters => parameters
            .Add(p => p.IncrementAmount, 5));
        var buttons = cut.FindAll("button");
        var incrementButton = buttons[0];

        // Act
        incrementButton.Click();

        // Assert
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("5", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_IncrementsMultipleTimes()
    {
        // Arrange
        var cut = Render<LmCounter>(parameters => parameters
            .Add(p => p.IncrementAmount, 2));
        var buttons = cut.FindAll("button");
        var incrementButton = buttons[0];

        // Act
        incrementButton.Click();
        incrementButton.Click();
        incrementButton.Click();

        // Assert
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("6", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_DecrementsWithNegativeIncrementAmount()
    {
        // Arrange
        var cut = Render<LmCounter>(parameters => parameters
            .Add(p => p.InitialCount, 10)
            .Add(p => p.IncrementAmount, -1));
        var buttons = cut.FindAll("button");
        var incrementButton = buttons[0];

        // Act
        incrementButton.Click();

        // Assert
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("9", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_ResetsToInitialCount()
    {
        // Arrange
        var cut = Render<LmCounter>(parameters => parameters
            .Add(p => p.InitialCount, 5));
        var buttons = cut.FindAll("button");
        var incrementButton = buttons[0];
        var resetButton = buttons[1]; // Second button is reset

        // Act - Increment a few times
        incrementButton.Click();
        incrementButton.Click();
        incrementButton.Click();

        // Verify it incremented
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("8", counterValue.TextContent);

        // Act - Reset
        resetButton.Click();

        // Assert - Back to initial count
        counterValue = cut.Find(".counter-value");
        Assert.Equal("5", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_ResetsToZero_WhenDefaultInitialCount()
    {
        // Arrange
        var cut = Render<LmCounter>();
        var buttons = cut.FindAll("button");
        var incrementButton = buttons[0];
        var resetButton = buttons[1];

        // Act - Increment
        incrementButton.Click();
        incrementButton.Click();

        // Verify incremented
        var counterValue = cut.Find(".counter-value");
        Assert.Equal("2", counterValue.TextContent);

        // Act - Reset
        resetButton.Click();

        // Assert
        counterValue = cut.Find(".counter-value");
        Assert.Equal("0", counterValue.TextContent);
    }

    [Fact]
    public void LmCounter_RendersTwoButtons()
    {
        // Arrange & Act
        var cut = Render<LmCounter>();

        // Assert
        var buttons = cut.FindAll("button");
        Assert.Equal(2, buttons.Count);
    }

    [Fact]
    public void LmCounter_ButtonsHaveCorrectText()
    {
        // Arrange & Act
        var cut = Render<LmCounter>();

        // Assert
        var buttons = cut.FindAll("button");
        Assert.Equal("Increment", buttons[0].TextContent);
        Assert.Equal("Reset", buttons[1].TextContent);
    }
}
