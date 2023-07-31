using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RobotUICSharp
{
    internal class State_MotorEnduranceTest : StateInterface
    {
        public void begin()
        {
            
        }

        public void end()
        {
            
        }

        public string getStateName()
        {
            return "Loosening Motors";
        }

        public bool isBlocking()
        {
           return false;
        }

        public void run()
        {
            MotorPositionsContainer PositionA = new MotorPositionsContainer("empty");
            PositionA.UpperMouthCaninePosition = 100;
            MotorPositionsContainer PositionB = new MotorPositionsContainer("empty");
            PositionA.UpperMouthCaninePosition = 70;
            RobotAnimator.Instance.CurrentMotorPositions = PositionA;
            Thread.Sleep(500);
            RobotAnimator.Instance.CurrentMotorPositions = PositionB;
            Thread.Sleep(500);

        }
    }
}
