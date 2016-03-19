using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace UIv2
{
    public partial class Form1 : Form
    {
        public int count = 0;
        Stopwatch stopwatch = Stopwatch.StartNew();

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if (progressBar1.Value == progressBar1.Maximum)
            {
                tabControl1.SelectTab(1); 
                timer1.Start(); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
           
            
            progressBar2.Maximum = 100;
            progressBar2.Step = 1;
            progressBar2.Value = 0;
           // Thread thread1 = new Thread(Thread2.RunPro);
            //thread1.Start();
           // Console.WriteLine("In main.");
            tabControl1.SelectTab(1);
            timer2.Start();
           // timer1.Stop(); 
           // Thread.Sleep(10000);
            

           /* while (true)
            {
                //some other processing to do possible
                if (stopwatch.ElapsedMilliseconds >= 5000)
                {
                  //  System.Console.WriteLine(stopwatch.ElapsedMilliseconds); 
                    System.Console.WriteLine("Hello World");
                    break;
                   
                }
            }
             */
              
            //Thread.Sleep(5000);
           
          //  Process.Start(@"C:\Users\tlewis\Desktop\KinectHandTracking\KinectHandTracking\bin\Debug\KinectHandTracking.exe");


           // ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.FileName = "KinectHandTracking.EXE"; //"DEVENV.EXE";
            //startInfo.Arguments = @"C:\Users\tlewis\Desktop\KinectHandTracking\KinectHandTracking\bin\Debug\KinectHandTracking.exe";
            //System.Diagnostics.Process.Start("C:\\Users\tlewis\Desktop\KinectHandTracking\KinectHandTracking"); 
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {
           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

          // progressBar2.Value = (int) stopwatch.ElapsedMilliseconds; 
            progressBar2.Increment(1);
          //  timer2.Start();
          //  Thread thread1 = new Thread(Thread2.RunPro);
            //thread1.Start();
           // Console.WriteLine("In main.");

             if ((progressBar2.Value == progressBar2.Maximum) && (count==0))
            {
                Process.Start(@"C:\Users\tlewis\Desktop\KinectHandTracking\KinectHandTracking\bin\Debug\KinectHandTracking.exe");
                count = 1; 
            }
        }
    }

    public class Thread2
    {
        public static void RunPro()
        {
            Thread.Sleep(15000);
            Console.WriteLine("Working thread...");
           
            
        }
    }
}

