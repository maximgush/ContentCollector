using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ContentCollector
{
    public class cContentEntityPhysicsIni : cContentEntitySimple
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityPhysicsIni(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
        
            string val = "";
            string fileName = build.GetManglePath(Name);
            StringBuilder stringBuilder = new StringBuilder(255);

            // [Common]
            // PhysicsFile = "meshes:Devices/Device08/device_p.hkx"
            GetPrivateProfileString("Common", "PhysicsFiles", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
            {
                build.AddContentEntity(new cContentEntitySimple(val, this));
            }         
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
