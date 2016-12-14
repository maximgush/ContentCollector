using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace ContentCollector
{
    public class cContentEntityGameTypesIni : cContentEntitySimple
    {
        public string BuildType = "";

        public cContentEntityGameTypesIni()
        {
        }

        override public string FileName
        {
            get { return null; }
        }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        private bool GetBoolValue(string key, bool defaultValue = false)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            GetPrivateProfileString(BuildType, key, defaultValue.ToString(), stringBuilder, 255, Name);
            return stringBuilder.ToString().ToLower() == "true";
        }

        private string GetStringValue(string key, string defaultValue = "")
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            GetPrivateProfileString(BuildType, key, defaultValue, stringBuilder, 255, Name);
            return stringBuilder.ToString();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            BuildType = build.ProductInternalName;

            StringBuilder stringBuilder = new StringBuilder(255);

            string GUI_Folder_Name = "pro";                 
            
            build.Seasons.Add("winter");
            if (GetBoolValue("SeasonAutumn", false))
                build.Seasons.Add("autumn");

            string patches_AdditionalContent = GetStringValue("Patches_AdditionalContent", "");
            if (patches_AdditionalContent != "")
            {
                foreach (var celebration in patches_AdditionalContent.Split(';'))
                    build.Celebrations.Add(celebration); 
            }         

            #region PlayerCars
                GetPrivateProfileString(BuildType, "cars", "", stringBuilder, 255, Name);
                string cars = stringBuilder.ToString();
                cars = cars.Trim(new char[] {'[', ']', ' '});
                string[] carNumbers = cars.Split(',');

                foreach (var carNumber in carNumbers)
                    build.AddContentEntity(new cContentEntityPlayerCar("PlayerCar\\cars\\car" + carNumber, this, "car" + carNumber));
            #endregion

            #region Missions
            {
                GetPrivateProfileString(BuildType, "MissionsSubdir", "n/a", stringBuilder, 255, Name);
                string missionSubdirPath = @"data\missions" + (stringBuilder.ToString() != "n/a" ? @"\" + stringBuilder.ToString() : "");

                int index = 1;
                while (true)
                {
                    GetPrivateProfileString(BuildType, "mission_" + index.ToString(), "", stringBuilder, 255, Name);
                    string mission = stringBuilder.ToString();

                    if (mission == "")
                        break;
                    
                    string missionPath = missionSubdirPath + @"\" + mission + ".xml";
                    build.AddContentEntity(typeof(cContentEntityMission), missionPath, this);

                    index++;
                }  
            }

            #endregion

            #region Location_db3
            {
                int index = 1;
                while (true)
                {
                    GetPrivateProfileString(BuildType, "Location_db_" + index.ToString(), "", stringBuilder, 255, Name);
                    string locationDb3 = stringBuilder.ToString();

                    if (locationDb3 == "")
                        break;

                    build.AddContentEntity(typeof(cContentEntityLocation), locationDb3.Replace(".db3",""), this);

                    index++;
                }  
            }
            #endregion

            #region LocationGuiImage
            {
                GetPrivateProfileString(BuildType, "GUI_Folder_Name", "", stringBuilder, 255, Name);
                string guiFolderName = stringBuilder.ToString();

                int index = 1;                
                while (true)
                {
                    GetPrivateProfileString(BuildType, "Location_" + index.ToString(), "", stringBuilder, 255, Name);
                    string locationGuiImage = stringBuilder.ToString();

                    if (locationGuiImage == "")
                        break;

                    string icon = @"data\gui\" + guiFolderName + @"\imagesets\locations\" + locationGuiImage + "_icon.png";
                    string detail = @"data\gui\" + guiFolderName + @"\imagesets\locations\" + locationGuiImage + "_detail.png";
                    string mini = @"data\gui\" + guiFolderName + @"\imagesets\locations\" + locationGuiImage + "_mini.png";
                    build.AddContentEntity(typeof(cContentEntitySimple), icon, this);
                    build.AddContentEntity(typeof(cContentEntitySimple), detail, this);
                    build.AddContentEntity(typeof(cContentEntitySimple), mini, this);

                    index++;
                }   
            }
            #endregion

            #region Languages
            {
                GetPrivateProfileString(BuildType, "Language", "", stringBuilder, 255, Name);
                string languages = stringBuilder.ToString();
                languages = languages.Trim(new char[] {'[', ']', ' '});
                string[] languageArray = languages.Split(',');

                foreach (var language in languageArray)
                {
                    build.AddContentEntity(typeof(cContentEntityLanguage), language, this);
                    build.Locales.Add(language.Replace('-','_'));
                }
            }
            #endregion

            #region GUI_Folder
            {
                GetPrivateProfileString(BuildType, "GUI_Folder_Name", "", stringBuilder, 255, Name);
                string guiFolderName = stringBuilder.ToString();

                string path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\imagesets\");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\imagesets\indicators");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                foreach (var carNumber in carNumbers)
                {
                    path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\imagesets\cars\car" + carNumber.ToString());
                    foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        string name = build.GetRelativePath(file);
                        build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                    }
                }

                path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\fonts\");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\looknfeel\");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\layouts\");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\schemes\");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                path = build.GetManglePath(@"data\gui\" + guiFolderName + @"\configs\");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }

                path = build.GetManglePath(@"data\gui\Common");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    string name = build.GetRelativePath(file);
                    build.AddContentEntity(typeof(cContentEntitySimple), name, this);
                }  
            }
            #endregion

            #region HardCodeFiles
            {
                string directory = Path.GetDirectoryName(Name) + "\\";

                build.AddContentEntity(typeof(cContentEntityHardCodeFiles), directory + @"hard_code.txt",this);
                build.AddContentEntity(typeof(cContentEntityHardCodeN2Files), directory + @"hard_n2.txt",this);
                build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files.txt", this);
                build.AddContentEntity(typeof(cContentEntityHardCodeFilesDataConfig), directory + @"data_config_files.txt", this);


                if (BuildType == "Home")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_Home.txt", this);
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesDataConfig), directory + @"data_config_files_Home.txt", this);

                }

                if (BuildType == "HomeRu")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_HomeRu.txt", this);
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesDataConfig), directory + @"data_config_files_Home.txt", this);

                }

                if (BuildType == "CCD_Steam")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_CCD_Steam.txt", this);
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesDataConfig), directory + @"data_config_files_CCD_Steam.txt", this);
                }

                if (BuildType == "MGI")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_MGI.txt", this);
                    build.AddContentEntity(typeof(cContentEntityHardCodeFiles), directory + @"hard_code_MGI.txt",this);
                }

                if (BuildType == "Autobahn")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_Autobahn.txt", this);
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesDataConfig), directory + @"data_config_files_Autobahn.txt", this);
                }

                if (GetBoolValue("Simulator"))
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_Simulator.txt", this);
                }
                if (GetBoolValue("Net"))
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_Net.txt", this);
                }

                if (GetStringValue("Psychophysics","") == "full")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_Psychophysics.txt", this);
                }
                else if (GetStringValue("Psychophysics", "") == "lite")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_Psychophysics_lite.txt", this);
                }

                if (GetBoolValue("VlcCamera",false))
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_vlc.txt", this);
                }

                if (GetBoolValue("ffmpegCamera",false))
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFilesBinWin32), directory + @"win32_files_ffmpeg.txt", this);
                    
                }

                if (BuildType == "Home" || BuildType == "HomeRu" || BuildType == "CCD_Kids" || BuildType == "CCD_Steam")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFiles), directory + @"hard_code_Home.txt", this);
                }

                if (BuildType != "Home" && BuildType != "HomeRu" && BuildType != "KSI" && BuildType != "Autobahn" && BuildType != "CCD_Steam")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFiles), directory + @"hard_code_Pro.txt", this);
                }

                if (BuildType == "Autocrane_Net" || BuildType == "Autocrane")
                {
                    build.AddContentEntity(typeof(cContentEntityHardCodeFiles), directory + @"hard_code_Autocrane.txt", this);
                }

                if (BuildType.ToLower().IndexOf("crane") >= 0)
                {
                    build.AddContentEntity(typeof(cContentEntityDirectory), directory + @"data_gamedata_defender.txt", this);
                }                                                                                 

                //"data_scripts.lua_files.txt"
            }
            #endregion

            #region DirectXDependencies

            {
                string directXStr = GetStringValue("DirectX", "[11]");
                foreach (var directX_version in directXStr.TrimStart('[').TrimEnd(']').Split(','))
                {
                    build.AddContentEntity(typeof(cContentEntitySimple), @"bin\win32\d3d" + directX_version + "renderer.dll", this);
                    build.AddContentEntity(typeof(cContentEntitySimple), @"bin\win32\CEGUIDirect3D" + directX_version + "Renderer.dll", this);
                    build.AddContentEntity(typeof(cContentEntityDirectory), @"data\shaders\dx" + directX_version, this);
                }

                #region  shaders
                {
                    build.AddContentEntity(typeof(cContentEntityShadersXml), @"data\shaders\shaders.xml", this);                   
                }
                #endregion
            }
            #endregion

            #region OtherEntities
            {
                build.AddContentEntity(typeof(cContentEntitySimple),@"bin\win32\gui_" + GetStringValue(GUI_Folder_Name,"pro") + ".dll", this);

                string DefaultKeybindOverride = GetStringValue("DefaultKeybindOverride", @"n/a");
                if (DefaultKeybindOverride != @"n/a")
                    build.AddContentEntity(typeof(cContentEntitySimple), build.GetManglePath(@"data\config\" + DefaultKeybindOverride), this);

                build.AddContentEntity(typeof(cContentEntityDirectory), @"data\scripts", this);
                build.AddContentEntity(typeof(cContentEntityDirectory), @"data\profiles", this);

                build.AddContentEntity(typeof(cContentEntityDirectory), @"data\config\licenseplates", this);
                build.AddContentEntity(typeof(cContentEntityDirectory), @"data\config\ai", this);           

                build.AddContentEntity(typeof(cContentEntitySimple), @"data\i18n\associations.txt", this);
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\i18n\language.txt", this);

                build.AddContentEntity(typeof(cContentEntitySimple), @"data\tables\anims.xml", this);     
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\tables\blueprints.xml", this);   
  
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\scripts.lua\missions\pro_category_c\common.lua", this);   
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\scripts.lua\missions\pro_category_c\endcontrol.lua", this);   
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\scripts.lua\missions\pro_category_c\mocking.lua", this);   
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\scripts.lua\missions\pro_category_c\statemachine.lua", this);
                build.AddContentEntity(typeof(cContentEntitySimple), @"data\tables\fines\russia.xls", this);                   
                
                string rulesLocales = GetStringValue("Patches_AdditionalContent", "");
                if (rulesLocales != "")
                {
                    foreach (var rules in rulesLocales.Split(';'))
                        build.AddContentEntity(typeof(cContentEntitySimple), @"data\config\rules\" + rules + ".ini", this);
                }
                else
                    build.AddContentEntity(typeof(cContentEntitySimple), @"data\config\rules\ru_RU.ini", this);

                build.AddContentEntity(typeof(cContentEntityPassengers), @"data\config\passengers.ini", this);

                build.AddContentEntity(typeof(cContentEntityTrafficCarsXml), @"data\config\traffic_cars.xml", this);

                // HACK: Не знаю зачем здесь это, но старый скрипт на питоне забирал эти файлы
                string path = build.GetManglePath("data\\physics\\objects");
                foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    string name = build.GetRelativePath(file);
                    if  (Path.GetExtension(file) == ".ini")
                        build.AddContentEntity(typeof(cContentEntityPhysicsIni), name, this);
                }                
            }
            #endregion
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityGameTypesIni
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
