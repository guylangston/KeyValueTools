using NUnit.Framework;

namespace GL.KeyValueTools
{
    [TestFixture]
    public class KeyValueReflectionTests
    {
        [Test]
        public void CanSetValues()
        {
            var obj = new KeyValueBinderTests.SomeClass();

            var bag = new KeyValueBag
            {
                ["String"] = "XXX",
                ["Integer"] = 543
            };

            var reflect = new KeyValueReflection();
            var misses = reflect.SetValues(bag, obj);

            Assert.That(obj.String, Is.EqualTo("XXX"));
            Assert.That(obj.Integer, Is.EqualTo(543));
            Assert.That(misses, Is.EqualTo(0));
        }
        
        [Test]
        public void MissesGracefully()
        {
            var obj = new KeyValueBinderTests.SomeClass();

            var bag = new KeyValueBag
            {
                ["PropDoesNotExist"] = "XXX",
                ["Integer"] = 543
            };

            var reflect = new KeyValueReflection();
            var misses = reflect.SetValues(bag, obj);

            Assert.That(obj.Integer, Is.EqualTo(543));
            Assert.That(misses, Is.EqualTo(1));
        }
        
        [Test]
        public void MissesGracefullyWithTypeCheck()
        {
            var obj = new KeyValueBinderTests.SomeClass();

            var bag = new KeyValueBag
            {
                ["Integer"] = "Not an Integer!"
            };

            var reflect = new KeyValueReflection();
            var misses = reflect.SetValues(bag, obj);
            
            Assert.That(misses, Is.EqualTo(1));
        }


        [Test]
        public void MissesGracefullyWithConversion()
        {
            var obj = new KeyValueBinderTests.SomeClass();

            var bag = new KeyValueBag
            {
                ["Integer"] = "1001"
            };

            var reflect = new KeyValueReflection();
            var misses = reflect.SetValues(bag, obj);

            Assert.That(obj.Integer, Is.EqualTo(1001));
            Assert.That(misses, Is.EqualTo(0));
        }
    }
}