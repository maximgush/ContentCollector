using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace ContentCollector
{
    public class Utils
    {
        static public int RunProcess(string fileName, string arguments = "", bool waitForExit = true)
        {
            Process process = new Process();
            int result = 0;

            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;

            process.Start();

            if (waitForExit)
            {
                process.WaitForExit(); // Ожидаем завершение потока
                result = process.ExitCode;
            }

            return result;
        }

        static public void CompareContent(string contentFile1, string contentFile2, string resultFile)
        {
            HashSet<string> content1 = new HashSet<string>();
            HashSet<string> content2 = new HashSet<string>();
            StreamReader reader = new StreamReader(new FileStream(contentFile1, FileMode.Open));
            while (!reader.EndOfStream)
                content1.Add(reader.ReadLine());
            reader.Close();

            StreamReader reader2 = new StreamReader(new FileStream(contentFile2, FileMode.Open));
            while (!reader2.EndOfStream)
            {
                string file = reader2.ReadLine();

                file = file.Replace("home:", "");
                file = file.Replace(":", "\\");
                file = file.Replace("/", "\\");
                file = file.Replace("\\\\", "\\");
                file = file.Replace("\\\\", "\\");
                file = file.TrimStart(new char[] { '\\' });
                file = file.ToLower();
                content2.Add(file);
            }
            reader2.Close();


            HashSet<string> setUniqueFromContent1 = new HashSet<string>();
            HashSet<string> setUniqueFromContent2 = new HashSet<string>();

            foreach (var file in content2)
            {
                if (!content1.Contains(file))
                    setUniqueFromContent2.Add(file);
            }

            foreach (var file in content1)
            {
                if (!content2.Contains(file))
                    setUniqueFromContent1.Add(file);
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(resultFile))
            {
                foreach (string line in setUniqueFromContent1)
                {
                    writer.WriteLine("+" + line);
                }

                writer.WriteLine();
                writer.WriteLine();

                foreach (string line in setUniqueFromContent2)
                {
                    writer.WriteLine("-" + line);
                }
            }
        }
    }
}