using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickMapper.Tests
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GET_INFERRED_MAPPING_PERSON_TO_PERSON_DTO()
        {
            var inferredMapper = new InferredMapper();
            inferredMapper.Config();
            
            Assert.IsTrue(inferredMapper.MapTypes.Keys.Contains(typeof(Person)));
            Assert.IsTrue(inferredMapper.MapTypes.Keys.First().Name == "Person");
        }
    }
}
