using System;
using System.Collections.Generic;

#nullable disable

namespace Forms.Models
{
    public partial class User
    {
        public User()
        {
            ContentProviders = new HashSet<ContentProvider>();
            CyberAttacks = new HashSet<CyberAttack>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserMobilePhone { get; set; }
        public string UserTask { get; set; }
        public string UserPassword { get; set; }
        public bool? Active { get; set; }
        public bool? Passive { get; set; }
        public bool? Authority { get; set; }

        public virtual ICollection<ContentProvider> ContentProviders { get; set; }
        public virtual ICollection<CyberAttack> CyberAttacks { get; set; }
    }
}
