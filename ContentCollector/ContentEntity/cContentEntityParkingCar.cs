using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Xml;

namespace ContentCollector
{
    public class cContentEntityParkingCar : cContentEntitySimple
    {        
        public string mCarName;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityParkingCar(string name, cContentEntitySimple parent, string carName)
            : base(name, parent)
        {
            mCarName = carName;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            //p_parking_setup.ini
            //build.AddContentEntity(typeof(cContentEntitySimple), @"data/physics/cars/" + carName + @"\p_parking_setup.ini", this);

            // n2
            string path = build.GetManglePath("export\\gfxlib\\cars\\" + mCarName);
            foreach (string file in Directory.EnumerateFiles(path, "*.n2", SearchOption.AllDirectories))
            {
                string name = build.GetRelativePath(file);
                build.AddContentEntity(typeof(cContentEntityN2), name, this);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityMission
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
