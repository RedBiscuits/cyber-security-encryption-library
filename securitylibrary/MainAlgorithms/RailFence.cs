using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            char CHAR = cipherText[1];
            int KEY = 0;
            for (int i = 0; i < plainText.Length; i++) {
                if (plainText[i] == CHAR && plainText[i + 1] == CHAR) {
                    KEY = i + 1;
                    break; }
                else if (plainText[i] == CHAR) {
                    KEY = i;
                    break; } }
            return KEY;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            string PLAINTXT = "";
            int P = 1;
            key = (int)Math.Ceiling(cipherText.Length / (float)key);
            for (int i = 0, j = 0; j < cipherText.Length; i += key, j++) {
                if (i >= cipherText.Length) {
                    i = P++; }
                PLAINTXT += cipherText[i]; }
            return PLAINTXT;
        }

        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            string CIPHERTXT = "";
            int k = 1;
            for (int i = 0, j = 0; j < plainText.Length; i += key, j++) {
                if (i >= plainText.Length) {
                    i = k++; }
                CIPHERTXT += plainText[i]; }
            return CIPHERTXT;
        }
    }
}
