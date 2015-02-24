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
            else
            {
                factorialBox.Text = "Not an int";
            }
        }

        private CancellationTokenSource tokenSource;

        private async void FactorialButtonClickAsync(object sender, EventArgs e)
        {
            tokenSource = new CancellationTokenSource();
            int n;
            if (int.TryParse(valueOfN.Text, out n))
            {
                button3.Enabled = true;
                button2.Enabled = false;
                factorialBox.Clear();
                Task<BigInteger> task = Task<BigInteger>.Run(() => Factorial(n, tokenSource.Token), tokenSource.Token);
                try
                {
                    BigInteger result = await task;
                    factorialBox.Text = result.ToString();
                }
                catch (OperationCanceledException)
                {
                    factorialBox.Text = "Cancelled";
                }
                button3.Enabled = false;
                button2.Enabled = true;
                // This belongs in a finally block:
                tokenSource.Dispose();
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        /// <summary>
        /// Returns n!
        /// </summary>
        private static BigInteger Factorial(int n, CancellationToken cancel)
        {
            BigInteger result = 1;
            while (n > 0)
            {
                cancel.ThrowIfCancellationRequested();
                result *= n;
                n--;
            }
            return result;
        }
    }
}
