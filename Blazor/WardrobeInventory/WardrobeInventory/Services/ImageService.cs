using Microsoft.AspNetCore.Components.Forms;

namespace WardrobeInventory.Services;

public class ImageService
{
    public void ConvertBytesToBase64(byte[] imageData)
    {
        
    }

    public async Task UploadImage(IBrowserFile file)
    {
        var newFile = await file.RequestImageFileAsync("image/png", 500, 500);
        
    }
}