using AntiCheat.Managers;
using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCheat.Patches
{
    [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_CallVote), new Type[] { typeof(PlayerRef), typeof(NetworkBool), typeof(RpcInfo) })]
    public class CallEmergencyMeetingPatch
    {
        public static bool Prefix(PlayerRef sourcePlayer, NetworkBool forceVote, RpcInfo info)
        {
            if (Settings.IsHost)
            {
                if (sourcePlayer.IsValid && info.Source.IsValid && (sourcePlayer != info.Source))
                {
                    AntiCheatManager.GetPlayerNameViaPlayerState(info.Source);
                    AntiCheatManager.KickPlayerForCheating(AntiCheatManager.NameResult, "Forced an Emergency Meeting", info.Source);
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_CallVote), new Type[] { typeof(int), typeof(PlayerRef), typeof(NetworkBool), typeof(RpcInfo) })]
    public class ReportBodyPatch
    {
        public static bool Prefix(int foundPlayer, PlayerRef sourcePlayer, NetworkBool forceVote, RpcInfo info)
        {
            if (Settings.IsHost)
            {
                AntiCheatManager.GetPlayerNameViaPlayerState(sourcePlayer);
                if (sourcePlayer.IsValid && info.Source.IsValid && (sourcePlayer != info.Source || foundPlayer == sourcePlayer || AntiCheatManager.CheckAliveState(foundPlayer) != false))
                {
                    AntiCheatManager.GetPlayerNameViaPlayerState(info.Source);
                    AntiCheatManager.KickPlayerForCheating(AntiCheatManager.NameResult, "Forced Body Reporting", info.Source.PlayerId);
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_Vote), new Type[] { typeof(PlayerRef), typeof(PlayerRef), typeof(RpcInfo) })]
    public class VotingPatch
    {
        public static bool Prefix(PlayerRef voteAgainstPlayer, PlayerRef sourcePlayer, RpcInfo info)
        {
            if (Settings.IsHost)
            {
                if (sourcePlayer.IsValid && info.Source.IsValid && (sourcePlayer != info.Source))
                {
                    AntiCheatManager.GetPlayerNameViaPlayerState(info.Source);
                    AntiCheatManager.KickPlayerForCheating(AntiCheatManager.NameResult, "Forced a Vote Against another Player", info.Source);
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_Vote), new Type[] { typeof(PlayerRef), typeof(RpcInfo) })]
    public class SkipVotePatch
    {
        public static bool Prefix(PlayerRef sourcePlayer, RpcInfo info)
        {
            if (Settings.IsHost)
            {
                if (sourcePlayer.IsValid && info.Source.IsValid && (sourcePlayer != info.Source))
                {
                    AntiCheatManager.GetPlayerNameViaPlayerState(info.Source);
                    AntiCheatManager.KickPlayerForCheating(AntiCheatManager.NameResult, "Forced Skip Vote", info.Source);
                    return true;
                }
            }
            return true;
        }
    }
}