using Encog.Neural.Activation;
using Encog.Neural.Data;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.NeuralData;
using Encog.Normalize;
using Encog.Normalize.Input;
using Encog.Normalize.Output;
using SN1.Items;
using System;
using System.Collections.Generic;
using System.Windows;
namespace SN1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// TODO : wgrywanie plikow
    /// TODO : Narazie zamiast 4 tablic wgrywanych z excela ( po dwie na zbiory learningowe i testowe
    /// TODO : Sa dwie mini tablice z palca do zbioru learningowego 
    /// </summary>
    public partial class MainWindow : Window
    {
        public double[][] KLASYFIKACJA_INPUT;
        public double[][] KLASYFIKACJA_IDEAL;

        public double[][] KLASYFIKACJA_TESTOWY_INPUT;
        public double[][] KLASYFIKACJA_ODPOWIEDZI;

        public double[] KLASYFIKACJA_WYNIK;
       
        public NetworkHelper nHelp;

        public MainWindow()
        {
            InitializeComponent();
            InitialSettings();
        }

        /*Funkcja która sprawdza przygotowane parametry przed rozpoczaciem obliczen.
         Zwraca true jezeli parametry sa poprawne, false w przeciwnym razie
         Jezeli zwraca true to jest tworzona pomocnicza klasa NetworkHelper z parametrami sieci*/
        public bool ValidateIntput()
        {
            bool isCorrect = true;
            int tb1 = 0 , tb2 = 0, tb3 = 0;
            double tb4 = 0.0, tb5 = 0.0;
            //TODO: sprawdzenie ładowania pliku nauki
            //TODO: sprawdzenie ładowania pliku testowego
            bool int1 = int.TryParse(TBLayers.Text, out tb1);
            bool int2 = int.TryParse(TBNeuronsInLayer.Text, out tb2);
            bool int3 = int.TryParse(TBIteracje.Text, out tb3);
            bool double1 = double.TryParse(TBWspUczenia.Text, out tb4);
            bool double2 = double.TryParse(TBWspBezwladnosci.Text, out tb5);
            if (int1 == false || tb1 < 3)
            {
                MessageBox.Show("Bład ! Sieć musi mieć conajmniej 3 warstwy.");
                return false;
            }
            if (int2 == false || tb2 < 2)
            {
                MessageBox.Show("Bład ! Sieć musi mieć conajmniej 2 neurony w warstwie.");
                return false;
            }
            if (int3 == false || tb3 < 1)
            {
                MessageBox.Show("Bład ! Trzeba przeprowadzić conajmniej jedną iteracje.");
                return false;
            }
            if (double1 == false || tb4 > 1.0 || tb4 <= 0.0)
            {
                MessageBox.Show("Bład ! Współczynnik nauki musi być z przedziału (0;1].");
                return false;
            }
            if (double2 == false || tb5 > 1.0 || tb5 <= 0.0)
            {
                MessageBox.Show("Bład ! Współczynnik bezwładnosci musi być z przedziału (0;1].");
                return false;
            }

            if (KLASYFIKACJA_INPUT.Length <= 0 || KLASYFIKACJA_IDEAL.Length <= 0)
            {
                MessageBox.Show("Bład ! Nalezy wczytac zbior uczacy");
                return false;
            }

            if (KLASYFIKACJA_TESTOWY_INPUT.Length <= 0 )
            {
                MessageBox.Show("Bład ! Nalezy wczytac zbior testowy");
                return false;
            }



            bool bias = true;
            if(CBObciazenie.SelectedIndex == 1)
                bias = false;

            nHelp = new NetworkHelper(bias, tb1, tb2, CBAktywacje.SelectedIndex, tb4, tb5, CBProblem.SelectedIndex, tb3);

            return isCorrect;
        }

        /*Funkcja która ustawia domyslne parametry przy uruchomieniu*/
        public void InitialSettings()
        {
            KLASYFIKACJA_INPUT = new double[0][];
            KLASYFIKACJA_IDEAL = new double[0][];

            KLASYFIKACJA_TESTOWY_INPUT = new double[0][];
            KLASYFIKACJA_ODPOWIEDZI = new double[0][];
            KLASYFIKACJA_WYNIK = new double[0];
            
            CBAktywacje.SelectedIndex = 6;
            CBObciazenie.SelectedIndex = 0;
            CBProblem.SelectedIndex = 0;
        }

        /*Zdazenie obslugujace klikniecie guzika Start. Funkcje sprawdza czy parametry sa poprawne. Jezeli tak 
         to kontunuuje.
         Nastepnie przygotowuje zbiór traninowy
         Nastepnie tworzymy sieć
         Na samym koncu uruchamiane sa obliczenia
         
         */
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateIntput() == false)
                return;

            //INeuralDataSet learningSet = CombineTrainingSet(KLASYFIKACJA_INPUT, KLASYFIKACJA_IDEAL);
            INeuralDataSet learningSet = NormaliseDataSet(KLASYFIKACJA_INPUT, KLASYFIKACJA_IDEAL);
        
            //INeuralDataSet trainingSet = CombineTrainingSet(KLASYFIKACJA_TESTOWY_INPUT, KLASYFIKACJA_ODPOWIEDZI);
            INeuralDataSet trainingSet = NormaliseDataSet(KLASYFIKACJA_TESTOWY_INPUT, KLASYFIKACJA_ODPOWIEDZI);


            ITrain learning = CreateNeuronNetwork(learningSet);
            int iteracja = 0; 
            do
            {
                learning.Iteration();
                Console.WriteLine("Epoch #" + iteracja + " Error:" + learning.Error);
                iteracja++;
                
            } while ((iteracja < nHelp.liczbaIteracji) && (learning.Error > 0.0005));

            // TUTAJ SKONCZYL SIE PROCES NAUKI
            // POWINNISMY NA TA SIEC NALOZYC TERAZ ZBIOR TESTOWY
            // ORAZ NARYSOWAC GRAFY
            int i=0;
            Console.WriteLine("Neural Network Results:");
            foreach (INeuralDataPair pair in trainingSet)
            {
                INeuralData output = learning.Network.Compute(pair.Input);
                if ((int)(output[0]) == 1)
                    KLASYFIKACJA_WYNIK[i] = 1.0;
                else if ((int)(output[1]) == 1)
                    KLASYFIKACJA_WYNIK[i] = 2.0;
                else
                    KLASYFIKACJA_WYNIK[i] = 3.0;
                i++;
                //Console.WriteLine(pair.Input[0] + "," + pair.Input[1]
                //+ ", actual=" + output[0] + ", " + output[1] + ", " + output[2] + ",ideal=" + pair.Ideal[0] + ", " + pair.Ideal[1] + ", " + pair.Ideal[2]);
            }

            Console.WriteLine("Calculated");

        }

        public INeuralDataSet NormaliseDataSet ( double[][] input, double[][] ideal)
        {
            double [][] norm_input = new double[input.Length][];

            double max = input[0][0], min = input[0][0];

            for(int i = 0 ; i < input.Length ; i ++)
            {
                if (input[i][0] < min)
                    min = input[i][0];
                if (input[i][1] < min)
                    min = input[i][1];

                if (input[i][0] > max)
                    max = input[i][0];
                if (input[i][1] > max)
                    max = input[i][1];
            }


            for (int i = 0; i < input.Length; i++)
            {
                norm_input[i] = new double[2];
                norm_input[i][0] = (input[i][0] - min) / (max - min);
                norm_input[i][1] = (input[i][1] - min) / (max - min);
            }

            INeuralDataSet dataset = CombineTrainingSet(norm_input, ideal);
            return dataset;
        }


        /*Funckja laczy dane wejsciowe zbioru uczacego z oczekiwanymi odpowiedziami w jeden obiekt bilbiotego Encog*/
        public INeuralDataSet CombineTrainingSet(double[][] dane, double[][] odpowiedzi)
        {
            return new BasicNeuralDataSet(dane, odpowiedzi);
        }

        /*Funkcja z parametrow tworzy odpowiednia siec neuronowa
         
         * TODO : Czy na koncu musi byc jeden neuron czy 3 ?
         * TODO : Jak zrobic zeby zamiast 'new ActivationSigmoid' byla jedna z wielu funkcji 
         * z comboBoxa CBAktywacje ?
         */
        public ITrain CreateNeuronNetwork(INeuralDataSet trainingSet)
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), nHelp.bias, 2));

            for (int i = 0; i < nHelp.layers-2; i++ )
                network.AddLayer(new BasicLayer(new ActivationSigmoid(), nHelp.bias, nHelp.neurons));

            if(nHelp.problem == 0)
                network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 3));
            else
                network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));

            network.Structure.FinalizeStructure();
            network.Reset();
            
            ITrain train = new Backpropagation(network, trainingSet, nHelp.learning, nHelp.momentum);
            return train;
        }

        private void WczytajUczacy_Click(object sender, RoutedEventArgs e)
        {
            var fileReader = new FileReader();
            List<RowObject> items = fileReader.GetItems();
            KLASYFIKACJA_INPUT = new double[items.Count][];
            KLASYFIKACJA_IDEAL = new double[items.Count][];
            for (int i = 0; i < items.Count; i++)
            {
                var item = (RowObject)items[i];
                KLASYFIKACJA_INPUT[i] = new double[2];
                KLASYFIKACJA_INPUT[i][0] = item.x;
                KLASYFIKACJA_INPUT[i][1] = item.y.Value;

                KLASYFIKACJA_IDEAL[i] = new double[3] { 0.0, 0.0, 0.0 };
                KLASYFIKACJA_IDEAL[i][item.cls.Value-1] = 1.0;
            }
        }

        private void WczytajTestowy_Click(object sender, RoutedEventArgs e)
        {
            var fileReader = new FileReader();
            List<RowObject> items = fileReader.GetItems();
            KLASYFIKACJA_TESTOWY_INPUT = new double[items.Count][];
            KLASYFIKACJA_ODPOWIEDZI = new double[items.Count][];
            KLASYFIKACJA_WYNIK = new double[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                var item = (RowObject)items[i];
                KLASYFIKACJA_TESTOWY_INPUT[i] = new double[2];
                KLASYFIKACJA_TESTOWY_INPUT[i][0] = item.x;
                KLASYFIKACJA_TESTOWY_INPUT[i][1] = item.y.Value;
                KLASYFIKACJA_ODPOWIEDZI[i] = new double[3] { 0.0, 0.0, 0.0 };
            }
        }

    }
}

