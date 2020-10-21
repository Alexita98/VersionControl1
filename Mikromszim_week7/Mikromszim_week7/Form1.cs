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

        //1x) Hozz létre két listát a férfi és a női lélekszámok tárolására, és a szimuláció során töltsd fel őket a megfelelő értékekkel.
        List<int> FemaleNum = new List<int>();
        List<int> MaleNum = new List<int>();

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

        //8) szimulációs lépés függvény elkészítés
        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.Probability).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.Probability).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        private void Simulation()
        {
            //7) Szimuláció vázának felépítése
            // Végigmegyünk a vizsgált éveken
            for (int year = 2005; year <= 2024; year++)
            {
                // Végigmegyünk az összes személyen
                for (int i = 0; i < Population.Count; i++)
                {
                    //9) Szimulációs lépés függvény meghívás
                    // Hozz létre egy visszatérési érték nélküli függvényt SimStep néven, és hívd meg a szimuláció belső ciklusából. 
                    // A függvénynek át kell adnod paraméterként az aktuális évet és az éppen kiválasztott személy entitást.
                    
                    SimStep(year, Population[i]);

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

                FemaleNum.Add(numOfFemales);
                MaleNum.Add(numOfMales);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            FemaleNum.Clear();
            MaleNum.Clear();
            //9) A szimuláció futtatás ne a konstruktorból induljon. Szervezd ki a kódot egy Simulation függvénybe, amit hívj meg a Start gomb eseménykezelőjéből.
            Simulation();
            //1x) Hozz létre egy DisplayResults függvényt, amit hívj meg a szimuláció futása után. 
            DisplayResults();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text = Convert.ToString("C:/Temp/nép.csv");
            }
        }

        private void DisplayResults()
        {
            int i = 0;
            for (int year = 2005; year < 2024; year++)
            {
                string text="Szimulációs év: " + Convert.ToString(year) + "\n" + "\t" + "Fiúk: " + MaleNum[i] + "\n" + "\t" + "Lányok: " + FemaleNum[i];

                richTextBox1.Text = text;

                i++;
            }
        }
    }
}
