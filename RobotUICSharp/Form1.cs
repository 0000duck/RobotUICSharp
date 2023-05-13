using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Runtime;
using Emgu.Util;
using Emgu.CV.CvEnum;


using System.Speech.Recognition;
using System.IO;
using Newtonsoft.Json;
// Create an in-process speech recognizer for the en-US locale.  



namespace RobotUICSharp
{
    public partial class Main : Form
    {
        private RobotAnimator Animator;
        private bool EyeSyncState = false;

        private SerialConnector EyeSerialConnector;
        private SerialConnector MouthSerialConnector;

        private Thread EyeThread;
        private Thread MouthThread;
        private Thread AnimatorThread;
        private Thread WebThread;

        private VideoCapture capture;
        private CascadeClassifier haarCascadeFrontal;
        private CascadeClassifier haarCascadeProfile;
        SpeechRecognitionEngine recognizer;

        RobotWebController robotWebController;



        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Animator = RobotAnimator.Instance;
            // Animator.StateChanged += AnimatorStateChanged;
            this.StartAnimatorWorker();

            EyeSerialConnector = new SerialConnector();
            MouthSerialConnector = new SerialConnector();

            FindSerialPortsMouth();
            FindSerialPortsEye();

            SetupArpabetSelector();


            capture = new VideoCapture(0);
            haarCascadeFrontal = new CascadeClassifier(@"C:\Users\nn\source\repos\RobotUICSharp\RobotUICSharp\haarcascade_frontalface_default.xml");
            haarCascadeProfile = new CascadeClassifier(@"C:\Users\nn\source\repos\RobotUICSharp\RobotUICSharp\haarcascade_profileface.xml");

            loadSpeechRecognition();

            Webtest();

            fixArpabetPositions();
        }

