using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace QuickCompile
{
    public class Class1
    {
        [DllExport("QuickCompile_Compile", CallingConvention.Cdecl)]
        public static unsafe double CompileTest(char* parametersC)
        {

            string[] parameters = Marshal.PtrToStringAnsi((IntPtr)parametersC).Replace("\\\\","\\").Split('*');
            

            var t = Task.Run(() => Compile(parameters));

            if (parameters[5] == "true")
            {
                t.Wait();
            }

                //Compile(parameters);

                //string output = p.StandardOutput.ReadToEnd();
                //Console.WriteLine(output);
                return 1;
        }
        static string lastRunCommand = "";
        static string lastRunArgs = "";
        static string lastRunCD = "";

        static string lastRunDebugCommand = "";
        static string lastRunDebugArgs = "";
        static bool lastRunWasDebug = false;

        private static void Compile(string[] parameters)
        {
            string cacheLoc = parameters[0];//Marshal.PtrToStringAnsi((IntPtr)detailsC);
            string tempLoc = parameters[1];//Marshal.PtrToStringAnsi((IntPtr)stateC);
            string compilerLoc = parameters[2];//Marshal.PtrToStringAnsi((IntPtr)largeC);
            string projectLoc = parameters[3];// Marshal.PtrToStringAnsi((IntPtr)smallC);
            string runnerLoc = "\"" + parameters[4] + "\"";
            bool doWait = parameters[5] == "true";
            bool doDebug = parameters[6] == "true";
            string featherweightParams = "";

            
            if (parameters.Length > 7)
            {
                featherweightParams = parameters[7];
                var excludeData = "";
                if (parameters.Length > 8)
                {
                    excludeData = " -nd";
                }
                var slurDir = Path.GetDirectoryName(projectLoc) + "\\meta\\gmslur.exe";
                string featherWeightProject = Path.GetDirectoryName(projectLoc);
                featherWeightProject = featherWeightProject + "\\lw_" + Path.GetFileName(projectLoc);
                //Console.WriteLine(fBuild);
                var argF = "-x -gp -uc" + excludeData + " -roomw \"" + featherweightParams + "\" -p \"" + projectLoc + "\""/*args*/;
                Console.WriteLine(argF);
                var fBuild = new ProcessStartInfo(slurDir)
                {
                    Arguments = argF,

                    UseShellExecute = !doWait,
                    CreateNoWindow = false,
                    //RedirectStandardInput = true,
                    RedirectStandardOutput = doWait
                };
                var f = Process.Start(fBuild);
                if (doWait)
                {
                    Console.WriteLine(f.StandardOutput.ReadToEnd());
                }
                f.WaitForExit();
                
                projectLoc = featherWeightProject;
            }
            DateTime now = DateTime.Now;
            string tempName = "\\gm_TQCFiles\\gm_TQC_" + now.Ticks.ToString();

            string gameName = Path.GetFileNameWithoutExtension(projectLoc).Replace(".project","");

            //string tempName = "\\gm_ttt_77777\\gm_ttt_50518";
            //string largeImage = parameters[4];// Marshal.PtrToStringAnsi((IntPtr)largeImageC);
            //string smallImage = parameters[5];// Marshal.PtrToStringAnsi((IntPtr)smallImageC);

            string debugStr = "";
            if (doDebug)
            {
                debugStr = "/debug ";
            }/*@"C:\Program Files (x86)\Steam\steamapps\common\gamemaker_studio\GMAssetCompiler.exe"*/
            string program = "\"" + compilerLoc + "\"";
            string args = " /c /m=win  " + debugStr + "/config=\"Default\" /tgt=64 /obob=True /obpp=False /obru=True /obes=False /i=3 /j=16 /cvm /tp=2048 /mv=1 /iv=0 /rv=0 /bv=1804 /gn=\"" + gameName + "\" /td=\"" + tempLoc + "\" /cd=\"" + cacheLoc + "\" /sh=True /dbgp=\"6502\" /hip=\"192.168.56.1\" /hprt=\"51268\" /o=\"" + tempLoc + tempName + "\" \"" + projectLoc + "\"";






            var psi = new ProcessStartInfo(program)//For technical reasons, this part can't do the usual wait.
            {
                Arguments = args,

                UseShellExecute = true,//!doWait,
                CreateNoWindow = false,
                //RedirectStandardInput = true,
                //RedirectStandardOutput = doWait
            };
            var p = Process.Start(psi);
            p.WaitForExit();
            Console.WriteLine(program + args);
            if (doDebug)//We need to launch this inside GMS as well sadly.
            {

                var debugCompilerLoc = Path.GetDirectoryName(compilerLoc) + "\\GMDebug\\GMDebug.exe";
                var xmlFind =
                program = "\"" + debugCompilerLoc + "\"";
                args = " -d=\"" + tempLoc + tempName + "\\" + gameName + ".yydebug\""
                     + " -t=\"127.0.0.1\""
                     + " -u=\"" + cacheLoc + "\\" + gameName + "\\" + FindTheXMLFile(cacheLoc + "\\" + gameName + "\\", gameName) + "\""
                     + " -p=\"" + projectLoc + "\" -c=\"Default\" -ac=\"" + compilerLoc + "\" -tp=6502";
                Console.WriteLine(program + args);
                lastRunDebugCommand = program;
                lastRunDebugArgs = args;
                lastRunWasDebug = true;
                psi = new ProcessStartInfo(program)//For technical reasons, this part can't do the usual wait.
                {
                    Arguments = args,

                    UseShellExecute = true,//!doWait,
                    CreateNoWindow = false,
                    //RedirectStandardInput = true,
                    //RedirectStandardOutput = doWait
                };
                p = Process.Start(psi);//We can't wait for exit since we need to finish!
                //p.WaitForExit();
            }
            else
            {
                lastRunWasDebug = false;
            }

            /*string newRunner = tempLoc + tempName + "\\Runner.exe";//Because of a quirk with FMOD running on a runner ran outside GMS, we need to copy it to the local area. 4MB per compile just about.
            Console.WriteLine(newRunner);//File.WriteAllText("test.txt", newRunner);
            File.Copy(parameters[4],newRunner);

            //File.Move(tempLoc+tempName + "\\" + gameName + ".win", tempLoc + tempName + "\\data.win");
            
            lastRunCommand = newRunner;
            lastRunArgs = " -game " + tempLoc + tempName + "\\" + gameName + ".win";


            File.WriteAllText("cd " + tempLoc + tempName + "\n" + "runLast.bat", lastRunCommand + lastRunArgs);
            lastRunCommand = "CMD.exe";
            lastRunArgs = "/C " + tempLoc + tempName + "\\" + "runLast.bat";
            */


            lastRunCommand = runnerLoc;//newRunner;
            lastRunArgs = " -game \"" + tempLoc + tempName + "\\" + gameName + ".win\"";
            lastRunCD = "cd \"" + tempLoc + tempName + "\\\"";

            File.WriteAllText(tempLoc + tempName + "\\" + "run.bat", lastRunCD + "\n" + lastRunCommand + lastRunArgs);

            lastRunCommand = tempLoc + tempName + "\\" + "run.bat";
            lastRunArgs = "";
            //Run();

            Run();

            return;
            psi = new ProcessStartInfo(lastRunCommand)
            {
                Arguments = lastRunArgs,
                UseShellExecute = !doWait,
                CreateNoWindow = true,
                WorkingDirectory = tempLoc + tempName + "\\",
                RedirectStandardOutput = doWait
            };
            Console.WriteLine(lastRunCommand);
            //p.StartInfo = psi;
            //p.Start();
            Process.Start(tempLoc + tempName + "\\" + "runLast.bat", null,null,"GMSTest");
            
        }

        [DllExport("QuickCompile_CleanTemp", CallingConvention.Cdecl)]
        public static unsafe double CleanTemp(char* parametersC)
        { 
            string[] parameters = Marshal.PtrToStringAnsi((IntPtr)parametersC).Replace("\\\\", "\\").Split('*');

            try
            {

                string dirs = parameters[0] + "\\gm_TQCFiles\\";
                if (!Directory.Exists(dirs))
                {
                    Console.WriteLine("Temp directory " + parameters[0] + " does not exist!");
                }
                else
                { 
                    foreach (string dir in Directory.GetDirectories(dirs))
                    {
                        var shortName = dir.Split('\\');
                        if (shortName[shortName.Length - 1].Contains("gm_TQC_"))
                        {


                            Console.WriteLine("Deleting " + dir);

                            Directory.Delete(dir,true);//file.Delete();

                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
            return 1;
        }


        [DllExport("QuickCompile_Run", CallingConvention.Cdecl)]
        public static unsafe double RunTest()
        {
            if (lastRunCommand == "")
            {
                Console.WriteLine("Wait for compile to finish first!");
                return 0;
            }
            if (lastRunWasDebug)//We need to launch this inside GMS as well sadly.
            {

                var program = lastRunDebugCommand;
                var args = lastRunDebugArgs;
                Console.WriteLine(program + args);
                var psi = new ProcessStartInfo(program)//For technical reasons, this part can't do the usual wait.
                {
                    Arguments = args,

                    UseShellExecute = true,//!doWait,
                    CreateNoWindow = false,
                    //RedirectStandardInput = true,
                    //RedirectStandardOutput = doWait
                };
                var p = Process.Start(psi);//We can't wait for exit since we need to finish!
                //p.WaitForExit();
            }

            Run(false);
            return 1;

        }
        private static void Run(bool doWait = false)
        {
            Console.WriteLine(lastRunCommand);

            var psi = new ProcessStartInfo(lastRunCommand)
            {
                Arguments = lastRunArgs,
                UseShellExecute = !doWait,
                CreateNoWindow = doWait,
                //RedirectStandardInput = true,
                RedirectStandardOutput = doWait
            };

            var p = Process.Start(psi);
            if (doWait)
            {
                Console.WriteLine(p.StandardOutput.ReadToEnd());
                p.WaitForExit();
            }
            /*p.StandardInput.WriteLine(lastRunCD);
            p.StandardInput.WriteLine(lastRunCommand + lastRunArgs);*/
        }
        private static string FindTheXMLFile(string directory,string gameName)
        {
            Console.WriteLine("Searching for first XML file for debugging.");
            var matches = Directory.GetFiles(directory,gameName + ".*.xml");
            Console.WriteLine("Found " + matches[0]);
            return Path.GetFileName(matches[0]);
        }
    }
}
