using System;
using System.Collections.Generic;

#nullable disable

namespace Forms.Models
{
    public partial class CyberAttack
    {
        public int CybetAttackId { get; set; }
        public int? UserId { get; set; }
        public int? AttackId { get; set; }
        public bool? SystemOutage { get; set; }
        public DateTime? DetectionDate { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }

        public virtual Attack Attack { get; set; }
        public virtual User User { get; set; }
    }
}
