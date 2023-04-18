namespace Chat.Models;

public class User
{
    // public User(Guid id, string userName, DateTime created)
    // {
    //     Id = id;
    //     UserName = userName;
    //     Created = created;
    //     Channels = new List<Channel>();
    //     Messages = new List<Message>();
    // }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public DateTime Created { get; set; }
   public List<Channel>? Channels { get; set; }
    public List<Message>? Messages { get; set; }
}