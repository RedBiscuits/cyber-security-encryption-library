using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        bool[] curLPlain32 = new bool[32];
        bool[] curRPlain32 = new bool[32];
        bool[] curRPlain48 = new bool[48];
        bool[] binaryKey48 = new bool[48];
        bool[] nxtLPlain32 = new bool[32];



        int[,,] S_BoxArr = { { {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                          { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                          { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                          { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 } },

                        { {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                          { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                          { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                          { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 } },

                        { {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                          { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                          { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                          { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 } },

                        { {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                          { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                          {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                          {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14} },

                        { {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                          { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                          { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                          { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 } },

                        { {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                          { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                          {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                          {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13} },

                        { {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                          { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                          { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                          { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 } },

                        { {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                          { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                          {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                          {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11} }
        };



        static int[] expantionPermutation = {32,1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9,
                                             8 ,9 ,10,11,12,13,12,13,14,15,16,17,
                                             16,17,18,19,20,21,20,21,22,23,24,25,
                                             24,25,26,27,28,29,28,29,30,31,32, 1};

        int[] perAfterSBoxArray = { 16,  7, 20, 21, 29, 12, 28, 17,
                                     1, 15, 23, 26,  5, 18, 31, 10,
                                     2,  8, 24, 14, 32, 27,  3,  9,
                                    19, 13, 30,  6, 22, 11,  4, 25};


        public static string ToBinary(string val)
        {
            string result = Convert.ToString(Convert.ToInt32(val, 16), 2);
            int end = 4 - result.Length;

            if (result.Length < 4)
                for (int i = 0; i < end; i++)
                    result = result.Insert(0, "0");

            return result;
        }

        public static string Tohexa(string val)
        {
            string result = Convert.ToString(Convert.ToInt32(val, 2), 16);
            return result;
        }

        public string createbinary(string temp)
        {
            string binary = "";
            for (int i = 0; i < temp.Length; i++)
            {
                binary += ToBinary(temp[i].ToString());
            }
            return binary;
        }

        public static string ToDecimal(string val)
        {
            return Convert.ToString(Convert.ToInt32(val, 2), 10);
        }

        public string intialpermutation(string binary)
        {
            int[] intialpermutation = {
                         58, 50, 42, 34, 26, 18, 10, 2,
                         60, 52, 44, 36, 28, 20, 12, 4,
                         62, 54, 46, 38, 30, 22, 14, 6,
                         64, 56, 48, 40, 32, 24, 16, 8,
                         57, 49, 41, 33, 25, 17, 9,  1,
                         59, 51, 43, 35, 27, 19, 11, 3,
                         61, 53, 45, 37, 29, 21, 13, 5,
                         63, 55, 47, 39, 31, 23, 15, 7};
            string result = "";
            for (int i = 0; i < binary.Length; i++)
            {
                result += binary[intialpermutation[i] - 1];
            }
            return result;
        }

        public string[] permutedOne(string binary)
        {
            int[] permu_C = {57, 49, 41, 33, 25, 17, 9,
                             1, 58, 50, 42, 34, 26, 18,
                             10, 2, 59, 51, 43, 35, 27,
                             19, 11, 3, 60, 52, 44, 36};
            int[] permu_D = {63, 55, 47, 39, 31, 23, 15,
                             7, 62, 54, 46, 38, 30, 22,
                             14, 6, 61, 53, 45, 37, 29,
                             21, 13, 5, 28, 20, 12, 4};

            string c = "", d = "";
            for (int i = 0; i < 28; i++)
            {
                c += binary[permu_C[i] - 1];
                d += binary[permu_D[i] - 1];
            }
            string[] temp = new string[2];
            temp[0] += c;
            temp[1] += d;
            return temp;
        }


        public string[] LeftCircularShift(string[] C_D, int round)
        {
            // Left Circular Shift by One
            if (round == 1 || round == 2 || round == 9 || round == 16)
            {
                C_D[0] += C_D[0][0];
                C_D[0] = C_D[0].Remove(0, 1);
                C_D[1] += C_D[1][0];
                C_D[1] = C_D[1].Remove(0, 1);
            }
            // Left Circular Shift by Two
            else
            {
                C_D[0] += C_D[0][0];
                C_D[0] += C_D[0][1];
                C_D[0] = C_D[0].Remove(0, 2);
                C_D[1] += C_D[1][0];
                C_D[1] += C_D[1][1];
                C_D[1] = C_D[1].Remove(0, 2);
            }
            return C_D;
        }

        public string permutedTwo(string binary)
        {
            int[] permmatrix = {
                         14, 17, 11, 24,  1,  5,
                         3,  28, 15,  6, 21, 10,
                         23, 19, 12,  4, 26,  8,
                         16,  7, 27, 20, 13,  2,
                         41, 52, 31, 37, 47, 55,
                         30, 40, 51, 45, 33, 48,
                         44, 49, 39, 56, 34, 53,
                         46, 42, 50, 36, 29, 32};

            string result = "";
            for (int i = 0; i < 48; i++)
            {
                result += binary[permmatrix[i] - 1];
            }
            return result;
        }

        public void setBinaryPlainRL(string plain)
        {
            for (int i = 0; i < 32; i++)
            {
                curLPlain32[i] = (plain[i] == '1') ? true : false;
                curRPlain32[i] = (plain[i + 32] == '1') ? true : false;
            }
        }

        public void setBinarykey(string key)
        {
            for (int i = 0; i < 48; i++)
            {
                binaryKey48[i] = (key[i] == '1') ? true : false;
            }
        }

        public void setExpand48()
        {
            for (int i = 0; i < 48; i++)
                curRPlain48[i] = curRPlain32[expantionPermutation[i] - 1];
        }

        public void XOR(ref bool[] temp1, ref bool[] temp2)
        {
            for (int i = 0; i < temp1.Length; i++)
            {
                temp1[i] ^= temp2[i];
            }
        }

        public void SBox()
        {
            for (int i = 0; i < 8; i++)
            {
                int s_bit = i * 6;
                int e_bit = (i + 1) * 6 - 1;

                string r1 = curRPlain48[s_bit] ? "1" : "0";
                string r2 = curRPlain48[e_bit] ? "1" : "0";
                string row = r1 + r2;

                string column = "";
                string res = "";

                for (int j = s_bit + 1; j < e_bit; j++)
                    column += curRPlain48[j] ? "1" : "0";

                int r = Convert.ToInt32(ToDecimal(row));
                int c = Convert.ToInt32(ToDecimal(column));

                res = Convert.ToString(S_BoxArr[i, r, c], 2);
                res = new string('0', 4 - res.Length) + res;

                for (int j = 0; j < 4; j++)
                    curRPlain32[i * 4 + j] = res[j].Equals('1') ? true : false;
            }
        }


        public void PermutationAfterSBox()
        {
            bool[] temp = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                int idx = perAfterSBoxArray[i];
                temp[i] = curRPlain32[idx - 1];
            }
            for (int i = 0; i < 32; i++)
            {
                curRPlain32[i] = temp[i];
            }
        }


        public string InversPermutation(string cipher)
        {
            int[] arr = {40, 8, 48, 16, 56, 24, 64, 32,
                         39, 7, 47, 15, 55, 23, 63, 31,
                         38, 6, 46, 14, 54, 22, 62, 30,
                         37, 5, 45, 13, 53, 21, 61, 29,
                         36, 4, 44, 12, 52, 20, 60, 28,
                         35, 3, 43, 11, 51, 19, 59, 27,
                         34, 2, 42, 10, 50, 18, 58, 26,
                         33, 1, 41, 9, 49, 17, 57, 25};
            string temp = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                temp += cipher[arr[i] - 1];
            }
            return temp;
        }

        public string Concatinate()
        {
            string cipher = "";
            for (int i = 0; i < 32; i++)
            {
                cipher += curRPlain32[i] ? "1" : "0";
            }
            for (int i = 0; i < 32; i++)
            {
                cipher += curLPlain32[i] ? "1" : "0";
            }
            return cipher;
        }

        public string Final(string str)
        {
            string final = "";
            for (int i = 0; i < str.Length; i += 4)
            {
                final += Tohexa(str.Substring(i, 4));
            }
            final = final.ToUpper();
            final = final.Insert(0, "0x");
            return final;
        }



        public override string Decrypt(string cipherText, string key)
        {
            string plainbinary = "";
            string keybinary = "";
            string plain;

            string[] keysOfRounds = new string[16];
            cipherText = cipherText.Remove(0, 2);
            key = key.Remove(0, 2);

            keybinary = createbinary(key);
            plainbinary = intialpermutation(createbinary(cipherText));
            string[] C_D = permutedOne(keybinary);

            for (int round = 1; round <= 16; round++)
            {
                C_D = LeftCircularShift(C_D, round);
                key = C_D[0];
                key += C_D[1];
                key = permutedTwo(key);
                keysOfRounds[round - 1] = key;
            }

            setBinaryPlainRL(plainbinary);

            for (int round = 15; round >= 0; round--)
            {
                setBinarykey(keysOfRounds[round]);
                Array.Copy(curRPlain32, nxtLPlain32, 32);
                setExpand48();
                XOR(ref curRPlain48, ref binaryKey48);
                SBox();
                PermutationAfterSBox();
                XOR(ref curRPlain32, ref curLPlain32);
                Array.Copy(nxtLPlain32, curLPlain32, 32);
            }

            //plain = Concatinate();
            plain = InversPermutation(Concatinate());
            //Console.WriteLine(plain);

            return Final(plain);
        }




        public override string Encrypt(string plainText, string key)
        {
            plainText = plainText.Remove(0, 2);
            key = key.Remove(0, 2);

            string plainbinary = "";
            string keybinary = "";
            string cipher;

            plainbinary = intialpermutation(createbinary(plainText));
            keybinary = createbinary(key);

            string[] C_D = permutedOne(keybinary);

            setBinaryPlainRL(plainbinary);

            for (int round = 1; round <= 16; round++)
            {
                C_D = LeftCircularShift(C_D, round);

                key = C_D[0]; key += C_D[1];
                key = permutedTwo(key);
                setBinarykey(key);

                Array.Copy(curRPlain32, nxtLPlain32, 32);
                setExpand48();
                XOR(ref curRPlain48, ref binaryKey48);
                SBox();
                PermutationAfterSBox();
                XOR(ref curRPlain32, ref curLPlain32);
                Array.Copy(nxtLPlain32, curLPlain32, 32);
            }
            cipher = InversPermutation(Concatinate());
            return Final(cipher);

        }
    }
}