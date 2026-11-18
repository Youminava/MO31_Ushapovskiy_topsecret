using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO31_Ushapovskiy_topsecret.NeuroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type)
        {

        }
        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hiden_out = new double[numofneurons];
            for (int i = 0; i < numofneurons; i++)
            {
                hiden_out[i] = neurons[i].Output;
            }
            nextLayer.Data = hiden_out;
        }
        public override double[] BackwardPass(double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];
            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                    sum += neurons[k].Weights[j + 1] * neurons[k].Derivative * gr_sum[k];
                gr_sum[j] = sum;
            }

            for(int i = 0; i < numofneurons; i++)
                for(int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0)
                        deltaw = learningrate * neurons[i].Derivative * gr_sums[i];
                    else
                        deltaw = learningrate * neurons[i].Inputs[n - 1] * neurons[i].Derivative;
                    lastdelaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw;
                }

            return gr_sum;
        }
    }
}
