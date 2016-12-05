﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ContentCollector
{
    public class cContentEntityCarProperty : cContentEntitySimple
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
//             AUTOMATION
//             properties = ConfigParser.RawConfigParser()            
//             properties.read(global_config.app_dir + "data/gamedata/cars/Car" + number + "/CarProperty.ini")
//             if properties.has_option('Humans', 'DriverAnimatedModel'):
//                 driver = properties.get('Humans', 'DriverAnimatedModel')[1:-1].split(',')                
//                 for driverPart in driver:
//                     part = (driverPart.split('=')[1])[1:-1]
//                     part = "export/gfxlib/" + part + ".n2"
//                     file.write(part + "\n")
//                     n2_search.main(file, global_config.app_dir + part, languages, lang_associations)            
            string val = "";
            string fileName = build.GetManglePath(Name);
            StringBuilder stringBuilder = new StringBuilder(255);

            GetPrivateProfileString(build.ProductInternalName, "cars", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();

            // [Common]
            // BaseSoundBankName = "BaseCar"
            GetPrivateProfileString("Common", "BaseSoundBankName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
            {
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\audio\" + val + ".xsb", this);
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\audio\" + val + ".xwb", this);
            }
            // [Common]
            // SoundBankName = "amaz"
            GetPrivateProfileString("Common", "SoundBankName", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
            {
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\audio\" + val + ".xsb", this);
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\audio\" + val + ".xwb", this);   
            }


            // [Common]
            // TuningConfigPath = "cars/Car01/tuning.xml"
            GetPrivateProfileString("Common", "TuningConfigPath", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(typeof(cContentEntitySimple),@"data\gamedata\" + val,this);

            // [Cameras]
            // CameraProfile="cars/Car01/cameras.xml"
            GetPrivateProfileString("Cameras", "CameraProfile", "", stringBuilder, 255, fileName);
            val = stringBuilder.ToString();
            if (val.Length > 0)
                build.AddContentEntity(typeof(cContentEntitySimple),@"data\gamedata\" + val,this);        
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сContentEntityPlayerCar
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
