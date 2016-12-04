using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityHardCodeN2Files : cContentEntitySimple
    {
        public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            StreamReader reader = new StreamReader(new FileStream(FileName, FileMode.Open));
            string text = reader.ReadToEnd();
            reader.Close();

            var files = text.Split(new char[] { '\n', '\r' });
            foreach (var file in files)
            {
                build.AddContentEntity(typeof(cContentEntityN2), file, this);   
            }                
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityHardCodeFiles
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
