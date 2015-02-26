using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebScraper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Sites being scraped
        private string[] SITES =
        {
            "http://google.com/",
            "http://www.utah.edu/",
            "http://www.cs.utah.edu/"
        };

        // Regex used to recognize tags (not foolproof!)
        private Regex TAG_REGEX = new Regex("<([a-zA-Z0-9]+)");

        /// <summary>
        /// Run when the Scape button is clicked
        /// </summary>
        private void scrapeButtonClick(object sender, EventArgs e)
        {
            // Reset some stuff
            tags = new SortedDictionary<string, int>();
            scrapeButton.Enabled = false;
            charCountBox.Text = "";
            tagBox.Text = "";

            // Fire off separate tasks to scrape the various sites
            List<Task<int>> tasks = new List<Task<int>>();
            foreach (string url in SITES)
            {
                tasks.Add(Task.Run(() => scrapeTags(url)));
            }

            // Create a task to wait for the tasks to be completed and
            // then finish up.
            Task.Run(() => finishScrapeButtonClick(tasks));
        }

        private void finishScrapeButtonClick (List<Task<int>> tasks)
        {
            // Wait for all the tasks to finish
            Task.WaitAll(tasks.ToArray());

            // Add up and display the lengths
            int sum = 0;
            foreach (Task<int> t in tasks)
            {
                sum += t.Result;
            }

            // Updating a UI element must be done on the GUI event
            // thread.  Thus, we have to use the Invoke method,
            // which exists for this purpose.
            charCountBox.Invoke((Action) (() => charCountBox.Text = sum.ToString()));

            // Display the tag counts and re-enable scrape button
            charCountBox.Invoke((Action) displayTagCounts);
            scrapeButton.Invoke((Action) (() => scrapeButton.Enabled = true));
        }

        /// <summary>
        /// Returns the number of characters in the referenced web page,
        /// and updates tag counts in the shared tags dictionary
        /// </summary>
        private int scrapeTags(string url)
        {
            // Get the web page in a synchronous fashion
            HttpClient client = new HttpClient();
            String result = client.GetStringAsync(url).Result;

            // Count all the different kinds of tags on the page
            foreach (Match m in TAG_REGEX.Matches(result))
            {
                updateTagCounts(m.Groups[1].ToString());
                //Task.Delay(1).Wait();
            }

            // Return the length of the page
            return result.Length;
        }

        // Maps tags to the number of times they occur.
        // Not that this is shared by all the scraping tasks.
        private SortedDictionary<string, int> tags;

        // Used for synchronization
        private readonly object sync = new object();

        /// <summary>
        /// Updates the count for the specified tag
        /// </summary>
        private void updateTagCounts(String tag)
        {
            lock (sync)
            {
                int count;
                if (tags.TryGetValue(tag, out count))
                {
                    tags[tag] = count + 1;
                }
                else
                {
                    tags[tag] = 1;
                }
            }
        }

        /// <summary>
        /// Displays the counts for all the tags
        /// </summary>
        private void displayTagCounts()
        {
            foreach (string tag in tags.Keys)
            {
                tagBox.AppendText(tag + " " + tags[tag] + "\r\n");
            }
        }
    }
}
