using ereferee.Models;
using System.Collections.Generic;
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
        public void StartGame(Game game)
        {
            game.status = 1;
            db.games.Update(game);
            db.SaveChanges();
        }

        public void FinishGame(Game game)
        {
            game.status = 2;
            db.games.Update(game);
            db.SaveChanges();
        }

        public void DeleteGame(Game game)
        {
            game.status = -1;
            db.games.Update(game);
            db.SaveChanges();
        }

        public Game GetGame(int gameId)
        {
            return db.games.FirstOrDefault(g => g.id == gameId);
        }

        public UserGame AssociateUserToGame(int userId, int gameId, int role)
        {
            UserGame userGame = new UserGame { userId = userId, gameId = gameId, role = role };
            db.userGames.Add(userGame);
            db.SaveChanges();
            return userGame;
        }

        public List<Game> GetGames(int userId, int status)
        {
            List<Game> games = new List<Game>();
            var userGames = db.userGames.Where(g => g.userId == userId);
            foreach (UserGame usrgm in userGames)
            {
                using var db1 = new DataContext();
                var game = db1.games.Where(g => g.id == usrgm.gameId && g.status == status).FirstOrDefault();

                if(game != null)
                    games.Add(game);
            }

            return games;
        }

        public GameData GetGameDataById(int gameId, int userId)
        {
            GameData gameData = new GameData();

            if (db.userGames.Where(g => g.userId == userId && g.gameId == gameId) != null)
            {
                gameData.game = db.games.Where(g => g.id == gameId).FirstOrDefault();
                gameData.homeTeam = db.teams.Where(g => g.id == gameData.game.homeTeamId).FirstOrDefault();
                gameData.visitorTeam = db.teams.Where(g => g.id == gameData.game.visitorTeamId).FirstOrDefault();
                gameData.homeAthletes = db.athletes.Where(g => g.teamId == gameData.homeTeam.id).ToList<Athlete>();
                gameData.visitorAthletes = db.athletes.Where(g => g.teamId == gameData.visitorTeam.id).ToList<Athlete>();
                gameData.events = db.gameEvents.Where(g => g.gameId == gameData.game.id).ToList<GameEvent>();
            }

            return gameData;
        }

        public void RegisterEvent(GameEvent gameEvent)
        {
            db.gameEvents.Add(gameEvent);
            db.SaveChanges();
        }
    }
}