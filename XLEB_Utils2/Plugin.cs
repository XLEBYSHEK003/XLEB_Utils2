﻿using System;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.Events.Handlers;
using XLEB_Utils2.Events;
using HarmonyLib;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Scp096 = Exiled.Events.Handlers.Scp096;
using Warhead = Exiled.Events.Handlers.Warhead;

namespace XLEB_Utils2
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Prefix { get; } = "XLEB_Utils2";
        public override string Name { get; } = "XLEB_Utils2";
        public override string Author { get; } = "XLEB_YSHEK";
        public override Version Version { get; } = new Version(3, 0, 0);
        public override PluginPriority Priority => PluginPriority.High;
        public PlayerEvents PlayerEvents;
        public ServerEvents ServerEvents;
        public WarheadEvents WarheadEvents;
        private Harmony harmony;

        public override void OnEnabled()
        {
            try
            {
                harmony = new Harmony($"com.xleb.XU-{DateTime.Now.Ticks}");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            Log.Info(string.Format("Plugin {0} ({1}) by {2} enabled sucessfully!", Name, Version, Author));
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            Log.Info(string.Format("Plugin {0} ({1}) by {2} disabled!", Name, Version, Author));
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            ServerEvents = new ServerEvents(this);
            PlayerEvents = new PlayerEvents(this);
            WarheadEvents = new WarheadEvents(this);

            Server.WaitingForPlayers += ServerEvents.OnWaitingForPlayers;
            Server.RoundStarted += ServerEvents.OnRoundStart;
            Server.RespawningTeam += ServerEvents.OnRespawnTeam;
            Server.RoundEnded += ServerEvents.OnRoundEnded;

            Scp096.AddingTarget += PlayerEvents.OnSCP095AddTarget;
            Player.UsingRadioBattery += PlayerEvents.OnUsingBattery;
            Player.Hurting += PlayerEvents.OnPlayerHurting;
            Player.ChangingRole += PlayerEvents.OnChangingRole;
            Player.Verified += PlayerEvents.OnPlayerVerified;
            if (Config.PinkCandyDonateDisable)
                Scp330.EatingScp330 += PlayerEvents.OnEatingCandy;
            Player.Died += PlayerEvents.OnDied;

            Warhead.Starting += WarheadEvents.OnWarheadStart;
            Warhead.Detonated += WarheadEvents.OnWarheadDetonated;
            Warhead.Stopping += WarheadEvents.OnWarheadStopping;

            base.OnEnabled();
        }

        public void UnregisterEvents()
        {
            Server.WaitingForPlayers -= ServerEvents.OnWaitingForPlayers;
            Server.RespawningTeam -= ServerEvents.OnRespawnTeam;
            Server.RoundStarted -= ServerEvents.OnRoundStart;
            Server.RoundEnded -= ServerEvents.OnRoundEnded;

            Player.Hurting -= PlayerEvents.OnPlayerHurting;
            Player.UsingRadioBattery -= PlayerEvents.OnUsingBattery;
            Player.Died -= PlayerEvents.OnDied;
            Player.Verified -= PlayerEvents.OnPlayerVerified;
            Player.ChangingRole += PlayerEvents.OnChangingRole;
            Scp096.AddingTarget -= PlayerEvents.OnSCP095AddTarget;
            Scp330.EatingScp330 -= PlayerEvents.OnEatingCandy;

            Warhead.Starting -= WarheadEvents.OnWarheadStart;
            Warhead.Detonated -= WarheadEvents.OnWarheadDetonated;
            Warhead.Stopping -= WarheadEvents.OnWarheadStopping;

            PlayerEvents = null;
            ServerEvents = null;
            WarheadEvents = null;

            base.OnDisabled();
        }
    }
}