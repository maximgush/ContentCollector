using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContentCollector
{
    class cContentEntityN2 : cContentEntitySimple
    {
        public cContentEntityN2()
        {
            EntityType = eContentEntityTypes.cetN2;
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            // TODO:
            // Парсим n2-файлы, вытаскиваем оттуда текстурки
            // Проверяем доступные языковые ассоциации
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityN2
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
