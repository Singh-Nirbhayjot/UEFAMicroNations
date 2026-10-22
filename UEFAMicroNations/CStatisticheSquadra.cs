using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CStatisticheSquadra : IComparable<CStatisticheSquadra>, IEquatable<CStatisticheSquadra>
    {
        public string Squadra { get; set; }
        public int Punti { get; set; }
        public int Giocate { get; set; }
        public int Vinte { get; set; }
        public int Pareggiate { get; set; }
        public int Perse { get; set; }
        public int GolFatti { get; set; }
        public int GolSubiti { get; set; }

        public CStatisticheSquadra(string squadra)
        {
            if (string.IsNullOrWhiteSpace(squadra))
                throw new ArgumentException("Il nome della squadra non è valido.");

            Squadra = squadra;
            Punti = 0;
            Giocate = 0;
            Vinte = 0;
            Pareggiate = 0;
            Perse = 0;
            GolFatti = 0;
            GolSubiti = 0;
        }

        public int CalcolaDifferenzaReti()
        {
            return GolFatti - GolSubiti;
        }

        public void AggiungiPartita(CPartitaGirone partita, bool inCasa)
        {
            Giocate++;

            if (inCasa)
            {
                GolFatti += partita.GolCasa;
                GolSubiti += partita.GolOspite;
            }
            else
            {
                GolFatti += partita.GolOspite;
                GolSubiti += partita.GolCasa;
            }

            Punti += partita.PuntiPer(Squadra);

            Esito esito = partita.CalcolaEsito();

            if (esito == Esito.Pareggio)
            {
                Pareggiate++;
            }
            else if ((esito == Esito.VittoriaCasa && inCasa) ||
                     (esito == Esito.VittoriaOspite && !inCasa))
            {
                Vinte++;
            }
            else
            {
                Perse++;
            }
        }

        public int CompareTo(CStatisticheSquadra? altra)
        {
            if (altra == null)
                return 1;

            int confronto = altra.Punti.CompareTo(Punti);

            if (confronto != 0)
                return confronto;

            confronto = altra.CalcolaDifferenzaReti().CompareTo(CalcolaDifferenzaReti());

            if (confronto != 0)
                return confronto;

            confronto = altra.GolFatti.CompareTo(GolFatti);

            if (confronto != 0)
                return confronto;

            return Squadra.ToLower().CompareTo(altra.Squadra.ToLower());
        }

        public bool Equals(CStatisticheSquadra? altra)
        {
            if ((object?)altra == null)
                return false;

            return Squadra.ToLower() == altra.Squadra.ToLower();
        }

        public override bool Equals(object? obj)
        {
            CStatisticheSquadra? altra = obj as CStatisticheSquadra;

            if (altra == null)
                return false;

            return Equals(altra);
        }

        public static bool operator ==(CStatisticheSquadra? prima,CStatisticheSquadra? seconda)
        {
            if ((object?)prima == null)
                return (object?)seconda == null;

            return prima.Equals(seconda);
        }
        public static bool operator !=(CStatisticheSquadra? prima,CStatisticheSquadra? seconda)
        {
            return !(prima == seconda);
        }

        public override string ToString()
        {
            return Squadra + " | Pt: " + Punti +
                " | G: " + Giocate +
                " | V: " + Vinte +
                " | N: " + Pareggiate +
                " | P: " + Perse +
                " | GF: " + GolFatti +
                " | GS: " + GolSubiti +
                " | DR: " + CalcolaDifferenzaReti();
        }
    }
}
