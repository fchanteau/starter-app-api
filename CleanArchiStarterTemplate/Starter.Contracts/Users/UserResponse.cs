namespace Starter.Contracts.Users;
public class UserResponse
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = "";

    private UserResponse() { }

    public UserResponse(Guid userId, string username)
    {
        UserId = userId;
        Username = username;
    }
}
