using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Pickups;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using UnityEngine;
using PlayerRoles;
using Map = Exiled.API.Features.Map;
using Random = UnityEngine.Random;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using Exiled.API.Enums;

namespace XLEB_Utils2.Events
{
    public class ServerEvents
    {
        private readonly Plugin _plugin;
        public ServerEvents(Plugin plugin) => _plugin = plugin;
        public WarheadEvents WarheadEvents;
        private SchematicObject LobbyRoom;
        public List<SchematicObject> schemaobject = new List<SchematicObject>();
        public List<CoroutineHandle> CoroutinesStartRound = new List<CoroutineHandle>();
        public List<CoroutineHandle> FastCoroutines = new List<CoroutineHandle>();

        public void OnWaitingForPlayers()
        {
            ClearCoroutines();             

            if(_plugin.Config.FriendlyFireEndRoundEnable && Server.FriendlyFire)
                Server.FriendlyFire = false;

            LobbyRoom = ObjectSpawner.SpawnSchematic("Backrooms", new Vector3(20f, 940f, -40));
        }

        public void OnRoundStart()
        { 
            if(_plugin.Config.PublicLogWebhookEnable)
                Webhook.Webhook.sendDiscordWebhook(_plugin.Config.WebhookUrl, $"Начался новый раунд!\nВ раунде {Player.List.Count()} игроков.\nTPS: {((int)Server.Tps)}", "Информация", "", _plugin.Config.ImageStartRoundWebhook.RandomItem());

            ClearCoroutines();
            Map.ShowHint("<color=#3AD9AE>При некорректном спавне, введите в консоль на Ё .helpme</color>", 12f);
            StartCoroutines();
            LobbyRoom.Destroy();

            UnSpawnScp();
            schemaobject.Add(ObjectSpawner.SpawnSchematic("DetailedGateA", new Vector3(0f, 1000f, 0)));

            if (_plugin.Config.FixSpawnOnStartRound)
                CoroutinesStartRound.Add(Timing.RunCoroutine(FixSpawnStartRound()));
        }

        public void OnRespawnTeam(RespawningTeamEventArgs ev)
        {
            FastCoroutines.Add(Timing.RunCoroutine(SpawnProtect(ev.Players)));
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (_plugin.Config.PublicLogWebhookEnable && _plugin.Config.ImageEndRoundWebhook.ContainsKey(ev.LeadingTeam))
                Webhook.Webhook.sendDiscordWebhook(_plugin.Config.WebhookUrl, $"Раунд закончился!\nВ раунде {Player.List.Count()} игроков.\nПобедили: {GetWinTeam(ev.LeadingTeam)}\nTPS: {((int)Server.Tps)}", "Информация", "", _plugin.Config.ImageEndRoundWebhook[ev.LeadingTeam]);

            if (_plugin.Config.FriendlyFireEndRoundEnable && !Server.FriendlyFire)
                Server.FriendlyFire = true;

            UnMuteAllPlayers();

            _plugin.WarheadEvents.ClearWarheadCoroutines();
            ClearCoroutines();
        }

        #region Различные методы

        public static void UnMuteAllPlayers() 
        {
            foreach (Player player in Player.List)
            {
                player.UnMute();
            }
        }

        private static readonly Dictionary<LeadingTeam, string> TeamList = new Dictionary<LeadingTeam, string>
        {
            {LeadingTeam.Anomalies, "SCP"},
            {LeadingTeam.ChaosInsurgency, "ХАОС"},
            {LeadingTeam.FacilityForces, "Служба Безопаности Комплекса"},
            {LeadingTeam.Draw, "Ничья"},
        };

        public static string GetWinTeam(LeadingTeam team) 
        {
            if (TeamList.ContainsKey(team))
            {
                return TeamList[team];
            }
            else
            {
                return "Неизвестная команда";
            }
        }

        public void DestroyAllBuildings() 
        {
            foreach (SchematicObject _object in schemaobject)
            {
                _object.Destroy();
            }
            schemaobject.Clear();
        }

