using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace ContentCollector
{
    public class cContentEntityLocation : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string[] words = Name.Split('\\');
            string city = words[0];
            string location = words[1];

            // db3
            build.AddContentEntity(typeof(cContentEntityLocationDB3), @"export\db\" + city + "\\" + location + ".db3", @"export\db3\" + city + "\\" + location + ".db3", this);

            // startpoints
            build.AddContentEntity(typeof(cContentEntitySimple), @"data\startpoints\" + city + "\\" + location + @"\startpoints.xml", @"data\startpoints\" + city + "\\" + location + @"\startpoints.xml", this);

            // тематические зоны
            build.AddContentEntity(typeof(cContentEntitySimple),  @"export\levels\" + city + "\\" + location + "_zones.xml",  @"export\levels\" + city + "\\" + location + "_zones.xml", this);

            // Физика (.pstatic)
            build.AddContentEntity(typeof(cContentEntityLocationPstatic), @"data\world\" + Name + ".pstatic", null, this);

            // Навигация
            build.AddContentEntity(typeof(cContentEntityLocationMap), @"export\levels\" + Name + ".map", null, this);
            build.AddContentEntity(typeof(cContentEntityLocationMap), @"export\levels\" + Name + ".add.map", null, this);    
        }
    }   // cContentEntityLocation
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // cContentEntityLocation
