﻿using Newtonsoft.Json.Linq;
using PUBGAPIWrapper.Models;
using PUBGAPIWrapper.RestWrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PUBGAPIWrapper
{
    /// <summary>
    /// Service for making requests to the PUBG API.
    /// Wraps all provided endpoints.
    /// </summary>
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

        #region Helpers

        private Response MakeRequest(string queryString, bool compressResponse = false)
        {
            Request request = new Request(queryString);

            request.AddHeader("Authorization", "Bearer " + ApiKey);

            if (compressResponse)
                request.AddHeader("Accept-Encoding", "gzip");
            else
                request.AddHeader("Accept", "application/vnd.api+json");

            Response response = Client.Execute(request);

            return response;
        }

        /// <summary>
        /// Given a Shard enum, builds the shard portion of the Uri string for a request.
        /// </summary>
        /// <remarks>
        /// We must do this because C# doesn't allow enums with the '-' character,
        /// so we have to pull the uri-friendly string out of the description of the Shard value.s
        /// </remarks>
        private string BuildShardUri(Shard shard)
        {
            return "/shards/" + shard.GetDescription() + "/";
        }

        /// <summary>
        /// Writes the given string to the given filename in the Data folder.
        /// </summary>
        public void WriteResponse(string filename, string body)
        {
            File.WriteAllText("../../../Data/" + filename, body);
        }

        #endregion

        #region Status

        /// <summary>
        /// Gets the status of the PUBG Api.
        /// </summary>
        public Status GetStatus()
        {
            string statusUri = "/status";
            Response response = MakeRequest(statusUri);

            Status status = Status.Deserialize(response.Content);
            return status;
        }

        #endregion

        #region Matches

        /// <summary>
        /// Gets a PUBG match by ID.
        /// </summary>
        public Match GetMatch(Shard shard, string matchId)
        {
            string shardUri = BuildShardUri(shard);
            string matchUri = shardUri + "matches/" + matchId;
            Response response = MakeRequest(matchUri);

            Match match = Match.Deserialize(response.Content);
            return match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// TODO: Implement createdAt filter
        /// </remarks>
        public Sample GetSampleMatches(Shard shard, DateTime? createdAtFilter = null)
        {
            string shardUri = BuildShardUri(shard);
            string sampleUri = shardUri + "samples";
            Response response = MakeRequest(sampleUri);

            Sample sample = Sample.Deserialize(response.Content);
            return sample;
        }

        #endregion

        #region Players

        /// <summary>
        /// Given a players username, gets the Id associated for that account.
        /// </summary>
        public string GetPlayerId(Shard shard, string playerName)
        {
            string shardUri = BuildShardUri(shard);
            string playerUri = shardUri + "players?filter[playerNames]=" + playerName;
            Response response = MakeRequest(playerUri);

            JObject obj = JObject.Parse(response.Content);
            return (string)obj["data"][0]["id"];
        }

        /// <summary>
        /// Makes a request to the PUBG API for information about a player, by player id.
        /// </summary>
        public Player GetPlayer(Shard shard, string id)
        {
            string shardUri = BuildShardUri(shard);
            string playerUri = shardUri + "players/" + id;
            Response response = MakeRequest(playerUri);
            Player player = Player.Deserialize(response.Content);
            return player;
        }

        /// <summary>
        /// Given a list of player ids or player names, queries for those players
        /// </summary>
        /// <remarks>
        /// Cannot query by both names and ids. Prefers ids when provided.
        /// </remarks>
        public List<Player> GetPlayers(Shard shard, List<string> ids, List<string> names)
        {
            if ((ids == null || !ids.Any()) && (names == null || !names.Any()))
                return new List<Player>();

            string shardUri = BuildShardUri(shard);
            string playerUri = shardUri + "players";
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

            Response response = MakeRequest(playerUri);

            List<Player> players = Player.DeserializePlayerList(response.Content);
            return players;
        }

        public dynamic GetPlayerSeason(string id, string seasonId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Seasons

        /// <summary>
        /// Makes a request to the PUBG Api for all of the seasons.
        /// </summary>
        public List<Season> GetSeasons(Shard shard)
        {
            string shardUri = BuildShardUri(shard);
            string seasonUri = shardUri + "seasons";
            Response response = MakeRequest(seasonUri);
            List<Season> seasons = Season.Deserialize(response.Content);
            return seasons;
        }

        #endregion

        #region Telemetry

        /// <summary>
        /// Given a telemetry URL from a match object,
        /// Makes a request to the PUBG API for that telemetry object.
        /// </summary>
        public Telemetry GetTelemetry(string url)
        {
            Response response = MakeRequest(url);
            Telemetry telemetry = Telemetry.Deserialize(response.Content);
            return telemetry;
        }

        #endregion
    }
}
