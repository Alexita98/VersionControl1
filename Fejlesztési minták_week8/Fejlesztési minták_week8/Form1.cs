using Fejlesztési_minták_week8.Abstractions;
using Fejlesztési_minták_week8.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fejlesztési_minták_week8
{
    public partial class Form1 : Form
    {
        //Hozz létre a Form1 osztály szintjén egy Ball típusú elemekből álló listát _balls néven.
        private List<Toy> _toys = new List<Toy>();

        private IToyFactory _factory;

        //Hozz létre egy BallFactory típusú kifejtett propertyt is Factory néven
        public IToyFactory Factory
        {
            get { return _factory; }
            set { _factory = value;
                DisplayNext();
            }
        }

        //Hozz létre egy osztályszintű Toy típusú változót _nextToy néven
        private Toy _nextToy;

        public Form1()
        {
            InitializeComponent();

            //A konstruktorban töltsd fel a Factory változót egy BallFactory példánnyal.
            Factory = new CarFactory();
            //Factory = new BallFactory();
        }

        private void CreateTimer_Tick(object sender, EventArgs e)
        {
            //A createTimer eseménykezelőjében a Factory CreateNew metódusát felhasználva hozz létre egy Ball példányt. Add hozzá a _balls listához, és a mainPanel vezérlőihez. 
            var toy = Factory.CreateNew();
            _toys.Add(toy);
            mainpanel.Controls.Add(toy);

            //A Left tulajdonságát pedig állítsd a szélessége negatív értékére. (Ezzel a képernyőn kívül jön majd létre a labda és a futószalagon szép folyamatosan úszik majd be.)
            toy.Left = -toy.Width;
        }

        private void ConveyorTimer_Tick(object sender, EventArgs e)
        {
            //egy a cikluson kívüli segédváltozóval tárold le a leginkább jobbra levő Ball példány pozícióját.
            var maxPosition = 0;
            foreach (var b in _toys)
            {
                b.MoveToy();
                if (b.Left > maxPosition)
                {
                    maxPosition = b.Left;
                }

                //A ciklus után, ha a legnagyobb pozíció eléri az 1000-et akkor tárold le egy változóba a _balls lista első elemét és töröld ki a listából és a Form vezérlőiből is.
                if (maxPosition>=1000)
                {
                    var OldestToy = _toys[0];
                    _toys.Remove(OldestToy);
                    mainpanel.Controls.Remove(OldestToy);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e) //Car
        {
            Factory = new CarFactory();
        }

        private void Button2_Click(object sender, EventArgs e) //Ball
        {
            Factory = new BallFactory();
        }

        private void DisplayNext()
        {
            if (_nextToy != null)
                Controls.Remove(_nextToy);
            _nextToy = Factory.CreateNew();
            _nextToy.Top = label1.Top + label1.Height + 20;
            _nextToy.Left = label1.Left;
            Controls.Add(_nextToy);
        }
    }

}
