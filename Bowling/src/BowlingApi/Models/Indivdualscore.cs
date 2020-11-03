using System;
using System.Collections.Generic;

namespace BowlingApi.Models
{
    public partial class Indivdualscore
    {
        public int Id { get; set; }
        public int GameFrameId { get; set; }
        public int ThrowNum { get; set; }
        public int? Score { get; set; }
        public bool? IsStrike { get; set; }
        public bool? IsSpare { get; set; }
        public bool? IsFoul { get; set; }

        public virtual Framescores GameFrame { get; set; }
    }
}
