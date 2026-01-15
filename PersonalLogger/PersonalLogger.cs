using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LuxuzDev.PersonalLogger;

public static class PersonalLogger
{
    private static string _logFilePath =
        Path.Combine(AppContext.BaseDirectory, "Logs", "personal.log");

    private static Process? _consoleProcess;
    private static bool _initialized = false;

    public static void Initialize(string? logFilePath = null)
    {
        if (_initialized) return;

        if (!string.IsNullOrWhiteSpace(logFilePath))
            _logFilePath = logFilePath;

        // Crear carpeta de logs
        var logDir = Path.GetDirectoryName(_logFilePath)!;
        Directory.CreateDirectory(logDir);

        _initialized = true;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            StartConsole();
            System.Threading.Thread.Sleep(1000);
        }
    }

    public static void Log(string message, LogType type = LogType.Info)
    {
        if (!_initialized)
            Initialize();

        string typeName = type.ToString().ToUpper();
        string logMessage = $"[{typeName}] {DateTime.Now:HH:mm:ss} {message}";

        // Consola principal (sin colores)
        Console.WriteLine(logMessage);

        // Guardar en archivo UTF-8
        File.AppendAllText(_logFilePath, logMessage + Environment.NewLine, Encoding.UTF8);
    }

    private static void StartConsole()
    {
        if (_consoleProcess != null && !_consoleProcess.HasExited)
            return;

        try
        {
            // Script de PowerShell para colorear según tipo
            string psScript = @"
Get-Content -Path '" + _logFilePath + @"' -Wait -Tail 0 |
ForEach-Object {
    $line = $_
    if ($line.StartsWith('[INFO]')) { Write-Host $line -ForegroundColor Cyan }
    elseif ($line.StartsWith('[SUCCESS]')) { Write-Host $line -ForegroundColor Green }
    elseif ($line.StartsWith('[WARNING]')) { Write-Host $line -ForegroundColor Yellow }
    elseif ($line.StartsWith('[ERROR]')) { Write-Host $line -ForegroundColor Red }
    elseif ($line.StartsWith('[DEBUG]')) { Write-Host $line -ForegroundColor Gray }
    else { Write-Host $line }
}";

            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -Command \"{psScript}\"",
                UseShellExecute = true, // Ventana independiente
                CreateNoWindow = false
            };

            _consoleProcess = Process.Start(psi);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ No se pudo abrir la consola PowerShell extra: {ex.Message}");
        }

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            try
            {
                if (_consoleProcess != null && !_consoleProcess.HasExited)
                    _consoleProcess.Kill();
            }
            catch { }
        };
    }
}
