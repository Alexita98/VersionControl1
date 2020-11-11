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
    class Ball: Label //származtasd a Label osztályból
    {
        public Ball() //ctor tab tab - konstruktor 
        {
            AutoSize = false;
            Width = 50; //labda méretei
            Height = 50;

            //rendelj eseménykezelőt a Paint eseményéhez (Paint += tab tab
            Paint += Ball_Paint;
        }

        private void Ball_Paint(object sender, PaintEventArgs e)
        {
            //A PaintEventArgs argumentumokból kérjük le az adott osztályunkhoz 
            //létrehozott grafika példányt (e.Graphics). Bármi, amit erre a “vászonra” 
            //rajzolunk, automatikusan meg fog jelenni a vezérlőnk felületén, amikor az 
            //frissül.
            DrawImage(e.Graphics);
        }

        //Hozz létre egy új függvényt DrawImage néven és Graphics típusú bemeneti paraméterrel
        //A private előtag azt jelenti, hogy az adott elemet (függvényt, tulajdonságot vagy eseményt) csak az osztályon belül lehet elérni. A public az osztályon kívülről is elérhetővé teszi az elemeket. Az új protected kulcsszó pedig egy köztes lehetőség, mely bár kívülről nem teszi hozzáférhetővé a függvényt, de a Ball osztály bármelyik leszármazottja is használhatja majd. 
        protected void DrawImage(Graphics graphics)
        {
            //A DrawImage metódusban rajzolj ki egy a vezérlőbe illeszkedő, kitöltött kék kört a Graphics osztály segítségével.
            graphics.FillEllipse(new SolidBrush(Color.Blue), 0, 0, Width, Height);
        }

        //Hozz létre egy publikus függvényt MoveBall néven. A függvényben növeld meg eggyel az aktuális Ball Left tulajdonságát.
        public void MoveBall()
        {
            Left += 1;
        }
    }
}
