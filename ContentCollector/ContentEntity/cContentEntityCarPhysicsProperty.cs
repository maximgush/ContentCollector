using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ContentCollector
{
    public class cContentEntityCarPhysicsProperty : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityCarPhysicsProperty(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
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
                build.AddContentEntity(new cContentEntitySimple(val, this));

            // [Engine]
            // EngineFileName="Engine/Car01/vaz2112.power.csv"
            GetPrivateProfileString("Engine", "EngineFileName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntitySimple(@"data\physics\" + val, this));

            // [Engine]
            // ResistFileName="Engine/Car01/vaz2112.losts.csv"
            GetPrivateProfileString("Engine", "ResistFileName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntitySimple(@"data\physics\" + val, this));

            // [Engine]
            // MountBrakeFileName="Engine/Car34/mount_brake.csv"
            GetPrivateProfileString("Engine", "MountBrakeFileName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntitySimple(@"data\physics\" + val, this));

            // [Transmission]
            // IntarderFileName=""
            GetPrivateProfileString("Engine", "ClutchFileName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntitySimple(@"data\physics\" + val, this));


            // [Transmission]
            // AutoTransmissionName="Transmission\Common"
            GetPrivateProfileString("Transmission", "AutoTransmissionName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntityDirectory(@"data\physics\" + val, this));

            // [Transmission]
            // IntarderFileName=""
            GetPrivateProfileString("Transmission", "IntarderFileName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntitySimple(@"data\physics\" + val, this));

            // [Brake]
            // ElectricBrakeFileName=""
            GetPrivateProfileString("Brake", "ElectricBrakeFileName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(new cContentEntitySimple(@"data\physics\" + val, this));
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityCarPhysicsProperty
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
