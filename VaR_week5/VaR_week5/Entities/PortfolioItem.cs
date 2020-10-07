using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaR_week5.Entities
{
    //5) A portfóliónk elemeit nem SQL adatbázisban tároljuk, hanem egy PortfolioItem típusú elemekből álló listában.
    //   Hozd létre egy Entities mappát és abban egy új fájlban a PortfolioItem osztályt!


    //6) Tedd publikussá az osztályt! (public)
    //7) Állítsd be a tulajdonságokat
    public class PortfolioItem
    {
        public string  Index { get; set; }
        public decimal Volume { get; set; }
    }
}
