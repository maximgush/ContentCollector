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
        string mCity;
        string mLocation;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityLocation(string name, cContentEntitySimple parent, string city, string location)
            : base(name, parent)
        {
            mCity = city;
            mLocation = location;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string citySlashName = mCity + "\\" + mLocation;

            // db3
            build.AddContentEntity(new cContentEntityLocationDB3(@"export\db\" + citySlashName + ".db3", this));

            // startpoints
            build.AddContentEntity(new cContentEntitySimple(@"data\startpoints\" + citySlashName + @"\startpoints.xml", this));

            // Какие-то координаты чего-то..
            build.AddContentEntity(new cContentEntitySimple(@"data\world\" + citySlashName + ".ini", this));

            // Триггерные зоны?..
            build.AddContentEntity(new cContentEntitySimple(@"data\world\" + citySlashName + "_actions.xml", this));

            // Физика (.pstatic)
            build.AddContentEntity(new cContentEntityLocationPstatic(@"data\world\" + citySlashName + ".pstatic", this));

            // Навигация
            build.AddContentEntity(new cContentEntityLocationMap(@"export\levels\" + citySlashName + ".map", this));
            build.AddContentEntity(new cContentEntityLocationMap(@"export\levels\" + citySlashName + ".add.map", this));

            // тематические зоны
            build.AddContentEntity(new cContentEntitySimple(@"export\levels\" + citySlashName + "_zones.xml", this));

            // ??
            build.AddContentEntity(new cContentEntitySimple(@"export\meshes\locations\" + citySlashName + @"\foliage.fbin", this));
        }
    }   // cContentEntityLocation
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // cContentEntityLocation
