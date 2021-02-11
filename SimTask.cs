using System;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Sim
{
    class SimTask
    {

        private XmlNode node;
        private int pauseTime = 5000;
        private int numLoops = 1;
        private bool killChildren = false;
        private string errorDir;
        private string errorLog;
        private List<Process> spawnedProcesses = new List<Process>();
        public SimTask(XmlNode _node, string _errorDir)
        {
            this.node = _node;
            errorDir = _errorDir;
            ParseNodeConfig();
        }

        private void ParseNodeConfig()
        {
            var configNode = node.SelectSingleNode("config");
            if (configNode == null)
                throw new Exception("Failed to get config handle.");
            var name = configNode.SelectSingleNode("name");
            string fileName;
            if (name == null)
            {
                fileName = Guid.NewGuid().ToString() + ".log";
            } else
            {
                fileName = name.InnerText + ".log";
            }
            errorLog = Path.Combine(errorDir, fileName);
            var pause = configNode.SelectSingleNode("pause");
            if (pause != null)
            {
                if (!int.TryParse(pause.InnerText, out pauseTime))
                {
                    Console.WriteLine($"[-] Failed to parse integer pause time from pause value of {pause.InnerText}");
                }
            }
            var loop = configNode.SelectSingleNode("loop");
            if (loop != null)
            {
                if (!int.TryParse(loop.InnerText, out numLoops))
                {
                    Console.WriteLine($"[-] Failed to parse integer loop number from loop value of {loop.InnerText}");
                } 
            }
        }

        private void LogError(Exception ex)
        {
            if (!File.Exists(errorLog))
            {
                using (StreamWriter sw = File.CreateText(errorLog))
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(errorLog))
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                }
            }
        }

        public void Run()
        {
            for (int l = 0; l < numLoops; l++)
            {
                var actionsNode = node.SelectSingleNode("actions");
            if (actionsNode == null)
                throw new Exception("Invalid XML schema for task node. Reason: No actions tag defined.");
            XmlNode childNode = actionsNode.FirstChild;
            
                while (childNode != null)
                {
                    try
                    {
                        switch (childNode.Name.ToLower())
                        {
                            case "setclipboard":
                                string clipValue = childNode.InnerText;
                                Keyboard.ClipSet(clipValue);
                                break;
                            case "getclipboard":
                                Keyboard.ClipGet();
                                break;
                            case "plain":
                                string keyvalue = childNode.InnerText;
                                Keyboard.KeyInput(keyvalue);
                                break;
                            case "special":
                                string keyValue = childNode.InnerText;
                                Keyboard.SkeyInput(keyValue);
                                break;
                            case "process":
                                string process = childNode.InnerText;
                                UserProcess(process);
                                break;
                            case "kill":
                                killChildren = true;
                                KillChildren(killChildren);
                                break;
                            case "powershell":
                                string powershell = childNode.InnerText;
                                UserPowershell(powershell);
                                break;
                            case "mount":
                                string path = childNode.InnerText;
                                MountDrive(path);
                                break;
                            case "sleep":
                                int.TryParse(childNode.InnerText, out int sleep);
                                Thread.Sleep(sleep);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }
                    childNode = childNode.NextSibling;
                    Thread.Sleep(pauseTime);
                }
            }
        }

        private void KillChildren(bool killChildren)
        {
            if (killChildren == true)
            {
                foreach (var proc in spawnedProcesses)
                {
                    proc.Kill();
                }
            }
        }

        private void UserProcess(string process)
        {
            var proc = Process.Start(process);
            spawnedProcesses.Add(proc);
        }
        
        static public void UserPowershell(string powershell)
        {
            string s = PowerShellRunner.InvokePS(powershell);
            Console.WriteLine(s);
        }

        static public void MountDrive(string path)
        {
            string argument = "use K: " + path;
            Process.Start("net.exe", argument);
        }
    }
}