        void fixArpabetPositions()
        {
            foreach (String s in ArpabetTranslator.arpabetPhonemes)
            {
                try
                {
                    MotorPositionsContainer mps = MotorPositionsContainer.ReadMotorPositionsFile(ArpabetTranslator.getMotorPositionFolderPath() + "/" + s + ".txt");
                    mps.LeftEyeHorizontalPosition = -1;
                    mps.LeftEyeVerticalPosition = -1;
                    mps.LeftEyelidUpperPosition = -1;
                    mps.LeftEyelidLowerPosition = -1;
                    mps.RightEyeHorizontalPosition = -1;
                    mps.RightEyeVerticalPosition = -1;
                    mps.RightEyelidUpperPosition = -1; ;
                    mps.RightEyelidLowerPosition = -1;
                    mps.NeckPitchPosition = -1;
                    mps.NeckRotationPosition = -1;
                    MotorPositionsContainer.WriteMotorPositionsFile(ArpabetTranslator.getMotorPositionFolderPath() + "/" + s + ".txt", mps);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void Webtest()
        {
            robotWebController = new RobotWebController("http://localhost:8000/");


        }
        public void SetupArpabetSelector()
        {
            foreach (String phoneme in ArpabetTranslator.arpabetPhonemes)
            {
                this.ArpabetSelector.Items.Add(phoneme);
            }
            this.ArpabetSelector.SelectedIndex = 0;
        }

        public void FindSerialPortsEye()
        {
            EyeSerialPortSelector.Items.Clear();
            foreach (String port in EyeSerialConnector.getSerialPorts())
            {
                EyeSerialPortSelector.Items.Add(port);
            }
            try
            {
                EyeSerialPortSelector.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void FindSerialPortsMouth()
        {
            MouthSerialPortSelector.Items.Clear();
            foreach (String port in MouthSerialConnector.getSerialPorts())
            {
                MouthSerialPortSelector.Items.Add(port);
            }
            try
            {
                MouthSerialPortSelector.SelectedIndex = MouthSerialPortSelector.Items.Count - 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void loadSpeechRecognition()
        {
            // Create an in-process speech recognizer for the en-US locale.  
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-UK"));

            var gb = new GrammarBuilder(CreateVoiceRecogLibrary());
            var g = new Grammar(gb);
            recognizer.LoadGrammar(g);

            // Add a handler for the speech recognized event.  
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(ReactToSpeech);

            // Configure input to the speech recognizer.  
            recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.  
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
        public Choices CreateVoiceRecogLibrary()
        {
            Choices AvailableLines = new Choices();
            //greetings
            foreach (Tuple<String, StateInterface> item in ActionConfiguration.GetSpeechConfig())
            {
                AvailableLines.Add(item.Item1);
            }
            return AvailableLines;
        }

        // Handle the SpeechRecognized event.  
        public void ReactToSpeech(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
            foreach (Tuple<String, StateInterface> item in ActionConfiguration.GetSpeechConfig())
            {
                if (e.Result.Text == item.Item1)
                {
                    RobotAnimator.Instance.ChangeState(item.Item2);
                }
            }
        }

        private void Vertical_Eye_Left_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LeftEyeVerticalPosition = Vertical_Eye_Left_Trackbar.Value;
            Label_Left_Vertical.Text = "Left Vertical: " + Vertical_Eye_Left_Trackbar.Value;
            if (this.EyeSyncState == true)
            {
                Animator.CurrentMotorPositions.RightEyeVerticalPosition = Vertical_Eye_Left_Trackbar.Value;
                Vertical_Eye_Right_Trackbar.Value = Vertical_Eye_Left_Trackbar.Value;
                Label_Right_Vertical.Text = "Right Vertical: " + Vertical_Eye_Right_Trackbar.Value;

            }

        }

        private void Horizontal_Eye_Left_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LeftEyeHorizontalPosition = Horizontal_Eye_Left_Trackbar.Value;
            Label_Left_Horizontal.Text = "Left Horizontal: " + Horizontal_Eye_Left_Trackbar.Value;

            if (this.EyeSyncState == true)
            {
                Animator.CurrentMotorPositions.RightEyeHorizontalPosition = Horizontal_Eye_Left_Trackbar.Value;
                Horizontal_Eye_Right_Trackbar.Value = Horizontal_Eye_Left_Trackbar.Value;
                Label_Right_Horizontal.Text = "Right Horizontal: " + Horizontal_Eye_Right_Trackbar.Value;
            }
        }

        private void Upper_Eyelid_Left_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LeftEyelidUpperPosition = Upper_Eyelid_Left_Trackbar.Value;
            Label_Eyelid_Upper_Left.Text = "Left U. Eyelid: " + Upper_Eyelid_Left_Trackbar.Value;
            if (this.EyeSyncState == true)
            {
                Animator.CurrentMotorPositions.RightEyelidUpperPosition = Upper_Eyelid_Left_Trackbar.Value;
                Upper_Eyelid_Right_Trackbar.Value = Upper_Eyelid_Left_Trackbar.Value;
                Label_Eyelid_Upper_Right.Text = "Right U. Eyelid: " + Upper_Eyelid_Right_Trackbar.Value;
            }
        }

        private void Lower_Eyelid_Left_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LeftEyelidLowerPosition = Lower_Eyelid_Left_Trackbar.Value;
            Label_Eyelid_Lower_Left.Text = "Left L. Eyelid: " + Lower_Eyelid_Left_Trackbar.Value;
            if (this.EyeSyncState == true)
            {
                Animator.CurrentMotorPositions.RightEyelidLowerPosition = Lower_Eyelid_Left_Trackbar.Value;
                Lower_Eyelid_Right_Trackbar.Value = Lower_Eyelid_Left_Trackbar.Value;
                Label_Eyelid_Lower_Right.Text = "Right L. Eyelid: " + Lower_Eyelid_Right_Trackbar.Value;
            }
        }

        private void Vertical_Eye_Right_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.RightEyeVerticalPosition = Vertical_Eye_Right_Trackbar.Value;
            Label_Right_Vertical.Text = "Right Vertical: " + Vertical_Eye_Right_Trackbar.Value;
        }

        private void Horizontal_Eye_Right_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.RightEyeHorizontalPosition = Horizontal_Eye_Right_Trackbar.Value;
            Label_Right_Horizontal.Text = "Right Horizontal: " + Horizontal_Eye_Right_Trackbar.Value;

        }

        private void NeckSpeedTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.NeckRotationSpeed = NeckSpeedTrackbar.Value;
            NeckSpeedLabel.Text = "Neck Speed: " + NeckSpeedTrackbar.Value;
        }

        private void Upper_Eyelid_Right_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.RightEyelidUpperPosition = Upper_Eyelid_Right_Trackbar.Value;
            Label_Eyelid_Upper_Right.Text = "Right U. Eyelid: " + Upper_Eyelid_Right_Trackbar.Value;
        }

