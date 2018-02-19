using System;
using System.ComponentModel;

namespace UserDashboard.Models
{
    public abstract class BaseEntity{
        
        public DateTime CreatedAt { get; set; }
    }
}