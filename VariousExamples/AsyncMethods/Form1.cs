using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using TaskDemo;
using System.Threading;

namespace AsyncMethods
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void FactorialButtonClick(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(valueOfN.Text, out n))
            {
                factorialBox.Clear();
                Task<BigInteger> task = Task<BigInteger>.Run(() => Tasks.Factorial(n));
                factorialBox.Text = task.Result.ToString();
            }
        }

        //private CancellationTokenSource tokenSource;

        private async void FactorialButtonClickAsync(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(valueOfN.Text, out n))
            {
                factorialBox.Clear();
                Task<BigInteger> task = Task<BigInteger>.Run(() => Factorial(n));
                BigInteger result = await task;
                factorialBox.Text = result.ToString();
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Returns n!
        /// </summary>
        private static BigInteger Factorial(int n)
        {
            BigInteger result = 1;
            while (n > 0)
            {
                result *= n;
                n--;
            }
            return result;
        }
    }
}
