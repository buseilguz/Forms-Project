using System;
using System.Collections.Generic;

#nullable disable

namespace Forms.Models
{
    public partial class Attack
    {
        public Attack()
        {
            CyberAttacks = new HashSet<CyberAttack>();
        }

        public int AttackId { get; set; }
        public string AttackName { get; set; }

        public virtual ICollection<CyberAttack> CyberAttacks { get; set; }
    }
}
