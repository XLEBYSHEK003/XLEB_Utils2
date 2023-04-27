﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;
using YamlDotNet.Serialization;

namespace XLEB_Utils2
{
    public class Config : IConfig
    {
        [Description("Плагин активирован?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Включить огонь по своим в конце раунда?")]
        public bool EnableFrendlyFireEndRound { get; set; } = true;

        [Description("Отключить урон для связанного человека, кроме урона от SCP?")]
        public bool AntiCuffHurtOn { get; set; } = false;

        [Description("Сообщение игроку, который атакует связанного")]
        public string MessageForAttacker { get; set; } = "Связанных нельзя убивать!";

        [Description("Сообщение игроку при агре скромника")]
        public string AddingTarget096 { get; set; } = "<color=red>Вы цель SCP - 096!</color>";

        [Description("Оповещение о совершении кила. (Форма: Сообщение + ник)")]
        public string KillMessage { get; set; } = "<color=yellow>Вы убили: %PlayerName%</color> ";

        [Description("Время до автоматического старта боеголовки")]
        public float AutoNukeTime { get; set; } = 1200f;

        [Description("Надпись при автоматической старте боеголовки")]
        public string MessageAutoNuke { get; set; } = "<color=red>Ликвидация комплекса неизбежна!</color>";

        [Description("Сделать бесконечное радио?")]
        public bool UnlimitedBattery { get; set; } = true;

        [Description("Автоматическая надпись:")]
        public List<string> ServerMessage { get; set; } = new List<string>
        {
            "<color=#27EC63>Заходи к нам в</color> <color=#31BEF2>Discord</color>",
            "<color=lime>Донат от 50 рублей!</color>"
        };

        [Description("Время повтора автоматических надписей")]
        public float ServerMessageReTime { get; set; } = 180f;

        [Description("Включить мигание света при начале боеголовки?")]
        public bool WarheadFlickerLightEnable { get; set; } = true;

        [Description("Надпись при начале боеголовки")]
        public string TextOnBlackoutStart { get; set; } = "Повреждение центрального энергоузла!";

        [Description("Надпись при окончании боеголовки")]
        public string TextOnBlackoutEnd { get; set; } = "Восстановление центрального энергоузла!";

        [Description("Время блекаута")]
        public float BlackoutTime { get; set; } = 4f;

        [Description("Время до возвращения блекаута")]
        public float BlackoutComeback { get; set; } = 7f;
        [Description("Время до очистки карты от трупов:")]
        public float CleanRagdollTime { get; set; } = 460f;

        [Description("Время до очистки карты от предметов:")]
        public float CleanItemsTime { get; set; } = 600f;

        [Description("Время до очистки карты от предметов:")]
        public string MessageWhenClean { get; set; } = "Молекулярное расщепление предметов";

        [Description("Список запрещённых к очистке предметов")]
        public List<ItemType> NotClearItems { get; set; } = new List<ItemType>
        {
            ItemType.SCP2176,
            ItemType.KeycardO5
        };

        [Description("Запретить розовую конфету донатерам")]
        public bool PinkCandyDonateDisable { get; set; } = true;

        [Description("Включить огонь по своим в конце раунда?")]
        public bool FriendlyFireEndRoundEnable { get; set; } = false;

        [Description("Список ролей, у которых будет кастомные ХП")]
        public Dictionary<RoleTypeId, int> HealthValues { get; set; } = new Dictionary<RoleTypeId, int>()
        {
            {
                RoleTypeId.Scp173, 3200
            },
            {
                RoleTypeId.NtfCaptain, 150
            }
        };

        [Description("Выдавать кастомные префиксы донатерам?")]
        public bool EnableDonatorsRank { get; set; } = true;

        [Description("Префиксы донатерам. (Steam Id 64: Префикс")]
        public Dictionary<string, Prefixes> PrefixesList { get; set; } = new Dictionary<string, Prefixes>()
        {
            {"278423472934@steam", new Prefixes{PrefixName ="Токийский трунь", PrefixColor ="red" } },
            {"278423472944@steam", new Prefixes{PrefixName ="Токийский гуль", PrefixColor ="red" }  },
        };

        [Description("Список запрещенных ролей для использования конфеты")]
        public List<string> DisablePinkCandyNameGroup { get; set; } = new List<string>
        {
            "admin",
            "helper"
        };

        [Description("Включить публичные логи для сервера? Всё по VSR.")]
        public bool PublicLogWebhookEnable { get; set; } = true;

        [Description("Ссылка на вебхук")]
        public string WebhookUrl { get; set; } = "https://";

        [Description("Имя вебхука")]
        public string WebhookName { get; set; } = "https://";

        [Description("Список изображений, которые будут использованы в логах сервера, при старте раунда")]
        public List<string> ImageStartRoundWebhook { get; set; } = new List<string>()
        {
            "https://i.pinimg.com/originals/b8/4b/a5/b84ba5bee7d85caa5704b571b116cf40.png",
            "https://cdn.shazoo.ru/452894_TEq7cLz1VG_christian_bravery_in_the_rox.jpg",
            "http://pm1.narvii.com/7029/501ba8242768d940c4e59950550e9598628035c1r1-1024-630v2_uhq.jpg"
        };

        [Description("Список изображений, которые будут использованы в логах сервера, при окончании раунда")]
        public Dictionary<LeadingTeam, string> ImageEndRoundWebhook { get; set; } = new Dictionary<LeadingTeam, string>()
        {
            [LeadingTeam.ChaosInsurgency] = "https://cdn.discordapp.com/attachments/1011138607764996158/1100463223913861260/2168D6B5E6B6CAAEFC6E7E004328A1299161C505.png",
            [LeadingTeam.Anomalies] = "https://i.pinimg.com/originals/8b/a1/55/8ba1551b75ed52b794f8ae40ad16a746.jpg",
            [LeadingTeam.FacilityForces] = "https://i.ytimg.com/vi/M5xD4pnavtg/maxresdefault.jpg",
            [LeadingTeam.Draw] = "https://img2.joyreactor.cc/pics/post/full/scp-2273-%D0%9A%D0%BB%D0%B0%D1%81%D1%81-%D0%95%D0%B2%D0%BA%D0%BB%D0%B8%D0%B4-%D0%9E%D0%B1%D1%8A%D0%B5%D0%BA%D1%82%D1%8B-SCP-The-SCP-Foundation-5554142.jpeg"
        };
        /*[Description("Банить донатеров за агр скромника в ноклипе?")]
        public bool BanOnAgrScp096InNoclip { get; set; } = true*/

        [Description("Хз что это, не трогай сука, а то сервер удалю!")]
        public bool Debug { get; set; } = true;
    }

    public class Prefixes 
    {
        public string PrefixName { get; set; }
        public string PrefixColor { get; set; }
    }
}

