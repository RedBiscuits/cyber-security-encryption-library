using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 

        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            //throw new NotImplementedException();
            long K = power(y, k, q);
            long C1 = power(alpha, k, q);
            long C2 = K * m % q;
            List<long> Enc = new List<long>();
            Enc.Add(C1);
            Enc.Add(C2);
            return Enc;

        }
        private long power(int x, int y, int z)
        {
            long initialVal = 1;
            for (int i = 0; i < y; ++i)
            {
                initialVal = (initialVal * x) % z;
            }
            return initialVal;
        }

        public int Decrypt(int c1, int c2, int x, int q)
        {
            //throw new NotImplementedException();
            int M;
            int K = (int)power(c1, x, q);
            int d = modInverse(K, q);
            M = c2 * d % q;
            return M;
        }
        static int modInverse(int a, int m)
        {
            int m0 = m;
            int y = 0, x = 1;
            if (m == 1)
                return 0;
            while (a > 1)
            {
                int q = a / m;
                int t = m;
                m = a % m;
                a = t;
                t = y;
                y = x - q * y;
                x = t;
            }
            if (x < 0)
                x += m0;
            return x;
        }
    }
}