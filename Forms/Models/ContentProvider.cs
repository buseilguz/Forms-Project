using System;
using System.Collections.Generic;

#nullable disable

namespace Forms.Models
{
    public partial class ContentProvider
    {
        public int ContentProviderId { get; set; }
        public int? UserId { get; set; }
        public string DomainName { get; set; }
        public bool? DatabaseRequest { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public DateTime? Date { get; set; }
        public int? PurposeId { get; set; }
        public int AuthorizedId { get; set; }

        public virtual Purpose Purpose { get; set; }
        public virtual User User { get; set; }
    }
}
