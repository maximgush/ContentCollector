using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace ContentCollector
{
    public class cContentEntityGameTypesIni : cContentEntitySimple
    {
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

        public override void Parse(cBuild build)
        {
            StringBuilder stringBuilder = new StringBuilder(255);

            #region PlayerCars
                GetPrivateProfileString(build.ProductInternalName, "cars", "", stringBuilder, 255, Name);
                string cars = stringBuilder.ToString();
                cars = cars.Trim(new char[] {'[', ']', ' '});
                string[] carNumbers = cars.Split(',');

                foreach (var carNumber in carNumbers)
                    build.AddContentEntity(typeof(cContentEntityPlayerCar), "cars\\car" + carNumber, this);
            #endregion

            #region Missions
            {
                GetPrivateProfileString(build.ProductInternalName, "MissionsSubdir", "n/a", stringBuilder, 255, Name);
                string missionSubdirPath = @"data\missions" + (stringBuilder.ToString() != "n/a" ? @"\" + stringBuilder.ToString() : "");

                int index = 1;
                while (true)
                {
                    GetPrivateProfileString(build.ProductInternalName, "mission_" + index.ToString(), "", stringBuilder, 255, Name);
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
                    GetPrivateProfileString(build.ProductInternalName, "Location_db_" + index.ToString(), "", stringBuilder, 255, Name);
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
                GetPrivateProfileString(build.ProductInternalName, "GUI_Folder_Name", "", stringBuilder, 255, Name);
                string guiFolderName = stringBuilder.ToString();

                int index = 1;                
                while (true)
                {
                    GetPrivateProfileString(build.ProductInternalName, "Location_" + index.ToString(), "", stringBuilder, 255, Name);
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
                GetPrivateProfileString(build.ProductInternalName, "Language", "", stringBuilder, 255, Name);
                string languages = stringBuilder.ToString();
                languages = languages.Trim(new char[] {'[', ']', ' '});
                string[] languageArray = languages.Split(',');

                foreach (var language in languageArray)
                    build.AddContentEntity(typeof(cContentEntityLanguage), language, this);
            }
            #endregion

            #region GUI_Folder
            {
                GetPrivateProfileString(build.ProductInternalName, "GUI_Folder_Name", "", stringBuilder, 255, Name);
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
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityGameTypesIni
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
