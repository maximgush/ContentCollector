using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    public class cContentEntityDevice : cContentEntitySimple
    {
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // device.ini
            build.AddContentEntity(typeof(cContentEntitySimple), @"data/physics/devices/Device" + Name + @"/device.ini", this);

            // device_p.hkx
            build.AddContentEntity(typeof(cContentEntitySimple), @"export/meshes/devices/Device" + Name + @"/device_p.hkx", this);

            // device.n2
            build.AddContentEntity(typeof(cContentEntityHardCodeN2Files), @"export/meshes/devices/Device" + Name + @"/device.n2", this);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityDevice
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
