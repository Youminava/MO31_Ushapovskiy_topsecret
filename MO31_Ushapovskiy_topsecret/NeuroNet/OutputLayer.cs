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
            // errors[k] = Fact[k] - Target[k]
            double[] gr_sum = new double[numofprevneurons];

            // 1. Градиент для предыдущего слоя (считаем ДО обновления весов)
            for (int j = 0; j < numofprevneurons; ++j)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; ++k)
                    sum += neurons[k].Weights[j + 1] * errors[k];
                gr_sum[j] = sum;
            }

            // 2. Коррекция весов с моментом
            for (int i = 0; i < numofneurons; ++i)
            {
                for (int n = 0; n < numofprevneurons + 1; ++n)
                {
                    double input = (n == 0) ? 1.0 : neurons[i].Inputs[n - 1]; // n == 0 — порог
                    double deltaw = momentum * lastdelaweights[i, n]
                                  - learningrate * input * errors[i];

                    lastdelaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw;
                }
            }

            return gr_sum;
        }

    }
}
