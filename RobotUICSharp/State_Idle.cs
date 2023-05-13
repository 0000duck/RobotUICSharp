using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    internal class State_Idle : StateInterface
    {
        public void begin()
        {
     //       RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public void end()
        {
     //       RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public string getStateName()
        {
            return "Idle";
        }

        public bool isBlocking()
        {
            return false;
        }

        public void run()
        {
            
        }
    }
}
