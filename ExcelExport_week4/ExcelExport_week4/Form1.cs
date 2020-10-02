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

        //Az Excel objektum könyvtár már része a projektnek, de még az aktuális fájlba is
        //be kell hivatkozni a using kulcsszó segítségével. Felülre bemásolva:
        //using Excel=Microsoft.Office.Interop.Excel;
        //using Sytem.Reflection;

        //Hozd létre a következő üres változókat a Form1 osztály szintjén
        Excel.Application xlApp; // A Microsoft Excel alkalmazás
        Excel.Workbook xlWB; // A létrehozott munkafüzet
        Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül

        public Form1()
        {
            InitializeComponent();

            //LoadData függvény meghívása
            LoadData();

            try
            {
                // Excel elindítása és az applikáció objektum betöltése
                xlApp = new Excel.Application();

                // Új munkafüzet
                xlWB = xlApp.Workbooks.Add(Missing.Value);

                // Új munkalap
                xlSheet = xlWB.ActiveSheet;

                // Tábla létrehozása
                //CreateTable();

                // Control átadása a felhasználónak
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                // Hiba esetén az Excel applikáció bezárása automatikusan
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }

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
