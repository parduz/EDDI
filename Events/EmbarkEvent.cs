﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using EddiDataDefinitions;

namespace EddiEvents
{
    public class EmbarkEvent : Event
    {
        public const string NAME = "Embark";
        public const string DESCRIPTION = "Triggered when you transition from on foot to a ship or SRV";
        public const string SAMPLE = "{ \"timestamp\":\"2020-10-12T09:06:17Z\", \"event\":\"Embark\", \"SRV\":false, \"Taxi\":false, \"Multicrew\":false, \"ID\":36 }";
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static EmbarkEvent()
        {
            VARIABLES.Add("tomulticrew", "True if embarking to another player's ship");
            VARIABLES.Add("toship", "True if embarking to your own ship");
            VARIABLES.Add("tosrv", "True if embarking to an SRV");
            VARIABLES.Add("totaxi", "True if embarking to a taxi transport ship");
            VARIABLES.Add("systemname", "The name of the star system in which the commander is embarking");
            VARIABLES.Add("bodyname", "The name of the body from which the commander is embarking (if any)");
            VARIABLES.Add("station", "The name of the station from which the commander is embarking (if any)");
            VARIABLES.Add("stationtype", "The type of station from which the commander is embarking (if any)");
            VARIABLES.Add("onstation", "True if embarking from a station"); 
            VARIABLES.Add("onplanet", "True if embarking from a planet"); 
        }

        [PublicAPI]
        public string systemname { get; }

        [PublicAPI]
        public string bodyname { get; private set; }

        [PublicAPI]
        public string station { get; private set; }

        [PublicAPI]
        public string stationtype => (stationModel ?? StationModel.None).localizedName;
        
        [PublicAPI]
        public bool tomulticrew { get; }

        [PublicAPI]
        public bool toship => toLocalId != null && !tosrv && !totaxi && !tomulticrew;

        [PublicAPI]
        public bool tosrv { get; }

        [PublicAPI]
        public bool totaxi { get; }

        [PublicAPI]
        public bool? onstation { get; }

        [PublicAPI]
        public bool? onplanet { get; }

        // Not intended to be user facing
        public int? toLocalId { get; }

        public long systemAddress { get;}

        public int? bodyId { get; }

        public long? marketId { get; }

        public StationModel stationModel { get; }

        public EmbarkEvent(DateTime timestamp, bool toSRV, bool toTaxi, bool toMultiCrew, int? toLocalId, string system, long systemAddress, string body, int? bodyId, bool? onStation, bool? onPlanet, string station, long? marketId, StationModel stationModel) : base(timestamp, NAME)
        {
            this.tosrv = toSRV;
            this.totaxi = toTaxi;
            this.tomulticrew = toMultiCrew;
            this.toLocalId = toLocalId;
            this.systemname = system;
            this.systemAddress = systemAddress;
            this.bodyname = body;
            this.bodyId = bodyId;
            this.onstation = onStation;
            this.onplanet = onPlanet;
            this.station = station;
            this.marketId = marketId;
            this.stationModel = stationModel;
        }
    }
}
