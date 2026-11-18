using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO31_Ushapovskiy_topsecret.NeuroNet
{
    class OutputLayer: Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type)
        {
        }
        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < neurons.Length; i++)
            {
                e_sum += neurons[i].Output;
            }
            for (int i = 0; i < neurons.Length; i++)
            {
                net.Fact[i] = neurons[i].Output / e_sum;
            }
        }

        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[numofprevneurons];

            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofprevneurons; k++)
                    sum += neurons[k].Weights[j + 1] * errors[k];
                gr_sum[j] = sum;
            }

            for (int i = 0; i < numofneurons; i++) 
                for(int n = 0; n < numofprevneurons; n++)
                {
                    double deltaw;
                    if (n == 0)
                        deltaw = learningrate * errors[i];
                    else
                        deltaw = lastdelaweights[i, n] + 
                            learningrate * neurons[i].Inputs[n - 1] * errors[i];
                    lastdelaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw; // 
                }   

            return gr_sum;
        }

    }
}
