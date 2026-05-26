using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using Il2CppValve.VR;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AntiCheat.Patches
{
    [HarmonyPatch(typeof(GameStateManager), nameof(GameStateManager.RPC_ToggleLobbyDoors))]
    public class ToggleLobbyDoorsPatch
    {
        public static void Prefix(GameStateManager __instance, NetworkBool close)
        {
            if (Settings.IsHost)
            {
                if (__instance.InLobbyState() && close == false)
                {
                    MelonCoroutines.Start(ReCorrect(__instance, true));
                }
                else if (__instance.InTaskState() && close == true)
                {
                    MelonCoroutines.Start(ReCorrect(__instance, false));
                }
            }
        }

        private static IEnumerator ReCorrect(GameStateManager __instance, bool close)
        {
            yield return new WaitForSeconds(0.1f);
            __instance.RPC_ToggleLobbyDoors(close);
        }
    }
}