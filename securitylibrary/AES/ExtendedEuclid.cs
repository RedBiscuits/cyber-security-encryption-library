using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        public int GetMultiplicativeInverse(int number, int baseN)
        {
        
            
            int B1 = 0, A1 = 1, B2 = 1, A3 = baseN, B3 = number,  A2 = 0 ;



            while (B3 != 1&& B3 != 0){

                int Q = A3 / B3
                    ;
                (A1, A2, A3, B1, B2, B3) 
                    = (B1, B2, B3, A1 - Q * B1, A2 - Q * B2, A3 - Q * B3)
                    ;
            }






            if (B3 == 1) return B2 < -1 ? B2 + baseN : B2
                    ;
            



            return -1
                ;
        }
    }
}
