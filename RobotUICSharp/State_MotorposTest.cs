using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    internal class State_MotorposTest : StateInterface
    {
        MotorPositionsContainer positions;
        String identifier;


        public State_MotorposTest(MotorPositionsContainer mpc, String identifier = "")
        {
            this.positions = mpc;
            this.identifier = identifier;
        }

        public void begin()
        {
           // RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public void end()
        {
           // RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public string getStateName()
        {
            return "Test Motorpositions: " + this.identifier;
        }

        public bool isBlocking()
        {
            return false;
        }

        public void run()
        {
            RobotAnimator animator = RobotAnimator.Instance;
            animator.ReceivePositionContainer(this.positions);
            animator.setStateToIdle();
        }
    }
}
