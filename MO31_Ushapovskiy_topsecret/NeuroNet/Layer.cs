using System;
using System.IO;
using System.Windows.Forms;

namespace MO31_Ushapovskiy_topsecret.NeuroNet
{
    abstract class Layer
    {
        protected string name_Layer;

        string pathDirWeights;

        string pathFileWeights;
        protected int numofneurons;
        protected int numofprevneurons;

        protected const double learningrate = 0.05d;
        protected const double momentum = 0.050d;
        protected double[,] lastdelaweights;

        protected Neuron[] neurons;

        public double[] Data
        {
            set
            {
                for (int i = 0; i < numofneurons; ++i)
                {
                    neurons[i].Activator(value);
                }
            }
        }

        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurons = non; // кол-во нейронов
            numofprevneurons = nopn; // кол-во нейронов на предыдущем слое
            neurons = new Neuron[non]; // определение массива нейронов
            name_Layer = nm_Layer;

            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            lastdelaweights = new double[non, nopn + 1];
            double[,] Weights;

            if (File.Exists(pathFileWeights))
            {
                Weights = WeightsInitialize(MemoryMode.GET, pathFileWeights); // считываем файл весов
            }
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightsInitialize(MemoryMode.INIT, pathFileWeights); // если файла весов нет 
            }

            for (int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                neurons[i] = new Neuron(tmp_weights, nt);
            }
        }

        public double[,] WeightsInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ', '_' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];
            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for (int j = 0; j < numofprevneurons; j++)
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(",", "."),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case MemoryMode.SET:
                    tmpStrWeights = new string[numofneurons];
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("! ! ! ВНИМАНИЕ ! ! ! \nФайл " + name_Layer + "_memory.csv синаптических весов" +
                            "\n НЕ НАЙДЕН!!!\nПосле нажатия кнопки ОК автоматически создатся отсутствующий файл, и все значения будут сохранены в нём",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    for (int i = 0; i < numofneurons; i++)
                    {
                        tmpStr = neurons[i].Weights[0].ToString();
                        for (int j = 1; j < numofprevneurons + 1; j++)
                        {
                            tmpStr += delim[0] + neurons[i].Weights[j].ToString();
                        }
                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

                case MemoryMode.INIT:
                    MessageBox.Show("! ! ! ВНИМАНИЕ ! ! ! \nФайл " + name_Layer + "_memory.csv синаптических весов" +
                        "\nНЕ НАЙДЕН!!!\nПосле нажатия кнопки ОК автоматически создастся отсутствующий файл, и все значения будут сохранены в нём",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Random random = new Random();
                    double tmpRatio = Math.Sqrt(12.0 / numofprevneurons); // ширина интервала
                    double tmpShift = tmpRatio / 2.0;                      // сдвиг к нулю
                    tmpStrWeights = new string[numofneurons];

                    for (int i = 0; i < numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons + 1; j++)     // j = 0 — порог
                            weights[i, j] = random.NextDouble() * tmpRatio - tmpShift;

                        tmpStr = weights[i, 0].ToString(System.Globalization.CultureInfo.InvariantCulture);
                        for (int j = 1; j < numofprevneurons + 1; j++)
                            tmpStr += delim[0] + weights[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture);
                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;
                default: break;
            }
            return weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer); // 
        abstract public double[] BackwardPass(double[] stuff); // 
    }
}
