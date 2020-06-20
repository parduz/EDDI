﻿using EddiDataDefinitions;
using EddiDataProviderService;
using EddiSpeechService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using EddiEvents;
using EddiJournalMonitor;
using Tests.Properties;
using System.Linq;

namespace UnitTests
{
    public class MockVAProxy
    {
        public List<string> vaLog = new List<string>();
        public Dictionary<string, object> vaVars = new Dictionary<string, object>();

        public void WriteToLog(string msg, string color = null)
        {
            vaLog.Add(msg);
        }

        public void SetText(string varName, string value)
        {
            vaVars.Add(varName, value);
        }

        public void SetInt(string varName, int? value)
        {
            vaVars.Add(varName, value);
        }

        public void SetBoolean(string varName, bool? value)
        {
            vaVars.Add(varName, value);
        }

        public void SetDecimal(string varName, decimal? value)
        {
            vaVars.Add(varName, value);
        }

        public void SetDate(string varName, DateTime? value)
        {
            vaVars.Add(varName, value);
        }
    }

    [TestClass]
    public class VoiceAttackPluginTests : TestBase
    {
        [TestInitialize]
        public void start()
        {
            MakeSafe();
        }

        private MockVAProxy vaProxy = new MockVAProxy();

        [TestMethod]
        public void TestOutfittingCosts()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            Dictionary<string, short?> shortIntValues = new Dictionary<string, short?>();
            Dictionary<string, string> textValues = new Dictionary<string, string>();
            Dictionary<string, int?> intValues = new Dictionary<string, int?>();
            Dictionary<string, decimal?> decimalValues = new Dictionary<string, decimal?>();
            Dictionary<string, bool?> booleanValues = new Dictionary<string, bool?>();
            Dictionary<string, DateTime?> dateTimeValues = new Dictionary<string, DateTime?>();
            Dictionary<string, object> extendedValues = new Dictionary<string, object>();
            //            VoiceAttackPlugin.VA_Init1(ref state, ref shortIntValues, ref textValues, ref intValues, ref decimalValues, ref booleanValues, ref dateTimeValues, ref extendedValues);
        }

        [TestMethod]
        public void TestSqlRepositoryPresent()
        {
            StarSystemRepository starSystemRepository = StarSystemSqLiteRepository.Instance;
            StarSystem DBData = starSystemRepository.GetOrFetchStarSystem("Sol", true);
            Assert.IsNotNull(DBData);
            Assert.AreEqual("Sol", DBData.systemname);
        }

        [TestMethod]
        public void TestSqlRepositoryMissing()
        {
            StarSystemRepository starSystemRepository = StarSystemSqLiteRepository.Instance;
            StarSystem DBData = starSystemRepository.GetStarSystem("Not here");
            Assert.IsNull(DBData);
        }


        [TestMethod]
        public void TestVolcanismConversion()
        {
            // Fetch a star system with various types of volcanism
            StarSystemRepository starSystemRepository = StarSystemSqLiteRepository.Instance;
            StarSystem sol = starSystemRepository.GetOrFetchStarSystem("Sol", true);
            Assert.IsNotNull(sol);

            // Ariel has no volcanism
            Body ariel = sol.bodies.Find(b => b.bodyname == "Ariel");
            Assert.IsNotNull(ariel);
            Assert.IsNull(ariel.volcanism);

            // Europa has water magma
            Body europa = sol.bodies.Find(b => b.bodyname == "Europa");
            Assert.IsNotNull(europa);
            Assert.IsNotNull(europa.volcanism);
            Assert.AreEqual("Magma", europa.volcanism.invariantType);
            Assert.AreEqual("Water", europa.volcanism.invariantComposition);
            // Eddb data does not include "major" or "minor" amounts 
        }

