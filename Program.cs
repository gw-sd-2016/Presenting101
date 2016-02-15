using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//IPC code found on http://v01ver-howto.blogspot.co.uk/2010/04/howto-use-named-pipes-to-communicate.html

namespace System.IO.Pipes
{
    class Hello
    {
        static void Main()
        {
            string echo = "";    
            while (true)
            {
                //Create pipe instance
                NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, 4);
                Console.WriteLine("[ECHO DAEMON] NamedPipeServerStream thread created.");

                //wait for connection
                Console.WriteLine("[ECHO DAEMON] Wait for a client to connect");
                pipeServer.WaitForConnection();

                Console.WriteLine("[ECHO DAEMON] Client connected.");
                try
                {
                    // Stream for the request. 
                    StreamReader sr = new StreamReader(pipeServer);
                    // Stream for the response. 
                    StreamWriter sw = new StreamWriter(pipeServer);
                    sw.AutoFlush = true;

                    // Read request from the stream. 
                     echo = sr.ReadLine();

                    Console.WriteLine("[ECHO DAEMON] Request message: " + echo);

                    // Write response to the stream.
                    sw.WriteLine("[ECHO]: " + echo);

                    pipeServer.Disconnect();
                }
                catch (IOException e)
                {
                    Console.WriteLine("[ECHO DAEMON]ERROR: {0}", e.Message);
                }
             
                System.IO.File.WriteAllText(@"C:\Users\tlewis\Desktop\WriteLines.txt", echo);
            
                pipeServer.Close();
            } 
        }
    }
}
