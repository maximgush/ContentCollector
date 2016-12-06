using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityPedestrian : cContentEntitySimple
    {
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string[] words = Name.Split(new char[]{'\\'});
            string pedName = words[1];

            // CarProperty
            build.AddContentEntity(typeof(cContentEntityCarProperty), @"data\gamedata\pedestrians\" + pedName + @"\CarProperty.ini", this);

            foreach (string humanN2 in new string[] { "human.n2", "human_run.n2", "human_idle.n2" })
            {
                build.AddContentEntity(typeof(cContentEntityN2), @"export/gfxlib/characters/pedestrians/" + pedName + @"/" + humanN2, this);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityPedestrian
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
