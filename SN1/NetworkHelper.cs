using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SN1
{
    public class NetworkHelper
    {
        public bool bias;
        public int layers;
        public int neurons;
        public int func;
        public double learning;
        public double momentum;
        public int problem;
        public int liczbaIteracji;

        public NetworkHelper(bool _bias, int _layers, int _neurons , int _func, double _learning, double _momentum, int _problem, int _iteracje)
        {
            this.bias = _bias;
            this.layers = _layers;
            this.neurons = _neurons;
            this.func = _func;
            this.learning = _learning;
            this.momentum = _momentum;
            this.problem = _problem;
            this.liczbaIteracji = _iteracje;
        }
    }
}
