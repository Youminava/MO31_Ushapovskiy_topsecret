using System.Windows.Forms;
using static System.Math;

namespace MO31_Ushapovskiy_topsecret.NeuroNet
{
    class Neuron
    {
        private NeuronType type;
        private double[] weights;
        private double[] inputs;
        private double output;
        private double derivative;

        private double a = 0.01d;

        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }

        public double Output { get => output; }

        public double Derivative { get => derivative; }

        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        public void Activator(double[] i)
        {
            inputs = i;
            double sum = weights[0];
            for (int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weights[j + 1];
            }

            switch (type)
            {
                case NeuronType.Hidden: 
                    output = LeakyReLU(sum);
                    derivative = LeakyReLUDerivative(sum);
                    break;

                case NeuronType.Output: 
                    output = Exp(sum);
                    break;
            }
        }
        private double LeakyReLU(double x)
        {
            if (x >= 0)
                return x;
            else
                return a * x;
        }
        private double LeakyReLUDerivative(double sum)
        {
            if (sum >= 0)
                return 1;
            else
                return a;
        }
    }
}

