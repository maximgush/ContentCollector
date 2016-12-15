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

            //build.ProjectPath = @"F:\Transporter\Project";
            build.ProjectPath = @"D:\Transporter\Trunk\Project";

            cContentEntityGameTypesIni root = new cContentEntityGameTypesIni();
            //root.Name =  @"F:\Transporter\Automation\Build System\config\Game_Types.ini";
            root.Name = @"D:\Transporter\Tools\Automation\Build System\config\Game_Types.ini";
            root.BuildType = "Category_C";

            build.AddRootContentEntity(root);

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


            // Вывод в файл список контента собранного питоноскриптом
            StreamReader reader =
                new StreamReader(new FileStream(
                    @"F:\Transporter\Automation\Build System\result\final_content_list.txt", FileMode.Open));           

            List<string> files = new List<string>();
            while (!reader.EndOfStream)
            {
                string str =  reader.ReadLine();
                Utils.GetNormalPath(ref str);
                if (build.ExistFileInProject(str))
                    files.Add(str);
            }
            reader.Close();

            files.Sort();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"content_list_python.txt"))
            {
                foreach (string line in files)
                {
                    file.WriteLine(line);
                }
            }
            //build.Update();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("Сборка контента завершена");
            Console.WriteLine("Количество файлов: " + contentList.Count);
            Console.WriteLine("Время сборки: " + elapsedTime);

            Utils.CompareContent("content_list.txt", "content_list_python.txt", "rst.txt");

            Console.ReadKey();
        }
    }
}
