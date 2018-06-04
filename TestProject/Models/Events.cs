﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestProject.Models
{
    public abstract class Event
    {
        [JsonProperty("_V")]
        public int Version { get; set; }

        [JsonProperty("_D")]
        public string Timestamp { get; set; }

        [JsonProperty("_T")]
        public string Type { get; set; }

        public Common Common { get; set; }
    }
    
    public class LogArmorDestroy : Event
    {
        public int AttackId { get; set; }
        public Character Attacker { get; set; }
        public Character Victim { get; set; }
        public string DamageTypeCategory { get; set; }
        public DamageReason DamageReason { get; set; }
        public string DamageCauserName { get; set; }
        public Item Item { get; set; }
        public float Distance { get; set; }
    }

    public class LogCarePackageLand : Event
    {
        public ItemPackage ItemPackage { get; set; }
    }

    public class LogCarePackageSpawn : Event
    {
        public ItemPackage ItemPackage { get; set; }
    }

    public class LogGameStatePeriodic : Event
    {
        public GameState GameState { get; set; }
    }

    public class LogItemAttach : Event
    {
        public Character Character { get; set; }
        public Item ParentItem { get; set; }
        public Item ChildItem { get; set; }
    }

    public class LogItemDetach : Event
    {
        public Character Character { get; set; }
        public Item ParentItem { get; set; }
        public Item ChildItem { get; set; }
    }

    public class LogItemDrop : Event
    {
        public Character Character { get; set; }
        public Item Item { get; set; }
    }

    public class LogItemEquip : Event
    {
        public Character Character { get; set; }
        public Item Item { get; set; }
    }

    public class LogItemPickup : Event
    {
        public Character Character { get; set; }
        public Item Item { get; set; }
    }

    public class LogItemUnequip : Event
    {
        public Character Character { get; set; }
        public Item Item { get; set; }
    }

    public class LogItemUse : Event
    {
        public Character Character { get; set; }
        public Item Item { get; set; }
    }

    public class LogMatchDefinition : Event
    {
        public string MatchId { get; set; }
    }

    public class LogMatchEnd : Event
    {
        public IEnumerable<Character> Characters { get; set; }
    }

    public class LogMatchStart : Event
    {
        public IEnumerable<Character> Characters { get; set; }
    }

    public class LogPlayerAttack : Event
    {
        public int AttackId { get; set; }
        public Character Attacker { get; set; }
        public AttackType? AttackType { get; set; }
        public Item Weapon { get; set; }
        public Vehicle Vehicle { get; set; }
    }

    public class LogPlayerCreate : Event
    {
        public Character Character { get; set; }
        
        public override string ToString()
        {
            return "Type: " + this.Type + "\n"
                + "Version: " + this.Version + "\n"
                + "Timestamp: " + this.Timestamp + "\n"
                + Character.ToString();
        }
    }

    public class LogPlayerKill : Event
    {
        public int AttackId { get; set; }
        public Character Killer { get; set; }
        public Character Victim { get; set; }
        public string DamageTypeCategory { get; set; }
        public string DamageCauserName { get; set; }
        public float Distance { get; set; }
    }

    public class LogPlayerLogin : Event
    {
        public bool Result { get; set; }
        public string ErrorMessge { get; set; }
        public string AccountId { get; set; }
    }

    public class LogPlayerLogout : Event
    {
        public string AccountId { get; set; }
    }

    public class LogPlayerPosition : Event
    {
        public Character Character { get; set; }
        public float ElapsedTime { get; set; }
        public int NumAlivePlayers { get; set; }

        public override string ToString()
        {
            string toString = "ElapsedTime: " + this.ElapsedTime + "\n"
                + "NumAlivePlayers: " + this.NumAlivePlayers + "\n"
                + "Type: " + this.Type + "\n"
                + "Version: " + this.Version + "\n"
                + "Timestamp: " + this.Timestamp + "\n"
                + Character.ToString();

            return toString;
        }
    }

    public class LogPlayerTakeDamage : Event
    {
        public int AttackId { get; set; }
        public Character Attacker { get; set; }
        public Character Victim { get; set; }
        public string DamageTypeCategory { get; set; }
        public DamageReason DamageReason { get; set; }
        public float Damage { get; set; }
        public string DamageCauserName { get; set; }
    }

    public class LogSwimEnd : Event
    {
        public Character Character { get; set; }
    }

    public class LogSwimStart : Event
    {
        public Character Character { get; set; }
    }

    public class LogVehicleDestroy : Event
    {
        public int AttackId { get; set; }
        public Character Attacker { get; set; }
        public Vehicle Vehicle { get; set; }
        public string DamageTypeCategory { get; set; }
        public string DamageCauserName { get; set; }
        public float Distance { get; set; }
    }

    public class LogVehicleLeave : Event
    {
        public Character Character { get; set; }
        public Vehicle Vehicle { get; set; }
        public float RideDistance { get; set; }
        public int SeatIndex { get; set; }
    }

    public class LogVehicleRide : Event
    {
        public Character Character { get; set; }
        public Vehicle Vehicle { get; set; }
        public int SeatIndex { get; set; }
    }
}
