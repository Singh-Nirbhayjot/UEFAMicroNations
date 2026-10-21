
using System;
using System.Collections.Generic;

namespace UEFAMicroNations
{
    public class CGestionePartite
    {
        private List<CPartita> partite;

        public CGestionePartite()
        {
            partite = new List<CPartita>();
        }

        public List<CPartita> OttieniPartite()
        {
            return new List<CPartita>(partite);
        }

        public bool AggiungiPartita(CPartita partita)
        {
            if (partita == null)
                return false;

            foreach (CPartita p in partite)
            {
                if (p.Equals(partita))
                    return false;
            }

            partite.Add(partita);
            return true;
        }

        public bool EliminaPartita(CPartita partita)
        {
            if (partita == null)
                return false;

            for (int i = 0; i < partite.Count; i++)
            {
                if (partite[i].Equals(partita))
                {
                    partite.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public CPartita CercaPartita(int giornata, string squadraCasa, string squadraOspite)
        {
            foreach (CPartita partita in partite)
            {
                if (partita.Giornata == giornata &&
                    partita.SquadraCasa.Nome.ToUpper() == squadraCasa.ToUpper() &&
                    partita.SquadraOspite.Nome.ToUpper() == squadraOspite.ToUpper())
                {
                    return partita;
                }
            }

            return null;
        }


        public List<CPartita> PartitePerGiornata(int giornata)
        {
            List<CPartita> risultato = new List<CPartita>();

            foreach (CPartita partita in partite)
            {
                if (partita.Giornata == giornata)
                    risultato.Add(partita);
            }

            return risultato;
        }

        public List<CPartita> TutteLePartite()
        {
            return new List<CPartita>(partite);
        }

        public List<CPartita> VisualizzaPartite()
        {
            return TutteLePartite();
        }

        public void OrdinaPartite()
        {
            for (int i = 0; i < partite.Count - 1; i++)
            {
                for (int j = i + 1; j < partite.Count; j++)
                {
                    if (partite[i].Giornata > partite[j].Giornata)
                    {
                        CPartita temp = partite[i];
                        partite[i] = partite[j];
                        partite[j] = temp;
                    }
                }
            }
        }

        public List<CPartita> CercaSquadra(string nome)
        {
            List<CPartita> risultato = new List<CPartita>();

            foreach (CPartita partita in partite)
            {
                if (partita.SquadraCasa.Nome.ToUpper() == nome.ToUpper() ||
                    partita.SquadraOspite.Nome.ToUpper() == nome.ToUpper())
                {
                    risultato.Add(partita);
                }
            }

            return risultato;
        }

        public CStatisticheSquadra CalcolaStatistiche(CSquadra squadra)
        {
            CStatisticheSquadra statistiche = new CStatisticheSquadra(squadra);

            foreach (CPartita partita in partite)
                statistiche.AggiungiPartita(partita);

            return statistiche;
        }

        public List<CStatisticheSquadra> CalcolaClassifica()
        {
            List<CSquadra> squadre = new List<CSquadra>();

            foreach (CPartita partita in partite)
            {
                if (partita is CPartitaGirone)
                {
                    bool trovataCasa = false;
                    bool trovataOspite = false;

                    foreach (CSquadra squadra in squadre)
                    {
                        if (squadra.Nome.ToUpper() ==
                            partita.SquadraCasa.Nome.ToUpper())
                            trovataCasa = true;

                        if (squadra.Nome.ToUpper() ==
                            partita.SquadraOspite.Nome.ToUpper())
                            trovataOspite = true;
                    }

                    if (!trovataCasa)
                        squadre.Add(partita.SquadraCasa);

                    if (!trovataOspite)
                        squadre.Add(partita.SquadraOspite);
                }
            }

            List<CStatisticheSquadra> classifica =
                new List<CStatisticheSquadra>();

            foreach (CSquadra squadra in squadre)
            {
                CStatisticheSquadra statistiche =
                    CalcolaStatistiche(squadra);

                classifica.Add(statistiche);
            }

            for (int i = 0; i < classifica.Count - 1; i++)
            {
                for (int j = 0; j < classifica.Count - 1 - i; j++)
                {
                    if (classifica[j].CompareTo(classifica[j + 1]) > 0)
                    {
                        CStatisticheSquadra temp = classifica[j];
                        classifica[j] = classifica[j + 1];
                        classifica[j + 1] = temp;
                    }
                }
            }

            return classifica;
        }
    }
}
