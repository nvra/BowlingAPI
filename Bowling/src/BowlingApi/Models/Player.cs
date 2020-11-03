using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BowlingApi.Models
{
    public partial class Player
    {
        public Player()
        {
            Game = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Game> Game { get; set; }
    }
}
