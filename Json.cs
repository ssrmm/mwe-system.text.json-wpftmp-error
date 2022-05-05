using System.Text.Json.Serialization;

namespace WpfApp;

public class Foo
{
    int Bar { get; set; }
}

[JsonSerializable(typeof(Foo))]
public partial class FooSerializerContext : JsonSerializerContext
{
}
