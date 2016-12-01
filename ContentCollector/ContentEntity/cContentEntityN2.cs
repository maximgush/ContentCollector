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
            if (!File.Exists(FileName))
                return;

            StreamReader reader = new StreamReader(new FileStream(FileName, FileMode.Open));
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
                    build.AddContentEntity(typeof(cContentEntityTexture), "(logic)" + match.Groups[1].Value, match.Groups[1].Value, this);
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
                    build.AddContentEntity(typeof(cContentEntitySimple), match.Groups[1].Value, match.Groups[1].Value, this);
                    match = match.NextMatch();
                }   
            }
            #endregion          
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityN2
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
