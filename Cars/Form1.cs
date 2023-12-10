using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cars
{
    public partial class Form1 : Form
    {
        private object bridgeLock = new object();
        private bool bridgeOccupied = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Monitor.Enter(bridgeLock);
            try
            {
                while (bridgeOccupied)
                {
                    Monitor.Wait(bridgeLock);
                }
                pictureBox2.Left -= 10;
                bridgeOccupied = true;
            }
            finally
            {
                Monitor.Exit(bridgeLock);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Monitor.Enter(bridgeLock);
            try
            {
                while (bridgeOccupied)
                {
                    Monitor.Wait(bridgeLock);
                }
                pictureBox3.Left -= 10;
                bridgeOccupied = false;
            }
            finally
            {
                Monitor.Exit(bridgeLock);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private volatile bool allowMovement = false;

        private void Start_Click(object sender, EventArgs e)
        {
            allowMovement = true;
            Thread car1Thread = new Thread(Car1Movement);
            Thread car2Thread = new Thread(Car2Movement);
            car1Thread.Start();
            car2Thread.Start();
        }

        private void Car1Movement()
        {
            while (allowMovement)
            {
                Monitor.Enter(bridgeLock);
                try
                {
                    while (bridgeOccupied)
                        Monitor.Wait(bridgeLock);
                    bridgeOccupied = true;
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        pictureBox2.Left += 10;
                    });
                }
                finally
                {
                    Monitor.Exit(bridgeLock);
                }
                Thread.Sleep(500);
            }
        }

        private void Car2Movement()
        {
            while (allowMovement)
            {
                Monitor.Enter(bridgeLock);
                try
                {
                    while (bridgeOccupied)
                        Monitor.Wait(bridgeLock);
                    bridgeOccupied = false;
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        pictureBox3.Left -= 10;
                    });
                }
                finally
                {
                    Monitor.Exit(bridgeLock);
                }
                Thread.Sleep(500);
            }
        }
    }
}
