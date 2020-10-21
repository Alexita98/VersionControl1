using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikromszim_week7.Entities
{
    public class Person //(Ne felejtsd el a ´public´ előtagot!)
    {
        //2) Írd fel a Person osztályt a fenti négy megfelelő típusú tulajdonsággal!
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public int NbrOfChildren { get; set; }
        public bool IsAlive { get; set; }

        public Person()
        {
            IsAlive = true;
        }
    }
}
