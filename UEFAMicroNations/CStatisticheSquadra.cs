using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CStatisticheSquadra : IComparable<CStatisticheSquadra>, IEquatable<CStatisticheSquadra>
    {
        public CSquadra Squadra { get; set; }
        public int PartiteGiocate { get; set; }
        public int Vittorie { get; set; }
        public int Pareggi { get; set; }
        public int Sconfitte { get; set; }
        public int GolFatti { get; set; }
        public int GolSubiti { get; set; }

        public int Punti
        {
            get { return Vittorie * 3 + Pareggi; }
        }

        public int DifferenzaReti
        {
            get { return GolFatti - GolSubiti; }
        }

        public CStatisticheSquadra(CSquadra squadra)
        {
            Squadra = squadra;
            PartiteGiocate = 0;
            Vittorie = 0;
            Pareggi = 0;
            Sconfitte = 0;
            GolFatti = 0;
            GolSubiti = 0;
        }

        public void AggiungiPartita(CPartita partita)
        {
            if (partita == null || !(partita is CPartitaGirone))
                return;
            if (partita.SquadraCasa.Nome.ToUpper() == Squadra.Nome.ToUpper())
            {
                PartiteGiocate++;
                GolFatti += partita.GolCasa;
                GolSubiti += partita.GolOspite;
            }
            else if (partita.SquadraOspite.Nome.ToUpper() == Squadra.Nome.ToUpper())
            {
                PartiteGiocate++;
                GolFatti += partita.GolOspite;
                GolSubiti += partita.GolCasa;
            }
            else
            {
                return;
            }

            if (partita.PuntiPer(Squadra) == 3)
                Vittorie++;
            else if (partita.PuntiPer(Squadra) == 1)
                Pareggi++;
            else
                Sconfitte++;
        }
        public int CompareTo(CStatisticheSquadra altra)
        {
            if (altra == null)
                return -1;

            if (Punti != altra.Punti)
                return altra.Punti.CompareTo(Punti);

            if (DifferenzaReti != altra.DifferenzaReti)
                return altra.DifferenzaReti.CompareTo(DifferenzaReti);

            if (GolFatti != altra.GolFatti)
                return altra.GolFatti.CompareTo(GolFatti);

            return Squadra.Nome.CompareTo(altra.Squadra.Nome);
        }

        public bool Equals(CStatisticheSquadra altra)
        {
            if (altra == null)
                return false;

            return Squadra.Nome.ToUpper() ==
                   altra.Squadra.Nome.ToUpper();
        }
        public override string ToString()
        {
            return Squadra.Nome +
                   " | PG: " + PartiteGiocate +
                   " | V: " + Vittorie +
                   " | P: " + Pareggi +
                   " | S: " + Sconfitte +
                   " | GF: " + GolFatti +
                   " | GS: " + GolSubiti +
                   " | DR: " + DifferenzaReti +
                   " | Punti: " + Punti;
        }
    }
}
