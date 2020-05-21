using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;



namespace CSACoreWin.Core {

    public class ApplicationData {
        //================================================================================
        private string                              mCompanyName;
        private string                              mApplicationName;

        private Dictionary<string, List<string>>    mRecentFiles = new Dictionary<string, List<string>>();


        //================================================================================
        //--------------------------------------------------------------------------------
        public ApplicationData(string companyName, string applicationName) {
            // Elements
            mCompanyName = companyName;
            mApplicationName = applicationName;
        }


        // ELEMENTS ================================================================================
        //--------------------------------------------------------------------------------
        public string CompanyName { get { return mCompanyName; } }
        public string ApplicationName { get { return mApplicationName; } }


        // PROGRAM DATA ================================================================================
        //--------------------------------------------------------------------------------
        public void CreateDataPaths(bool allUsers = true) {
            // Company path
            if (!Directory.Exists(ProgramDataCompanyPath)) {
                DirectoryInfo info = Directory.CreateDirectory(ProgramDataCompanyPath);
                DirectorySecurity security = info.GetAccessControl();
                AccessRule accessRule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                                                                 FileSystemRights.Write | FileSystemRights.ReadAndExecute | FileSystemRights.Modify, AccessControlType.Allow);
                security.ModifyAccessRule(AccessControlModification.Add, accessRule, out bool modified);
                info.SetAccessControl(security);
            }

            // Application path
            if (!Directory.Exists(ProgramDataPath)) {
                DirectoryInfo info = Directory.CreateDirectory(ProgramDataPath);

                if (allUsers) {
                    DirectorySecurity security = info.GetAccessControl();
                    AccessRule accessRule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                                                                     FileSystemRights.Write | FileSystemRights.ReadAndExecute | FileSystemRights.Modify,
                                                                     InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                                                     PropagationFlags.InheritOnly, AccessControlType.Allow);
                    security.ModifyAccessRule(AccessControlModification.Add, accessRule, out bool modified);
                    info.SetAccessControl(security);
                }
            }
        }

        //--------------------------------------------------------------------------------
        public string CreateProgramDataCompanyPath() {
            CreateDataPaths();
            return ProgramDataCompanyPath;
        }

        //--------------------------------------------------------------------------------
        public string CreateProgramDataPath(bool allUsers = true) {
            CreateDataPaths();
            return ProgramDataPath;
        }

        //--------------------------------------------------------------------------------
        public string ProgramDataCompanyPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), mCompanyName); } }
        public string ProgramDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), mCompanyName, mApplicationName); } }


        // USER DATA ================================================================================
        //--------------------------------------------------------------------------------
        public void CreateUserDataPaths(bool allUsers = true) {
            // Company path
            if (!Directory.Exists(UserDataCompanyPath)) {
                DirectoryInfo info = Directory.CreateDirectory(UserDataCompanyPath);
                /*DirectorySecurity security = info.GetAccessControl();
                AccessRule accessRule = new FileSystemAccessRule("Users",
                                                                 FileSystemRights.Write | FileSystemRights.ReadAndExecute | FileSystemRights.Modify, AccessControlType.Allow);
                security.ModifyAccessRule(AccessControlModification.Add, accessRule, out bool modified);
                info.SetAccessControl(security);*/
            }

            // Application path
            if (!Directory.Exists(UserDataPath)) {
                DirectoryInfo info = Directory.CreateDirectory(UserDataPath);

                /*if (allUsers) {
                    DirectorySecurity security = info.GetAccessControl();
                    AccessRule accessRule = new FileSystemAccessRule("Users",
                                                                     FileSystemRights.Write | FileSystemRights.ReadAndExecute | FileSystemRights.Modify,
                                                                     InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                                                     PropagationFlags.InheritOnly, AccessControlType.Allow);
                    security.ModifyAccessRule(AccessControlModification.Add, accessRule, out bool modified);
                    info.SetAccessControl(security);
                }*/
            }
        }

        //--------------------------------------------------------------------------------
        public string CreateUserDataCompanyPath() {
            CreateUserDataPaths();
            return UserDataCompanyPath;
        }

        //--------------------------------------------------------------------------------
        public string CreateUserDataPath(bool allUsers = true) {
            CreateUserDataPaths();
            return UserDataPath;
        }

        //--------------------------------------------------------------------------------
        public string UserDataCompanyPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), mCompanyName); } }
        public string UserDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), mCompanyName, mApplicationName); } }


        // RECENT FILES ================================================================================
        //--------------------------------------------------------------------------------
        private List<string> CreateRecentFiles(string type) {
            if (!mRecentFiles.ContainsKey(type))
                mRecentFiles.Add(type, new List<string>());
            return mRecentFiles[type];
        }

        //--------------------------------------------------------------------------------
        public void AddRecentFile(string type, string path) {
            // Add
            string fullPath = Path.GetFullPath(path);
            List<string> recent = CreateRecentFiles(type);
            recent.Remove(fullPath);
            recent.Insert(0, fullPath);

            // Save
            SaveRecentFiles(type);
        }
        
        //--------------------------------------------------------------------------------
        public void ClearRecentFiles(string type) {
            // Clear
            List<string> recent = CreateRecentFiles(type);
            recent.Clear();

            // Save
            SaveRecentFiles(type);
        }

        //--------------------------------------------------------------------------------
        public void LoadRecentFiles(string type) {
            // Clear
            List<string> recent = CreateRecentFiles(type);
            recent.Clear();

            // Load
            try {
                StreamReader reader = File.OpenText(Path.Combine(CSAWin.ApplicationData.CreateUserDataPath(), $"recent-{type}.txt"));

                while (true) {
                    string recentFile = reader.ReadLine();
                    if (recentFile == null)
                        break;
                    recent.Add(recentFile);
                }

                reader.Close();
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { }
        }
        
        //--------------------------------------------------------------------------------
        private void SaveRecentFiles(string type) {
            StreamWriter writer = File.CreateText(Path.Combine(CSAWin.ApplicationData.CreateUserDataPath(), $"recent-{type}.txt"));
            foreach (string r in CreateRecentFiles(type)) {
                writer.WriteLine(r);
            }
            writer.Close();
        }
        
        //--------------------------------------------------------------------------------
        public IEnumerable<string> RecentFiles(string type) { return CreateRecentFiles(type); }
    }

}
