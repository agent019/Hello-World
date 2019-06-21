﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO: Verify the Match objects deserialize correctly
namespace PUBGAPIWrapper.Models
{
    /// <summary>
    /// Represents a PUBG match.
    /// Match objects contain information about a completed match such as the 
    /// game mode played, duration, and which players participated.
    /// </summary>
    /// <remarks>
    /// Flattened representation of the JSON provided by the API.
    /// </remarks>
    public class Match
    {
        /// <summary>
        /// Match ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Time this match object was stored in the API.
        /// </summary>
        public DateTime MatchCompletion { get; set; }

        /// <summary>
        /// Length of the match measured in seconds.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Game mode played.
        /// </summary>
        public string GameMode { get; set; }

        /// <summary>
        /// Map name.
        /// </summary>
        public string Map { get; set; }

        /// <summary>
        /// True if this match is a custom match.
        /// </summary>
        public bool IsCustomMatch { get; set; }

        public string PatchVersion { get; set; }

        /// <summary>
        /// The state of the season.
        /// </summary>
        public string SeasonState { get; set; }

        /// <summary>
        /// Platform shard.
        /// </summary>
        public string Shard { get; set; }

        /// <summary>
        /// Identifies the studio and game.
        /// </summary>
        public string Title { get; set; }

        public List<string> RosterIds { get; set; }
        /*public List<Participant> Participants { get; set; }
        public List<Roster> Rosters { get; set; }
        public MatchTelemetry Telemetry { get; set; }*/

        public override string ToString()
        {
            string matchString = "Id: " + Id + "\n";
            matchString += "Region: " + Shard + "\n";
            matchString += "Duration: " + Duration + "\n";
            matchString += "Match completion: " + MatchCompletion.ToString() + "\n";
            matchString += "Map: " + Map + "\n";
            matchString += "Mode: " + GameMode + "\n";

            return matchString;
        }

        public static Match Deserialize(string matchJson)
        {
            MatchDTO dto = JsonConvert.DeserializeObject<MatchDTO>(matchJson);
            Match match = new Match();
            /*{
                Id = dto.Data.Id,
                MatchCompletion = dto.Data.Attributes.Created,
                Duration = dto.Data.Attributes.Duration,
                GameMode = dto.Data.Attributes.GameMode,
                Map = dto.Data.Attributes.MapName,
                Shard = dto.Data.Attributes.Shard,
                Title = dto.Data.Attributes.Title,
                RosterIds = dto.Data.Relationships.Rosters.Data.Select(x => x.Id).ToList()
            };

            foreach (dynamic obj in dto.Included)
            {
                switch ((string)obj.type)
                {
                    case "roster":
                        Roster r = new Roster()
                        {
                            Id = obj.id,
                            Rank = obj.attributes.stats.rank,
                            TeamId = obj.attributes.stats.teamId,
                            Won = obj.attributes.won,
                            //PlayerIds = obj.relationships.participants.data.Select(x => x.id)
                        };
                        match.Rosters.Add(r);
                        break;
                    case "participant":
                        Participant p = new Participant()
                        {
                            Id = obj.id,
                            Stats = new PlayerStats()
                            {
                                DBNOs = obj.attributes.stats.DBNOs,
                                Assists = obj.attributes.stats.assists,
                                Boosts = obj.attributes.stats.boosts,
                                DamageDealt = obj.attributes.stats.damageDealt,
                                DeathType = obj.attributes.stats.deathType,
                                HeadshotKills = obj.attributes.stats.headshotKills,
                                Heals = obj.attributes.stats.heals,
                                KillPlace = obj.attributes.stats.killPlace,
                                KillPoints = obj.attributes.stats.killPoints,
                                KillPointsDelta = obj.attributes.stats.killPointsDelta,
                                Kills = obj.attributes.stats.killStreaks,
                                KillStreaks = obj.attributes.stats.kills,
                                LastKillPoints = obj.attributes.stats.lastKillPoints,
                                LastWinPoints = obj.attributes.stats.lastWinPoints,
                                LongestKill = obj.attributes.stats.longestKill,
                                MostDamage = obj.attributes.stats.mostDamage,
                                Name = obj.attributes.stats.name,
                                PlayerId = obj.attributes.stats.playerId,
                                Revives = obj.attributes.stats.revives,
                                RideDistance = obj.attributes.stats.rideDistance,
                                RoadKills = obj.attributes.stats.roadKills,
                                TeamKills = obj.attributes.stats.teamKills,
                                TimeSurvived = obj.attributes.stats.timeSurvived,
                                VehicleDestroys = obj.attributes.stats.vehicleDestroys,
                                WalkDistance = obj.attributes.stats.walkDistance,
                                WeaponsAcquired = obj.attributes.stats.weaponsAcquired,
                                WinPlace = obj.attributes.stats.winPlace,
                                WinPoints = obj.attributes.stats.winPoints,
                                WinPointsDelta = obj.attributes.stats.winPointsDelta,

                            }
                        };
                        match.Participants.Add(p);
                        break;
                    case "asset":
                        MatchAsset t = new MatchAsset()
                        {
                            Id = obj.id,
                            URL = obj.attributes.URL,
                            Description = obj.attributes.description,
                            Created = DateTime.Parse((string)obj.attributes.createdAt),
                            Name = obj.attributes.name
                        };
                        match.Telemetry = t;
                        break;
                    default:
                        throw new NotImplementedException("match.included contained object of type " + obj.type);
                }
            }*/

            return match;
        }
    }

