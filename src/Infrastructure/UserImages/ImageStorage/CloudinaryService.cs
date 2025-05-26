using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.UserImages.ImageStorage;
internal class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }
    public async Task<Uri> UploadImageAsync(byte[] imageData, string fileName)
    {
        using var stream = new MemoryStream(imageData);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = false,
            Folder = "user_images"
        };

        ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return new Uri(uploadResult.SecureUrl?.AbsoluteUri ?? throw new Exception("Upload failed"));

    }
}
