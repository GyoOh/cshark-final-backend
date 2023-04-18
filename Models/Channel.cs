using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    public class Channel
    {
        // public Channel(Guid id, string name, DateTime createdAt, string creatorId)
        // {
        //     Id = id;
        //     Name = name;
        //     Created = createdAt;
        //     CreatorId = creatorId;
        //     Users = new List<User>();
        //     Messages = new List<Message>();
        // }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "general";

        public DateTime Created { get; set; }

        [Display(Name = "Created By")]
        public string CreatorId { get; set; }

        [NotMapped]
        public User? Creator { get; set; }

        public List<User>? Users { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
