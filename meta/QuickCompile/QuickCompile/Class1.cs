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
            //Compile(parameters);
            
            //string output = p.StandardOutput.ReadToEnd();
            //Console.WriteLine(output);
            return 1;
        }
        static string lastRunCommand = "";
        static string lastRunArgs = "";
        static string lastRunCD = "";
        private static void Compile(string[] parameters)
        {
            string cacheLoc = parameters[0];//Marshal.PtrToStringAnsi((IntPtr)detailsC);
            string tempLoc = parameters[1];//Marshal.PtrToStringAnsi((IntPtr)stateC);
            string compilerLoc = parameters[2];//Marshal.PtrToStringAnsi((IntPtr)largeC);
            string projectLoc = parameters[3];// Marshal.PtrToStringAnsi((IntPtr)smallC);
            string runnerLoc = "\"" + parameters[4] + "\"";
            DateTime now = DateTime.Now;
            string tempName = "\\gm_TQCFiles\\gm_TQC_" + now.Ticks.ToString();

            string gameName = Path.GetFileNameWithoutExtension(projectLoc).Replace(".project","");

            //string tempName = "\\gm_ttt_77777\\gm_ttt_50518";
            //string largeImage = parameters[4];// Marshal.PtrToStringAnsi((IntPtr)largeImageC);
            //string smallImage = parameters[5];// Marshal.PtrToStringAnsi((IntPtr)smallImageC);


            string program = "\"" + @"C:\Program Files (x86)\Steam\steamapps\common\gamemaker_studio\GMAssetCompiler.exe" + "\"";
            string args = " /c /m=win  /config=\"Default\" /tgt=64 /obob=True /obpp=False /obru=True /obes=False /i=3 /j=16 /cvm /tp=2048 /mv=1 /iv=0 /rv=0 /bv=1804 /gn=\"" + gameName + "\" /td=\"" + tempLoc + "\" /cd=\"" + cacheLoc + "\" /sh=True /dbgp=\"6502\" /hip=\"192.168.56.1\" /hprt=\"51268\" /o=\"" + tempLoc + tempName + "\" \"" + projectLoc + "\"";






            var psi = new ProcessStartInfo(program)
            {
                Arguments = args,

                UseShellExecute = true,
                CreateNoWindow = false,
                //RedirectStandardInput = true,
                //RedirectStandardOutput = true
            };
            var p = Process.Start(psi);
            p.WaitForExit();


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
            lastRunArgs = " -game " + tempLoc + tempName + "\\" + gameName + ".win";
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
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = tempLoc + tempName + "\\"
            };
            Console.WriteLine(lastRunCommand);
            //p.StartInfo = psi;
            //p.Start();
            Process.Start(tempLoc + tempName + "\\" + "runLast.bat", null,null,"GMSTest");
            Console.WriteLine(program + args);
        }
        [DllExport("QuickCompile_Run", CallingConvention.Cdecl)]
        public static unsafe double RunTest()
        {
            if (lastRunCommand == "")
            {
                Console.WriteLine("Wait for compile to finish first!");
                return 0;
            }
            Run();
            return 1;

        }
        private static void Run()
        {
            Console.WriteLine(lastRunCommand);

            var psi = new ProcessStartInfo(lastRunCommand)
            {
                Arguments = lastRunArgs,
                UseShellExecute = true,
                CreateNoWindow = false,
                //RedirectStandardInput = true,
                //RedirectStandardOutput = true
            };
            var p = Process.Start(psi);

            /*p.StandardInput.WriteLine(lastRunCD);
            p.StandardInput.WriteLine(lastRunCommand + lastRunArgs);*/
        }
    }
}
