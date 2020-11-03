using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BowlingApi.Models
{
    public partial class Game
    {
        public Game()
        {
            Framescores = new HashSet<Framescores>();
        }

        public int Id { get; set; }
        public int? PlayerId { get; set; }

        public virtual Player Player { get; set; }
        [JsonIgnore]
        public virtual ICollection<Framescores> Framescores { get; set; }
    }
}
