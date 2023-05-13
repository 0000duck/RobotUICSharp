using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace RobotUICSharp
{
    public class RobotAnimator
    {
        //  public event EventHandler<StateChangeEventArgs> StateChanged;
        public MotorPositionsContainer CurrentMotorPositions { get; set; }
        public Rectangle[] frontalFaces { get; set; }
        public Rectangle[] profileFaces { get; set; }
        public StateInterface CurrentState { get; set; }

        private Thread AudioPlayThread;

        public int CameraEyeOffset = -3;

        public AudioSynchronizer AudioSynchronizer;

        public int NeckRotationSpeed = 200;

        public RobotAnimator()
        {
            CurrentMotorPositions = new MotorPositionsContainer("standard");
            AudioSynchronizer = new AudioSynchronizer();
            AudioSynchronizer.NewMouthPositions += ReceiveMotorPositionsEvent;
            AudioSynchronizer.SpeechComplete += ReceiveSpeechInfo;
            CurrentState = new State_Idle();
        }
        private static readonly object locked = new object();
        private static RobotAnimator instance = null;

        public static RobotAnimator Instance
        {
            get
            {
                lock (locked)
                {
                    if (instance == null)
                    {
                        instance = new RobotAnimator();
                    }
                    return instance;
                }
            }
        }

        public void run()
        {
            while (true)
            {
                if (CurrentState != null)
                {
                    CurrentState.run();
                }
            }
        }

        public void ChangeState(StateInterface newState)
        {
            if (CurrentState.isBlocking() == false )
            {
                if (CurrentState != null)
                {
                    CurrentState.end();
                }

                CurrentState = newState;
                StateChangeEventArgs args = new StateChangeEventArgs();
                args.newState = newState;
                // StateChanged.Invoke(this, args);
            }

            else
            {
                Console.WriteLine("State {0} is blocking the state transition for state {1}",CurrentState.getStateName(), newState.getStateName());
            }
        }
        public void ReceiveMotorPositionsEvent(object sender, NewMotorPositionsEventArgs e)
        {
            MotorPositionsContainer newPositions = e.Positions;

            ReceivePositionContainer(newPositions);

        }
        public void ReceivePositionContainer(MotorPositionsContainer container)
        {
            //Console.WriteLine("Received PositionContainer");

            if (container.LeftEyeHorizontalPosition != -1)
            {
                CurrentMotorPositions.LeftEyeHorizontalPosition = container.LeftEyeHorizontalPosition;
            }
            if (container.LeftEyeVerticalPosition != -1)
            {
                CurrentMotorPositions.LeftEyeVerticalPosition = container.LeftEyeVerticalPosition;
            }
            if (container.LeftEyelidUpperPosition != -1)
            {
                CurrentMotorPositions.LeftEyelidUpperPosition = container.LeftEyelidUpperPosition;
            }
            if (container.LeftEyelidLowerPosition != -1)
            {
                CurrentMotorPositions.LeftEyelidLowerPosition = container.LeftEyelidLowerPosition;
            }

            if (container.RightEyeHorizontalPosition != -1)
            {
                CurrentMotorPositions.RightEyeHorizontalPosition = container.RightEyeHorizontalPosition;
            }
            if (container.RightEyeVerticalPosition != -1)
            {
                CurrentMotorPositions.RightEyeVerticalPosition = container.RightEyeVerticalPosition;
            }
            if (container.RightEyelidLowerPosition != -1)
            {
                CurrentMotorPositions.RightEyelidUpperPosition = container.RightEyelidLowerPosition;
            }
            if (container.RightEyelidUpperPosition != -1)
            {
                CurrentMotorPositions.RightEyelidLowerPosition = container.RightEyelidUpperPosition;
            }

            if (container.UpperMouthCornerPosition != -1)
            {
                CurrentMotorPositions.UpperMouthCornerPosition = container.UpperMouthCornerPosition;
            }
            if (container.UpperMouthCheekPosition != -1)
            {
                CurrentMotorPositions.UpperMouthCheekPosition = container.UpperMouthCheekPosition;
            }
            if (container.UpperMouthCaninePosition != -1)
            {
                CurrentMotorPositions.UpperMouthCaninePosition = container.UpperMouthCaninePosition;
            }
            if (container.UpperMouthMiddlePosition != -1)
            {
                CurrentMotorPositions.UpperMouthMiddlePosition = container.UpperMouthMiddlePosition;
            }

            if (container.LowerMouthCornerPosition != -1)
            {
                CurrentMotorPositions.LowerMouthCornerPosition = container.LowerMouthCornerPosition;
            }
            if (container.LowerMouthCheekPosition != -1)
            {
                CurrentMotorPositions.LowerMouthCheekPosition = container.LowerMouthCheekPosition;
            }
            if (container.LowerMouthCaninePosition != -1)
            {
                CurrentMotorPositions.LowerMouthCaninePosition = container.LowerMouthCaninePosition;
            }
            if (container.LowerMouthMiddlePosition != -1)
            {
                CurrentMotorPositions.LowerMouthMiddlePosition = container.LowerMouthMiddlePosition;
            }

            if (container.JawPosition != -1)
            {
                CurrentMotorPositions.JawPosition = container.JawPosition;
            }
            if (container.NeckRotationPosition != -1)
            {
                CurrentMotorPositions.NeckRotationPosition = container.NeckRotationPosition;
            }
            if (container.NeckPitchPosition != -1)
            {
                CurrentMotorPositions.NeckPitchPosition = container.NeckPitchPosition;
            }
        }

        public void setStateToIdle()
        {
            Console.WriteLine("Set state to idle");
            this.ChangeState(new State_Idle());
        }


        public void PlaySoundWithMovement(String file)
        {
            AudioSynchronizer.setFilepath(file);
            this.AudioPlayThread = new Thread(new ThreadStart(AudioSynchronizer.playAudio));
            this.AudioPlayThread.Start();
        }
        public void ReceiveSpeechInfo(object sender, EventArgs e)
        {

            Console.WriteLine("Speech ended ");

        }

        public void SetStandardMotorPositions()
        {
            this.CurrentMotorPositions = new MotorPositionsContainer("standard");
        }
    }


    public class StateChangeEventArgs : EventArgs
    {
        public StateInterface newState { get; set; }

    }
}
