﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ContentCollector
{
    public class cContentEntityDirectory : cContentEntitySimple
    {
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string path = build.GetManglePath(Name);
            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                string name = build.GetRelativePath(file);
                build.AddContentEntity(typeof(cContentEntitySimple), name, this);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityDirectory
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
