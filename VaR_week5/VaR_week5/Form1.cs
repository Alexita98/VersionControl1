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
using VaR_week5.Entities;

namespace VaR_week5
{
    public partial class Form1 : Form
    {
        //Form1 osztály szintjén
        //1) Tick típusú elemkből álló listára mutató referencia
        List<Tick> Ticks;

        //2) példányosítsd az ORM objektumot
        PortfolioEntities context = new PortfolioEntities();

        //8) Hozz létre egy PortfolioItem típusú elemekből álló Portfolio nevű listát a Form1 szintjén.
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        List<decimal> Nyereségek = new List<decimal>();

        public Form1() //konstruktor
        {
            InitializeComponent();

            //3) a konstruktorban másold az adattábát a memóriába
            Ticks = context.Ticks.ToList();

            //4) töltsd fel a lista elemeivel a DataGridView-t
            dataGridView1.DataSource = Ticks;

            //12) hívd meg a CreatePortfolio() függvényt
            CreatePortfolio();

            //14) Számítsd ki a VaR értékét
            //List<decimal> Nyereségek = new List<decimal>();
            int intervalum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());

        }

        //9) készítsd el a CreatePortfolio() függvényt
        private void CreatePortfolio()
        {
            //10) vedd fel az alábbi három részvényt a Portfolio listába
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            /* VAGY így is lehet részletesen szemléltetve
            PortfolioItem p = new PortfolioItem();
            p.Index = "OTP";
            p.Volume = 10;
            Portfolio.Add(p); */

            //11) portfóliódat jelenítsd megy DataGridView-ban
            dataGridView2.DataSource = Portfolio;
        }

        //13) GetPortfolioValue() függvényt másold be
        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //14) gy gombra kattintva jöjjön fel egy mentés ablak, ahol a felhasználó megadhatja, hová szeretné elmenteni a nyereség listát
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK) return;

            //15) A fájl első sorában szerepeljen a fejléc, mely az “Időszak” és a “Nyereség” szavakat tartalmazza
            //    A sorokban először a lista aktuális elemszáma, majd a megfelelő elem értéke jelenjen meg
            int k = 0;
            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.Default))
            {
                sw.Write("Időszak ");
                sw.Write("Nyereség");
                sw.WriteLine();
                foreach(var ny in Nyereségek)
                {
                    //sw.Write(Nyereségek[k]);
                    sw.Write(k);
                    sw.Write(" ");
                    sw.Write(ny);
                    sw.WriteLine();
                    k++;
                }
            }

        }
        //where záradékban elsőként kiszűrjük azokat a Tick-eket, ahol az Index megegyezik a keresett index-el
        //A Trim()-re azért van szükség, mert az adatbázisban az Index mező nchar(15) típusú. A fel nem használt karaktereket betűkkel tölti fel. Ezek levágására szolgál a Trim() függvény.
        //A tőzsdén nem minden nap van kereskedés. Ha a kérdéses napon az adott indexhez nem tartozik rekord az adatbázisban, a kérdéses naphoz képest visszafelé számolva a legközelebbi kereskedési nappal számolunk: date <= x.TradingDay.


    }
}
