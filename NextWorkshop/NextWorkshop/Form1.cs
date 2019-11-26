using Svt.Caspar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextWorkshop
{
    public partial class Form1 : Form
    {
        private CasparDevice _device = new CasparDevice();

        public Form1()
        {
            InitializeComponent();

            // init caspar device
            _device.Settings.Hostname = "172.20.130.69";
            _device.Settings.Port = 5250;
            _device.Settings.AutoConnect = true;

            // set color
            lblColor.BackColor = cdColorPicker.Color;

            // connect to caspar server
            _device.Connect();

            // set connection status delegate
            _device.ConnectionStatusChanged += new EventHandler<Svt.Network.ConnectionEventArgs>(delegate (object sender, Svt.Network.ConnectionEventArgs e) 
            {
                if (e.Connected)
                {
                    this.SetTitleBar($"Connected to {e.Hostname} at {e.Port}");
                }
                else
                {
                    this.SetTitleBar("Disconnected");
                }
            });
        }

        private void SetTitleBar(string Text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Text = Text;
            });
        }

        private void lblColor_Click(object sender, EventArgs e)
        {
            cdColorPicker.ShowDialog();
            lblColor.BackColor = cdColorPicker.Color;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // create datacollection
            CasparCGDataCollection DataCollection = new CasparCGDataCollection();
            DataCollection.SetData("Lijn1", txtLijn1.Text);
            DataCollection.SetData("Lijn2", txtLijn2.Text);
            DataCollection.SetData("Color", $"0x{cdColorPicker.Color.R.ToString("X2")}{cdColorPicker.Color.G.ToString("X2")}{cdColorPicker.Color.B.ToString("X2")}");

            // load template with datacollection
            _device.Channels[0].CG.Add(0, "kdg/lowerthird", DataCollection);
            _device.Channels[0].CG.Play(0);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _device.Channels[0].CG.Next(0);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _device.Channels[0].Clear();
        }
    }
}
