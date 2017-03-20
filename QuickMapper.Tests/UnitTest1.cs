using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickMapper.Compiler;

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

    public interface IQuickMapper<L,R>
    {
        L Map(R right);
    }

    public class QuickMapperPersonToPersonDto : IQuickMapper<Person, PersonDto>
    {
        public Person Map(PersonDto right)
        {
            return new Person
            {
                FirstName = right.FirstName,
                LastName = right.LastName
            }; 
        }
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

        [TestMethod]
        public void COMPILER_TEST()
        {
            var compiler = new QuickMapperCompiler();
            compiler.Test();
        }
    }
}
