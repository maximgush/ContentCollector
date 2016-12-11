using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ContentCollector
{
    public class cContentEntityN2 : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string fileName = build.GetManglePath(Name);
            if (!File.Exists(fileName))
                return;

            StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open));
            string n2fileText = reader.ReadToEnd();
            reader.Close();

            #region Textures
            {
                //.settexture "BumpMap0" "textures:/cars/Car01/Car01_vaz2110_bump_x0.tga"
                string pattern = "settexture[^\\r\\n]+\"([^\\r\\n\"]+)\"[\\r\\n]";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(n2fileText);

                while (match.Success)
                {
                    build.AddContentEntity(typeof(cContentEntityTextureTga), "(logic)" + match.Groups[1].Value, this);
                    match = match.NextMatch();
                }
            }
            #endregion

            #region Meshes
            {
                //.setmesh "home:export/meshes/cars/Car01/carLod0.nvx2"
                string pattern = "setmesh\\s+\"([^\\r\\n]+)\"[\\r\\n]";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(n2fileText);

                while (match.Success)
                {
                    build.AddContentEntity(typeof(cContentEntitySimple), match.Groups[1].Value, this);
                    match = match.NextMatch();
                }   
            }
            #endregion  
        
            #region Animation
            {
                //.setanim "home:export/anims/characters/pedestrians/pedestrian01/human0.nax2"
                string pattern = "setanim\\s+\"([^\\r\\n]+)\"[\\r\\n]";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(n2fileText);

                while (match.Success)
                {
                    build.AddContentEntity(typeof(cContentEntitySimple), match.Groups[1].Value, this);
                    match = match.NextMatch();
                }
            }
            #endregion  

            #region hkx
            {
                string hkx_fileName = Name.Replace("\\gfxlib\\","\\meshes\\").Replace(".n2", "_c_0.hkx");
                if (build.ExistFileInProject(hkx_fileName))
                    build.AddContentEntity(typeof(cContentEntitySimple), hkx_fileName, this);
            }
            #endregion

            AddEntityVariants(build);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityN2
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
