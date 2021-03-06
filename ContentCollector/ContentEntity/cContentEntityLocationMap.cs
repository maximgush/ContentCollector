﻿using System;
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
        public cContentEntityLocationMap(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // Мап-конвертер
            Utils.RunProcess(build.GetManglePath(@"bin\win32\MapConverter.exe"), build.GetManglePath(Name));
            build.AddContentEntity(new cContentEntitySimple(Name.Replace(".map",".bmap"), this));
        }
    }   // cContentEntityLocationMap
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
