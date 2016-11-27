using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ContentCollector
{
    public class cContentEntitySimple : IContentEntity
    {
        private string m_name = "";

        private string m_fileName = "";

        private bool m_isRoot = false;

        private List<IContentEntity> m_childContentEntities = new List<IContentEntity>();
        private List<IContentEntity> m_parentContentEntities = new List<IContentEntity>();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntitySimple()
        {
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [XmlAttribute]
        public string Name
        {
            get { return m_name; } 
            set { m_name = value;}
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [XmlAttribute]
        public string FileName
         {
            get { return m_fileName; } 
            set { m_fileName = value;}
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<IContentEntity> ParentContentEntities
        {
            get { return m_parentContentEntities; } 
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<IContentEntity> ChildContentEntities
        {
            get { return m_childContentEntities; }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [XmlAttribute]
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
