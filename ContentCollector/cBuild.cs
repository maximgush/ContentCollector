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
        private string mRepositoryURL = "";
        private int mLastBuildRevision = -1;
        private string mProductInternalName = "";

        private List<cContentEntitySimple> mRootEntities = new List<cContentEntitySimple>();

        private Dictionary<string, cContentEntitySimple> mContentDictionary = new Dictionary<string, cContentEntitySimple>();
        private ConcurrentQueue<cContentEntitySimple> mQueryContentEntitiesToParse = new ConcurrentQueue<cContentEntitySimple>();

        private SpinLock spLock = new SpinLock();

        public string ProjectPath { get { return mProjectPath; } set { mProjectPath = value; } }
        public string RepositoryURL { get { return mRepositoryURL; } }
        public int LastBuildRevision { get { return mLastBuildRevision; } }
        public string ProductInternalName { get { return mProductInternalName; } set { mProductInternalName = value; }}


        [XmlArrayItem("cContentEntitySimple", Type = typeof(cContentEntitySimple))]
        [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]
        [XmlArrayItem("cContentEntityPlayerCar", Type = typeof(cContentEntityPlayerCar))]
        [XmlArrayItem("cContentEntityTrafficCar", Type = typeof(cContentEntityTrafficCar))]
        [XmlArrayItem("cContentEntityParkingCar", Type = typeof(cContentEntityParkingCar))]
        [XmlArrayItem("cContentEntityTrafficCarsXml", Type = typeof(cContentEntityTrafficCarsXml))]
        [XmlArrayItem("cContentEntityCarProperty", Type = typeof(cContentEntityCarProperty))]
        [XmlArrayItem("cContentEntityDevice", Type = typeof(cContentEntityDevice))]
        [XmlArrayItem("cContentEntityMission", Type = typeof(cContentEntityMission))]
        [XmlArrayItem("cContentEntityLocation", Type = typeof(cContentEntityLocation))]
        [XmlArrayItem("cContentEntityLocationDB3", Type = typeof(cContentEntityLocationDB3))]
        [XmlArrayItem("cContentEntityLocationMap", Type = typeof(cContentEntityLocationMap))]
        [XmlArrayItem("cContentEntityLocationPstatic", Type = typeof(cContentEntityLocationPstatic))]
        [XmlArrayItem("cContentEntityN2", Type = typeof(cContentEntityN2))]
        [XmlArrayItem("cContentEntityTexture", Type = typeof(cContentEntityTexture))]
        [XmlArrayItem("cContentEntityLanguage", Type = typeof(cContentEntityLanguage))]
        [XmlArrayItem("cContentEntityHardCodeFiles", Type = typeof(cContentEntityHardCodeFiles))]
        [XmlArrayItem("cContentEntityHardCodeN2Files", Type = typeof(cContentEntityHardCodeN2Files))]
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
        public void AddRootContentEntity(System.Type entityType, string name)
        {
            if (mRootEntities.FindAll(x => x.Name == name).Count > 0)
            {
                MessageBox.Show("Root с именем " + name + " уже существует!");
                return;
            }
            
            cContentEntitySimple rootEntity = (cContentEntitySimple)Activator.CreateInstance(entityType);
            rootEntity.Name = name;
            mRootEntities.Add(rootEntity);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddContentEntity(System.Type entityType, string name, cContentEntitySimple parent, bool isRoot = false)
        {
            // TODO: Переделать этот костыль
            name = name.Replace("home:", "");
            name = name.Replace(":", "\\");
            name = name.Replace("/", "\\");
            name = name.Replace("\\\\","\\");
            name = name.Replace("\\\\", "\\");
#if MULTITHREADING
            bool lockTaken = false;
            try
            {
                spLock.Enter(ref lockTaken);
#endif
                if (mContentDictionary.ContainsKey(name))
                {
                    cContentEntitySimple entity = mContentDictionary[name];
                    if (entity.GetType() != entityType)
                        MessageBox.Show("Элемент с таким именем уже существует под другим типом!");

                    entity.AddParentContentEntity(parent);
                }
                else
                {
                    cContentEntitySimple entity = (cContentEntitySimple)Activator.CreateInstance(entityType);
                    entity.Name = name;
                    entity.AddParentContentEntity(parent);
                    parent.AddChildContentEntity(entity);

                    mContentDictionary.Add(entity.Name, entity);
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
            cContentEntitySimple entity = mContentDictionary[name];
            entity.RemoveYouselfFromChildContentEntities();
            entity.RemoveYouselfFromParentContentEntities();
            mContentDictionary.Remove(name);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Update(string[] changedFiles)
        {
            // Файлы которые были изменены, удаляют у дочернего контента ссылки на себя
            // Изменённые файлы пересобираются
            // После пересборки всех изменившихся файлов удаляем из контента все файлы на которые нет родительской ссылки (рекурсивно).

            foreach (var changingFileName in changedFiles)
            {
                string changingFileNameRelative = changingFileName.Substring(mProjectPath.Length);
                if (mContentDictionary.ContainsKey(changingFileNameRelative))
                {
                    cContentEntitySimple entity = mContentDictionary[changingFileNameRelative];
                    entity.RemoveYouselfFromChildContentEntities();
                    mQueryContentEntitiesToParse.Enqueue(entity);
                }
            }

            ParseContentEntitiesInQuery();

            List<string> entitiesWithoutParent = new List<string>();
            do
            {
                foreach (var name in entitiesWithoutParent)
                {
                    RemoveContentEntity(name);
                }

                foreach(var pair_name_entity in mContentDictionary)
                {
                    if (!pair_name_entity.Value.HasParentEntities())
                        entitiesWithoutParent.Add(pair_name_entity.Key);
                }
            }
            while (entitiesWithoutParent.Count > 0);
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
//                    while (events.Count >= 64) ;
                    cContentEntitySimple entity = null;
                    if (mQueryContentEntitiesToParse.TryDequeue(out entity))
                    {
                        var resetEvent = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(
                                                        arg =>
                                                        {
                                                            entity.Parse(this);
//                                                            resetEvent.Set();
                                                        });
//                        events.Add(resetEvent);
                    }
                }

                System.Threading.ThreadPool.GetMaxThreads(out maxThreads,out placeHolder);
                System.Threading.ThreadPool.GetAvailableThreads(out availThreads, out placeHolder);
                if (availThreads != maxThreads)
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(50));
//                WaitHandle.WaitAll(events.ToArray());
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
            StreamWriter stream = new StreamWriter(xmlFileName);

            var serializer = new XmlSerializer(typeof(cBuild));                       
            serializer.Serialize(stream, this);

            stream.Close();
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
            return ProjectPath + "\\" + path.Replace("(logic)", "");
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetRelativePath(string path)
        {
            return path.Replace(ProjectPath + "\\","");
        }
    }   // сBuild
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // ContentCollector
