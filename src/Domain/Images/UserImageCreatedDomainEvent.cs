using SharedKernel;

namespace Domain.Images;

public sealed record UserImageCreatedDomainEvent(Guid UserImageId) : IDomainEvent;
