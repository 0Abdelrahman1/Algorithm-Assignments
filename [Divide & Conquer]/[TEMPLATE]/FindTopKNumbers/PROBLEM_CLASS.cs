using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 
        //Your Code is Here:
        //==================

        public static int[] nums;

        public static void swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
        public static int Partition(int begin, int end)
        {
            int pivot = new Random((int)DateTime.Now.Ticks).Next(begin, end);
            swap(ref nums[pivot], ref nums[begin]);
            pivot = nums[begin];

            int i = begin, j = end;
            while (i <= j)
            {
                while (i + 1 < end && nums[++i] < pivot);
                while (j - 1 > begin && nums[--j] > pivot);

                if (i == j)
                    if (nums[i] > pivot) j--;
                    else i++;

                if (i > j) break;

                swap(ref nums[i], ref nums[j]);
            }

            swap(ref nums[begin], ref nums[j]);

            return j;
        }


        public static void TopKLargestNumbers(int begin, int end, int k)
        {
            if (end - begin <= k)
                return ;

            int idx = Partition(begin, end);

            if (end - idx >= k)
                TopKLargestNumbers(idx + 1, end, k);
            else
                TopKLargestNumbers(begin, idx, k - end + idx);
        }

        /// <summary>
        /// Find TOP k numbers in the given array
        /// </summary>
        /// <param name="numbers">All numbers</param>
        /// <param name="k">The required number of Top</param>
        /// <returns>Array of top k numbers</returns>
        public static int[] RequiredFunction(int[] numbers, int k)
        {
            nums = numbers;

            TopKLargestNumbers(0, numbers.Length, k);

            int[] ret = new int[k];
            for (int i = 0, j = numbers.Length - k; i < k; i++, j++)
                ret[i] = nums[j];

            Array.Sort(ret, (a, b) => b.CompareTo(a));
            return ret;
        }
        #endregion

    }
}

