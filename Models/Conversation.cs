using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpSeaUsers.Models
{
    public class Conversation : BaseEntity
    {
        [Key]
        public int ConversationId { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Message> Messages { get; set; }

        public Conversation() {
            Messages = new List<Message>();
        }
    }
}