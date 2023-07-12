using System.Collections.Generic;

namespace com.Pizia.Tools
{
    public interface IQuickSortCompatible
    {
        public int GetValue();
    }

    public static class SortMethods
    {
        public static void Quick_Sort64(this List<double> arr, int left, int right, double tolerance)
        {
            if (left < right)
            {
                int pivot = Partition64(ref arr, left, right, tolerance);

                if (pivot > 1) Quick_Sort64(arr, left, pivot - 1, tolerance);
                if (pivot + 1 < right) Quick_Sort64(arr, pivot + 1, right, tolerance);
            }
        }

        static int Partition64(ref List<double> arr, int left, int right, double tolerance)
        {
            double pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left < right)
                {
                    if ((arr[left] - arr[right]) <= tolerance) return right;

                    double temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort64(this double[] arr, int left, int right, double tolerance)
        {
            if (left < right)
            {
                int pivot = Partition64(ref arr, left, right, tolerance);

                if (pivot > 1) Quick_Sort64(arr, left, pivot - 1, tolerance);
                if (pivot + 1 < right) Quick_Sort64(arr, pivot + 1, right, tolerance);
            }
        }

        static int Partition64(ref double[] arr, int left, int right, double tolerance)
        {
            double pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left < right)
                {
                    if ((arr[left] - arr[right]) <= tolerance) return right;

                    double temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort32(this List<float> arr, int left, int right, float tolerance)
        {
            if (left < right)
            {
                int pivot = Partition32(ref arr, left, right, tolerance);

                if (pivot > 1) Quick_Sort32(arr, left, pivot - 1, tolerance);
                if (pivot + 1 < right) Quick_Sort32(arr, pivot + 1, right, tolerance);
            }
        }

        static int Partition32(ref List<float> arr, int left, int right, float tolerance)
        {
            float pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left < right)
                {
                    if ((arr[left] - arr[right]) <= tolerance) return right;

                    float temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort32(this float[] arr, int left, int right, float tolerance)
        {
            if (left < right)
            {
                int pivot = Partition32(ref arr, left, right, tolerance);

                if (pivot > 1) Quick_Sort32(arr, left, pivot - 1, tolerance);
                if (pivot + 1 < right) Quick_Sort32(arr, pivot + 1, right, tolerance);
            }
        }

        static int Partition32(ref float[] arr, int left, int right, float tolerance)
        {
            float pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left < right)
                {
                    if ((arr[left] - arr[right]) <= tolerance) return right;

                    float temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort(this List<IQuickSortCompatible> arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(ref arr, left, right);

                if (pivot > 1) Quick_Sort(arr, left, pivot - 1);
                if (pivot + 1 < right) Quick_Sort(arr, pivot + 1, right);
            }
        }

        static int Partition(ref List<IQuickSortCompatible> arr, int left, int right)
        {
            int pivot = arr[left].GetValue();
            while (true)
            {
                while (arr[left].GetValue() < pivot) left++;
                while (arr[right].GetValue() > pivot) right--;

                if (left < right)
                {
                    if (arr[left].GetValue() == arr[right].GetValue()) return right;

                    IQuickSortCompatible temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort(this IQuickSortCompatible[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(ref arr, left, right);

                if (pivot > 1) Quick_Sort(arr, left, pivot - 1);
                if (pivot + 1 < right) Quick_Sort(arr, pivot + 1, right);
            }
        }

        static int Partition(ref IQuickSortCompatible[] arr, int left, int right)
        {
            int pivot = arr[left].GetValue();
            while (true)
            {
                while (arr[left].GetValue() < pivot) left++;
                while (arr[right].GetValue() > pivot) right--;

                if (left < right)
                {
                    if (arr[left].GetValue() == arr[right].GetValue()) return right;

                    IQuickSortCompatible temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort(this List<int> arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(ref arr, left, right);

                if (pivot > 1) Quick_Sort(arr, left, pivot - 1);
                if (pivot + 1 < right) Quick_Sort(arr, pivot + 1, right);
            }
        }

        static int Partition(ref List<int> arr, int left, int right)
        {
            int pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    int temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        public static void Quick_Sort(this int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(ref arr, left, right);

                if (pivot > 1) Quick_Sort(arr, left, pivot - 1);
                if (pivot + 1 < right) Quick_Sort(arr, pivot + 1, right);
            }
        }

        static int Partition(ref int[] arr, int left, int right)
        {
            int pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    int temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
