using AntiCheat.Managers;
using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using MelonLoader;
using static AntiCheat.References;
using UnityEngine;
using System.Collections;
using Il2CppSG.Airlock.Network;
using Il2CppFusion.Photon.Realtime;

namespace AntiCheat.Patches
{
    [HarmonyPatch(typeof(NetworkRunner), nameof(NetworkRunner.Fusion_Simulation_ICallbacks_PlayerJoined))]
    public class OnPlayerJoinedPatch
    {
        public static void Postfix(NetworkRunner __instance, PlayerRef player)
        {
            if (Settings.IsHost)
            {
                try
                {
                    PlayerState playerState = AntiCheatManager.GetPlayerStateByID(player.PlayerId);
                    if (AntiCheatManager.knownModders.Contains(AntiCheatManager.ModerationIDtoSHA256(__instance.GetPlayerUserId(player))))
                    {
                        MelonCoroutines.Start(WaitKickBlacklisted(playerState));
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static IEnumerator WaitKickBlacklisted(PlayerState target)
        {
            yield return new WaitForSeconds(4.5f);
            networkRunner.Disconnect(target.PlayerId);
        }
    }

    [HarmonyPatch(typeof(NetworkedLocomotionPlayer), nameof(NetworkedLocomotionPlayer.RPC_SpawnInitialization))]
    public class SpawnInitPatch
    {
        public static bool Prefix(NetworkedLocomotionPlayer __instance, int color, int hat, int hands, int skin, string name, string moderationID, string moderationUsername, string accountID, bool is3D)
        {
            if (Settings.IsHost)
            {
                try
                {
                    if (AntiCheatManager.knownModders.Contains(AntiCheatManager.ModerationIDtoSHA256(moderationID)))
                    {
                        return false;
                    }
                    else if (moderationID != networkRunner.GetPlayerUserId(__instance.PlayerID))
                    {
                        AntiCheatManager.KickPlayerForCheating(__instance.PState.NetworkName.Value, "Spoofed mod id", __instance.PlayerID);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return true;
        }
    }
}