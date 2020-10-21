using Mikromszim_week7.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mikromszim_week7
{
    public partial class Form1 : Form
    {
        //3) Hozz létre a Form1-ben egy-egy megfelelő típusú elemekből álló listát a beolvasandó adatok tárolására
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        //7) Hozz létre egy véletlenszám generátort az osztály szintjén, és adj neki egy tetszőleges induló Seed-et
        Random rng = new Random(1234);

        public Form1()
        {
            InitializeComponent();
            
            //5) Hívd meg a betöltő függvényeket a Form1 konstruktorából, és az eredményüket töltsd be a megfelelő listába.
            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\nép.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\nép.csv");

            //6) A betöltés tesztelésének legegyszerűbb módja egy ideiglenes DataGridView a Form1-en. Ennek az adatforrásába betöltve a felolvasott listák egyikét, megjelenik a lista tartalma.
            //dataGridView1.DataSource = BirthProbabilities;
            //NE IJEDJ MEG, CSAK SOKAT KELL VÁRNI, de működik

            //7) Szimuláció vázának felépítése
            // Végigmegyünk a vizsgált éveken
            for (int year = 2005; year <= 2024; year++)
            {
                // Végigmegyünk az összes személyen
                for (int i = 0; i < Population.Count; i++)
                {
                    // Ide jön a szimulációs lépés
                }

                int numOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int numOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, numOfMales, numOfFemales));
                //Output ablakban a jobb alsó sarokban találod meg, csak feljebb kell görgetni
            }

        }

        //4) Hozz létre a Form1-ben egy-egy metódust a három .csv állomány felolvasásához!
        //A három .csv fájl beolvasása külön-külön metódussal történjen. A metódusok 
        //bemenő paramétere a kérdéses fájl elérési útvonala, míg a visszatérési értéke 
        //egy a megfelelő osztályból (pl. Person) képzett lista.
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            //megnyitja a paraméterként kapott .csv kiterjesztésű szövegfájlt olvasásra;
            using (StreamReader sr=new StreamReader(csvpath,Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    //a fájl valamennyi sorából egy megfelelő (pl.: Person) objektum képezhető, ezért iteratívan végigolvassa a fájlt, és a beolvasott sort felbontja elemekre (például a Split metódus segítségével), azaz például az év, nem és gyermekszám adatokra;
                    var elem = sr.ReadLine().Split(';');

                    //minden iterációs lépésben a megfelelő listához hozzácsatolja (Add) az új objektumot;
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(elem[0]),

                        // !!!!!!! Ez az alsó sor új, tanuld meg!!!!!!!!!!!!!
                        Gender = (Gender)Enum.Parse(typeof(Gender), elem[1]),
                        NbrOfChildren = int.Parse(elem[2])
                    });
                }
            }
            //a ciklus zárása után a metódus visszatér a listával (return).
            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();

            using (StreamReader sr=new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var elem = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability()
                    {
                        Age=int.Parse(elem[0]),
                        NumOfChildren=int.Parse(elem[1]),
                        Probability=double.Parse(elem[2])
                    });
                }
            }
            return birthProbabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();
            using (StreamReader sr=new StreamReader(csvpath,Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var elem = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender),elem[0]),
                        Age = int.Parse(elem[1]),
                        Probability = double.Parse(elem[2])
                    });
                }
            }

            return deathProbabilities;
        }
    }
}
