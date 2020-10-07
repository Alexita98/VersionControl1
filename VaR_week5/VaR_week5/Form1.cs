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
    }
}
