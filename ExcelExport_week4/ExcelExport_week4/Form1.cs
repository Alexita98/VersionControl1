using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport_week4
{
    public partial class Form1 : Form
    {
        //A Form1 osztály szintjén hozz létre egy Flat típusú elemekből álló listára 
        //mutató referenciát. (Nem kell inicializálni new-val.)
        List<Flat> Flats;

        //A Form1 osztály szintjén példányosítsd az ORM objektumot!
        RealEstateEntities context = new RealEstateEntities();

        
        public Form1()
        {
            InitializeComponent();

            //LoadData függvény meghívása
            LoadData();

            
        }

        //A létrehozott függvény célja kizárólag a program struktúrálása. 
        //Enélkül az egész kód ömlesztve szerepelne a konstruktorban.
        private void LoadData()
        {
            //A LoadData függvényen belül másold az adattáblát a memóriába!
            Flats = context.Flats.ToList();
        }

        //CreateExcel függvény létrehozás
        
    }
}
