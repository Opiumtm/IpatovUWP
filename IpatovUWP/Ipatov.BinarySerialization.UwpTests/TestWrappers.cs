using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
            d = new Dictionary<Type, IExternalSerializationTokensProvider>();
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

        private static readonly ITypeMapper AssemblyMapper = new AssemblyTypesMapper(typeof(ComplexWrappedClass).GetTypeInfo().Assembly);

        private byte[] SerializeIbsf(ComplexWrappedClass o)
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            context.TypeMapper = new CompositeTypeMapper(CompositeTypeMapper.DefaultTypeMapper, AssemblyMapper);
            byte[] serialized;
            using (var str = new MemoryStream())
            {
                using (var wr = new BinaryWriter(str, Encoding.UTF8))
                {
                    wr.Serialize(o, context);
                }
                serialized = str.ToArray();
            }
            return serialized;
        }

        private ComplexWrappedClass DeserializeIbsf(byte[] serialized)
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            var context = new SerializationContext(new ReadOnlyDictionary<Type, IExternalSerializationTokensProvider>(d));
            context.TypeMapper = new CompositeTypeMapper(CompositeTypeMapper.DefaultTypeMapper, AssemblyMapper);
            ComplexWrappedClass o2;
            using (var str = new MemoryStream(serialized))
            {
                using (var rd = new BinaryReader(str, Encoding.UTF8))
                {
                    o2 = rd.Deserialize<ComplexWrappedClass>(context);
                }
            }
            return o2;
        }

        private static readonly DataContractSerializer TestSerializer = new DataContractSerializer(typeof(ComplexWrappedClass));

        private byte[] SerializeDataContract(ComplexWrappedClass o)
        {
            byte[] serialized;
            using (var str = new MemoryStream())
            {
                using (var wr = XmlDictionaryWriter.CreateBinaryWriter(str))
                {
                    TestSerializer.WriteObject(wr, o);
                    wr.Flush();
                }
                serialized = str.ToArray();
            }
            return serialized;
        }

        private ComplexWrappedClass DeserializeDataContract(byte[] serialized)
        {
            ComplexWrappedClass o2;
            using (var str = new MemoryStream(serialized))
            {
                using (var rd = XmlDictionaryReader.CreateBinaryReader(str, XmlDictionaryReaderQuotas.Max))
                {
                    o2 = TestSerializer.ReadObject(rd) as ComplexWrappedClass;
                }
            }
            return o2;
        }

        [TestMethod]
        public void ComplexBinaryWriteBenchmark()
        {
            var o = new ComplexWrappedClass()
            {
                Wrapped = new WrappersSubclassTestClass()
                {
                    TestProperty = "Test string",
                    TestIntValue = 1024,
                    TestPair = new KeyValuePair<int, int>(10, 20)
                }
            };
            const int testCount = 10000;

            byte[] serialized = null;
            ComplexWrappedClass o2 = null;

            Stopwatch sw;

            sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < testCount; i++)
            {
                serialized = SerializeIbsf(o);
            }
            sw.Stop();
            Logger.LogMessage($"IBSF serialize: {sw.Elapsed.Ticks}, len = {serialized.Length}");

            sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < testCount; i++)
            {
                o2 = DeserializeIbsf(serialized);
            }
            sw.Stop();
            Logger.LogMessage($"IBSF deserialize: {sw.Elapsed.Ticks}");

            sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < testCount; i++)
            {
                serialized = SerializeDataContract(o);
            }
            sw.Stop();
            Logger.LogMessage($"Data contract serialize: {sw.Elapsed.Ticks}, len = {serialized.Length}");

            sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < testCount; i++)
            {
                o2 = DeserializeDataContract(serialized);
            }
            sw.Stop();
            Logger.LogMessage($"Data contract deserialize: {sw.Elapsed.Ticks}");
        }

        public static bool ComplexDeepClone_isRetrv = false;
    }

    [DataContract]
    [KnownType(typeof(WrappersTestClass))]
    [KnownType(typeof(WrappersSubclassTestClass))]
    public class WrappersTestClassBase
    {
        [DataMember]
        public string TestProperty { get; set; }
    }

    [TypeIdentity("Test.WrappersTestClass")]
    [DataContract]
    [KnownType(typeof(WrappersSubclassTestClass))]
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

    [TypeIdentity("Test.WrappersSubclassTestClass")]
    [DataContract]
    public class WrappersSubclassTestClass : WrappersTestClass, ISerializationTokensProvider
    {
        [DataMember]
        public int TestIntValue { get; set; }

        [DataMember]
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
    [DataContract]
    public class ComplexWrappedClass : ISerializationTokensProvider
    {
        [DataMember]
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