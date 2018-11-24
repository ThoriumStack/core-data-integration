## Use Case

Many external integrations make use of specific text based file formats delivered via protocols other than http. The data integration service makes these types of legacy integrations simple and based on modern c# programming concepts such as automatic serialization/deserialization and easy access to common transport protocols. 

## Concepts

Any data integration implementation is based on four distinct interchangeable components, namely a serializer, a transport, various pre-processors and type converters.

### Serializer

Any serializer can be built for the data integration service, whether it be CSV, flat files or even Morse code, as long as the concrete implements `IIntegrationDataSerializer`. The serializer takes class objects and converts them to a `MemoryStream` for the transport, or reads a `MemoryStream` from a transport and converts data back to strongly typed C# objects. 

### Transport

The transport reads data from an external source or writes it back to an external source. Any transport only has to inherit from `IIntegrationTransport` and implement the required methods. Any kind of transport can be defined from FTP to scp to local files to archives. 

### It all comes together

You only require a serializer and a transport to get data to and from an external integration party. The serializer takes objects and converts them to a `MemoryStream` and the given transport takes that data and sends it over a network, or to a local file system or wherever you wish. When data is read from the third party the transport passes the raw data to the serializer for conversion back to C# objects.

## Simple Example

Send data to a local file in CSV format. 

```csharp
var a = new Integrator();
var planets = new List<Planet>()
{
    new Planet() { Name="Mercury", DistanceFromSun = 57.91, OrderFromSun = 1 },
    new Planet() { Name="Venus", DistanceFromSun = 108.2, OrderFromSun = 2 },
    new Planet() { Name="Earth", DistanceFromSun = 149.6, OrderFromSun = 3 },
    new Planet() { Name="Mars", DistanceFromSun = 227.9, OrderFromSun = 4 },
    new Planet() { Name="Jupiter", DistanceFromSun = 778.5, OrderFromSun = 5 },
    new Planet() { Name="Saturn", DistanceFromSun = 1429, OrderFromSun = 6 },
    new Planet() { Name="Uranus", DistanceFromSun = 2877, OrderFromSun = 7 },
    new Planet() { Name="Neptune", DistanceFromSun = 4498, OrderFromSun = 8 },
};
var csvSerializer = new CsvSerializer
{
    Delimiter = "|",
    HasHeaderRecord = true
};
var transport = new LocalFileTransport { FilePath = $"C:\\temp\\testplanets.csv" };
var result = a.SendData(planets, csvSerializer, transport);
```

Read data back from a CSV file on the local filesystem

```csharp
var a = new Modules.DataIntegration.Service.DataIntegrationService();

var csvSerializer = new CsvSerializer
{
    Delimiter = "|",
    HasHeaderRecord = true
};
var transport = new LocalFileTransport { FilePath = $"C:\\temp\\testplanets.csv" };
var result = a.ReceiveAsyncData<Planet>(csvSerializer, transport);
return;
```

## Using the builder for more complex files

Some external parties have files that contain different types of records in the same files. Such files usually have a header, body with multiple records and a footer record, but this may vary according to the integration. The input and Output builders of the data integration service can cater for such complex files by allowing a definition of the file format to be given to a builder to create or read such files. 

Below is an example of how to create a file with a header, body items and a footer using the `DataIntegrationOutputBuilder`

### Our example classes

```csharp
public class Planet
{
    public string RecordType { get; set; } = "PLANET";
    public string Name { get; set; }
    public int OrderFromSun { get; set; }
    /// <summary>
    /// In millions of kilometers
    /// </summary>
    public double DistanceFromSun { get; set; }
}

public class StellarSystem
{
    public string RecordType { get; set; } = "STELLARSYSTEM";
    public string StarType { get; set; }
    public bool IsBinarySystem { get; set; }
    public string Name { get; set; }
    public double Radius { get; set; }
}

public class Galaxy
{
    public string RecordType { get; set; } = "GALAXY";
    public string Name { get; set; } = "Milky Way";
    public string Type { get; set; } = "Spiral";
    public string Address { get; set; } = "Local Group";
}
```

