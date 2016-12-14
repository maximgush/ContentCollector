using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;

namespace ContentCollector
{
    public class cContentEntityTuningXml : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityTuningXml(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(build.GetManglePath(Name));

            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Texture"))
            {
                string path = @"export\textures\" + node.Attributes["path"].Value;
                build.AddContentEntity(new cContentEntityTextureTga(path, this));
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
