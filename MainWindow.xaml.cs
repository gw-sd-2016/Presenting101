using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Collections;
using System.Threading;

namespace KinectHandTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        float pL;
        private static System.Timers.Timer aTimer;
        Stopwatch stopWatch = new Stopwatch();
        Boolean noMove;
        int second = 0;
        String nMove = "";
        int rightLean = 0;
        int leftLean = 0;
        int rigtHandUP = 0;
        int rightHandRight = 0; 
        int leftHandUP = 0;
        int leftHandLeft = 0;
        int paceUER = 0;
        int flag = 0;
        String print = "";
        String p2 = "";
        ArrayList al = new ArrayList();
        double staticPace;
        string pace = "";
        int lright=0;
        int lleft = 0;
        int hgleft = 0;
        int hgright = 0;
        int pflag = 0; 
        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }
        void timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            second = second + 1;
            
            if (second > 10) 
            {
                pace = "woorksss"; 
                noMove = true;
                //aTimer.Enabled = false;

            }
            
        }
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                // Find the joints
                                Joint handRight = body.Joints[JointType.HandRight];
                                Joint thumbRight = body.Joints[JointType.ThumbRight];

                                Joint handLeft = body.Joints[JointType.HandLeft];
                                Joint thumbLeft = body.Joints[JointType.ThumbLeft];

                                Joint leftShoulder = body.Joints[JointType.ShoulderRight];
                                Joint rightShoulder = body.Joints[JointType.ShoulderLeft];

                                Joint pacer = body.Joints[JointType.SpineMid];

                                Joint leftHip = body.Joints[JointType.HipLeft];
                                Joint rightHip = body.Joints[JointType.HipRight];


                                // Draw hands and thumbs
                                canvas.DrawHand(handRight, _sensor.CoordinateMapper);
                                canvas.DrawHand(handLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbLeft, _sensor.CoordinateMapper);
                              

                                // Find the hand states
                                string rightHandState = "";
                                string leftHandState = "";
                                string leftshoulder = "";
                                string rightshoulder = "";
                                string upright = "-";
                                string paceListener = "-";
                             //   string pace = "-";
                                string rflg = "##";
                                string lflg = "##";
                                string status = ""; 
                                string status2 ="";
                                string leanStatusR = "";
                                string leanStatusL = "";
                                string ptr;
                                
                              //  float pL = 0;
                                float test = 0; 
                                float ls = 0;
                                float rs = 0;
                                float ur = 0;
                                float lsx = 0; 

                                float nleftHip = 0;
                                float nrightHip = 0;
                                float testHandrighty = 0;
                                float testHandlefty = 0;
                                float testHandrightx = 0;
                                float testHandleftx = 0; 

                                //const float tpL ; 
                                 
                                int cnt = 0;
                                float fin01 = 0;
                                float fin02 = 0;
                                float pt = .45000f;
                                float pt2 = .1000f;
                                double tollerance = 0.020000; //TODO need to figure out a good tollerance
                                double paceTollerance = .1000;//0.100000; //Pacing tollerance for horizontal movement
                                double pToll = 0.3000; //TODO play with these tollerances for better readings 
                               
          
				                //hand lasson counter
                                
                               
                              

                                /*---------------------------------------------Relational movement tracking------------------------------------*/
                             
                                /* this is to fix the issue with pacing where there is a constantly changing value */
                                

                                pL  = pacer.Position.X;
                                System.Timers.Timer aTimer = new System.Timers.Timer();
                                pace = "not pace";

                                //gets your starting position
                                if (flag != 1)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        pL = pacer.Position.X;
                                        al.Add(pL);
                                        staticPace = Convert.ToDouble(al[0]); 
                                        flag = 1;
                                        pace = "";
                                    }
                                }
                                ptr = al[0].ToString();


                                //tracks pacing 


                                if ((Math.Abs(pL) - (Math.Abs(staticPace)) > .3000))//|| (Math.Abs((staticPace + pL)) > pToll)) //case when position is 0 this does not handle it
                               {
                              /*  if(Math.Abs(staticPace)<1) 
                                {
                                    staticPace = staticPace + .1; 
                                }
                                if((staticPace<pL)&& (pL - staticPace > .3000))
                                {

                                } */
                                   // aTimer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                   /* aTimer = new System.Timers.Timer(1000);
                                    aTimer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                    aTimer.Interval = 1000;
                                    aTimer.Enabled = true; */
                                    //stopWatch.Start();

                                   // aTimer.Interval = 1000;
                                    test = pacer.Position.X;
                                    //nMove = "Pacing++"; 
                                    pace = "pacing*";
                                   // paceUER++; 
                                  //  aTimer.Enabled = true;
                                    
                                }

                                if (pace.Equals("pacing*")) //see what the pacing int value would be b4 updating position
                                {
                                    stopWatch.Start();
                                   // pace = stopWatch.Elapsed.Seconds.ToString(); 
                                    if ((stopWatch.Elapsed.Seconds) > 10)
                                    {
                                        //int keeping track of how many times you moved around?
                                        al.Clear();
                                        flag = 0; 
                                        pace = "not pacing";
                                        stopWatch.Stop();
                                        stopWatch.Reset();
                                        pflag = 0;
                                        if (paceUER != 0)
                                        {
                                            paceUER = paceUER - 1; 
                                        }
                                    }
                                    else if ((stopWatch.Elapsed.Seconds < 10) && pflag ==0)
                                    {
                                        paceUER++;
                                        pflag = 1; 
                                    }
                                    
                                }
                                
                                if (pace.Equals("not pace") && pflag == 1)
                                {
                                    pflag = 0;
                                  
                                }
                                //how long do they stay in that one place?

                                /*-----------------------------------------------Lean tracking--------------------------------------------------*/
                                if ((leftShoulder.Position.Y + tollerance ) >= rightShoulder.Position.Y && (leftShoulder.Position.Y - tollerance) <= rightShoulder.Position.Y) 
                                {
                                    //No Direction
                                    upright = "yep";
                                    ur = leftShoulder.Position.Y + rightShoulder.Position.Y;
                                    lleft = 0;
                                    lright = 0; 
                                }
                                else if (leftShoulder.Position.Y < rightShoulder.Position.Y)
                                {
                                    //leaning left
                                    leanStatusL = "Stand up straight - leaning right!";
                                    leftshoulder = "leaning right..";
                                  
                                    rs = rightShoulder.Position.Y;
                                    if (leanStatusL.Equals("Stand up straight - leaning right!") && lright == 0)
                                    {
                                        rightLean++;
                                        lright = 1; //flag to keep rightlean value incrementing by 1 
                                        lleft = 0; 
                                    }
                                    
                                 
                                }
                                else if (leftShoulder.Position.Y > rightShoulder.Position.Y)
                                {
                                    //leaning right
                                    leanStatusR = "Stand up straight - leaning left!"; 
                                    rightshoulder = "leaning left..";
                                    if (leanStatusR.Equals("Stand up straight - leaning left!") && lleft == 0)
                                    {
                                        leftLean++;
                                        lleft = 1; //flag to keep rightlean value incrementing by 1
                                        lright = 0; 
                                    }
                 
                                    ls = leftShoulder.Position.Y;
                                    //leftLean++;
                                }

                                rs = rightShoulder.Position.X;  
                                ls = leftShoulder.Position.Y;
                                lsx = leftShoulder.Position.X; 
                                nleftHip = leftHip.Position.X;
                                nrightHip = rightHip.Position.X; 
                                testHandlefty = handLeft.Position.Y;
                                testHandrighty = handRight.Position.Y;
                                testHandleftx = handLeft.Position.X;
                                testHandrightx = handRight.Position.X; 

                               /*-------------------------------Hand Gesture tracking--------------------------------------*/
                                    /*N.T.S: when tracking the hips and shoulders they are different x values and a tolerance should 
                                     * be used to normalize the difference (if using both values prob will just use hips since more
                                     * accurate with hand coordinates)*/
                              //  testHandright = rightShoulder.Position.X; 

                                //NTS these values will have to be within rather than simply less than or greater than
                                /*
                                if(testHandright <= nrightHip)
                                {
                                    //then right hand is within "the box"
                                }
                                if(testHandleft >= nleftHip)
                                {
                                    //then left hand is within "the box"
                                }
                                if (testHandright <= ls)
                                {
                                    //then right hand is within "the box" i.e below shoulders
                                } 
                                if (testHandleft <= rs)
                                {
                                    //then left hand is within "the box" below shoulders -> TODO need to double check values
                                }
                                if(testHandright <= nrightHip)
                                {
                                    //then right hand is within the box above the hip
                                }
                                if(testHandleft <= nleftHip)
                                {
                                    //then left hand is within the box i.e. above the hip
                                }
                                //-------------------------- */

                                fin01 = rs + pt; //this creates the tollerance for horizontal movment of hand to make sure within the box 
                                if ((testHandrighty <= ls)&&(testHandrightx <= fin01))//&&(testHandright<=lsx)//&&(testHandright <= nrightHip))
                                {
                                    //right hand is to the left of the shoulder and below the shoulder
                                    //right hand i.e below shoulders above hip    testHandrightx <= fin //right hand is to left of rightshoulder
                                    status = ""; 
                                    rflg = "Right in";
                                   
                                    hgright = 0;

                                }
                                else
                                {
                                    status = "Watch your right hand";
                                    rflg = "right out";

                                    if (status.Equals("Watch your right hand") && hgright == 0)
                                    {
                                        rightHandRight++; 
                                        hgright = 1; //flag to keep rightlean value incrementing by 1 
                                        hgleft = 0;
                                    }
                                    
                                }
                                fin02 = ls - .1000f; 
                                if ((testHandlefty <= rs) && (testHandleftx >= fin02))
                                {
                                    //right hand i.e below shoulders above hip
                                    lflg = "Left in";
                                    status2 = "";
                                    hgleft = 0;
                                }
                                else
                                {
                                     lflg = "left out";
                                     status2 = "Watch your left hand";

                                     if (status2.Equals("Watch your left hand") && hgleft == 0)
                                     {
                                         leftHandLeft++;
                                         hgleft = 1; //flag to keep rightlean value incrementing by 1 
                                         hgright = 0;
                                     }
                                    
                                }



                                /*
                               if(testHandright <= nrightHip)
                               {
                                   //then right hand is within "the box"
                               }
                               if(testHandleft >= nleftHip)
                               {
                                   //then left hand is within "the box"
                               }
                               if (testHandright <= ls)
                               {
                                   //then right hand is within "the box" i.e below shoulders
                               } 
                               if (testHandleft <= rs)
                               {
                                   //then left hand is within "the box" below shoulders -> TODO need to double check values
                               }
                               if(testHandright <= nrightHip)
                               {
                                   //then right hand is within the box above the hip
                               }
                               if(testHandleft <= nleftHip)
                               {
                                   //then left hand is within the box i.e. above the hip
                               }
                               */

                                /*-----------------------------------------Hand tracking------------------------------------*/
                                switch (body.HandRightState)
                                {
                                    case HandState.Open:
                                        rightHandState = "Open";
                                        break;
                                    case HandState.Closed:
                                        rightHandState = "Closed";
                                        break;
                                    case HandState.Lasso:
                                        rightHandState = "Lasso";
					                  //  cnt++;
                                        break;
                                    case HandState.Unknown:
                                        rightHandState = "Unknown...";
                                        break;
                                    case HandState.NotTracked:
                                        rightHandState = "Not tracked";
                                        break;
                                    default:
                                        break;
                                }

                                switch (body.HandLeftState)
                                {
                                    case HandState.Open:
                                        leftHandState = "Open";
                                        break;
                                    case HandState.Closed:
                                        leftHandState = "Closed";
                                        break;
                                    case HandState.Lasso:
                                        leftHandState = "Lasso";
					                    cnt++; 
                                        break;
                                    case HandState.Unknown:
                                        leftHandState = "Unknown...";
                                        break;
                                    case HandState.NotTracked:
                                        leftHandState = "Not tracked";
                                        break;
                                    default:
                                        break;
                                }
                               // reCount.Text = cnt.ToString();
                               // tblRightHandState.Text = rightHandState;
                                //tblLeftHandState.Text = leftHandState;
                                
                                
                               // tbleftShoulder.Text = leftshoulder;
                              
                                //tbrightShoulder.Text = rightshoulder;


                               // tbupright.Text = upright;
                              //---  tbpl.Text = pL.ToString();

                                
                                tbtest.Text = ptr; //  pace; //nMove; // pace; //pace testing
                               tbtest2.Text = pace;// test.ToString();  //pace testing 
                                tbtest3.Text = pL.ToString();  //this is for pacing testing 
                                //tbtest.Text = test.ToString();
                             //  tblh.Text = nleftHip.ToString();
                               //tbrh.Text = nrightHip.ToString();
                               // tbhandl.Text = testHandleft.ToString();
                                //tbhandr.Text = testHandright.ToString(); 
                                //tbleftShoulder.Text = rs.ToString();
                               // tbrightShoulder.Text = ls.ToString();
                               // tbupright.Text = ur.ToString(); 

                                if (leftshoulder.Equals("leaning right.."))
                                {
                                    tbleftShoulder.Text = leanStatusL; 
                                }
                                else if (rightshoulder.Equals("leaning left.."))
                                {
                                    tbleftShoulder.Text = leanStatusR; 
                                }
                                else
                                {
                                    tbleftShoulder.Text = ""; 
                                }

                                if((rflg.Equals("right out"))){
                                    tbrflag.Text = status; 
                                }
                                else if (lflg.Equals("left out")){
                                    tbrflag.Text = status2;
                                }
                                else
                                {
                                    tbrflag.Text = ""; 
                                }

                                // tbrflag.Text = rflg;// testHandleftx.ToString(); // rflg;

                               //  fin02 = ls - .1000f; 
                           
                                //tblflag.Text = lflg; //fin02.ToString();  //lflg; 

                               

                                String printRighthand = rightHandRight.ToString();
                                String printLefthand = leftHandLeft.ToString();
                                String printLeanLeft = leftLean.ToString();
                                String printLeanRight = rightLean.ToString();
                                String printPacing = paceUER.ToString(); 

                                
                                /*
                                 * Will write data to a file and then will go back to that file to read it to create the UER
                                 */
                                System.IO.File.WriteAllText(@"C:\Users\tlewis\Desktop\pacing.txt", printPacing);
                                System.IO.File.WriteAllText(@"C:\Users\tlewis\Desktop\gestures.txt",printLefthand + " " + printRighthand);
                                System.IO.File.WriteAllText(@"C:\Users\tlewis\Desktop\posture.txt", printLeanLeft + " " + printLeanRight);
                                 //System.IO.File.WriteAllText(@"C:\Users\tlewis\Desktop\WriteLines.txt", print);
                                // Thread.Sleep(1); 
                                 System.IO.File.WriteAllText(@"C:\Users\tlewis\Desktop\WriteLines.txt", p2);
                                 //Thread.Sleep(1);
                            
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