    #region DTO

    public class MatchDTO
    {
        public MatchData Data { get; set; }
        //TODO: Better deserialization then dynamic and second pass
        public List<dynamic> Included { get; set; }
        public Links Links { get; set; }
        public Meta Meta { get; set; }
    }

    public class MatchData {
        public string Type { get; set; }
        public string Id { get; set; }
        public MatchAttributes Attributes { get; set; }
        public MatchRelationships Relationships { get; set; }
        public Links Links { get; set; }
    }

    public class MatchAttributes
    {
        [JsonProperty("createdAt")]
        public DateTime Created { get; set; }
        public long Duration { get; set; }
        public string GameMode { get; set; }
        public string MapName { get; set; }
        public bool IsCustomMatch { get; set; }
        public string SeasonState { get; set; }
        [JsonProperty("shardId")]
        public string Shard { get; set; }
        // Dynamic because api returns null in all cases, for now
        public dynamic Stats { get; set; }
        // Dynamic because api returns null in all cases, for now
        public dynamic Tags { get; set; }
        [JsonProperty("titleId")]
        public string Title { get; set; }
    }

    public class MatchRelationships {
        public MultiRelationship Assets { get; set; }
        public MultiRelationship Rosters { get; set; }
        public MultiRelationship Rounds { get; set; }
        public MultiRelationship Spectator { get; set; }
    }

    public class MatchParticipant
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public ParticipantAttributes Attributes { get; set; }
    }

    public class ParticipantAttributes
    {
        [JsonProperty("ShardId")]
        public string Shard { get; set; }
        public string Actor { get; set; }
        public ParticipantStats Stats { get; set; }
    }

    // TODO: some of these are probably not ints
    public class ParticipantStats
    {
        public int DBNOs { get; set; }
        public int Assists { get; set; }
        public int Boosts { get; set; }
        public decimal DamageDealt { get; set; }
        public string DeathType { get; set; }
        public int HeadshotKills { get; set; }
        public int Heals { get; set; }
        public int KillPlace { get; set; }
        public int KillStreaks { get; set; }
        public int Kills { get; set; }
        public long LongestKill { get; set; }
        public string Name { get; set; }
        public string PlayerId { get; set; }
        public int Revives { get; set; }
        public decimal RideDistance { get; set; }
        public int RoadKills { get; set; }
        public int SwimDistance { get; set; }
        public int TeamKills { get; set; }
        public long TimeSurvived { get; set; }
        public int VehicleDestroys { get; set; }
        public decimal WalkDistance { get; set; }
        public int WeaponsAcquired { get; set; }
        public int WinPlace { get; set; }
    }

    public class MatchRoster
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public RosterAttributes Attributes { get; set; }
        public RosterRelationships Relationships { get; set; }

        public int Rank { get; set; }
        public int TeamId { get; set; }
        public List<string> PlayerIds { get; set; }
    }

    public class RosterAttributes
    {
        public bool Won { get; set; }
        [JsonProperty("ShardId")]
        public string Shard { get; set; }
        public RosterStats Stats { get; set; }
    }

    public class RosterStats
    {
        public int Rank { get; set; }
        public int TeamId { get; set; }
    }

    public class RosterRelationships
    {
        public RosterTeam Team { get; set; }
        public MultiRelationship Participants { get; set; }
    }

    public class RosterTeam
    {
        public dynamic Data { get; set; }
    }
    
    public class MatchAsset
    {
        /// <summary>
        /// ID for this asset
        /// </summary>
        public string Id { get; set; }
        public string Type { get; set; }
        public AssetAttributes Attributes { get; set; }
    }

    public class AssetAttributes
    {
        /// <summary>
        /// URL to an endpoint containing all the telemetry data for a match.
        /// </summary>
        public string URL { get; set; }
        [JsonProperty("createdAt")]
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    #endregion
}
