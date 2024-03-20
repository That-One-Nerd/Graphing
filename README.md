# Graphing

This is a graphing calculator I made initially for a Calculus project in a day or so. I've written a basic rendering system in Windows Forms that runs on .NET 8.0.

Currently, it doesn't have a whole lot of features, but I'll be adding more in the future. Here's currently what it can do:
- Graph an equation (duh).
    - There are currently some rendering issues with asymptotes which will be focused on at some point.
- Integrate and derive equations.
- Graph a slope field of a `dy/dx =` style equation.
- View a tangent line of an equation.
- Display a vertical bar graph.

However, you can develop your own features as well.

The system does not and likely will not (at least for a while) support text-to-equation parsing. You must import this project as a library and add graphs that way.

There are some tools in the menu bar that can assist but those are fairly limited and will be added on to in the future. For now, you can drag to move the graph and use the mouse wheel to zoom. Fairly self-explanatory, I think.

## How to Install

This project is a NuGet package, so if you want to install it that way, you can do so.

- In the terminal for your project, run `dotnet add package ThatOneNerd.Graphing`
- Alternatively, you can search for the package called "ThatOneNerd.Graphing" in the Visual Studio NuGet Package Manager.
- If you have an alternative package manager and know how to use it, then do that instead obviously.

---

You can also directly import the DLL file. Go to the latest release and download the ZIP folder called "ThatOneNerd.Graphing.zip". Extract the contents somewhere you can access.
- In Visual Studio, you can then right click your project in the solution explorer, go to Add > Project Reference, and browse for the DLL called "ThatOneNerd.Graphing.dll"
- If you want to edit the project file itself, you can. Open the `.csproj` file and add the following lines to the XML:
```xml
<ItemGroup>
  <Reference Include="Graphing">
    <HintPath>FILE PATH GOES HERE (can be relative)</HintPath>
  </Reference>
</ItemGroup>
```
- If you have a different IDE, I don't know how to help you with that.

## How to Use

Once you've installed the package, you just need to use standard Windows Forms startup code.

```csharp
using Graphing.Forms;
using Graphing.Graphables;

internal static class Program
{
    [STAThread]
    public static void Main()
    {
        // Optional configuration.
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        // Create the graph form and give it a name.
        GraphForm graph = new("Form Name");

        // Graph the equation x^2.
        graph.Graph(new Equation(x => x * x));

        // Display the graph.
        Application.Run(graph);
    }
}
```

That's it. Not bad, eh?

An equation requires a delegate such as the one you see. Alternatively, you can change the name and color of the equation.
```csharp
graph.Graph(new Equation(x => Math.Pow(2, x))
{
    Color = Color.Green,
    Name = "Exponential Base 2"
});
```

Default colors and names are assigned if none are provided.

Slope fields work quite the same way, but with a delegate taking in both an `x` and a `y`.
```csharp
graph.Graph(new SlopeField((x, y) => -y / x)
{
    Color = Color.DarkRed,
    Name = "Slope Field Example"
});
```

You've got the hang of this. I'll be adding more features for a while.
