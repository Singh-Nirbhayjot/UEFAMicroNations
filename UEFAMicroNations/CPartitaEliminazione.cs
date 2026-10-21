using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CPartitaEliminazione : CPartita
    {
        private int? rigoriCasa; //int? significa che può essere intero o null
        private int? rigoriOspite; //possono essere null perchè non è detto che la partita arriva a rig

        public override string Fase
        {
            get { return "Eliminazione"; }
        }

        public int? RigoriCasa
        {
            get { return rigoriCasa; }
            set
            {
                if (GolCasa == GolOspite && value == null)
                    throw new ArgumentException("In caso di pareggio devi inserire i rigori.");

                if (GolCasa != GolOspite && value != null)
                    throw new ArgumentException("I rigori servono solo in caso di pareggio.");

                if (value != null && value < 0)
                    throw new ArgumentException("I rigori non possono essere negativi.");

                rigoriCasa = value;
            }
        }

        public int? RigoriOspite
        {
            get { return rigoriOspite; }
            set
            {
                if (GolCasa == GolOspite && value == null)
                    throw new ArgumentException("In caso di pareggio devi inserire i rigori.");

                if (GolCasa != GolOspite && value != null)
                    throw new ArgumentException("I rigori servono solo in caso di pareggio.");

                if (value != null && value < 0)
                    throw new ArgumentException("I rigori non possono essere negativi.");

                rigoriOspite = value;
            }
        }

        public CPartitaEliminazione(int giornata, CSquadra squadraCasa,CSquadra squadraOspite, int golCasa, int golOspite,int? rigoriCasa, int? rigoriOspite): base(giornata, squadraCasa, squadraOspite, golCasa, golOspite)
        {
            if (golCasa == golOspite)
            {
                if (rigoriCasa == null || rigoriOspite == null)
                    throw new ArgumentException("Devi inserire entrambi i rigori.");

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

        public override CSquadra Vincitore()
        {
            if (GolCasa > GolOspite)
                return SquadraCasa;

            if (GolOspite > GolCasa)
                return SquadraOspite;

            if (RigoriCasa > RigoriOspite)
                return SquadraCasa;

            return SquadraOspite;
        }

        public override string Descrizione()
        {
            string risultato = base.Descrizione();

            if (RigoriCasa != null && RigoriOspite != null)
                risultato += " (rigori " + RigoriCasa + "-" + RigoriOspite + ")";

            return risultato;
        }
    }
}
