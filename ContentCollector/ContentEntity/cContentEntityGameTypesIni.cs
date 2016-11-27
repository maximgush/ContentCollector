using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace ContentCollector
{
    public class cContentEntityGameTypesIni : cContentEntitySimple
    {
        public cContentEntityGameTypesIni()
        {
        }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
 
        public override void Parse(cBuild build)
        {
            StringBuilder stringBuilder = new StringBuilder(255);

            // PlayerCars
            GetPrivateProfileString(build.ProductInternalName, "cars","",stringBuilder,255,this.FileName);
            string cars = stringBuilder.ToString();
            cars = cars.Trim(new char[] {'[', ']', ' '});
            string[] carsArrayStrings = cars.Split(',');

            foreach (var car in carsArrayStrings)
            {
                build.AddContentEntity(typeof(cContentEntityPlayerCar), "PlayerCar: " + car, null, this);
            }

            // Missions
            int index = 1;
            GetPrivateProfileString(build.ProductInternalName, "mission_" + index.ToString(), "", stringBuilder, 255, this.FileName);
            string missionPath = stringBuilder.ToString();

            while (missionPath != "")
            {
                build.AddContentEntity(typeof(cContentEntityMission), "Mission: " + missionPath, missionPath, this);

                index++;
                GetPrivateProfileString(build.ProductInternalName, "mission_" + index.ToString(), "", stringBuilder, 255, this.FileName);
                missionPath = stringBuilder.ToString();
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
