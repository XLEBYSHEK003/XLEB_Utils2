using InventorySystem.Items.ThrowableProjectiles;
using Server = Exiled.API.Features.Server;
using Player = Exiled.API.Features.Player;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using Exiled.API.Features;
using UnityEngine;
using PlayerRoles;
using System.Linq;
using Mirror;
using MEC;

namespace XLEB_Utils2.Lobby
{
    public class LobbyMethods
    {
        private readonly Plugin _plugin;
        public LobbyMethods(Plugin plugin) => _plugin = plugin;
        private string text;
        public void LobbyWaitingForPlayer() 
        {
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;

            if (Server.FriendlyFire)
                FriendlyFireConfig.PauseDetector = true;

           CoroutineHandle LobbyTimerT = Timing.RunCoroutine(LobbyTimer());
        }

        public void LobbyRoundStart() 
        {
            foreach (Player player in Player.List)
            {
                player.ClearInventory();
                player.RoleManager.ServerSetRole(RoleTypeId.Spectator, RoleChangeReason.RoundStart);
            }

            foreach (ThrownProjectile throwable in Object.FindObjectsOfType<ThrownProjectile>())
            {
                if (throwable.Rb.velocity.sqrMagnitude <= 1f)
                    continue;

                throwable.transform.position = Vector3.zero;
                Timing.CallDelayed(1f, () => NetworkServer.Destroy(throwable?.gameObject));
            }

            if (Server.FriendlyFire)
            {
                FriendlyFireConfig.PauseDetector = false;
            }
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (Round.IsLobby)
            {
                text = string.Empty;

                text += $"<size={_plugin.Config.TopTextSize}>" + _plugin.Translation.TitleText + "</size>";

                text += "\n" + $"<size={_plugin.Config.BottomTextSize}>" + _plugin.Translation.PlayerCountText + "</size>";

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;
                switch (NetworkTimer)
                {
                    case -2: text = text.Replace("{seconds}", _plugin.Translation.ServerPauseText); break;
                    case -1: text = text.Replace("{seconds}", _plugin.Translation.RoundStartText); break;
                    case 1: text = text.Replace("{seconds}", _plugin.Translation.SecondLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                    case 0: text = text.Replace("{seconds}", _plugin.Translation.RoundStartText); break;
                    default: text = text.Replace("{seconds}", _plugin.Translation.SecondsLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                }

                if (Player.List.Count() == 1)
                {
                    text = text.Replace("{players}", $"{Player.List.Count()} " + _plugin.Translation.PlayerJoinText);
                }
                else
                {
                    text = text.Replace("{players}", $"{Player.List.Count()} " + _plugin.Translation.PlayersJoinText);
                }

                for (int i = 0; i < 25; i++)
                {
                    text += "\n";
                }

                foreach (Player ply in Player.List)
                {
                    ply.ShowHint(text.ToString(), 1f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
