using System;
using System.Collections.Generic;

#nullable disable

namespace Forms.Models
{
    public partial class Purpose
    {
        public Purpose()
        {
            ContentProviders = new HashSet<ContentProvider>();
        }

        public int PurposeId { get; set; }
        public string PurposeName { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ContentProvider> ContentProviders { get; set; }
    }
}
