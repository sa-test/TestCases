using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegasysTest
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] fruitSet1= {"orange", "lime", "apple", "mango" };
            string[] fruitSet2 = { "pineapple", "orange" };

            fruitSet1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine("");
            fruitSet2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine("");

            int equalCount = 0;
            bool result = fruitSet1.EqualCount(fruitSet2, out equalCount);
            Console.WriteLine(@"EqualCount result is:{0}, equalCount={1}", result, equalCount);

            Console.ReadLine();

        }
    }

    public static class ENumCompare
    {
        public static bool EqualCount<TSource> (this IEnumerable<TSource> enumerable1, IEnumerable<TSource> enumerable2, out int count)
        {
            bool res = false;
            count = 0;

            foreach (TSource item1 in enumerable1)
            {
                foreach(TSource item2 in enumerable2)
                {
                    if (item1.Equals(item2))
                    {
                        count++;
                    }
                }
            }
            
            res = count > 0 ? true: false;

            return res;
        }


    }

}
