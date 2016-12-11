using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityPlayerCar : cContentEntitySimple
    {
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string[] words = Name.Split(new char[]{'\\'});
            string carName = words[2];

            // CarProperty
            build.AddContentEntity(typeof(cContentEntityCarProperty), @"data\gamedata\cars\" + carName + @"\CarProperty.ini", this);

            // p_player_setup.ini
            build.AddContentEntity(typeof(cContentEntityCarPhysicsProperty), @"data\physics\cars\" + carName + @"\p_player_setup.ini", this);

            // n2
            string path = build.GetManglePath("export\\gfxlib\\cars\\" + carName);
            foreach (string file in Directory.EnumerateFiles(path, "*.n2", SearchOption.AllDirectories))
            {
                string name = build.GetRelativePath(file);
                build.AddContentEntity(typeof(cContentEntityN2), name, this);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
