using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            //1) Példányosítás (ehhez névtér behivatkozás)
            var mnbService = new MNBArfolyamServiceSoapClient();

            //2) Példány létrehozás, paraméterek kitöltése
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020 - 01 - 01",
                endDate = "2020-06-30"
            };

            //3) Hívd meg az mnbService GetExchangeRates nevű függvényét a request bemeneti
            //   paraméterrel, és a függvény visszatérési értékét tárold egy response nevű 
            //   változóba.
            var response = mnbService.GetExchangeRates(request);

            //4) A válaszból kérdezd le a GetExchangeRatesResult tulajdonság értékét egy 
            //   result változóba
            var result = response.GetExchangeRatesResult;

            //5) Entities mappa létrehozása a projektben
            //6) A mappában hozz létre egy RateData nevű osztályt Rate, Currency, Value tulajdonságokkal

            //8) Hozz létre egy DataGridView-t a Form1-en, és állítsd be, hogy a Rates legyen az adatforrása
            dataGridView1.DataSource = Rates;

            xmlProcessing();

        }

        private void xmlProcessing()
        {
            //9) Példányosíts egy XmlDocument osztályt xml néven
            var xml = new XmlDocument();

            //10) Hívd meg a példányosított XmlDocument LoadXml metódusát, és add át 
            //    neki a korábban lekérdezett string formátumú XML-t amit 
            //    szolgáltatásból kaptál vissza válaszként.
            xml.LoadXml(result);

            //11) Végigmegünk a dokumentum fő elemének gyermekein. 
            foreach (XmlElement element in xml.DocumentElement)
            {
                //12) A foreach-en belül hozz létre egy példányt a RateData osztályból, és add hozzá a Rates listához.
                var rate = new RateData();
                Rates.Add(rate);

                //13) A foreach-en belül töltsd fel a RateData tulajdonságait az aktuális XML elemnek megfelelően.
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
    }
}
