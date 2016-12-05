using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;

namespace ContentCollector
{
    public class cContentEntityTrafficCarsXml : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
//          AUTOMATION
//          dom = xml.dom.minidom.parse(global_config.app_dir + "data\\config\\traffic_cars.xml")
//          for node in dom.getElementsByTagName('Pedestrian'):
//              n = node.getAttribute("name")    
//              r.write ('export/gfxlib/characters/pedestrians/' + n + '/human.n2 \n')        
//              n2_search.main(r, global_config.app_dir + 'export//gfxlib//characters//pedestrians//' + n + '//human.n2', languages, lang_associations)

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(build.GetManglePath(Name));

            #region Cars
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Car"))
            {
                string carName = node.Attributes["name"].Value;
                if (node.Attributes != null && carName != null)
                    build.AddContentEntity(typeof(cContentEntityTrafficCar), @"cars\" + carName, this);
            }
            #endregion

            #region Tram
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Tram"))
            {
                string carName = node.Attributes["name"].Value;
                if (node.Attributes != null && carName != null)
                    build.AddContentEntity(typeof(cContentEntityTrafficCar), @"cars\" + carName, this);
            }
            #endregion

            #region Pedestrians
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Pedestrian"))
            {
                string pedName = node.Attributes["name"].Value;

                if (node.Attributes != null && pedName != null)
                {
                    foreach (string humanN2 in new string[]{"human.n2","human_run.n2","human_idle.n2"})
                    {
                        build.AddContentEntity(typeof(cContentEntityN2), @"export/gfxlib/characters/pedestrians/" + pedName + @"/" + humanN2, this);   
                    }
                }
            }
            #endregion
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
