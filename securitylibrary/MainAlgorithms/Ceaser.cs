using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            string ENC = "";
            for (int i = 0; i < plainText.Length; i++){
                int var=plainText[i] -97;
                int index=(var+key) %26;
                ENC+= (char)(index +97);}
            return ENC;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            string DEC = "";
            for (int i = 0 ; i <cipherText.Length ; i++){
                int var =cipherText[i] - 65;
                int index =(var - key);
                if (index <0)
                    index += 26;
                DEC += (char)(index +97); }
            return DEC;
        }

        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            int EN = plainText[0] - 96;
            int DC = cipherText[0] - 64;
            int KEY = DC - EN;
            if (KEY <0)
                KEY +=26;
            return KEY;
        }
    }
}