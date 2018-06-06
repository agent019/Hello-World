﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestProject.Models;
using TestProject.RestWrapper;

namespace TestProject
{
    public class RequestService
    {
        private const string BaseUri = "https://api.playbattlegrounds.com/";

        private string ApiKey { get; set; }
        private Client Client { get; set; }

        #region Constructors

        public RequestService(string key)
        {
            Client = new Client(BaseUri);
            this.ApiKey = key;
        }

        #endregion

        private Response MakeRequest(string queryString, bool requestCompressed = false)
        {
            Request request = new Request(queryString);

            if (requestCompressed)
            {
                throw new NotImplementedException(); // and needs support for unzipping
            }

            request.AddHeader("Authorization", ApiKey);
            request.AddHeader("Accept", "application/vnd.api+json");

            return Client.Execute(request);
        }

        #region Status

        public Status GetStatus()
        {
            string statusUri = "/status";
            Response response = MakeRequest(statusUri);

            Status status = Status.DeserializeStatus(response.Content);
            return status;
        }

        #endregion

        #region Matches

        public Match GetMatch(string matchId)
        {
            string matchUri = "/shards/pc-na/matches/" + matchId;
            Response response = MakeRequest(matchUri);

            Match match = Match.DeserializeMatch(response.Content);
            return match;
        }

        public List<Match> GetSampleMatches(DateTime createdAtFilter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Players

        /// <summary>
        /// Given a players username, gets the Id associated for that account.
        /// </summary>
        public string GetPlayerId(string playerName)
        {
            string playerUri = "shards/pc-na/players?filter[playerNames]=" + playerName;
            Response response = MakeRequest(playerUri);

            JObject obj = JObject.Parse(response.Content);
            return (string)obj["data"][0]["id"];
        }

        /// <summary>
        /// Makes a request to the pubg API for information about a player, by player id.
        /// </summary>
        public Player GetPlayer(string id)
        {
            string playerUri = "shards/pc-na/players/" + id;
            Response response = MakeRequest(playerUri);

            Player player = Player.DeserializePlayer(response.Content);
            return player;
        }

        /// <summary>
        /// Given a list of player ids or player names, queries for those players
        /// </summary>
        /// <remarks>
        /// Cannot query by both names and ids. Prefers ids when provided.
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// Thrown when no arguments are provided
        /// </exception>
        public List<Player> GetPlayers(List<string> ids, List<string> names)
        {
            string playerUri = "shards/pc-na/players";
            if (ids != null && ids.Any())
            {
                string concatenatedIds = string.Join(",", ids);
                playerUri = playerUri + "filters[playerIds]" + concatenatedIds;
            }
            else if (names != null && names.Any())
            {
                string concatenatedNames = string.Join(",", names);
                playerUri = playerUri + "filters[playerIds]" + concatenatedNames;
            }
            else throw new ArgumentException("ids and names cannot be empty"); // should probably short circuit instead of throwing

            Response response = MakeRequest(playerUri);

            List<Player> players = Player.DeserializePlayerList(response.Content);
            return players;
        }

        public dynamic GetPlayerSeason(string id, string seasonId)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Season> GetSeasons()
        {
            string seasonUri = "/seasons";
            Response response = MakeRequest(seasonUri);

            List<Season> seasons = Season.DeserializeSeason(response.Content);
            return seasons;
        }

        public Telemetry GetTelemetry(string url)
        {
            Response response = MakeRequest(url);
            Telemetry telemetry = Telemetry.DeserializeTelemetry(response.Content);
            return telemetry;
        }

        public void WriteResponse(string filename, string body)
        {
            File.WriteAllText("../../../Data/" + filename, body);
        }
    }
}
