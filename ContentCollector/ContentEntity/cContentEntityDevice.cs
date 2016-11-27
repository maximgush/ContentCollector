using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    class cContentEntityDevice : cContentEntitySimple
    {
        public cContentEntityDevice()
        {
            EntityType = eContentEntityTypes.cetDevice;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse()
        {
//          AUTOMATION
//         file.write (@"data/physics/devices/Device" + Name + @"/device.ini" + "\n")
//         file.write (@"export/meshes/devices/Device" + Name + @"/device_p.hkx" + "\n")
//         file.write (@"export/gfxlib/devices/Device" + Name + @"/device.n2" + "\n")
//         n2_search.main(file, global_config.app_dir + "export/gfxlib/devices/Device" + number + "/device.n2", languages, lang_associations)
//         
//         addIni = "data/gamedata/devices/Device" + number + "/device.ini"
//         if os.path.exists(global_config.app_dir + addIni):
//             file.write (addIni + "\n") 

            string fileName; 

            // device.ini
            fileName = @"data/physics/devices/Device" + Name + @"/device.ini";
            cBuild.Instance().AddContentEntity(eContentEntityTypes.cetSimple, fileName, fileName, this);

            // device_p.hkx
            fileName = @"export/meshes/devices/Device" + Name + @"/device_p.hkx";
            cBuild.Instance().AddContentEntity(eContentEntityTypes.cetSimple, fileName, fileName, this);

            // device.n2
            fileName = @"export/meshes/devices/Device" + Name + @"/device.n2";
            cBuild.Instance().AddContentEntity(eContentEntityTypes.cetN2, fileName, fileName, this);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityDevice
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
