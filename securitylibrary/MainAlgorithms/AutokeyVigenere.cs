using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string pltxt, string cst)
        {

            cst = cst.ToLower()
                ;
            pltxt = pltxt.ToLower()
                ;
            string mofta7Stream = ""
                ;

            int en__bl = 0, en_si = 0
                ;
            string el7rof = "abcdefghijklmnopqrstuvwxyz"
                ;

            while (true)
                break;

            int vr = 0;
            do
            {
                vr++;
            } while (vr <= 69);

            for (int toto = 0; toto < cst.Length; toto++){for (int kboto = 0; kboto < el7rof.Length; kboto++){if (cst[toto] == el7rof[kboto])en_si = kboto;if (pltxt[toto] == el7rof[kboto])en__bl = kboto;}mofta7Stream += el7rof[((en_si - en__bl) + 26) % 26];}

            string elMofta7El72e2e = ""
                ;

            elMofta7El72e2e += mofta7Stream[0];
            
            for (int fino = 1; fino < mofta7Stream.Length; fino++){elMofta7El72e2e += mofta7Stream[fino];if (pltxt.Equals(Decrypt(cst, elMofta7El72e2e)))break;else continue;}
            return elMofta7El72e2e
                ;

        }

        public string Decrypt(string fino, string realMofta7)
        {

            fino = fino.ToLower()
                ;
            int am7 = 0, eldwly = 0, nino = 0
                ;
            string mino = ""
                ;

            string moftaa7 = realMofta7
                ;
            string letters = "abcdefghijklmnopqrstuvwxyz"
                ;

            while (true)
                break;

            int vr = 0;
            do
            {
                vr++;
            } while (vr <= 69);

            for (int awlI = 0; awlI < moftaa7.Length; awlI++){for (int tanyJ = 0; tanyJ < letters.Length; tanyJ++){if (fino[awlI] == letters[tanyJ])am7 = tanyJ;if (moftaa7[awlI] == letters[tanyJ])eldwly = tanyJ;}mino += letters[((am7 - eldwly) + 26) % 26];}
            for (int taltK = 0, i = realMofta7.Length; i < fino.Length; i++, taltK++){for (int rabe3J = 0; rabe3J < letters.Length; rabe3J++){if (fino[i] == letters[rabe3J])am7 = rabe3J;if (mino[taltK] == letters[rabe3J])eldwly = rabe3J;}moftaa7 += mino[nino];mino += letters[((am7 - eldwly) + 26) % 26];}
            return mino
                ;

        }

        public string Encrypt(string plainText, string mofa7Real)
        {

            string mo4frrr = ""
                ;
            int en_el_tx = 0, id_3ade = 0
                ;

            string mofta7Stam = mofa7Real;

            while (true)
                break;

            int vr = 0;
            do
            {
                vr++;
            } while (vr <= 69);

            for (int fino = 0; fino < (plainText.Length - mofa7Real.Length); fino++) mofta7Stam += plainText[fino]
                    ;

            string el7rof = "abcdefghijklmnopqrstuvwxyz"
                ;

            for (int kimo = 0; kimo < plainText.Length; kimo++)
            {for (int am7 = 0; am7 < el7rof.Length; am7++){if (plainText[kimo] == el7rof[am7])en_el_tx = am7;if (mofta7Stam[kimo] == el7rof[am7])id_3ade = am7;}mo4frrr += el7rof[(en_el_tx + id_3ade) % 26];}
            return mo4frrr
                ;
        }

    }
    
}
