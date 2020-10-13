using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Webszolgaltatas_week6.MnbServiceReference;

namespace Webszolgaltatas_week6
{
    public partial class Form1 : Form
    {
        
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
            //paraméterrel, és a függvény visszatérési értékét tárold egy response nevű 
            //változóba.
            var response = mnbService.GetExchangeRates(request);

            //4) A válaszból kérdezd le a GetExchangeRatesResult tulajdonság értékét egy 
            //result változóba
            var result = response.GetExchangeRatesResult;

            //5) Entities mappa létrehozása a projektben
            //6) A mappában hozz létre egy RateData nevű osztályt Rate, Currency, Value tulajdonságokkal


        }
    }
}
