using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1() //konstruktor
        {
            InitializeComponent();

            //3) a konstruktorban másold az adattábát a memóriába
            Ticks = context.Ticks.ToList();

            //4) töltsd fel a lista elemeivel a DataGridView-t
            dataGridView1.DataSource = Ticks;

            //12) hívd meg a CreatePortfolio() függvényt
            CreatePortfolio();



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
        //where záradékban elsőként kiszűrjük azokat a Tick-eket, ahol az Index megegyezik a keresett index-el
        //A Trim()-re azért van szükség, mert az adatbázisban az Index mező nchar(15) típusú. A fel nem használt karaktereket betűkkel tölti fel. Ezek levágására szolgál a Trim() függvény.
        //A tőzsdén nem minden nap van kereskedés. Ha a kérdéses napon az adott indexhez nem tartozik rekord az adatbázisban, a kérdéses naphoz képest visszafelé számolva a legközelebbi kereskedési nappal számolunk: date <= x.TradingDay.


    }
}
