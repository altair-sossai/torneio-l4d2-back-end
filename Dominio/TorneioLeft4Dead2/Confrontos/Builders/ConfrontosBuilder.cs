using System.Collections.Generic;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Confrontos.Builders
{
    public class ConfrontosBuilder
    {
        private readonly List<TimeEntity> _times;

        public ConfrontosBuilder(List<TimeEntity> times)
        {
            _times = times;
        }

        public IEnumerable<ConfrontoEntity> Build()
        {
            var delta = _times.Count % 2;
            var matriz = new int[_times.Count, _times.Count];

            for (var i = 0; i < _times.Count; i++)
            {
                for (var j = 0; j < _times.Count; j++)
                    matriz[i, j] = -1;
            }

            for (int i = 0, k = 1; i < _times.Count; i++, k = i + 1)
            for (var j = 0; j < _times.Count; j++)
            {
                if (matriz[j, i] != -1)
                    continue;

                if (k >= _times.Count + delta)
                    k = 1;

                matriz[j, i] = k++;

                if (i == j)
                    matriz[_times.Count - 1, j] = matriz[j, i];
            }

            for (var rodada = 1; rodada < _times.Count + delta; rodada++)
            {
                for (var i = 1; i < _times.Count; i++)
                for (var j = 0; j <= i - 1; j++)
                {
                    if (matriz[i, j] != rodada)
                        continue;

                    var entity = new ConfrontoEntity
                    {
                        Rodada = rodada,
                        Status = (int) StatusConfronto.Aguardando,
                        CodigoTimeA = _times[j].Codigo,
                        CodigoTimeB = _times[i].Codigo,
                        CodigoCampanha = rodada
                    };

                    yield return entity;
                }
            }
        }
    }
}