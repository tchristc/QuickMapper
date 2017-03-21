using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickMapper.Tests
{
    [TestClass]
    public class QuickMapperTests
    {
        private Mapper _mapper;

        [TestInitialize]
        public void Initialize()
        {
            _mapper = new Mapper();
        }

        [TestMethod]
        public void MAP_PERSON_TO_PERSON_DTO()
        {
            var personDto = new PersonDto {FirstName = "Tom", LastName = "Test"};
            var person = _mapper.Map<Person, PersonDto>(personDto);
            Assert.IsTrue(personDto.FirstName == person.FirstName);
            Assert.IsTrue(personDto.LastName == person.LastName);
        }

        [TestMethod]
        public void MAP_PERSON_DTO_TO_PERSON()
        {
            var person = new Person { FirstName = "Tom", LastName = "Test" };
            var personDto = _mapper.Map<PersonDto, Person>(person);
            Assert.IsTrue(personDto.FirstName == person.FirstName);
            Assert.IsTrue(personDto.LastName == person.LastName);
        }
    }
}
