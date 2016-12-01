using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ContentCollector
{
    public class cContentEntitySimple
    {
        private string m_name = "";

        private string m_fileName = "";

        private bool m_isRoot = false;

        private ISet<cContentEntitySimple> m_childContentEntities = new HashSet<cContentEntitySimple>();
        private ISet<cContentEntitySimple> m_parentContentEntities = new HashSet<cContentEntitySimple>();
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
        public List<string> ParentContentEntities
        {
            get
            {
                List<string> parents = new List<string>(m_parentContentEntities.Select(elem => elem.Name).ToArray());
                return parents;
            } 
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> ChildContentEntities
        {
            get
            {
                List<string> childs = new List<string>(m_childContentEntities.Select(elem => elem.Name).ToArray());
                return childs;
            } 
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [XmlAttribute]
        public bool IsRoot
        {
            get { return m_isRoot; }
            set { m_isRoot = value; }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddChildContentEntity(cContentEntitySimple entity)    { m_childContentEntities.Add(entity); }
        public void RemoveChildContentEntity(cContentEntitySimple entity) { m_childContentEntities.Remove(entity); }

        public void AddParentContentEntity(cContentEntitySimple entity) { m_parentContentEntities.Add(entity); }
        public void RemoveParentContentEntity(cContentEntitySimple entity) { m_parentContentEntities.Remove(entity); }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveYouselfFromChildContentEntities()
        {
            foreach (cContentEntitySimple entity in m_childContentEntities)
            {
                entity.RemoveParentContentEntity(this);
            }
            m_childContentEntities.Clear();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveYouselfFromParentContentEntities()
        {
            foreach (cContentEntitySimple entity in m_parentContentEntities)
            {
                entity.RemoveChildContentEntity(this);
            }
            m_parentContentEntities.Clear();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Equals(cContentEntitySimple entity)
        {
            return this.Name.Equals(entity.Name);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HasParentEntities() { return m_parentContentEntities.Count > 0; }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Parse(cBuild build) {}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Serialize() { /* TODO */ }
        public void DeSerialize() { /* TODO */ }
    }   // сContentSimpleEntity
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
