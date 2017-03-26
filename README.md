## Welcome to the QuickMapper

### Download C# 
```
*Install-Package QuickMapper -Version 1.0.0
```

### CI Build
|Branch|AnyCPU|
|:--:|:--:|
|**master**|[![quickmapper-ci MyGet Build Status](https://www.myget.org/BuildSource/Badge/quickmapper-ci?identifier=581f5f6a-603c-4381-8b19-eebf295fccfa)](https://www.myget.org/feed/Packages/quickmapper-ci)

[QuickMapper - NuGet V3](https://www.myget.org/F/quickmapper-ci/api/v3/index.json)

[QuickMapper - NuGet V2](https://www.myget.org/F/quickmapper-ci/api/v2)


### Code
```cs
var person = new Person { FirstName = "Tom", LastName = "Test" };
var personDto = _mapper.Map<PersonDto, Person>(person);
```
