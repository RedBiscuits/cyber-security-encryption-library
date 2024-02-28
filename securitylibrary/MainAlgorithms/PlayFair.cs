using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            char[,] Table = get_table(key);
            cipherText = cipherText.ToLower();

            string plain = "";

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                plain += inverse_search(cipherText[i], cipherText[i + 1], Table);
            }

            Console.WriteLine(postprocesstext(plain));
            return postprocesstext(plain);
        }

        public string Encrypt(string text, string key)
        {

            char[,] Table = get_table(key);
            string newtext = process_text(text);

            string cipher = "";

            for (int i = 0; i < newtext.Length; i += 2)
            {
                cipher += search(newtext[i], newtext[i + 1], Table);
            }

            return cipher;
        }


        private char[,] get_table(string k)
        {
            char[,] table = new char[5, 5];

            construct_data(table, new string((k + "abcdefghiklmnopqrstuvwxyz").Distinct().ToArray()));

            return table;
        }

        private void construct_data(char[,] table, string all)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    table[i, j] = all[(i * 5) + j];
                }
            }
        }

        private string process_text(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (insert_x(text, i))
                {
                    text = text.Insert(i + 1, "x");
                }
            }

            if (text.Length % 2 != 0)
                text += "x";

            return text;
        }

        private bool insert_x(string text, int index)
        {
            return index + 1 < text.Length && text[index] == text[index + 1] && index % 2 == 0;
        }

        private string search(char A, char B, char[,] Table)
        {
            int indexArow, indexAcol, indexBrow, indexBcol;
            find_indeces(A, B, Table, out indexArow, out indexAcol, out indexBrow, out indexBcol);

            return get_answer(A, B, Table, indexArow, indexAcol, indexBrow, indexBcol);
        }

        private void find_indeces(char A, char B, char[,] Table, out int indexArow, out int indexAcol, out int indexBrow, out int indexBcol)
        {
            indexArow = indexAcol = indexBrow = indexBcol = -1;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Table[i, j] == A)
                    {
                        indexArow = i;
                        indexAcol = j;
                    }
                    else if (Table[i, j] == B)
                    {
                        indexBrow = i;
                        indexBcol = j;
                    }
                }
            }
        }

        private string get_answer(char A, char B, char[,] Table, int indexArow, int indexAcol, int indexBrow, int indexBcol)
        {
            string answer = "";

            if (indexArow == indexBrow)
            {
                answer += Table[indexArow, (indexAcol + 1) % 5];
                answer += Table[indexBrow, (indexBcol + 1) % 5];
            }
            else if (indexAcol == indexBcol)
            {
                answer += Table[(indexArow + 1) % 5, indexAcol];
                answer += Table[(indexBrow + 1) % 5, indexBcol];
            }
            else
            {
                answer += Table[indexArow, indexBcol];
                answer += Table[indexBrow, indexAcol];
            }

            return answer;
        }

        private string inverse_search(char A, char B, char[,] Table)
        {
            int indexArow, indexAcol, indexBrow, indexBcol;
            find_indeces(A, B, Table, out indexArow, out indexAcol, out indexBrow, out indexBcol);

            return get_inverse(A, B, Table, indexArow, indexAcol, indexBrow, indexBcol);
        }

        private string get_inverse(char A, char B, char[,] Table, int indexArow, int indexAcol, int indexBrow, int indexBcol)
        {
            string answer = "";

            if (indexArow == indexBrow)
            {
                int newAcol = (indexAcol - 1) < 0 ? (indexAcol - 1 + 5) : (indexAcol - 1);
                int newBcol = (indexBcol - 1) < 0 ? (indexBcol - 1 + 5) : (indexBcol - 1);
                answer += Table[indexArow, newAcol % 5];
                answer += Table[indexBrow, newBcol % 5];
            }
            else if (indexAcol == indexBcol)
            {
                int newArow = (indexArow - 1) < 0 ? (indexArow - 1 + 5) : (indexArow - 1);
                int newBrow = (indexBrow - 1) < 0 ? (indexBrow - 1 + 5) : (indexBrow - 1);
                answer += Table[newArow % 5, indexAcol];
                answer += Table[newBrow % 5, indexBcol];
            }
            else
            {
                answer += Table[indexArow, indexBcol];
                answer += Table[indexBrow, indexAcol];
            }

            return answer;
        }

        private string postprocesstext(string text) // postProcess huh? patent here
        {
            rem_last_x(ref text);

            for (int i = 0; i < text.Length; i += 2)
            {
                if (i + 2 < text.Length && text[i] == text[i + 2] && text[i + 1] == 'x')
                {
                    text = text.Remove(i + 1, 1);
                    i--;
                }
            }

            return text;
        }

        private void rem_last_x(ref string text) {
            if (text[text.Length - 1] == 'x')
                text = text.Remove(text.Length - 1, 1);
        }
    }
}