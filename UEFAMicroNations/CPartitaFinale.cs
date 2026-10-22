using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CPartitaFinale : CPartitaEliminazione
    {
        public override string Fase
        {
            get { return "Finale"; }
        }

        public CPartitaFinale(int giornata, string squadraCasa,string squadraOspite, int golCasa, int golOspite, int? rigoriCasa = null, int? rigoriOspite = null, DateTime? data = null) : base(giornata, squadraCasa, squadraOspite, golCasa, golOspite,rigoriCasa, rigoriOspite, data)
        {
        }

        public int RankingStorico()
        {
            return CalcolaGolTotali() * 2;
        }

        public override string Descrizione()
        {
            return base.Descrizione() + " [Ranking: " + RankingStorico() + "]";
        }
    }
}
