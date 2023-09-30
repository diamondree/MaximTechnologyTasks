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


        public static char[] TreeSort(char[] arr)
        {
            int[] array = Array.ConvertAll(arr, x => (int)x);
            var treeNode = new TreeNode(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                treeNode.Insert(new TreeNode(array[i]));
            }

            return Array.ConvertAll(treeNode.Transform(), x => (char)x);
        }



        private class TreeNode
        {
            public int Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }

            public TreeNode(int data)
            {
                Value = data;
            }

            
            public void Insert(TreeNode node)
            {
                if (node.Value < Value)
                {
                    if (Left == null)
                    {
                        Left = node;
                    }
                    else
                    {
                        Left.Insert(node);
                    }
                }
                else
                {
                    if (Right == null)
                    {
                        Right = node;
                    }
                    else
                    {
                        Right.Insert(node);
                    }
                }
            }

            
            public int[] Transform(List<int> elements = null)
            {
                if (elements == null)
                {
                    elements = new List<int>();
                }

                if (Left != null)
                {
                    Left.Transform(elements);
                }

                elements.Add(Value);

                if (Right != null)
                {
                    Right.Transform(elements);
                }

                return elements.ToArray();
            }
        }
    }

    
}
