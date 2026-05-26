using Il2CppFusion;
using Il2CppFusion.Photon.Realtime;
using Il2CppInternal.Cryptography;
using Il2CppSG.Airlock;
using Il2CppSG.Platform;
using Il2CppSteamworks;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using static AntiCheat.References;
using static AntiCheat.Settings;
using static Il2Cpp.Interop;

namespace AntiCheat.Managers
{
    public class AntiCheatManager
    {
        private static Dictionary<PlayerState, Vector3> PrevPosition = new Dictionary<PlayerState, Vector3>();
        private static Dictionary<PlayerState, int> SpeedDetections = new Dictionary<PlayerState, int>();
        private static string CheaterModID = "";
        private static float SpeedLimit = 9.5f;
        private static int MaxViolations = 3;

        public static string NameResult = "";

        internal static List<string> knownModders = new List<string>()
        {
            "dd08c79ecfdff018c7fde222e21de977efe9cc613101ec6370f948a0aa696001",
            "2bf3c6274fc582aba3697049e4f2c27003500f48707c08f8fbb4aea5f28f5c7c",
            "373acd78757aaf249c2be156a802edde69309b001391e0bf719ddb2f0bd1ed95",
            "112f855dc6c9ea1348c977dc0e9e3a1484328c14926076ed2ee919a52279fe31",
            "0fc8377a526c2da1640a4f7fa0411f53640b11b741d22216464de78de08164bf",
            "01f50286f95ca96ace92e0e698d77178ac2b7688d1a066111b1e39bddbb888ab",
            "9af953f98f2866c68dab71cdcfde2ce2632a3f455245a8fc16ba5892af3bf2fe",
            "e07cf1d5f26f49734905f5568fbf0324ec7bda8fb3271d3a98151af1eeb12c50",
            "bdff392349932d312560f5e5d404bf95463370681bcecc13b43c70c126697cdd",
            "64c2e4d171f526325389cbf30249abd6d25c2a334ce070e105013083966a4380",
            "36392a0cac918019ab50eb9e115bd59d4f64be773816482426f85b708780089d",
            "d1de9b2e764c2393d5c2c95831054f9a02cd7c8d371275438e8584b373d55026",
            "29e6a4d56916b8430890a2ab36a4d733456ee7519da2c0398b03a36488f123d4",
            "f94a916b67a4caef0f6006f90d8484272dc95766f94776037b8f15fa298be0ba",
            "dca0d5112ac6be3019a8c41b3043fc480e0b318a56c3a02bd0e00deb192a612f",
            "e8bf4282d6b29ad8b11708d745455dba36c06805415071389fcdedaf36cf4b29",
            "41b9b168325e5221a26b82cc9af5f8c5a7d4502448a67d251f4c9c680e2bb024",
            "802ece1db5bd6393c1d585335c2b21bb7b89f3a4761e1078cc9a93f504d3e236",
            "cad3340840e87a6a7d3fd199603f9782a021c157ba388a32e04de3126e187d58",
            "c3cd23202916af1377736ac7968002401cd29afb85ba91c491c0fe75509e22d8",
            "2d1be52cb82bc2fca9f4f0ff3dd56483387bbb882c1a1b99c05d3c567d62d824",
            "2c4fd86f73fab3daf9225e4575743ad75f1bcd65c633fd47b06657d592c3da73",
            "a00ea6c4564a3f119574dc4cda60e5845e95b56fbfd8004dff684d558873f8b0",
            "d3a6f0dad9b7b7b0faa5e955fda53a4bd096ba028e8610598bf66750992a64ab",
            "2714cbfb644d0cde880ba7bac466efb762b6e60a64ef91a958aae82e410eca86",
            "35eaed85ef9d2f61eb38434f160f21ad8685d0aa4c652c4d67356a082df1bc2d",
            "9cb969e6b7ae53560e86e8148e2cc0d85c03515a027f940d1acd287e8027a2ca",
            "861cc86a28c59cdcd7858253263cfc7108ddc9d5b311ec85dfb73d95311a30b8",
            "81b41bc7dad89f14d0c123621a9ccc6d7c6abefb74d006b3c3329bb85ea537d0",
            "1168282cc3767a4a07ec899d8eb64530c49fdbefc747ec528f7bc2efbfc1da73",
            "3188de2db3bae1fc470c35c0daa0b4b8682c8fe516a1ef0de99f4d0cf96df0e9",
            "654965be800831739f7fec538084b57e682258f6348674ec8dcdd275043afedd",
            "d0f1bb9509892efe10011c4d6510c3cf3127f164485ec982624f3481f6b33bd4",
            "ac813706d392cb56ff00426cfb8dd8035ba156cdd4d125bb8165825085438565",
            "a3cb0aa8be9e3583e3fd1362d6b52b6fb7b39c9e0e34891fff8433a92f92023e",
            "29b704c94de61ac4ce79b70d09a041dd2d7cab8b8a09acbb35eb3a4bb06c18d1",
            "9a44a17d252ac8b1a28f41ab0972a37939fcfaa1f38aa31f7323fcd624fe2034",
            "08a9c8dfac447d6b6066c10d65a5c9833bee26c3d0bb96a808ce42c531a1d863",
            "e37de7b884730810b2dc4c677b10555e063632aa06dddaa6f9e0a31694a5bb96",
            "2590d5d781d683d465d7177b3bb675be73eb15dc66bf97e0e936a0216975629d",
            "7cc406a94228819c2ed7a2e422eb75420a5fb398f27ca66d17a80643df68e1fb",
            "e116c68b190732c767fde9e443a3cd6f74a2c3f4d6e78e415fe8c6971cee29d7",
            "3073474bb8cb771e81b974770e0b005ce8f2b3982406b65d6f8e7049e6291e76",
            "4564947a449b34707e6181318b642eda0bbfaded01c25ef6d362cef7251bc151",
            "69601c0e139dd0db363b34b57cdc67f1564050126343c7471bedadae9cbd4461",
            "a1bb89a751f7d45bc9cfbada3707cd00421777edcdecec61c3ad8f71b5e2c821",
            "d0b5a2e3248ca5d9898ff4de3c0dd42efeebb2e1718402bd2c2e2455f4871616",
            "7b8f2554398bd65fa465ae7054c2b49d49d4abd167cfad0c4942b5bc7e5f4b63",
            "70ce70eb8b59b71c9a3a60edb9b40ff4aa1a0a71e756a0a82997dfc1660f42da",
            "e05967e724544d616174193af7a2700dc6ce584e267ee35613da762c936a03d6",
            "9dee7b097d8bacc478850800d630d7f0f46eb3a5b80a70a4358dedd8e92e3c5c",
            "b7f956c4dbdf7ddef35b9b31e0c07e41746a15cf893fdcf4fbb3a91a5661b13e",
            "00239900e15dec04a64014cec4816e8bfb10448922db2148c1940608ed29681c",
            "caf7320232839730528a3d2f61093daf13c08a0e5e92913e2ceeea8d701ac79d",
            "0689fa594d239d9b4579cd3822aee9e2b4432b37d0fb266a5a0e8c1b077c3124",
            "eed687081b18ae9bdfadcc2dc4985b65c39aeb64e7255b2bc8d8633c900af4ba",
            "4dc9cc0a77b7fe3aae6c3b31d968a96756c50d01db616f6b0a51dd124177843e",
            "5a0db91fa796b26e4c155727ae8c059a1041dd1f419cd7893b0c0269207b84fa",
        };

