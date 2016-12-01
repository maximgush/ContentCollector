using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;

namespace ContentCollector
{
    public class cContentEntityMission : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(this.FileName);

            #region lua-script
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("LuaScript"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(typeof(cContentEntitySimple), node.Attributes["File"].Value, node.Attributes["File"].Value, this);
            }
            #endregion    

            #region ActiveZones
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("ActiveZones"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(typeof(cContentEntitySimple), node.Attributes["File"].Value, node.Attributes["File"].Value, this);
            }
            #endregion

            #region Tips
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Tip"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(typeof(cContentEntitySimple), node.Attributes["File"].Value, node.Attributes["File"].Value, this);
            }
            #endregion

            #region RulesControl
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Task"))
            {
                if (node.Attributes != null && node.Attributes["RulesControl"] != null)
                {
                    string devices = node.Attributes["RulesControl"].Value;
                    build.AddContentEntity(typeof(cContentEntityDevice), node.Attributes["RulesControl"].Value, node.Attributes["RulesControl"].Value, this);
                }
            }
            #endregion 

            #region Video
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Video"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(typeof(cContentEntitySimple), node.Attributes["File"].Value, node.Attributes["File"].Value, this);
            }
            #endregion

            #region Locations
            string cityName = null;
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Locations"))
            {
                if (node.Attributes != null && node.Attributes["CityName"] != null)
                    cityName = node.Attributes["CityName"].Value;
            }

            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Location"))
            {
                if (node.Attributes != null)
                    build.AddContentEntity(typeof(cContentEntityLocation), cityName + '\\' + node.InnerText, null, this);
            }
            #endregion

            #region Devices
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Mission"))
            {
                if (node.Attributes != null && node.Attributes["device"] != null)
                {
                    string devices = node.Attributes["device"].Value;
                    string[] deviceArrayStrings = devices.Split(',');
                    foreach (string device in deviceArrayStrings)
                        build.AddContentEntity(typeof(cContentEntityDevice), device, null, this);
                }                    
            }
            #endregion 

            #region TrafficCarsXml
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Mission"))
            {
                if (node.Attributes != null && node.Attributes["TrafficCarsXml"] != null)
                    build.AddContentEntity(typeof(cContentEntityTrafficCarsXml), node.Attributes["TrafficCarsXml"].Value, node.Attributes["TrafficCarsXml"].Value, this);
            }
            #endregion  
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityMission
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
