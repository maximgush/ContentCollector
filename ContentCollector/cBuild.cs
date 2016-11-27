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
    class cBuild
    {
        private static cBuild m_instance = new cBuild();
        public  static cBuild Instance() { return m_instance;}

        private string m_projectPath = "";
        
        private string m_svnURL = "";
        private int m_lastRevision = -1;
        
        private Dictionary<string, IContentEntity> m_contentDictionary = new Dictionary<string, IContentEntity>();
        private List<IContentEntity> m_queryContentEntitiesToParse = new List<IContentEntity>();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddRootContentEntity()
        {
            AddContentEntity(eContentEntityTypes.cetGameTypesIni, "GameType.ini", "GameType.ini", null, true);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddContentEntity(eContentEntityTypes contentType, string name, string fileName, IContentEntity parent, bool isRoot = false)
        {
            if (m_contentDictionary.ContainsKey(name))
            {
                IContentEntity entity = m_contentDictionary[name];
                if (entity.EntityType != contentType)
                    MessageBox.Show("Элемент с таким именем уже существует под другим типом!");

                entity.AddParentContentEntity(parent);
            }
            else
            {
                IContentEntity entity = сFactoryContentEntity.Create(contentType);
                entity.Name = name;
                entity.FileName = fileName;
                entity.AddParentContentEntity(parent);
                entity.IsRoot = isRoot;
                
                m_contentDictionary.Add(entity.Name, entity);
                m_queryContentEntitiesToParse.Add(entity);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveContentEntity(string name)
        {
            IContentEntity entity = m_contentDictionary[name];
            entity.RemoveYouselfFromChildContentEntities();
            entity.RemoveYouselfFromParentContentEntities();
            m_contentDictionary.Remove(name);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Update(string[] changedFiles)
        {
            // Файлы которые были изменены, удаляют у дочернего контента ссылки на себя
            // Изменённые файлы пересобираются
            // После пересборки всех изменившихся файлов удаляем из контента все файлы на которые нет родительской ссылки (рекурсивно).

            foreach (var changingFileName in changedFiles)
            {
                string changingFileNameRelative = changingFileName.Substring(m_projectPath.Length);
                if (m_contentDictionary.ContainsKey(changingFileNameRelative))
                {
                    IContentEntity entity = m_contentDictionary[changingFileNameRelative];
                    entity.RemoveYouselfFromChildContentEntities();
                    m_queryContentEntitiesToParse.Add(entity);
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

                foreach(var pair_name_entity in m_contentDictionary)
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
            while (m_queryContentEntitiesToParse.Count > 0)
            {
                m_queryContentEntitiesToParse[0].Parse();
                m_queryContentEntitiesToParse.RemoveAt(0);
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Rebuild()
        {
            foreach (var pair_name_entity in m_contentDictionary)
            {
                IContentEntity entity = pair_name_entity.Value;
                entity.RemoveYouselfFromChildContentEntities();
                entity.RemoveYouselfFromParentContentEntities();
            }

            m_contentDictionary.Clear();

            AddRootContentEntity();

            ParseContentEntitiesInQuery();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Serialize(string xmlFileName)
        {
            var ser = new XmlSerializer(typeof(cBuild));
            StreamWriter stream = new StreamWriter(xmlFileName);
            ser.Serialize(stream, this);
            stream.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Deserialize(string xmlFileName)
        {            
            StreamReader stream = new StreamReader(xmlFileName);
            XmlSerializer serializer = new XmlSerializer(typeof(cBuild));
            cBuild wrapper = (cBuild)serializer.Deserialize(stream);
            this.m_contentDictionary = wrapper.m_contentDictionary;
            stream.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> GetContentList()
        {
            List<string> contentList = new List<string>();
            foreach (var pair_name_entity in m_contentDictionary)
            {
                if (pair_name_entity.Value.FileName.Length != 0)
                    contentList.Add(pair_name_entity.Value.FileName);
            }

            return contentList;
        }
    }   // сBuild
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // ContentCollector
