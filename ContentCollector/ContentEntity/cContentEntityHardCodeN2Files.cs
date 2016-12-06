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
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            StreamReader reader = new StreamReader(new FileStream(Name, FileMode.Open));            
            while (!reader.EndOfStream)
            {
                string file = reader.ReadLine();
                build.AddContentEntity(typeof(cContentEntityN2), file, this);
            }
            reader.Close();              
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityHardCodeFiles
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
