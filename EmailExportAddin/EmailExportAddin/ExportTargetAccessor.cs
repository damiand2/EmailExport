using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EmailExportAddin
{
    public class ExportTargetAccessor
    {
        Settings settings;

        private static const Regex nameRegex = new Regex(@"\d{5,7}(\s*\((?<name>.*)\)){0,1}", RegexOptions.Compiled | RegexOptions.Singleline);

        public ExportTargetAccessor(Settings s)
        {
            settings = s;
        }

        public FindResults Find(string number)
        {
            if (string.IsNullOrEmpty(number) || number.Length < 5 || number.Length > 7)
            {
                MessageBox.Show("Project number must contain only digits and be at least 5 and at most 7 digits long.");
                return null;
            }

            long value;
            if (!long.TryParse(number, out value))
            {
                MessageBox.Show("Project number must contain only digits and be at least 5 and at most 7 digits long.");
                return null;
            }

            string yearPart = number.Substring(0, 2);
            string projectTypePart = number.Substring(number.Length - 2, 2);
            var results = new FindResults();
            GatherAllResults(number, yearPart, results);
            FillMissingProjectNames(results);
            switch(projectTypePart)
            {
                case "00": results.DefaultResult = TargetType.Proposal;
                    break;
                case "01": results.DefaultResult = TargetType.Project;
                    break;
                default: results.DefaultResult = TargetType.Marketing;
                    break;
            }
            return results;
        }

        private void FillMissingProjectNames(FindResults results)
        {
            var existing = results.Results.FirstOrDefault(r => r.Exists && !string.IsNullOrEmpty(r.ProjectName));
            if (existing == null)
                return;
            results.Results.ForEach(r =>
            {
                if (!r.Exists && r.NoAccess == false && string.IsNullOrEmpty(r.ProjectName))
                {
                    r.ProjectName = existing.ProjectName;
                    r.ProjectPath = r.ProjectPath.AppendSlash() + r.ProjectName.AppendSlash();
                }
            });
        }

        private void GatherAllResults(string number, string yearPart, FindResults results)
        {
            var result = FindResult(settings.MarketingPath, yearPart, number);
            result.Type = TargetType.Marketing;
            results.Results.Add(result);
            result = FindResult(settings.ProjectPath, yearPart, number);
            result.Type = TargetType.Project;
            results.Results.Add(result);
            result = FindResult(settings.ProposalPath, yearPart, number);
            result.Type = TargetType.Proposal;
            results.Results.Add(result);
        }

        private SingleResult FindResult(string rootPath, string yearPart, string projectNumber)
        {
            rootPath = rootPath.AppendSlash() + yearPart.AppendSlash();
            if (!Directory.Exists(rootPath))
                return new SingleResult { Exists = false, NoAccess = true, FatalError = true, WarningMessage = "Folder for specified year: " + yearPart + " does not exist at all, please create it manually" };
            var dirs = Directory.GetDirectories(rootPath, projectNumber + "*", SearchOption.TopDirectoryOnly);
            if(dirs.Length > 1)
            {
                string message = "Found more then one folder in '" + rootPath + "' that matches project number: " + projectNumber + ". Export mail does not know what to do with that situation";
                MessageBox.Show(message);
                return new SingleResult { Exists = false, NoAccess = true, FatalError = true, WarningMessage = message};
            }
            if (dirs.Length == 0)
                return new SingleResult { Exists = false, WarningMessage = "Main project's folder does not exist", ProjectPath = rootPath };
            
            DirectoryInfo dir = new DirectoryInfo(dirs[0]);            

            if (!HasWritePermissions(dir))
                return new SingleResult { Exists = true, NoAccess = true, WarningMessage = "No write permissions to folder", ProjectName = dir.Name, ProjectPath = dir.FullName };

            CheckMailSubfolder(dir);
            return new SingleResult { Exists = true, NoAccess = false, ProjectName = dir.Name, ProjectPath = dir.FullName };
        }

        private void CheckMailSubfolder(DirectoryInfo dir)
        {
            var dirs = dir.GetDirectories("Mail", SearchOption.TopDirectoryOnly);
            if (dirs.Length == 0)
                dir.CreateSubdirectory("Mail");

        }

        private bool HasWritePermissions(DirectoryInfo di)
        {
            try
            {                
                var acl = di.GetAccessControl();
                var rules = acl.GetAccessRules(true, true, typeof(NTAccount));

                var currentUser = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(currentUser);
                foreach (AuthorizationRule rule in rules)
                {
                    FileSystemAccessRule fsAccessRule = rule as FileSystemAccessRule;
                    if (fsAccessRule == null)
                        continue;

                    if (fsAccessRule.FileSystemRights.HasFlag(FileSystemRights.WriteData))
                    {
                        NTAccount ntAccount = rule.IdentityReference as NTAccount;
                        if (ntAccount == null)
                        {
                            continue;
                        }

                        if (principal.IsInRole(ntAccount.Value))
                        {
                            return true;
                        }                        
                    }
                }
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        
    }

    public static class StringExtensions
    {
        public static string AppendSlash(this string value)
        {
            if(!value.EndsWith("\\"))
                value += "\\";
            return value;
        }
    }

    public class FindResults
    {
        public List<SingleResult> Results = new List<SingleResult>();
        public TargetType DefaultResult;
        public bool IsAtLeastOneValid()
        {
            return Results.Any(r => r.Exists && r.NoAccess == false && !r.FatalError && !string.IsNullOrEmpty(r.ProjectName));
        }
    }

    public enum TargetType
    {
        Proposal,
        Project, 
        Marketing
    }

    public class SingleResult
    {
        public TargetType Type;
        public string ProjectName;
        public string ProjectPath;
        public bool Exists;
        public bool NoAccess;
        public string WarningMessage;
        public bool FatalError;

    }
}
