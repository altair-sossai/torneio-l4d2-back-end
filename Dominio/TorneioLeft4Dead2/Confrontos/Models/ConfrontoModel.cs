using TorneioLeft4Dead2.Campanhas.Entidades;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Confrontos.Models
{
    public class ConfrontoModel : ConfrontoEntity
    {
        public CampanhaEntity Campanha { get; set; }
        public TimeModel TimeA { get; set; }
        public TimeModel TimeB { get; set; }
        public TimeModel TimeVencedor { get; set; }
    }
}