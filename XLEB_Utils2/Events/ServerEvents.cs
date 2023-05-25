using MapEditorReborn.API.Features.Objects;
using Exiled.Events.EventArgs.Server;
using Map = Exiled.API.Features.Map;
using XLEB_Utils2.Commands.ScpSwap;
using MapEditorReborn.API.Features;
using Exiled.API.Features.Pickups;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Enums;
using System.Linq;
using UnityEngine;
using PlayerRoles;
using MEC;

namespace XLEB_Utils2.Events
{
    public class ServerEvents
    {
        private readonly Plugin _plugin;
        public ServerEvents(Plugin plugin) => _plugin = plugin;

        public List<CoroutineHandle> CoroutinesStartRound = new List<CoroutineHandle>();
        public List<CoroutineHandle> FastCoroutines = new List<CoroutineHandle>();
        public List<SchematicObject> schemaobject = new List<SchematicObject>();
        ScpSwapComponent swapComponent = new ScpSwapComponent();
        public WarheadEvents WarheadEvents;
        private SchematicObject LobbyRoom;
        public static bool OffFunctions;

        public void OnWaitingForPlayers()
        {
            SetOffFunctions(false);
            ClearCoroutines();

            if(_plugin.Config.FriendlyFireEndRoundEnable && Server.FriendlyFire)
                Server.FriendlyFire = false;

            if(_plugin.Config.LobbyBuilding.SchematicName != "0001")
                LobbyRoom = ObjectSpawner.SpawnSchematic(_plugin.Config.LobbyBuilding.SchematicName, new Vector3(_plugin.Config.LobbyBuilding.x, _plugin.Config.LobbyBuilding.y, _plugin.Config.LobbyBuilding.z));
        }

        public void OnRoundStart()
        {
            ClearCoroutines();
            StartCoroutines();
            UnSpawnScp();
            GetTypesScpInRound();

            if (LobbyRoom != null)
                LobbyRoom.Destroy();

            if (_plugin.Config.PublicLogWebhookEnable)
                Webhook.Webhook.sendDiscordWebhook(_plugin.Config.WebhookUrl, $"Начался новый раунд!\nВ раунде {Player.List.Count()} игроков.\nTPS: {((int)Server.Tps)}", "Информация", "", _plugin.Config.ImageStartRoundWebhook.RandomItem());

            if (_plugin.Config.SchematicList.Count > 0)
                SpawnBuildings();

            if (_plugin.Config.FixSpawnOnStartRound)
                CoroutinesStartRound.Add(Timing.RunCoroutine(FixSpawnStartRound()));
        }

        public void OnRespawnTeam(RespawningTeamEventArgs ev)
        {
            if (_plugin.Config.SquadProtectOnSpawn) 
            {
                FastCoroutines.Add(Timing.RunCoroutine(SpawnProtect(ev.Players)));
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            UnMuteAllPlayers();
            _plugin.WarheadEvents.ClearWarheadCoroutines();
            ClearCoroutines();

            if (_plugin.Config.PublicLogWebhookEnable && _plugin.Config.ImageEndRoundWebhook.ContainsKey(ev.LeadingTeam))
                Webhook.Webhook.sendDiscordWebhook(_plugin.Config.WebhookUrl, $"Раунд закончился!\nВ раунде {Player.List.Count()} игроков.\nПобедили: {GetWinTeam(ev.LeadingTeam)}\nВремя раунда: {(int)Round.ElapsedTime.TotalMinutes}:{(int)Round.ElapsedTime.TotalSeconds}\nTPS: {((int)Server.Tps)}", "Информация", "", _plugin.Config.ImageEndRoundWebhook[ev.LeadingTeam]);

            if (_plugin.Config.FriendlyFireEndRoundEnable && !Server.FriendlyFire)
                Server.FriendlyFire = true;
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
            {LeadingTeam.FacilityForces, "Служба Безопасности Комплекса"},
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

        public void GetTypesScpInRound() 
        {
            foreach (Player player in Player.List) 
            {
                if (player.Role.Side == Side.Scp && !swapComponent.TotalScpInRound.Contains(player.Role.Type)) 
                {
                    swapComponent.TotalScpInRound.Add(player.Role.Type); 
                }
            }  
        }

        public void SpawnBuildings() 
        {
            foreach (SchematicClass schematic in _plugin.Config.SchematicList) 
            {
                schemaobject.Add(ObjectSpawner.SpawnSchematic(schematic.SchematicName, new Vector3(schematic.x, schematic.y, schematic.z)));
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
            
            if(_plugin.Config.CustomIntercomeEnable)
                _plugin.CustomIntercom.CustomIntercomUsual = Timing.RunCoroutine(_plugin.CustomIntercom.СustomIntercom());
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

            if (_plugin.Config.CustomIntercomeEnable)
                _plugin.CustomIntercom.ClearIntercomCoroutines();

            _plugin.WarheadEvents.ClearWarheadCoroutines();
            FastCoroutines.Clear();
        }

        public static void SetOffFunctions(bool value)
        {
            OffFunctions = value;
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

        private IEnumerator<float> ServerBroadcast()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.ServerMessageReTime);

                Map.ShowHint("\n\n\n" + _plugin.Config.ServerMessage.RandomItem(), 5);
            }
        }

        private IEnumerator<float> SpawnProtect(List<Player> players) 
        {
            foreach (Player player in players) 
            {
                player.IsGodModeEnabled = true;
            }
            yield return Timing.WaitForSeconds(_plugin.Config.SpawnProtectTime);

            foreach (Player player in players)
            {
                player.IsGodModeEnabled = false;
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
            yield return Timing.WaitForSeconds(_plugin.Config.CallDelayCheckPluginHealth);

            if (!OffFunctions)
            {
                if (Round.IsLocked)
                    Round.IsLocked = false;

                if (Server.FriendlyFire && _plugin.Config.FriendlyFireEndRoundEnable && !Round.IsEnded)
                    Server.FriendlyFire = false;
            }
        }

        private IEnumerator<float> ClearItems()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.CleanItemsTime);

                if (OffFunctions)
                    break;

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

