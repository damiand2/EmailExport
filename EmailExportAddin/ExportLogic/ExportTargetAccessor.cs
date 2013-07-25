using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ExportLogic
{
    public class ExportTargetAccessor
    {
        Settings settings;

        private static readonly Regex nameRegex = new Regex(@"(?<number>\d{5,7})(\s*(?<name>\(*.*\)*)){0,1}", RegexOptions.Compiled | RegexOptions.Singleline);

        public ExportTargetAccessor(Settings s)
        {
            settings = s;
        }

        public bool CreateProjectFolder(SingleResult result, FindResults allResults, string projectNumber)
        {
            if (result == null || string.IsNullOrEmpty(result.ProjectPath))
                return false;

            if (!string.IsNullOrEmpty(result.EmailFolderPath) && Directory.Exists(result.EmailFolderPath))
                return true;//folder already exists and was selected by user, nothing to do here
                        
            if (!allResults.IsAtLeastOneValid())
            {
                if (!SetNewProject(allResults, projectNumber))
                    return false;
                
            }

            
            var dir = new DirectoryInfo(result.ProjectPath);
            if (!dir.Exists)
            {
                var dialogResult = MessageBox.Show(string.Format("Do you want to create new folder {0} {1} - {2} ?", result.ProjectNumber, result.ProjectName, result.Type), "Create new folder?", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.Cancel)
                    return false;
            }
            if (CreateFolder(dir.Parent, dir.Name))
            {
                if (dir.ExistsSubFolder("Emails"))
                {
                    result.EmailFolderPath = result.ProjectPath.AppendSlash() + "Emails".AppendSlash();
                    return true;
                }
                    
                if (dir.ExistsSubFolder("Email"))
                {
                    result.EmailFolderPath = result.ProjectPath.AppendSlash() + "Email".AppendSlash();
                    return true;
                }
                if( CreateFolder(dir, "Emails"))
                {
                    result.EmailFolderPath = result.ProjectPath.AppendSlash() + "Emails".AppendSlash();
                    return true;
                }
            }
            return false;    
        }

        private bool SetNewProject(FindResults results, string projectNumber)
        {
            projectNumber = projectNumber.Substring(0, 5);
            var projNameDialog = new ProjectName();
            var dialogResult = projNameDialog.ShowDialog();
            string projName = null;
            if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
                return false;
            if (!string.IsNullOrEmpty(projNameDialog.tbProjectName.Text.Trim()))
                projName = projNameDialog.tbProjectName.Text.Trim();
            if (!projName.StartsWith("("))
                projName = "(" + projName + ")";
            projNameDialog.Dispose();
            FillProjectDataForAll(results, projectNumber, projName);
            return true;
        }

        private static void FillProjectDataForAll(FindResults results, string projectNumber, string projName)
        {
            results.Results.ForEach(r =>
            {
                if (r.FatalError)
                    return;
                if (!r.Exists && r.NoAccess == false && string.IsNullOrEmpty(r.ProjectName))
                {
                    var fullName = projectNumber;
                    var projNum = projectNumber;
                    switch (r.Type)
                    {
                        case TargetType.Project:
                            fullName = projNum + (string.IsNullOrEmpty(projName) ? "" : (" " + projName));
                            break;
                        case TargetType.Proposal:
                        case TargetType.Marketing:
                            projNum += "00";
                            fullName = projNum + (string.IsNullOrEmpty(projName) ? "" : (" " + projName));
                            break;

                    }
                    r.ProjectNumber = projNum;
                    r.ProjectName = projName;
                    r.ProjectPath = r.ProjectPath.AppendSlash() + fullName.AppendSlash();
                }
            });
        }

        public FindResults Find(string number)
        {
            if (string.IsNullOrEmpty(number) || (number.Length != 5 && number.Length != 7))
            {
                MessageBox.Show("Project number must contain only digits and be  5 or 7 digits long.");
                return null;
            }

            long value;
            if (!long.TryParse(number, out value))
            {
                MessageBox.Show("Project number must contain only digits and be  5 or 7 digits long.");
                return null;
            }
            string projectTypePart = "00";
            if (number.Length == 7)
            {
                projectTypePart = number.Substring(number.Length - 2, 2);
                number = number.Substring(0, 5);
            }

            string yearPart = number.Substring(0, 2);            
            var results = new FindResults();
            GatherAllResults(number, yearPart, results);
            FillMissingProjectNames(results);
            if (number.Length == 5 && results.Results.Find(r=> r.Type == TargetType.Project).Exists)
            {
                results.DefaultResult = TargetType.Project;
            }
            else
            {                
                switch (projectTypePart)
                {
                    case "00": results.DefaultResult = TargetType.Proposal;
                        break;
                    case "01": results.DefaultResult = TargetType.Project;
                        break;
                    default: results.DefaultResult = TargetType.Project;
                        break;
                }
            }
            return results;
        }

        private void FillMissingProjectNames(FindResults results)
        {
            var existing = results.Results.FirstOrDefault(r => r.Exists && !string.IsNullOrEmpty(r.ProjectName));
            if (existing == null)
                return;
            var projNum = existing.ProjectNumber.Substring(0, 5);
            FillProjectDataForAll(results, projNum, existing.ProjectName);
            //results.Results.ForEach(r =>
            //{
            //    if (!r.Exists && r.NoAccess == false && string.IsNullOrEmpty(r.ProjectName))
            //    {
            //        r.ProjectName = existing.ProjectName;
            //        r.ProjectPath = r.ProjectPath.AppendSlash() + r.ProjectName.AppendSlash();
            //    }
            //});
        }

        private void GatherAllResults(string number, string yearPart, FindResults results)
        {
            var result = FindResult(settings.MarketingPath, yearPart, number +"00");
            result.Type = TargetType.Marketing;
            results.Results.Add(result);
            result = FindResult(settings.ProjectPath, yearPart, number);
            result.Type = TargetType.Project;
            results.Results.Add(result);
            result = FindResult(settings.ProposalPath, yearPart, number + "00");
            result.Type = TargetType.Proposal;
            results.Results.Add(result);
        }

        private SingleResult FindResult(string rootPath, string yearPart, string projectNumber)
        {
            var originalRoot = rootPath;
            rootPath = rootPath.AppendSlash() + yearPart.AppendSlash();
            if (!Directory.Exists(rootPath))
            {
                rootPath = originalRoot.AppendSlash() + "20" + yearPart.AppendSlash();
                if (!Directory.Exists(rootPath))
                    return new SingleResult { Exists = false, NoAccess = true, FatalError = true, WarningMessage = "Folder for specified year: " + yearPart + " does not exist at all, please create it manually", ProjectPath = originalRoot };
            }

            var dirs = Directory.GetDirectories(rootPath, projectNumber, SearchOption.TopDirectoryOnly);
            if(dirs.Length != 1)
                dirs = Directory.GetDirectories(rootPath, projectNumber + "*", SearchOption.TopDirectoryOnly);
            if (dirs.Length > 1)
            {
                string message = "Found more then one folder in '" + rootPath + "' that matches project number: " + projectNumber + ". Export mail does not know what to do with that situation";
                MessageBox.Show(message);
                return new SingleResult { Exists = false, NoAccess = true, FatalError = true, WarningMessage = message, ProjectPath = originalRoot };
            }
            if (dirs.Length == 0)
                return new SingleResult { Exists = false, WarningMessage = "Main project's folder does not exist", ProjectPath = rootPath };

            DirectoryInfo dir = new DirectoryInfo(dirs[0]);
            var match = nameRegex.Match(dir.Name);
            var projName = match.Groups["name"].Value;
            var projNumber = match.Groups["number"].Value;
            if (!HasWritePermissions(dir))
                return new SingleResult { Exists = true, NoAccess = true, WarningMessage = "No write permissions to folder", ProjectName = projName, ProjectNumber = projNumber, ProjectPath = dir.FullName };

            var result = new SingleResult { Exists = true, NoAccess = false, ProjectName = projName, ProjectNumber = projNumber, ProjectPath = dir.FullName };
            
            
            return result;
        }        

        private bool CreateFolder(DirectoryInfo dir, string folderName)
        {
            try
            {
                if(!dir.ExistsSubFolder(folderName))                
                    dir.CreateSubdirectory(folderName);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("No write access to create folder '" + folderName + "' underneath '" + dir.FullName + "'");
                return false;
            }
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
            if (!value.EndsWith("\\"))
                value += "\\";
            return value;
        }

        public static bool ExistsSubFolder(this DirectoryInfo dir, string folderName)
        {
            return dir.GetDirectories(folderName, SearchOption.TopDirectoryOnly).Length > 0;
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
        public string ProjectNumber;
        public string ProjectPath;
        public bool Exists;
        public bool NoAccess;
        public string WarningMessage;
        public bool FatalError;
        public string EmailFolderPath;
        public string SubFolder;
    }
}
