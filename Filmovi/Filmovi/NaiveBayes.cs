using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi
{
    class NaiveBayes
    {
        public static int BROJ;
        public static Dictionary<string, int> vocabulary = new Dictionary<string, int>();
        public static Dictionary<string, Dictionary<int, List<string>>> name_year_genres = new Dictionary<string, Dictionary<int, List<string>>>();
        public static Dictionary<string, List<string>> name_genres = new Dictionary<string, List<string>>();
        public static Dictionary<string, string> name_link = new Dictionary<string, string>();

        //public static Dictionary<int, Dictionary<string, int>> word_counts = new Dictionary<int, Dictionary<string, int>>();

        public NaiveBayes()
        {
            //word_counts[0] = new Dictionary<string, int>();
            //word_counts[1] = new Dictionary<string, int>();
        }
        /// <summary>
        /// Formiranje globalnog recnika, recnika iz pozitivnih i negativnih recenzija 
        /// </summary>
        /// <param name="model">train model ucitan iz tsv datoteke</param>
        public List<String> fit(DataModel model)
        {
            List<String> povrat = new List<String>();
            for (int i = 0; i < model.name.Count; i++)
            {
                int id = model.id[i];
                string name = model.name[i];
                int year = model.year[i];
                //Console.WriteLine(name);
                //string genres = model.genres[i];
                string tagovi = model.tags[i];


                string[] tags = TextUtil.Tokenize(tagovi);
                Dictionary<int, List<string>> temp = new Dictionary<int, List<string>>();
                List<string> zajedno = tags.ToList<string>();
                //zajedno.Add(year.ToString());
                string temp1 = name;
                //Console.WriteLine(temp1);

                while (temp1.Contains('('))
                {
                    int loc = temp1.IndexOf('(');
                    temp1 = temp1.Remove(loc, 2);
                }
                while (temp1.Contains(')'))
                {
                    int loc = temp1.IndexOf(')');
                    temp1 = temp1.Remove(loc);
                }
                while (temp1.Contains(','))
                {
                    int loc = temp1.IndexOf(',');
                    temp1 = temp1.Remove(loc, 2);
                }
                while (temp1.Contains(" The "))
                {
                    int loc = temp1.IndexOf(" The ");
                    temp1 = temp1.Remove(loc, 4);
                }

                string[] deoImena = temp1.Split(' ');
                foreach (string deo in deoImena)
                {
                    if (!deo.Equals(' '))
                    {
                        zajedno.Add(deo.ToLower());
                        //zajedno.Add(deo.ToLower());
                    }

                }
                zajedno.Add(name.Substring(0, name.Length - 7).ToLower());

                temp.Add(year, zajedno);

                name_year_genres.Add(name, temp);
                name_genres.Add(name, zajedno);
                povrat.Add(name);
                //Console.WriteLine(name);
                //Console.WriteLine(" --- "+year);
                //foreach (string str in name_year_genres[name][year])
                //  Console.WriteLine(" --- "+str);


                /*
                foreach (KeyValuePair<string, int> item in counts)
                {
                    string word = item.Key;
                    int count = item.Value;

                    //TODO 5 - Popuniti globalni recnik svih reci, kao i recnike za odredjene sentimente

                }
                */
            }
            return povrat;


        }
        /// <summary>
        /// Racunanje verovatnoca za prosledjeni tekst
        /// </summary>
        /// <param name="text">Tekst koji se klasifikuje</param>
        /// 
        Dictionary<string, int> temp = new Dictionary<string, int>();

        public List<String> predict()
        {
            List<String> povrat = new List<String>();
            List<string> zanrovi_odgledanih = new List<string>();
            foreach (String str in DataModel.odgledani)
            {
                //Console.WriteLine(str);
                List<string> zanrovi = name_genres[str];
                foreach (string zanr in zanrovi)
                {
                    zanrovi_odgledanih.Add(zanr);
                }
            }

            Dictionary<string, double> rec_ponavljanje = new Dictionary<string, double>();

            foreach (string rec in zanrovi_odgledanih)
            {
                double i = 0;
                foreach (string zanr in zanrovi_odgledanih)
                {
                    if (rec == zanr) i++;
                }
                if (rec.Equals("the")) continue;
                if (rec.Equals("of")) continue;
                if (rec.Equals("and")) continue;
                if (rec.Equals("dvd")) continue;
                if (!rec_ponavljanje.Keys.Contains(rec))
                {
                    rec_ponavljanje.Add(rec, i * i);
                }
            }

            var sortedRecPonavljanje = from entry in rec_ponavljanje orderby entry.Value ascending select entry;
            //foreach (KeyValuePair<string, double> kvp in sortedRecPonavljanje)
            //{
            //    Console.WriteLine(kvp.Key + " - " + kvp.Value);
            //}
            //Console.WriteLine("------------------------------------------------------------------------------------");

            Dictionary<string, double> film_vrednost = new Dictionary<string, double>();
            Dictionary<string, double> film_vrednost1 = film_vrednost;

            foreach (string film in name_genres.Keys)
            {
                double i = 0;
                foreach (string zanr in name_genres[film])
                {
                    if (rec_ponavljanje.Keys.Contains(zanr))
                    {
                        i += rec_ponavljanje[zanr];
                    }
                }
                film_vrednost.Add(film, i);
            }
            double max = 0;
            int j = 0;

            max = film_vrednost.Values.Max();
            /*
            foreach (KeyValuePair<string, double> fv in film_vrednost)
            {
                foreach (KeyValuePair<string, double> fv1 in film_vrednost)
                {
                    string[] delovi = fv.Key.Split(' ');
                    foreach (string deo in delovi)
                    {
                        string[] delovi1 = fv1.Key.Split(' ');
                        double vrednost = 0;
                        foreach (string deo1 in delovi1)
                        {
                            if (deo.ToLower().Equals(deo1.ToLower()))
                            {
                                vrednost += Convert.ToDouble(deo.Length);

                            }
                        }
                        vrednost = vrednost / Convert.ToDouble(fv.Key.Length - 5);
                        if (vrednost > 0.5)
                        {
                            film_vrednost1[fv1.Key] = vrednost * max + fv1.Value;
                        }
                        
                    }
                }
            }
            */
            var sortedDict = from entry in film_vrednost orderby entry.Value descending select entry;

            Dictionary<int, string> za_upis = new Dictionary<int, string>();
            foreach (KeyValuePair<string, double> f_v in sortedDict)
            {
                if (j == BROJ) break;
                if (DataModel.odgledani.Contains(f_v.Key))
                {
                    continue;
                }
                else j++;
                za_upis.Add(j, f_v.Key);
                //Console.WriteLine(j + ". " + f_v.Key + " - " + f_v.Value);
                povrat.Add(f_v.Key);
            }

            return povrat;

            //string unos = Console.ReadLine();

            //int br = Convert.ToInt32(unos);
            //string fl = za_upis[br];
            //DataModel.odgledani.Add(fl);
            //System.IO.File.AppendAllText(@"D:\Projekat ORI\NaiveBayes\NaiveBayes\bin\Debug\data\odgledano.txt", "\n" + fl);
            //Console.Clear();

            //predict();




            /*
            string[] words = TextUtil.Tokenize(text);
            //var counts = TextUtil.CountWords(words);

            //double documentCount = documents_sentiment_count.Values.Sum();
            //TODO 6 - Izracunati verovatnoce da je dokument za predikciju bas pozitivnog ili negativnog sentimenta - P(cj)
            double Pcj_neg;
            double Pcj_pos;


            double log_prob_neg = 0.0;
            double log_prob_pos = 0.0;

            foreach (KeyValuePair<string, int> item in counts)
            {
                string w = item.Key;
                int cnt = item.Value;
                if (!vocabulary.ContainsKey(w) || w.Length <= 3)
                    continue;
                //TODO 7.1 - Iterativno racunati logaritamski zbir verovatnoca sentimenta svake reci
                
            }

            //TODO 7.2 Izracunati konacnu vrednost verovatnoce sentimenta prosledjenog teksta

            //TODO 8 - Ispisati vrednosti predikcije za pozitivan i negativan sentiment teksta
            */
        }
    }
}
