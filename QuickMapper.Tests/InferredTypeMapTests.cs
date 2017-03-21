using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickMapper.Tests
{
    [TestClass]
    public class InferredTypeMapTests
    {
        [TestMethod]
        public void GET_INFERRED_MAPPING_PERSON_TO_PERSON_DTO()
        {
            var inferredMapper = new InferredTypeMap();
            inferredMapper.Config();

            Assert.IsTrue(inferredMapper.MapTypes.Keys.Contains(typeof(Person)));
            Assert.IsTrue(inferredMapper.MapTypes.Keys.First().Name == "Person");
        }

    }
}
