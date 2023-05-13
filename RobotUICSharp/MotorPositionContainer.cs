using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    [Serializable]
    public class MotorPositionsContainer
    {
        private int StandardValue = -1;

        public int LeftEyeHorizontalPosition { get; set; }
        public int LeftEyeVerticalPosition { get; set; }
        public int LeftEyelidUpperPosition { get; set; }
        public int LeftEyelidLowerPosition { get; set; }
        public int RightEyeHorizontalPosition { get; set; }
        public int RightEyeVerticalPosition { get; set; }
        public int RightEyelidUpperPosition { get; set; }
        public int RightEyelidLowerPosition { get; set; }
        public int UpperMouthCornerPosition { get; set; }
        public int UpperMouthCheekPosition { get; set; }
        public int UpperMouthCaninePosition { get; set; }
        public int UpperMouthMiddlePosition { get; set; }
        public int LowerMouthCornerPosition { get; set; }
        public int LowerMouthCheekPosition { get; set; }
        public int LowerMouthCaninePosition { get; set; }
        public int LowerMouthMiddlePosition { get; set; }
        public int JawPosition { get; set; }
        public int NeckRotationPosition { get; set; }
        public int NeckPitchPosition { get; set; }

        static MotorPositionsContainer standard = MotorPositionsContainer.ReadMotorPositionsFile(ArpabetTranslator.getMotorPositionFolderPath() + "/Standard.txt");


        public MotorPositionsContainer(String variant)
        {

            if (variant == "standard")
            {
                Console.WriteLine("STANDARD");
                this.LeftEyeHorizontalPosition = standard.LeftEyeHorizontalPosition;
                this.LeftEyeVerticalPosition = standard.LeftEyeVerticalPosition;
                this.LeftEyelidUpperPosition = standard.LeftEyelidUpperPosition;
                this.LeftEyelidLowerPosition = standard.LeftEyelidLowerPosition;
                this.RightEyeHorizontalPosition = standard.RightEyeHorizontalPosition;
                this.RightEyeVerticalPosition = standard.RightEyeVerticalPosition;
                this.RightEyelidUpperPosition = standard.RightEyelidUpperPosition;
                this.RightEyelidLowerPosition = standard.RightEyelidLowerPosition;
                this.UpperMouthCornerPosition = standard.UpperMouthCornerPosition;
                this.UpperMouthCheekPosition = standard.UpperMouthCheekPosition;
                this.UpperMouthCaninePosition = standard.UpperMouthCaninePosition;
                this.UpperMouthMiddlePosition = standard.UpperMouthMiddlePosition;
                this.LowerMouthCornerPosition = standard.LowerMouthCornerPosition;
                this.LowerMouthCheekPosition = standard.LowerMouthCheekPosition;
                this.LowerMouthCaninePosition = standard.LowerMouthCaninePosition;
                this.LowerMouthMiddlePosition = standard.LowerMouthMiddlePosition;
                this.JawPosition = standard.JawPosition;
                this.NeckRotationPosition = standard.NeckRotationPosition;
                this.NeckPitchPosition = standard.NeckPitchPosition;


            }
            else if (variant == "standard_mouth")
            {
                //Console.WriteLine("MOUTH");
                this.LeftEyeHorizontalPosition = -1;
                this.LeftEyeVerticalPosition = -1;
                this.LeftEyelidUpperPosition = -1;
                this.LeftEyelidLowerPosition = -1;
                this.RightEyeHorizontalPosition = -1;
                this.RightEyeVerticalPosition = -1;
                this.RightEyelidUpperPosition = -1;
                this.RightEyelidLowerPosition = -1;
                this.UpperMouthCornerPosition = standard.UpperMouthCornerPosition;
                this.UpperMouthCheekPosition = standard.UpperMouthCheekPosition;
                this.UpperMouthCaninePosition = standard.UpperMouthCaninePosition;
                this.UpperMouthMiddlePosition = standard.UpperMouthMiddlePosition;
                this.LowerMouthCornerPosition = standard.LowerMouthCornerPosition;
                this.LowerMouthCheekPosition = standard.LowerMouthCheekPosition;
                this.LowerMouthCaninePosition = standard.LowerMouthCaninePosition;
                this.LowerMouthMiddlePosition = standard.LowerMouthMiddlePosition;
                this.JawPosition = standard.JawPosition;
                this.NeckRotationPosition = -1;
                this.NeckPitchPosition = -1;
            }
            else if (variant == "standard_eyes")
            {
                Console.WriteLine("EYES");
                this.LeftEyeHorizontalPosition = standard.LeftEyeHorizontalPosition;
                this.LeftEyeVerticalPosition = standard.LeftEyeVerticalPosition;
                this.LeftEyelidUpperPosition = standard.LeftEyelidUpperPosition;
                this.LeftEyelidLowerPosition = standard.LeftEyelidLowerPosition;
                this.RightEyeHorizontalPosition = standard.RightEyeHorizontalPosition;
                this.RightEyeVerticalPosition = standard.RightEyeVerticalPosition;
                this.RightEyelidUpperPosition = standard.RightEyelidUpperPosition;
                this.RightEyelidLowerPosition = standard.RightEyelidLowerPosition;
                this.UpperMouthCornerPosition = -1;
                this.UpperMouthCheekPosition = -1;
                this.UpperMouthCaninePosition = -1;
                this.UpperMouthMiddlePosition = -1;
                this.LowerMouthCornerPosition = -1;
                this.LowerMouthCheekPosition = -1;
                this.LowerMouthCaninePosition = -1;
                this.LowerMouthMiddlePosition = -1;
                this.JawPosition = -1;
                this.NeckRotationPosition = -1;
                this.NeckPitchPosition = -1;


            }
            else 
            {

                this.LeftEyeHorizontalPosition = this.StandardValue;
                this.LeftEyeVerticalPosition = this.StandardValue;
                this.LeftEyelidUpperPosition = this.StandardValue;
                this.LeftEyelidLowerPosition = this.StandardValue;
                this.RightEyeHorizontalPosition = this.StandardValue;
                this.RightEyeVerticalPosition = this.StandardValue;
                this.RightEyelidUpperPosition = this.StandardValue;
                this.RightEyelidLowerPosition = this.StandardValue;
                this.UpperMouthCornerPosition = this.StandardValue;
                this.UpperMouthCheekPosition = this.StandardValue;
                this.UpperMouthCaninePosition = this.StandardValue;
                this.UpperMouthMiddlePosition = this.StandardValue;
                this.LowerMouthCornerPosition = this.StandardValue;
                this.LowerMouthCheekPosition = this.StandardValue;
                this.LowerMouthCaninePosition = this.StandardValue;
                this.LowerMouthMiddlePosition = this.StandardValue;
                this.JawPosition = this.StandardValue;
                this.NeckRotationPosition = this.StandardValue;
                this.NeckPitchPosition = this.StandardValue;
            }

        }
        public int mirrorMotorValue(int val, int mirrorAround)
        {

            return mirrorAround + (mirrorAround - val);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"LeftEyeHorizontalPosition: {LeftEyeHorizontalPosition}");
            sb.AppendLine($"LeftEyeVerticalPosition: {LeftEyeVerticalPosition}");
            sb.AppendLine($"LeftEyelidUpperPosition: {LeftEyelidUpperPosition}");
            sb.AppendLine($"LeftEyelidLowerPosition: {LeftEyelidLowerPosition}");
            sb.AppendLine($"RightEyeHorizontalPosition: {RightEyeHorizontalPosition}");
            sb.AppendLine($"RightEyeVerticalPosition: {RightEyeVerticalPosition}");
            sb.AppendLine($"RightEyelidUpperPosition: {RightEyelidUpperPosition}");
            sb.AppendLine($"RightEyelidLowerPosition: {RightEyelidLowerPosition}");
            sb.AppendLine($"UpperMouthCornerPosition: {UpperMouthCornerPosition}");
            sb.AppendLine($"UpperMouthCheekPosition: {UpperMouthCheekPosition}");
            sb.AppendLine($"UpperMouthCaninePosition: {UpperMouthCaninePosition}");
            sb.AppendLine($"UpperMouthMiddlePosition: {UpperMouthMiddlePosition}");
            sb.AppendLine($"LowerMouthCornerPosition: {LowerMouthCornerPosition}");
            sb.AppendLine($"LowerMouthCheekPosition: {LowerMouthCheekPosition}");
            sb.AppendLine($"LowerMouthCaninePosition: {LowerMouthCaninePosition}");
            sb.AppendLine($"LowerMouthMiddlePosition: {LowerMouthMiddlePosition}");
            sb.AppendLine($"JawPosition: {JawPosition}");
            sb.AppendLine($"NeckRotationPosition: {NeckRotationPosition}");
            sb.AppendLine($"NeckPitchPosition: {NeckPitchPosition}");
            return sb.ToString();
        }


        public static void WriteMotorPositionsFile<MotorPositionsContainer>(string filePath, MotorPositionsContainer PositionsToWrite) 
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(PositionsToWrite);
                writer = new StreamWriter(filePath);
                writer.Write(contentsToWriteToFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write Motor File Error:", ex.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        public static MotorPositionsContainer ReadMotorPositionsFile(string filePath) 
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<MotorPositionsContainer>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }


}
