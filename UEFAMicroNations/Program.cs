using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CGestionePartite gestione = new CGestionePartite();
            int scelta;

            do
            {
                Console.WriteLine("\nUEFA MicroNations Championship");
                Console.WriteLine("1. Inserisci partita dei gironi");
                Console.WriteLine("2. Inserisci partita a eliminazione");
                Console.WriteLine("3. Visualizza tutte le partite");
                Console.WriteLine("4. Visualizza classifica");
                Console.WriteLine("5. Ordina le partite");
                Console.WriteLine("6. Cerca partite di una squadra");
                Console.WriteLine("7. Elimina una partita");
                Console.WriteLine("0. Esci");
                Console.Write("Scelta: ");

                if (!int.TryParse(Console.ReadLine(), out scelta))
                    scelta = -1;

                try
                {
                    switch (scelta)
                    {
                        case 1:
                            InserisciPartitaGirone(gestione);
                            break;

                        case 2:
                            InserisciPartitaEliminazione(gestione);
                            break;

                        case 3:
                            List<CPartita> partite = gestione.VisualizzaPartite();

                            if (partite.Count == 0)
                            {
                                Console.WriteLine("Non ci sono partite.");
                            }
                            else
                            {
                                foreach (CPartita partita in partite)
                                    Console.WriteLine(partita.Descrizione());
                            }
                            break;

                        case 4:
                            List<CStatisticheSquadra> classifica =
                                gestione.CalcolaClassifica();

                            if (classifica.Count == 0)
                            {
                                Console.WriteLine("Non ci sono partite dei gironi.");
                            }
                            else
                            {
                                Console.WriteLine("\nCLASSIFICA DEI GIRONI");

                                for (int i = 0; i < classifica.Count; i++)
                                {
                                    Console.WriteLine(
                                        (i + 1) + ". " + classifica[i]);
                                }
                            }
                            break;

                        case 5:
                            gestione.OrdinaPartite();
                            Console.WriteLine("Partite ordinate per giornata.");
                            break;

                        case 6:
                            Console.Write("Nome della squadra: ");
                            string nome = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(nome))
                            {
                                Console.WriteLine("Nome non valido.");
                                break;
                            }

                            List<CPartita> trovate =
                                gestione.CercaSquadra(nome);

                            if (trovate.Count == 0)
                            {
                                Console.WriteLine("Nessuna partita trovata.");
                            }
                            else
                            {
                                foreach (CPartita partita in trovate)
                                    Console.WriteLine(partita.Descrizione());
                            }
                            break;

                        case 7:
                            Console.Write("Giornata: ");
                            int giornata = LeggiInteroPositivo();

                            Console.Write("Squadra di casa: ");
                            string casa = Console.ReadLine();

                            Console.Write("Squadra ospite: ");
                            string ospite = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(casa) ||
                                string.IsNullOrWhiteSpace(ospite))
                            {
                                Console.WriteLine("Nome non valido.");
                                break;
                            }

                            CPartita daEliminare =
                                gestione.CercaPartita(giornata, casa, ospite);

                            if (daEliminare == null)
                            {
                                Console.WriteLine("Partita non trovata.");
                            }
                            else if (gestione.EliminaPartita(daEliminare))
                            {
                                Console.WriteLine("Partita eliminata.");
                            }
                            else
                            {
                                Console.WriteLine("Impossibile eliminare la partita.");
                            }
                            break;

                        case 0:
                            Console.WriteLine("Uscita dal programma.");
                            break;

                        default:
                            Console.WriteLine("Scelta non valida.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Errore: " + e.Message);
                }

            } while (scelta != 0);
        }

        static void InserisciPartitaGirone(CGestionePartite gestione)
        {
            Console.WriteLine("\nInserimento partita dei gironi");

            Console.Write("Giornata: ");
            int giornata = LeggiInteroPositivo();

            Console.Write("Squadra di casa: ");
            string nomeCasa = Console.ReadLine();

            Console.Write("Squadra ospite: ");
            string nomeOspite = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nomeCasa) ||
                string.IsNullOrWhiteSpace(nomeOspite))
            {
                Console.WriteLine("Nome non valido.");
                return;
            }

            CSquadra casa = new CSquadra(nomeCasa);
            CSquadra ospite = new CSquadra(nomeOspite);

            Console.Write("Gol casa: ");
            int golCasa = LeggiInteroNonNegativo();

            Console.Write("Gol ospite: ");
            int golOspite = LeggiInteroNonNegativo();

            CPartitaGirone partita = new CPartitaGirone(
                giornata, casa, ospite, golCasa, golOspite);

            if (gestione.AggiungiPartita(partita))
                Console.WriteLine("Partita inserita.");
            else
                Console.WriteLine("La partita è già presente.");
        }

        static void InserisciPartitaEliminazione(CGestionePartite gestione)
        {
            Console.WriteLine("\nInserimento partita a eliminazione");

            Console.Write("Giornata: ");
            int giornata = LeggiInteroPositivo();

            Console.Write("Squadra di casa: ");
            string nomeCasa = Console.ReadLine();

            Console.Write("Squadra ospite: ");
            string nomeOspite = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nomeCasa) ||
                string.IsNullOrWhiteSpace(nomeOspite))
            {
                Console.WriteLine("Nome non valido.");
                return;
            }

            CSquadra casa = new CSquadra(nomeCasa);
            CSquadra ospite = new CSquadra(nomeOspite);

            Console.Write("Gol casa: ");
            int golCasa = LeggiInteroNonNegativo();

            Console.Write("Gol ospite: ");
            int golOspite = LeggiInteroNonNegativo();

            int? rigoriCasa = null;
            int? rigoriOspite = null;

            if (golCasa == golOspite)
            {
                Console.Write("Rigori casa: ");
                rigoriCasa = LeggiInteroNonNegativo();

                Console.Write("Rigori ospite: ");
                rigoriOspite = LeggiInteroNonNegativo();

                while (rigoriCasa == rigoriOspite)
                {
                    Console.WriteLine(
                        "I rigori devono avere un vincitore.");

                    Console.Write("Rigori casa: ");
                    rigoriCasa = LeggiInteroNonNegativo();

                    Console.Write("Rigori ospite: ");
                    rigoriOspite = LeggiInteroNonNegativo();
                }
            }

            CPartitaEliminazione partita = new CPartitaEliminazione(
                giornata, casa, ospite, golCasa, golOspite,
                rigoriCasa, rigoriOspite);

            if (gestione.AggiungiPartita(partita))
                Console.WriteLine("Partita inserita.");
            else
                Console.WriteLine("La partita è già presente.");
        }

        static int LeggiInteroNonNegativo()
        {
            int valore;

            while (!int.TryParse(Console.ReadLine(), out valore) ||
                   valore < 0)
            {
                Console.Write("Inserisci un intero non negativo: ");
            }

            return valore;
        }

        static int LeggiInteroPositivo()
        {
            int valore;

            while (!int.TryParse(Console.ReadLine(), out valore) ||
                   valore <= 0)
            {
                Console.Write("Inserisci un intero positivo: ");
            }

            return valore;
        }
    }
}
