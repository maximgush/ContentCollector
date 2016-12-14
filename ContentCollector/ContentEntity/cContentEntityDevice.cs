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
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityDevice(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // device.ini
            build.AddContentEntity(new cContentEntitySimple(@"data/physics/devices/Device" + Name + @"/device.ini", this));

            // device_p.hkx
            build.AddContentEntity(new cContentEntitySimple(@"export/meshes/devices/Device" + Name + @"/device_p.hkx", this));

            // device.n2
            build.AddContentEntity(new cContentEntityHardCodeN2Files(@"export/meshes/devices/Device" + Name + @"/device.n2", this));
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityDevice
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
