﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ContentCollector
{
    public class cContentEntityMission : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityMission(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(build.GetManglePath(Name));

            #region lua-script
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("LuaScript"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(new cContentEntitySimple(node.Attributes["File"].Value, this));
            }
            #endregion    

            #region ActiveZones
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("ActiveZones"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(new cContentEntitySimple(node.Attributes["File"].Value, this));
            }
            #endregion

            #region MissionObjects
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("MissionObjects"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(new cContentEntitySimple(node.Attributes["File"].Value, this));
            }
            #endregion

            #region Tips
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Tip"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                    build.AddContentEntity(new cContentEntitySimple(node.Attributes["File"].Value, this));
            }
            #endregion

            #region RulesControl
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Task"))
            {
                if (node.Attributes != null && node.Attributes["RulesControlScript"] != null)
                {
                    string[] scripts = node.Attributes["RulesControlScript"].Value.Split(';');
                    foreach (string script in scripts)
                    {
                        build.AddContentEntity(new cContentEntityRulesControl(script, this));
                    }
                    
                }
            }
            #endregion 

            #region Video
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Video"))
            {
                if (node.Attributes != null && node.Attributes["File"] != null)
                {
                    string videoFile = node.Attributes["File"].Value;
                    videoFile = videoFile.Replace(".avi", ".caf");
                    build.AddContentEntity(new cContentEntitySimple(videoFile, this));
                }
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
                    build.AddContentEntity(new cContentEntityLocation(cityName + '\\' + node.InnerText, this, cityName, node.InnerText));
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
                        build.AddContentEntity(new cContentEntityDevice(device, this));
                }                    
            }
            #endregion 

            #region TrafficCarsXml
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("Mission"))
            {
                if (node.Attributes != null && node.Attributes["TrafficCarsXml"] != null)
                    build.AddContentEntity(new cContentEntityTrafficCarsXml(node.Attributes["TrafficCarsXml"].Value, this));
            }
            #endregion
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityMission
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
