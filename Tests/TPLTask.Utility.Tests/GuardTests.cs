using Xunit;

namespace TPLTask.Utility.Tests;

public sealed class GuardTests
{
    [Fact]
    public void NotNull_ShouldThrowArgumentNullException_WhenDataIsNull()
    {
        var paramName = "testParam";
        object data = null!;

        Assert.Throws<ArgumentNullException>(() => Guard.NotNull(paramName, data));
    }

    [Fact]
    public void NotNull_ShouldNotThrow_WhenDataIsNotNull()
    {
        var paramName = "testParam";
        var data = new object();

        var exception = Record.Exception(() => Guard.NotNull(paramName, data));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNullOrEmpty_ShouldThrowArgumentNullException_WhenTextIsNull()
    {
        var paramName = "testParam";
        string text = null!;

        Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty(paramName, text));
    }

    [Fact]
    public void NotNullOrEmpty_ShouldThrowArgumentNullException_WhenTextIsEmpty()
    {
        var paramName = "testParam";
        var text = string.Empty;

        Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty(paramName, text));
    }

    [Fact]
    public void NotNullOrEmpty_ShouldNotThrow_WhenTextIsNotNullOrEmpty()
    {
        var paramName = "testParam";
        var text = "non-empty";

        var exception = Record.Exception(() => Guard.NotNullOrEmpty(paramName, text));
        Assert.Null(exception);
    }

    [Fact]
    public void NotHasAnySymbol_ShouldThrowArgumentException_WhenTextContainsInvalidSymbol()
    {
        var paramName = "testParam";
        var text = "invalid@text";
        var invalidSymbols = new[] { '@', '#', '$' };

        Assert.Throws<ArgumentException>(() => Guard.NotHasAnySymbol(paramName, text, invalidSymbols));
    }

    [Fact]
    public void NotHasAnySymbol_ShouldNotThrow_WhenTextDoesNotContainInvalidSymbol()
    {
        var paramName = "testParam";
        var text = "validText";
        var invalidSymbols = new[] { '@', '#', '$' };

        var exception = Record.Exception(() => Guard.NotHasAnySymbol(paramName, text, invalidSymbols));
        Assert.Null(exception);
    }

    [Fact]
    public void NotHasSymbol_ShouldThrowArgumentException_WhenTextContainsInvalidSymbol()
    {
        var paramName = "testParam";
        var text = "invalidText";
        var invalidSymbol = 'i';

        Assert.Throws<ArgumentException>(() => Guard.NotHasSymbol(paramName, text, invalidSymbol));
    }

    [Fact]
    public void NotHasSymbol_ShouldNotThrow_WhenTextDoesNotContainInvalidSymbol()
    {
        var paramName = "testParam";
        var text = "validText";
        var invalidSymbol = 'z';

        var exception = Record.Exception(() => Guard.NotHasSymbol(paramName, text, invalidSymbol));
        Assert.Null(exception);
    }

    [Fact]
    public void NotInvalidFilePath_ShouldThrowArgumentException_WhenTextContainsInvalidFileNameChars()
    {
        var paramName = "testParam";
        var text = "invalid|path.txt";

        Assert.Throws<ArgumentException>(() => Guard.NotInvalidFilePath(paramName, text));
    }

    [Fact]
    public void NotInvalidFilePath_ShouldNotThrow_WhenTextIsValidFilePath()
    {
        var paramName = "testParam";
        var text = "valid_path.txt";

        var exception = Record.Exception(() => Guard.NotInvalidFilePath(paramName, text));
        Assert.Null(exception);
    }

    [Fact]
    public void DoFileNameValid_ShouldReplaceInvalidChars_WithUnderscore()
    {
        var fileName = "invalid|name.txt";

        Guard.DoFileNameValid(ref fileName);

        Assert.Equal("invalid_name.txt", fileName);
    }

    [Fact]
    public void Is_ShouldThrowArgumentException_WhenDataIsNotOfExpectedType()
    {
        var paramName = "testParam";
        object data = 42;

        Assert.Throws<ArgumentException>(() => Guard.Is<string>(paramName, data, out var _));
    }

    [Fact]
    public void Is_ShouldNotThrow_WhenDataIsOfExpectedType()
    {
        var paramName = "testParam";
        object data = "validString";

        Guard.Is<string>(paramName, data, out var result);

        Assert.Equal("validString", result);
    }

    [Fact]
    public void NotInvalidFileExtension_ValidExtension_DoesNotThrow()
    {
        var path = "my_image.png";

        Guard.NotInvalidFileExtension(nameof(path), path, "png");
    }

    [Fact]
    public void NotInvalidFileExtension_InvalidExtension_ThrowsArgumentException()
    {
        var path = "model.txt";

        Assert.Throws<ArgumentException>(() => Guard.NotInvalidFileExtension(nameof(path), path, "jpg"));
    }

    [Fact]
    public void NotInvalidFileExtension_NullPath_ThrowsArgumentNullException()
    {
        string path = null!;

        Assert.Throws<ArgumentNullException>(() => Guard.NotInvalidFileExtension(nameof(path), path, "jpg"));
    }

    [Fact]
    public void NotInvalidFileExtension_EmptyExtension_ThrowsArgumentNullException()
    {
        var path = "model.onnx";

        Assert.Throws<ArgumentNullException>(() => Guard.NotInvalidFileExtension(nameof(path), path, ""));
    }

    [Fact]
    public void NotInvalidFileExtension_ExtensionCaseInsensitive_DoesNotThrow()
    {
        var path = "model.ONNX";

        Guard.NotInvalidFileExtension(nameof(path), path, "onnx");
    }
}