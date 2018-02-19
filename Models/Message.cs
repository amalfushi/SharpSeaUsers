using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpSeaUsers.Models
{
    public class Message : BaseEntity
    {
        [Key]
        public int MessageId { get; set; }
        public String Text { get; set; }
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}