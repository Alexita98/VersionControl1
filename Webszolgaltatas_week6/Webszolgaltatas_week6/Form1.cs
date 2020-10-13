using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using Webszolgaltatas_week6.Entities;
using Webszolgaltatas_week6.MnbServiceReference;

namespace Webszolgaltatas_week6
{
    public partial class Form1 : Form
    {
        //7) A Form1 osztály szintjén hozz létre egy RateData típusokat tartalmazó 
        //   BindingList -et Rates néven
        BindingList<RateData> Rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();

            //23) Az eddig a konstruktorba írt kódot szervezd ki egy RefreshData nevű
            //    függvénybe.
            RefreshData();

            //0) Webszolgáltatás hívás függvény meghívása
            string result = webServiceCalling();

            //5) Entities mappa létrehozása a projektben
            //6) A mappában hozz létre egy RateData nevű osztályt Rate, Currency, Value tulajdonságokkal

            //9) xml feldolgozás függvény meghívása
            xmlProcessing(result);

            //15) Adatvizualizációs függvény meghívása
            dataVisualization();

            //22) Adj két DateTimePicker-t és egy üres ComboBox-ot a Form1-hez

            //24) Rendelj eseménykezelőt a DateTimePicker-ek és a ComboBox alapértelmezett
            //    eseményéhez (dupla klikk) is, és mindegyikből hívd meg a RefreshData-t.

        }

        private string webServiceCalling()
        {
            //1) Példányosítás (ehhez névtér behivatkozás)
            var mnbService = new MNBArfolyamServiceSoapClient();

            //25) A korábbi request létrehozást módosítsd úgy, hogy a dátumok a 
            //    DateTimePicker-ek Value-jából, a valuta neve pedig a ComboBox 
            //    SelectedItem -jéből érkezzenek. (Ne felejtsd el őket string-é konvertálni!)
            //    A ComboBox Items listája a design nézetből kézzel is feltölthető néhány értékkel. Teszteléshez írd bele az “EUR” értéket (idézőjelek nélkül).
            //2) Példány létrehozás, paraméterek kitöltése
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            //3) Hívd meg az mnbService GetExchangeRates nevű függvényét a request bemeneti
            //   paraméterrel, és a függvény visszatérési értékét tárold egy response nevű 
            //   változóba.
            var response = mnbService.GetExchangeRates(request);


            //4) A válaszból kérdezd le a GetExchangeRatesResult tulajdonság értékét egy 
            //   result változóba (ami amúgy string típusú)
            var result = response.GetExchangeRatesResult;

            return result;
        }

        private void xmlProcessing(string result)
        {
            //10) Példányosíts egy XmlDocument osztályt xml néven
            var xml = new XmlDocument();

            //11) Hívd meg a példányosított XmlDocument LoadXml metódusát, és add át 
            //    neki a korábban lekérdezett string formátumú XML-t amit 
            //    szolgáltatásból kaptál vissza válaszként.
            xml.LoadXml(result);

            //12) Végigmegünk a dokumentum fő elemének gyermekein. 
            foreach (XmlElement element in xml.DocumentElement)
            {
                //13) A foreach-en belül hozz létre egy példányt a RateData osztályból, és add hozzá a Rates listához.
                var rate = new RateData();
                Rates.Add(rate);

                //14) A foreach-en belül töltsd fel a RateData tulajdonságait az aktuális XML elemnek megfelelően.
                    //Date
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                    //Valuta
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                    //Érték
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0) rate.Value = value / unit;

            }

        }
        private void dataVisualization()
        {
            //16) Adj egy Chart-ot a Form1-hez design nézetben. A neve legyen chartRateData.
            // 17) A Chart adatforrása legyen a Rates lista
            chartRateData.DataSource = Rates;

            //18) A Series tulajdonsága egy adatsorokból álló tömb, ami alapértelmezetten
            //    egy elemű. A tömb első elemét érdemes lekérdezni egy változóba, 
            //    hogy könnyebb legyen átírni a tulajdonságait.
            var series = chartRateData.Series[0];

            //19) Az adatsor típusa legyen SeriesChartType.Line
            series.ChartType = SeriesChartType.Line;

            //20) Az adatsornak meg kell mondani, hogy az egyes tengelyein az 
            //    adatforrás mely elemei látszódjanak. Az XValueMember értéke legyen 
            //    a “Date”, az YValueMember értéke pedig legyen “Value”.
            series.XValueMember = "Date";
            series.YValueMembers = "Value";

            //21) Diagram formázás

                //Az adatsor vastagsága legyen kétszeres
                series.BorderWidth = 2;
                //Ne látszódjon oldalt a címke (legend)
                var legend = chartRateData.Legends[0];
                legend.Enabled = false;
                //Ne látszódjanak a fő grid vonalak se az X, se az Y tengelyen
                var ChartArea = chartRateData.ChartAreas[0];
                ChartArea.AxisX.MajorGrid.Enabled = false;
                ChartArea.AxisY.MajorGrid.Enabled = false;
                //Az Y tengely ne nullától induljon (ez egy bool tulajdonság)
                ChartArea.AxisY.IsStartedFromZero = false;

        }

        private void RefreshData()
        {
            //24) ürítsd le a Rates lista tartalmát a Clear függvénnyel
            Rates.Clear();

            //8) Hozz létre egy DataGridView-t a Form1-en, és állítsd be, hogy a Rates legyen az adatforrása
            dataGridView1.DataSource = Rates;
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
