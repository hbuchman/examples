using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace AsyncMethods2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void GetWebPageClickAsync(object sender, EventArgs e)
        {
            // GetWebPageAsync returns a Task<string>. That means that when you await the 
            // task you'll get a string (urlContents).
            Task<string> getStringTask = GetWebPageAsync(textBox1.Text);

            // The await operator suspends GetWebPageClickAsync.  
            string urlContents = await getStringTask;

            // Finish up
            textBox2.Text = urlContents;
        }

        private async Task<string> GetWebPageAsync (string url)
        {
            // You need to add a reference to System.Net.Http to declare client.
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the 
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync(url);

            // The await operator suspends GetWebPageAsync. 
            //  - GetWebPageAsync can't continue until getStringTask is complete. 
            //  - Meanwhile, control returns to the caller of GetWebPageAsync. 
            //  - Control resumes here when getStringTask is complete.  
            //  - The await operator then retrieves the string result from getStringTask. 
            string urlContents = await getStringTask;
            return urlContents;
        }
    }
}
