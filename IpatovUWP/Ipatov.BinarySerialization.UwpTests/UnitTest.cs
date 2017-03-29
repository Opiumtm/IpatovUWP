using System;
using System.Diagnostics;
using System.Reflection;
using Ipatov.BinarySerialization.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace Ipatov.BinarySerialization.UwpTests
{
    [TestClass]
    public class TestPlatformReflection
    {
        [TestMethod]
        public void ReflectionProprty()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            p.SetValue(o, "Test string");
            var s = p.GetValue(o);
            Assert.AreEqual(s, "Test string");
        }

        [TestMethod]
        public void DelegateProperty()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            var getFunc = p.GetMethod.CreateDelegate(typeof(Func<string>), o) as Func<string>;
            var setFunc = p.SetMethod.CreateDelegate(typeof(Action<string>), o) as Action<string>;
            Assert.IsNotNull(getFunc);
            Assert.IsNotNull(setFunc);
            setFunc("Test string");
            var s = getFunc();
            Assert.AreEqual(s, "Test string");
        }

        [TestMethod]
        public void OpenDelegateProperty()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            var getFunc = p.GetMethod.CreateDelegate(typeof(Func<TestClass, string>)) as Func<TestClass, string>;
            var setFunc = p.SetMethod.CreateDelegate(typeof(Action<TestClass, string>)) as Action<TestClass, string>;
            Assert.IsNotNull(getFunc);
            Assert.IsNotNull(setFunc);
            setFunc(o, "Test string");
            var s = getFunc(o);
            Assert.AreEqual(s, "Test string");
        }

        [TestMethod]
        public void OpenDelegatePropertyNonGeneric()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            var getFunc = p.GetMethod.CreateDelegate(typeof(Func<TestClass, string>));
            var setFunc = p.SetMethod.CreateDelegate(typeof(Action<TestClass, string>));
            Assert.IsNotNull(getFunc);
            Assert.IsNotNull(setFunc);
            setFunc.DynamicInvoke(o, "Test string");
            var s = (string)getFunc.DynamicInvoke(o);
            Assert.AreEqual(s, "Test string");
        }

        [TestMethod]
        public void ReflectedPropertyStruct()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            var property = new ReflectedProperty(p);
            property.Set(o, "Test string");
            var s = property.Get<TestClass, string>(o);
            Assert.AreEqual(s, "Test string");
        }

        [TestMethod]
        public void ReflectedPropertyStructNonGeneric()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            var property = new ReflectedProperty(p);
            object o1 = o;
            object s1 = "Test string";
            property.Set(o1, s1);
            var s = (string)property.Get(o1);
            Assert.AreEqual(s, "Test string");
        }

        [TestMethod]
        public void ReflectedPropertyStructBenchmark()
        {
            var o = new TestClass();
            var t = typeof(TestClass);
            var p = t.GetProperty(nameof(TestClass.TestProperty));
            var property = new ReflectedProperty(p);
            object o1 = o;
            object s1 = "test";
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 10000000; i++)
            {
                o.TestProperty = "test";
                var s = o.TestProperty;
            }
            sw.Stop();
            var direct = sw.ElapsedMilliseconds;
            sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 10000000; i++)
            {
                property.Set(o, "test");
                var s = property.Get<TestClass, string>(o);
            }
            sw.Stop();
            var propGeneric = sw.ElapsedMilliseconds;
            sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 10000000; i++)
            {
                property.Set(o1, s1);
                var s = (string)property.Get(o);
            }
            sw.Stop();
            var propNonGeneric = sw.ElapsedMilliseconds;
            Logger.LogMessage($"Direct access = {direct}");
            Logger.LogMessage($"Generic reflected access = {propGeneric}");
            Logger.LogMessage($"Non-generic reflected access = {propNonGeneric}");
        }
    }

    public sealed class TestClass
    {
        public string TestProperty { get; set; }
    }
}
