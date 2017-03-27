# QuickMapper.NuGetPackage Readme

Mapper mapper  = new Mapper();
var person = new Person { FirstName = "Tom", LastName = "Test" };
var personDto = mapper.Map<PersonDto, Person>(person);