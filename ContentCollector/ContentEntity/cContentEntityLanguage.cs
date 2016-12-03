using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityLanguage : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string path = build.GetManglePath("data\\tables\\" + Name.Replace('-','_') + "\\");
            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                string name = build.GetRelativePath(file);
                build.AddContentEntity(typeof(cContentEntitySimple), name, name, this);
            }

            path = build.GetManglePath("data\\i18n\\" + Name.Replace('-', '_') + "\\");
            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                string name = build.GetRelativePath(file);
                build.AddContentEntity(typeof(cContentEntitySimple), name, name, this);
            }

            path = build.GetManglePath("data\\audio\\" + Name.Replace('-', '_') + "\\");
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, name, this);
                }                
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сcContentEntityLanguage
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
