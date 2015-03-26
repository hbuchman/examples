using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    public class SimpleChatServer
    {
        /// <summary>
        /// Launches a SimpleChatServer on port 4000.  Keeps the main
        /// thread active so we can send output to the console.
        /// </summary>
        static void Main(string[] args)
        {
            new SimpleChatServer(4000);
            Console.ReadLine();
        }


        // Listens for incoming connection requests
        private TcpListener server;


        /// <summary>
        /// Creates a SimpleChatServer that listens for connection requests on port 4000.
        /// </summary>
        public SimpleChatServer(int port)
        {
            // A TcpListener listens for incoming connection requests
            server = new TcpListener(IPAddress.Any, port);

            // Start the TcpListener
            server.Start();

            // Ask the server to call ConnectionRequested at some point in the future when 
            // a connection request arrives.  It could be a very long time until this happens.
            // The waiting and the calling will happen on another thread.  BeginAcceptSocket 
            // returns immediately, and the constructor returns to Main.
            server.BeginAcceptSocket(ConnectionRequested, null);
        }

        /// <summary>
        /// This is the callback method that is passed to BeginAcceptSocket.  It is called
        /// when a connection request has arrived at the server.
        /// </summary>
        public void ConnectionRequested(IAsyncResult result)
        {
            // We obtain the socket corresonding to the connection request.  Notice that we
            // are passing back the IAsyncResult object.
            Socket s = server.EndAcceptSocket(result);

            // We ask the server to listen for another connection request.  As before, this
            // will happen on another thread.
            server.BeginAcceptSocket(ConnectionRequested, null);

            // We create a new ClientConnection, which will take care of communicating with
            // the remote client.
            new ClientConnection(s);
        }
    }


    /// <summary>
    /// Represents a connection with a remote client.  Takes care of receiving and sending
    /// information to that client according to the protocol.
    /// </summary>
    class ClientConnection
    {
        // The socket through which we communicate with the remote client
        private Socket socket;

        // Text that has been received from the client but not yet dealt with
        private String incoming;

        // Text that needs to be sent to the client but has not yet gone
        private String outgoing;

        // Records whether an asynchronous send attempt is ongoing
        private bool sendIsOngoing = false;

        // For synchronizing sends
        private readonly object sendSync = new object();

        // Encoding used for incoming/outgoing data
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

        /// <summary>
        /// Creates a ClientConnection from the socket, then begins communicating with it.
        /// </summary>
        public ClientConnection(Socket s)
        {
            // Record the socket and clear incoming
            socket = s;
            incoming = "";
            outgoing = "";

            // Send a welcome message to the remote client
            SendMessage("Welcome!\r\n");

            // Ask the socket to call MessageReceive as soon as up to 1024 bytes arrive.
            byte[] buffer = new byte[1024];
            socket.BeginReceive(buffer, 0, buffer.Length,
                                SocketFlags.None, MessageReceived, buffer);
        }


        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        private void MessageReceived(IAsyncResult result)
        {
            // Get the buffer to which the data was written.
            byte[] buffer = (byte[])(result.AsyncState);

            // Figure out how many bytes have come in
            int bytes = socket.EndReceive(result);

            // If no bytes were received, it means the client closed its side of the socket.
            // Report that to the console and close our socket.
            if (bytes == 0)
            {
                Console.WriteLine("Socket closed");
                socket.Close();
            }

            // Otherwise, decode and display the incoming bytes.  Then request more bytes.
            else
            {
                // Convert the bytes into a string
                incoming += encoding.GetString(buffer, 0, bytes);
                Console.WriteLine(incoming);

                // Echo any complete lines, converted to upper case
                int index;
                while ((index = incoming.IndexOf('\n')) >= 0)
                {
                    String line = incoming.Substring(0, index);
                    if (line.EndsWith("\r"))
                    {
                        line = line.Substring(0, index - 1);
                    }
                    SendMessage(line.ToUpper() + "\r\n");
                    incoming = incoming.Substring(index + 1);
                }

                // Ask for some more data
                socket.BeginReceive(buffer, 0, buffer.Length,
                    SocketFlags.None, MessageReceived, buffer);
            }
        }

        /// <summary>
        /// Sends a string to the client
        /// </summary>
        private void SendMessage(String message)
        {
            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // Append the message to the unsent string
                outgoing += message;

                // If there's not a send ongoing, start one.
                if (!sendIsOngoing)
                {
                    sendIsOngoing = true;
                    SendBytes();
                }
            }
        }


        /// <summary>
        /// Attempts to send the entire outgoing string.
        /// This method should not be called unless sendSync has been acquired.
        /// </summary>
        private void SendBytes()
        {
            if (outgoing == "")
            {
                sendIsOngoing = false;
            }
            else
            {
                byte[] outgoingBuffer = encoding.GetBytes(outgoing);
                outgoing = "";
                socket.BeginSend(outgoingBuffer, 0, outgoingBuffer.Length,
                                 SocketFlags.None, MessageSent, outgoingBuffer);
            }
        }


        /// <summary>
        /// Called when a message has been successfully sent
        /// </summary>
        private void MessageSent(IAsyncResult result)
        {
            // Find out how many bytes were actually sent
            int bytes = socket.EndSend(result);

            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // Get the bytes that we attempted to send
                byte[] outgoingBuffer = (byte[])result.AsyncState;
                
                // The socket has been closed
                if (bytes == 0)
                {
                    socket.Close();
                    Console.WriteLine("Socket closed");
                }

                // Prepend the unsent bytes and try sending again.
                else
                {
                    outgoing = encoding.GetString(outgoingBuffer, bytes, 
                                                  outgoingBuffer.Length - bytes) + outgoing;
                    SendBytes();
                }
            }

        }
    }

}

