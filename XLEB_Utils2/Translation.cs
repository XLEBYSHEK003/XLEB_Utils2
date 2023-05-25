using Exiled.API.Interfaces;
using System.ComponentModel;

namespace XLEB_Utils2 
{
    public class Translation : ITranslation
    {
        [Description("Текст вебхука при окончании раунда")]
        public string RoundEndText { get; set; } = "";

        [Description("Надпись при начале боеголовки")]
        public string TextOnBlackoutStart { get; set; } = "Повреждение центрального энергоузла!";

        [Description("Надпись при окончании боеголовки")]
        public string TextOnBlackoutEnd { get; set; } = "Восстановление центрального энергоузла!";

        [Description("Сообщение при очистке карты от предметов")]
        public string MessageWhenClean { get; set; } = "Молекулярное расщепление предметов";

        [Description("Сообщение игроку, который атакует связанного")]
        public string MessageForAttacker { get; set; } = "Связанных нельзя убивать!";

        [Description("Сообщение игроку при агре скромника")]
        public string AddingTarget096 { get; set; } = "<color=red>Вы цель SCP - 096!</color>";

        [Description("Оповещение о совершении кила. (Форма: Сообщение + ник)")]
        public string KillMessage { get; set; } = "<color=yellow>Вы убили: %PlayerName%</color> ";

        [Description("Надпись при автоматическом старте боеголовки")]
        public string MessageAutoNuke { get; set; } = "<color=red>Ликвидация комплекса неизбежна!</color>";

        [Description("Надпись при переспавне людей, если включён фикс спавна")]
        public string MessageWhenFixRespawn { get; set; } = "Все были переспавнены, для избежания проблем со спавном";

        [Description("Кастомный интерком. Надпись при ожидании. Желательно не трогать!")]
        public string TextIntercomeWaitSpeak { get; set; } = "<size=70%>Сводка C.A.S.S.I.E</size>\n\n<size=35%>SCP на свободе: <color=#E81116>%alive_scp%</color>\nОбнаружено живых людей на территории: <color=#E81116>%alive_people%</color>\nTPS: <color=#E81116>%tps%</color>\n\n<color=#11C1E8>Статус интеркома:</color>\nГОТОВ</size>";

        [Description("Кастомный интерком. Надпись при вещании")]
        public string TextIntercomeOnSpeak { get; set; } = "<size=70%>Сводка C.A.S.S.I.E</size>\n\n<size=35%>SCP на свободе: <color=#E81116>%alive_scp%</color>\nОбнаружено живых людей на территории: <color=#E81116>%alive_people%</color>\nTPS: <color=#E81116>%tps%</color>\n\n<color=#E81411>Статус интеркома:</color>\nЗАНЯТ</size>";

        [Description("Кастомный интерком. Надпись при перезагрузке интеркома")]
        public string TextIntercomeReload { get; set; } = "<size=70%>Сводка C.A.S.S.I.E</size>\n\n<size=35%>SCP на свободе: <color=#E81116>%alive_scp%</color>\nОбнаружено живых людей на территории: <color=#E81116>%alive_people%</color>\nTPS: <color=#E81116>%tps%</color>\n\n<color=#E8DA11>Статус интеркома:</color>\nПЕРЕЗАГРУЗКА</size>";

        [Description("Кастомный интерком. Надпись при угрозе детонации боеголовки")]
        public string TextIntercomeWarhead { get; set; } = "<size=70%>Сводка C.A.S.S.I.E</size>\n\n<size=35%>SCP на свободе: <color=#E81116>%alive_scp%</color>\nОбнаружено живых людей на территории: <color=#E81116>%alive_people%</color>\nTPS: <color=#E81116>%tps%</color>\n\n<color=#E8DA11>Статус интеркома:</color>\nПЕРЕЗАГРУЗКА</size>";
    }
}
