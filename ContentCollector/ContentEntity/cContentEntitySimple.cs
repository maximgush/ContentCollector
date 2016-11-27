using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ContentCollector
{
    class cContentEntitySimple : IContentEntity
    {
        [XmlAttribute]
        private string m_name = "";

        [XmlAttribute]
        private string m_fileName = "";

        [XmlAttribute]
        private eContentEntityTypes m_entityType = eContentEntityTypes.cetSimple;
        
        private bool m_isRoot;

        private List<IContentEntity> m_childContentEntities = new List<IContentEntity>();
        private List<IContentEntity> m_parentContentEntities = new List<IContentEntity>();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntitySimple()
        {
            m_entityType = eContentEntityTypes.cetSimple;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Name
        {
            get { return m_name; } 
            set { m_name = value;}
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string FileName
         {
            get { return m_fileName; } 
            set { m_fileName = value;}
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public eContentEntityTypes EntityType
        {
            get { return m_entityType; }
            set { m_entityType = value; }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool IsRoot
        {
            get { return m_isRoot; }
            set { m_isRoot = value; }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddChildContentEntity(IContentEntity entity)    { m_childContentEntities.Add(entity); }
        public void RemoveChildContentEntity(IContentEntity entity) { m_childContentEntities.RemoveAll(entity.Equals); }

        public void AddParentContentEntity(IContentEntity entity) { m_parentContentEntities.Add(entity); }
        public void RemoveParentContentEntity(IContentEntity entity) { m_parentContentEntities.RemoveAll(entity.Equals); }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveYouselfFromChildContentEntities()
        {
            foreach (IContentEntity entity in m_childContentEntities)
            {
                entity.RemoveParentContentEntity(this);
            }
            m_childContentEntities.Clear();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveYouselfFromParentContentEntities()
        {
            foreach (IContentEntity entity in m_parentContentEntities)
            {
                entity.RemoveChildContentEntity(this);
            }
            m_parentContentEntities.Clear();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HasParentEntities() { return m_parentContentEntities.Count > 0; }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Parse() {}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Serialize() { /* TODO */ }
        public void DeSerialize() { /* TODO */ }
    }   // сContentSimpleEntity
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
