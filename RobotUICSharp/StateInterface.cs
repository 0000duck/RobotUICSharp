using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    public interface StateInterface
    {
        void begin();
        void end();
        void run();
        String getStateName();

        bool isBlocking();
    }
}
