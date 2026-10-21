using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public interface IRepository<T>
    {
        void Aggiungi(T elemento);
        List<T> LeggiTutti();
        void Salva();
        void Carica();
    }
}
