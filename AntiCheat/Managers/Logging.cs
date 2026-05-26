using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCheat.Managers
{
    public class Logging
    {
        static void Print(string message = "", int type = 0)
        {
            if (type == 0)
            {
                MelonLogger.Msg(message);
            }
            else if (type == 1)
            {
                MelonLogger.Warning(message);
            }
            else if (type == 2)
            {
                MelonLogger.Error(message);
            }
        }
        public static void AntiCheatLog(string message)
        {
            Print($"[ANTICHEAT]: {message}", 0);
        }
        public static void AntiCheatWarn(string message)
        {
            Print($"[ANTICHEAT]: {message}", 1);
        }
        public static void AntiCheatError(string message)
        {
            Print($"[ANTICHEAT]: {message}", 2);
        }
        public static void BlackListLog(string message)
        {
            Print($"[BLACKLIST]: {message}", 0);
        }
        public static void BlackListWarn(string message)
        {
            Print($"[BLACKLIST]: {message}", 1);
        }
        public static void BlackListError(string message)
        {
            Print($"[BLACKLIST]: {message}", 2);
        }
    }
}