using System;
using System.IO;

using System.Xml;
using System.Collections.Generic;

namespace Sim
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.ApartmentState = System.Threading.ApartmentState.STA;
            if (args.Length == 1)
            {
                string file = args[0];
                if (File.Exists(file))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    DeleteFile(doc, file);
                    List<SimTask> tasks = new List<SimTask>();
                    if (doc.DocumentElement.SelectSingleNode("/sim/task") == null)
                    {
                        Console.WriteLine($"[-] Failed to parse a single task to perform from XML document {file}");
                        return;
                    }
                    var errorDirNode = doc.DocumentElement.SelectSingleNode("/sim/errordirectory");
                    if (errorDirNode == null)
                    {
                        Console.WriteLine($"[-] Failed to parse error directory location.");
                        return;
                    }
                    if (!Directory.Exists(errorDirNode.InnerText))
                    {
                        Console.WriteLine($"[-] Invalid error directory given. Reason: Path does not exist.");
                        return;
                    }
                    var nodes = doc.DocumentElement.SelectNodes("/sim/task");
                    for(int i = 0; i < nodes.Count; i++)
                    {
                        try
                        {
                            tasks.Add(new SimTask(nodes[i], errorDirNode.InnerText));
                        } catch (Exception ex)
                        {
                            Console.WriteLine($"[-] Error parsing task from XML schema. Reason: {ex.Message}");
                        }
                    }
                    foreach (var t in tasks)
                        t.Run();
                }
                else
                {
                    Console.WriteLine("[-]Could not find the XML script. Please ensure the script is present and that the path provided is valid.");
                    Console.ReadLine();
                }
            } 
            else if (args.Length < 1)
            {
                Console.WriteLine("[-] Sim requires at least one arguement.");
            } 
            else if (args.Length > 1)
            {
                Console.WriteLine("[+] Too many arguments provided. Sim only requires one argument for the path to the XML script.");
            }
        }
        
        static public void DeleteFile(XmlDocument doc, string file)
        {
            XmlNode deleteNode = doc.DocumentElement.SelectSingleNode("/sim/delete");
            string delete = deleteNode.InnerText.ToLower();
            if (delete == "true")
            {
                File.Delete(file);
            } else
            {
                Console.WriteLine("[+] File will not be removed.");
                return;
            }
        }

    }
}