using Application.Images.ImageUpload;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.UserImages;

internal sealed class UploadImageData : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public byte[]? ImageData { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/FileUpload", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UploadUserImageCommand
            {
                UserId = request.UserId,
                ImageData = request.ImageData,
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Images)
        .RequireAuthorization();
    }
}
