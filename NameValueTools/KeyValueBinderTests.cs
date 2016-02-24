using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GL.KeyValueTools
{
    [TestFixture]
    public class KeyValueBinderTests
    {

        [Test]
        public void FromUrl()
        {
            var bag = KeyValueBinder.FromURL("hello=world&age=12&empty=&extra");
            Assert.AreEqual("world", bag["hello"]);
        }
        
        public class SomeClass
        {
            public string String { get; set; } = "Bob";
            public bool Bool { get; set; } = true;
            public DateTime DateTime { get; set; } = new DateTime(2016, 2, 3);
            public int Integer { get; set; } = -123;
            public decimal Decimal { get; set; } = 123.456m;
        }

        [Test]
        public void FromObject()
        {
            var bag = KeyValueBinder.FromObject(new SomeClass());
            Assert.AreEqual("Bob", bag["String"]);
        }

        public class SomeOther
        {
            
        }

        public class Inheritance : SomeClass
        {
            public SomeOther Complex { get; set; } = new SomeOther();
            private int Hidden { get; set; } = 456;
        }

        [Test]
        public void FromObject_Inheritance()
        {
            var bag = KeyValueBinder.FromObject(new Inheritance());
            Assert.AreEqual("Bob", bag["String"]);
        }

        [Test]
        public void FromObject_Complex()
        {
            var bag = KeyValueBinder.FromObject(new Inheritance());
            Assert.AreEqual("GL.KeyValueTools.KeyValueBinderTests+SomeOther", bag["Complex"]);
        }

        [Test]
        public void FromObject_Private()
        {
            var bag = KeyValueBinder.FromObject(new Inheritance());
            Assert.Null(bag["Hidden"]);
        }

        [Test]
        public void FromJSON()
        {
            var json = JsonConvert.SerializeObject(new SomeClass());
            var bag = KeyValueBinder.FromJSON(json);
            //Console.WriteLine(json);
            //Console.WriteLine(bag);
            Assert.AreEqual("Bob", bag["String"]);
        }

        [Test]
        public void FromCommandLine()
        {
           
            var bag = KeyValueBinder.FromCommandLine(new string[]
            {
              "-hello:world",
              "-empty1:",
              "-empty2"
            });
            Console.WriteLine(bag);
            Assert.AreEqual("world", bag["Hello"]);
            Assert.AreEqual("", bag["empty1"]);
            Assert.That(bag.ContainsKey("empty2"));
        }

        [Test]
        public void ToStringXXX()
        {
            var bag = new KeyValueBag()
            {
                ["Hello"] = "world",
                ["Age"] = 40
            };
            Assert.That(bag.ToString("url"), Is.EqualTo("Hello=world&Age=40"));
        }


    }
}