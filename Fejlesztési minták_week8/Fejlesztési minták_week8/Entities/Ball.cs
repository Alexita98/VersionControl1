using Fejlesztési_minták_week8.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fejlesztési_minták_week8.Entities
{
    /* System.Windows.Forms-t kell behivatkozni, nem a System.Reflection.Emit-et!*/
    //publicot tedd elé!
    public class Ball : Toy //származtasd a Label osztályból
    {

        public SolidBrush BallColor { get; private set; }
        public Ball(Color color)
        {
            BallColor = new SolidBrush(color);
        }

        //Hozz létre egy új függvényt DrawImage néven és Graphics típusú bemeneti paraméterrel
        //A private előtag azt jelenti, hogy az adott elemet (függvényt, tulajdonságot vagy eseményt) csak az osztályon belül lehet elérni. A public az osztályon kívülről is elérhetővé teszi az elemeket. Az új protected kulcsszó pedig egy köztes lehetőség, mely bár kívülről nem teszi hozzáférhetővé a függvényt, de a Ball osztály bármelyik leszármazottja is használhatja majd. 
        protected override void DrawImage(Graphics graphics)
        {
            //A DrawImage metódusban rajzolj ki egy a vezérlőbe illeszkedő, kitöltött kék kört a Graphics osztály segítségével.
            graphics.FillEllipse(BallColor, 0, 0, Width, Height);
        }



    }
}
