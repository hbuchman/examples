using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using Chat;
using System.Threading.Tasks;
using System.Text;

namespace ChatServerTester
{
    [TestClass]
    public class ChatTester
    {
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

        [TestMethod]
        public void TestMethod1()
        {
            SimpleChatServer server = new SimpleChatServer(4000);
            SimpleTest("Test 1\r\nHello world\r\n");


            Task[] tasks = new Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                String testString = "Test " + i + "\r\n";
                tasks[i] = Task.Run(() => SimpleTest(testString));
            }
            Task.WaitAll(tasks);

        }

        private void SimpleTest(string testString)
        {
            // Open a socket to the server
            TcpClient client = new TcpClient("localhost", 4000);
            Socket socket = client.Client;

            // This is the string we expect to get back
            String expectedString = "Welcome!\r\n" + testString.ToUpper();

            // Convert the string into an array of bytes and send them all out.
            byte[] outgoingBuffer = encoding.GetBytes(testString);
            int index = 0;
            int size = outgoingBuffer.Length;
            int byteCount;
            while ((byteCount = socket.Send(outgoingBuffer, index, size, 0)) != size)
            {
                index += byteCount;
                size -= byteCount;
            }

            // Read bytes from the socket until we have the number we expect
            byte[] incomingBuffer = encoding.GetBytes(expectedString);
            Array.Clear(incomingBuffer, 0, incomingBuffer.Length);
            index = 0;
            size = incomingBuffer.Length;
            while ((byteCount = socket.Receive(incomingBuffer, index, size, 0)) != size)
            {
                index += byteCount;
                size -= byteCount;
            }

            // Convert the buffer into a string and make sure it is what was expected
            String result = encoding.GetString(incomingBuffer);
            Assert.AreEqual(expectedString, result);
        }
    }
}
