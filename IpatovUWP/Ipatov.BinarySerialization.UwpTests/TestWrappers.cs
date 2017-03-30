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
    }

    public class WrappersTestClassBase
    {
        public string TestProperty { get; set; }
    }

    public class WrappersTestClass : WrappersTestClassBase, ISerializationTokensProvider
    {
        public IEnumerable<SerializationProperty> GetProperties(SerializationContext context)
        {
            yield return new SerializationProperty()
            {
                Property = nameof(TestProperty),
                Token = TestProperty.CreateSerializationToken(context)
            };
        }

        public void FillProperties<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            foreach (var prop in properties)
            {
                switch (prop.Property)
                {
                    case nameof(TestProperty):
                        TestProperty = prop.Token.ExtractValue<string>(context);
                        break;
                }
            }
        }
    }
}