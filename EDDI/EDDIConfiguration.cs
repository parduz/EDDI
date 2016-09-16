﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace EDDI
{
    /// <summary>Configuration for EDDI</summary>
    public class EDDIConfiguration
    {
        [JsonProperty("homeSystem")]
        public string HomeSystem { get; set; }
        [JsonProperty("homeStation")]
        public string HomeStation { get; set; }
        [JsonProperty("debug")]
        public bool Debug { get; set; }
        [JsonProperty("insurance")]
        public decimal Insurance { get; set; }

        [JsonIgnore]
        private string dataPath;

        public EDDIConfiguration()
        {
            Debug = false;
            Insurance = 5;
        }

        /// <summary>
        /// Obtain configuration from a file.  If the file name is not supplied the the default
        /// path of %APPDATA%\EDDI\eddi.json is used
        /// </summary>
        public static EDDIConfiguration FromFile(string filename=null)
        {
            if (filename == null)
            {
                filename = Constants.DATA_DIR + @"\eddi.json";
            }

            EDDIConfiguration configuration;
            try
            {
                configuration = JsonConvert.DeserializeObject<EDDIConfiguration>(File.ReadAllText(filename));
            }
            catch
            {
                configuration = new EDDIConfiguration();
            }
            configuration.dataPath = filename;

            return configuration;
        }

        /// <summary>
        /// Write configuration to a file.  If the filename is not supplied then the path used
        /// when reading in the configuration will be used, or the default path of 
        /// %APPDATA%\EDDI\eddi.json will be used
        /// </summary>
        public void ToFile(string filename=null)
        {
            if (filename == null)
            {
                filename = dataPath;
            }
            if (filename == null)
            {
                filename = Constants.DATA_DIR + @"\eddi.json";
            }

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filename, json);
        }
    }
}
