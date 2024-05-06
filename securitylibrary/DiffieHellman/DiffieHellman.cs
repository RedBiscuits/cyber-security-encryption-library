using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SecurityLibrary.DiffieHellman
{


    public class DiffieHellman
    {

        public int power(int f, int s, int sf)
        {
            //throw new NotImplementedException();
            int Res = 1;
            if (s > 0)
            {
                for (int i = 1; i <= s; ++i)
                {
                    Res *= f;
                    Res %= sf;
                }
            }
            return Res;
        }

        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            //throw new NotImplementedException();
            int ay, aya;
            List<int> key = new List<int> { };
            int temp3, temp4;
            ay = (power(alpha, xa, q));
            aya = (power(alpha, xb, q));
            temp3 = (power(aya, xa, q));
            key.Add(temp3);
            temp4 = power(ay, xb, q);
            key.Add(temp4);
            return key;
        }

    }
}