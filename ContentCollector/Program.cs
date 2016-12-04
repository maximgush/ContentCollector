using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            cBuild build = new cBuild();

//             build.AddRootContentEntity(typeof(cContentEntityGameTypesIni), "GameTypeIni",
//                 @"E:\Programming\Forward_Development\ContentCollector\Project\GameType.ini");
            build.AddRootContentEntity(typeof(cContentEntityGameTypesIni), @"F:\Transporter\Automation\Build System\config\Game_Types.ini");
//             build.AddRootContentEntity(typeof(cContentEntityHardCodeN2Files),
//                 @"F:\Transporter\Automation\Build System\config\hard_n2.txt",
//                 @"F:\Transporter\Automation\Build System\config\hard_n2.txt");

            build.ProductInternalName = "Category_C";
            build.ProjectPath = @"F:\Transporter\Project";

            build.Rebuild();

            build.Serialize(@"test.xml");

            var contentList = build.GetContentList();
            contentList.Sort();
            // Вывод в файл список контента
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"content_list.txt"))
            {
                foreach (string line in contentList)
                {
                    file.WriteLine(line);
                }
            }

/*
            // Вывод в файл список контента собранного питоноскриптом
            StreamReader reader =
                new StreamReader(new FileStream(
                    @"F:\Transporter\Automation\Build System\result\final_content_list.txt", FileMode.Open));
            string text = reader.ReadToEnd();
            reader.Close();

            var files = text.Split(new char[] {'\n', '\r'}).ToList();
            files.ForEach(line => { line = line.Replace("home:", "");
                                    line = line.Replace(":", "\\");
                                    line = line.Replace("/", "\\");
                                    line = line.Replace("\\\\", "\\");
                                    line = line.Replace("\\\\", "\\");
                                    line = line.TrimStart(new char[]{'\\'});
            });

            files.Sort();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"content_list_python.txt"))
            {
                foreach (string line in files)
                {
                    if (line == "")
                        continue;
                    file.WriteLine(line);
                }
            }
            //build.Update();
*/
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("Сборка контента завершена");
            Console.WriteLine("Количество файлов: " + contentList.Count);
            Console.WriteLine("Время сборки: " + elapsedTime);

            Console.ReadKey();
        }
    }
}
