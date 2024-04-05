using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            
            key = key.ToUpper();
            cipherText = cipherText.ToUpper();
            string[,] MixcolumnsState = new string[4, 4];
            string[] Keys = new string[11];

            Keys[0] = key;
            int i = 0;
            do
            {
                Keys[i + 1] = convertString(Generatekey(Keys[i], i));
                i++;
            } while (i < 10);

            key = Keys[10];
            cipherText = AddRoundkey(cipherText, key);
            cipherText = shiftrowsInv(cipherText);
            cipherText = InvSubstituteByteMatrix(cipherText);

            i = 9;
            do
            {
                cipherText = AddRoundkey(cipherText, Keys[i]);
                MixcolumnsState = InvMixcolumns(cipherText);
                cipherText = shiftrowsInv(convertString(MixcolumnsState));
                cipherText = InvSubstituteByteMatrix(cipherText);
                i--;
            } while (i > 0);

            cipherText = AddRoundkey(cipherText, Keys[0]);

            return cipherText;

        }

        public override string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            key = key.ToUpper();
            plainText = plainText.ToUpper();
            string state;
            string[,] MixcolumnsState = new string[4, 4];


            state = AddRoundkey(plainText, key);
            int i = 0;
            do
            {
                state = SubstituteByteMatrix(state);
                state = shiftrows(state);
                MixcolumnsState = Mixcolumns(state);
                key = convertString(Generatekey(key, i));
                state = AddRoundkey(convertString(MixcolumnsState), key);
                i++;
            } while (i < 9);
            state = SubstituteByteMatrix(state);
            state = shiftrows(state);
            key = convertString(Generatekey(key, 9));
            state = AddRoundkey(state, key);
            return state;
        }

        public string[,] Generatekey(string key, int roundNumber)
        {
            string[,] keyMat = Convert2D(key);
            string[,] newKey = new string[4, 4];
            string[] vector3 = new string[4];
            string[,] RCON = new string[4, 10] {

        {  "01", "02", "04", "08", "10", "20", "40", "80", "1b", "36"},
        {  "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {  "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {  "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"}
            };

            for (int i = 0; i < 4; i++)
                vector3[i] = keyMat[i, 3];
            string temp = "";

            temp = vector3[3];
            vector3[3] = vector3[0];
            vector3[0] = vector3[1];
            vector3[1] = vector3[2];
            vector3[2] = temp;
            temp = "0x";

            for (int i = 0; i < 4; i++)
                temp += vector3[i];
            temp = SubstituteByteVector(temp);

            int k = 2;
            for (int i = 0; i < 4; i++)
            {
                vector3[i] = temp.Substring(k, 2);
                k += 2;
            }
            int vector3dec, keyDec, rconDec, temp1;
            for (int i = 0; i < 4; i++)
            {
                vector3dec = Convert.ToInt32(vector3[i], 16);
                keyDec = Convert.ToInt32(keyMat[i, 0], 16);
                rconDec = Convert.ToInt32(RCON[i, roundNumber], 16);
                temp1 = vector3dec ^ keyDec ^ rconDec;
                newKey[i, 0] = IntHex(temp1);

            }

            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    keyDec = Convert.ToInt32(keyMat[j, i], 16);
                    vector3dec = Convert.ToInt32(newKey[j, i - 1], 16);
                    temp1 = vector3dec ^ keyDec;
                    newKey[j, i] = IntHex(temp1);

                }
            }


            return newKey;
        }

        public string AddRoundkey(string plainText, string key)
        {
            string hexResult = "0x";
            for (int i = 2; i < 34; i = i + 2)
            {
                int partPT = Convert.ToInt32(plainText.Substring(i, 2), 16);
                int partKey = Convert.ToInt32(key.Substring(i, 2), 16);
                int result = partPT ^ partKey;
                if (result < 16)
                    hexResult += "0";
                hexResult += result.ToString("X");

            }
            return hexResult;
        }
        public string SubstituteByteMatrix(string state)
        {
            int column;
            int row;
            string part;
            string[,] Sbox = BuildSbox();
            string result = "0x";
            for (int i = 2; i < 34; i += 2)
            {

                row = HexInt(state[i]);
                column = HexInt(state[i + 1]);
                part = Sbox[row, column].Substring(2).ToUpper();
                result += part;

            }
            return result;
        }
        public string SubstituteByteVector(string state)
        {
            int column;
            int row;
            string part;
            string[,] Sbox = BuildSbox();
            string result = "0x";

            for (int i = 2; i < 10; i += 2)
            {

                row = HexInt(state[i]);
                column = HexInt(state[i + 1]);
                part = Sbox[row, column].Substring(2).ToUpper();
                result += part;

            }
            return result;
        }
        public string[,] Mixcolumns(string state)
        {
            string[,] statMat = Convert2D(state);
            int[,] GivenMatrix = new int[4, 4]{
                {2,3,1,1 },
                {1,2,3,1 },
                {1,1,2,3 },
                {3,1,1,2 }
            };
            string[,] result = new string[4, 4];
            int val;
            int resultTemp = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        val = Convert.ToInt32(statMat[j, k], 16);
                        if (GivenMatrix[i, j] == 1)
                        {
                            resultTemp ^= val;
                            continue;
                        }

                        val <<= 1;
                        if ((val & 256) != 0)
                        {
                            val -= 256;
                            val ^= 27;
                        }
                        if (GivenMatrix[i, j] == 3)
                            val ^= Convert.ToInt32(statMat[j, k], 16);

                        resultTemp ^= val;

                    }
                    //  string x = IntHex(resultTemp);
                    result[i, k] = IntHex(resultTemp);
                    resultTemp = 0;
                }

            }
            return result;
        }
        public string shiftrows(string state)
        {
            string result = "0x";
            result += state.Substring(2, 2);
            result += state.Substring(12, 2);
            result += state.Substring(22, 2);
            result += state.Substring(32, 2);

            result += state.Substring(10, 2);
            result += state.Substring(20, 2);
            result += state.Substring(30, 2);
            result += state.Substring(8, 2);

            result += state.Substring(18, 2);
            result += state.Substring(28, 2);
            result += state.Substring(6, 2);
            result += state.Substring(16, 2);

            result += state.Substring(26, 2);
            result += state.Substring(4, 2);
            result += state.Substring(14, 2);
            result += state.Substring(24, 2);
            return result;
        }

        public string[,] Convert2D(string state)
        {
            string[,] statMat = new string[4, 4];
            int k = 2;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    statMat[j, i] = state.Substring(k, 2);
                    k += 2;
                }

            return statMat;
        }
        public string[,] BuildSbox()
        {
            string[,] Sbox = new string[16, 16] {  // populate the Sbox matrix
    /*      0     1     2     3     4     5     6     7     8     9     a     b     c     d     e     f */
    /*0*/  {"0x63","0x7c","0x77","0x7b","0xf2","0x6b","0x6f","0xc5","0x30","0x01","0x67","0x2b","0xfe","0xd7","0xab","0x76"},
    /*1*/  {"0xca","0x82","0xc9","0x7d","0xfa","0x59","0x47","0xf0","0xad","0xd4","0xa2","0xaf","0x9c","0xa4","0x72","0xc0" },
    /*2*/  {"0xb7","0xfd","0x93","0x26","0x36","0x3f","0xf7","0xcc","0x34","0xa5","0xe5","0xf1","0x71","0xd8","0x31","0x15" },
    /*3*/  {"0x04","0xc7","0x23","0xc3","0x18","0x96","0x05","0x9a","0x07","0x12","0x80","0xe2","0xeb","0x27","0xb2","0x75" },
    /*4*/  {"0x09","0x83","0x2c","0x1a","0x1b","0x6e","0x5a","0xa0","0x52","0x3b","0xd6","0xb3","0x29","0xe3","0x2f","0x84" },
    /*5*/  {"0x53","0xd1","0x00","0xed","0x20","0xfc","0xb1","0x5b","0x6a","0xcb","0xbe","0x39","0x4a","0x4c","0x58","0xcf" },
    /*6*/  {"0xd0","0xef","0xaa","0xfb","0x43","0x4d","0x33","0x85","0x45","0xf9","0x02","0x7f","0x50","0x3c","0x9f","0xa8" },
    /*7*/  {"0x51","0xa3","0x40","0x8f","0x92","0x9d","0x38","0xf5","0xbc","0xb6","0xda","0x21","0x10","0xff","0xf3","0xd2" },
    /*8*/  {"0xcd","0x0c","0x13","0xec","0x5f","0x97","0x44","0x17","0xc4","0xa7","0x7e","0x3d","0x64","0x5d","0x19","0x73" },
    /*9*/  {"0x60","0x81","0x4f","0xdc","0x22","0x2a","0x90","0x88","0x46","0xee","0xb8","0x14","0xde","0x5e","0x0b","0xdb" },
    /*a*/  {"0xe0","0x32","0x3a","0x0a","0x49","0x06","0x24","0x5c","0xc2","0xd3","0xac","0x62","0x91","0x95","0xe4","0x79" },
    /*b*/  {"0xe7","0xc8","0x37","0x6d","0x8d","0xd5","0x4e","0xa9","0x6c","0x56","0xf4","0xea","0x65","0x7a","0xae","0x08" },
    /*c*/  {"0xba","0x78","0x25","0x2e","0x1c","0xa6","0xb4","0xc6","0xe8","0xdd","0x74","0x1f","0x4b","0xbd","0x8b","0x8a" },
    /*d*/  {"0x70","0x3e","0xb5","0x66","0x48","0x03","0xf6","0x0e","0x61","0x35","0x57","0xb9","0x86","0xc1","0x1d","0x9e" },
    /*e*/  {"0xe1","0xf8","0x98","0x11","0x69","0xd9","0x8e","0x94","0x9b","0x1e","0x87","0xe9","0xce","0x55","0x28","0xdf" },
    /*f*/  {"0x8c","0xa1","0x89","0x0d","0xbf","0xe6","0x42","0x68","0x41","0x99","0x2d","0x0f","0xb0","0x54","0xbb","0x16" } };
            return Sbox;
        }
        public int HexInt(char x)
        {
            if (x <= '9')
                return Convert.ToInt32(x - '0');
            else
                return Convert.ToInt32(x - 'A' + 10);
        }
        public string IntHex(int decimalNumber)
        {
            int quotient;
            int temp = 0;
            bool ReverseFlag = true;
            string hexadecimalNumber = "";
            char temp1;
            quotient = decimalNumber;

            if (quotient == 0)
            {
                hexadecimalNumber += "00";
                ReverseFlag = false;
            }
            else if (quotient < 16)
            {
                hexadecimalNumber += "0";
                ReverseFlag = false;
            }
            while (quotient != 0)
            {

                temp = quotient % 16;
                if (temp < 10)

                    temp = temp + 48;
                else
                    temp = temp + 55;

                temp1 = Convert.ToChar(temp);
                hexadecimalNumber += temp1;
                quotient = quotient / 16;
            }
            if (ReverseFlag == true)
            {
                char[] array = hexadecimalNumber.ToCharArray();
                Array.Reverse(array);
                return new string(array);
            }
            return hexadecimalNumber;
        }
        public string convertString(string[,] Matrix)
        {
            string st = "0x";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    st += Matrix[j, i];
            }
            return st;
        }


        public string InvSubstituteByteMatrix(string state)
        {
            int column;
            int row;
            string part;
            string[,] invSbox = BuildInvSbox();
            string result = "0x";

            for (int i = 2; i < 34; i += 2)
            {

                row = HexInt(state[i]);
                column = HexInt(state[i + 1]);
                part = invSbox[row, column].Substring(2).ToUpper();
                result += part;

            }
            return result;
        }
        public string[,] BuildInvSbox()
        {
            string[,] iSbox = new string[16, 16] {  
    /* 0     1     2     3     4     5     6     7     8     9     a     b     c     d     e     f */
    /*0*/  {"0x52", "0x09", "0x6a", "0xd5", "0x30", "0x36", "0xa5", "0x38", "0xbf", "0x40", "0xa3", "0x9e", "0x81", "0xf3", "0xd7", "0xfb" },
    /*1*/  {"0x7c", "0xe3", "0x39", "0x82", "0x9b", "0x2f", "0xff", "0x87", "0x34", "0x8e", "0x43", "0x44", "0xc4", "0xde", "0xe9", "0xcb" },
    /*2*/  {"0x54", "0x7b", "0x94", "0x32", "0xa6", "0xc2", "0x23", "0x3d", "0xee", "0x4c", "0x95", "0x0b", "0x42", "0xfa", "0xc3", "0x4e" },
    /*3*/  {"0x08", "0x2e", "0xa1", "0x66", "0x28", "0xd9", "0x24", "0xb2", "0x76", "0x5b", "0xa2", "0x49", "0x6d", "0x8b", "0xd1", "0x25" },
    /*4*/  {"0x72", "0xf8", "0xf6", "0x64", "0x86", "0x68", "0x98", "0x16", "0xd4", "0xa4", "0x5c", "0xcc", "0x5d", "0x65", "0xb6", "0x92" },
    /*5*/  {"0x6c", "0x70", "0x48", "0x50", "0xfd", "0xed", "0xb9", "0xda", "0x5e", "0x15", "0x46", "0x57", "0xa7", "0x8d", "0x9d", "0x84" },
    /*6*/  {"0x90", "0xd8", "0xab", "0x00", "0x8c", "0xbc", "0xd3", "0x0a", "0xf7", "0xe4", "0x58", "0x05", "0xb8", "0xb3", "0x45", "0x06" },
    /*7*/  {"0xd0", "0x2c", "0x1e", "0x8f", "0xca", "0x3f", "0x0f", "0x02", "0xc1", "0xaf", "0xbd", "0x03", "0x01", "0x13", "0x8a", "0x6b" },
    /*8*/  {"0x3a", "0x91", "0x11", "0x41", "0x4f", "0x67", "0xdc", "0xea", "0x97", "0xf2", "0xcf", "0xce", "0xf0", "0xb4", "0xe6", "0x73" },
    /*9*/  {"0x96", "0xac", "0x74", "0x22", "0xe7", "0xad", "0x35", "0x85", "0xe2", "0xf9", "0x37", "0xe8", "0x1c", "0x75", "0xdf", "0x6e" },
    /*a*/  {"0x47", "0xf1", "0x1a", "0x71", "0x1d", "0x29", "0xc5", "0x89", "0x6f", "0xb7", "0x62", "0x0e", "0xaa", "0x18", "0xbe", "0x1b" },
    /*b*/  {"0xfc", "0x56", "0x3e", "0x4b", "0xc6", "0xd2", "0x79", "0x20", "0x9a", "0xdb", "0xc0", "0xfe", "0x78", "0xcd", "0x5a", "0xf4" },
    /*c*/  {"0x1f", "0xdd", "0xa8", "0x33", "0x88", "0x07", "0xc7", "0x31", "0xb1", "0x12", "0x10", "0x59", "0x27", "0x80", "0xec", "0x5f" },
    /*d*/  {"0x60", "0x51", "0x7f", "0xa9", "0x19", "0xb5", "0x4a", "0x0d", "0x2d", "0xe5", "0x7a", "0x9f", "0x93", "0xc9", "0x9c", "0xef" },
    /*e*/  {"0xa0", "0xe0", "0x3b", "0x4d", "0xae", "0x2a", "0xf5", "0xb0", "0xc8", "0xeb", "0xbb", "0x3c", "0x83", "0x53", "0x99", "0x61" },
    /*f*/  {"0x17", "0x2b", "0x04", "0x7e", "0xba", "0x77", "0xd6", "0x26", "0xe1", "0x69", "0x14", "0x63", "0x55", "0x21", "0x0c", "0x7d" } };
            return iSbox;
        }
        public string shiftrowsInv(string state)
        {
            string result = "0x";
            result += state.Substring(2, 2);
            result += state.Substring(28, 2);
            result += state.Substring(22, 2);
            result += state.Substring(16, 2);

            result += state.Substring(10, 2);
            result += state.Substring(4, 2);
            result += state.Substring(30, 2);
            result += state.Substring(24, 2);

            result += state.Substring(18, 2);
            result += state.Substring(12, 2);
            result += state.Substring(6, 2);
            result += state.Substring(32, 2);

            result += state.Substring(26, 2);
            result += state.Substring(20, 2);
            result += state.Substring(14, 2);
            result += state.Substring(8, 2);
            return result;

        }
        public string[,] InvMixcolumns(string state)
        {
            string[,] statMat = Convert2D(state);
            int[,] GivenMatrix = new int[4, 4]{

                {14,11,13,9 },
                {9,14,11,13 },
                {13,9,14,11 },
                {11,13,9,14 }
            };
            string[,] result = new string[4, 4];
            int val;
            int resultTemp = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        val = Convert.ToInt32(statMat[j, k], 16);

                        if (GivenMatrix[i, j] == 9)
                        {
                            val = SLFval(val);
                            val = SLFval(val);
                            val = SLFval(val);
                            val ^= Convert.ToInt32(statMat[j, k], 16);
                        }
                        else if (GivenMatrix[i, j] == 11)
                        {
                            val = SLFval(val);
                            val = SLFval(val);

                            val ^= Convert.ToInt32(statMat[j, k], 16);
                            val = SLFval(val);
                            val ^= Convert.ToInt32(statMat[j, k], 16);
                        }
                        else if (GivenMatrix[i, j] == 13)
                        {

                            val = SLFval(val);
                            val ^= Convert.ToInt32(statMat[j, k], 16);
                            val = SLFval(val);
                            val = SLFval(val);
                            val ^= Convert.ToInt32(statMat[j, k], 16);
                        }
                        else
                        {

                            val = SLFval(val);
                            val ^= Convert.ToInt32(statMat[j, k], 16);

                            val = SLFval(val);
                            val ^= Convert.ToInt32(statMat[j, k], 16);
                            val = SLFval(val);
                        }

                        resultTemp ^= val;
                    }
                    result[i, k] = IntHex(resultTemp);
                    resultTemp = 0;
                }

            }
            return result;
        }
        public int SLFval(int val)
        {
            val <<= 1;
            if ((val & 256) != 0)
            {
                val -= 256;
                val ^= 27;
            }
            return val;
        }
    }
}
