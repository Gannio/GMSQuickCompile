using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml;

namespace RET
{
    public class Class1
    {
        [DllExport("RET_GetText", CallingConvention.Cdecl)]
        private static unsafe byte* GetText(char* parametersC)
        {

            string[] parameters = Marshal.PtrToStringAnsi((IntPtr)parametersC).Replace("\\\\","\\").Split('*');
            
            return ReturnString(File.ReadAllText(parameters[0]));
        }
        private static unsafe byte* ReturnString(string outputText)
        {
            fixed (byte* unmanagedOutput = new byte[outputText.Length])//(byte*)&output)
            {
                for (var i = 0; i < outputText.Length; i++)
                {
                    unmanagedOutput[i] = (byte)outputText[i];
                }
                return unmanagedOutput;
            }
        }
        [DllExport("RET_GetObjectSpriteData", CallingConvention.Cdecl)]
        private static unsafe byte* GetObjectSpriteData(char* parametersC)
        {
            
            string[] parameters = Marshal.PtrToStringAnsi((IntPtr)parametersC).Replace("\\\\", "\\").Split('*');
            Console.WriteLine("Grabbing params...");


            string projectLocation = parameters[0];
            string objectName = parameters[1];
            string appDataLoc = parameters[2];
            Console.WriteLine("Loading Object XML...");
            XmlDocument doc = new XmlDocument();
            try
            {
                var objDir = Path.GetDirectoryName(projectLocation) + "\\objects\\" + objectName + ".object.gmx";
                Console.WriteLine(objDir);
                Console.WriteLine("Loading...");
                doc.Load(objDir);
                var dep = doc["object"]["depth"].InnerText;
                Console.WriteLine("Getting sprite");
                string spriteName = doc["object"]["spriteName"].InnerText;
                if (spriteName != null && spriteName != "<undefined>")
                {
                    //File.Copy(Path.GetDirectoryName(projectLocation) + "/sprites/" + spriteName + ".sprite.gmx",appDataLoc + "/sprites/" + spriteName + ".sprite.gmx");
                    doc = new XmlDocument();
                    Console.WriteLine("Sprite XML..." + spriteName);
                    doc.Load(Path.GetDirectoryName(projectLocation) + "\\sprites\\" + spriteName + ".sprite.gmx");
                    var xOrig = doc["sprite"]["xorig"].InnerText;
                    var yOrig = doc["sprite"]["yorigin"].InnerText;//WHY ARE THESE NAMES DIFFERENT???
                    //Width and height can be inferred from the image.
                    Console.WriteLine("Copying Sprite...");
                    Directory.CreateDirectory(appDataLoc + "sprites\\images\\");
                    File.Delete(appDataLoc + "sprites\\images\\" + spriteName + "_0.png");
                    File.Copy(Path.GetDirectoryName(projectLocation) + "\\sprites\\images\\" + spriteName + "_0.png", appDataLoc + "sprites\\images\\" + spriteName + "_0.png");


                    Console.WriteLine(spriteName + "*" + xOrig + "*" + yOrig + "*" + dep);
                    return ReturnString(spriteName + "*" + xOrig + "*" + yOrig + "*" + dep);
                }
                else
                {
                    return ReturnString("*nosprite");
                }
                
            }
            catch (Exception e){
                Console.WriteLine(e.ToString());
                return ReturnString("*error");
            }
        }
        [DllExport("RET_GetBackground", CallingConvention.Cdecl)]
        private static unsafe byte* GetBackground(char* parametersC)
        {

            string[] parameters = Marshal.PtrToStringAnsi((IntPtr)parametersC).Replace("\\\\", "\\").Split('*');
            Console.WriteLine("Grabbing params...");


            string projectLocation = parameters[0];
            string backgroundName = parameters[1];
            string appDataLoc = parameters[2];
            Console.WriteLine("Loading Object XML...");
            XmlDocument doc = new XmlDocument();
            try
            {
                //File.Copy(Path.GetDirectoryName(projectLocation) + "/sprites/" + spriteName + ".sprite.gmx",appDataLoc + "/sprites/" + spriteName + ".sprite.gmx");
                doc = new XmlDocument();
                Console.WriteLine("Sprite XML..." + backgroundName);
                doc.Load(Path.GetDirectoryName(projectLocation) + "\\background\\" + backgroundName + ".background.gmx");
                var tileWidth = doc["background"]["tilewidth"].InnerText;
                var tileHeight = doc["background"]["tileheight"].InnerText;//WHY ARE THESE NAMES DIFFERENT???
                var tileXOff = doc["background"]["tilexoff"].InnerText;
                var tileYOff = doc["background"]["tileyoff"].InnerText;
                var tileHSep = doc["background"]["tilehsep"].InnerText;
                var tileVSep = doc["background"]["tilevsep"].InnerText;

                //Width and height can be inferred from the image.
                Console.WriteLine("Copying Background...");
                Directory.CreateDirectory(appDataLoc + "background\\images\\");
                File.Delete(appDataLoc + "background\\images\\" + backgroundName + ".png");
                File.Copy(Path.GetDirectoryName(projectLocation) + "\\background\\images\\" + backgroundName + ".png", appDataLoc + "background\\images\\" + backgroundName + ".png");


                Console.WriteLine(backgroundName + "*" + tileWidth + "*" + tileHeight + "*" + tileXOff + "*" + tileYOff + "*" + tileHSep + "*" + tileVSep);
                return ReturnString(backgroundName + "*" + tileWidth + "*" + tileHeight + "*" + tileXOff + "*" + tileYOff + "*" + tileHSep + "*" + tileVSep);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return ReturnString("*error");
            }
        }
    }
}
