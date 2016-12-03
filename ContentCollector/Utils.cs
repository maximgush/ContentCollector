using System.Diagnostics;

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
    }
}