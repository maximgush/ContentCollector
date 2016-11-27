using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    public class cContentEntityPlayerCar : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // CarProperty
//            cBuild.Instance().AddContentEntity(typeof(cContentEntityCarProperty), Name + @"_CarProperty.ini", @"data/gamedata/cars/Car" + Name + @"\CarProperty.ini", this);

            // p_player_setup.ini
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarPropretyINI, Name + @"_p_player_setup.ini", @"data/physics/cars/" + Name + @"\p_player_setup.ini", this);

            // tuning
            //cBuild.Instance().AddContentEntity(eContentEntityTypes.cetCarTuningXML, Name + @"_tuning.xml", @"data/gamedata/cars/Car" + Name + @"\tuning.xml", this);

            // n2
            // TODO:
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
