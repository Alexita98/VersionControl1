using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VaR_week5
{
    public partial class Form1 : Form
    {
        //Form1 osztály szintjén
        //1) Tick típusú elemkből álló listára mutató referencia
        List<Tick> Ticks;

        //2) példányosítsd az ORM objektumot
        PortfolioEntities context = new PortfolioEntities();
        public Form1() //konstrukto
        {
            InitializeComponent();

            //3) A konstruktorban másold az adattábát a memóriába
            Ticks = context.Ticks.ToList();

            //4) töltsd fel a lista elemeivel a DataGridView-t
            dataGridView1.DataSource = Ticks;



        }
    }
}
