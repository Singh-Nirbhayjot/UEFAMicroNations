using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CPartitaEliminazione : CPartita
    {
        private int? rigoriCasa;
        private int? rigoriOspite;

        public int? RigoriCasa
        {
            get { return rigoriCasa; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I rigori non possono essere negativi.");

                rigoriCasa = value;
            }
        }

        public int? RigoriOspite
        {
            get { return rigoriOspite; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I rigori non possono essere negativi.");

                rigoriOspite = value;
            }
        }

        public override string Fase
        {
            get { return "Eliminazione"; }
        }

        public CPartitaEliminazione(int giornata, string squadraCasa,string squadraOspite, int golCasa, int golOspite,int? rigoriCasa = null, int? rigoriOspite = null,DateTime? data = null): base(giornata, squadraCasa, squadraOspite, golCasa, golOspite, data)
        {
            if (golCasa == golOspite)
            {
                if (rigoriCasa == null || rigoriOspite == null)
                    throw new ArgumentException("In caso di pareggio i rigori sono obbligatori.");

                if (rigoriCasa == rigoriOspite)
                    throw new ArgumentException("I rigori non possono finire in pareggio.");
            }
            else
            {
                if (rigoriCasa != null || rigoriOspite != null)
                    throw new ArgumentException("Non devi inserire i rigori se non c'è pareggio.");
            }

            RigoriCasa = rigoriCasa;
            RigoriOspite = rigoriOspite;
        }

        public override string? CalcolaVincitore()
        {
            if (GolCasa > GolOspite)
                return SquadraCasa;

            if (GolOspite > GolCasa)
                return SquadraOspite;

            if (RigoriCasa > RigoriOspite)
                return SquadraCasa;

            return SquadraOspite;
        }

        public override int PuntiPer(string squadra)
        {
            return 0;
        }

        public override string Descrizione()
        {
            string risultato = "G" + Giornata + " - " + SquadraCasa + " " +
                GolCasa + "-" + GolOspite + " " + SquadraOspite;

            if (Data != null)
                risultato += " [" + Data.Value.ToString("dd/MM/yyyy") + "]";

            if (GolCasa == GolOspite)
            {
                risultato += " d.c.r. " + RigoriCasa + "-" + RigoriOspite +", passa " + CalcolaVincitore();
            }
            else
            {
                risultato += ", passa " + CalcolaVincitore();
            }

            return risultato + " (" + Fase + ")";
        }
    }
}
