using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCollector
{
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IContentEntity
    {
        string Name     { get; set;}
        string FileName { get; set;}
        bool   IsRoot { get; set; }
        List<IContentEntity> ParentContentEntities { get; }
        List<IContentEntity> ChildContentEntities { get; }

        void    AddChildContentEntity(IContentEntity entity);
        void    RemoveChildContentEntity(IContentEntity entity);

        void    AddParentContentEntity(IContentEntity entity);
        void    RemoveParentContentEntity(IContentEntity entity);

        void    RemoveYouselfFromChildContentEntities();
        void    RemoveYouselfFromParentContentEntities();

        bool    HasParentEntities();

        void    Parse();

        void    Serialize();
        void    DeSerialize();
    };
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
