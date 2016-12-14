using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityTrafficCar : cContentEntitySimple
    {
        public cContentEntityTrafficCar(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string mCarName;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityTrafficCar(string name, cContentEntitySimple parent, string carName)
            : base(name, parent)
        {
            mCarName = carName;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // CarProperty
            build.AddContentEntity(new cContentEntityCarProperty(@"data\gamedata\cars\" + mCarName + @"\CarProperty.ini", this));

            // p_traffic_setup.ini
            build.AddContentEntity(new cContentEntityCarPhysicsProperty(@"data/physics/cars/" + mCarName + @"\p_traffic_setup.ini", this));

            // n2
            string path = build.GetManglePath("export\\gfxlib\\cars\\" + mCarName);
            foreach (string file in Directory.EnumerateFiles(path, "*.n2", SearchOption.AllDirectories))
            {
                string name = build.GetRelativePath(file);
                build.AddContentEntity(new cContentEntityN2(name, this));
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
