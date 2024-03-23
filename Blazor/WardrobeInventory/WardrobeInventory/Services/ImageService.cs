using System.IO.Pipelines;
using Microsoft.AspNetCore.Components.Forms;

namespace WardrobeInventory.Services;

public class ImageService
{
    public async Task<byte[]> ConvertImageToBytes(IBrowserFile file)
    {
        var buffer = new byte[file.Size];
        await file.OpenReadStream().ReadAsync(buffer);
        return buffer;
    }

    public string GetImageUrl(byte[] data)
    {
        return $"data:image/png;base64,{Convert.ToBase64String(data)}";
    }
}