using Encog.Neural.Activation;
using Encog.Neural.Data;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.NeuralData;
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
        public double[][] KLASYFIKACJA_INPUT ={
                                                new double[2] { -0.120835004807233, 0.185909693092369 },
                                                new double[2] { -0.0981255045917765, 0.6999098823963 },
                                                new double[2] { 0.0778210176149909, 0.439883965705725 },
                                                new double[2] { 0.539304250361261, 0.148853158752743 } ,
                                                new double[2] { 0.21250722755268, 0.867787766532976 },
                                                new double[2] { -0.0212862244396033,0.295283891486845}, 

                                                new double[2] { 0.295601662332601, 0.544705418646667 },
                                                new double[2] { -0.154313932113388, 0.304927409695447 },
                                                new double[2] {-0.0605675806479125, 0.347600285504779 },
                                                new double[2] { 0.0968516969760852, 0.445603711822102 } ,
                                                new double[2] { -0.253502048302602, 0.277060250829347 },
                                                new double[2] { 0.297582547062529,0.551893928088361}, 

                                                new double[2] { -0.321482745560569, 0.155377462442698 },
                                                new double[2] { -0.742035943895382, 0.214322961393839 },
                                                new double[2] {-0.828034138303721, 0.194952273583263 },
                                                new double[2] { -0.893115411309494, 0.237755562196031} ,
                                                new double[2] { -0.63389483375305, 0.261240601080596 },
                                                new double[2] { -0.682195460002969,0.190750388286338}, 

                                                new double[2] { -0.343425484304401, 0.205441700604293 },
                                                new double[2] { -0.723574053959815, 0.287234401873544 },
                                                new double[2] { -0.623332263941705, 0.202120808652005 },
                                                new double[2] { -0.747225445090313, 0.196389294363966} ,
                                                new double[2] { -0.903236472201214, 0.20713991529638 },
                                                new double[2] { -0.26158428291182, 0.241401008413324},

                                                new double[2] { 0.138491831342076, -0.290209732527407 },
                                                new double[2] { -0.117770161902206, -0.318418371805794 },
                                                new double[2] { 0.154527592206068, -0.386415823250062 },
                                                new double[2] { 0.191029508101819, -0.42328951740268 } ,
                                                new double[2] { 0.154843979455491, -0.514968676573543 },
                                                new double[2] { -0.00506506800112611,-0.0814994894139582},

                                                new double[2] { -0.0416822049597956, -0.17245900641548 },
                                                new double[2] { -0.197651109323041, 0.493131239528278 },
                                                new double[2] { -0.2746436119221, 0.27560509493857 },
                                                new double[2] { -0.19841506694444, -0.552323916767805 } ,
                                                new double[2] { 0.00410489596212299, -0.235381956512001 },
                                                new double[2] { -0.0858066440374611,-0.503867349987556}
                                            };
         public double[][] KLASYFIKACJA_IDEAL = {                                              
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},
                                                  new double[3] { 1.0 , 0.0, 0.0},

                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  new double[3] { 0.0 , 1.0, 0.0 },
                                                  
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0},
                                                  new double[3] { 0.0 , 0.0, 1.0}
                                                  };
         
       /* public static double[][] KLASYFIKACJA_IDEAL = {                                              
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },
                                                 new double[1] { 1.0 },

                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
                                                 new double[1] { 2.0 },
 
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 },
                                                 new double[1] { 3.0 }
                                             };*/
        
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

            bool bias = true;
            if(CBObciazenie.SelectedIndex == 1)
                bias = false;

            nHelp = new NetworkHelper(bias, tb1, tb2, CBAktywacje.SelectedIndex, tb4, tb5, CBProblem.SelectedIndex, tb3);

            return isCorrect;
        }

        /*Funkcja która ustawia domyslne parametry przy uruchomieniu*/
        public void InitialSettings()
        {
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

            INeuralDataSet trainingSet = CombineTrainingSet(KLASYFIKACJA_INPUT, KLASYFIKACJA_IDEAL);

            ITrain training = CreateNeuronNetwork(trainingSet);
            int iteracja = 0; 
            do
            {
                training.Iteration();
                Console.WriteLine("Epoch #" + iteracja + " Error:" + training.Error);
                iteracja++;
            } while ((iteracja < nHelp.liczbaIteracji) && (training.Error > 0.0005));

            // TUTAJ SKONCZYL SIE PROCES NAUKI
            // POWINNISMY NA TA SIEC NALOZYC TERAZ ZBIOR TESTOWY
            // ORAZ NARYSOWAC GRAFY

            Console.WriteLine("Neural Network Results:");
            foreach (INeuralDataPair pair in trainingSet)
            {
                /*INeuralData output = training.Network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + "," + pair.Input[1]
                + ", actual=" + output[0] + ",ideal=" + pair.Ideal[0]);*/

                INeuralData output = training.Network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + "," + pair.Input[1]
                + ", actual=" + output[0] + ", " + output[1] + ", " + output[2] + ",ideal=" + pair.Ideal[0] + ", " + pair.Ideal[1] + ", " + pair.Ideal[2]);
            }


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

        private void SelectCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var fileReader = new FileReader();
            List<ThreeVariableItem> items = fileReader.GetItems();
            KLASYFIKACJA_INPUT = new double[items.Count][];
            KLASYFIKACJA_IDEAL = new double[items.Count][];
            for (int i = 0; i < items.Count; i++)
            {
                var item = (ThreeVariableItem)items[i];
                KLASYFIKACJA_INPUT[i] = new double[2];
                KLASYFIKACJA_INPUT[i][0] = item.x;
                KLASYFIKACJA_INPUT[i][1] = item.y;

                KLASYFIKACJA_IDEAL[i] = new double[3] { 0.0, 0.0, 0.0 };
                KLASYFIKACJA_IDEAL[i][item.cls - 1] = 1.0;
            }
        }

    }
}