        private void Lower_Eyelid_Right_Trackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.RightEyelidLowerPosition = Lower_Eyelid_Right_Trackbar.Value;
            Label_Eyelid_Lower_Right.Text = "Right L. Eyelid: " + Lower_Eyelid_Right_Trackbar.Value;
        }

        private void UpperCornerTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.UpperMouthCornerPosition = UpperCornerTrackbar.Value;
            Upper_Corner_Label.Text = "Upper Corner: " + UpperCornerTrackbar.Value;
        }

        private void UpperCheekTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.UpperMouthCheekPosition = UpperCheekTrackbar.Value;
            Upper_Cheek_Label.Text = "Upper Cheek: " + UpperCheekTrackbar.Value;
        }

        private void UpperCanineTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.UpperMouthCaninePosition = UpperCanineTrackbar.Value;
            Upper_Canine_Label.Text = "Upper Canines: " + UpperCanineTrackbar.Value;
        }

        private void UpperMiddleTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.UpperMouthMiddlePosition = UpperMiddleTrackbar.Value;
            Upper_Middle_Label.Text = "Upper Middle: " + UpperMiddleTrackbar.Value;
        }

        private void LowerCornerTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LowerMouthCornerPosition = LowerCornerTrackbar.Value;
            Lower_Corner_Label.Text = "Lower Corner: " + LowerCornerTrackbar.Value;
        }

        private void LowerCheekTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LowerMouthCheekPosition = LowerCheekTrackbar.Value;
            Lower_Cheek_Label.Text = "Lower Cheek: " + LowerCheekTrackbar.Value;
        }

        private void LowerCanineTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LowerMouthCaninePosition = LowerCanineTrackbar.Value;
            Lower_Canine_Label.Text = "Lower Canines: " + LowerCanineTrackbar.Value;
        }

        private void LowerMiddleTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.LowerMouthMiddlePosition = LowerMiddleTrackbar.Value;
            Lower_Middle_Label.Text = "Lower Middle: " + LowerMiddleTrackbar.Value;
        }

        private void JawTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.JawPosition = JawTrackbar.Value;
            JawLabel.Text = "Jaw: " + JawTrackbar.Value;
        }

        private void NeckRotationTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.NeckRotationPosition = NeckRotationTrackbar.Value;
            NeckRotationLabel.Text = "Neck Rotation: " + NeckRotationTrackbar.Value;
        }

        private void NeckPitchTrackbar_Scroll(object sender, EventArgs e)
        {
            Animator.CurrentMotorPositions.NeckPitchPosition = NeckPitchTrackbar.Value;
            NeckPitchLabel.Text = "Neck Pitch: " + NeckPitchTrackbar.Value;
        }
        private void CheckBoxSyncEye_CheckedChanged(object sender, EventArgs e)
        {
            this.EyeSyncState = CheckBoxSyncEye.Checked;
            if (this.EyeSyncState == true)
            {
                Container_Eye_Right.Enabled = false;
            }
            else
            {
                Container_Eye_Right.Enabled = true;
            }
        }

        private void AnimatorStateChanged(object sender, StateChangeEventArgs e)
        {
            try { State_Name_Label.Text = e.newState.getStateName(); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void MouthConnectButton_Click(object sender, EventArgs e)
        {
            if (MouthSerialConnector.isOpen() == false)
            {
                if (MouthSerialPortSelector.Text != "")
                {
                    MouthSerialConnector.setPort(MouthSerialPortSelector.Text);
                    bool connectStatus = MouthSerialConnector.openPort();
                    if (connectStatus == true)
                    {
                        MouthConnectButton.BackColor = Color.Green;
                        this.MouthThread = new Thread(new ThreadStart(this.MouthWorker));
                        this.MouthThread.Start();
                        FindSerialPortsEye();
                    }
                    else
                    {
                        MouthConnectButton.BackColor = Color.Red;
                    }
                }
                else
                {
                    string message = "Select Port for Eyes";
                    MessageBox.Show(message);
                    EyeConnectButton.BackColor = Color.Red;
                }
            }
            else
            {
                MouthSerialConnector.shutdownPort();
                MouthConnectButton.BackColor = Color.Red;
            }
        }

        private void EyeConnectButton_Click(object sender, EventArgs e)
        {
            if (EyeSerialConnector.isOpen() == false)
            {
                if (EyeSerialPortSelector.Text != "")
                {
                    EyeSerialConnector.setPort(EyeSerialPortSelector.Text);
                    bool connectStatus = EyeSerialConnector.openPort();
                    if (connectStatus == true)
                    {
                        EyeConnectButton.BackColor = Color.Green;
                        this.EyeThread = new Thread(new ThreadStart(this.EyeWorker));
                        this.EyeThread.Start();
                        FindSerialPortsMouth();
                    }
                    else
                    {
                        EyeConnectButton.BackColor = Color.Red;
                        string message = "Error while opening Serial for Eyes";
                        MessageBox.Show(message);
                    }
                }
                else
                {
                    string message = "Select Port for Eyes";
                    MessageBox.Show(message);
                    EyeConnectButton.BackColor = Color.Red;

                }
            }
            else
            {
                MouthSerialConnector.shutdownPort();
                MouthConnectButton.BackColor = Color.Red;
            }

        }


        private void StartAnimatorWorker()
        {
            this.AnimatorThread = new Thread(new ThreadStart(this.Animator.run));
            this.AnimatorThread.Start();
        }
        //This method creates a string that is suitable for the arduino controlling the servo motors on the EYES and uses the SerialConnector to send it
        private void EyeWorker()
        {
            while (true)
            {
                String msg = "EYE-" + Animator.CurrentMotorPositions.LeftEyeHorizontalPosition.ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.LeftEyeVerticalPosition.ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.LeftEyelidUpperPosition.ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.LeftEyelidLowerPosition.ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.RightEyeHorizontalPosition.ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.mirrorMotorValue(Animator.CurrentMotorPositions.RightEyeVerticalPosition, 90).ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.mirrorMotorValue(Animator.CurrentMotorPositions.RightEyelidUpperPosition, 90).ToString("000"); ;
                msg += "-" + Animator.CurrentMotorPositions.mirrorMotorValue(Animator.CurrentMotorPositions.RightEyelidLowerPosition, 90).ToString("000"); ;
                msg = "<" + msg + ">";
                this.EyeSerialConnector.sendMessage(msg);
                // Console.WriteLine(msg);
                Thread.Sleep(100);

            }
        }

        //This method creates a string that is suitable for the arduino controlling the servo motors on the MOUTH and uses the SerialConnector to send it 
        private void MouthWorker()
        {
            while (true)
            {
                string msg = "MOU-";
                msg += Animator.CurrentMotorPositions.LowerMouthCaninePosition.ToString().PadLeft(3, '0') + "-";
                msg += Animator.CurrentMotorPositions.LowerMouthCheekPosition.ToString().PadLeft(3, '0') + "-";
                msg += Animator.CurrentMotorPositions.LowerMouthMiddlePosition.ToString().PadLeft(3, '0') + "-";

                msg += Animator.CurrentMotorPositions.UpperMouthCaninePosition.ToString().PadLeft(3, '0') + "-";
                msg += Animator.CurrentMotorPositions.UpperMouthCheekPosition.ToString().PadLeft(3, '0') + "-";
                msg += Animator.CurrentMotorPositions.UpperMouthMiddlePosition.ToString().PadLeft(3, '0') + "-";

                msg += Animator.CurrentMotorPositions.LowerMouthCornerPosition.ToString().PadLeft(3, '0') + "-";
                msg += Animator.CurrentMotorPositions.UpperMouthCornerPosition.ToString().PadLeft(3, '0') + "-";

                msg += Animator.CurrentMotorPositions.JawPosition.ToString().PadLeft(3, '0') + "-";
                msg += Animator.CurrentMotorPositions.NeckPitchPosition.ToString().PadLeft(3, '0');
                msg = "<" + msg + ">";
                //    Console.WriteLine(msg);
                this.MouthSerialConnector.sendMessage(msg);
                //  Console.WriteLine(msg);
                Thread.Sleep(20);

            }
        }

        //Takes the current animator positions and transfers their values into the UI 
        //Same for state-Names 
        private void UpdateUI()
        {
            try
            {
                Vertical_Eye_Left_Trackbar.Value = Animator.CurrentMotorPositions.LeftEyeVerticalPosition;
                Horizontal_Eye_Left_Trackbar.Value = Animator.CurrentMotorPositions.LeftEyeHorizontalPosition;
                Vertical_Eye_Right_Trackbar.Value = Animator.CurrentMotorPositions.RightEyeVerticalPosition;
                Horizontal_Eye_Right_Trackbar.Value = Animator.CurrentMotorPositions.RightEyeHorizontalPosition;

                Lower_Eyelid_Left_Trackbar.Value = Animator.CurrentMotorPositions.LeftEyelidLowerPosition;
                Upper_Eyelid_Left_Trackbar.Value = Animator.CurrentMotorPositions.LeftEyelidUpperPosition;
                Lower_Eyelid_Right_Trackbar.Value = Animator.CurrentMotorPositions.RightEyelidLowerPosition;
                Upper_Eyelid_Right_Trackbar.Value = Animator.CurrentMotorPositions.RightEyelidUpperPosition;


                UpperCanineTrackbar.Value = Animator.CurrentMotorPositions.UpperMouthCaninePosition;
                UpperCheekTrackbar.Value = Animator.CurrentMotorPositions.UpperMouthCheekPosition;
                UpperCornerTrackbar.Value = Animator.CurrentMotorPositions.UpperMouthCornerPosition;
                UpperMiddleTrackbar.Value = Animator.CurrentMotorPositions.UpperMouthMiddlePosition;
                LowerCanineTrackbar.Value = Animator.CurrentMotorPositions.LowerMouthCaninePosition;
                LowerCheekTrackbar.Value = Animator.CurrentMotorPositions.LowerMouthCheekPosition;
                LowerCornerTrackbar.Value = Animator.CurrentMotorPositions.LowerMouthCornerPosition;
                LowerMiddleTrackbar.Value = Animator.CurrentMotorPositions.LowerMouthMiddlePosition;
                JawTrackbar.Value = Animator.CurrentMotorPositions.JawPosition;
                NeckPitchTrackbar.Value = Animator.CurrentMotorPositions.NeckPitchPosition;
                NeckRotationTrackbar.Value = Animator.CurrentMotorPositions.NeckRotationPosition;
                if (Animator.CurrentState != null)
                {
                    State_Name_Label.Text = Animator.CurrentState.getStateName();
                }
                JawLabel.Text = "Jaw: " + JawTrackbar.Value;
           
                Lower_Middle_Label.Text = "Lower Middle: " + LowerMiddleTrackbar.Value;
                Lower_Cheek_Label.Text = "Lower Cheek: " + LowerCheekTrackbar.Value;
                Lower_Canine_Label.Text = "Lower Canines: " + LowerCanineTrackbar.Value;
                Lower_Corner_Label.Text = "Lower Corner: " + LowerCornerTrackbar.Value;

                Upper_Middle_Label.Text = "Upper Middle: " + UpperMiddleTrackbar.Value;
                Upper_Canine_Label.Text = "Upper Canines: " + UpperCanineTrackbar.Value;
                Upper_Corner_Label.Text = "Upper Corner: " + UpperCornerTrackbar.Value;
                Upper_Cheek_Label.Text = "Upper Cheek: " + UpperCheekTrackbar.Value;

                Label_Eyelid_Lower_Right.Text = "Right L. Eyelid: " + Lower_Eyelid_Right_Trackbar.Value;
                Label_Eyelid_Upper_Right.Text = "Right U. Eyelid: " + Upper_Eyelid_Right_Trackbar.Value;
                Label_Right_Horizontal.Text = "Right Horizontal: " + Horizontal_Eye_Right_Trackbar.Value;
                Label_Right_Vertical.Text = "Right Vertical: " + Vertical_Eye_Right_Trackbar.Value;

                Label_Eyelid_Lower_Left.Text = "Left L. Eyelid: " + Lower_Eyelid_Left_Trackbar.Value;
                Label_Eyelid_Upper_Left.Text = "Left U. Eyelid: " + Upper_Eyelid_Left_Trackbar.Value;
                Label_Left_Horizontal.Text = "Left Horizontal: " + Horizontal_Eye_Left_Trackbar.Value;
                Label_Left_Vertical.Text = "Left Vertical: " + Vertical_Eye_Left_Trackbar.Value;

                NeckPitchLabel.Text = "Neck Pitch: " + NeckPitchTrackbar.Value; 
                NeckRotationLabel.Text = "Neck Rotation: " + NeckRotationTrackbar.Value;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(RobotAnimator.Instance.CurrentMotorPositions.ToString());
            }
        }

        private void FaceRecogTimer_Tick(object sender, EventArgs e)
        {
            if (CameraFeedCheckbox.Checked == true)
            {

                using (Emgu.CV.Mat frame = capture.QueryFrame())
                {
                    // Console.WriteLine(frame.Width + ":" + frame.Height);
                    if (frame != null)
                    {
                        Rectangle[] RectFacesFrontal = haarCascadeFrontal.DetectMultiScale(frame, 1.03, 1, new Size(frame.Width / 13, frame.Height / 13), new Size((int)((double)frame.Width / 1.05), (int)((double)frame.Width / 1.05)));
                        Rectangle[] RectFacesProfile = haarCascadeProfile.DetectMultiScale(frame, 1.03, 1, new Size(frame.Width / 13, frame.Height / 13), new Size((int)((double)frame.Width / 1.05), (int)((double)frame.Width / 1.05)));

                        Bitmap image = frame.ToBitmap();
                        foreach (Rectangle rect in RectFacesFrontal)
                        {
                            Pen greenPen = new Pen(Color.Green, 3);

                            using (var graphics = Graphics.FromImage(image))
                            {
                                graphics.DrawRectangle(greenPen, rect);
                            }
                        }
                        foreach (Rectangle rect in RectFacesProfile)
                        {
                            Pen bluePen = new Pen(Color.Blue, 3);

                            using (var graphics = Graphics.FromImage(image))
                            {
                                graphics.DrawRectangle(bluePen, rect);
                            }
                        }
                        Animator.profileFaces = RectFacesProfile;
                        Animator.frontalFaces = RectFacesFrontal;

                        Bitmap resized = new Bitmap(image, new Size(CameraFeed.Width, CameraFeed.Height));
                        CameraFeed.Image = resized;
                        //  CameraFeed.Size = new Size(frame.Width, frame.Height);
                    }
                    else
                    {
                        Console.WriteLine("Err");
                    }
                }
            }
        }



        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void Test_Button_Click(object sender, EventArgs e)
        {
            Animator.ChangeState(new State_Sleep_Awake(1000));
        }

        private void TestMouthPositionButton_Click(object sender, EventArgs e)
        {
            Animator.ChangeState(new State_MotorposTest(new ArpabetTranslator().getMouthPosition(ArpabetSelector.Text), ArpabetSelector.Text));
        }

        private void MotorPosSaveButton_Click(object sender, EventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (SaveFileDialog SaveDialog = new SaveFileDialog())
            {
                SaveDialog.InitialDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString()).ToString() + "/MotorPositions";
                SaveDialog.Filter = "txt files (*.txt)|*.txt";
                SaveDialog.FilterIndex = 2;
                SaveDialog.RestoreDirectory = true;

                if (SaveDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = SaveDialog.FileName;

                }
            }
            MotorPositionsContainer.WriteMotorPositionsFile(filePath, Animator.CurrentMotorPositions);
        }

        private void LoadMotorPosButton_Click(object sender, EventArgs e)
        {

            String filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString()).ToString() + "/MotorPositions";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    Animator.CurrentMotorPositions = MotorPositionsContainer.ReadMotorPositionsFile(filePath);


                }
            }

        }

        private void StandardPositionMotorButton_Click(object sender, EventArgs e)
        {
            Animator.SetStandardMotorPositions();
        }

        private void CameraFeedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraFeedCheckbox.Checked) { }
            else { CameraFeed.Image = null; }
        }


    }
}

