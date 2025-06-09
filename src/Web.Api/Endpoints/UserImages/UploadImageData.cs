using Application.Images.ImageUpload;
using Domain.Users;
using Infrastructure.UserImages.ImageStorage;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Endpoints.Users;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace Web.Api.Endpoints.UserImages;

internal sealed class UploadImageFile : IEndpoint
{

    public sealed class UploadImageRequest
    {
        [FromForm]
        public Guid UserId { get; set; }

        [FromForm]
        public IFormFile Image { get; set; } = null!;
    }
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/upload", async ([FromForm] UploadImageRequest request,
            ISender sender,
             ICloudinaryService cloudinaryService,
            CancellationToken cancellationToken) =>
        {
            using var memoryStream = new MemoryStream();
            await request.Image.CopyToAsync(memoryStream, cancellationToken);

            byte[] imageBytes = memoryStream.ToArray();

            Uri cloudinaryUrl = await cloudinaryService.UploadImageAsync(imageBytes, request.Image.FileName);


            var command = new UploadUserImageCommand
            {
                UserId = request.UserId,
                ImageData = memoryStream.ToArray(),
                ImageUrl = cloudinaryUrl
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(
        id => Results.Ok(new
        {
            Id = id,
            ImageUrl = cloudinaryUrl
        }),
        CustomResults.Problem);
        })
         .DisableAntiforgery()
        .Accepts<UploadImageRequest>("multipart/form-data")
        .WithTags(Tags.Images)
        .RequireAuthorization();
    }
}
