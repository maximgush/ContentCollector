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
    public class cContentEntityLocationMap : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // Мап-конвертер
            Utils.RunProcess(build.GetManglePath(@"bin\win32\MapConverter.exe"), build.GetManglePath(Name));
            build.AddContentEntity(typeof(cContentEntitySimple), Name.Replace(".map",".bmap"),Name.Replace(".map",".bmap"), this);          
        }
    }   // cContentEntityLocationMap
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
