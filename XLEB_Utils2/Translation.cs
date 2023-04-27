using Exiled.API.Interfaces;
using System.ComponentModel;

namespace XLEB_Utils2 
{
    public class Translation : ITranslation
    {
        [Description("Текст вебхука при окончании раунда")]
        public string RoundEndText { get; set; } = "";
    }
}
