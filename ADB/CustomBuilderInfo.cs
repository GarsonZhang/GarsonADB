using System;
using System.Collections.Generic;
using System.Text;

namespace ADB
{
    class BuilderInfo
    {
        string _name;

        public string Name
        {
            get { return _name; }
        }

        string _configFile;

        public string ConfigFile
        {
            get { return _configFile; }
        }

        string _assemblyFile;

        public string AssemblyFile
        {
            get { return _assemblyFile; }
        }

        string _entry;

        public string Entry
        {
            get { return _entry; }
        }

        string _adbVersion;

        public string ADBVersion
        {
            get { return _adbVersion; }
        }

        public BuilderInfo(string name,string entry, string configFile, string assemblyFile,string adbVersion)
        {
            _name = name;
            _configFile = configFile;
            _assemblyFile = assemblyFile;
            _entry = entry;
            _adbVersion = adbVersion;
        }
    }
}
