namespace MaximTechnologyTasks.Services
{
    public class StringFormatService
    {
        public async Task<string> FormatStr (string origin)
        {
            if (origin == null)
                return new string("");

            char[] result = new char[origin.Length];

            if (origin.Length % 2 == 0)
            {

                for (int i = origin.Length / 2 - 1; i >= 0; i--)
                    result[origin.Length / 2 - i - 1] = origin[i];

                int k = origin.Length / 2;
                for (int i = origin.Length - 1; i >= origin.Length / 2; i--)
                {
                    result[k] = origin[i];
                    k++;
                }

                return new string(result);
            }


            for (int i = origin.Length - 1; i >= 0; i--)
                result[origin.Length - i - 1] = origin[i];

            string res = String.Concat(new string(result), origin);

            return res;
        }
    }
}
