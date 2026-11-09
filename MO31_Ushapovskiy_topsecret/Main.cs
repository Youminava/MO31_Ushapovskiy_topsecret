using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MO31_Ushapovskiy_topsecret.NeuroNet;

namespace MO31_Ushapovskiy_topsecret
{
    public partial class Main : Form
    {
        private double[] inputPixels;
        private Network network;
        public Main()
        {
            InitializeComponent();
            inputPixels = new double[15];
            network = new Network();
            foreach (Control c in this.Controls)
            {
                if (c is Button && c.TabIndex < 15)
                {
                    c.Click += Changing_State_Pixel_Button_Click;
                }
            }
        }
        private void Changing_State_Pixel_Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Black)
            {
                ((Button)sender).BackColor = Color.White;
                inputPixels[((Button)sender).TabIndex] = 1d;
            }
            else
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }

        private void ButtonSaveTrainSimple_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();
            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += '\n';
            File.AppendAllText(path, tmpStr);
        }

        private void button_recognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(inputPixels);
            label_Output.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            label_Probability.Text = (100 * network.Fact.Max()).ToString("0.00") + "%";
        }
    }
}
