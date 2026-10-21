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
            get { return "Gironi"; }
        }

        public CPartitaGirone(int giornata, CSquadra squadraCasa,CSquadra squadraOspite, int golCasa, int golOspite): base(giornata, squadraCasa, squadraOspite, golCasa, golOspite)
        {
        }
    }
}
