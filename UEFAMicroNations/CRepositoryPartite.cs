using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UEFAMicroNations
{
    public class CRepositoryPartite : IRepository<CPartita>
    {
        private List<CPartita> elenco;
        private string nomeFile;
        private JsonSerializerOptions opzioni; 
        public CRepositoryPartite()
        {
            elenco = new List<CPartita>();
            nomeFile = "partite.json";

            opzioni = new JsonSerializerOptions();        //dice a jsonSerialize come leggere e scrivere
            opzioni.PropertyNameCaseInsensitive = true;//dice quando leggi json non fare caso a lettere Maiusc o min
        } //così tipo e Tipo sono la stessa cosa altrimenti mi dava problemi

        public IReadOnlyList<CPartita> GetElenco()
        {
            return elenco.AsReadOnly();
        }

        public bool Aggiungi(CPartita elemento)
        {
            if (elemento == null)
                return false;

            foreach (CPartita partita in elenco)
            {
                if (partita.Equals(elemento))
                    return false;
            }

            elenco.Add(elemento);
            return true;
        }

        public bool Rimuovi(CPartita elemento)
        {
            if (elemento == null)
                return false;

            for (int i = 0; i < elenco.Count; i++)
            {
                if (elenco[i].Equals(elemento))
                {
                    elenco.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void Carica()
        {
            elenco.Clear();

            if (!File.Exists(nomeFile))
                return;

            string contenuto = File.ReadAllText(nomeFile);

            if (string.IsNullOrWhiteSpace(contenuto))
                return;

            List<CPartitaDati>? dati =JsonSerializer.Deserialize<List<CPartitaDati>>(contenuto, opzioni);

            if (dati == null)
                return;

            foreach (CPartitaDati dato in dati)
            {
                CPartita? partita = null;
                DateTime? data = null;

                if (!string.IsNullOrWhiteSpace(dato.Data))
                {
                    DateTime dataLetta;

                    if (DateTime.TryParse(dato.Data, out dataLetta))
                        data = dataLetta;
                }

                try
                {
                    if (dato.Tipo == "girone")
                    {
                        partita = new CPartitaGirone(dato.Giornata,dato.SquadraCasa, dato.SquadraOspite,dato.GolCasa, dato.GolOspite, data);
                    }
                    else if (dato.Tipo == "eliminazione")
                    {
                        partita = new CPartitaEliminazione(dato.Giornata,dato.SquadraCasa, dato.SquadraOspite,dato.GolCasa, dato.GolOspite,dato.RigoriCasa, dato.RigoriOspite, data);
                    }
                    else if (dato.Tipo == "finale")
                    {
                        partita = new CPartitaFinale(dato.Giornata,dato.SquadraCasa, dato.SquadraOspite,dato.GolCasa, dato.GolOspite,dato.RigoriCasa, dato.RigoriOspite, data);
                    }
                }
                catch (Exception)
                {
                    partita = null;
                }

                if (partita != null)
                    Aggiungi(partita);
            }
        }

        public void Salva()
        {
            List<CPartitaDati> dati = new List<CPartitaDati>();

            foreach (CPartita partita in elenco)
            {
                CPartitaDati dato = new CPartitaDati();

                if (partita is CPartitaFinale)
                    dato.Tipo = "finale";
                else if (partita is CPartitaEliminazione)
                    dato.Tipo = "eliminazione";
                else
                    dato.Tipo = "girone";

                dato.Giornata = partita.Giornata;
                dato.SquadraCasa = partita.SquadraCasa;
                dato.SquadraOspite = partita.SquadraOspite;
                dato.GolCasa = partita.GolCasa;
                dato.GolOspite = partita.GolOspite;

                if (partita.Data != null)
                    dato.Data = partita.Data.Value.ToString("dd/MM/yyyy");

                if (partita is CPartitaEliminazione eliminazione)
                {
                    dato.RigoriCasa = eliminazione.RigoriCasa;
                    dato.RigoriOspite = eliminazione.RigoriOspite;
                }

                dati.Add(dato);
            }

            string contenuto = JsonSerializer.Serialize(dati, opzioni);

            File.WriteAllText(nomeFile, contenuto);
        }
    }
}
