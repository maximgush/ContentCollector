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
    public class cContentEntityLocationPstatic : cContentEntitySimple
    {
        public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string[] words = Name.Split('\\');
            string locationPstaticFileName = words[words.Length-1];

            // Запускаем конвертер физики
            Utils.RunProcess(build.GetManglePath(@"bin\win32\buildStaticPLevels.exe"), build.GetManglePath(Name));
            build.AddContentEntity(typeof(cContentEntitySimple), @"data\physics\levels\" + locationPstaticFileName + ".mopp", this);
            build.AddContentEntity(typeof(cContentEntitySimple), @"data\physics\levels\" + locationPstaticFileName + ".sg", this);      
        }
    }   // cContentEntityLocationPstatic
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // cContentEntityLocation
