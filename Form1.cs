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
using System.Collections;


namespace UIv2
{
    public partial class Form1 : Form
    {
        public int count = 0;
        Stopwatch stopwatch = Stopwatch.StartNew();

        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Add("             Hello and thank you for using P101 below are the directions:");
            listBox1.Items.Add(""); 
            listBox1.Items.Add("The system looks at 6 categories - volume, rhythm, disfluencies, hand gestures, posture");
            listBox1.Items.Add("and if you are pacing. You have the option to have the system to detect \"bad words\" by");
            listBox1.Items.Add("adding it to the field below and clicking add. Otherwise you can use the system defaults");
            listBox1.Items.Add("which detect \"ahh\" \"umm\" and \" basically\".");
            listBox1.Items.Add("To being press the start button and you will have 15 seconds to get into place");
            listBox1.Items.Add("Once the system starts you may begin presenting as you would in front of an audience");
            listBox1.Items.Add("When you are done just select the done button and you will get a report detailing");
            listBox1.Items.Add("what went well, what didn't and our suggestions");
            listBox1.Items.Add("");
            listBox1.Items.Add("***Please note: we recommend that you stand roughly 5 feet away from the system. ");
            listBox1.Items.Add("The systems also will not begin until it see your hands and the rest of your body"); 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(sender);
           
 
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
            /*
            progressBar1.Increment(1);
            if (progressBar1.Value == progressBar1.Maximum)
            {
                tabControl1.SelectTab(1); 
                timer1.Start(); 
            }*/ 
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

                //populate uer
                Thread.Sleep(3000);
                tabControl1.SelectTab(2);
                //listBox2.Items.Add("test");

                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\tlewis\Desktop\badwords.txt");
                listBox2.Items.Add("You said a total of 4 bad words");
                listBox2.Items.Add("You said umm 1 times and ahh 2 times and");
                listBox2.Items.Add("and \"test\" 1 times ");
                listBox2.Items.Add("");
                listBox2.Items.Add("We suggest going over what you will say");
                listBox2.Items.Add("next time");

                listBox3.Items.Add("Average Volume: 64 decebels");
                listBox3.Items.Add("You we a bit to quiet");
                listBox3.Items.Add("");
                listBox3.Items.Add("We suggest you speak up and");
                listBox3.Items.Add("practice talking loudly for");

                listBox4.Items.Add("You had 2 gaps while speaking");
                listBox4.Items.Add("You had a good flow");
                listBox4.Items.Add("");
                listBox4.Items.Add("We suggest you slow down when");
                listBox4.Items.Add("speaking");

                listBox5.Items.Add("Your hands went out of the box");
                listBox5.Items.Add("3 times");
                listBox5.Items.Add("");
                listBox5.Items.Add("Over all this is good. We suggest");
                listBox5.Items.Add("you watch where your hands are when");
                listBox5.Items.Add("presenting");

                listBox6.Items.Add("You leaned 0 times");
                listBox6.Items.Add("You had great posture");
                listBox6.Items.Add("");
                listBox6.Items.Add("We suggest you keep doing");
                listBox6.Items.Add("what you're doing");

                listBox7.Items.Add("You walked around 0 times");
                listBox7.Items.Add("You did not move");
                listBox7.Items.Add("");
                listBox7.Items.Add("We suggest you keep doing");
                listBox7.Items.Add("what you're doing");

                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    //listBox2.Items.Add(line);
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            

            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); 
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

