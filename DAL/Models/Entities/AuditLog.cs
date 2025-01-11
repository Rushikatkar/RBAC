using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entities
{
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }

        public int? UserId { get; set; } // Nullable for unauthenticated users
        public string Action { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public string Details { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
