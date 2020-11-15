using BowlingApi.Models;
using BowlingApi.Repositories;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BowlingApi.Tests.RepositoryTest
{
    public class BowlingRepositoryTests_Moq
    {
        public BowlingRepositoryTests_Moq()
        {
        }

        [Fact]
        public void GetPreviousFrame_Success()
        {
            // Arrange
            var frameScores = FrameScoresMockData();
            var context = new Mock<BowlingDBContext>();
            context.Setup(x => x.Framescores).ReturnsDbSet(frameScores);

            var repository = new BowlingDBRepository(context.Object);

            // Act
            var responseNotNull = repository.GetPreviousFrame(1, 1);
            var responseShouldBeNull = repository.GetPreviousFrame(1, 0);

            // Assert
            Assert.NotNull(responseNotNull);
            Assert.IsType<Framescores>(responseNotNull);
            Assert.Equal(1, responseNotNull.Id);
            Assert.Equal(10, responseNotNull.TotalScore);

            Assert.Null(responseShouldBeNull);
        }

        [Fact]
        public void GetTotalScoreByGameId_Success()
        {
            // Arrange
            var frameScores = FrameScoresMockData();
            var context = new Mock<BowlingDBContext>();
            context.Setup(x => x.Framescores).ReturnsDbSet(frameScores);

            var repository = new BowlingDBRepository(context.Object);
            var expected = frameScores.Sum(x => x.TotalScore);

            // Act
            var response = repository.GetTotalScoreByGameId(1);

            // Assert
            Assert.IsType<int>(response);
            Assert.Equal(expected, response);
        }

        [Fact]
        public void IsPreviousFrameStrike_Success()
        {
            // Arrange
            var individualScores = IndividualScoresMockData();
            var context = new Mock<BowlingDBContext>();
            context.Setup(x => x.Indivdualscore).ReturnsDbSet(individualScores);

            var repository = new BowlingDBRepository(context.Object);

            // Act
            var responseStrike = repository.IsPreviousFrameStrike(1);
            var responseNotStrike = repository.IsPreviousFrameStrike(2);
            var responseNull = repository.IsPreviousFrameStrike(7);

            // Assert
            Assert.True(responseStrike);
            Assert.False(responseNotStrike);
            Assert.False(responseNull);
        }

        [Fact]
        public void IsPreviousFrameStrikeOrSpare()
        {
            // Arrange
            var individualScores = IndividualScoresMockData();
            var context = new Mock<BowlingDBContext>();
            context.Setup(x => x.Indivdualscore).ReturnsDbSet(individualScores);

            var repository = new BowlingDBRepository(context.Object);

            // Act
            var responseStrike = repository.IsPreviousFrameStrikeOrSpare(1, 1);
            var responseSpare = repository.IsPreviousFrameStrikeOrSpare(2, 1);
            var responseFalse = repository.IsPreviousFrameStrikeOrSpare(2, 2);

            // Assert
            Assert.True(responseStrike);
            Assert.True(responseSpare);
            Assert.False(responseFalse);
        }

        private IList<Framescores> FrameScoresMockData()
        {
            return new List<Framescores>
            {
                new Framescores
                {
                    Id = 1,
                    GameId = 1,
                    FrameNum = 1,
                    TotalScore = 10
                },
                new Framescores
                {
                    Id = 2,
                    GameId = 1,
                    FrameNum = 2,
                    TotalScore = 10
                },
                new Framescores
                {
                    Id = 3,
                    GameId = 1,
                    FrameNum = 3,
                    TotalScore = 10
                }
            };
        }

        private IList<Indivdualscore> IndividualScoresMockData()
        {
            return new List<Indivdualscore>
            {
                new Indivdualscore
                {
                    Id = 1,
                    GameFrameId = 1,
                    ThrowNum = 1,
                    Score = 10,
                    IsStrike = true
                },
                new Indivdualscore
                {
                    Id = 2,
                    GameFrameId = 2,
                    ThrowNum = 1,
                    Score =  3
                },
                new Indivdualscore
                {
                    Id = 3,
                    GameFrameId = 2,
                    ThrowNum = 2,
                    Score = 7,
                    IsSpare = true
                },
                new Indivdualscore
                {
                    Id = 4,
                    GameFrameId = 3,
                    ThrowNum = 1,
                    Score = 2
                },
                new Indivdualscore
                {
                    Id = 5,
                    GameFrameId = 3,
                    ThrowNum = 2,
                    Score = 2
                }
            };
        }
    }
}
