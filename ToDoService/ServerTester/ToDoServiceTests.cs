using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ToDoList
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        // Location of IIS Express
        private const string IIS_EXECUTABLE = @"C:\Program Files (x86)\IIS Express\iisexpress.exe";

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }

    [TestClass]
    public class ToDoServiceTests
    {
        /// <summary>
        /// Creates a generic client for communicating with the ToDoList service.
        /// Your port number may differ and will need to be changed.
        /// </summary>
        public static HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:32513/");
            return client;
        }

        /// <summary>
        /// Returns the result of getting all items
        /// </summary>
        public static async Task<List<ToDoItem>> GetAllItems(string owner, bool completed)
        {
            using (HttpClient client = CreateClient())
            {
                String url = String.Format("/ToDoService.svc/GetAllItems?completed={0}&user={1}", completed, Uri.EscapeDataString(owner));
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ToDoItem>>(result);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the result (a uid) of adding an item
        /// </summary>
        public static async Task<string> AddItem(ToDoItem item)
        {
            using (HttpClient client = CreateClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/ToDoService.svc/AddItem", content);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<string>(result);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Deletes the specified item.  Reports whether the attempt succeeded.
        /// </summary>
        public static async Task<bool> DeleteItem(string uid)
        {
            using (HttpClient client = CreateClient())
            {
                String url = String.Format("/ToDoService.svc/DeleteItem/{0}", uid);
                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Marks the specified item as completed.  Reports whether the attempt succeeded.
        /// </summary>
        public static async Task<bool> MarkCompleted(string uid)
        {
            using (HttpClient client = CreateClient())
            {
                String url = String.Format("/ToDoService.svc/MarkCompleted/{0}", uid);
                HttpResponseMessage response = await client.PutAsync(url, new StringContent(""));
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            IISAgent.Start("/site:\"ToDoService\" /apppool:\"Clr4IntegratedAppPool\"");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            IISAgent.Stop();
        }

        // NOTE: The two tests cannot be run simultaneously, since they are accessing
        // the same server.
        [TestMethod]
        public void SimpleTest()
        {
            ToDoItem item = new ToDoItem() { Owner = "Joe", Description = "reason", Completed = true };
            string uid = AddItem(item).Result;
            Assert.AreEqual("0", uid);
            Assert.IsTrue(DeleteItem("0").Result);
            uid = AddItem(item).Result;
            Assert.AreEqual("1", uid);
            List<ToDoItem> result = GetAllItems("Joe", true).Result;
            Assert.AreEqual(1, result.Count);
            item.Owner = "Ben";
            AddItem(item).Wait();
            result = GetAllItems("Joe", true).Result;
            Assert.AreEqual(1, result.Count);
            result = GetAllItems("Joe", false).Result;
            Assert.AreEqual(0, result.Count);
            result = GetAllItems("", true).Result;
            Assert.AreEqual(2, result.Count);
        }

        // Number of owners to use in ComplexTest
        private const int OWNERS = 20;

        // Number of ToDoItems (half completed) to use per
        // owner in ComplexTest
        private const int GOAL = 20;

        [TestMethod]
        public void ComplexTest()
        {
            Task[] tasks = new Task[OWNERS];
            for (int i = 0; i < OWNERS; i++)
            {
                string owner = i.ToString();
                tasks[i] = Task.Run(() => DoOperations(owner));
            }
            Task.WaitAll(tasks);
            List<ToDoItem> result = GetAllItems("", true).Result;
            Assert.AreEqual(OWNERS * GOAL, result.Count);
            result = GetAllItems("", false).Result;
            Assert.AreEqual(OWNERS * GOAL / 2, result.Count);
        }

        private void DoOperations(string owner)
        {
            string[] uids = new string[GOAL];
            for (int i = 0; i < GOAL; i++)
            {
                uids[i] = AddItem(new ToDoItem() { Owner = owner, Completed = false }).Result;
            }
            for (int i = 0; i < GOAL; i += 2)
            {
                DeleteItem(uids[i]).Wait();
            }
            for (int i = 0; i < GOAL; i += 2)
            {
                uids[i] = AddItem(new ToDoItem() { Owner = owner, Completed = false }).Result;
            }
            for (int i = 0; i < GOAL; i += 2)
            {
                MarkCompleted(uids[i]).Wait();
            }
            List<ToDoItem> result = GetAllItems(owner, true).Result;
            Assert.AreEqual(GOAL, result.Count);
            result = GetAllItems(owner, false).Result;
            Assert.AreEqual(GOAL/2, result.Count);
        }
    }
}
