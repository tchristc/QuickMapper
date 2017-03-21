## Welcome to the QuickMapper

### Code
```cs
var person = new Person { FirstName = "Tom", LastName = "Test" };
var personDto = _mapper.Map<PersonDto, Person>(person);
```