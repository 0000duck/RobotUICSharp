using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Newtonsoft.Json;

namespace RobotUICSharp
{
    //This class plays audio and synchronizes the mouth to the words being said 
    public class AudioSynchronizer
    {

        String filepath;
        public event EventHandler<NewMotorPositionsEventArgs> NewMouthPositions;
        public event EventHandler<EventArgs> SpeechComplete;
        public bool isPlaying = false;


        public AudioSynchronizer() { }

        //Sets the filepath to be played 
        public void setFilepath(String filepath)
        {
            this.filepath = filepath;
        }

        //Translates a TimeSpan-Object into Seconds (Taken from StackOverflow)
        private float TimeSpanToFloat(TimeSpan input)
        {
            float floatTimeSpan;
            int seconds, milliseconds;
            seconds = input.Seconds;
            milliseconds = input.Milliseconds;
            floatTimeSpan = (float)seconds + ((float)milliseconds / 1000);
            return floatTimeSpan;

        }

        //Plays an Audiofile in 3 steps:
        //A- Parse the TXT -file associated with the soundfile - create a list of words, phonemes and the times they are being spoken 
        //B - Start Audio playback
        //C - While playback is active, watch the current playbacktime and compare with the list of words. Send mouth positions using the arpabet translator 
        public void playAudio()
        {
            //isPlaying helps other components react when the playback is finished ( -While (isPlaying) { doSomething() }
            isPlaying = true;
            List<Word> words = new List<Word>();
            String line;
            //Try-Catch in case the path provided is incorrect. 
            try
            {
                StreamReader sr = new StreamReader(@"C:\Users\nn\source\repos\RobotUICSharp\RobotUICSharp\Voicelines\" + this.filepath + @"\" + this.filepath + @".txt");
                //Reading the file 
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line != "")
                    {
                        dynamic deserializedWord = JsonConvert.DeserializeObject(line);
                        Word newWord = new Word(deserializedWord.word.ToObject<String>(), deserializedWord.start.ToObject<float>(), deserializedWord.end.ToObject<float>(), deserializedWord.phonemes.ToObject<List<String>>(), deserializedWord.conf.ToObject<float>());
                        words.Add(newWord);
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            //Starting the playback 
            using (var audioFile = new AudioFileReader(@"C:\Users\nn\source\repos\RobotUICSharp\RobotUICSharp\Voicelines\" + this.filepath + @"\" + this.filepath + @".wav"))
            {
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    dynamic current_word = null;
                    //counts through phonemes in the word 
                    int phoneme_iterator = 0;
                    //time when phoneme changes 
                    DateTime next_phoneme_time = DateTime.Now;
                    //While playback is happening... 
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        //Bool speaking helps the algorithm recognize when no word is currently being said, so the mouth can be closed in that case 
                        bool Speaking = false;
                        foreach (dynamic word in words)
                        {
                            //Check if the current time is within the range of any word in the utterance. 
                            if (TimeSpanToFloat(audioFile.CurrentTime) > word.start && TimeSpanToFloat(audioFile.CurrentTime) < word.end)
                            {
                                //if so, a word is being spoken 
                                Speaking = true;
                                if (word != current_word)
                                {
                                    //if the word has changed, reset the phoneme counter and reset the time when the next phoneme is due 
                                    current_word = word;
                                    next_phoneme_time = DateTime.Now;
                                    phoneme_iterator = 0;
                                }
                                //if it is past the next phoneme time
                                if (DateTime.Now > next_phoneme_time)
                                {
                                    //set new phoneme time (eyeballed for now)
                                    next_phoneme_time = DateTime.Now.AddSeconds(((60 / 175) / (current_word.phonemes.Count + 1)));
                                    //translate current phoneme to motor positions
                                    MotorPositionsContainer positions = new ArpabetTranslator().getMouthPosition(current_word.phonemes[phoneme_iterator]);
                                    //prepare to emit motor position changed event (linked to the Animators "Motor Position Hook" 
                                    NewMotorPositionsEventArgs args = new NewMotorPositionsEventArgs();
                                    args.Positions = positions;
                                    NewMouthPositions.Invoke(this, args);
                                    //Iterate through possible phonemes 
                                    if (phoneme_iterator + 1 < current_word.phonemes.Count) { 
                                        phoneme_iterator += 1; 
                                    }

                                }

                            }
                        }
                        //if no word is spoken at the current time, return a standard position container. 
                        if (Speaking == false)
                        {
                         //   Console.WriteLine("NO WORD");
                            MotorPositionsContainer positions = new MotorPositionsContainer("standard_mouth");
                            //positions = new ArpabetTranslator().getMouthPosition("XX");
                            NewMotorPositionsEventArgs args = new NewMotorPositionsEventArgs();
                            args.Positions = positions;
                            NewMouthPositions.Invoke(this, args);
                        }
                      
                    }
                    //Finally, change the isPlaying variable back to false
                    isPlaying = false;
                    //and evoke the speech stopped event 
                    SpeechComplete.Invoke(this, new EventArgs());
                }
            }
        }
    }

    //Costum inheritor of Event args for conveying a MotorPositionsContainer Object. 
    public class NewMotorPositionsEventArgs : EventArgs
    {
        public MotorPositionsContainer Positions { get; set; }

    }

    class Word
    {
        public string word;
        public float start;
        public float end;
        public List<String> phonemes;
        public float conf;

        public Word(string word, float start, float end, List<string> phonemes, float conf)
        {
            this.word = word;
            this.start = start;
            this.end = end;
            this.phonemes = phonemes;
            this.conf = conf;
        }
    }
}
