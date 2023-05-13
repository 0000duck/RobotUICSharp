using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotUICSharp
{
    //This class translates a phoneme into mouth and jaw positions 
    public class ArpabetTranslator
    {
        //Contains all arpabet phonemes (for testing)
        static public string[] arpabetPhonemes = {
            "AA", "AE", "AH", "AO", "AW", "AY", "B", "CH", "D", "DH", "EH",
            "ER", "EY", "F", "G", "HH", "IH", "IY", "JH", "K", "L", "M",
            "N", "NG", "OW", "OY", "P", "R", "S", "SH", "T", "TH", "UH",
            "UW", "V", "W", "Y", "Z", "ZH"
        };

        public static string getMotorPositionFolderPath()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString()).ToString() + "/MotorPositions";
        }


        //Receives an Arpabet-Phoneme as a String, returns a MotorPositionsContainer representing jaw and lip movement. 
        public virtual MotorPositionsContainer getMouthPosition(String phoneme)
        {
            MotorPositionsContainer motorpos;
            //filter out stresses etc. which are signified by a number after the phoneme
            if (phoneme.Length == 3)
            {
                phoneme = phoneme.Substring(0, 2);
            }
            //bAlm
            if (phoneme == "AA")
            {
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 93;
                motorpos.UpperMouthCaninePosition = 90;
                motorpos.UpperMouthMiddlePosition = 90;
                motorpos.LowerMouthCaninePosition = 70;
                motorpos.LowerMouthMiddlePosition = 70;
                motorpos.LowerMouthCheekPosition = 73;
                motorpos.UpperMouthCheekPosition = 87;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/AA.txt");
            }
            else if (phoneme == "AE")
            {
                //bAt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 93;
                motorpos.UpperMouthCaninePosition = 90;
                motorpos.UpperMouthMiddlePosition = 90;
                motorpos.LowerMouthCaninePosition = 70;
                motorpos.LowerMouthMiddlePosition = 70;
                motorpos.LowerMouthCheekPosition = 70;
                motorpos.UpperMouthCheekPosition = 90;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/AE.txt");
            }
            else if (phoneme == "AH")
            {
                //bUtt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 93;
                motorpos.UpperMouthCaninePosition = 86;
                motorpos.UpperMouthMiddlePosition = 88;
                motorpos.LowerMouthCaninePosition = 74;
                motorpos.LowerMouthMiddlePosition = 74;
                motorpos.LowerMouthCheekPosition = 74;
                motorpos.UpperMouthCheekPosition = 86;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/AH.txt");
            }
            else if (phoneme == "AO")
            {
                //cAUght
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 93;
                motorpos.UpperMouthCaninePosition = 88;
                motorpos.UpperMouthMiddlePosition = 88;
                motorpos.LowerMouthCaninePosition = 72;
                motorpos.LowerMouthMiddlePosition = 72;
                motorpos.LowerMouthCheekPosition = 72;
                motorpos.UpperMouthCheekPosition = 88;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/AO.txt");
            }
            else if (phoneme == "AW")
            {
                //bOUt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 99;
                motorpos.UpperMouthCaninePosition = 90;
                motorpos.UpperMouthMiddlePosition = 90;
                motorpos.LowerMouthCaninePosition = 70;
                motorpos.LowerMouthMiddlePosition = 70;
                motorpos.LowerMouthCheekPosition = 80;
                motorpos.UpperMouthCheekPosition = 84;
                motorpos.LowerMouthCornerPosition = 85;
                motorpos.UpperMouthCornerPosition = 85;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/AW.txt");
            }
            else if (phoneme == "AO")
            {
                //commA
                return this.getMouthPosition("AW");
            }
            else if (phoneme == "AY")
            {
                //bIte
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 96;
                motorpos.UpperMouthCaninePosition = 82;
                motorpos.UpperMouthMiddlePosition = 82;
                motorpos.LowerMouthCaninePosition = 73;
                motorpos.LowerMouthMiddlePosition = 73;
                motorpos.LowerMouthCheekPosition = 73;
                motorpos.UpperMouthCheekPosition = 80;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/AY.txt");
            }
            else if (phoneme == "EH")
            {
                //bEt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 97;
                motorpos.UpperMouthCaninePosition = 88;
                motorpos.UpperMouthMiddlePosition = 88;
                motorpos.LowerMouthCaninePosition = 72;
                motorpos.LowerMouthMiddlePosition = 72;
                motorpos.LowerMouthCheekPosition = 72;
                motorpos.UpperMouthCheekPosition = 88;
                motorpos.LowerMouthCornerPosition = 85;
                motorpos.UpperMouthCornerPosition = 85;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/EH.txt");
            }
            else if (phoneme == "ER")
            {
                //bIRd, wORd
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 98;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 84;
                motorpos.LowerMouthCaninePosition = 76;
                motorpos.LowerMouthMiddlePosition = 76;
                motorpos.LowerMouthCheekPosition = 78;
                motorpos.UpperMouthCheekPosition = 82;
                motorpos.LowerMouthCornerPosition = 85;
                motorpos.UpperMouthCornerPosition = 85;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/ER.txt");
            }
            else if (phoneme == "EY")
            {
                //bAIt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 97;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 84;
                motorpos.LowerMouthCaninePosition = 76;
                motorpos.LowerMouthMiddlePosition = 76;
                motorpos.LowerMouthCheekPosition = 76;
                motorpos.UpperMouthCheekPosition = 84;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/EY.txt");
            }
            else if (phoneme == "IH")
            {
                //bIt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 98;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 84;
                motorpos.LowerMouthCaninePosition = 76;
                motorpos.LowerMouthMiddlePosition = 76;
                motorpos.LowerMouthCheekPosition = 78;
                motorpos.UpperMouthCheekPosition = 82;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/IH.txt");
            }
            else if (phoneme == "IX")
            {
                //rosEs, rabbIt - similar position to bAIt
                return this.getMouthPosition("AE");
            }
            else if (phoneme == "IY")
            {
                //bEAt
                return this.getMouthPosition("IH");
            }
            else if (phoneme == "OW")
            {
                //bOAt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 98;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 84;
                motorpos.LowerMouthCaninePosition = 76;
                motorpos.LowerMouthMiddlePosition = 76;
                motorpos.LowerMouthCheekPosition = 76;
                motorpos.UpperMouthCheekPosition = 84;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/OW.txt");
            }
            else if (phoneme == "OY")
            {
                //bOY
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 98;
                motorpos.UpperMouthCaninePosition = 89;
                motorpos.UpperMouthMiddlePosition = 89;
                motorpos.LowerMouthCaninePosition = 71;
                motorpos.LowerMouthMiddlePosition = 71;
                motorpos.LowerMouthCheekPosition = 73;
                motorpos.UpperMouthCheekPosition = 86;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/OY.txt");
                
            }
            else if (phoneme == "UH")
            {
                //bOOk
                return this.getMouthPosition("AE");
            }
            else if (phoneme == "UW")
            {
                //bOOt
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 98;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 82;
                motorpos.LowerMouthCaninePosition = 80;
                motorpos.LowerMouthMiddlePosition = 80;
                motorpos.LowerMouthCheekPosition = 80;
                motorpos.UpperMouthCheekPosition = 80;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/UW.txt");
            }
            else if (phoneme == "UX")
            {
                //dUde ??? 
                return this.getMouthPosition("UW");
            }
            else if (phoneme == "B")
            {
                // CONSONANTS
                //Buy (Pot and MoM = same)
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 95;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 84;
                motorpos.LowerMouthCaninePosition = 80;
                motorpos.LowerMouthMiddlePosition = 80;
                motorpos.LowerMouthCheekPosition = 80;
                motorpos.UpperMouthCheekPosition = 84;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/B.txt");
            }
            else if (phoneme == "CH")
            {
                //CHina (SHut, Jump, meaSure = same)
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 97;
                motorpos.UpperMouthCaninePosition = 87;
                motorpos.UpperMouthMiddlePosition = 87;
                motorpos.LowerMouthCaninePosition = 73;
                motorpos.LowerMouthMiddlePosition = 73;
                motorpos.LowerMouthCheekPosition = 80;
                motorpos.UpperMouthCheekPosition = 82;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/CH.txt");
            }
            else if (phoneme == "D")
            {
                //Die (Top Nod = same, THick, THat = same w/o tongue)
 
                return this.getMouthPosition("B");
            }
            else if (phoneme == "DH")
            {
                //THy
                return this.getMouthPosition("D");
            }
            else if (phoneme == "DX")
            {
                //buTTer
                return this.getMouthPosition("D");
            }
            else if (phoneme == "EL")
            {
                //bottEL
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 99;
                motorpos.UpperMouthCaninePosition = 84;
                motorpos.UpperMouthMiddlePosition = 84;
                motorpos.LowerMouthCaninePosition = 73;
                motorpos.LowerMouthMiddlePosition = 73;
                motorpos.LowerMouthCheekPosition = 75;
                motorpos.UpperMouthCheekPosition = 82;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/EL.txt");
            }
            else if (phoneme == "EM")
            {
                //rythM
                return this.getMouthPosition("B");
            }
            else if (phoneme == "EN")
            {
                //buttoN
                return this.getMouthPosition("D");
            }
            else if (phoneme == "F")
            {
                //Fog, same as Vat
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 96;
                motorpos.UpperMouthCaninePosition = 88;
                motorpos.UpperMouthMiddlePosition = 86;
                motorpos.LowerMouthCaninePosition = 80;
                motorpos.LowerMouthMiddlePosition = 80;
                motorpos.LowerMouthCheekPosition = 80;
                motorpos.UpperMouthCheekPosition = 82;
                motorpos.LowerMouthCornerPosition = 80;
                motorpos.UpperMouthCornerPosition = 80;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/F.txt");
            }
            else if (phoneme == "G")
            {
                //Guy
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/G.txt");
            }
            else if (phoneme == "HH")
            {
                //High
                return this.getMouthPosition("D");
            }
            else if (phoneme == "JH")
            {
                //Jive
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/JH.txt");
            }
            else if (phoneme == "K")
            {
                //Kite

                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/K.txt");
            }
            else if (phoneme == "L")
            {
                //Lie
                return this.getMouthPosition("EL");
            }
            else if (phoneme == "M")
            {
                //My
                return this.getMouthPosition("B");
            }
            else if (phoneme == "N")
            {
                //My
                return this.getMouthPosition("D");
            }
            else if (phoneme == "NG")
            {
                //siNG
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/NG.txt");
            }
            else if (phoneme == "P")
            {
                //Pot
                return this.getMouthPosition("B");
            }
            else if (phoneme == "R")
            {
                //Rat
                motorpos = new MotorPositionsContainer("empty");
                motorpos.JawPosition = 99;
                motorpos.UpperMouthCaninePosition = 90;
                motorpos.UpperMouthMiddlePosition = 88;
                motorpos.LowerMouthCaninePosition = 72;
                motorpos.LowerMouthMiddlePosition = 72;
                motorpos.LowerMouthCheekPosition = 80;
                motorpos.UpperMouthCheekPosition = 80;
                motorpos.LowerMouthCornerPosition = 83;
                motorpos.UpperMouthCornerPosition = 83;
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/R.txt");
            }
            else if (phoneme == "S")
            {
                //Sat
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/S.txt");
            }
            else if (phoneme == "SH")
            {
                //SHy
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/SH.txt");
            }
            else if (phoneme == "T")
            {
                //Tie
                return this.getMouthPosition("D");
            }
            else if (phoneme == "TH")
            {
                //THigh
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/TH.txt");
            }
            else if (phoneme == "V")
            {
                //Via
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/V.txt");
            }
            else if (phoneme == "W")
            {
                //Wise
                return MotorPositionsContainer.ReadMotorPositionsFile(getMotorPositionFolderPath() + "/W.txt");
            }
            else if (phoneme == "WH")
            {
                //WHy
                return this.getMouthPosition("W");
            }
            else if (phoneme == "Y")
            {
                //Yacht, You
                return this.getMouthPosition("IY");
            }
            else if (phoneme == "Z")
            {
                //Zoo
                return this.getMouthPosition("S");
            }
            else if (phoneme == "ZH")
            {
                //pleaSure
                return this.getMouthPosition("CH");
            }
            else
            {
                //Return Standard Mouth Positions if no Arpabet-Phoneme matched the input - the mouth is closed but eyes and neck arent affected 
                motorpos = new MotorPositionsContainer("standard_mouth");

                return motorpos;
            }
        }
    }
}
