using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public abstract class CPartita : IRisultato, IStampabile,IComparable<CPartita>, IEquatable<CPartita>
    {
        public static string Criterio = "naturale";

        private int giornata;
        private string squadraCasa = "";
        private string squadraOspite = "";
        private int golCasa;
        private int golOspite;

        public int Giornata
        {
            get { return giornata; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La giornata deve essere positiva.");

                giornata = value;
            }
        }

        public string SquadraCasa
        {
            get { return squadraCasa; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La squadra di casa non è valida.");

                squadraCasa = value;
            }
        }

        public string SquadraOspite
        {
            get { return squadraOspite; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La squadra ospite non è valida.");

                squadraOspite = value;
            }
        }

        public int GolCasa
        {
            get { return golCasa; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I gol non possono essere negativi.");

                golCasa = value;
            }
        }

        public int GolOspite
        {
            get { return golOspite; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I gol non possono essere negativi.");

                golOspite = value;
            }
        }

        public DateTime? Data { get; set; }

        public abstract string Fase { get; }

        public CPartita(int giornata, string squadraCasa, string squadraOspite,int golCasa, int golOspite, DateTime? data = null)
        {
            if (squadraCasa.ToLower() == squadraOspite.ToLower())
                throw new ArgumentException("Le squadre devono essere diverse.");

            Giornata = giornata;
            SquadraCasa = squadraCasa;
            SquadraOspite = squadraOspite;
            GolCasa = golCasa;
            GolOspite = golOspite;
            Data = data;
        }

        public int CalcolaGolTotali()
        {
            return GolCasa + GolOspite;
        }

        public int CalcolaScarto()
        {
            return Math.Abs(GolCasa - GolOspite);
        }

        public Esito CalcolaEsito()
        {
            if (GolCasa > GolOspite)
                return Esito.VittoriaCasa;

            if (GolCasa < GolOspite)
                return Esito.VittoriaOspite;

            return Esito.Pareggio;
        }

        public virtual string? CalcolaVincitore()
        {
            if (GolCasa > GolOspite)
                return SquadraCasa;

            if (GolOspite > GolCasa)
                return SquadraOspite;

            return null;
        }

        public abstract int PuntiPer(string squadra);

        public virtual string Descrizione()
        {
            string risultato = "G" + Giornata + " - " + SquadraCasa + " " +GolCasa + "-" + GolOspite + " " + SquadraOspite;

            if (Data != null)
                risultato += " [" + Data.Value.ToString("dd/MM/yyyy") + "]";

            return risultato + " (" + Fase + ")";
        }

        public bool Equals(CPartita? altra)
        {
            if ((object?)altra == null)
                return false;

            return Giornata == altra.Giornata && SquadraCasa.ToLower() == altra.SquadraCasa.ToLower() && SquadraOspite.ToLower() == altra.SquadraOspite.ToLower();
        }

        public override bool Equals(object? obj)
        {
            CPartita? altra = obj as CPartita;

            if (altra == null)
                return false;

            return Equals(altra);
        }

        public static bool operator ==(CPartita? prima, CPartita? seconda)
        {
            if ((object?)prima == null)
                return (object?)seconda == null;

            return prima.Equals(seconda);
        }

        public static bool operator !=(CPartita? prima, CPartita? seconda)
        {
            return !(prima == seconda);
        }

        public int CompareTo(CPartita? altra)
        {
            if (altra == null)
                return 1;

            if (Criterio == "gol")
            {
                int confronto = altra.CalcolaGolTotali().CompareTo(CalcolaGolTotali());

                if (confronto != 0)
                    return confronto;
            }
            else if (Criterio == "scarto")
            {
                int confronto = altra.CalcolaScarto().CompareTo(CalcolaScarto());

                if (confronto != 0)
                    return confronto;
            }
            else if (Criterio == "casaAZ")
            {
                int confronto = SquadraCasa.ToLower().CompareTo(altra.SquadraCasa.ToLower());

                if (confronto != 0)
                    return confronto;
            }
            else if (Criterio == "casaZA")
            {
                int confronto = altra.SquadraCasa.ToLower().CompareTo(SquadraCasa.ToLower());

                if (confronto != 0)
                    return confronto;
            }
            else if (Criterio == "data")
            {
                DateTime d1 = Data ?? DateTime.MaxValue;
                DateTime d2 = altra.Data ?? DateTime.MaxValue;

                int confronto = d1.CompareTo(d2);

                if (confronto != 0)
                    return confronto;
            }

            int confrontoGiornata = Giornata.CompareTo(altra.Giornata);

            if (confrontoGiornata != 0)
                return confrontoGiornata;

            int confrontoCasa = SquadraCasa.ToLower().CompareTo(altra.SquadraCasa.ToLower());

            if (confrontoCasa != 0)
                return confrontoCasa;

            return SquadraOspite.ToLower().CompareTo(altra.SquadraOspite.ToLower());
        }
    }
}
