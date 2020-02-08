using ereferee.Models;
using ereferee.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ereferee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [Route("createGame")]
        [DisableRequestSizeLimit]
        public ActionResult<GameData> CreateGame([FromBody] Game game)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }
            return new NotFoundResult();
        }

        [HttpPost]
        [Authorize]
        [Route("createGameWithTeams")]
        [DisableRequestSizeLimit]
        public ActionResult<GameData> CreateGameWithTeams([FromBody] GameData gameData)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (gameData.game != null && gameData.homeTeam != null && gameData.visitorTeam != null &&
                gameData.homeAthletes.Count != 0 && gameData.visitorAthletes.Count != 0)
            {
                using var gameService = new GameService();
                using var teamService = new TeamService();

                //Create Teams
                var homeTeam = teamService.CreateTeam(gameData.homeTeam);
                var visitorTeam = teamService.CreateTeam(gameData.visitorTeam);

                if (homeTeam == null || visitorTeam == null)
                    return new NoContentResult();

                gameData.homeTeam.id = gameData.game.homeTeamId = homeTeam.id;
                gameData.visitorTeam.id = gameData.game.visitorTeamId = visitorTeam.id;

                //Create Game
                gameData.game.userId = user.id;
                var game = gameService.CreateGame(gameData.game);

                if (game == null)
                    return new NoContentResult();

                gameData.game.id = game.id;

                //Create Athletes
                var homeAthletes = teamService.CreateAthletes(gameData.homeAthletes, gameData.game.id, gameData.homeTeam.id, AthleteType.Player);
                var visitorAthletes = teamService.CreateAthletes(gameData.visitorAthletes, gameData.game.id, gameData.visitorTeam.id, AthleteType.Player);

                if (homeAthletes == null || visitorAthletes == null)
                    return new NoContentResult();

                gameData.homeAthletes = homeAthletes;
                gameData.visitorAthletes = visitorAthletes;

                gameService.AssociateUserToGame(user.id, game.id, 1);

                return gameData;
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [Authorize]
        [Route("startGame")]
        public ActionResult<SvcResult> StartGame(int gameId)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (gameId != 0)
            {
                using var gameService = new GameService();
                var game = gameService.GetGame(gameId);

                if (game.status == 0)
                {
                    gameService.StartGame(game);
                    return new SvcResult(0, "Success");
                }
            }

            return new SvcResult(1, "Error");
        }

        [HttpPost]
        [Authorize]
        [Route("finishGame")]
        public ActionResult<SvcResult> FinishGame(Game gm)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (gm.id != 0)
            {
                using var gameService = new GameService();
                var game = gameService.GetGame(gm.id);
                game.homeScore = gm.homeScore;
                game.visitorScore = gm.visitorScore;

                if (game.status == 1)
                {
                    gameService.FinishGame(game);
                    return new SvcResult(0, "Success");
                }
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("pendingGames")]
        public ActionResult<List<Game>> PendingGames()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var gameService = new GameService();
            return gameService.GetGames(user.id, 0);
        }

        [HttpGet]
        [Authorize]
        [Route("activeGames")]
        public ActionResult<List<Game>> ActiveGames()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var gameService = new GameService();
            return gameService.GetGames(user.id, 1);
        }

        [HttpGet]
        [Authorize]
        [Route("previousGames")]
        public ActionResult<List<Game>> PreviousGames()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var gameService = new GameService();
            return gameService.GetGames(user.id, 2);
        }

        [HttpGet]
        [Authorize]
        [Route("gameDataById")]
        public ActionResult<GameData> GameDataById(int gameId)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var gameService = new GameService();
            return gameService.GetGameDataById(gameId, user.id);
        }

        [HttpPost]
        [Route("registerEvent")]
        [Authorize]
        public ActionResult<SvcResult> RegisterEvent(GameEvent gameEvent)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var gameService = new GameService();
            gameService.RegisterEvent(gameEvent);

            return SvcResult.Get(0, "Success");
        }

        [HttpDelete]
        [Authorize]
        [Route("deleteGame")]
        public ActionResult<SvcResult> DeleteGame(int gameId)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var gameService = new GameService();
            var game = gameService.GetGame(gameId);
            gameService.DeleteGame(game);
            return SvcResult.Get(0, "Success");
        }
    }
}