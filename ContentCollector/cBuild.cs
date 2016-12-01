using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private List<cContentEntitySimple> mQueryContentEntitiesToParse = new List<cContentEntitySimple>();

        public string ProjectPath { get { return mProjectPath; } set { mProjectPath = value; } }
        public string RepositoryURL { get { return mRepositoryURL; } }
        public int LastBuildRevision { get { return mLastBuildRevision; } }
        public string ProductInternalName { get { return mProductInternalName; } set { mProductInternalName = value; }}


        [XmlArrayItem("cContentEntitySimple", Type = typeof(cContentEntitySimple))]
        [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]
        [XmlArrayItem("cContentEntityPlayerCar", Type = typeof(cContentEntityPlayerCar))]
        [XmlArrayItem("cContentEntityMission", Type = typeof(cContentEntityMission))]
        [XmlArrayItem("cContentEntityLocation", Type = typeof(cContentEntityLocation))]
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
        public void AddRootContentEntity(System.Type entityType, string name, string fileName)
        {
            if (mRootEntities.FindAll(x => x.Name == name).Count > 0)
            {
                MessageBox.Show("Root с именем " + name + " уже существует!");
                return;
            }

            cContentEntitySimple rootEntity = (cContentEntitySimple)Activator.CreateInstance(entityType);
            rootEntity.Name = name;
            rootEntity.FileName = fileName;
            rootEntity.IsRoot = true;
            mRootEntities.Add(rootEntity);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddContentEntity(System.Type entityType, string name, string fileName, cContentEntitySimple parent, bool isRoot = false)
        {            
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
                entity.FileName = fileName != null ? ProjectPath + "\\" + fileName : null;
                entity.AddParentContentEntity(parent);
                parent.AddChildContentEntity(entity);
                entity.IsRoot = isRoot;
                
                mContentDictionary.Add(entity.Name, entity);
                mQueryContentEntitiesToParse.Add(entity);
            }
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
                    mQueryContentEntitiesToParse.Add(entity);
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
                    if (!pair_name_entity.Value.HasParentEntities() && !pair_name_entity.Value.IsRoot)
                        entitiesWithoutParent.Add(pair_name_entity.Key);
                }
            }
            while (entitiesWithoutParent.Count > 0);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ParseContentEntitiesInQuery()
        {
            while (mQueryContentEntitiesToParse.Count > 0)
            {
                mQueryContentEntitiesToParse[0].Parse(this);
                mQueryContentEntitiesToParse.RemoveAt(0);
            }
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
                mQueryContentEntitiesToParse.Add(rootEntity);   
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
                if (pair_name_entity.Value.FileName.Length != 0)
                    contentList.Add(pair_name_entity.Value.FileName);
            }

            return contentList;
        }
    }   // сBuild
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // ContentCollector
