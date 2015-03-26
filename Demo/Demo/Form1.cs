using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            String s = await f();
            MessageBox.Show(s);
        }

        private async Task<string> f ()
        {
            Task t = Task.Run(() => { Thread.Sleep(2000); });
            await t;
            return "Hello";
        }
    }
}
