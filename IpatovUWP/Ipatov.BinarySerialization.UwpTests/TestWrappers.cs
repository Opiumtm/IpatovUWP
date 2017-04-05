using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Ipatov.BinarySerialization.Reflection;
using Ipatov.BinarySerialization.TokenProviders;
using Ipatov.BinarySerialization.TypeMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

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

        [TestMethod]
        public void ComplexDeepClone()
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            var o = new ComplexWrappedClass() { Wrapped = new WrappersSubclassTestClass()
            {
                TestProperty = "Test string",
                TestIntValue = 1024,
                TestPair = new KeyValuePair<int, int>(10, 20)
            }};
            var o2 = o.DeepClone(context);
            Assert.IsNotNull(o2);
            Assert.IsNotNull(o2.Wrapped);
            Assert.AreNotSame(o, o2);
            Assert.AreNotSame(o.Wrapped, o2.Wrapped);
            var w = o.Wrapped as WrappersSubclassTestClass;
            var w2 = o2.Wrapped as WrappersSubclassTestClass;
            Assert.IsNotNull(w);
            Assert.IsNotNull(w2);
            Assert.AreEqual(w.TestProperty, w2.TestProperty);
            Assert.AreEqual(w.TestIntValue, w2.TestIntValue);
            Assert.AreEqual(w.TestPair, w2.TestPair);
            Assert.IsTrue(ComplexDeepClone_isRetrv, "Not called known providers");
        }

        [TestMethod]
        public async Task ComplexBinaryWrite()
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            context.TypeMapper = new CompositeTypeMapper(CompositeTypeMapper.DefaultTypeMapper, new AssemblyTypesMapper(this.GetType().GetTypeInfo().Assembly));
            var o = new ComplexWrappedClass()
            {
                Wrapped = new WrappersSubclassTestClass()
                {
                    TestProperty = "Test string",
                    TestIntValue = 1024,
                    TestPair = new KeyValuePair<int, int>(10, 20)
                }
            };
            byte[] serialized;
            using (var str = new MemoryStream())
            {
                using (var wr = new BinaryWriter(str, Encoding.UTF8))
                {
                    wr.Serialize(o, context);
                }
                serialized = str.ToArray();
            }
            Assert.IsTrue(serialized.Length > 0);
            Logger.LogMessage($"Serialized data length = {serialized.Length} bytes");
            var myDocs = KnownFolders.DocumentsLibrary;
            using (var str = await (await myDocs.CreateFileAsync("ComplexBinaryWrite.ibsf", CreationCollisionOption.ReplaceExisting)).OpenStreamForWriteAsync())
            {
                await str.WriteAsync(serialized, 0, serialized.Length);
            }

            // deserialize
            context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            context.TypeMapper = new CompositeTypeMapper(CompositeTypeMapper.DefaultTypeMapper, new AssemblyTypesMapper(this.GetType().GetTypeInfo().Assembly));
            ComplexWrappedClass o2;
            using (var str = new MemoryStream(serialized))
            {
                using (var rd = new BinaryReader(str, Encoding.UTF8))
                {
                    o2 = rd.Deserialize<ComplexWrappedClass>(context);
                }
            }

            Assert.IsNotNull(o2);
            Assert.IsNotNull(o2.Wrapped);
            Assert.AreNotSame(o, o2);
            Assert.AreNotSame(o.Wrapped, o2.Wrapped);
            var w = o.Wrapped as WrappersSubclassTestClass;
            var w2 = o2.Wrapped as WrappersSubclassTestClass;
            Assert.IsNotNull(w);
            Assert.IsNotNull(w2);
            Assert.AreEqual(w.TestProperty, w2.TestProperty);
            Assert.AreEqual(w.TestIntValue, w2.TestIntValue);
            Assert.AreEqual(w.TestPair, w2.TestPair);
        }

        public static bool ComplexDeepClone_isRetrv = false;
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

    [KnownTokenProviders(typeof(WrappedClassKnownProviders))]
    [TypeIdentity("Test.ComplexWrappedClass")]
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

    public class WrappedClassKnownProviders : IKnownTokenProviders
    {
        public KnownTokenProviders GetKnownTokenProviders()
        {
            TestWrappers.ComplexDeepClone_isRetrv = true;
            return new KnownTokenProviders(new Dictionary<Type, IExternalSerializationTokensProvider>()
            {
                { typeof(WrappersTestClass), new SerializationTokensProviderWrapper<WrappersTestClass>()},
                { typeof(WrappersSubclassTestClass), new SerializationTokensProviderWrapper<WrappersSubclassTestClass>()},
                { typeof(KeyValuePair<int, int>), new KeyValuePairTokensProvider<int, int>() }
            });
        }
    }
}