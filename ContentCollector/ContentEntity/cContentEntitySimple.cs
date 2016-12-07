using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace ContentCollector
{
    public class cContentEntitySimple
    {
        private string m_name = "";

        private string m_fileName = "";

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
        public virtual string FileName
        {
            get { return Name.Replace("(logic)",""); }
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
        public virtual void AddEntityVariants(cBuild build)
        {
            string fileName = Path.GetFileName(Name);
            string directory = Path.GetDirectoryName(build.GetManglePath(build.GetRelativePath(Name))) + "\\";

            // Выделяем расширение файла
            // Это делается таким сложным способом (вместо File.GetExtension(...)) поскольку у нас есть файлы с несколькими расширениями
            // Например image.tga.dds
            int indexDot = fileName.IndexOf('.');
            if (indexDot < 0)
                return;

            string extension = fileName.Substring(indexDot);
            fileName = fileName.Substring(0, indexDot);

            foreach (var season in build.Seasons)
            {
                string prefix = season != "" ? '(' + season + ")_" : "";
                foreach (var celebration in build.Celebrations)
                {
                    string suffixCelebration = celebration != "" ? '.' + celebration : "";
                    foreach (var locale in build.Locales)
                    {
                        string suffixLocale = locale != "" ? '_' + locale : "";
                        string path = directory + prefix + fileName + suffixLocale + suffixCelebration + extension;

                        if (File.Exists(build.GetManglePath(path)))
                            build.AddContentEntity(this.GetType(), path, this);
                        else if (build.LocaleAssociations.ContainsKey(locale))
                        {
                            foreach (var localeAssociation in build.LocaleAssociations[locale])
                            {
                                suffixLocale = localeAssociation != "" ? '_' + localeAssociation : "";
                                path = directory + prefix + fileName + suffixLocale + suffixCelebration + extension;
                                if (File.Exists(build.GetManglePath(path)))
                                    build.AddContentEntity(this.GetType(), path, this);
                            }
                        }
                    }
                }
            }            
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Serialize() { /* TODO */ }
        public void DeSerialize() { /* TODO */ }
    }   // сContentSimpleEntity
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
