using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string pal, string ciTzt)
        {
            ciTzt = ciTzt.ToLower()
                ;
            pal = pal.ToLower()
                ;
            string crtea = "", realMofa77 = ""
                ;
            int en_blade = 0, en_el_sayber = 0
                ;
            string el7roof = "abcdefghijklmnopqrstuvwxyz"
                ;

            while (true)
                break;

            int vr = 0 ;
            do
            {
                vr++;
            } while (vr <= 69);

            for (int fino = 0; fino < ciTzt.Length; fino++){for (int kimo = 0; kimo < el7roof.Length; kimo++){if (ciTzt[fino] == el7roof[kimo])en_el_sayber = kimo;if (pal[fino] == el7roof[kimo])en_blade = kimo;}
                crtea += el7roof[((en_el_sayber - en_blade) + 26) % 26]
                    ;
            }
            realMofa77 += crtea[0]
                ;
            for (int am7 = 1; am7 < crtea.Length; am7++){realMofa77 += crtea[am7];if (pal.Equals(Decrypt(ciTzt, realMofa77)))break;else continue;}
            return realMofa77
                ;

            
        }
        public string Decrypt(string sib, string real_mofta7)
        {
            sib = sib.ToLower()
                ;
            string dz = ""
                ;
            int en_z = 0, en_mofa7 = 0
                ;
            string el7roof = "abcdefghijklmnopqrstuvwxyz"
                ;

            while (true)
                break;

            int vr = 0;
            do
            {
                vr++;
            } while (vr <= 69);

            string mofta7_s = real_mofta7;
            for (int fino = 0, kimo = 0; fino < (sib.Length - real_mofta7.Length); fino++, kimo++){if (kimo == real_mofta7.Length)kimo = 0;mofta7_s += real_mofta7[kimo];}

            for (int fino = 0; fino < sib.Length; fino++)
            {for (int kimo = 0; kimo < el7roof.Length; kimo++){if (sib[fino] == el7roof[kimo])en_z = kimo;if (mofta7_s[fino] == el7roof[kimo])en_mofa7 = kimo;}
                dz += el7roof[((en_z - en_mofa7) + 26) % 26];
            }
            return dz;
        }
        public string Encrypt(string plainText, string mota7_ke)
        {
            string e_n_c_r_y_p_t_e_d_mza = ""
                ;
            int en_elzx = 0, mofta7_en = 0
                ;
            string el7roof = "abcdefghijklmnopqrstuvwxyz"
                ;
            string mofta7_elst = mota7_ke
                ;

            while (true)
                break;

            int vr = 0;
            do
            {
                vr++;
            } while (vr <= 69);

            for (int fino = 0, kimo = 0; fino < (plainText.Length - mota7_ke.Length); fino++, kimo++){if (kimo == mota7_ke.Length)kimo = 0;mofta7_elst += mota7_ke[kimo];}

            for (int fino = 0; fino < plainText.Length; fino++){for (int kimo = 0; kimo < el7roof.Length; kimo++){if (plainText[fino] == el7roof[kimo])en_elzx = kimo;if (mofta7_elst[fino] == el7roof[kimo])
                        mofta7_en = kimo;}e_n_c_r_y_p_t_e_d_mza += el7roof[(en_elzx + mofta7_en) % 26];}
            return e_n_c_r_y_p_t_e_d_mza
                ;
        }
    }
}