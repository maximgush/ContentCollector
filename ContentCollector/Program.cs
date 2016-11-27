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

            build.Rebuild();

            build.Serialize(@"test.xml");
            //build.Update();

        }
    }
}
