using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CPartitaGirone : CPartita
    {
        public override string Fase
        {
            get { return "Girone"; }
        }

        public CPartitaGirone(int giornata, string squadraCasa,string squadraOspite, int golCasa, int golOspite,DateTime? data = null): base(giornata, squadraCasa, squadraOspite, golCasa, golOspite, data)
        {
        }

        public override int PuntiPer(string squadra)
        {
            if (string.IsNullOrWhiteSpace(squadra))
                throw new ArgumentException("Il nome della squadra non è valido.");

            if (squadra.ToLower() == SquadraCasa.ToLower())
            {
                if (GolCasa > GolOspite)
                    return 3;

                if (GolCasa == GolOspite)
                    return 1;

                return 0;
            }
            if (squadra.ToLower() == SquadraOspite.ToLower())
            {
                if (GolOspite > GolCasa)
                    return 3;

                if (GolOspite == GolCasa)
                    return 1;

                return 0;
            }
            throw new ArgumentException("La squadra non partecipa alla partita.");
        }
    }
}
