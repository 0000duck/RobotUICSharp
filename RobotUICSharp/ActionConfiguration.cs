using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    internal class ActionConfiguration
    {

        public static List<Tuple<String,StateInterface>> GetSpeechConfig()
        {
            List<Tuple<String, StateInterface>> toReturn = new List<Tuple<String, StateInterface>>();
            toReturn.Add(new Tuple<string, StateInterface>("Perform Speech Test", new State_Speak("TestA")));
            toReturn.Add(new Tuple<string, StateInterface>("What are your hobbies?", new State_Speak("Hobbies")));
            toReturn.Add(new Tuple<string, StateInterface>("What is your favorite movie?", new State_Speak("Fav_Movie")));
            toReturn.Add(new Tuple<string, StateInterface>("Why do you want to be human?", new State_Speak("Why_Human")));
            toReturn.Add(new Tuple<string, StateInterface>("Perform Multi Test", new MultiState_Test()));
            return toReturn;
        }

        public static List<Tuple<String, StateInterface>> GetURLConfig()
        {
            List<Tuple<String, StateInterface>> toReturn = new List<Tuple<String, StateInterface>>();
            toReturn.Add(new Tuple<string, StateInterface>("/TestA", new State_Speak("TestA")));
            return toReturn;
        }

    }
}
