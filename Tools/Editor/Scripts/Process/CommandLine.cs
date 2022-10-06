using UnityEngine;
using System.Diagnostics;
using System;

namespace CelesteEditor.Tools
{
    public class CommandLine
    {
        public static void ExecuteProcessTerminal(string executable, string argument)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = executable,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = argument,
                    WorkingDirectory = Application.dataPath
                };

                Process myProcess = new Process()
                {
                    StartInfo = startInfo,
                };

                myProcess.Start();
                myProcess.WaitForExit();
                myProcess.Close();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}
