﻿using System;
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

        // Флаг обозначающий, что данный элемент контента представляет вариацию для конкретного языка, времени года, праздника и тд
        private bool mIsEntityVariant = false;

        private ISet<cContentEntitySimple> m_childContentEntities = new HashSet<cContentEntitySimple>();
        private ISet<cContentEntitySimple> m_parentContentEntities = new HashSet<cContentEntitySimple>();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntitySimple()
        {
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntitySimple(string name, cContentEntitySimple parent)
        {
            m_name = name;
            Utils.GetNormalPath(ref m_name);
 
            if (parent != null)
                m_parentContentEntities.Add(parent);
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
            get { return Name; }
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
        public virtual void Parse(cBuild build)
        {
            AddEntityVariants(build);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void AddEntityVariants(cBuild build)
        {
            if (mIsEntityVariant)
                return;

            string fileName = Path.GetFileName(Name);
            string directory = Path.GetDirectoryName(build.GetRelativePath(Name)) + "\\";

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

                        if (build.ExistFileInProject(path))
                        {
                            cContentEntitySimple entity = (cContentEntitySimple)Activator.CreateInstance(this.GetType(), path, this);
                            entity.mIsEntityVariant = true;
                            build.AddContentEntity(entity);
                        }
                        else if (build.LocaleAssociations.ContainsKey(locale))
                        {
                            foreach (var localeAssociation in build.LocaleAssociations[locale])
                            {
                                suffixLocale = localeAssociation != "" ? '_' + localeAssociation : "";
                                path = directory + prefix + fileName + suffixLocale + suffixCelebration + extension;
                                if (build.ExistFileInProject(path))
                                {
                                    cContentEntitySimple entity = (cContentEntitySimple)Activator.CreateInstance(this.GetType(), path, this);
                                    entity.mIsEntityVariant = true;
                                    build.AddContentEntity(entity);
                                }
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
