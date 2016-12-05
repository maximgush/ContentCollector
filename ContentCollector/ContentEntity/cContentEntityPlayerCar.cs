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
            string carName = words[1];

            // CarProperty
            build.AddContentEntity(typeof(cContentEntityCarProperty), @"data\gamedata\cars\" + carName + @"\CarProperty.ini", this);

            // p_player_setup.ini
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarPropretyINI, Name + @"_p_player_setup.ini", @"data/physics/cars/" + Name + @"\p_player_setup.ini", this);

            // tuning
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarTuningXML, Name + @"_tuning.xml", @"data/gamedata/cars/Car" + Name + @"\tuning.xml", this);

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
