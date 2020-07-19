using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace GeksoFileLib.FileListener
{
    public class IniFileListener
    {
        private string iniPath = string.Empty;
        private string exeFileName = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFileListener(string IniPath = null)
        {
            string filePath = IniPath ?? exeFileName + ".ini";
            if (File.Exists(filePath))
                iniPath = new FileInfo(filePath).FullName;
            else
                throw new System.Exception("Can't find " + filePath +" File");
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? exeFileName, Key, "", RetVal, 255, iniPath);
            if (string.IsNullOrEmpty(RetVal.ToString()))
                throw new System.Exception("Can't read " + Key +" key");
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? exeFileName, Key, Value, iniPath);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? exeFileName);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? exeFileName);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
