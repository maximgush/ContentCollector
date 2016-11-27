using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace ContentCollector
{
    class сFactoryContentEntity
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static  сFactoryContentEntity instance = new сFactoryContentEntity();
        public static сFactoryContentEntity Instance
        {
            get{ return instance;}
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        delegate IContentEntity FactoryMethod();
        Dictionary<eContentEntityTypes, FactoryMethod>   m_FactoryMethods = new Dictionary<eContentEntityTypes, FactoryMethod>();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void RegisterContentEntity(eContentEntityTypes cetType, FactoryMethod creator)
        {
            Type TestType = Type.GetType("Reflection1.TestClass", false, true);

            //если класс не найден
            if (TestType != null)
            {
                //получаем конструктор
                System.Reflection.ConstructorInfo ci = TestType.GetConstructor(new Type[] { });

                //вызываем конструтор
                object Obj = ci.Invoke(new object[] { });
            }
            else
            {
                Console.WriteLine("Класс не найден");
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IContentEntity Create(eContentEntityTypes cetType)
        {
            return instance.m_FactoryMethods[cetType]();
        }
    }   // сFactoryContentEntity
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // ContentCollector
