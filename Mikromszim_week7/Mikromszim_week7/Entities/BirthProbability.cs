using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikromszim_week7.Entities
{
    public class BirthProbability //(Ne felejtsd el a ´public´ előtagot!)
    {
        public int Age { get; set; }
        public int NumOfChildren { get; set; }
        public double Probability { get; set; }
    }
}
