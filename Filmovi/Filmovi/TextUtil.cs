using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi
{
    class TextUtil
    {
        public static DataModel LoadData()
        {
            DataModel dataModel = new DataModel();
            string[] lines = File.ReadAllLines(@"./data/movies_tags_uredjeno.txt");

            string[] linkovi = File.ReadAllLines(@"./data/linkovi.txt");

            foreach(String str in linkovi)
            {
                int index = str.LastIndexOf(',');
                NaiveBayes.name_link.Add(str.Substring(0,index),str.Substring(index+1));
            }

            //TODO 1 - implementirati metodu koja ucitava podatke iz tsv fajla i smesta ih u odgovarajuce atribute data modela

            //Console.WriteLine(lines[1]);
            /*
            string[] gledani = File.ReadAllLines(@"./data/odgledano.txt");

            foreach (String film in gledani)
            {
                DataModel.odgledani.Add(film);
            }

            */
            foreach (String str in lines)
            {
                string[] delovi = str.Split(',');

                //string genres = delovi[delovi.Length-1];
                //if(genres.Contains("no genres"))
                //{
                //    genres = "";
                //    continue;
                //}


                int id = Convert.ToInt32(delovi[0]);
                string names = delovi[1];

                if (names.Contains('"'))
                {
                    names = str.Substring(str.IndexOf(',') + 1, str.LastIndexOf('"') - 1 - str.IndexOf(','));
                    names = names.Substring(1, names.Length - 1);
                }
                //Console.WriteLine(names);

                string year = names.Substring(names.LastIndexOf("(") + 1, 4);

                names = names.Trim();

                string tags = str.Substring(str.LastIndexOf(",") + 1);

                List<string> lista_tagova = new List<string>();

                string[] tag = tags.Split('|');

                foreach (String t in tag)
                {
                    lista_tagova.Add(t);
                }

                //Console.WriteLine(id+" "+names+" "+year+" "+genres);
                int yearID = Convert.ToInt32(year);

                dataModel.id.Add(id);
                dataModel.name.Add(names);
                dataModel.year.Add(yearID);
                dataModel.tags.Add(tags);



                /*
                Console.WriteLine(names);
                string text ="\n"+id + "," + names + ",";
                foreach(String t in lista_tagova)
                {

                    text += t+"|";
                }
                string zaispis = text.Substring(0, text.Length - 1);
                System.IO.File.AppendAllText(@"D:\Projekat ORI\NaiveBayes\NaiveBayes\bin\Debug\data\WriteText.txt", zaispis);
                */


            }

            return dataModel;
        }

        /// <summary>
        /// Metoda za uklanjanje znakova interpunkcije iz teksta.
        /// Npr. "Milan,Darko i Ivan polazu kolokvijum!" ce biti transformisan u
        /// "Milan Darko i Ivan polazu kolokvijum"
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Tekst bez znakova interpunkcije</returns>
        public static string RemovePunctuation(string text)
        {
            //TODO 2 - Ukloniti znakove interpunkcije iz teksta
            string retVal = null;

            return retVal;
        }

        /// <summary>
        /// Tokenizacija teksta na reci. Pre razdvajanja teksta na reci (tokene),
        /// potrebno je ukloniti sve znake interpunkcije i pretvoriti 
        /// sva slova u mala. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Niz tokena (reci)</returns>
        public static string[] Tokenize(string text)
        {
            //TODO 3 - implementirati tokenizaciju teksta na reci 
            //text = RemovePunctuation(text);
            // text = text.ToLower();
            string[] tokens = null;

            tokens = text.Split('|');

            return tokens;
        }
        /// <summary>
        /// Metoda za brojanje reci u teksu. Formira se recnik ciji je kljuc
        /// sama rec, a vrednost broj pojavljivanja te reci.
        /// Npr za niz reci: ["rec1", "rec2", "rec1"] bice formiran recnik
        ///     { "rec1" : 2,
        ///       "rec2" : 1   
        ///     } 
        /// </summary>
        /// <param name="words">Niz reci</param>
        /// <returns></returns>
        public static Dictionary<string, int> CountWords(List<string> words)
        {
            //TODO 4 - Formirati recnik koji sadrzi broj pojavljivanja svake reci iz niza reci words
            Dictionary<string, int> vocabulary = new Dictionary<string, int>();

            return vocabulary;

        }
    }
}
