using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipatov.BinarySerialization.UwpTests
{
    [TestClass]
    public class TestWrappers
    {
        [TestMethod]
        public void DefaultTokenProvider()
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            var tp = context.GetTokensProvider<WrappersTestClass>();
            Assert.IsNotNull(tp);
        }

        [TestMethod]
        public void NoDefaultTokenProvider()
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            var tp = context.GetTokensProvider<WrappersTestClassBase>();
            Assert.IsNull(tp);
        }

        [TestMethod]
        public void SimpleDeepClone()
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            var o1 = new WrappersTestClass() { TestProperty = "Test string"};
            var o2 = o1.DeepClone(context);
            Assert.IsNotNull(o2);
            Assert.AreNotEqual(o1, o2);
            Assert.AreEqual(o1.TestProperty, o2.TestProperty);
        }

        [TestMethod]
        public void SubclassDeepClone()
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            var so1 = new WrappersSubclassTestClass() { TestProperty = "Test string", TestIntValue = 1024, TestPair = new KeyValuePair<int, int>(5, 10)};
            var o1 = (WrappersTestClass) so1;
            var o2 = o1.DeepClone(context);
            var so2 = o2 as WrappersSubclassTestClass;
            Assert.IsNotNull(o2, "o2 != null");
            Assert.IsNotNull(so2, "so2 != null");
            Assert.AreNotEqual(o1, o2);
            Assert.AreEqual(o1.TestProperty, o2.TestProperty);
            Assert.AreEqual(so1.TestIntValue, so2.TestIntValue);
            Assert.AreEqual(so1.TestPair, so2.TestPair);
        }
    }

    public class WrappersTestClassBase
    {
        public string TestProperty { get; set; }
    }

    public class WrappersTestClass : WrappersTestClassBase, ISerializationTokensProvider
    {
        public virtual IEnumerable<SerializationProperty> GetProperties(SerializationContext context)
        {
            yield return new SerializationProperty()
            {
                Property = nameof(TestProperty),
                Token = TestProperty.CreateSerializationToken(context)
            };
        }

        public virtual void FillProperty(ref SerializationProperty property, SerializationContext context)
        {
            switch (property.Property)
            {
                case nameof(TestProperty):
                    TestProperty = context.ExtractValue<string>(ref property.Token);
                    break;
            }
        }
    }

    public class WrappersSubclassTestClass : WrappersTestClass, ISerializationTokensProvider
    {
        public int TestIntValue { get; set; }

        public KeyValuePair<int, int> TestPair { get; set; }

        public IEnumerable<SerializationProperty> GetProperties(SerializationContext context)
        {
            foreach (var prop in base.GetProperties(context))
            {
                yield return prop;
            }
            yield return new SerializationProperty()
            {
                Property = nameof(TestIntValue),
                Token = TestIntValue.CreateSerializationToken(context)
            };
            yield return new SerializationProperty()
            {
                Property = nameof(TestPair),
                Token = TestPair.CreateSerializationToken(context)
            };
        }

        public override void FillProperty(ref SerializationProperty property, SerializationContext context)
        {
            base.FillProperty(ref property, context);
            switch (property.Property)
            {
                case nameof(TestIntValue):
                    TestIntValue = context.ExtractValue<int>(ref property.Token);
                    break;
                case nameof(TestPair):
                    TestPair = context.ExtractValue<KeyValuePair<int, int>>(ref property.Token);
                    break;
            }
        }
    }

    public class ComplexWrappedClass : ISerializationTokensProvider
    {
        public WrappersTestClass Wrapped { get; set; }

        public IEnumerable<SerializationProperty> GetProperties(SerializationContext context)
        {
            yield return new SerializationProperty()
            {
                Property = nameof(Wrapped),
                Token = Wrapped.CreateSerializationToken(context)
            };
        }

        public void FillProperty(ref SerializationProperty property, SerializationContext context)
        {
            switch (property.Property)
            {
                case nameof(Wrapped):
                    Wrapped = context.ExtractValue<WrappersTestClass>(ref property.Token);
                    break;
            }
        }
    }
}