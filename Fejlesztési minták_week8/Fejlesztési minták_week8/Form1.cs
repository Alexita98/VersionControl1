﻿using Fejlesztési_minták_week8.Entities;
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
        private List<Ball> _balls = new List<Ball>();

        private BallFactory _factory;

        //Hozz létre egy BallFactory típusú kifejtett propertyt is Factory néven
        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }


        public Form1()
        {
            InitializeComponent();

            //A konstruktorban töltsd fel a Factory változót egy BallFactory példánnyal.
            Factory = new BallFactory();
        }

        private void CreateTimer_Tick(object sender, EventArgs e)
        {
            //A createTimer eseménykezelőjében a Factory CreateNew metódusát felhasználva hozz létre egy Ball példányt. Add hozzá a _balls listához, és a mainPanel vezérlőihez. 
            var ball = Factory.CreateNew();
            _balls.Add(ball);
            mainpanel.Controls.Add(ball);

            //A Left tulajdonságát pedig állítsd a szélessége negatív értékére. (Ezzel a képernyőn kívül jön majd létre a labda és a futószalagon szép folyamatosan úszik majd be.)
            ball.Left = -ball.Width;
        }

        private void ConveyorTimer_Tick(object sender, EventArgs e)
        {
            //egy a cikluson kívüli segédváltozóval tárold le a leginkább jobbra levő Ball példány pozícióját.
            var maxPosition = 0;
            foreach (var b in _balls)
            {
                b.MoveBall();
                if (b.Left > maxPosition)
                {
                    maxPosition = b.Left;
                }

                //A ciklus után, ha a legnagyobb pozíció eléri az 1000-et akkor tárold le egy változóba a _balls lista első elemét és töröld ki a listából és a Form vezérlőiből is.
                if (maxPosition>=1000)
                {
                    var OldestBall = _balls[0];
                    _balls.Remove(OldestBall);
                    mainpanel.Controls.Remove(OldestBall);
                }
            }
        }
    }
}
