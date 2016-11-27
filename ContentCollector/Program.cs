using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            cBuild build = new cBuild();

            build.AddRootContentEntity(typeof(cContentEntityGameTypesIni), "GameTypeIni", @"E:\Programming\Forward_Development\ContentCollector\Project\GameType.ini");
            build.ProductInternalName = "CATEGORY_C_EX";
            build.ProjectPath = @"E:\Programming\Forward_Development\ContentCollector\Project";

            build.Rebuild();

            build.Serialize(@"test.xml");
            //build.Update();
        }
    }
}