        public static void KickPlayerForCheating(string playerName, string reason, PlayerRef Cheater)
        {
            if (IsHost)
            {
                if (Cheater.PlayerId != 9)
                {
                    CheaterModID = networkRunner.GetPlayerUserId(Cheater);
                    if (CheaterModID.Contains("Steam"))
                    {
                        if (playerName == "null" || playerName == "")
                        {
                            networkRunner.Disconnect(Cheater);
                            Logging.AntiCheatWarn($"Could not get Cheating Player's UserName Kicking anyways for Reason: {reason}");
                        }
                        else
                        {
                            networkRunner.Disconnect(Cheater);
                            Logging.AntiCheatWarn($"Kicked {playerName} for cheating. || Reason: {reason}");
                        }
                    }
                }
            }
        }

        public static void GetPlayerNameViaPlayerState(PlayerRef RequestedPlayer)
        {
            foreach (PlayerState player in Spawn.ActivePlayerStates)
            {
                if (player == null)
                {
                    continue;
                }
                if (player.PlayerId == RequestedPlayer.PlayerId)
                {
                    NameResult = player.PlayerModerationUsername;
                }
            }
        }

        public static bool CheckAliveState(PlayerRef RequestedPlayer)
        {
            foreach (PlayerState player in Spawn.ActivePlayerStates)
            {
                if (player.PlayerId == RequestedPlayer)
                {
                    return player.IsAlive;
                }
            }
            return true;
        }

        public static IEnumerator SpeedCheckLoop()
        {
            while (true)
            {
                foreach (PlayerState player in Spawn.ActivePlayerStates)
                {
                    if (player.PlayerId == 9)
                    {
                        continue;
                    }

                    Vector3 PlayerPosition = player.LocomotionPlayer.NetworkRigidbody.Rigidbody.position;

                    if (PrevPosition.TryGetValue(player, out Vector3 LastPos))
                    {
                        float distance = Vector3.Distance(PlayerPosition, LastPos);
                        float speed = distance / 1f;

                        if (speed > SpeedLimit)
                        {
                            if (!SpeedDetections.ContainsKey(player))
                                SpeedDetections[player] = 0;
                            SpeedDetections[player]++;

                            if (SpeedDetections[player] >= MaxViolations)
                            {
                                KickPlayerForCheating(player.NetworkName.Value, "Speed Hacks", player.PlayerId);
                                SpeedDetections[player] = 0;
                            }
                        }
                        else
                        {
                            SpeedDetections[player] = 0;
                        }
                    }

                    PrevPosition[player] = PlayerPosition;
                    if (player.HatId == 98)
                    {
                        KickPlayerForCheating(player.NetworkName.Value, "DevHat", player.PlayerId);
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        public static PlayerState GetPlayerStateByID(PlayerRef Player)
        {
            foreach (PlayerState player in Spawn.ActivePlayerStates)
            {
                if (player.PlayerId == Player.PlayerId)
                {
                    return player;
                }
            }
            return null!;
        }

        public static string ModerationIDtoSHA256(string input)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

    }
}