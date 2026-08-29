using Nerd_STF.Mathematics;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Graphing;

// A maybe overcomplicated graph point. It can support as many dimensions as required
// and even supports conversions between axes (for example, conversion of XYZ to polar).
//
// As I've developed this, I'm beginning to think it's easier to make a larger variable
// system and consider components as part of that variable. It would make some conversions
// easier.
public readonly struct GraphPoint() : IEnumerable<(string, double)>, IEquatable<GraphPoint>
{
    private static readonly List<string> rootComponents = ["x", "y", "z", "w", "t", "u", "v"];
    private readonly Dictionary<string, double> components = [];

    public GraphPoint(IEnumerable<(string, double)> nums) : this()
    {
        foreach ((string, double) tuple in nums) Add(tuple);
    }

    public double this[string dim] { get => Get(dim); set => Set(dim, value); }

    public void Add(string dim, double v) => Set(dim, v);
    public void Add((string dim, double v) tuple) => Set(tuple.dim, tuple.v);
    public IEnumerator<(string, double)> GetEnumerator() => (from kvp in components select (kvp.Key, kvp.Value)).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public double Get(string dim)
    {
        // See if we have the exact component desired.
        if (components.TryGetValue(dim, out double v)) return v;
        else if (specialConversions.TryGetValue(dim, out var convert)) return convert(in this);

        // See if we have the necessary input component(s) to convert to the desired.
        var possibles = conversions.Where(c => c.Key.outputs.Contains(dim));
        int maxInputs = possibles.Any() ? possibles.Max(c => c.Key.inputs.Count) : 0;
        Span<double> inputs = stackalloc double[maxInputs];
        foreach (var possible in possibles)
        {
            // This conversion can generate what we need.
            // Make sure we have all the necessary values.
            if (!possible.Key.inputs.All(components.ContainsKey)) continue;

            // We can assign values during this step too, if we're smart about it.
            for (int i = 0; i < possible.Key.inputs.Count; i++)
            {
                if (components.TryGetValue(possible.Key.inputs[i], out double temp)) inputs[i] = temp;
                else continue; // This conversion requires an input we don't have. Skip it.
            }

            // If we assign all the values, we're good to complete the conversion.
            // We can add the converted values to our component store.
            IList<double> result = possible.Value(inputs);
            int returnIndex = possible.Key.outputs.IndexOf(dim);
            for (int i = 0; i < possible.Key.outputs.Count; i++)
            {
                double component = result[i];
                if (!components.TryAdd(dim, component)) components[dim] = component;
                if (i == returnIndex) v = component;
            }
            return v;
        }

        // Otherwise, it doesn't have a value.
        return 0;
    }
    public void Set(string dim, double v)
    {
        if (specialConversions.ContainsKey(dim)) throw new ArgumentException($"'{dim}' is a readonly component.");

        // Add or update our base value.
        if (!components.TryAdd(dim, v)) components[dim] = v;

        // If this value is used in a conversion, we need to invalidate all
        // components that rely on this value.
        var dependents = conversions.Where(c => c.Key.inputs.Contains(dim));
        foreach (var possible in dependents)
        {
            // If this conversion is not fully fulfilled by our components,
            // then skip it. This is to prevent data loss.
            if (!possible.Key.inputs.All(components.ContainsKey)) continue;

            // Remove produced outputs.
            foreach (string output in possible.Key.outputs) components.Remove(output);
        }
    }
    public bool TryGetRaw(string dim, out double v) => components.TryGetValue(dim, out v);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is GraphPoint otherPoint) return Equals(otherPoint);
        else if (obj is Float2 otherFloat2) return Equals((GraphPoint)otherFloat2);
        else if (obj is Float3 otherFloat3) return Equals((GraphPoint)otherFloat3);
        else if (obj is Float4 otherFloat4) return Equals((GraphPoint)otherFloat4);
        else return false;
    }
    public bool Equals(GraphPoint other)
    {
        // Compare only the root components.
        // The others ideally shouldn't be necessary.
        foreach (string dim in rootComponents)
        {
            if (Get(dim) != other.Get(dim)) return false;
        }
        return true;
    }
    public override int GetHashCode()
    {
        int code = 0;
        foreach (string dim in rootComponents) code ^= Get(dim).GetHashCode();
        return code;
    }
    public override string ToString()
    {
        string[] dims = [.. components.Keys];
        Array.Sort(dims);
        StringBuilder result = new("{ ");
        for (int i = 0; i < dims.Length; i++)
        {
            string dim = dims[i];
            result.Append($"{dim} = {components[dim]}");
            if (i < dims.Length - 1) result.Append(',');
            result.Append(' ');
        }
        return result.Append('}').ToString();
    }

    // Conversions below.
    private delegate IList<double> DimConversionDelegate(ReadOnlySpan<double> inputs);
    private static readonly Dictionary<(IList<string> inputs, IList<string> outputs), DimConversionDelegate> conversions = new()
    {
        { (["x", "y"], ["theta", "r"]), ToPolar },
        { (["theta", "r"], ["x", "y"]), ToXyz },

        { (["r"], ["mag"]), (inputs) => inputs.ToArray() }
    };

    private static IList<double> ToPolar(ReadOnlySpan<double> inputs)
    {
        // Inputs: x, y
        // Outputs: theta, r

        double x = inputs[0], y = inputs[1];
        double theta, r;
        if (x == 0)
        {
            r = y;
            theta = Math.PI * 0.5 * Math.Sign(y);
        }
        else
        {
            r = Math.Sqrt(x * x + y * y);
            theta = Math.Atan2(y, x);
        }

        return [theta, r];
    }
    private static IList<double> ToXyz(ReadOnlySpan<double> inputs)
    {
        // Inputs: theta, r
        // Outputs: x, y

        double theta = inputs[0], r = inputs[1];
        return [r * Math.Cos(theta),
                r * Math.Sin(theta)];
    }

    private delegate double SpecialConversionDelegate(ref readonly GraphPoint point);
    private static readonly Dictionary<string, SpecialConversionDelegate> specialConversions = new()
    {
        { "mag", GetMagnitude }
    };

    private static double GetMagnitude(ref readonly GraphPoint point)
    {
        double x = point["x"], y = point["y"], z = point["z"], w = point["w"];
        return Math.Sqrt(x * x + y * y + z * z + w * w);
    }

    // Math operations.
    public static GraphPoint operator +(GraphPoint a, GraphPoint b) => new(from dim in rootComponents select (dim, a[dim] + b[dim]));
    public static GraphPoint operator -(GraphPoint a) => new(from dim in rootComponents select (dim, -a[dim]));
    public static GraphPoint operator -(GraphPoint a, GraphPoint b) => new(from dim in rootComponents select (dim, a[dim] - b[dim]));
    public static GraphPoint operator *(GraphPoint a, double b) => new(from dim in rootComponents select (dim, a[dim] * b));
    public static GraphPoint operator *(double a, GraphPoint b) => new(from dim in rootComponents select (dim, a * b[dim]));
    public static GraphPoint operator ^(GraphPoint a, GraphPoint b) => new(from dim in rootComponents select (dim, a[dim] * b[dim]));
    public static GraphPoint operator /(GraphPoint a, double b) => new(from dim in rootComponents select (dim, a[dim] / b));
    public static GraphPoint operator /(double a, GraphPoint b) => new(from dim in rootComponents select (dim, a / b[dim]));
    public static bool operator ==(GraphPoint a, GraphPoint b) => a.Equals(b);
    public static bool operator !=(GraphPoint a, GraphPoint b) => !a.Equals(b);

    // Tuple conversions.
    public static implicit operator GraphPoint((double x, double y) tuple) => [("x", tuple.x), ("y", tuple.y)];
    public static implicit operator GraphPoint((double x, double y, double z) tuple) => [("x", tuple.x), ("y", tuple.y), ("z", tuple.z)];
    public static implicit operator GraphPoint(Float2 floats) => [("x", floats.x), ("y", floats.y)];
    public static implicit operator GraphPoint(Float3 floats) => [("x", floats.x), ("y", floats.y), ("z", floats.z)];
    public static implicit operator GraphPoint(Float4 floats) => [("x", floats.x), ("y", floats.y), ("z", floats.z), ("w", floats.w)];
}
