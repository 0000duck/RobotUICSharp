using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    internal class State_Facetrack : StateInterface
    {
        int imageWidth = 640;
        int imageHeight = 480;
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
            return "Facetrack";
        }

        public void run()
        {
            RobotAnimator animator = RobotAnimator.Instance;
            Rectangle[] frontal = animator.frontalFaces;
            Rectangle[] profile = animator.profileFaces;
            Rectangle selected = new Rectangle(0, 0, 0, 0);

            if (frontal.Length > 0 || profile.Length > 0)
            {
                if (frontal.Length > 0)
                {
                    foreach (Rectangle rect in frontal)
                    {
                        if (rect.Size.Width * rect.Size.Height > selected.Size.Width * selected.Size.Height)
                            selected = rect;
                    }
                }
                else if (frontal.Length == 0 && profile.Length > 0)
                {
                    foreach (Rectangle rect in profile)
                    {
                        if (rect.Size.Width * rect.Size.Height > selected.Size.Width * selected.Size.Height)
                            selected = rect;
                    }
                }
                RobotAnimator.Instance.ReceivePositionContainer(MakeEyePositions(selected));

            }

        }

        private static int map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            int returnVal = (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
            Console.WriteLine(returnVal);
            return returnVal;
        }

        public MotorPositionsContainer MakeEyePositions(Rectangle input)
        {
            int faceX = (input.X + (input.Size.Width / 2));
            int faceY = (input.Y + (input.Size.Height / 2));
            int eyePosHorizontal = map(faceX, 0, imageWidth, 80, 100);
            int eyePosVertical = map(faceY, 0, imageHeight, 80, 100) +RobotAnimator.Instance.CameraEyeOffset;



            MotorPositionsContainer generatedPositions = new MotorPositionsContainer("empty");
            generatedPositions.LeftEyeHorizontalPosition = eyePosHorizontal;
            generatedPositions.RightEyeHorizontalPosition = eyePosHorizontal;
            generatedPositions.LeftEyeVerticalPosition = eyePosVertical;
            generatedPositions.RightEyeVerticalPosition = eyePosVertical;
            generatedPositions.LeftEyelidLowerPosition = 90;
            generatedPositions.LeftEyelidUpperPosition = 90;
            generatedPositions.RightEyelidLowerPosition = 90;
            generatedPositions.RightEyelidUpperPosition = 90;
            return generatedPositions;
        }

        public bool isBlocking()
        {
            return false;
        }
    }
}
