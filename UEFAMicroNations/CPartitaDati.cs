
using System;
using System.Collections.Generic;

namespace UEFAMicroNations
{
    public class CPartitaDati
    {
        public string Tipo { get; set; } = "";
        public int Giornata { get; set; }
        public string SquadraCasa { get; set; } = "";
        public string SquadraOspite { get; set; } = "";
        public int GolCasa { get; set; }
        public int GolOspite { get; set; }
        public int? RigoriCasa { get; set; }
        public int? RigoriOspite { get; set; }
        public string? Data { get; set; }
    }
}
