using BowlingApi.Controllers;
using BowlingApi.Models;
using BowlingApi.Models.Requests;
using BowlingApi.Models.Response;
using BowlingApi.Services;
using BowlingApi.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace BowlingApi.Tests.ControllerTests
{
    public class GameControllerTest
    {
        private readonly IBowlingService _service;
        private readonly GameController _controller;

        public GameControllerTest()
        {
            _service = Substitute.For<IBowlingService>();
            _controller = new GameController(_service);
        }

        [Fact]
        public void StartGame_Success()
        {
            // Arrange
            var expectedResponse = GameMockData.StartGame_SuccessMockData();
            _service.InsertGame(Arg.Any<int>()).Returns(expectedResponse);

            //Act
            var response = _controller.StartGame(1);

            //Assert
            Assert.NotNull(response);
            var successReqResult = Assert.IsType<OkObjectResult>(response.Result);
            var apiResult = Assert.IsType<Game>(successReqResult.Value);

            Assert.NotNull(apiResult);

            Assert.Equal(1, apiResult.Id);

            Assert.Equal(1, apiResult.PlayerId);
        }

        [Fact]
        public void StartGame_DbException()
        {
            // Arrange
            _service.When(x => x.InsertGame(Arg.Any<int>())).Do(x => throw new Exception("DB Error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.StartGame(1));
        }

        [Fact]
        public void GetGameScores_Success()
        {
            // Arrange
            var expectedResponse = GameMockData.BowlingResponse_MockData();
            _service.GetScoresByGameId(Arg.Any<int>()).Returns(expectedResponse);

            //Act
            var response = _controller.GetGameScores(1);

            //Assert
            Assert.NotNull(response);
            var successReqResult = Assert.IsType<OkObjectResult>(response.Result);
            var apiResult = Assert.IsType<BowlingResponse>(successReqResult.Value);

            Assert.NotNull(apiResult);

            Assert.Equal(2, apiResult.Frames.Count);

            Assert.Equal(2, apiResult.Frames[0].Throws.Count);
        }

        [Fact]
        public void GetGameScores_DbException()
        {
            // Arrange
            _service.When(x => x.GetScoresByGameId(Arg.Any<int>())).Do(x => throw new Exception("DB Error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.GetGameScores(1));
        }

        [Fact]
        public void FrameThrowScore_Success()
        {
            // Arrange
            var expectedResponse = GameMockData.BowlingResponse_MockData();
            _service.InsertFrameScore(Arg.Any<FramethrowRequest>()).Returns(expectedResponse);

            var request = new FramethrowRequest
            {
                GameId = 1,
                FrameNum = 1,
                ThrowNum = 1,
                Score = 5
            };

            //Act
            var response = _controller.FrameThrowScore(request);

            //Assert
            Assert.NotNull(response);
            var successReqResult = Assert.IsType<OkObjectResult>(response.Result);
            var apiResult = Assert.IsType<BowlingResponse>(successReqResult.Value);

            Assert.NotNull(apiResult);

            Assert.Equal(2, apiResult.Frames.Count);

            Assert.Equal(2, apiResult.Frames[0].Throws.Count);
        }

        [Fact]
        public void FrameThrowScore_DbException()
        {
            // Arrange
            _service.When(x => x.InsertFrameScore(Arg.Any<FramethrowRequest>())).Do(x => throw new Exception("DB Error"));

            var request = new FramethrowRequest
            {
                GameId = 1,
                FrameNum = 1,
                ThrowNum = 1,
                Score = 5
            };

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.FrameThrowScore(request));
        }

        [Fact]
        public void FrameThrowScore_BadRequest()
        {
            // Arrange & Act
            var response = _controller.FrameThrowScore(null);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public void DeleteAllScores_Success()
        {
            // Arrange
            _service.DeleteGameScores(Arg.Any<int>()).Returns(true);

            //Act
            var response = _controller.DeleteAllScores(1);

            //Assert
            Assert.NotNull(response);
            var successReqResult = Assert.IsType<OkObjectResult>(response.Result);
            var apiResult = Assert.IsType<bool>(successReqResult.Value);;

            Assert.True(apiResult);
        }

        [Fact]
        public void DeleteAllScores_DbException()
        {
            // Arrange
            _service.When(x => x.DeleteGameScores(Arg.Any<int>())).Do(x => throw new Exception("DB Error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.DeleteAllScores(1));
        }
    }
}
