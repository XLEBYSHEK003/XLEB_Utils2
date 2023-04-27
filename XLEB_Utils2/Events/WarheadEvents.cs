using MEC;
using System;
using System.Collections.Generic;
using Map = Exiled.API.Features.Map;
using Exiled.Events.EventArgs.Warhead;

namespace XLEB_Utils2.Events
{
    public class WarheadEvents
    {
        private readonly Plugin _plugin;
        public WarheadEvents(Plugin plugin) => _plugin = plugin;
        public List<CoroutineHandle> WarheadOffLights = new List<CoroutineHandle>();

        public void OnWarheadStart(StartingEventArgs ev) 
        {
            ClearWarheadCoroutines();

            if (_plugin.Config.WarheadFlickerLightEnable)         
                WarheadOffLights.Add(Timing.RunCoroutine(WarheadBlackout()));
        }

        public void OnWarheadStopping(StoppingEventArgs ev) 
        {
            ClearWarheadCoroutines();
            Map.ShowHint("\n\n\n\n" + _plugin.Config.TextOnBlackoutEnd);
        }

        public void OnWarheadDetonated() 
        {
            ClearWarheadCoroutines();
        }

        public void ClearWarheadCoroutines() 
        {
            foreach (CoroutineHandle coroutineHandle in WarheadOffLights)
            {
                Timing.KillCoroutines(coroutineHandle);
            }

            WarheadOffLights.Clear();
        }

        public IEnumerator<float> WarheadBlackout() 
        {
            Map.ShowHint("\n\n\n\n" + _plugin.Config.TextOnBlackoutStart);

            for (; ;)
            {
                Map.TurnOffAllLights(_plugin.Config.BlackoutTime, Exiled.API.Enums.ZoneType.HeavyContainment);
                Map.TurnOffAllLights(_plugin.Config.BlackoutTime, Exiled.API.Enums.ZoneType.Surface);
                Map.TurnOffAllLights(_plugin.Config.BlackoutTime, Exiled.API.Enums.ZoneType.LightContainment);

                yield return Timing.WaitForSeconds(_plugin.Config.BlackoutComeback);
            }
        }
    }
}