### Using the output builder

```csharp
var a = new Modules.DataIntegration.Service.DataIntegrationService();

var star = new StellarSystem()
{
    IsBinarySystem = false,
    Name = "Sol",
    StarType = "Yellow Dwarf",
    Radius = 695.700
};
var planets = new List<Planet>()
{
    new Planet() { Name="Mercury", DistanceFromSun = 57.91, OrderFromSun = 1 },
    new Planet() { Name="V#e#n#u#s", DistanceFromSun = 108.2, OrderFromSun = 2 },
    new Planet() { Name="_êarth_", DistanceFromSun = 149.6, OrderFromSun = 3 },
    new Planet() { Name="((Mars))", DistanceFromSun = 227.9, OrderFromSun = 4 },
    new Planet() { Name="Jupitër", DistanceFromSun = 778.5, OrderFromSun = 5 },
    new Planet() { Name="Sa____turn", DistanceFromSun = 1429, OrderFromSun = 6 },
    new Planet() { Name="<Uranus>", DistanceFromSun = 2877, OrderFromSun = 7 },
    new Planet() { Name="Nep~~~~~~~tune!!!%", DistanceFromSun = 4498, OrderFromSun = 8 },
};
var csvSerializer = new CsvSerializer
{
    Delimiter = "|",
    HasHeaderRecord = false,

};
var transport = new LocalFileTransport { FilePath = $"C:\\temp\\broken.csv" };
var build = new DataIntegrationOutputBuilder()
    .SetSerializer(csvSerializer)
    .AddPreProcessor(new DiacriticRemover())
    .AddPreProcessor(new RegexRemover(@"[^a-zA-Z0-9/\.\-+&><=*,;'\(\)$]+"))
    .AddData(star)
    .AddListData(planets)
    .AddData(new Galaxy() {Name = "Alpha Centauri"});

    var result = a.SendAsyncData(build, transport);

```

Data is added sequentially to the output, but data is only actually read from the input variables when the `.build()` method is called on the output builder object. In this case, an overload of `SendAsyncData()` will call build and create the output for sending via the transport.

Below is an example of using the `DataIntegrationInputBuilder`. In this example data is read into C# objects by specifying which records in a file match which type with a Discriminator object

### Our input file:
```
STELLARSYSTEM|YellowDwarf|False|Sol|695.7
PLANET|Mercury|1|57.91
PLANET|Venus|2|108.2
PLANET|earth|3|149.6
PLANET|((Mars))|4|227.9
PLANET|Jupiter|5|778.5
PLANET|Saturn|6|1429
PLANET|<Uranus>|7|2877
PLANET|Neptune|8|4498
GALAXY|AlphaCentauri|Spiral|LocalGroup
```
### Code Example:
```csharp

var star = new StellarSystem();
var planets = new List<Planet>();
var galaxy = new Galaxy();

var csvSerializer = new CsvSerializer
{
    Delimiter = "|",
    HasHeaderRecord = false,

};
var builtFile = BuilderTest().Build(); // input data is faked. Would normally come through a transport
          
var nb = new DataIntegrationInputBuilder()
    .SetData(builtFile)
    .SetSerializer(csvSerializer)
    .ReadOnce<StellarSystem, FirstFieldDiscriminator<string>>(system => star = system, discriminator => discriminator.Value == "STELLARSYSTEM")
    .ReadMany<Planet, FirstFieldDiscriminator<string>>(planets, discriminator => discriminator.Value == "PLANET")
    .ReadOnce<Galaxy, FirstFieldDiscriminator<string>>(galaxy1 => galaxy = galaxy1, discriminator => discriminator.Value == "GALAXY");

nb.Build();
```

Calls to `ReadOnce()` specify which records are only expected once in a file, and a lambda is passed in to assign the value to an output system. Calls to ReadMany() specify record types that will occur multiple times in a file, and will be pushed into the specified `List<T>`.