        [TestMethod]
        public void TestVolcanismObject()
        {
            // Hand-crafted body
            string data = @"{""allegiance"":""Federation"",
                ""faction"":""Mother Gaia"",
                ""government"":""Democracy"",
                ""id"":17072,""is_populated"":true,
                ""name"":""Sol"",""population"":22780871769,    
                ""power"":""Zachary Hudson"",""power_state"":""Control"",""primary_economy"":""Refinery"",""reserve_type"":""Common"",""security"":""High"",""state"":""Boom"",""updated_at"":1487552337,""x"":0,""y"":0,""z"":0,""bodies"":[{""arg_of_periapsis"":55.19,""atmosphere_composition"":[{""atmosphere_component_id"":3,""atmosphere_component_name"":""Carbon dioxide"",""share"":96.5},{""atmosphere_component_id"":9,""atmosphere_component_name"":""Nitrogen"",""share"":3.5}],""atmosphere_type_id"":6,""atmosphere_type_name"":""Carbon dioxide"",""axis_tilt"":177.3,""created_at"":1466612896,""distance_to_arrival"":361,""earth_masses"":0.815,""gravity"":0.91,""group_id"":6,""group_name"":""Planet"",""id"":4,""is_landable"":0,""is_rotational_period_tidally_locked"":true,""materials"":[{""material_id"":22,""material_name"":""Ruthenium"",""share"":null}],""name"":""Venus"",""orbital_eccentricity"":0.0067,""orbital_inclination"":3.39,""orbital_period"":224.7,""radius"":6052,""rotational_period"":243,""semi_major_axis"":0.72,""solid_composition"":[{""share"":70,""solid_component_id"":3,""solid_component_name"":""Rock""},{""share"":30,""solid_component_id"":2,""solid_component_name"":""Metal""}],""surface_pressure"":93.19,""surface_temperature"":735,""system_id"":17072,""terraforming_state_id"":1,""terraforming_state_name"":""Not terraformable"",""type_id"":30,""type_name"":""High metal content world"",""updated_at"":1477503587,
                ""volcanism"":{""type"":""Geysers"",""composition"":""Iron"",""amount"":""Major""}}]}";

            StarSystem system = JsonConvert.DeserializeObject<StarSystem>(data);
            Assert.IsNotNull(system);
            Body body = system.bodies[0];
            Assert.IsNotNull(body);
            Assert.IsNotNull(body.volcanism);
            Assert.AreEqual("Geysers", body.volcanism.invariantType);
            Assert.AreEqual("Iron", body.volcanism.invariantComposition);
            Assert.AreEqual("Major", body.volcanism.invariantAmount);
        }

        [TestMethod]
        public void TestVADiscoveryScanEvent() 
        {
            string line = @"{ ""timestamp"":""2019-10-26T02:15:49Z"", ""event"":""FSSDiscoveryScan"", ""Progress"":0.439435, ""BodyCount"":7, ""NonBodyCount"":3, ""SystemName"":""Outotz WO-A d1"", ""SystemAddress"":44870715523 }";
            List<Event> events = JournalMonitor.ParseJournalEntry(line);
            Assert.IsTrue(events.Count == 1);
            Assert.IsInstanceOfType(events[0], typeof(DiscoveryScanEvent));
            DiscoveryScanEvent ev = events[0] as DiscoveryScanEvent;

            Assert.AreEqual(7, ev.totalbodies);
            Assert.AreEqual(3, ev.nonbodies);
            Assert.AreEqual(44, ev.progress);

            List<string> setKeys = new List<string>();
            EddiVoiceAttackResponder.VoiceAttackVariables.setEventValues(vaProxy, ev, setKeys);
            EddiVoiceAttackResponder.VoiceAttackVariables.setEventExtendedValues(vaProxy, "EDDI " + ev.type.ToLowerInvariant(), JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ev)), setKeys);

            Assert.AreEqual(7, vaProxy.vaVars.FirstOrDefault(k => k.Key == "EDDI discovery scan totalbodies").Value);
            Assert.AreEqual(3, vaProxy.vaVars.FirstOrDefault(k => k.Key == "EDDI discovery scan nonbodies").Value);
            Assert.AreEqual(44M, vaProxy.vaVars.FirstOrDefault(k => k.Key == "EDDI discovery scan progress").Value);
        }
    }
}
