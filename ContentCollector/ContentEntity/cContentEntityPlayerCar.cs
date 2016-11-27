using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    class cContentEntityPlayerCar : cContentEntitySimple
    {
        public cContentEntityPlayerCar()
        {
            EntityType = eContentEntityTypes.cetPlayerCar;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            // CarProperty
            cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarPropretyINI, Name + @"_CarProperty.ini", @"data/gamedata/cars/Car" + Name + @"\CarProperty.ini", this);

            // p_player_setup.ini
            cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarPropretyINI, Name + @"_p_player_setup.ini", @"data/physics/cars/" + Name + @"\p_player_setup.ini", this);

            // tuning
            cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarTuningXML, Name + @"_tuning.xml", @"data/gamedata/cars/Car" + Name + @"\tuning.xml", this);

            // n2
            // TODO:
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
