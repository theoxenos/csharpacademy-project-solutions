using Microsoft.AspNetCore.Components.Forms;

namespace WardrobeInventory.Blazor.Services;

public class ImageService
{
    private const long MaxFileSize = 1024 * 1024 * 5; // 5MB

    public async Task<byte[]> ConvertImageToBytes(IBrowserFile file)
    {
        await using var stream = file.OpenReadStream(MaxFileSize);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public string GetImageUrl(byte[]? data)
    {
        if (data is not { Length: not 0 })
        {
            return "img/default.png";
        }

        string base64;
        try
        {
            base64 = Convert.ToBase64String(data);
        }
        catch (Exception)
        {
            return "img/default.png";
        }

        return $"data:image/png;base64,{base64}";
    }
}