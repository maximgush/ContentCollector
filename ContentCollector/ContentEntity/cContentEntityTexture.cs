using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    public class cContentEntityTexture : cContentEntitySimple
    {
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string texturesName = Name.Replace("(logic)","").Replace(@"\textures\", @"\texturesdds\");
            if (!texturesName.EndsWith(".dds"))
            {
                texturesName = texturesName + ".dds";
                build.AddContentEntity(typeof(cContentEntitySimple), texturesName, this);
            }

            AddEntityVariants(build);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
