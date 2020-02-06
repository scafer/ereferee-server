using ereferee.Models;
using System;
using System.Linq;

namespace ereferee.Services
{
    public class GameService : BaseService
    {
        DataContext db;

        public GameService()
        {
            db = new DataContext();
        }

        public Game CreateGame(Game game)
        {
            db.games.Add(game);
            db.SaveChanges();
            return game;
        }

        public UserGame AssociateUserToGame(int userId, int gameId, int role)
        {
            UserGame userGame = new UserGame { userId = userId, gameId = gameId, role = role };
            db.userGames.Add(userGame);
            db.SaveChanges();
            return userGame;
        }

        public Game GetGame(int gameId)
        {
            return db.games.FirstOrDefault(g => g.id == gameId);
        }

        public void StartGame(int gameId)
        {
            var game = GetGame(gameId);
            game.status = 1;

            db.games.Update(game);
            db.SaveChanges();
        }
    }
}
