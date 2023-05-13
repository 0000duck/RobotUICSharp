using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    internal class MultiState_Test : StateInterface
    {
        StateInterface[] states = new StateInterface[4];
        int stateIterator = 0; 
        public MultiState_Test()
        {
            MotorPositionsContainer rightLook = new MotorPositionsContainer("empty");
            rightLook.LeftEyeHorizontalPosition = 100;
            rightLook.RightEyeHorizontalPosition = 100;
            rightLook.LeftEyelidLowerPosition = 72;
            rightLook.LeftEyelidUpperPosition = 72;
            rightLook.RightEyelidLowerPosition = 72;
            rightLook.RightEyelidUpperPosition = 72;
            rightLook.NeckRotationPosition = -50;


            MotorPositionsContainer leftLook = new MotorPositionsContainer("empty");
            leftLook.LeftEyeHorizontalPosition = 80;
            leftLook.RightEyeHorizontalPosition = 80;
            leftLook.LeftEyelidLowerPosition = 72;
            leftLook.LeftEyelidUpperPosition = 72;
            leftLook.RightEyelidLowerPosition = 72;
            leftLook.RightEyelidUpperPosition = 72;
            leftLook.NeckRotationPosition = 50;
            states[0] = new State_MotorposTest(rightLook, "Right Look");
            states[1] = new State_Speak("Look_Right",startIdle:false,endIdle:false);
            states[2] = new State_MotorposTest(leftLook, "Left Look");
            states[3] = new State_Speak("Look_Left", startIdle: false, endIdle: false);
        }
        public void begin()
        {
            RobotAnimator.Instance.SetStandardMotorPositions();

        }

        public void end()
        {
            RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public string getStateName()
        {
            return "MultiState_Test";
        }

        public bool isBlocking()
        {
            return false;
        }

        public void run()
        {
            Console.WriteLine(stateIterator);
            if (stateIterator == 0)
            {
                states[0].run();
                Thread.Sleep(1000);
                stateIterator = 1;
            }
            else if (stateIterator == 1)
            {
                states[1].run();
                while (states[1].isBlocking() == true)
                {
                    Thread.Sleep(1);
                }
                states[1].end();
                stateIterator = 2;
            }
            else if (stateIterator == 2)
            {
                states[2].run();
                Thread.Sleep(1000);
                stateIterator = 3;
            }
            else if (stateIterator == 3)
            {
                states[3].run();
                while (states[3].isBlocking() == true)
                {
                    Thread.Sleep(1);
                }
                RobotAnimator.Instance.setStateToIdle();

            }


        }
    }
}
