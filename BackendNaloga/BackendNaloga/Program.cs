using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendNaloga
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var izbira = "";
            string imeDatoteke = "";
            while (true)
            {
                Console.WriteLine("GLAVNI MENI");
                Console.WriteLine("Vnesite ustrezno črko in pritisnite Enter.");
                Console.WriteLine("Vnesite i za Iskanje poljubne datoteke");
                Console.WriteLine("Vnesite a za Prikaz datoteke: README");
                Console.WriteLine("Vnesite s za Prikaz datoteke: x11-common");
                Console.WriteLine("Vnesite d za Prikaz datoteke: Xmessage");
                Console.WriteLine("Vnesite x za Izhod");

                izbira = Console.ReadLine().ToLower();
                Console.Clear();
                bool izhod = false;
                switch (izbira)
                {
                    case "i":
                        Console.WriteLine("Vnesi ime datoteke:");
                        imeDatoteke = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine("Poti do datotek:");
                        foreach (var test in PoisciDatoteke(imeDatoteke))
                        {
                            Console.WriteLine(test);
                        }
                        Console.ReadKey();
                        Console.Clear();
                        break;


                    case "a":
                        imeDatoteke = "README";
                        Console.WriteLine("Poti do datotek:");
                        foreach (var test in PoisciDatoteke(imeDatoteke))
                        {
                            Console.WriteLine(test);
                        }
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "s":
                        imeDatoteke = "x11-common";
                        Console.WriteLine("Poti do datotek:");
                        foreach (var test in PoisciDatoteke(imeDatoteke))
                        {
                            Console.WriteLine(test);
                        }
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "d":
                        imeDatoteke = "Xmessage";
                        Console.WriteLine("Poti do datotek:");
                        foreach (var test in PoisciDatoteke(imeDatoteke))
                        {
                            Console.WriteLine(test);
                        }
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:

                        izhod = true;
                        break;

                }

                if (izhod)
                {
                    break;
                }

            }
        }

        private static string[] PoisciDatoteke(string imeDatoteke)
        {
            //Pot do json datoteke
            string fileName = "tree_structure.json";

            string jsonString = File.ReadAllText(fileName);
            //Deserializacija Json v objekt Content
            var rootLists = JsonConvert.DeserializeObject<List<Content>>(jsonString);

            var poti = new List<string>();
            var rez = new string[0];
            //Rekurzivna metoda, ki omogoča iskanje po neomejenih nivojih
            rez = Rekurzija(rootLists, poti, rez, imeDatoteke);
            if (rez.Length == 0)
            {
                Array.Resize(ref rez, rez.Length + 1);
                rez[rez.Length - 1] = "Datoteka z tem imenom ne obstaja";
                return rez;
            }
            else
            {
                return rez;
            }
        }

        static string[] Rekurzija(List<Content> contentLists, List<string> poti, string[] rez, string imeDatoteke)
        {

            foreach (var content in contentLists)
            {
                if (content.contents != null && content.type == "directory")
                {
                    if (content.name.Contains("/"))
                    {
                        poti.Add(content.name);
                    }
                    else
                    {
                        poti.Add("/" + content.name);
                    }

                    rez = Rekurzija(content.contents, poti, rez, imeDatoteke);
                    poti.RemoveAt(poti.Count - 1);

                }
                else if (content.type == "file" && content.name == imeDatoteke)
                {
                    poti.Add("/" + content.name);
                    string test = "";
                    foreach (var potke in poti)
                    {
                        test += potke;
                    }
                    Array.Resize(ref rez, rez.Length + 1);
                    rez[rez.Length - 1] = test;
                    poti.RemoveAt(poti.Count - 1);
                }
            }
            return rez;
        }
    }
    public class Content
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<Content> contents { get; set; }
        public string target { get; set; }
    }
}
