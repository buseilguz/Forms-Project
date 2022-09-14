using System;
using System.Collections.Generic;

#nullable disable

namespace Forms.Models
{
    public partial class Form
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string FormUrl { get; set; }
        public int Sira { get; set; }
        public int? UstId { get; set; }
        public bool? Active { get; set; }
        public bool? Passive { get; set; }
    }
}
