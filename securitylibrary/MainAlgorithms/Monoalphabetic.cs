using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            bool[] FREQUENCY = new bool[26];
            for (int i = 0; i < FREQUENCY.Length; i++) { 
                 FREQUENCY[i] = false; }
            string CipherToLow = cipherText.ToLower();
            char[] PLTXTarr = plainText.ToCharArray();
            char[] CIPHERTXTarr = CipherToLow.ToCharArray();
            char[] KEYarr = new char[26];
            for (int i = 0; i < plainText.Length; i++) {
                KEYarr[PLTXTarr[i] - 97] = CIPHERTXTarr[i];
                FREQUENCY[CIPHERTXTarr[i] - 97] = true; }
            for (int i = 0; i < KEYarr.Length; i++) {
                if (KEYarr[i] == '\0') {
                    for (int j = 0; j < FREQUENCY.Length; j++) {
                        if (!FREQUENCY[j]){
                            KEYarr[i] = (char)(j + 97);
                            FREQUENCY[j] = true;
                            break; } } } }
            return new string(KEYarr);
        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            string CIPHERToLow = cipherText.ToLower();
            char[] CIIPHERTXTarr = CIPHERToLow.ToCharArray();
            char[] KEYarr = key.ToCharArray();
            char[] PLTXTarr = new char[cipherText.Length];
            char VAR = '0';
            char OUT = 'a';
            for (int i = 0; i < CIIPHERTXTarr.Length; i++) {
                VAR = CIIPHERTXTarr[i];
                int counter = 0;
                for (int j = 0; j < key.Length; j++) {
                    if (VAR == KEYarr[j])
                        break;
                    else
                        counter++; }
                PLTXTarr[i] = (char)((int)OUT + counter); }
            return new string(PLTXTarr);
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            char[] KEYarr = key.ToCharArray();
            char[] PLTXTarr = plainText.ToCharArray();
            char[] CIPHERTXTarr = new char[plainText.Length];
            char VAR = '0';
            int INDX;
            for (int i = 0; i < PLTXTarr.Length; i++) {
                VAR = PLTXTarr[i];
                INDX = (VAR - 97) % 26;
                CIPHERTXTarr[i] = KEYarr[INDX]; }
            return new string(CIPHERTXTarr);
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	=
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 

        public string AnalyseUsingCharFrequency(string cipher)
        {
            //throw new NotImplementedException();
            string PLTXT = "";
            string KEY = "etaoinsrhldcumfpgwybvkxjqz";
            string CIPHERToLow = cipher.ToLower();
            char[] KEYarr = KEY.ToCharArray();
            char[] CIPHERTXTarr = CIPHERToLow.ToCharArray();
            char[] MAParr = new char[26];
            int[]  FREQarr = new int[26];

            for (int i = 0; i < FREQarr.Length; i++) {
                FREQarr[i] = 0; }
            for (int i = 0; i < cipher.Length; i++) {
                FREQarr[CIPHERTXTarr[i] - 97]++; }
            for (int i = 0; i < FREQarr.Length; i++) {
                int X = 0;
                int P = 0;
                for (int j = 0; j < FREQarr.Length; j++) {
                    if (X < FREQarr[j]){
                        X = FREQarr[j];
                        P = j; } }
                FREQarr[P] = 0;
                MAParr[P] = KEY[i]; }
            for (int i = 0; i < cipher.Length; i++) {
                PLTXT += MAParr[CIPHERTXTarr[i] - 97]; }
            return PLTXT;
        }
    }
}