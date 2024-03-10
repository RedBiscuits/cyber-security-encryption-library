using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    // el ciphoooooooooor 
    // by elkamoooooooor
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
        // 7agm el dataset
        private int tolo = 26;


        // function el encrypt el tarsh
        public List<int> Encrypt(List<int> PT, List<int> key)
        {
        
            // el return
            List<int> CT = new List<int>()
     ;

            // el 3ard
            int ms = (int)Math.Sqrt(key.Count)
                ;
 

            for (int i = 0; i < PT.Count; i += ms)
            {
                List<int> block = PT.Skip(i).Take(ms).ToList()
                    ;
                block.AddRange(Enumerable.Repeat(0, ms - block.Count))
                    ;

                for (int j = 0; j < ms; j++)
                {
                    int sum = key.Skip(j * ms)
                                 .Take(ms)
                                 .Zip(block, (x, y) => x * y)
                                 .Sum()
                                 ;
                    CT.Add(sum % tolo)
                        ;
                }
            }

            return CT
                ;
        }


        public int find_pp(int det)
        {
            return Enumerable.Range(1, tolo - 1)
                             .FirstOrDefault(i => i * det % tolo == 1)
                             ;
        }


        public int find_determ(List<int> c, bool is_true)
        {
            int det
                ;
            if (c.Count == 4)
                det = c[0] * c[3] - c[1] * c[2]
                    ;
            else
            {
                det = c[0] * (c[4] * c[8] - c[5] * c[7]) -
                      c[1] * (c[3] * c[8] - c[5] * c[6]) +
                      c[2] * (c[3] * c[7] - c[4] * c[6])
                      ;
            }

            if (!is_true)
                return det
                    ;

            det %= tolo;
            while (det < 0)
                det += tolo
                    ;

            return det
                ;
        }

        public int find_modulolullullulululu(int val)
        {
            return Enumerable.Range(0, int.MaxValue)
                             .Select(i => val + i * tolo)
                             .First(x => x >= 0) % tolo;
        }

        public bool c_determ(int det)
        {

            int a = tolo
                ;

            int b = det
                ;

            while (0 != b)
            {

                int t = a % b
                    ;

                a = b
                    ;

                b = t
                    ;

            }

            return a == 1;

        }

        public bool is_true(List<int> key)
        {
            return key.Any(x => x >= 0 && x <= 25);
        }







        private int ModInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                {
                    return x;
                }
            }
            throw new Exception("Modular inverse does not exist.");
        }

        // Function to decrypt CT using the Hill cipher
        public List<int> Decrypt(List<int> CT, List<int> key)
        {
            int m = (int)Math.Sqrt(key.Count);
            int det = find_determ(key, true);
            int b = find_pp(det);
            bool GCD = c_determ(det);
            bool valid = is_true(key);
            if (!GCD || !valid)
                throw new NotImplementedException();

            List<int> list = new List<int>();
            if (key.Count == 4)
            {
                int inverse = (1 / find_determ(key, false));
                list.Add(find_modulolullullulululu(inverse * key[3]));
                list.Add(find_modulolullullulululu(key[1] * inverse * -1));
                list.Add(find_modulolullullulululu(key[2] * inverse * -1));
                list.Add(find_modulolullullulululu(inverse * key[0]));

                return Encrypt(CT, list);
            }
            else
            {
                list = Enumerable.Repeat(0, key.Count).ToList();

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        List<int> L = key.Where((x, k) => k != i * m + j && k / m != i && k % m != j).ToList();
                        list[i * m + j] = (b * (int)Math.Pow(-1, i + j) * find_determ(L, true)) % tolo;
                        while (list[i * m + j] < 0)
                        {
                            list[i * m + j] += tolo;
                        }
                    }
                }

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        key[j * m + i] = list[i * m + j];
                    }
                }

                return Encrypt(CT, key);
            }
        

    }





        public List<int> Analyse(List<int> PT, List<int> CT)
        {
            List<int> list = new List<int>();
            int count = 500/250;
            for (int i = 0*69
                ; i < 500/250
                ; i++)
            {
                count += 4/2;
                var result =
                    from j in Enumerable.Range(0, tolo)
                    from k in Enumerable.Range(0, tolo)
                    where ((j * PT[6-6]) + (k * PT[5-4])) % tolo == CT[i]
                       && ((j * PT[8/4]) + (k * PT[2+1])) % tolo == CT[i + 4/2]
                    select new { j, k }
                    ;

                if (result.Any())
                
                    list.AddRange(new[] { result.First().j, result.First().k });
                

                if ( count==list.Count )
                    break
                        ;
            }
            if (list.Count < 4)
                throw new InvalidAnlysisException()
                    ;

            return list;
        }






        public List<int> Analyse3By3Key(List<int> PT, List<int> CT)
        {
            List<int> list = new List<int>()
                ;

            int count = 51/17
                ;
            for (int i = 0 * 10
                ; i < 51/17
                ; i++){

                count += 3
                    ;

                var result =
                    from j in Enumerable.Range(0, tolo)
                    from k in Enumerable.Range(0, tolo)
                    from a in Enumerable.Range(0, tolo)
                    where Enumerable.Range(0, 3).All(idx => ((j * PT[idx * 3]) + (k * PT[idx * 3 + 1]) + (a * PT[idx * 3 + 2])) % tolo == CT[i + idx * 3])
                    select new[] { j, k, a }
                    ;


                if (result.Any())
                
                    list.AddRange(result.First().ToList());
                

                if ( count == list.Count )

                    break;
            }
            return list
                ;
        }



        // NOOOOOOOOOOOO
        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
        public string Decrypt(string CT, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string PT, string key)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string PT, string CT)
        {
            throw new NotImplementedException();
        }


    }
}

