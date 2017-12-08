using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ipatov.DataStructures;

namespace Ipatov.PrefixTree.Benchmark
{
    class Program
    {
        static Random _rnd = new Random();

        static void Main(string[] args)
        {
            TimeSpan ts;
            Console.Write("Standard dictionary insert (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new Dictionary<string, int>(), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Prefix dictionary insert (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new StringPrefixTreeDictionary<int>(false), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Standard dictionary insert (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new Dictionary<string, int>(), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Prefix dictionary insert (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new StringPrefixTreeDictionary<int>(false), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));
        }

        static string FormatResult(double seconds, long numOps)
        {
            var spo = seconds / numOps;
            if (spo > 1.0 / 100)
            {
                return $"{spo} sec.";
            }
            if (spo > 1.0 / 100_000)
            {
                return $"{spo*1000} msec.";
            }
            return $"{spo * 1000_1000} nsec.";
        }

        static string GenerateRandomKey(byte[] keyBuffer)
        {
            var sb = new StringBuilder();
            _rnd.NextBytes(keyBuffer);
            for (var i = 0; i < keyBuffer.Length; i++)
            {
                sb.Append($"{keyBuffer[i]:X2}");
            }
            return sb.ToString();
        }

        static string[,] GenerateRandomKeys(int keySize, int keysToAdd, int iterations)
        {
            var keyBuf = new byte[keySize / 2];
            var keys = new string[iterations, keysToAdd];
            for (var i = 0; i < iterations; i++)
            {
                for (var a = 0; a < keysToAdd; a++)
                {
                    keys[i, a] = GenerateRandomKey(keyBuf);
                }
            }
            return keys;
        }

        static TimeSpan BenchamarkInserts<T>(Func<T> factoryFunc, int keySize, int keysToAdd, int iterations) where T : IDictionary<string, int>
        {
            var keys = GenerateRandomKeys(keySize, keysToAdd, iterations);
            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < iterations; i++)
            {
                var d = factoryFunc();
                for (var k = 0; k < keysToAdd; k++)
                {
                    d[keys[i, k]] = i + k;
                }
            }

            sw.Stop();
            return sw.Elapsed;
        }
    }
}
