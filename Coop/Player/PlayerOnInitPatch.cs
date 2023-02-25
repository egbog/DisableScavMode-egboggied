﻿using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using Newtonsoft.Json;
using SIT.Coop.Core.Web;
using SIT.Core.Coop;
using SIT.Tarkov.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIT.Coop.Core.Player
{
    internal class PlayerOnInitPatch : ModulePatch
    {
        private static ConfigFile _config;
        public PlayerOnInitPatch(ConfigFile config)
        {
            _config = config;
        }

        protected override MethodBase GetTargetMethod()
        {
            return PatchConstants.GetMethodForType(typeof(EFT.LocalPlayer), "Init");
        }

        //[PatchPrefix]
        //public static
        //  bool
        //  PatchPrefix(EFT.LocalPlayer __instance)
        //{
        //    var EnableAISpawnWaveSystem = _config.Bind("Server", "Enable AI Spawn Wave System", true
        //                           , new ConfigDescription("Whether to run the Wave Spawner System. Useful for testing.")).Value;

        //    var result = !__instance.IsAI || (!Matchmaker.MatchmakerAcceptPatches.IsClient && EnableAISpawnWaveSystem);
        //    return result;
        //}

        //[PatchPostfix]
        //public static
        //    async
        //    void
        //    PatchPostfix(Task __result, EFT.LocalPlayer __instance)
        //{

        [PatchPostfix]
        public static void PatchPostfix(EFT.LocalPlayer __instance)
        {
            var player = __instance;
            var accountId = player.Profile.AccountId;

            //await __result;
            Logger.LogInfo($"Init. {accountId}");


           

            var gameWorld = Singleton<GameWorld>.Instance;
            var coopGC = gameWorld.GetComponent<CoopGameComponent>();
            if (coopGC == null)
            {
                Logger.LogError("Cannot add player to Coop Game Component because its NULL");
                return;
            }

            if (!coopGC.Players.ContainsKey(accountId))
            {
                coopGC.Players.TryAdd(accountId, player);
            }


            Dictionary<string, object> dictionary2 = new Dictionary<string, object>
                    {
                        {
                            "SERVER",
                            "SERVER"
                        },
                        {
                            "serverId",
                            coopGC.ServerId
                        },
                        {
                            "isAI",
                            player.IsAI
                        },
                        {
                            "accountId",
                            player.Profile.AccountId
                        },
                        {
                            "profileId",
                            player.ProfileId
                        },
                        {
                            "groupId",
                            Matchmaker.MatchmakerAcceptPatches.GetGroupId()
                        },
                        {
                            "sPx",
                            player.Transform.position.x
                        },
                        {
                            "sPy",
                            player.Transform.position.y
                        },
                        {
                            "sPz",
                            player.Transform.position.z
                        },
                        { "m", "PlayerSpawn" },
                        {
                            "p.info",
                            JsonConvert.SerializeObject(player.Profile.Info
                                , Formatting.None
                                , new JsonSerializerSettings() { })//.SITToJson()
                        },
                        {
                            "p.cust",
                             player.Profile.Customization.SITToJson()
                        },
                        {
                            "p.equip",
                            //player.Profile.Inventory.Equipment.CloneItem().ToJson()
                            player.Profile.Inventory.Equipment.SITToJson()
                        }
                    };
            //new Request().PostJson("/client/match/group/server/players/spawn", dictionary2.ToJson());

            var prc = player.GetOrAddComponent<PlayerReplicatedComponent>();
            prc.player = player;
            ServerCommunication.PostLocalPlayerData(player, dictionary2);

            CoopGameComponent.GetCoopGameComponent().Players.TryAdd(player.Profile.AccountId, player);
        }

        public static void SendOrReceiveSpawnPoint(EFT.Player player)
        {
            var position = player.Transform.position;
            if (!Matchmaker.MatchmakerAcceptPatches.IsClient)
            {
                Dictionary<string, object> value2 = new Dictionary<string, object>
                {
                    {
                        "m",
                        "SpawnPointForCoop"
                    },
                    {
                        "playersSpawnPointx",
                        position.x
                    },
                    {
                        "playersSpawnPointy",
                        position.y
                    },
                    {
                        "playersSpawnPointz",
                        position.z
                    }
                };
                Logger.LogInfo("Setting Spawn Point to " + position);
                ServerCommunication.PostLocalPlayerData(player, value2);
            }
            else
            {

            }
        }
    }
}
