using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserImages.ImageStorage;
public interface ICloudinaryService
{
    Task<Uri> UploadImageAsync(byte[] imageData, string fileName);
}
