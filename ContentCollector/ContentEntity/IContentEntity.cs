﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCollector
{
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    enum eContentEntityTypes
    {
        cetSimple,

        cetPlayerCar,
        cetTrafficCar,
        cetCarPropretyINI,
        cetCarPhysicsSetupINI,
        cetCarTuningXML,

        cetDevice,

        cetPedestrian,

        cetN2,

        cetLocation,
        cetMission,
        cetGameTypesIni
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    interface IContentEntity
    {
        string Name     { get; set;}
        string FileName { get; set;}
        eContentEntityTypes EntityType { get; set; }
        bool    IsRoot { get; set; }

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
