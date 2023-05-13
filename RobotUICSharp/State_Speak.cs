using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    internal class State_Speak : StateInterface
    {
        String filepath;
        bool speechStarted = false;

        bool blockstate = true;
        bool startIdle;
        bool endIdle;
        public State_Speak(String filepath, bool startIdle = true,bool endIdle=true)
        {
            this.filepath = filepath;
            this.startIdle = startIdle;
            this.endIdle = endIdle;
        }
        public void begin()
        {
            if (startIdle)
            {
                RobotAnimator.Instance.SetStandardMotorPositions();
            }
        }

        public void end()
        {
            if (endIdle)
            {
                RobotAnimator.Instance.SetStandardMotorPositions();
            }
            RobotAnimator.Instance.AudioSynchronizer.SpeechComplete -= SpeechEnded;
        }

        public string getStateName()
        {
            return "Speak: " + this.filepath;  
        }

        public bool isBlocking()
        {
            return this.blockstate;
        }

        public void run()
        {
            if (this.speechStarted == false)
            {
                RobotAnimator.Instance.PlaySoundWithMovement(this.filepath);

                    RobotAnimator.Instance.AudioSynchronizer.SpeechComplete += SpeechEnded;
               
                this.speechStarted = true;
            }
        }

        public void SpeechEnded(object sender, EventArgs e)
        {
            Console.WriteLine("State Speak noticed that speech has ended");
            this.blockstate = false;
            if (endIdle)
            {
                RobotAnimator.Instance.setStateToIdle();
            }
        }
    }
}
