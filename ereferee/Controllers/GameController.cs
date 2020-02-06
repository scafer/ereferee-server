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
        public ActionResult<GameData> CreateGame([FromBody] GameData gameData)
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

                if(game.status == 0)
                {
                    gameService.StartGame(gameId);
                    return new SvcResult(0, "Success");
                }
            }

            return new SvcResult(1, "Error");
        }

        [HttpPost]
        [Authorize]
        [Route("finishGame")]
        public ActionResult<SvcResult> FinishGame(int gameId, int homeScore, int visitorScore)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (gameId != 0)
            {

            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("pendingGames")]
        public ActionResult<List<GameData>> PendingGames()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("pendingGameById")]
        public ActionResult<GameData> PendingGameById(int id)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("activeGames")]
        public ActionResult<List<GameData>> ActiveGames()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("activeGameById")]
        public ActionResult<GameData> ActiveGameById(int id)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("previousGames")]
        public ActionResult<List<GameData>> PreviousGames()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("previousGameById")]
        public ActionResult<GameData> PreviousGameById(int id)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [Route("registerEvent")]
        [Authorize]
        public ActionResult<SvcResult> RegisterEvent(EventType eventType, int gameId, int? athleteId, string description, string gameTime)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }

        [HttpDelete]
        [Authorize]
        [Route("deleteGame")]
        public ActionResult<SvcResult> DeleteGame(int id)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new NotFoundResult();
        }
    }
}