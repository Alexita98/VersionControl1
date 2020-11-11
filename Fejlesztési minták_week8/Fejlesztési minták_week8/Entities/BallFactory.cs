using Fejlesztési_minták_week8.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fejlesztési_minták_week8.Entities
{
    //publicot tedd elé
    public class BallFactory : IToyFactory
    {
        public Color BallColor { get; set; }

        //Az osztálynak legyen egy függvénye CreateNew néven Ball visszatérési értékkel.
        public Toy CreateNew()
        {
            return new Ball(BallColor); //A függvényben hozz létre egy Ball példányt és add vissza az értékét.
        }
    }
}
