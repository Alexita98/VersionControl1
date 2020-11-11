using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fejlesztési_minták_week8.Abstractions
{
    public abstract class Toy : Label
    {
        public Toy() //ctor tab tab - konstruktor 
        {
            AutoSize = false;
            Width = 50; //labda méretei
            Height = 50;

            //rendelj eseménykezelőt a Paint eseményéhez (Paint += tab tab
            Paint += Toy_Paint;
        }

        private void Toy_Paint(object sender, PaintEventArgs e)
        {
            //A PaintEventArgs argumentumokból kérjük le az adott osztályunkhoz 
            //létrehozott grafika példányt (e.Graphics). Bármi, amit erre a “vászonra” 
            //rajzolunk, automatikusan meg fog jelenni a vezérlőnk felületén, amikor az 
            //frissül.
            DrawImage(e.Graphics);
        }

        //Hozz létre egy új függvényt DrawImage néven és Graphics típusú bemeneti paraméterrel
        //A private előtag azt jelenti, hogy az adott elemet (függvényt, tulajdonságot vagy eseményt) csak az osztályon belül lehet elérni. A public az osztályon kívülről is elérhetővé teszi az elemeket. Az új protected kulcsszó pedig egy köztes lehetőség, mely bár kívülről nem teszi hozzáférhetővé a függvényt, de a Ball osztály bármelyik leszármazottja is használhatja majd. 

        /*egy absztrakt osztályban az adott függvényt is absztrakttá lehet tenni az abstract előtaggal. Ilyenkor a függvényt tartalmazó kapcsos jeleket el kell hagyni, és egy pontosvesszővel kell lezárni a függvény deklarálás sorát. Az absztrakt osztályból való származtatás után a Visual Studio hibát fog jelezni, ha az új osztályban nem szerepel ez a függvény. Ezzel a sorral tehát lényegében ki lehet kényszeríteni, hogy egy adott típusú függvény (tetszőleges tartalommal) meg legyen valósítva az összes leszármazottban.*/
        protected abstract void DrawImage(Graphics graphics);

        //Hozz létre egy publikus függvényt MoveBall néven. A függvényben növeld meg eggyel az aktuális Ball Left tulajdonságát.
        //Az absztrakció egy másik változata az úgynevezett virtuális (virtual) elem. Ez annyiban különbözik az absztrakt elemektől, hogy alapvetően ki van fejtve, és önmagában is használható, de lehetőséget ad arra, hogy a leszármazott osztályban felülírjuk az adott függvény működését.
        public virtual void MoveBall()
        {
            Left += 1;
        }
    }
}
