using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Graphing.Helpers;

internal static class UpdaterHelper
{
    public static async Task<bool> UsingNugetPackage(string? project = null)
    {
        project ??= GetProjectPath();

        Process packageListProc = new();
        packageListProc.StartInfo.FileName = "dotnet";
        packageListProc.StartInfo.Arguments = "list package";
        packageListProc.StartInfo.WorkingDirectory = Path.GetDirectoryName(project);

        packageListProc.StartInfo.UseShellExecute = false;
        packageListProc.StartInfo.RedirectStandardOutput = true;

        StringBuilder contentBuilder = new();
        packageListProc.OutputDataReceived += (o, e) => contentBuilder.AppendLine(e.Data);

        packageListProc.Start();
        packageListProc.BeginOutputReadLine();
        await packageListProc.WaitForExitAsync();

        string content = contentBuilder.ToString();
        return content.Contains($"ThatOneNerd.Graphing");
    }
    public static async Task<bool> UpdateProjectByNuget(string version, string? project = null)
    {
        project ??= GetProjectPath();

        Process updateProc = new();
        updateProc.StartInfo.FileName = "dotnet";
        updateProc.StartInfo.Arguments = $"add package ThatOneNerd.Graphing --version {version}";
        updateProc.StartInfo.WorkingDirectory = Path.GetDirectoryName(project);

        updateProc.StartInfo.UseShellExecute = false;
        updateProc.StartInfo.RedirectStandardError = true;

        StringBuilder errorBuilder = new();
        updateProc.ErrorDataReceived += (o, e) => errorBuilder.AppendLine(e.Data);

        updateProc.Start();
        updateProc.BeginErrorReadLine();
        await updateProc.WaitForExitAsync();

        // Could be shrunk but it makes it less clear. If there's any data written to
        // the error stream, it did not succeed.
        if (errorBuilder.Length > 0) return false; // Error.
        else return true; // Success.
    }
    public static async Task<bool> UpdateProjectByGitHub(string version, string? project = null)
    {
        // TODO
        return false;
    }

    public static string? GetProjectPath()
    {
        Assembly? entryAsm = Assembly.GetEntryAssembly();
        if (entryAsm is null) return null;

        string? directory = Path.GetDirectoryName(entryAsm.Location);
        while (directory is not null)
        {
            string[] files = Directory.GetFiles(directory);
            string? project = files.FirstOrDefault(x => x.EndsWith(".csproj") ||
                                                        x.EndsWith(".sln"));
            if (project is not null) return project;

            directory = Path.GetDirectoryName(directory);
        }

        return null;
    }
}
