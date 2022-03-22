using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace meanMedianMode
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = Console.ReadLine();

            int n = Int32.Parse(line);

            int[] arr = Array.ConvertAll<string, int>(Console.ReadLine().Split(' '), int.Parse);

            double mean = Mean(arr);
            double median = Median(arr);
            int mode = Mode(arr);

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Console.WriteLine($"{mean:0.0}");
            Console.WriteLine($"{median:0.0}");
            Console.WriteLine($"{mode}");

            return;

        }

        static double Mean(int[] arr)
        {
            int sum = arr.Sum();
            return sum / (double)arr.Length;
        }

        static int Mode(int[] arr)
        {
            
            ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();

            foreach(int key in arr)
            {
                dict.AddOrUpdate(key, 1, (k, v) => v + 1);
            }

            KeyValuePair<int, int> mode = new KeyValuePair<int, int>(0, 0);
            foreach(KeyValuePair<int, int> entry in dict)
            {
                if (entry.Value > mode.Value || (entry.Value == mode.Value && entry.Key < mode.Key))
                {
                    mode = entry;
                }
            }

            return mode.Key;

        }

        static double Median(int[] arr)
        {
            int k = arr.Length / 2;
            SelectLomuto(arr, 0, arr.Length - 1, k);

            double median = arr[k];
            if(arr.Length%2 == 0)
            {
                k--;
                SelectLomuto(arr, 0, k+1, k);
                median = (median + arr[k]) / 2;
            }
            return median;
        }

        static void SelectLomuto(int[] arr, int left, int right, int k)
        {
            int kr;
            for (;;)
            {
                if (left == right) return;
                kr = PartitionLomuto(arr, left, right);
                if (kr == k) return;
                if(k < kr)
                {
                    right = kr - 1;
                }
                else
                {
                    left = kr + 1;
                }
            }
        }

        static int PartitionLomuto(int[] arr, int left, int right)
        {
            int storeIndex = left;

            int pivot = arr[right];
            int aux;
            for(int i = left; i < right; i++)
            {
                if(arr[i] < pivot)
                {
                    aux = arr[storeIndex];
                    arr[storeIndex] = arr[i];
                    arr[i] = aux;
                    storeIndex++;
                }
            }
            
            aux = arr[storeIndex];
            arr[storeIndex] = arr[right];
            arr[right] = aux;

            return storeIndex;
        }
    }
}
