#define MULTITHREADING

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ContentCollector
{
    [XmlRoot("cBuild")]
    public class cBuild
    {
        private string mProjectPath = "";        

        public List<string> Seasons = new List<string>() { "" };
        public List<string> Locales = new List<string>();
        public Dictionary<string, List<string>> LocaleAssociations = new Dictionary<string, List<string>>() { { "ru_RU", new List<string>() { "" } }};
        public List<string> Celebrations = new List<string>() { "" };

        private List<cContentEntitySimple> mRootEntities = new List<cContentEntitySimple>();

        private Dictionary<string, cContentEntitySimple> mContentDictionary = new Dictionary<string, cContentEntitySimple>();
        private ConcurrentQueue<cContentEntitySimple> mQueryContentEntitiesToParse = new ConcurrentQueue<cContentEntitySimple>();

        // Список всех файлов проекта (кэш для быстрой проверки существования файлов)
        HashSet<string> mAllFilesInProjectSet = new HashSet<string>();

        private SpinLock spLock = new SpinLock();

        public string ProjectPath
        {
            get { return mProjectPath; }
            set
            {
                mProjectPath = value;

                string path = GetManglePath(@"data\i18n\associations.txt");
                if (File.Exists(path))
                {
                    StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
                    while (!reader.EndOfStream)
                    {
                        string str = reader.ReadLine();
                        string[] locale_association = str.Split(';');
                        if (!LocaleAssociations.ContainsKey(locale_association[0]));
                             LocaleAssociations.Add(locale_association[0], new List<string>());
                        
                        LocaleAssociations[locale_association[0]].Add(locale_association[1]);
                    }
                    reader.Close();
                }
            }
        }

        [XmlArrayItem("cContentEntitySimple", Type = typeof(cContentEntitySimple))]
        [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]

        [XmlArrayItem("cContentEntityPlayerCar", Type = typeof(cContentEntityPlayerCar))]
        [XmlArrayItem("cContentEntityTrafficCar", Type = typeof(cContentEntityTrafficCar))]
        [XmlArrayItem("cContentEntityParkingCar", Type = typeof(cContentEntityParkingCar))]
        [XmlArrayItem("cContentEntityPedestrian", Type = typeof(cContentEntityPedestrian))]

        [XmlArrayItem("cContentEntityTrafficCarsXml", Type = typeof(cContentEntityTrafficCarsXml))]

        [XmlArrayItem("cContentEntityCarProperty", Type = typeof(cContentEntityCarProperty))]
        [XmlArrayItem("cContentEntityCarPhysicsProperty", Type = typeof(cContentEntityCarPhysicsProperty))]

        [XmlArrayItem("cContentEntityDevice", Type = typeof(cContentEntityDevice))]

        [XmlArrayItem("cContentEntityMission", Type = typeof(cContentEntityMission))]
        [XmlArrayItem("cContentEntityLocation", Type = typeof(cContentEntityLocation))]
        [XmlArrayItem("cContentEntityLocationDB3", Type = typeof(cContentEntityLocationDB3))]
        [XmlArrayItem("cContentEntityLocationMap", Type = typeof(cContentEntityLocationMap))]
        [XmlArrayItem("cContentEntityLocationPstatic", Type = typeof(cContentEntityLocationPstatic))]
        
        [XmlArrayItem("cContentEntityN2", Type = typeof(cContentEntityN2))]
        [XmlArrayItem("cContentEntityTextureTga", Type = typeof(cContentEntityTextureTga))]
        [XmlArrayItem("cContentEntityLanguage", Type = typeof(cContentEntityLanguage))]

        [XmlArrayItem("cContentEntityHardCodeFiles", Type = typeof(cContentEntityHardCodeFiles))]
        [XmlArrayItem("cContentEntityHardCodeFilesBinWin32", Type = typeof(cContentEntityHardCodeFilesBinWin32))]
        [XmlArrayItem("cContentEntityHardCodeFilesDataConfig", Type = typeof(cContentEntityHardCodeFilesDataConfig))]
        [XmlArrayItem("cContentEntityHardCodeN2Files", Type = typeof(cContentEntityHardCodeN2Files))]
        [XmlArrayItem("cContentEntityDirectory", Type = typeof(cContentEntityDirectory))]

        [XmlArrayItem("cContentEntityRulesControl", Type = typeof(cContentEntityRulesControl))]
        public List<cContentEntitySimple> Entities
        {
            get
            {
                List<cContentEntitySimple> entities = new List<cContentEntitySimple>();
                foreach (var pair_name_entity in mContentDictionary)
                {
                    entities.Add(pair_name_entity.Value);
                }

                return entities;
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddRootContentEntity(cContentEntitySimple entity)
        {
            if (mRootEntities.FindAll(x => x.Name == entity.Name).Count > 0)
            {
                MessageBox.Show("Root с именем " + entity.Name + " уже существует!");
                return;
            }

            mRootEntities.Add(entity);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddContentEntity(cContentEntitySimple entity)
        {
            string dictionaryName = entity.Name + "(" + entity.GetType().ToString() + ")";
#if MULTITHREADING
            bool lockTaken = false;
            try
            {
                spLock.Enter(ref lockTaken);
#endif
                if (mContentDictionary.ContainsKey(dictionaryName))
                {
                    cContentEntitySimple extistEntity = mContentDictionary[dictionaryName];
                    if (extistEntity.GetType() != entity.GetType())
                        MessageBox.Show("Элемент с таким именем уже существует под другим типом!");

                    extistEntity.ParentContentEntities.AddRange(entity.ParentContentEntities);
                }
                else
                {
                    mContentDictionary.Add(dictionaryName, entity);
                    mQueryContentEntitiesToParse.Enqueue(entity);
                }
#if MULTITHREADING
            }
            finally
            {
                if (lockTaken) spLock.Exit();
            }
#endif
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveContentEntity(string name)
        {
            // TODO: Здесь нужно удалить все файлы содержащие name, а не только name
            //cContentEntitySimple entity = mContentDictionary[name];
            //entity.RemoveYouselfFromChildContentEntities();
            //entity.RemoveYouselfFromParentContentEntities();
            //mContentDictionary.Remove(name);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Update(string[] changedFiles)
        {
            // Файлы которые были изменены, удаляют у дочернего контента ссылки на себя
            // Изменённые файлы пересобираются
            // После пересборки всех изменившихся файлов удаляем из контента все файлы на которые нет родительской ссылки (рекурсивно).

//             foreach (var changingFileName in changedFiles)
//             {
//                 string changingFileNameRelative = changingFileName.Substring(mProjectPath.Length);
//                 if (mContentDictionary.ContainsKey(changingFileNameRelative))
//                 {
//                     // TODO: здесь надо обработать все файлы содежащие в имени changingFileNameRelative, а не только changingFileNameRelative
//                     cContentEntitySimple entity = mContentDictionary[changingFileNameRelative];
//                     entity.RemoveYouselfFromChildContentEntities();
//                     mQueryContentEntitiesToParse.Enqueue(entity);
//                 }
//             }
// 
//             ParseContentEntitiesInQuery();
// 
//             List<string> entitiesWithoutParent = new List<string>();
//             do
//             {
//                 foreach (var name in entitiesWithoutParent)
//                 {
//                     RemoveContentEntity(name);
//                 }
// 
//                 foreach(var pair_name_entity in mContentDictionary)
//                 {
//                     if (!pair_name_entity.Value.HasParentEntities())
//                         entitiesWithoutParent.Add(pair_name_entity.Key);
//                 }
//             }
//             while (entitiesWithoutParent.Count > 0);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ParseContentEntitiesInQuery()
        {

#if MULTITHREADING
//*****************************************************
                int    maxThreads   = 0;
                int    placeHolder  = 0;
                int    availThreads = 0;

                System.Threading.ThreadPool.GetMaxThreads(out maxThreads,out placeHolder);
                System.Threading.ThreadPool.GetAvailableThreads(out availThreads, out placeHolder);
//*****************************************************
            while (mQueryContentEntitiesToParse.Count > 0 || availThreads != maxThreads)
            {
                while (mQueryContentEntitiesToParse.Count > 0)
                {
                    cContentEntitySimple entity = null;
                    if (mQueryContentEntitiesToParse.TryDequeue(out entity))
                    {
                        var resetEvent = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(
                                                        arg =>
                                                        {
                                                            entity.Parse(this);
                                                        });
                    }
                }

                System.Threading.ThreadPool.GetMaxThreads(out maxThreads,out placeHolder);
                System.Threading.ThreadPool.GetAvailableThreads(out availThreads, out placeHolder);
                if (availThreads != maxThreads)
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }            
#else
            while (mQueryContentEntitiesToParse.Count > 0)
            {
                cContentEntitySimple entity = null;
                if (mQueryContentEntitiesToParse.TryDequeue(out entity))
                    entity.Parse(this);
            }
#endif
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Rebuild()
        {
            BuildAllFilesInProjectSet();

            foreach (var pair_name_entity in mContentDictionary)
            {
                cContentEntitySimple entity = pair_name_entity.Value;
                entity.RemoveYouselfFromChildContentEntities();
                entity.RemoveYouselfFromParentContentEntities();
            }

            mContentDictionary.Clear();

            foreach (var rootEntity in mRootEntities)
            {
                mContentDictionary.Add(rootEntity.Name, rootEntity);
                mQueryContentEntitiesToParse.Enqueue(rootEntity);   
            }            

            ParseContentEntitiesInQuery();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Serialize(string xmlFileName)
        {
/*
            StreamWriter stream = new StreamWriter(xmlFileName);

            var serializer = new XmlSerializer(typeof(cBuild));                       
            serializer.Serialize(stream, this);

            stream.Close();
 */ 
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Deserialize(string xmlFileName)
        {            
            StreamReader stream = new StreamReader(xmlFileName);
            var serializer = new XmlSerializer(typeof(cBuild));
            //XmlSerializer serializer = new XmlSerializer(typeof(ContentDictionaryItem[]), new XmlRootAttribute() { ElementName = "ContentDictionaryItems" });
          //  m_contentDictionary = ((ContentDictionaryItem[])serializer.Deserialize(stream)).ToDictionary(i => i.Name, i => i.Entity);

            stream.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> GetContentList()
        {
            List<string> contentList = new List<string>();
            foreach (var pair_name_entity in mContentDictionary)
            {
                if (pair_name_entity.Value.FileName != null 
                    && pair_name_entity.Value.FileName.Length != 0)
                    contentList.Add(pair_name_entity.Value.FileName);
            }

            return contentList;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetManglePath(string path)
        {
            return ProjectPath + "\\" + path;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetRelativePath(string path)
        {
            return path.Replace(ProjectPath + "\\","");
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BuildAllFilesInProjectSet()
        {            
            foreach (string file in Directory.EnumerateFiles(ProjectPath, "*.*", SearchOption.AllDirectories))
            {
                string name = GetRelativePath(file).ToLower();
                mAllFilesInProjectSet.Add(name);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool ExistFileInProject(string path)
        {
            return mAllFilesInProjectSet.Contains(path);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // сBuild
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // ContentCollector
