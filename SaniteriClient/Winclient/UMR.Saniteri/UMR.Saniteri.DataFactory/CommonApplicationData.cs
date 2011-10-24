using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace UMR.Saniteri.DataFactory
{
    public class CommonApplicationData
    {
        private string applicationFolder { get; set; }
        private string companyFolder { get; set; }

        private static CommonApplicationData _commonApplicationData;
        public static CommonApplicationData commonApplicationData
        {
            get
            {
                if (_commonApplicationData == null)
                    _commonApplicationData = new CommonApplicationData("UMR", "Saniteri", true);
                return _commonApplicationData;
            }
        }

        private CommonApplicationData(string companyFolder, string applicationFolder)
            : this(companyFolder, applicationFolder, false)
        { }

        private CommonApplicationData(string companyFolder, string applicationFolder, bool allUsers)
        {
            this.applicationFolder = applicationFolder;
            this.companyFolder = companyFolder;
            createFolders(allUsers);
        }

        public string applicationFolderPath
        {
            get { return Path.Combine(companyFolderPath, applicationFolder); }
        }

        public string companyFolderPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), companyFolder); }
        }

        private void createFolders(bool allUsers)
        {
            DirectoryInfo directoryInfo;
            DirectorySecurity directorySecurity;
            AccessRule rule;
            SecurityIdentifier securityIdentifier = new SecurityIdentifier
                (WellKnownSidType.BuiltinUsersSid, null);
            if (!Directory.Exists(companyFolderPath))
            {
                directoryInfo = Directory.CreateDirectory(companyFolderPath);
                bool modified;
                directorySecurity = directoryInfo.GetAccessControl();
                rule = new FileSystemAccessRule(
                        securityIdentifier,
                        FileSystemRights.Write |
                        FileSystemRights.ReadAndExecute |
                        FileSystemRights.Modify,
                        AccessControlType.Allow);
                directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                directoryInfo.SetAccessControl(directorySecurity);
            }
            if (!Directory.Exists(applicationFolderPath))
            {
                directoryInfo = Directory.CreateDirectory(applicationFolderPath);
                if (allUsers)
                {
                    bool modified;
                    directorySecurity = directoryInfo.GetAccessControl();
                    rule = new FileSystemAccessRule(
                        securityIdentifier,
                        FileSystemRights.Write |
                        FileSystemRights.ReadAndExecute |
                        FileSystemRights.Modify,
                        InheritanceFlags.ContainerInherit |
                        InheritanceFlags.ObjectInherit,
                        PropagationFlags.InheritOnly,
                        AccessControlType.Allow);
                    directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                    directoryInfo.SetAccessControl(directorySecurity);
                }
            }
        }

        public override string ToString()
        {
            return applicationFolderPath;
        }
    }
}
