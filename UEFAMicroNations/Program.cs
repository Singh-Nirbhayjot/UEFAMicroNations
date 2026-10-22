using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    internal class Program
    {
        static CRepositoryPartite repository = new CRepositoryPartite();

        static void Main(string[] args)
        {
            try
            {
                repository.Carica();
            }
            catch (Exception e)
            {
                Console.WriteLine("Errore nel caricamento: " + e.Message);
            }

            int scelta;

            do
            {
                do
                {
                    MostraMenu();
                }
                while (!int.TryParse(Console.ReadLine(), out scelta) ||
                       scelta < 0 || scelta > 6);

                try
                {
                    switch (scelta)
                    {
                        case 1:
                            VisualizzaNaturale();
                            break;

                        case 2:
                            VisualizzaConCriterio();
                            break;

                        case 3:
                            InserisciPartita();
                            break;

                        case 4:
                            MostraClassifica();
                            break;

                        case 5:
                            CercaSquadra();
                            break;

                        case 6:
                            EliminaPartita();
                            break;

                        case 0:
                            Console.WriteLine("Uscita dal programma.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Errore: " + e.Message);
                }

            } while (scelta != 0);
        }

        static void MostraMenu()
        {
            Console.WriteLine("\n=== UEFA MicroNations Championship ===");
            Console.WriteLine("1. Visualizza partite (ordine naturale)");
            Console.WriteLine("2. Visualizza partite (altro criterio)");
            Console.WriteLine("3. Inserisci partita");
            Console.WriteLine("4. Classifica gironi");
            Console.WriteLine("5. Cerca partite di una squadra");
            Console.WriteLine("6. Elimina partita");
            Console.WriteLine("0. Esci");
            Console.Write("Scelta: ");
        }

        static void VisualizzaNaturale()
        {
            CPartita.Criterio = "naturale";

            List<CPartita> lista = new List<CPartita>(repository.GetElenco());

            lista.Sort();
            StampaLista(lista);
        }

        static void VisualizzaConCriterio()
        {
            Console.WriteLine("Criteri: naturale, gol, scarto, casaAZ, casaZA, data");
            Console.Write("Scegli criterio: ");

            string criterio = Console.ReadLine() ?? "";

            if (criterio != "naturale" && criterio != "gol" && criterio != "scarto" && criterio != "casaAZ" && criterio != "casaZA" && criterio != "data")
            {
                Console.WriteLine("Criterio non valido.");
                return;
            }

            CPartita.Criterio = criterio;

            List<CPartita> lista = new List<CPartita>(repository.GetElenco());

            lista.Sort();
            StampaLista(lista);
        }

        static void StampaLista(List<CPartita> lista)
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("Nessuna partita presente.");
                return;
            }

            foreach (CPartita partita in lista)
            {
                Console.WriteLine(partita.Descrizione());
            }
        }

        static void InserisciPartita()
        {
            Console.Write("Tipo di partita (girone, eliminazione, finale): ");

            string tipo = (Console.ReadLine() ?? "").ToLower();

            if (tipo != "girone" && tipo != "eliminazione" && tipo != "finale")
            {
                Console.WriteLine("Tipo non valido.");
                return;
            }

            Console.Write("Giornata: ");
            int giornata = LeggiInteroPositivo();

            Console.Write("Squadra di casa: ");
            string squadraCasa = Console.ReadLine() ?? "";

            Console.Write("Squadra ospite: ");
            string squadraOspite = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(squadraCasa) ||
                string.IsNullOrWhiteSpace(squadraOspite))
            {
                Console.WriteLine("Nome della squadra non valido.");
                return;
            }

            if (squadraCasa.ToLower() == squadraOspite.ToLower())
            {
                Console.WriteLine("Le squadre devono essere diverse.");
                return;
            }

            Console.Write("Data (dd/MM/yyyy): ");
            string testoData = Console.ReadLine() ?? "";
            DateTime? data = null;

            if (!string.IsNullOrWhiteSpace(testoData))
            {
                DateTime dataLetta;

                if (DateTime.TryParse(testoData, out dataLetta))
                {
                    data = dataLetta;
                }
                else
                {
                    Console.WriteLine("Data non valida.");
                }
            }

            Console.Write("Gol casa: ");
            int golCasa = LeggiInteroNonNegativo();

            Console.Write("Gol ospite: ");
            int golOspite = LeggiInteroNonNegativo();

            CPartita nuova;

            if (tipo == "girone")
            {
                nuova = new CPartitaGirone(giornata, squadraCasa, squadraOspite, golCasa, golOspite, data);
            }
            else
            {
                int? rigoriCasa = null;
                int? rigoriOspite = null;

                if (golCasa == golOspite)
                {
                    Console.WriteLine("Pareggio. Si va ai rigori");

                    do
                    {
                        Console.Write("Rigori casa: ");
                        rigoriCasa = LeggiInteroNonNegativo();

                        Console.Write("Rigori ospite: ");
                        rigoriOspite = LeggiInteroNonNegativo();

                        if (rigoriCasa == rigoriOspite)
                            Console.WriteLine("I rigori devono avere un vincitore.");
                    }
                    while (rigoriCasa == rigoriOspite);
                }

                if (tipo == "finale")
                {
                    nuova = new CPartitaFinale(giornata, squadraCasa, squadraOspite,golCasa, golOspite, rigoriCasa, rigoriOspite, data);
                }
                else
                {
                    nuova = new CPartitaEliminazione(giornata, squadraCasa, squadraOspite,golCasa, golOspite, rigoriCasa, rigoriOspite, data);
                }
            }

            if (repository.Aggiungi(nuova))
            {
                repository.Salva();
                Console.WriteLine("Partita inserita e salvata.");
            }
            else
            {
                Console.WriteLine("Partita già presente");
            }
        }

        static void MostraClassifica()
        {
            List<CStatisticheSquadra> classifica = new List<CStatisticheSquadra>();

            foreach (CPartita partita in repository.GetElenco())
            {
                if (partita is CPartitaGirone girone)
                {
                    AggiornaStatistica(classifica, girone.SquadraCasa, girone, true);
                    AggiornaStatistica(classifica, girone.SquadraOspite, girone, false);
                }
            }

            if (classifica.Count == 0)
            {
                Console.WriteLine("Non ci sono partite dei gironi.");
                return;
            }

            classifica.Sort();

            Console.WriteLine("\n--- CLASSIFICA GIRONI ---");

            int posizione = 1;

            foreach (CStatisticheSquadra squadra in classifica)
            {
                Console.WriteLine(posizione + ") " + squadra.ToString());
                posizione++;
            }
        }

        static void AggiornaStatistica(List<CStatisticheSquadra> classifica,
            string squadra, CPartitaGirone partita, bool inCasa)
        {
            CStatisticheSquadra? statistica = null;

            foreach (CStatisticheSquadra elemento in classifica)
            {
                if (elemento.Squadra.ToLower() == squadra.ToLower())
                {
                    statistica = elemento;
                    break;
                }
            }

            if (statistica == null)
            {
                statistica = new CStatisticheSquadra(squadra);
                classifica.Add(statistica);
            }

            statistica.AggiungiPartita(partita, inCasa);
        }

        static void CercaSquadra()
        {
            Console.Write("Nome squadra: ");
            string squadra = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(squadra))
            {
                Console.WriteLine("Nome non valido.");
                return;
            }

            List<CPartita> trovate = new List<CPartita>();

            foreach (CPartita partita in repository.GetElenco())
            {
                if (partita.SquadraCasa.ToLower() == squadra.ToLower() ||
                    partita.SquadraOspite.ToLower() == squadra.ToLower())
                {
                    trovate.Add(partita);
                }
            }

            StampaLista(trovate);
        }

        static void EliminaPartita()
        {
            Console.Write("Giornata: ");
            int giornata = LeggiInteroPositivo();

            Console.Write("Squadra di casa: ");
            string squadraCasa = Console.ReadLine() ?? "";

            Console.Write("Squadra ospite: ");
            string squadraOspite = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(squadraCasa) ||
                string.IsNullOrWhiteSpace(squadraOspite))
            {
                Console.WriteLine("Nome della squadra non valido.");
                return;
            }

            if (squadraCasa.ToLower() == squadraOspite.ToLower())
            {
                Console.WriteLine("Le squadre devono essere diverse.");
                return;
            }

            CPartita partitaDaEliminare = new CPartitaGirone(giornata, squadraCasa, squadraOspite, 0, 0);

            if (repository.Rimuovi(partitaDaEliminare))
            {
                repository.Salva();
                Console.WriteLine("Partita eliminata e archivio salvato.");
            }
            else
            {
                Console.WriteLine("Partita non trovata.");
            }
        }

        static int LeggiInteroPositivo()
        {
            int valore;

            while (!int.TryParse(Console.ReadLine(), out valore) || valore <= 0)
            {
                Console.Write("Inserisci un intero positivo: ");
            }

            return valore;
        }
        static int LeggiInteroNonNegativo()
        {
            int valore;

            while (!int.TryParse(Console.ReadLine(), out valore) || valore < 0)
            {
                Console.Write("Inserisci un intero non negativo: ");
            }

            return valore;
        }
    }
}
