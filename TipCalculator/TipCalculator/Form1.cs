using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double amt, tipPct;
            if (Double.TryParse(amount.Text, out amt))
            {
                if (Double.TryParse(tip.Text, out tipPct))
                {
                    total.Text = (amt + amt * tipPct / 100).ToString();
                }
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void saveToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
