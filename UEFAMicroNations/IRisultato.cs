using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public interface IRisultato
    {
        CSquadra Vincitore();
        int PuntiPer(CSquadra squadra);
    }
}
