using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UnityTCPClient : MonoBehaviour
{
    // BIG LIST OF TODO:
    // 1. Make TPC Client work as the main "connector"
    // 2. Networked Objects Framework! (Each Object should have the ability to be owned/synchronized
    // 3. More Elegant Way to handling and sending data into packets and shit like that
    // 4. Program the actual server
    public Transform target;
    TCP client;
    // Start is called before the first frame update
    void Start()
    {
        client = new TCP(0);
        client.Connect(new System.Net.Sockets.TcpClient());
    }

    // Update is called once per frame
    void Update()
    {
        if (client != null)
        {
            string msg = "PLAYER POSITION: " + target.transform.position.ToString();
            client.Serialize(msg);
        }
    }

    /**
    *   Class that is meant to handle asynchronous networking TCP Socket bullshit
    *   by @Gwarf
    *
    */
    class TCP
    {
        private TcpClient socket;
        private int networkID; // network id for the whole ass client
        private NetworkStream stream;

        int readWriteSize = 512;
        private byte[] writeBuffer;
        private byte[] readBuffer;
        string IP = "127.0.0.1";
        int PORT = 65432;

        public TCP(int id)
        {
            writeBuffer = new byte[readWriteSize];
            readBuffer = new byte[readWriteSize];

            networkID = id;
        }

        /**
        * Connect socket client to server
        *
        */
        public void Connect(TcpClient client)
        {
            socket = client;
            socket.BeginConnect(IPAddress.Parse("127.0.0.1"), 65432, new AsyncCallback(ConnectCallback), socket);
        }

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            socket.EndConnect(asyncResult);

            if (!socket.Connected) { return; }

            stream = socket.GetStream();

            byte[] data = System.Text.Encoding.ASCII.GetBytes("WELCOME");
            stream.BeginWrite(data, 0, data.Length, new AsyncCallback(SendCallback), socket);
            //stream.BeginRead(readBuffer, 0, readWriteSize, new AsyncCallback(ReceiveCallback), socket);
        }

        //Test serialization
        public void Serialize(string message)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.BeginWrite(data, 0, data.Length, new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                stream.EndWrite(asyncResult);
                Debug.Log("Sent some data!");
            }
            catch (Exception e)
            {
                Debug.Log("SEND FAILED: " + e);
            }
        }
        /**
         * "It receives the data from the network device and builds a message string.
         * It reads one or more bytes of data from the network into the data buffer and then calls the BeginReceive method again until the data sent by the client is complete.
         * Once all the data is read from the client, ReceiveCallback signals the application thread that the data is complete by setting the ManualResetEvent sendDone"
         */
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                int bytesRead = stream.EndRead(asyncResult);

                if (bytesRead > 0)
                {
                    // Data is still coming in
                    builder.Append(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
                    Debug.Log(builder.ToString());

                    stream.BeginRead(readBuffer, 0, readWriteSize, new AsyncCallback(ReceiveCallback), socket);
                }
                else
                {
                    if (builder.Length > 1)
                    {
                        Debug.Log(builder.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("SEND FAILED: " + e);
            }
        }
    }
}
