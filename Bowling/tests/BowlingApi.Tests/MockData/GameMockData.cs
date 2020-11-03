using BowlingApi.Models;
using BowlingApi.Models.Response;
using System.Collections.Generic;

namespace BowlingApi.Tests.MockData
{
    public class GameMockData
    {
        public static Game StartGame_SuccessMockData()
        {
            return new Game
            {
                Id = 1,
                PlayerId = 1
            };
        }

        public static List<Game> Game_MockData()
        {
            return new List<Game>
            {
                new Game
                {
                    Id = 1,
                    PlayerId = 1
                },
                new Game
                {
                    Id = 2,
                    PlayerId = 2
                }
            };
        }

        public static List<Framescores> Framescores_MockData()
        {
            return new List<Framescores>
            {
                new Framescores
                {
                    Id = 1,
                    GameId = 1,
                    FrameNum = 4,
                    TotalScore = 10
                },
                new Framescores
                {
                    Id = 2,
                    GameId = 1,
                    FrameNum = 5,
                    TotalScore = 7
                },
                new Framescores
                {
                    Id = 5,
                    GameId = 1,
                    FrameNum = 10,
                    TotalScore = 5
                },
                new Framescores
                {
                    Id = 3,
                    GameId = 2,
                    FrameNum = 10,
                    TotalScore = 10
                }
            };
        }

        public static SpResponse SpResponse_MockData()
        {
            return new SpResponse
            {
                RetValue = 1
            };
        }

        public static BowlingResponse BowlingResponse_MockData()
        {
            return new BowlingResponse
            {
                PlayerName = "AAAA",
                GameId = 1,
                TotalScore = 18,
                Frames = new List<Frame>
                {
                    new Frame
                    {
                         FrameNum = 1,
                          TotalScore = 8,
                          Throws = new List<Throw>()
                          {
                              new Throw
                              {
                                   Score = "4",
                                   ThrowNum = 1
                              },
                              new Throw
                              {
                                   Score = "4",
                                   ThrowNum = 2
                              }
                          }
                    },
                    new Frame
                    {
                         FrameNum = 2,
                         TotalScore = 10,
                         Throws = new List<Throw>()
                          {
                              new Throw
                              {
                                   Score = "5",
                                   ThrowNum = 1
                              },
                              new Throw
                              {
                                   Score = "5",
                                   ThrowNum = 2
                              }
                         }
                    }
                }
            };
        }

        public static List<GetScoresByGameResponse> GetScoresByGameResponse_MockData()
        {
            return new List<GetScoresByGameResponse>
            {
                new GetScoresByGameResponse
                {
                    FrameNum = 1,
                    GameId = 1,
                    PlayerName = "AAAA",
                    Score = "4",
                    TotalScore = 8,
                    ThrowNum = 1
                },
                new GetScoresByGameResponse
                {
                    FrameNum = 1,
                    GameId = 1,
                    PlayerName = "AAAA",
                    Score = "4",
                    TotalScore = 8,
                    ThrowNum = 2
                },
                new GetScoresByGameResponse
                {
                    FrameNum = 2,
                    GameId = 1,
                    PlayerName = "AAAA",
                    Score = "5",
                    TotalScore = 10,
                    ThrowNum = 1
                },
                new GetScoresByGameResponse
                {
                    FrameNum = 2,
                    GameId = 1,
                    PlayerName = "AAAA",
                    Score = "5",
                    TotalScore = 10,
                    ThrowNum = 2
                }
            };
        }

        public static Frame Frame_MockData()
        {
            return new Frame
            {
                Id = 1,
                TotalScore = 5
            };
        }
    }
}
