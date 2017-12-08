using System;
using Ipatov.DataStructures;

namespace Ipatov.PrefixTree.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDic = new StringPrefixTreeDictionary<int>();

            void SetValue(string key, int value)
            {
                testDic[key] = value;
                Console.WriteLine($"set for \"{key}\": {testDic[key]}, count = {testDic.Count}");
            }

            void RemoveValue(string key)
            {
                var b = testDic.Remove(key);
                Console.WriteLine($"remove for \"{key}\": {b}, count = {testDic.Count}");
            }


            void PrintAllValues()
            {
                Console.WriteLine("----------------------");
                foreach (var kv in testDic)
                {
                    Console.WriteLine($"\"{kv.Key}\": {kv.Value}");
                }
                Console.WriteLine("----------------------");
            }

            SetValue("", 0);
            SetValue("", 1);
            SetValue("aaa", 3);
            SetValue("aaaa", 4);
            SetValue("aab", 5);

            PrintAllValues();
            SetValue("aab", 2);
            PrintAllValues();

            SetValue("a", 6);
            PrintAllValues();

            RemoveValue("a");
            PrintAllValues();

            RemoveValue("aab");
            PrintAllValues();

            RemoveValue("aaa");
            PrintAllValues();

            Console.WriteLine("end");
        }
    }
}
