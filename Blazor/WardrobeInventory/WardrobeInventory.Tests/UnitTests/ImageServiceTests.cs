using Microsoft.AspNetCore.Components.Forms;
using Moq;
using WardrobeInventory.Blazor.Services;

namespace WardrobeInventory.Tests.UnitTests;

public class ImageServiceTests
{
    [Fact]
    public async Task ConvertImageToBytes_Should_Return_Correct_Bytes()
    {
        // Arrange
        var imageService = new ImageService();
        var expectedBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header bytes
        var stream = new MemoryStream(expectedBytes);

        var mockFile = new Mock<IBrowserFile>();
        mockFile.Setup(f => f.Size).Returns(expectedBytes.Length);
        mockFile.Setup(f => f.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns(stream);

        // Act
        var result = await imageService.ConvertImageToBytes(mockFile.Object);

        // Assert
        Assert.Equal(expectedBytes, result);
    }

    [Fact]
    public async Task ConvertImageToBytes_Should_Throw_When_File_Does_Not_Exist()
    {
        // Arrange
        var imageService = new ImageService();
        byte[] result = null!;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () =>
        {
            result = await imageService.ConvertImageToBytes(null!);
        });

        Assert.Null(result);
    }

    [Fact]
    public void GetImageUrl_Should_Return_Correct_Url()
    {
        // Arrange
        var imageService = new ImageService();
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header bytes
        var expectedUrl = $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";

        // Act
        var result = imageService.GetImageUrl(imageBytes);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUrl, result);
    }

    [Fact]
    public void GetImageUrl_Should_Return_Default_When_File_Does_Not_Exist()
    {
        // Arrange
        var imageService = new ImageService();

        // Act
        var result = imageService.GetImageUrl(null!);

        // Assert
        Assert.Equal("img/default.png", result);
    }
}