using Application.Images.AddImage;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.UserImages;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/ImageUrl", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UploadUserImageCommand
            {
                UserId = request.UserId,
                ImageUrl = request.ImageUrl,
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Images)
        .RequireAuthorization();
    }
}
