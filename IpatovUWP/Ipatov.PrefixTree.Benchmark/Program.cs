using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

            Console.Write("Standard hash dictionary insert (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new Dictionary<string, int>(), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Standard sorted dictionary insert (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new SortedDictionary<string, int>(), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Prefix dictionary insert (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new StringPrefixTreeDictionary<int>(false), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Sorted list dictionary insert (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new BinarySortedListDictionary<string, int, StringComparer>(StringComparer.Ordinal), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.WriteLine();

            Console.Write("Standard hash dictionary insert (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new Dictionary<string, int>(), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Standard sorted dictionary insert (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new SortedDictionary<string, int>(), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Prefix dictionary insert (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new StringPrefixTreeDictionary<int>(false), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Sorted list dictionary insert (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkInserts(() => new BinarySortedListDictionary<string, int, StringComparer>(StringComparer.Ordinal), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.WriteLine();

            Console.Write("Standard hash dictionary search (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new Dictionary<string, int>(), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Standard sorted dictionary search (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new SortedDictionary<string, int>(), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Prefix dictionary search (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new StringPrefixTreeDictionary<int>(false), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Sorted list dictionary search (k:10, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new BinarySortedListDictionary<string, int, StringComparer>(StringComparer.Ordinal), 10, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.WriteLine();

            Console.Write("Standard hash dictionary search (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new Dictionary<string, int>(), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Standard sorted dictionary search (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new SortedDictionary<string, int>(), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Prefix dictionary search (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new StringPrefixTreeDictionary<int>(false), 100, 1000, 1000);
            Console.WriteLine(FormatResult(ts.TotalSeconds, 1000 * 1000));

            Console.Write("Sorted list dictionary search (k:100, ka: 1000, i:1000): ");
            ts = BenchamarkSearches(() => new BinarySortedListDictionary<string, int, StringComparer>(StringComparer.Ordinal), 100, 1000, 1000);
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
            var bs = new BinaryFormatter();
            var fname = $"{keySize}-{keysToAdd}-{iterations}.cache";
            if (!File.Exists(fname))
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
                using (var str = File.Create(fname))
                {
                    bs.Serialize(str, keys);
                }
                return keys;
            }
            else
            {
                using (var str = File.OpenRead(fname))
                {
                    return (string[,]) bs.Deserialize(str);
                }
            }
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

        static TimeSpan BenchamarkSearches<T>(Func<T> factoryFunc, int keySize, int keysToAdd, int iterations) where T : IDictionary<string, int>
        {
            var keys = GenerateRandomKeys(keySize, keysToAdd, 1);
            var sw = new Stopwatch();
            var d = factoryFunc();
            for (var k = 0; k < keysToAdd; k++)
            {
                d[keys[0, k]] = k;
            }
            sw.Start();

            for (var i = 0; i < iterations; i++)
            {
                for (var k = 0; k < keysToAdd; k++)
                {
                    var kr = d[keys[0, k]];
                    if (kr != k)
                    {
                        throw new InvalidOperationException($"invalid value for key {keys[0, k]}, expected {k}, got {kr}");
                    }
                }
            }

            sw.Stop();
            return sw.Elapsed;
        }
    }
}
