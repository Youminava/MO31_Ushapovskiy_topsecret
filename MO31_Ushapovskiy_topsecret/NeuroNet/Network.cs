namespace MO31_Ushapovskiy_topsecret.NeuroNet
{
    class Network
    {
        private InputLayer inputLayer = null;
        private HiddenLayer hiddenLayer1 = new HiddenLayer(70, 15, NeuronType.Hidden, "HiddenLayer1");
        private HiddenLayer hiddenLayer2 = new HiddenLayer(30, 70, NeuronType.Hidden, "HiddenLayer2");
        private OutputLayer outputLayer = new OutputLayer(10, 30, NeuronType.Output, "OutputLayer");
        private double[] fact = new double[10];
        private double[] e_error_avr;
        public double[] Fact
        {
            get => fact;
        }
        public Network() { }

        public void ForwardPass(double[] netInput)
        {
            hiddenLayer1.Data = netInput;
            hiddenLayer2.Recognize(null, hiddenLayer2);
            hiddenLayer2.Recognize(null, outputLayer);
            outputLayer.Recognize(this, null);
        }
    }
}
