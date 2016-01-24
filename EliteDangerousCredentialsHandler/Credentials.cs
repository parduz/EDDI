﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace EliteDangerousDataProvider
{
    /// <summary>Storage of credentials for a single Elite: Dangerous user to access the Companion App</summary>
    public class Credentials
    {
        [JsonProperty("CompanionApp")]
        public String appId { get; set; }
        [JsonProperty("mid")]
        public String machineId { get; set; }
        [JsonProperty("mtk")]
        public String machineToken { get; set; }

        [JsonIgnore]
        private String dataPath;

        /// <summary>
        /// Obtain credentials from a file.  If the file name is not supplied the the default
        /// path of %APPDATA%\EDDI\credentials.json is used
        /// </summary>
        public static Credentials FromFile(string filename=null)
        {
            if (filename == null)
            {
                String dataDir = Environment.GetEnvironmentVariable("AppData") + "\\EDDI";
                System.IO.Directory.CreateDirectory(dataDir);
                filename = dataDir + "\\credentials.json";
            }

            Credentials credentials;
            try
            {
                String credentialsData = File.ReadAllText(filename);
                credentials = JsonConvert.DeserializeObject<Credentials>(credentialsData);
            }
            catch
            {
                credentials = new Credentials();
            }

            credentials.dataPath = filename;
            return credentials;
        }

        /// <summary>
        /// Clear the information held by credentials.
        /// </summary>
        public void Clear()
        {
            appId = null;
            machineId = null;
            machineToken = null;
        }

        /// <summary>
        /// Obtain credentials to a file.  If the filename is not supplied then the path used
        /// when reading in the credentials will be used, or the default path of 
        /// %APPDATA%\EDDI\credentials.json will be used
        /// </summary>
        public void ToFile(string filename=null)
        {
            if (filename == null)
            {
                filename = dataPath;
            }

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filename, json);
        }
    }
}
