using BowlingApi.Models;
using BowlingApi.Models.Requests;
using BowlingApi.Repositories;
using BowlingApi.Services;
using BowlingApi.Tests.RepositoryTest;
using Xunit;

namespace BowlingApi.Tests.GameTest
{
    public class GameIntegrationTest
    {
        private IBowlingDBRepository _repository;
        private IBowlingService _service;
        private BowlingDBContext _dbContext;

        public GameIntegrationTest()
        {
            _dbContext = new InMemoryDbContextFactory().GetBowlingDbContext();
            _repository = new BowlingDBRepository(_dbContext);
            _service = new BowlingService(_repository);
        }

        [Fact]
        public void GameTest_All_Tens()
        {
            for(var frame = 1; frame < 11; frame++)
            {
                var thrw = 0;
                do
                {
                    thrw++;
                    var request = new FramethrowRequest
                    {
                        GameId = 1,
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = 10
                    };

                    var response = _service.InsertScores(request);

                    Assert.True(response);
                } while (frame == 10 && thrw <= 3);
            }

            var total = _repository.GetTotalScoreByGameId(1);
            Assert.Equal(300, total);
        }

        [Fact]
        public void GameTest_All_Zeros()
        {
            for (var frame = 1; frame < 11; frame++)
            {                
                for(var thrw = 1; thrw < 3; thrw++)
                {
                    var request = new FramethrowRequest
                    {
                        GameId = 1,
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = 0
                    };

                    var response = _service.InsertScores(request);

                    Assert.True(response);
                } 
            }

            var total = _repository.GetTotalScoreByGameId(1);
            Assert.Equal(0, total);
        }

        [Fact]
        public void GameTest_All_Ones()
        {
            for (var frame = 1; frame < 11; frame++)
            {
                for (var thrw = 1; thrw < 3; thrw++)
                {
                    var request = new FramethrowRequest
                    {
                        GameId = 1,
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = 1
                    };

                    var response = _service.InsertScores(request);

                    Assert.True(response);
                }
            }

            var total = _repository.GetTotalScoreByGameId(1);
            Assert.Equal(20, total);
        }

        [Fact]
        public void GameTest_All_Nine_One()
        {
            for (var frame = 1; frame < 11; frame++)
            {
                for (var thrw = 1; thrw < 4; thrw++)
                {
                    var request = new FramethrowRequest
                    {
                        GameId = 1,
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = thrw == 1 ? 9 : 1
                    };

                    var response = _service.InsertScores(request);

                    Assert.True(response);
                    if (frame < 10 && thrw == 2)
                        break;
                }
            }

            var total = _repository.GetTotalScoreByGameId(1);
            Assert.Equal(182, total);
        }

        [Fact]
        public void GameTest_All_One_Nine()
        {
            for (var frame = 1; frame < 11; frame++)
            {
                for (var thrw = 1; thrw < 4; thrw++)
                {
                    var request = new FramethrowRequest
                    {
                        GameId = 1,
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = thrw == 1 ? 1 : 9
                    };

                    var response = _service.InsertScores(request);

                    Assert.True(response);
                    if (frame < 10 && thrw == 2)
                        break;
                }
            }

            var total = _repository.GetTotalScoreByGameId(1);
            Assert.Equal(118, total);
        }

        [Fact]
        public void GameTest_All_Fives()
        {
            for (var frame = 1; frame < 11; frame++)
            {
                for (var thrw = 1; thrw < 4; thrw++)
                {
                    var request = new FramethrowRequest
                    {
                        GameId = 1,
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = 5
                    };

                    var response = _service.InsertScores(request);

                    Assert.True(response);
                    if (frame < 10 && thrw == 2)
                        break;
                }
            }

            var total = _repository.GetTotalScoreByGameId(1);
            Assert.Equal(150, total);
        }
    }
}
