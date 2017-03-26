## Welcome to the QuickMapper

### Download C# 
```
*Install-Package QuickMapper -Version 1.0.0
```

### CI Build
|Branch|AnyCPU|
|:--:|:--:|
|**master**|[![quickmapper-ci MyGet Build Status](https://www.myget.org/BuildSource/Badge/quickmapper-ci?identifier=f675cb57-f9ac-4634-87fe-2a3454c5d3ed)](https://www.myget.org/feed/Packages/quickmapper-ci)

[QuickMapper - NuGet V3](https://www.myget.org/F/quickmapper-ci/api/v3/index.json)

[QuickMapper - NuGet V2](https://www.myget.org/F/quickmapper-ci/api/v2)


### Code
```cs
var person = new Person { FirstName = "Tom", LastName = "Test" };
var personDto = _mapper.Map<PersonDto, Person>(person);
```
