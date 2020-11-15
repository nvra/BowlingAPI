using BowlingApi.Models;
using BowlingApi.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingApi.Repositories
{
    public interface IBowlingDBRepository
    {
        public List<Player> GetPlayers();

        public List<Player> GetPlayersByName(string name);

        public Player GetPlayerById(int id);

        public int InsertPlayer(string name);

        public int InsertGame(int player_id);

        public Game GetGameById(int id);

        public bool IsGameIdValid(int gameId);

        public int InsertFrameScore(Indivdualscore score);

        public Frame CheckFrameScore(int gameId, int frameNum);

        public int InsertFrameScore(int gameId, int frameNum, int score);

        public bool IsThrowNumValid(int gameId, int frameNum);

        public bool IsThrowStrike(int gameId, int frameNum);

        public List<GetScoresByGameResponse> GetScoresByGameId(int gameId);

        public int UpdateTotalScore(Framescores request);

        public SpResponse UpdateStrikeOrSpareScores(int gameId, int scoreId);

        public int? IsScoreValid(int gameId, int frameNum);

        public bool IsSpareForFrame10(int gameId);

        public bool IsFrameNumValid(int gameId, int frameNum);

        public List<Framescores> GetAllScoresByGameId(int gameId);

        public SpResponse DeleteGameScores(int gameId);

        public Game GetGameByPlayer(int playerID);

        public Framescores GetPreviousFrame(int gameId, int frameNum);

        public bool IsPreviousFrameStrike(int? previousframeScoreId);

        public int GetTotalScoreByGameId(int gameId);

        public bool IsPreviousFrameStrikeOrSpare(int? previousframeScoreId, int thrwNum);
    }

    public class BowlingDBRepository : IBowlingDBRepository
    {
        private BowlingDBContext _context;
        public BowlingDBRepository(BowlingDBContext context)
        {
            _context = context;
        }

        public List<Player> GetPlayers()
        {
            return _context.Player.ToList();
        }

        public Player GetPlayerById(int id)
        {
            return _context.Player.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public int InsertPlayer(string name)
        {
            var player = new Player { Name = name };
            _context.Player.Add(player);
            _context.SaveChanges();

            return player.Id;
        }

        public int InsertGame(int player_id)
        {
            var game = new Game { PlayerId = player_id };
            _context.Game.Add(game);
            _context.SaveChanges();

            return game.Id;
        }

        public Game GetGameById(int id)
        {
            return _context.Game
                .Include("Player")
                .Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public bool IsGameIdValid(int gameId)
        {
            return _context.Game.Any(x => x.Id == gameId);
        }

        public int InsertFrameScore(Indivdualscore score)
        {
            _context.Indivdualscore.Add(score);
            _context.SaveChanges();

            return score.Id;
        }

        public Frame CheckFrameScore(int gameId, int frameNum)
        {
            return  _context.Framescores.Where(x => x.GameId == gameId && x.FrameNum == frameNum)
                .Select(x => new Frame { Id =  x.Id, TotalScore = Convert.ToInt32(x.TotalScore) }).FirstOrDefault();
        }

        public bool IsThrowNumValid(int gameId, int frameNum)
        {
            return _context.Framescores.Any(x => x.GameId == gameId && x.FrameNum == frameNum);
        }

        public bool IsFrameNumValid(int gameId, int frameNum)
        {
            return _context.Framescores.Any(x => x.GameId == gameId && x.FrameNum == frameNum - 1);
        }

        public bool IsThrowStrike(int gameId, int frameNum)
        {
            return _context.Framescores.Any(x => x.GameId == gameId && x.FrameNum == frameNum && x.TotalScore == 10);
        }

        public bool IsSpareForFrame10(int gameId)
        {
            return _context.Framescores.Where(x => x.GameId == gameId && x.FrameNum == 10).Any(x => x.TotalScore >= 10);
        }

        public int? IsScoreValid(int gameId, int frameNum)
        {
            return _context.Framescores.Where(x => x.GameId == gameId && x.FrameNum == frameNum).Select(x => x.TotalScore).FirstOrDefault();
        }

        public int InsertFrameScore(int gameId, int frameNum, int score)
        {
            var frame = new Framescores { GameId = gameId, FrameNum = frameNum, TotalScore = score };
            _context.Framescores.Add(frame);
            _context.SaveChanges();
            _context.Entry(frame).State = EntityState.Detached;

            return frame.Id;
        }

        public List<GetScoresByGameResponse> GetScoresByGameId(int gameId)
        {
            return _context.GetScoresByGameResponse.FromSqlInterpolated($"GetScoresByGame {gameId}").ToList();
        }

        public int UpdateTotalScore(Framescores request)
        {
            _context.Update(request);
            var returnValue = _context.SaveChanges();
            _context.Entry(request).State = EntityState.Detached;
            return returnValue;
        }

        public SpResponse UpdateStrikeOrSpareScores(int gameId, int scoreId)
        {
            return _context.SpResponse.FromSqlInterpolated($"UpdateStrikeSpareScores {scoreId}, {gameId}").ToList().FirstOrDefault();
        }

        public List<Player> GetPlayersByName(string name)
        {
            return _context.Player.Where(x => x.Name.Contains(name)).ToList();
        }

        public List<Framescores> GetAllScoresByGameId(int gameId)
        {
            return _context.Framescores.Include("Indivdualscore").Where(x => x.GameId == gameId).ToListAsync().Result;
        }

        public SpResponse DeleteGameScores(int gameId)
        {
            return _context.SpResponse.FromSqlInterpolated($"DeleteGameScoresById {gameId}").ToList().FirstOrDefault();
        }

        public Game GetGameByPlayer(int playerID)
        {
            return _context.Game.FromSqlInterpolated($"GetGameIdByPlayer {playerID}").ToList().FirstOrDefault();
        }

        public Framescores GetPreviousFrame(int gameId, int frameNum)
        {
            var response = _context.Framescores.Where(x => x.GameId == gameId && x.FrameNum == frameNum).Select(x => x).FirstOrDefault();
            return response;
        }

        public bool IsPreviousFrameStrike(int? previousframeScoreId)
        {
            return _context.Indivdualscore.Where(x => x.GameFrameId == previousframeScoreId && x.ThrowNum == 1 && x.IsStrike == true).Any();
        }

        public bool IsPreviousFrameStrikeOrSpare(int? previousframeScoreId, int thrwNum)
        {
            return _context.Indivdualscore.Where(x => x.GameFrameId == previousframeScoreId && (x.IsStrike == true || (x.IsSpare == true && thrwNum == 1))).Any();
        }

        public int GetTotalScoreByGameId(int gameId)
        {
            return _context.Framescores.Where(x => x.GameId == gameId).Sum(x => x.TotalScore) ?? 0;
        }
    }
}
