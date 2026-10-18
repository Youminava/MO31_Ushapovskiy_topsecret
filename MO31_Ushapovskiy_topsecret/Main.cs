using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MO31_Ushapovskiy_topsecret
{
    public partial class Main : Form
    {
        private double[] inputPixels;
        public Main()
        {
            InitializeComponent();
            inputPixels = new double[15];
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
    }
}
