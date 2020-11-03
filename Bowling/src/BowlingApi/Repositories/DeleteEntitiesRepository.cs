using BowlingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApi.Repositories
{
    public interface IDeleteEntitiesRepository
    {
        public bool DeleteGame(Game game);

        public bool DeleteFrameScores(List<Framescores> request);

        public bool DeleteIndividualScores(List<Indivdualscore> request);
    }
    public class DeleteEntitiesRepository : IDeleteEntitiesRepository
    {
        private BowlingDBContext _context;
        public DeleteEntitiesRepository(BowlingDBContext context)
        {
            _context = context;
        }

        public bool DeleteIndividualScores(List<Indivdualscore> request)
        {
            _context.Indivdualscore.RemoveRange(request);
            _context.SaveChanges();

            return true;
        }

        public bool DeleteFrameScores(List<Framescores> request)
        {
            _context.Framescores.RemoveRange(request);
            _context.SaveChangesAsync();

            return true;
        }

        public bool DeleteGame(Game game)
        {
            _context.Game.Remove(game);
            _context.SaveChanges();

            return true;
        }
    }
}
