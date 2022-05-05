Minimal reproduction case for a bug that where WPF projects fail to compile if they
* reference a ResX file from within a XAML file and
* use the `System.Text.Json` source generator at the same time.

# Points of interest
`MainWindow.xaml` directly accesses UI texts from `Resources.resx`:

```xml
<Window x:Class="WpfApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="clr-namespace:WpfApp"
        Title="{x:Static Member=local:Resources.WindowTitle}"
        Height="450" Width="800">
</Window>
```

Independently `Json.cs` contains the serializer context for source generator based JSON serialization:
```csharp
public class Foo
{
    int Bar { get; set; }
}

[JsonSerializable(typeof(Foo))]
public partial class FooSerializerContext : JsonSerializerContext
{
}
```

This produces the following errors during compilation:
```
<path>\Json.cs(11,22): error CS0534: 'FooSerializerContext' does not implement inherited abstract member 'JsonSerializerContext.GetTypeInfo(Type)' [<path>\WpfApp_uelfbtyn_wpftmp.csproj]
<path>\Json.cs(11,22): error CS0534: 'FooSerializerContext' does not implement inherited abstract member 'JsonSerializerContext.GeneratedSerializerOptions.get' [<path>\WpfApp_uelfbtyn_wpftmp.csproj]
```

The error goes away, when removing the following lines from `MainWindow.xaml`:
```
xmlns:local="clr-namespace:WpfApp"
Title="{x:Static Member=local:Resources.WindowTitle}"
```

It can also be worked around by adding a `PackageReference` to `System.Text.Json` to the project file
```xml
<ItemGroup>
  <PackageReference Include="System.Text.Json" Version="6.0.3" />
</ItemGroup>
```
