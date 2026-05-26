using AntiCheat.Managers;
using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using Il2CppSG.Airlock.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCheat.Patches
{
    [HarmonyPatch(typeof(NetworkedKillBehaviour), nameof(NetworkedKillBehaviour.RPC_TargetedAction))]
    public class CheckKillsPatch
    {
        public static void Postfix(NetworkedKillBehaviour __instance, PlayerRef targetedPlayer, PlayerRef perpetrator, int action)
        {
            if (Settings.IsHost)
            {
                if (AntiCheatManager.GetPlayerStateByID(perpetrator).ActionCooldownRemaining < 7)
                {
                    AntiCheatManager.GetPlayerNameViaPlayerState(perpetrator);
                    AntiCheatManager.KickPlayerForCheating(AntiCheatManager.NameResult, "Has 0 Second Kill Cooldown", perpetrator);
                }
            }
        }
    }
}