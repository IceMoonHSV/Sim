using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sim
{
    public class Keyboard
    {
        public static Dictionary<string, byte> SpecialKeys = new Dictionary<string, byte>()
        {
            { "{BACKSPACE}", 0x08 },
            { "{TAB}", 0x09 },
            { "{CLEAR}", 0x0C },
            { "{ENTER}", 0x0D },
            { "{SHIFT}", 0x10 },
            { "{CONTROL}", 0x11 },
            { "{ALT}", 0x12 },
            { "{PAUSE}", 0x13 },
            { "{CAPS}", 0x14 },
            { "{ESC}", 0x1B },
            { "{PAGEUP}", 0x21 },
            { "{PAGEDOWN}", 0x22 },
            { "{END}", 0x23 },
            { "{HOME}", 0x24 },
            { "{ARROWLEFT}", 0x25 },
            { "{ARROWUP}", 0x26 },
            { "{ARROWRIGHT}", 0x27 },
            { "{ARROWDOWN}", 0x28 },
            { "{SELECT}", 0x29 },
            { "{PRINT}", 0x2A },
            { "{EXECUTE}", 0x2B },
            { "{PRINTSCREEN}", 0x2C },
            { "{INSERT}", 0x2D },
            { "{DELETE}", 0x2E },
            { "{HELP}", 0x2F },
            { "{F1}", 0x70 },
            { "{F2}", 0x71 },
            { "{F3}", 0x72 },
            { "{F4}", 0x73 },
            { "{F5}", 0x74 },
            { "{F6}", 0x75 },
            { "{F7}", 0x76 },
            { "{F8}", 0x77 },
            { "{F9}", 0x78 },
            { "{F10}", 0x79 },
            { "{F11}", 0x7A },
            { "{F12}", 0x7B },
            { "{F13}", 0x7C },
            { "{F14}", 0x7D },
            { "{F15}", 0x7E },
            { "{F16}", 0x7F },
        };
        
        public static Dictionary<string, byte> KeysLower = new Dictionary<string, byte>()
        {
            { "a", 0x41 },
            { "b", 0x42 },
            { "c", 0x43 },
            { "d", 0x44 },
            { "e", 0x45 },
            { "f", 0x46 },
            { "g", 0x47 },
            { "h", 0x48 },
            { "i", 0x49 },
            { "j", 0x4A },
            { "k", 0x4B },
            { "l", 0x4C },
            { "m", 0x4D },
            { "n", 0x4E },
            { "o", 0x4F },
            { "p", 0x50 },
            { "q", 0x51 },
            { "r", 0x52 },
            { "s", 0x53 },
            { "t", 0x54 },
            { "u", 0x55 },
            { "v", 0x56 },
            { "w", 0x57 },
            { "x", 0x58 },
            { "y", 0x59 },
            { "z", 0x5A },
            { "0", 0x30 },
            { "1", 0x31 },
            { "2", 0x32 },
            { "3", 0x33 },
            { "4", 0x34 },
            { "5", 0x35 },
            { "6", 0x36 },
            { "7", 0x37 },
            { "8", 0x38 },
            { "9", 0x39 },
            { " ", 0x20 },
            { "=", 0xBB },
            { ",", 0xBC },
            { "-", 0xBD },
            { ".", 0xBE },
            { "/", 0xBF },
            { "`", 0xC0 },
            { "[", 0xDB },
            { "\\", 0xDC },
            { "]", 0xDD },
            { "\'", 0xDE },
            { ";", 0xBA }
        };
        
        public static Dictionary<string, byte> KeysUpper = new Dictionary<string, byte>()
        {
            { "A", 0x41 },
            { "B", 0x42 },
            { "C", 0x43 },
            { "D", 0x44 },
            { "E", 0x45 },
            { "F", 0x46 },
            { "G", 0x47 },
            { "H", 0x48 },
            { "I", 0x49 },
            { "J", 0x4A },
            { "K", 0x4B },
            { "L", 0x4C },
            { "M", 0x4D },
            { "N", 0x4E },
            { "O", 0x4F },
            { "P", 0x50 },
            { "Q", 0x51 },
            { "R", 0x52 },
            { "S", 0x53 },
            { "T", 0x54 },
            { "U", 0x55 },
            { "V", 0x56 },
            { "W", 0x57 },
            { "X", 0x58 },
            { "Y", 0x59 },
            { "Z", 0x5A },
            { "?", 0xBF },
            { "~", 0xC0 },
            { "{", 0xDB },
            { "|", 0xDC },
            { "}", 0xDD },
            { "\"", 0xDE },
            { ")", 0x30 },
            { "!", 0x31 },
            { "@", 0x32 },
            { "#", 0x33 },
            { "$", 0x34 },
            { "%", 0x35 },
            { "^", 0x36 },
            { "&", 0x37 },
            { "*", 0x38 },
            { "(", 0x39 },
            { "_", 0xBD },
            { "+", 0xBB },
            { "<", 0xBC },
            { ">", 0xBE },
            { ":", 0xBA }
        };

        static public void ClipSet(string clipValue)
        {
            Clipboard.SetText(clipValue);
        }

        static public void ClipGet()
        {
            string keyValue = "{CONTROL}+v";
            SkeyInput(keyValue);
        }

        [DllImport("user32.dll")]
        
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        
        const byte VK_SHIFT = 0x10;
        
        static public void LowerKey(byte a)
        {
            keybd_event(a, 0, 0, 0);
            Thread.Sleep(100);
            keybd_event(a, 0, 2, 0);
        }
        
        static public void UpperKey(byte a)
        {
            keybd_event(VK_SHIFT, 0, 0, 0);
            keybd_event(a, 0, 0, 0);
            Thread.Sleep(100);
            keybd_event(a, 0, 2, 0);
            keybd_event(VK_SHIFT, 0, 2, 0);
        }
        
        static public void ComboKey(byte[] keys)
        {
            foreach (byte a in keys)
            {
                keybd_event(a, 0, 0, 0);
            }
            Thread.Sleep(100);
            foreach (byte a in keys)
            {
                keybd_event(a, 0, 2, 0);
            }
        }
        
        static public void KeyInput(string keyValue)
        {
            char[] keys = keyValue.ToCharArray();
            foreach (char k in keys)
            {
                if (KeysLower.TryGetValue(k.ToString(), out byte lowerchar))
                {
                    LowerKey(lowerchar);
                }
                else if (KeysUpper.TryGetValue(k.ToString(), out byte upperchar))
                {
                    UpperKey(upperchar);
                }
            }
        }
        
        static public void SkeyInput(string keyValue)
        {
            string[] keys = keyValue.Split();
            foreach (string key in keys)
            {
                if (key.Contains("+"))
                {
                    string[] keycombo = key.Split('+');
                    List<byte> keylist = new List<byte>();
                    foreach (string combo in keycombo)
                    {
                        if (SpecialKeys.TryGetValue(combo, out byte singlekey))
                        {
                            keylist.Add(singlekey);
                        }
                        else if (KeysLower.TryGetValue(combo, out byte skey))
                        {
                            keylist.Add(skey);
                        }
                        else
                        {
                            throw new Exception($"[-] Could not find key: {combo}");
                        }
                    }
                    ComboKey(keylist.ToArray());
                }
                else
                {
                    if (SpecialKeys.TryGetValue(key, out byte singlek))
                    {
                        LowerKey(singlek);
                    }
                }
            }

        }
    }
}
