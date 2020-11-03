using BowlingApi.Models;
using BowlingApi.Models.Response;
using BowlingApi.Repositories;
using BowlingApi.Tests.MockData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BowlingApi.Tests.RepositoryTest
{
    public class BowlingDBRepositoryTest
    {
        private IBowlingDBRepository _repository;
        private BowlingDBContext _dbContext;

        public BowlingDBRepositoryTest()
        {
            _dbContext = new InMemoryDbContextFactory().GetBowlingDbContext();
            _repository = new BowlingDBRepository(_dbContext);
        }

        [Fact]
        public void GetPlayers_Success()
        {
            //Arrange
            _dbContext.AddRange(PlayerMockData.GetPlayer_SuccessMockData());
            _dbContext.SaveChangesAsync();

            //Act
            var players = _repository.GetPlayers();

            //Assert
            Assert.NotNull(players);
            Assert.IsType<List<Player>>(players);
            Assert.Equal(3, players.Count);
            Assert.Equal(1, players[0].Id);
            Assert.Equal("BBBB", players[1].Name);
        }

        [Fact]
        public void GetPlayerById_Success()
        {
            //Arrange
            _dbContext.AddRange(PlayerMockData.GetPlayer_SuccessMockData());
            _dbContext.SaveChangesAsync();

            //Act
            var playerExists = _repository.GetPlayerById(1);
            var playerNotExists = _repository.GetPlayerById(6);

            //Assert
            Assert.NotNull(playerExists);
            Assert.IsType<Player>(playerExists);
            Assert.Equal(1, playerExists.Id);

            Assert.Null(playerNotExists);
        }

        [Fact]
        public void InsertPlayer_Success()
        {
            // Arrange
            var name = "AAAA";

            // Act
            var id = _repository.InsertPlayer(name);

            //Assert
            Assert.True(id > 0);
            Assert.Equal(1, id);
        }

        [Fact]
        public void InsertGame_Success()
        {
            // Arrange
            var playerId = 1;

            // Act 
            var id = _repository.InsertGame(playerId);

            //Assert
            Assert.True(id > 0);
            Assert.Equal(1, id);
        }

        [Fact]
        public void IsGameIdValid_Should_Return_True_IfExists()
        {
            // Arrange
            _dbContext.Game.Add(new Game { Id = 1, PlayerId = 1 });
            _dbContext.SaveChangesAsync();

            var gameId = 1;

            // Act
            var isExists = _repository.IsGameIdValid(gameId);

            // Assert 
            Assert.True(isExists);
        }

        [Fact]
        public void IsGameIdValid_Should_Return_False_IfNotExists()
        {
            // Arrange
            _dbContext.Game.AddRange(GameMockData.StartGame_SuccessMockData());
            _dbContext.SaveChangesAsync();
            var gameId = 3;

            // Act
            var isExists = _repository.IsGameIdValid(gameId);

            // Assert 
            Assert.False(isExists);
        }

        [Fact]
        public void GetGameById_Success()
        {
            //Arrange
            _dbContext.AddRangeAsync(GameMockData.Game_MockData());
            _dbContext.SaveChangesAsync();

            //Act
            var idExists = _repository.GetGameById(1);
            var idNotExists = _repository.GetGameById(6);

            //Assert
            Assert.NotNull(idExists);
            Assert.IsType<Game>(idExists);
            Assert.Equal(1, idExists.Id);

            Assert.Null(idNotExists);
        }

        [Fact]
        public void InsertFrameScore_Success()
        {
            //Arrange
            _dbContext.Framescores.Add(new Framescores { GameId = 1, FrameNum = 1, TotalScore = 0 });
            _dbContext.SaveChanges();

            var score = new Indivdualscore
            {
                GameFrameId = 1,
                ThrowNum = 1,
                Score = 7,
                IsStrike = false,
                IsSpare = false,
                IsFoul = false
            };

            //Act
            var id = _repository.InsertFrameScore(score);
            var newscore = _dbContext.Indivdualscore.Where(x => x.Id == 0 && x.GameFrameId == 1).Select(x => x).FirstOrDefault();

            //Assert
            Assert.Equal(0, id);
            Assert.NotNull(newscore);
            Assert.Equal(7, newscore.Score);
            Assert.Equal(1, newscore.ThrowNum);
        }

        [Fact]
        public void CheckFrameScore_Success()
        {
            // Arrange
            _dbContext.Framescores.AddRangeAsync(GameMockData.Framescores_MockData());
            _dbContext.SaveChangesAsync();

            // Act
            var frame_Exists = _repository.CheckFrameScore(1, 4);
            var frame_notExists = _repository.CheckFrameScore(1, 1);

            //Assert
            Assert.Null(frame_notExists);

            Assert.NotNull(frame_Exists);
            Assert.IsType<Frame>(frame_Exists);
            Assert.Equal(1, frame_Exists.Id);
            Assert.Equal(10, frame_Exists.TotalScore);
        }

        [Fact]
        public void IsThrowNumValid_Success()
        {
            // Arrange
            _dbContext.Framescores.AddRangeAsync(GameMockData.Framescores_MockData());
            _dbContext.SaveChangesAsync();

            // Act
            var exists = _repository.IsThrowNumValid(1, 4);
            var notExists = _repository.IsThrowNumValid(1, 1);

            //Asset
            Assert.True(exists);
            Assert.False(notExists);
        }

        [Fact]
        public void IsFrameNumValid_Success()
        {
            // Arrange
            _dbContext.Framescores.AddRangeAsync(GameMockData.Framescores_MockData());
            _dbContext.SaveChangesAsync();

            // Act
            var exists = _repository.IsFrameNumValid(1, 5);
            var notExists = _repository.IsFrameNumValid(1, 4);

            //Asset
            Assert.True(exists);
            Assert.False(notExists);
        }

        [Fact]
        public void IsThrowStrike_Success()
        {
            // Arrange
            _dbContext.Framescores.AddRangeAsync(GameMockData.Framescores_MockData());
            _dbContext.SaveChangesAsync();

            // Act
            var strike = _repository.IsThrowStrike(1, 4);
            var notStrike = _repository.IsThrowStrike(1, 5);

            //Asset
            Assert.True(strike);
            Assert.False(notStrike);
        }

        [Fact]
        public void IsSpareForFrame10_Success()
        {
            // Arrange
            _dbContext.Framescores.AddRangeAsync(GameMockData.Framescores_MockData());
            _dbContext.SaveChangesAsync();

            // Act
            var spare = _repository.IsSpareForFrame10(2);
            var notSpare = _repository.IsSpareForFrame10(1);

            //Asset
            Assert.True(spare);
            Assert.False(notSpare);
        }

        [Fact]
        public void IsScoreValid_Success()
        {
            // Arrange
            _dbContext.Framescores.AddRangeAsync(GameMockData.Framescores_MockData());
            _dbContext.SaveChangesAsync();

            // Act
            var valid = _repository.IsScoreValid(1, 4);
            var invalid = _repository.IsScoreValid(1, 1);

            //Assert
            Assert.Null(invalid);

            Assert.NotNull(valid);
            Assert.IsType<int>(valid);
            Assert.Equal(10, valid);
        }
    }
}
