namespace Chat.Models;

public class Message
{
   
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; }
    public string? UserName { get; set; }
    public DateTime? Created { get; set; }
    public string ChannelId { get; set; }
    public Channel? Channel { get; set; }
    public string UserId { get; set; }
    public User? User { get; set; }

}