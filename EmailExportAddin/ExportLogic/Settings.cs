using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExportLogic
{
    public class Settings
    {
        private ILog log = LogManager.GetLogger(typeof(Settings));
        public string NetworkConfigPath { get; set; }
        public string ProposalPath { get; set; }
        public string MarketingPath { get; set; }
        public string ProjectPath { get; set; }
        public string AttachmentsSuffix { get; set; }
        

        private static Settings settings;

        public static Settings Initialize()
        {
            if (settings != null)
                return settings;
            var tempSettings = new Settings();
            if (!tempSettings.ReadSettingsFromFile("local-config.txt"))
                return null;
            if (!tempSettings.ReadSettingsFromFile(tempSettings.NetworkConfigPath))
                return null;
            settings = tempSettings;
            return settings;
        }

        private bool ReadSettingsFromFile(string file)
        {
            if(!Path.IsPathRooted(file))
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
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
        private readonly string userSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "emailexport.usersettings.txt");

        public UserSettings GetUserSettings()
        {
            var us = new UserSettings { MruItems = new List<MruItem>(), RemoveMailAfterExport = true };
            if (!File.Exists(userSettingsPath))
                return us;

            us = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(userSettingsPath));
            return us;
        }

        public void SetUserSettings(UserSettings us)
        {
            File.WriteAllText(userSettingsPath, Newtonsoft.Json.JsonConvert.SerializeObject(us));            
        }        
    }

    public class MruItem
    {
        public string ProjectNumber { get; set; }
        public TargetType ProjectType { get; set; }
        public string ProjectName { get; set; }

        public override string ToString()
        {
            return ProjectNumber + " "  + ProjectName + " - " + ProjectType;
        }
        public override bool Equals(object obj)
        {
            MruItem mi = obj as MruItem;
            if (mi == null)
                return false;
            return mi.ProjectNumber == ProjectNumber && mi.ProjectType == ProjectType;
        }

        public override int GetHashCode()
        {
            return ProjectNumber.GetHashCode() ^ ProjectType.GetHashCode();
        }
    }
    public class UserSettings
    {
        public UserSettings()
        {
            OpenFolderAfterExport = true;
        }
        public List<MruItem> MruItems { get; set; }
        public bool RemoveMailAfterExport { get; set; }

        public bool OpenFolderAfterExport { get; set; }
    }
}
