using BowlingApi.Models;
using BowlingApi.Models.Requests;
using BowlingApi.Models.Response;
using BowlingApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingApi.Services
{
    public interface IBowlingService
    {
        public List<Player> GetPlayers();

        public Player InsertPlayer(string name);

        public Game InsertGame(int playerId);

        public bool IsGameIdValid(int gameId);

        public bool IsThrowNumValid(int gameId, int frameNum);

        public BowlingResponse InsertFrameScore(FramethrowRequest request);

        public bool IsThrowStrike(int gameId, int frameNum);

        public BowlingResponse GetScoresByGameId(int gameId);

        public int IsScoreValid(int gameId, int frameNum);

        public bool IsSpareForFrame10(int gameId);

        public bool IsFrameNumValid(int gameId, int frameNum);

        public List<Player> GetPlayersByName(string name);

        //public bool DeleteAllScoresByGameId(int gameId);

        public bool DeleteGameScores(int gameId);

        public int? GetGameByPlayer(int playerID);
    }

    public class BowlingService : IBowlingService
    {
        private IBowlingDBRepository _repository;
        public BowlingService(IBowlingDBRepository repository)
        {
            _repository = repository;
        }

        public List<Player> GetPlayers()
        {
            return _repository.GetPlayers();
        }

        public Player InsertPlayer(string name)
        {
            var id = _repository.InsertPlayer(name);
            //_repository.InsertGame(id);

            return _repository.GetPlayerById(id);
        }

        public Game InsertGame(int playerId)
        {
            var id = _repository.InsertGame(playerId);

            return _repository.GetGameById(id);
        }

        public bool IsGameIdValid(int gameId)
        {
            return _repository.IsGameIdValid(gameId);
        }

        public BowlingResponse InsertFrameScore(FramethrowRequest request)
        {
            int gameFrameId;
            int score = 0;
            request.Score = request.IsFoul ? 0 : request.Score;

            if (request.ThrowNum == 1)
            {
                gameFrameId = _repository.InsertFrameScore(request.GameId, request.FrameNum, request.Score);
            }
            else
            {
                var gameFrame = _repository.CheckFrameScore(request.GameId, request.FrameNum);
                gameFrameId = gameFrame.Id;
                score = gameFrame.TotalScore;
            }

            var individualScore = new Indivdualscore
            {
                GameFrameId = gameFrameId,
                ThrowNum = request.ThrowNum,
                Score = request.Score,
                IsFoul = request.IsFoul,
                IsStrike = request.IsStrike,
                IsSpare = (!request.IsFoul && request.ThrowNum == 2 && (score + request.Score == 10))
            };

            var individualScoreId = _repository.InsertFrameScore(individualScore);

            var framescores = new Framescores
            {
                Id = gameFrameId,
                GameId = request.GameId,
                FrameNum = request.FrameNum,
                TotalScore = score + request.Score
            };

            if (request.ThrowNum == 2 || request.ThrowNum == 3)
            {
                _repository.UpdateTotalScore(framescores);
            }

            _repository.UpdateStrikeOrSpareScores(request.GameId, individualScoreId);

            return GetScoresByGameId(request.GameId);
        }

        public bool IsThrowNumValid(int gameId, int frameNum)
        {
            return _repository.IsThrowNumValid(gameId, frameNum);
        }

        public bool IsThrowStrike(int gameId, int frameNum)
        {
            return _repository.IsThrowStrike(gameId, frameNum);
        }

        public int IsScoreValid(int gameId, int frameNum)
        {
            var score = _repository.IsScoreValid(gameId, frameNum);

            return Convert.ToInt32(score);
        }

        public BowlingResponse GetScoresByGameId(int gameId)
        {
            var result = _repository.GetScoresByGameId(gameId);

            var frames = result.GroupBy(x => (x.FrameNum, x.TotalScore)).Select(f => new Frame
            {
                FrameNum = f.Key.FrameNum,
                TotalScore = f.Key.TotalScore,
                Throws = result.Where(t => t.FrameNum == f.Key.FrameNum).Select(t => new Throw
                { 
                    ThrowNum = t.ThrowNum,
                    Score = t.Score
                }).ToList()
            }).ToList();

            var response = new BowlingResponse
            {
                PlayerName = result.Select(x => x.PlayerName).FirstOrDefault(),
                GameId = result.Select(x => x.GameId).FirstOrDefault(),
                Frames = frames.OrderBy(x => x.FrameNum).ToList(),
                TotalScore = frames.Sum(x => x.TotalScore)
            };

            return response;
        }

        public bool IsSpareForFrame10(int gameId)
        {
            return _repository.IsSpareForFrame10(gameId);
        }

        public bool IsFrameNumValid(int gameId, int frameNum)
        {
            return _repository.IsFrameNumValid(gameId, frameNum);
        }

        public List<Player> GetPlayersByName(string name)
        {
            return _repository.GetPlayersByName(name);
        }

        //public bool DeleteAllScoresByGameId(int gameId)
        //{
        //    var response = _repository.GetAllScoresByGameId(gameId);

        //    var indivdualscores = response.SelectMany(x => x.Indivdualscore).ToList();

        //    var framescores = response
        //        .Select(x => new Framescores { GameId = x.GameId, Id = x.Id }).ToList();

        //    var game = new Game { Id = gameId };

        //    _deleteRepository.DeleteIndividualScores(indivdualscores);
        //    _deleteRepository.DeleteFrameScores(framescores);
        //    _deleteRepository.DeleteGame(game);

        //    return true;
        //}

        public bool DeleteGameScores(int gameId)
        {
            _repository.DeleteGameScores(gameId);

            return true;
        }

        public int? GetGameByPlayer(int playerID)
        {
            return _repository.GetGameByPlayer(playerID)?.Id;
        }
    }
}
