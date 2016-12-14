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
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityHardCodeFiles(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
        virtual public string IntermediateDir { get { return ""; } }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            StreamReader reader = new StreamReader(new FileStream(Name, FileMode.Open));            
            while (!reader.EndOfStream)
            {
                string file = IntermediateDir + reader.ReadLine();
                Utils.GetNormalPath(ref file);
                if (Path.GetExtension(file) == ".tga" || Path.GetExtension(file) == ".dds")
                    build.AddContentEntity(new cContentEntityTextureTga("(logic)" + file, this));
                else if  (Path.GetExtension(file) == ".ini" && file.Contains(@"\physics\"))
                    build.AddContentEntity(new cContentEntityPhysicsIni(file, this));
                else
                    build.AddContentEntity(new cContentEntitySimple(file, this));
            }
            reader.Close();              
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityHardCodeFiles
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
