using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Starter.Application.Features.Users.Dto;
using Starter.Application.Interfaces;

namespace Starter.Application.Features.Users.Queries;
public record GetUsersQuery : IRequest<ErrorOr<GetUsersResponse>>;

public record GetUsersResponse(List<UserDto> Users);

public class GetUsersQueryHandler(ITracingService tracingService, ILogger<GetUsersQueryHandler> logger) : IRequestHandler<GetUsersQuery, ErrorOr<GetUsersResponse>>
{
    public Task<ErrorOr<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        using var activity = tracingService.StartActivity("FetchUsers");

        logger.LogInformation("Fetching users...");

        Thread.Sleep(2000);

        var users = new List<UserDto>
        {
            new(Guid.NewGuid(), "user1"),
            new(Guid.NewGuid(), "user2"),
        };

        logger.LogInformation("Fetched {UserCount} users", users.Count);

        return Task.FromResult(new GetUsersResponse(users).ToErrorOr());
    }
}
