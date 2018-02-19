using System;
using System.ComponentModel;

namespace SharpSeaUsers.Models
{
    public abstract class BaseEntity{
        
        public DateTime CreatedAt { get; set; }
    }
}