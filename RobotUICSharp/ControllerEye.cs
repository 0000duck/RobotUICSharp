using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace RobotUICSharp
{

    public partial class ControllerEye : UserControl
    {

        int x, y;
        Graphics g;
        int radius = 10;
        int half = 0;
        bool clicked = false;
        Color cirColor = Color.Brown;
        Color EyeColor = Color.Brown;
        public event EventHandler<EyeMovementArgs> NewEyePosition;
        public ControllerEye()
        {
            InitializeComponent();
            x = panel1.Width / 2;
            y = panel1.Height / 2;  
        }

        private void panel_click(object sender, MouseEventArgs e)
        {
            half = radius / 2;
            Point p = new Point(e.X, e.Y);
            x = p.X;
            y = p.Y;
            clicked = true;
            panel1.Invalidate();
            NewEyePosition.Invoke(this,new EyeMovementArgs(x , y));
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
               // half = radius / 2;
                Point p = new Point(e.X, e.Y);
                x = p.X;
                y = p.Y;

                panel1.Invalidate();
                NewEyePosition.Invoke(this, new EyeMovementArgs(x-1, y - 1));
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black);    
            Pen p2 = new Pen(Color.Chartreuse);
           
            g.DrawEllipse(p, x - half, y - half, radius , radius );
           // angle = atan2(my - y, mx - x);
            double angle = Math.Atan2(x-panel1.Width/2,y-panel1.Height/2);
            float deg = Convert.ToSingle(angle) * 180 / Convert.ToSingle(Math.PI);
            g.FillEllipse(new SolidBrush(EyeColor), new Rectangle(panel1.Width / 2-100, panel1.Height / 2-100, 200, 200));
            g.DrawLine(p, new Point(panel1.Width / 2, 0),new Point( panel1.Width / 2, panel1.Height));
            g.DrawLine(p, new Point(0, panel1.Height/2), new Point(panel1.Width , panel1.Height/2));
            g.DrawRectangle(p, new Rectangle(0, 0, panel1.Width-1, panel1.Height-1));
            e.Graphics.TranslateTransform(panel1.Width / 2, panel1.Height / 2);
            e.Graphics.RotateTransform(-deg+90);
            //
           // e.Graphics.DrawString(angle.ToString(), new Font("Arial", 10),sb,new Point(panel1.Width / 2, panel1.Height/2));
            g.DrawEllipse(new Pen(Color.Magenta), panel1.Width/2 +radius / 4, panel1.Width / 2,500, 500);
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), 10, 0, 80,80);

        }

    }

    public class EyeMovementArgs : EventArgs
    {
        public int x;
        public int y;

        public EyeMovementArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

}
