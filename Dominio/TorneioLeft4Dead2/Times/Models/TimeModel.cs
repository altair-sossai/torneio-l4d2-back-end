using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Models
{
    public class TimeModel : TimeEntity
    {
        public TimeModel()
        {
            Jogadores = new List<JogadorEntity>();
        }

        public JogadorEntity Capitao => Jogadores?.FirstOrDefault();
        public List<JogadorEntity> Jogadores { get; set; }
    }
}