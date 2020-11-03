using BowlingApi.Controllers;
using BowlingApi.Models;
using BowlingApi.Services;
using BowlingApi.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace BowlingApi.Tests
{
    public class PlayerControllerTest
    {
        private readonly IBowlingService _service;
        private readonly PlayerController _controller;

        public PlayerControllerTest()
        {
            _service = Substitute.For<IBowlingService>();
            _controller = new PlayerController(_service);
        }

        [Fact]
        public void GetPlayers_Success()
        {
            // Arrange
            var expectedResponse = PlayerMockData.GetPlayer_SuccessMockData();
            _service.GetPlayers().Returns(expectedResponse);

            //Act
            var response = _controller.GetPlayers();

            //Assert
            Assert.NotNull(response);
            var successReqResult = Assert.IsType<OkObjectResult>(response.Result);
            var apiResult = Assert.IsType<List<Player>>(successReqResult.Value);

            Assert.NotNull(apiResult);
            Assert.Equal(3, apiResult.Count);

            Assert.Equal(1, apiResult[0].Id);
            Assert.Equal(2, apiResult[1].Id);
            Assert.Equal(3, apiResult[2].Id);

            Assert.Equal("AAAA", apiResult[0].Name);
            Assert.Equal("BBBB", apiResult[1].Name);
            Assert.Equal("CCCC", apiResult[2].Name);
        }

        [Fact]
        public void GetPlayers_DbException()
        {
            // Arrange
            _service.When(x => x.GetPlayers()).Do(x => throw new Exception("DB Error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.GetPlayers());
        }

        [Fact]
        public void InsertPlayer_Success()
        {
            // Arrange
            var expectedResponse = PlayerMockData.InsertPlayer_SuccessMockData();
            _service.InsertPlayer(Arg.Any<string>()).Returns(expectedResponse);

            //Act
            var response = _controller.InsertPlayer("AAAA");

            //Assert
            Assert.NotNull(response);
            var successReqResult = Assert.IsType<OkObjectResult>(response.Result);
            var apiResult = Assert.IsType<Player>(successReqResult.Value);

            Assert.NotNull(apiResult);

            Assert.Equal(1, apiResult.Id);

            Assert.Equal("AAAA", apiResult.Name);
        }

        [Fact]
        public void InsertPlayer_DbException()
        {
            // Arrange
            _service.When(x => x.InsertPlayer(Arg.Any<string>())).Do(x => throw new Exception("DB Error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.InsertPlayer("AAAA"));
        }
    }
}
