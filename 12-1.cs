using System;
using System.IO;
using Newtonsoft.Json;

namespace aoc
{   
    internal class Program
    {
        private static void Main()
        {
            int sum = 0;

            using (JsonReader reader = new JsonTextReader(new StreamReader(@"c:\temp\aoc-12.txt")))
            {
                while (reader.Read())
                {
                    if (reader.Value == null)
                        continue;

                    int i;

                    if (int.TryParse(reader.Value.ToString(), out i))
                        sum += i;
                }
            }

            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}



