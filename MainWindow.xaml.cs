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

                                // Draw hands and thumbs
                                canvas.DrawHand(handRight, _sensor.CoordinateMapper);
                                canvas.DrawHand(handLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbLeft, _sensor.CoordinateMapper);
                              

                                // Find the hand states
                                string rightHandState = "-";
                                string leftHandState = "-";
                                string leftshoulder = "-";
                                string rightshoulder = "-";
                                string upright = "-";
                                string paceListener = "-";
                              //  float pL = 0;
                                float test = 0; 
                                float ls = 0;
                                float rs = 0;
                                float ur = 0;
                                //const float tpL ; 
                                int cnt = 0;
                                double tollerance = 0.020000; //TODO need to figure out a good tollerance
                                double paceTollerance = 0.100000; //Pacing tollerance for horizontal movement
                                double pToll = 0.100000; //TODO play with these tollerances for better readings 
				                //hand lasson counter


                                pL  = pacer.Position.X;
                            
                                if((pacer.Position.X + paceTollerance < pToll) || (pacer.Position.X - paceTollerance > pToll )) //tracks pacing 
                                {
                                    test = pacer.Position.X; 
                                }


                                if ((leftShoulder.Position.Y + tollerance ) >= rightShoulder.Position.Y && (leftShoulder.Position.Y - tollerance) <= rightShoulder.Position.Y) 
                                {
                                    //No Direction
                                    upright = "yep";
                                    ur = leftShoulder.Position.Y + rightShoulder.Position.Y; 
                                }
                                else if (leftShoulder.Position.Y < rightShoulder.Position.Y)
                                {
                                    //leaning left
                                    leftshoulder = "leaning right..";
                                    rs = rightShoulder.Position.Y; 
                                }
                                else if (leftShoulder.Position.Y > rightShoulder.Position.Y)
                                {
                                    //leaning right
                                    rightshoulder = "leaning left..";
                                    ls = leftShoulder.Position.Y; 
                                }

				               
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
					                    cnt++;
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
                                tblRightHandState.Text = rightHandState;
                                tblLeftHandState.Text = leftHandState;
                               // tbleftShoulder.Text = leftshoulder;
                                tbrightShoulder.Text = rightshoulder;
                               // tbupright.Text = upright;
                                tbpl.Text = pL.ToString();
                                tbtest.Text = test.ToString(); 
                               /* tbleftShoulder.Text = rs.ToString();
                                tbrightShoulder.Text = ls.ToString();
                                tbupright.Text = ur.ToString(); */ 
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
