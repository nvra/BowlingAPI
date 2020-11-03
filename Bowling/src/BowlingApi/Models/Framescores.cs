using System;
using System.Collections.Generic;

namespace BowlingApi.Models
{
    public partial class Framescores
    {
        public Framescores()
        {
            Indivdualscore = new HashSet<Indivdualscore>();
        }

        public int Id { get; set; }
        public int? GameId { get; set; }
        public int? FrameNum { get; set; }
        public int? TotalScore { get; set; }

        public virtual Game Game { get; set; }
        public virtual ICollection<Indivdualscore> Indivdualscore { get; set; }
    }
}
