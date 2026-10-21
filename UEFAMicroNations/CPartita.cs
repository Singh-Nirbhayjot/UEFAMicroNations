using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public abstract class CPartita : IRisultato, IStampabile, IEquatable<CPartita>, IComparable<CPartita>
    {
        private int giornata;
        private CSquadra squadraCasa;
        private CSquadra squadraOspite;
        private int golCasa;
        private int golOspite;

        public int Giornata
        {
            get => giornata;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La giornata deve essere positiva.");

                giornata = value;
            }
        }

        public CSquadra SquadraCasa
        {
            get => squadraCasa; 
            set
            {
                if (value == null)
                    throw new ArgumentException("La squadra di casa è obbligatoria.");

                if (squadraOspite != null && value.Nome.ToUpper() == squadraOspite.Nome.ToUpper())
                    throw new ArgumentException("Le squadre devono essere diverse.");

                squadraCasa = value;
            }
        }

        public CSquadra SquadraOspite
        {
            get => squadraOspite;
            set
            {
                if (value == null)
                    throw new ArgumentException("La squadra ospite è obbligatoria.");

                if (squadraCasa != null &&
                    value.Nome.ToUpper() == squadraCasa.Nome.ToUpper())
                    throw new ArgumentException("Le squadre devono essere diverse.");

                squadraOspite = value;
            }
        }

        public int GolCasa
        {
            get => golCasa;
            set
            {
                if (value < 0)
                    throw new ArgumentException("I gol non possono essere negativi.");

                golCasa = value;
            }
        }

        public int GolOspite
        {
            get => golOspite;
            set
            {
                if (value < 0)
                    throw new ArgumentException("I gol non possono essere negativi.");

                golOspite = value;
            }
        }

        public abstract string Fase { get; }

        protected CPartita(int giornata, CSquadra squadraCasa,CSquadra squadraOspite, int golCasa, int golOspite)
        {
            Giornata = giornata;
            SquadraCasa = squadraCasa;
            SquadraOspite = squadraOspite;
            GolCasa = golCasa;
            GolOspite = golOspite;
        }

        public virtual CSquadra Vincitore()
        {
            if (GolCasa > GolOspite)
                return SquadraCasa;

            if (GolOspite > GolCasa)
                return SquadraOspite;

            return null;
        }

        public int PuntiPer(CSquadra squadra)
        {
            if (squadra == null)
                return 0;

            if (squadra.Nome.ToUpper() == SquadraCasa.Nome.ToUpper())
            {
                if (GolCasa > GolOspite)
                    return 3;

                if (GolCasa == GolOspite)
                    return 1;

                return 0;
            }

            if (squadra.Nome.ToUpper() == SquadraOspite.Nome.ToUpper())
            {
                if (GolOspite > GolCasa)
                    return 3;

                if (GolOspite == GolCasa)
                    return 1;

                return 0;
            }

            return 0;
        }

        public virtual string Descrizione()
        {
            return $"Giornata {Giornata} - {Fase}: {SquadraCasa.Nome} {GolCasa} - {GolOspite} {SquadraOspite.Nome}";
        }

        public override string ToString()
        {
            return Descrizione();
        }

        public bool Equals(CPartita altra)
        {
            if ((object)altra == null)//faccio cast per far diventare un tipo generico e per non chiamare operatore ==
                return false;

            return Giornata == altra.Giornata &&SquadraCasa.Nome.ToUpper() == altra.SquadraCasa.Nome.ToUpper() &&SquadraOspite.Nome.ToUpper() == altra.SquadraOspite.Nome.ToUpper();
        }

        public static bool operator ==(CPartita a, CPartita b)
        {
            if ((object)a == null && (object)b == null)
                return true;

            if ((object)a == null || (object)b == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(CPartita a, CPartita b)
        {
            return !(a == b);
        }

        public int CompareTo(CPartita altra)
        {
            if (altra == null)
                return 1;

            if (Giornata < altra.Giornata)
                return -1;

            if (Giornata > altra.Giornata)
                return 1;

            int confronto = SquadraCasa.Nome.CompareTo(altra.SquadraCasa.Nome);

            if (confronto != 0)
                return confronto;

            return SquadraOspite.Nome.CompareTo(altra.SquadraOspite.Nome);
        }
    }
}
