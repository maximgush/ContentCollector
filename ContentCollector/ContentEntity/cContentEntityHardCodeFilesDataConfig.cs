﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityHardCodeFilesDataConfig : cContentEntityHardCodeFiles
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityHardCodeFilesDataConfig(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string IntermediateDir { get { return "data\\config\\"; } }
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector