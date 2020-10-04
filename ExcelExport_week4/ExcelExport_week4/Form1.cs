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
        private void CreateTable()
        {
            //A CreateTable függvényen belül, hozz létre egy tömböt, mely tartalmazza a 
            //tábla fejléceit + egy extra oszlop fejlécét.
            string[] headers = new string[]
            {
             "Kód",
             "Eladó",
             "Oldal",
             "Kerület",
             "Lift",
             "Szobák száma",
             "Alapterület (m2)",
             "Ár (mFt)",
             "Négyzetméter ár (Ft/m2)"
            };

            //Ezután egy for ciklus segítségével írd ki a tömb elemeit a munkalap első sorába.
            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[0];
            }

            //Hozz létre egy object típusú két dimenziós tömböt az adatok tárolására.
            object[,] values = new object[Flats.Count, headers.Length];

            //Egy foreach ciklussal menj végig a Flats lista sorain, és tölts fel a tömböt a megfelelő adatokkal.
            int k = 0;
            foreach (Flat flat in Flats)
            {
                values[k, 0] = flat.Code;
                values[k, 1] = flat.Vendor;
                values[k, 2] = flat.Side;
                values[k, 3] = flat.District;

                if (flat.Elevator == true)
                { values[k, 4] = "Van"; }
                else { values[k, 4] = "Nincs"; }

                values[k, 5] = flat.NumberOfRooms;
                values[k, 6] = flat.FloorArea;
                values[k, 7] = flat.Price;

                //Az utolsó oszlopba egy Excel képlet kerüljön string formában.
                //Az Excel képletek szövegesek és mindig “=” jellel kezdődnek.
                values[k, 8] = "=" + GetCell(k + 2, 7) + "*" + GetCell(k + 2, 8);
                //Excelben: =G2*H2 majd G3*H3 stb...
                
                k++;
            }
            xlSheet.get_Range(
             GetCell(2, 1),
             GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;

            //Fejléc formázás
            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            //Tábla mérete
            int lastRowID = xlSheet.UsedRange.Rows.Count;
            int lastColumnID = xlSheet.UsedRange.Columns.Count; //9

            //Tábla formázás
            Excel.Range table = xlSheet.get_Range(GetCell(1, 1), GetCell(lastRowID, lastColumnID));
            Excel.Range firstCol = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID, 1));
            Excel.Range lastCol = xlSheet.get_Range(GetCell(2, lastColumnID), GetCell(lastRowID, lastColumnID));

            //Az egész táblának is legyen olyan körbe szegélye, mint a fejlécnek
            table.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            //Az első oszlop adatai legyenek félkövérek és a háttér legyen halvány sárga
            firstCol.Font.Bold = true;
            firstCol.Interior.Color = Color.LightYellow;

            //Az utolsó oszlop adatainak háttere legyen halványzöld.
            lastCol.Interior.Color = Color.LightGreen;

            //Az utolsó oszlop adatai két tizedesre kerekített formában jelenjenek meg. (Google)
            lastCol.NumberFormat = "0.00";

        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}
