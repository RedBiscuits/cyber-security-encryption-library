using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            int COL_NUM = 0, flag = 0;
            for (int i = 0; i < plainText.Length; i++) {
                if (cipherText[0] == plainText[i]) {
                    for (int j = i + 1; j < cipherText.Length; j++) {
                        if (cipherText[1] == plainText[j]) {
                            for (int k = j + 1; k < cipherText.Length; k++) {
                                if (cipherText[2] == plainText[k]) {
                                    if (k - j == j - i) {
                                        COL_NUM = k - j;
                                        flag = 1;
                                        break; } }
                                else if (k - j > j - i)
                                    break; } }
                        if (flag == 1)
                            break; } }
                if (flag == 1)
                    break; }
            List<int> key = new List<int>(COL_NUM);
            int NUM_RAW = (int)Math.Ceiling(plainText.Length / (float)COL_NUM), PTR = 0;
            char[,] cip = new char[NUM_RAW, COL_NUM];
            for (int i = 0; i < NUM_RAW; i++) {
                for (int j = 0; j < COL_NUM; j++) {
                    if (PTR >= plainText.Length)
                        cip[i, j] = 'x';
                    else
                        cip[i, j] = plainText[PTR++]; } }
            int POINTER = 0, COUNTER = 2;
            for (int i = 0; i < COL_NUM; i++) {
                POINTER = flag = 0;
                COUNTER = 2;
                for (int j = 0; j < NUM_RAW; j++) {
                    if ((POINTER >= cipherText.Length || cip[j, i] == cipherText[POINTER])) {
                        flag++;
                        if (flag >= NUM_RAW)
                        { key.Add((int)Math.Ceiling(POINTER / (float)NUM_RAW)); break; }
                        POINTER++; }
                    else {
                        j = -1;
                        POINTER = COUNTER++ * NUM_RAW - NUM_RAW; } } }
            return key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            //throw new NotImplementedException();
            int COL_NUM = cipherText.Length / key.Count;
            if (COL_NUM * key.Count != cipherText.Length)
                return " ";
            else
            {
                char[,] cip = new char[key.Count, COL_NUM];
                int PTR = 0;
                string PLAINTXT = "";

                int i = 0;
                do
                {
                    int j = 0;
                    do
                    {
                        cip[i, j] = cipherText[PTR++];
                        j++;
                    } while (j < COL_NUM);
                    i++;
                } while (i < key.Count);

                i = 0;
                do
                {
                    int j = 0;
                    do
                    {
                        PLAINTXT += cip[key[j] - 1, i];
                        j++;
                    } while (j < key.Count);
                    i++;
                } while (i < COL_NUM);

                return PLAINTXT;
            }
            }

        public string Encrypt(string plainText, List<int> key)
        {
            //throw new NotImplementedException();
            string CIPHERTXT = "";
            int COL_NUM = (int)Math.Ceiling(plainText.Length / (float)key.Count);
            char[,] cip = new char[key.Count, COL_NUM];
            int PTR = 0, STR = 1;
            for (int i = 0; i < key.Count; i++) {
                int j = 0;
                do {
                    cip[key[i] - 1, j] = plainText[PTR];
                    PTR += key.Count;
                    if (PTR >= plainText.Length)
                        break;
                    j++;
                } while (j < COL_NUM);
                PTR = STR++;
            }
            for (int i = 0; i < key.Count; i++) {
                for (int j = 0; j < COL_NUM; j++) {
                    CIPHERTXT += cip[i, j]; }
            }
            return CIPHERTXT;
        }
    }
}
