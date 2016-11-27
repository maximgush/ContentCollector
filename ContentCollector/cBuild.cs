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
        private static cBuild m_instance = new cBuild();
        public  static cBuild Instance() { return m_instance;}

        private string m_projectPath = "";
        
        private string m_svnURL = "";
        private int m_lastRevision = -1;

        private Dictionary<string, IContentEntity> m_contentDictionary = new Dictionary<string, IContentEntity>();
        private List<IContentEntity> m_queryContentEntitiesToParse = new List<IContentEntity>();

        [XmlArrayItem("UserInput_Mouse", Type = typeof(сTestEvent_UserInput_Mouse))]
        [XmlArrayItem("UserInput_Keyboard", Type = typeof(сTestEvent_UserInput_Keyboard))]
        [XmlArrayItem("ExecuteScript", Type = typeof(сTestEvent_ExecuteScript))]
        [XmlArrayItem("ProgramMessage", Type = typeof(сTestEvent_ProgramMessage_Info))]
        public List<cTestEvent> Events {
            get
            {
                List<cTestEvent> events = new List<cTestEvent>();
                events.Add(new сTestEvent_UserInput_Mouse());
                events.Add(new сTestEvent_ExecuteScript());
                return events;

            }
        }

//         [XmlArray]
//         [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]
//         [XmlArrayItem("cContentEntitySimple", Type = typeof(cContentEntitySimple))]
//         public List<cContentEntitySimple> Entities
//         {
//             get
//             {
//                 List<cContentEntitySimple> events = new List<cContentEntitySimple>();
//                 events.Add(new cContentEntitySimple());
//                 return events;
// 
//             }
//         }

//          [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]
//          [XmlArrayItem("cContentEntityPlayerCar", Type = typeof(cContentEntityPlayerCar))]
//          public List<cContentEntityGameTypesIni> ToSerialize
//          {
//              get
//              {
//                  List<cContentEntityGameTypesIni> toSerialize = new List<cContentEntityGameTypesIni>();
//                  toSerialize.Add(new cContentEntityGameTypesIni());
//                  return toSerialize;
//              }
//          }

/*        public class ContentDictionaryItem
        {
            [XmlAttribute]
            public string Name;
            
            [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]
            [XmlArrayItem("cContentEntityPlayerCar", Type = typeof(cContentEntityPlayerCar))]
            public IContentEntity Entity;

            //[XmlAttribute]
            //public String Entity;
        }

        [XmlArrayItem("cContentEntityGameTypesIni", Type = typeof(cContentEntityGameTypesIni))]
        [XmlArrayItem("cContentEntityPlayerCar", Type = typeof(cContentEntityPlayerCar))]
        public Dictionary<string, IContentEntity> Content
        {
            get { return m_contentDictionary; }
        }
 */
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddRootContentEntity()
        {
            AddContentEntity(typeof(cContentEntitySimple), "GameType.ini", "GameType.ini", null, true);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddContentEntity(System.Type entityType, string name, string fileName, IContentEntity parent, bool isRoot = false)
        {            
            if (m_contentDictionary.ContainsKey(name))
            {
                IContentEntity entity = m_contentDictionary[name];
                if (entity.GetType() != entityType)
                    MessageBox.Show("Элемент с таким именем уже существует под другим типом!");

                entity.AddParentContentEntity(parent);
            }
            else
            {
                IContentEntity entity = (IContentEntity)Activator.CreateInstance(entityType);
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
            StreamWriter stream = new StreamWriter(xmlFileName);

            var serializer = new XmlSerializer(typeof(cBuild));                       
            serializer.Serialize(stream, this);
/*
            var serializer2 = new XmlSerializer(typeof(List<String>));
            serializer2.Serialize(stream, ToSerialize);

            XmlSerializer serializer = new XmlSerializer(typeof(ContentDictionaryItem[]), new XmlRootAttribute() { ElementName = "ContentDictionaryItems" });
            serializer.Serialize(stream, m_contentDictionary.Select(kv => new ContentDictionaryItem() { Name = kv.Key, Entity = kv.Value }).ToArray());
*/
            stream.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Deserialize(string xmlFileName)
        {            
            StreamReader stream = new StreamReader(xmlFileName);
            //XmlSerializer serializer = new XmlSerializer(typeof(ContentDictionaryItem[]), new XmlRootAttribute() { ElementName = "ContentDictionaryItems" });
          //  m_contentDictionary = ((ContentDictionaryItem[])serializer.Deserialize(stream)).ToDictionary(i => i.Name, i => i.Entity);

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
