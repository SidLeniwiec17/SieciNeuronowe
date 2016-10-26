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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

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
        IActivationFunction ActivationFunction { get; set; }
        public double[][] neuralInput;
        public double[][] neuralIdeal;
        public double[][] neuralTestInput;
        public double[][] neuralAnswers;
        public double[] neuralAnswer;
        public List<double> errors = new List<double>();
        public int sety = 4;
       
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

            if (neuralInput.Length <= 0 || neuralIdeal.Length <= 0)
            {
                MessageBox.Show("Bład ! Nalezy wczytac zbior uczacy");
                return false;
            }

            if (neuralTestInput.Length <= 0 )
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
            neuralInput = new double[0][];
            neuralIdeal = new double[0][];

            neuralTestInput = new double[0][];
            neuralAnswers = new double[0][];
            neuralAnswer = new double[0];
            
            CBAktywacje.SelectedIndex = 3;
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
            errors = new List<double>();
            if (ValidateIntput() == false)
                return;

            INeuralDataSet learningSet, trainingSet;

            learningSet = CombineTrainingSet(neuralInput, neuralIdeal);
            //learningSet = NormaliseDataSet(neuralInput, neuralIdeal);

            trainingSet = CombineTrainingSet(neuralTestInput, neuralAnswers);
            //trainingSet = NormaliseDataSet(neuralTestInput, neuralAnswers);
       
            //NormaliseDataSetReg

            ITrain learning = CreateNeuronNetwork(learningSet);
            int iteracja = 0; 
            do
            {
                learning.Iteration();
                Console.WriteLine("Epoch #" + iteracja + " Error:" + learning.Error);
                errors.Add(learning.Error);
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
                if (CBProblem.SelectedIndex == 0)
                {
                    if (sety == 4)
                    {
                        if ((double)(output[0]) >= 0.6)
                            neuralAnswer[i] = 1.0;
                        else if ((double)(output[1]) >=0.6)
                            neuralAnswer[i] = 2.0;
                        else if ((double)(output[2]) >= 0.6)
                            neuralAnswer[i] = 3.0;
                        else
                            neuralAnswer[i] = 4.0;
                    }
                    else if (sety == 3)
                    {
                        if ((double)(output[0]) >= 0.6)
                            neuralAnswer[i] = 1.0;
                        else if ((double)(output[1]) >= 0.6)
                            neuralAnswer[i] = 2.0;
                        else
                            neuralAnswer[i] = 3.0;
                    }
                }
                else
                    neuralAnswer[i] = output[0];
                i++;
                //Console.WriteLine(pair.Input[0] + "," + pair.Input[1]
                //+ ", actual=" + output[0] + ", " + output[1] + ", " + output[2] + ",ideal=" + pair.Ideal[0] + ", " + pair.Ideal[1] + ", " + pair.Ideal[2]);
            }

            Console.WriteLine("Calculated");
            CreateErrorFile();
            if(CBProblem.SelectedIndex == 0)
            {
                CreateClassificationFile();
            }
            else
            {
                CreateRegressionFile();
            }

        }

        public INeuralDataSet NormaliseDataSet ( double[][] input, double[][] ideal)
        {
            double[][] norm_input = new double[input.Length][];
              
            double max = input[0][0], min = input[0][0];

                for (int i = 0; i < input.Length; i++)
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
            if (nHelp.problem == 0)
                network.AddLayer(new BasicLayer(ActivationFunction, nHelp.bias, 2));
            else
                network.AddLayer(new BasicLayer(ActivationFunction, nHelp.bias, 1));

            for (int i = 0; i < nHelp.layers-2; i++ )
                network.AddLayer(new BasicLayer(ActivationFunction, nHelp.bias, nHelp.neurons));

            if(nHelp.problem == 0)
                network.AddLayer(new BasicLayer(ActivationFunction, false, 4));
            else
                network.AddLayer(new BasicLayer(ActivationFunction, false, 1));

            network.Structure.FinalizeStructure();
            network.Reset();       
            ITrain train = new Backpropagation(network, trainingSet, nHelp.learning, nHelp.momentum);
            return train;
        }

        private void WczytajUczacy_Click(object sender, RoutedEventArgs e)
        {
            var fileReader = new FileReader();
            List<RowObject> items = fileReader.GetItems();
            neuralInput = new double[items.Count][];
            neuralIdeal = new double[items.Count][];
            RowObject item = new RowObject();
            for (int i = 0; i < items.Count; i++)
            {
                item = (RowObject)items[i];
                if(CBProblem.SelectedIndex == 0)
                {
                    neuralInput[i] = new double[2];
                    neuralInput[i][0] = item.x;
                    neuralInput[i][1] = item.y.Value;

                    if(sety == 4)
                    neuralIdeal[i] = new double[4] { 0.0, 0.0, 0.0, 0.0 };
                    else if (sety == 3)
                    neuralIdeal[i] = new double[3] {0.0, 0.0, 0.0 };
                    
                    neuralIdeal[i][item.cls.Value - 1] = 1.0;
                }
                else
                {
                    neuralInput[i] = new double[1];
                    neuralInput[i][0] = item.x;

                    neuralIdeal[i] = new double[1] { 0.0};
                    neuralIdeal[i][0] = item.y.Value;
                }
            }
            if ((CBProblem.SelectedIndex == 1 && item.cls.HasValue) || (CBProblem.SelectedIndex == 0 && !item.cls.HasValue))
            {
                MessageBox.Show("Input error");
                this.Close();
            }

        }

        private void WczytajTestowy_Click(object sender, RoutedEventArgs e)
        {
            var fileReader = new FileReader();
            List<RowObject> items = fileReader.GetItems();
            neuralTestInput = new double[items.Count][];
            neuralAnswers = new double[items.Count][];
            neuralAnswer = new double[items.Count];

            RowObject item = new RowObject();
            for (int i = 0; i < items.Count; i++)
            {
                item = (RowObject)items[i];
                if (CBProblem.SelectedIndex == 0)
                {
                    neuralTestInput[i] = new double[2];
                    neuralTestInput[i][0] = item.x;
                    neuralTestInput[i][1] = item.y.Value;

                    if (sety == 4)
                        neuralAnswers[i] = new double[4] { 0.0, 0.0, 0.0, 0.0 };
                    else if (sety == 3)
                        neuralAnswers[i] = new double[3] { 0.0, 0.0, 0.0 };
                    
                }
                else
                {
                    neuralTestInput[i] = new double[1];
                    neuralTestInput[i][0] = item.x;
                    neuralAnswers[i] = new double[1] { 0.0 };
                }
            }
            /*if ((CBProblem.SelectedIndex == 1 && item.cls.HasValue) || (CBProblem.SelectedIndex == 0 && !item.cls.HasValue))
            {
                MessageBox.Show("Input error");
                this.Close();
            }*/
        }


        private void CBAktywacje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)CBAktywacje.SelectedItem;
            string value = typeItem.Content.ToString();
            switch (value)
            {
                case "BiPolar":
                    ActivationFunction = new ActivationBiPolar();
                    break;
                case "Linear":
                    ActivationFunction = new ActivationLinear();
                    break;
                case "LOG":
                    ActivationFunction = new ActivationLOG();
                    break;
                case "Sigmoid":
                    ActivationFunction = new ActivationSigmoid();
                    break;
                case "SIN":
                    ActivationFunction = new ActivationSIN();
                    break;
                case "SoftMax":
                    ActivationFunction = new ActivationSoftMax();
                    break;
                case "TANH":
                    ActivationFunction = new ActivationTANH();
                    break;
            }
        }
        public void CreateErrorFile()
        {
            string line = "";
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("errors.R");

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            line = "points<- c(";
            int i = 0;
            for (i=0;i<errors.Count-1;i++)
            {
                line += errors[i].ToString(nfi) + ",";
            }
            line += errors[i].ToString(nfi) + ")";
            file.WriteLine(line);
            file.WriteLine(@"plot(points , type= ""o"", col= ""red"")");
            file.WriteLine(@"title(main= ""Error"", col.main= ""black"", font.main= 4)");
            //points<- c(0.9, 0.8, 0.8, 0.8, 0.7, 0.6666, 0.655, 0.65544, 0.3, 0.02, 0.015, 0.002)
            //plot(points , type= "o", col= "red")
            //title(main= "Error", col.main= "black", font.main= 4)

            file.Close();
        }
        public void CreateRegressionFile()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            System.IO.StreamWriter file = new System.IO.StreamWriter("regressionChart.R");
            string line1;
            string line2;
            line1 = "x <- c(";
            line2 = "y <- c(";
            int i;
            for (i=0;i<neuralTestInput.Length-1;i++)
            {
                line1 += neuralTestInput[i][0].ToString(nfi) + ",";
                line2 += neuralAnswer[i].ToString(nfi) + ",";
            }
            line1 += neuralTestInput[i][0].ToString(nfi) + ")";
            line2 += neuralAnswer[i].ToString(nfi) + ")";
            file.WriteLine(line1);
            file.WriteLine(line2);
            file.WriteLine(@"plot(x, y, type=""p"", col=""black"" ,main=""Regression"")");
            file.Close();
        }

        public void CreateClassificationFile()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            System.IO.StreamWriter file = new System.IO.StreamWriter("classificationChart.R");
            string line1= "x <- c(";
            string line2 = "y <- c(";
            string line3 = "col <- c(";
            int i;
            for(i=0;i<neuralTestInput.Length-1;i++)
            {
                line1 += neuralTestInput[i][0].ToString(nfi) + ",";
                line2 += neuralTestInput[i][1].ToString(nfi) + ",";
                line3 += (int)neuralAnswer[i] + ",";
            }
            line1 += neuralTestInput[i][0].ToString(nfi) + ")";
            line2 += neuralTestInput[i][1].ToString(nfi) + ")";
            line3 += (int)neuralAnswer[i] + ")";
            file.WriteLine(line1);
            file.WriteLine(line2);
            file.WriteLine(line3);
            file.WriteLine(@"plot(x, y, pch= 21, bg= c(""red"", ""green3"", ""blue"", ""yellow"")[col], main= ""Classification"")");
            file.Close();         
        }
    }
}



