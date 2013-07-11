using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EmailExportAddin
{
    internal class Settings
    {
        private ILog log = LogManager.GetLogger(typeof(Settings));
        public string NetworkConfigPath { get; set; }
        public string ProposalPath { get; set; }
        public string MarketingPath { get; set; }
        public string ProjectPath { get; set; }
        private static Settings settings;

        public static Settings Initialize()
        {
            if (settings != null)
                return settings;
            var tempSettings = new Settings();
            if (!tempSettings.ReadSettingsFromFile("local-config.txt"))
                return null;
            if (!tempSettings.ReadSettingsFromFile(settings.NetworkConfigPath))
                return null;
            settings = tempSettings;
            return settings;
        }

        private bool ReadSettingsFromFile(string file)
        {
            if(!Path.IsPathRooted(file))
                file = Path.GetFullPath(file);
            if (!File.Exists(file))
            {
                var message = "Could not find settings file: " + file + " . Export cannot continue.";
                log.Error(message);
                MessageBox.Show(message);
                return false;
            }
            try
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (IsStringEmpty(line))
                        continue;
                    var parts = SplitByFirstOccurence(line, ':');
                    if (parts.Length != 2)
                        continue;
                    this.GetType().GetProperty(parts[0]).SetValue(this, parts[1], null);
                }
                return true;
            }
            catch (Exception ex)
            {
                var message = "Error while reading settings file: " + file + " . Export cannot continue. Details:" + ex.Message;
                log.Fatal(message, ex);
                MessageBox.Show(message);
                return false;                
            }
            
        }

        private string[] SplitByFirstOccurence(string value, char marker)
        {
            int index = value.IndexOf(marker);
            return new string[] { value.Substring(0, index), value.Substring(index + 1) };
        }
        
        private bool IsStringEmpty(string s)
        {
            return s == null || string.IsNullOrEmpty(s.Trim());
        }


        
    }
}