        public void StartCoroutines()
        {
            CoroutinesStartRound.Add(Timing.RunCoroutine(ServerBroadcast()));
            CoroutinesStartRound.Add(Timing.RunCoroutine(ClearRagdoll()));
            CoroutinesStartRound.Add(Timing.RunCoroutine(ClearItems()));
            CoroutinesStartRound.Add(Timing.RunCoroutine(AutoNuke()));
            CoroutinesStartRound.Add(Timing.RunCoroutine(CheckPluginHealth()));
        }

        public void ClearCoroutines() 
        {
            foreach (CoroutineHandle _coroutine in CoroutinesStartRound)
            {
               Timing.KillCoroutines(_coroutine);
            }
            CoroutinesStartRound.Clear();

            foreach (CoroutineHandle _coroutine in FastCoroutines)
            {
                Timing.KillCoroutines(_coroutine);
            }
            _plugin.WarheadEvents.ClearWarheadCoroutines();
            FastCoroutines.Clear();
        }

        public static void UnSpawnScp() 
        {
            int SCPLeft = 0;

            if (Player.List.Count() <= 20)
                SCPLeft = 1;
            else if (Player.List.Count() <= 25)
                SCPLeft = 1;
            else if (Player.List.Count() <= 30)
                SCPLeft = 2;
            else if (Player.List.Count() <= 35)
                SCPLeft = 2;
            List<Player> Scps = Player.List.Where(x => x.IsScp).ToList();
            Scps.ShuffleList();
            int s = 0;
            int cds = Random.Range(1, 3);
            foreach (Player scp in Scps)
            {
                if (s > SCPLeft)
                {
                    if (cds == 1)
                        scp.Role.Set(RoleTypeId.ClassD);
                    if (cds == 2)
                        scp.Role.Set(RoleTypeId.Scientist);
                    if (cds == 3)
                        scp.Role.Set(RoleTypeId.FacilityGuard);
                }
                s++;
            }
        }
        #endregion
        #region Корутины

        private IEnumerator<float> AutoNuke()
        {
            yield return Timing.WaitForSeconds(_plugin.Config.AutoNukeTime);

            if (!Warhead.IsInProgress)
                Warhead.Start();
            
            Warhead.IsLocked = true;
            Map.Broadcast(6, _plugin.Translation.MessageAutoNuke); 
        }

        private IEnumerator<float> SpawnProtect(List<Player> players)
        {
            foreach (Player player in players)
            {
                player.IsGodModeEnabled = true;
            }

            yield return Timing.WaitForSeconds(10f);

            foreach (Player player in players)
            {
                player.IsGodModeEnabled = false;
            }
        }

        private IEnumerator<float> ServerBroadcast()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.ServerMessageReTime);

                Map.ShowHint("\n\n\n" + _plugin.Config.ServerMessage.RandomItem(), 5);
            }
        }

        private IEnumerator<float> ClearRagdoll()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.CleanRagdollTime);

                foreach (Ragdoll ragdoll in Ragdoll.List)
                {
                    ragdoll.Destroy();
                }
            }
        }

        private IEnumerator<float> FixSpawnStartRound()
        {
            Round.IsLocked = true;

            yield return Timing.WaitForSeconds(_plugin.Config.FixSpawnTimeWaitRun);

            foreach (Player player in Player.List)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.FixSpawnTime);

                player.RoleManager.ServerSetRole(player.Role, RoleChangeReason.RoundStart);
            }

            Round.IsLocked = false;
            Map.Broadcast(7, _plugin.Translation.MessageWhenFixRespawn, Broadcast.BroadcastFlags.Normal);
        }

        private IEnumerator<float> CheckPluginHealth()
        {
            for (; ;) 
            {
                yield return Timing.WaitForSeconds(_plugin.Config.CallDelayCheckPluginHealth);

                if(Round.IsLocked)
                    Round.IsLocked = false;

                if(Server.FriendlyFire && _plugin.Config.FriendlyFireEndRoundEnable && !Round.IsEnded)
                    Server.FriendlyFire = false;          
            }
        }

        private IEnumerator<float> ClearItems()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.CleanItemsTime);
                Map.Broadcast(5, _plugin.Translation.MessageWhenClean);
                foreach (Pickup item in Pickup.List)
                {
                   if (!_plugin.Config.NotClearItems.Contains(item.Info.ItemId))
                      item.Destroy();
                }

            }

        }
        #endregion
    }

}

