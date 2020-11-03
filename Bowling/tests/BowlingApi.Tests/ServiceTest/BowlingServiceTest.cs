using BowlingApi.Models;
using BowlingApi.Models.Requests;
using BowlingApi.Models.Response;
using BowlingApi.Repositories;
using BowlingApi.Services;
using BowlingApi.Tests.MockData;
using NSubstitute;
using System;
using Xunit;

namespace BowlingApi.Tests.ServiceTest
{
    public class BowlingServiceTest
    {
        private IBowlingDBRepository _repository;
        private IBowlingService _service;

        public BowlingServiceTest()
        {
            _repository = Substitute.For<IBowlingDBRepository>();
            _service = new BowlingService(_repository);
        }

        [Fact]
        public void InsertFrameScore_Success()
        {
            // Arrange
            var request = new FramethrowRequest
            {
                GameId = 1,
                FrameNum = 1,
                ThrowNum = 1,
                Score = 5
            };

            _repository.InsertFrameScore(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(1);
            _repository.InsertFrameScore(Arg.Any<Indivdualscore>()).Returns(1);
            _repository.UpdateStrikeOrSpareScores(Arg.Any<int>(), Arg.Any<int>()).Returns(GameMockData.SpResponse_MockData());
            _repository.GetScoresByGameId(Arg.Any<int>()).Returns(GameMockData.GetScoresByGameResponse_MockData());

            var expectedResponse = GameMockData.BowlingResponse_MockData();

            // Act
            var actualResponse = _service.InsertFrameScore(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<BowlingResponse>(actualResponse);

            Assert.Equal(expectedResponse.Frames.Count, actualResponse.Frames.Count);

            Assert.Equal(expectedResponse.Frames[0].Throws.Count, actualResponse.Frames[0].Throws.Count);
            Assert.Equal(expectedResponse.TotalScore, actualResponse.TotalScore);
            Assert.Equal(expectedResponse.PlayerName, actualResponse.PlayerName);
            Assert.Equal(expectedResponse.Frames[0].TotalScore, actualResponse.Frames[0].TotalScore);
            Assert.Equal(expectedResponse.Frames[1].TotalScore, actualResponse.Frames[1].TotalScore);
        }

        [Fact]
        public void InsertFrameScore_DbException()
        {
            // Arrange
            var request = new FramethrowRequest
            {
                GameId = 1,
                FrameNum = 1,
                ThrowNum = 1,
                Score = 5
            };

            _repository.When(x => x.InsertFrameScore(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>())).Do(x => throw new Exception("DB Error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _service.InsertFrameScore(request));
        }
    }
}
