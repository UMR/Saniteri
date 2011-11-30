using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace UMR.Saniteri.Common
{
   public class ApplicationData
    {
        static ApplicationData _applicationData;

        public static ApplicationData applicationData
        {
            get { return ApplicationData._applicationData; }
            set { ApplicationData._applicationData = value; }
        }

        static ApplicationData()
        {
            _applicationData = new ApplicationData();
        }

        Assembly _assemblyInfo;
        public Assembly assemblyInfo
        {
            get
            {
                if (this._assemblyInfo == null) this._assemblyInfo = Assembly.GetExecutingAssembly();
                return this._assemblyInfo;
            }
        }

        public string executingLocation { get { return Path.GetDirectoryName(this.assemblyInfo.Location); } }

        public string version { get { return this.assemblyInfo.GetName().Version.ToString(); } }
    }
}
