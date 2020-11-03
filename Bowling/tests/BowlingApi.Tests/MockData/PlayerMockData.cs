using BowlingApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingApi.Tests.MockData
{
    public class PlayerMockData
    {
        public static List<Player> GetPlayer_SuccessMockData()
        {
            return new List<Player>
            {
                new Player
                {
                    Id = 1,
                    Name = "AAAA"
                },
                new Player
                {
                    Id = 2,
                    Name = "BBBB"
                },
                new Player
                {
                    Id = 3,
                    Name = "CCCC"
                }
            };
        }


        public static Player InsertPlayer_SuccessMockData()
        {
            return new Player
            {
                Id = 1,
                Name = "AAAA"
            };
        }
    }
}
