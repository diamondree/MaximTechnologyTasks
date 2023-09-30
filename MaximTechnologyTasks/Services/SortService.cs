namespace MaximTechnologyTasks.Services
{
    internal class SortService
    {
        public static char[] QuickSort(char[] array)
        {
            int[] reversedArray = Array.ConvertAll(array, x => (int)x);

            QuickSortRecursive(reversedArray, 0, array.Length - 1);

            return Array.ConvertAll(reversedArray, x => (char)x);
        }

        private static void QuickSortRecursive(int[] array, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = Partition(array, left, right);
                QuickSortRecursive(array, left, pivotIndex - 1);
                QuickSortRecursive(array, pivotIndex + 1, right);
            }
        }

        private static int Partition(int[] array, int left, int right)
        {
            int pivot = array[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, right);
            return i + 1;
        }

        private static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
