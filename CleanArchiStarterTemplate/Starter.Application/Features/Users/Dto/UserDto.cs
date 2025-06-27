namespace Starter.Application.Features.Users.Dto;
public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = "";

    public UserDto(Guid userId, string username)
    {
        UserId = userId;
        Username = username;
    }
}
