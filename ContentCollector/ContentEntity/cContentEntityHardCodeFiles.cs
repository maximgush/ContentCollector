using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityHardCodeFiles : cContentEntitySimple
    {
        override public string FileName { get { return null; } }
        virtual public string IntermediateDir { get { return ""; } }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            StreamReader reader = new StreamReader(new FileStream(Name, FileMode.Open));            
            while (!reader.EndOfStream)
            {
                string file = IntermediateDir + reader.ReadLine();
                build.AddContentEntity(typeof(cContentEntitySimple), file, this);
            }
            reader.Close();              
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityHardCodeFiles
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
