using UnityEngine;
using System.Diagnostics;
using System;

namespace CelesteEditor.Tools
{
    public class CommandLine
    {
        public static Tuple<string, string, int> ExecuteProcessTerminalReturnAll(
            string executable, 
            string argument, 
            bool verbose = false, 
            bool useShell = false)
        {
            try
            {
                if (verbose)
                {
                    UnityEngine.Debug.Log("============== Start Executing [" + executable + " " + argument + "] ===============");
                    UnityEngine.Debug.Log("Current Working Directory: " + Application.dataPath);
                }

                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = executable,
                    UseShellExecute = useShell,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = argument,
                    WorkingDirectory = Application.dataPath
                };

                Process myProcess = Process.Start(startInfo);
                
                string output = myProcess.StandardOutput.ReadToEnd();

                if (verbose)
                {
                    UnityEngine.Debug.Log(output);
                }

                string error = myProcess.StandardError.ReadToEnd();

                if (verbose && !string.IsNullOrWhiteSpace(error))
                {
                    UnityEngine.Debug.LogError(error);
                }

                myProcess.WaitForExit();

                if (verbose)
                {
                    UnityEngine.Debug.Log("============== End ===============");
                }

                return new Tuple<string, string, int>(output, error, myProcess.ExitCode);
            }
            catch (Exception e)
            {
                if (verbose)
                {
                    UnityEngine.Debug.LogException(e);
                }

                return new Tuple<string, string, int>(null, e.ToString(), -1);
            }
        }

        public static string ExecuteProcessTerminal(string executable, string argument, bool verbose = false, bool useShell = false)
        {
            var data = ExecuteProcessTerminalReturnAll(executable, argument, verbose, useShell);
            var output = data.Item1;
            var error = data.Item2;
            var returnCode = data.Item3;

            if (returnCode == -1)
            {
                return null;
            }
            else
            {
                return output;
            }
        }
    }
}
