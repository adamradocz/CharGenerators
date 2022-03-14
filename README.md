# CharGenerators
A source generator for generating fast methods for chars.

Based on: Andrew Lock's [NetEscapades.EnumGenerators](https://github.com/andrewlock/NetEscapades.EnumGenerators) project and [Creating an incremental generator](https://andrewlock.net/creating-a-source-generator-part-1-creating-an-incremental-source-generator/) article.

> This source generator requires the .NET 6 SDK. You can target earlier frameworks like .NET Core 3.1 etc, but the _SDK_ must be at least 6.0.100

## Usage

Add the `CharGenerators` to your project. You can additionally mark the package as `PrivateAsets="all"` and `ExcludeAssets="runtime"` in the `<PackageReference>`.

- `PrivateAssets="all"` means any projects referencing this one won't get a reference to the _CharGenerators_ package.
- `ExcludeAssets="runtime"` ensures the _CharGenerators.Attributes.dll_ file is not copied to your build output (it is not required at runtime).

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <!-- Add the package -->
  <PackageReference Include="CharGenerators" Version="1.0.0" PrivateAssets="all" ExcludeAssets="runtime" />
  <!-- -->

</Project>
```

Adding the package will automatically add a marker attribute, `[CharExtensions]`, to your project.
To use the generator, add the `[CharExtensions]` attribute to a class. For example:

```csharp
using CharGenerators;

namespace MyNamespace
{
    [CharExtensions(Global = true)]
    public class MyClass
    {
    }
}
```

This will generate a class called `CharExtensions`, which contains the `ToStringFast()` extension method and optimized for printable ASCII characters. For example:

```csharp
public static partial class CharExtensions
{
    public static string ToStringFast(this char value)
        => value switch
        {
            ' ' => " ",
            '!' => "!",
            '"' => "\"",
            '#' => "#",
            '$' => "$",
            '%' => "%",
            '&' => "&",
            '\'' => "'",
            '(' => "(",
            ')' => ")",
            '*' => "*",
            '+' => "+",
            ',' => ",",
            '-' => "-",
            '.' => ".",
            '/' => "/",
            '0' => "0",
            '1' => "1",
            '2' => "2",
            '3' => "3",
            '4' => "4",
            '5' => "5",
            '6' => "6",
            '7' => "7",
            '8' => "8",
            '9' => "9",
            ':' => ":",
            ';' => ";",
            '<' => "<",
            '=' => "=",
            '>' => ">",
            '?' => "?",
            '@' => "@",
            'A' => "A",
            'B' => "B",
            'C' => "C",
            'D' => "D",
            'E' => "E",
            'F' => "F",
            'G' => "G",
            'H' => "H",
            'I' => "I",
            'J' => "J",
            'K' => "K",
            'L' => "L",
            'M' => "M",
            'N' => "N",
            'O' => "O",
            'P' => "P",
            'Q' => "Q",
            'R' => "R",
            'S' => "S",
            'T' => "T",
            'U' => "U",
            'V' => "V",
            'W' => "W",
            'X' => "X",
            'Y' => "Y",
            'Z' => "Z",
            '[' => "[",
            '\\' => "\\",
            ']' => "]",
            '^' => "^",
            '_' => "_",
            '`' => "`",
            'a' => "a",
            'b' => "b",
            'c' => "c",
            'd' => "d",
            'e' => "e",
            'f' => "f",
            'g' => "g",
            'h' => "h",
            'i' => "i",
            'j' => "j",
            'k' => "k",
            'l' => "l",
            'm' => "m",
            'n' => "n",
            'o' => "o",
            'p' => "p",
            'q' => "q",
            'r' => "r",
            's' => "s",
            't' => "t",
            'u' => "u",
            'v' => "v",
            'w' => "w",
            'x' => "x",
            'y' => "y",
            'z' => "z",
            '{' => "{",
            '|' => "|",
            '}' => "}",
            '~' => "~",
            _ => value.ToString()
        };
}
```

**Attribute settings:**
- `OptimizeFor`: Override to which characters to optimize for. It's optimized for printable ASCII characters by default.
- `Global`: `true` generates a public extension method. `false` generates a private helper method for the class. It's `false` by default.

For example:

```csharp
using CharGenerators;

namespace MyNamespace
{
    [CharExtensions(OptimizeFor = "0123456789")]
    public partial class MyClass
    {
    }
}";
```

This will generate a `partial class MyClass` class, which contains the `CharToStringFast()` helper method.

```csharp
namespace MyNamespace
{
    public partial class MyClass
    {
        private string CharToStringFast(char value)
            => value switch
            {
                '0' => "0",
                '1' => "1",
                '2' => "2",
                '3' => "3",
                '4' => "4",
                '5' => "5",
                '6' => "6",
                '7' => "7",
                '8' => "8",
                '9' => "9",
                _ => value.ToString()
            };
    }
}
```

## Embedding the attributes in your project

By default, the `[CharExtensions]` attributes referenced in your application are contained in an external dll. It is also possible to embed the attributes directly in your project, so they appear in the dll when your project is built. If you wish to do this, you must do two things:

1. Define the MSBuild constant `CAHRGENERATORS_EMBED_ATTRIBUTES`. This ensures the attributes are embedded in your project.
2. Add `compile` to the list of excluded assets in your `<PackageReference>` element. This ensures the attributes in your project are referenced, instead of the CharGenerators.Attributes.dll_ library.

Your project file should look something like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <!-- Define the MSBuild constant -->
    <DefineConstants>CAHRGENERATORS_EMBED_ATTRIBUTES</DefineConstants>
  </PropertyGroup>

  <!-- Add the package -->
  <PackageReference Include="CharGenerators" Version="1.0.0" PrivateAssets="all" ExcludeAssets="compile;runtime" />
  <!-- ☝ Add compile to the list of excluded assets. -->

</Project>
```

## Preserving usages of the `[CharExtensions]` attribute

The `[CharExtensions]` attribute is decorated with the `[Conditional]` attribute, [so their usage will not appear in the build output of your project](https://andrewlock.net/conditional-compilation-for-ignoring-method-calls-with-the-conditionalattribute/#applying-the-conditional-attribute-to-classes). If you use reflection at runtime on one of your `class`es, you will not find `[CharExtensions]` in the list of custom attributes.

If you wish to preserve these attributes in the build output, you can define the `CHARGENERATORS_USAGES` MSBuild variable. Note that this means your project will have a runtime-dependency on _CharGenerators.Attributes.dll_ so you need to ensure this is included in your build output.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <!-- Define the MSBuild constant to preserve usages -->
    <DefineConstants>CHARGENERATORS_USAGES</DefineConstants>
  </PropertyGroup>

  <!-- Add the package -->
  <PackageReference Include="CharGenerators" Version="1.0.0" PrivateAssets="all" />
  <!-- ☝ You must not exclude the runtime assets in this case -->

</Project>
```

## Error CS0436 and [InternalsVisibleTo]

> In the latest version of _CharGenerators_, you should not experience error CS0436 by default.

If you added the source generator package to multiple projects, and used the `[InternalsVisibleTo]` attribute, you could experience errors when you build:

```bash
warning CS0436: The type 'CharExtensionsAttribute' in 'CharGenerators\CharExtensionsAttribute.cs' conflicts with the imported type 'CharExtensionsAttribute' in 'MyProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'.
```
