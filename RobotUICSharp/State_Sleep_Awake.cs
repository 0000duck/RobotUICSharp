using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RobotUICSharp
{
    internal class State_Sleep_Awake : StateInterface
    {
        bool blockstate = true;
        int sleeptime;
        public State_Sleep_Awake(int sleeptimeMS)
        {
            this.sleeptime = sleeptimeMS;
        }

        public void begin()
        {
            //RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public void end()
        {
            //RobotAnimator.Instance.SetStandardMotorPositions();
        }

        public string getStateName()
        {
            return "Sleeping for " + sleeptime / 1000 + "seconds";
        }

        public bool isBlocking()
        {
            return blockstate;
        }

        public void run()
        {
            RobotAnimator animator = RobotAnimator.Instance;
            MotorPositionsContainer motorpos = new MotorPositionsContainer("empty");
            motorpos.NeckPitchPosition = 70;
            motorpos.LeftEyelidLowerPosition = 90;
            motorpos.LeftEyelidUpperPosition = 90;
            motorpos.RightEyelidLowerPosition = 90;
            motorpos.RightEyelidUpperPosition = 90;
            animator.ReceivePositionContainer(motorpos);
            Thread.Sleep(sleeptime);
            motorpos = new MotorPositionsContainer("empty");
            motorpos.NeckPitchPosition = 90;
            motorpos.LeftEyelidLowerPosition = 75;
            motorpos.LeftEyelidUpperPosition = 75;
            motorpos.RightEyelidLowerPosition = 75;
            motorpos.RightEyelidUpperPosition = 75;
            animator.ReceivePositionContainer(motorpos);
            Thread.Sleep(500);
            String[] choices = { "Dream_Herzog", "Dream_Creator", "Dream_Sheep" };
            Random random = new Random();
            int select = random.Next(0, choices.Length);
            animator.PlaySoundWithMovement(choices[select]);
            Thread.Sleep(200);
            while (animator.AudioSynchronizer.isPlaying) {
                Thread.Sleep(1);
            }
            this.blockstate = false;
            //Thread.Sleep(5000);
            animator.setStateToIdle();
        }
    }
}
