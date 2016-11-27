using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    class cContentEntityMission : cContentEntitySimple
    {
        public cContentEntityMission()
        {
            EntityType = eContentEntityTypes.cetMission;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse()
        {
//          AUTOMATION
//         file.write ("data/physics/devices/Device" + number + "/device.ini" + "\n")
//         file.write ("export/meshes/devices/Device" + number + "/device_p.hkx" + "\n")
//         file.write ("export/gfxlib/devices/Device" + number + "/device.n2" + "\n")
//         n2_search.main(file, global_config.app_dir + "export/gfxlib/devices/Device" + number + "/device.n2", languages, lang_associations)
//         
//         addIni = "data/gamedata/devices/Device" + number + "/device.ini"
//         if os.path.exists(global_config.app_dir + addIni):
//             file.write (addIni + "\n") 

            // TODO:
            // Распарсить mission.xml

            // lua-script
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetSimple, , , this);

            // rulesControl.lua
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetSimple, , , , this);

            // Video
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetSimple, , , this);

            // ActiveZones
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetSimple, , , this);

            // Locations
            // cBuild.Instance().AddContentEntity(eContentEntityTypes.cetLocation, , , this);

            // CarModel
            // cBuild.Instance().AddContentEntity(eContentEntityTypes.cetPlayerCar, , , this);

            // Device
            // cBuild.Instance().AddContentEntity(eContentEntityTypes.cetDevice, , , this);


            // TODO:
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityMission
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
