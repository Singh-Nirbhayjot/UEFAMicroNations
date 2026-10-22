using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public enum Esito
    {
        VittoriaCasa,
        Pareggio,
        VittoriaOspite
    }
    public interface IRisultato
    {
        int CalcolaGolTotali();
        int CalcolaScarto();
        Esito CalcolaEsito();
        string? CalcolaVincitore();
    }
}
