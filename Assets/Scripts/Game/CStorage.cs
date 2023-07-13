using System.IO;

using static sdw.cpp;
using static sdw.std;
using static sdw.zzz;

// alias
// typedef begin
using n8 = System.SByte;
using n16 = System.Int16;
using n32 = System.Int32;
using n64 = System.Int64;
using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;

using f32 = System.Single;
using f64 = System.Double;

using static_cast_n8 = System.SByte;
using static_cast_n16 = System.Int16;
using static_cast_n32 = System.Int32;
using static_cast_n64 = System.Int64;
using static_cast_u8 = System.Byte;
using static_cast_u16 = System.UInt16;
using static_cast_u32 = System.UInt32;
using static_cast_u64 = System.UInt64;

using static_cast_f32 = System.Single;
using static_cast_f64 = System.Double;
// typedef end

namespace majiang0
{
    public class CStorage
    {
        private string m_sDirPath = "";

        public CStorage(string a_sDirPath)
        {
            m_sDirPath = a_sDirPath;
            Directory.CreateDirectory(m_sDirPath);
        }

        public void SetItem(string a_sId, string a_sValue, string a_sExtName = "json")
        {
            string sPath = m_sDirPath + "/" + a_sId + "." + a_sExtName;
            File.WriteAllText(sPath, a_sValue);
        }

        public string GetItem(string a_sId, string a_sExtName = "json")
        {
            string sPath = m_sDirPath + "/" + a_sId + "." + a_sExtName;
            if (File.Exists(sPath))
            {
                return File.ReadAllText(sPath);
            }
            else
            {
                return null;
            }
        }

        public void RemoveItem(string a_sId, string a_sExtName = "json")
        {
            string sPath = m_sDirPath + "/" + a_sId + "." + a_sExtName;
            if (File.Exists(sPath))
            {
                File.Delete(sPath);
            }
        }
    }
}
