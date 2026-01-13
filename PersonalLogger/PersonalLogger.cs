using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PersonalLogger;


public static class PersonalLogger
{
    private static string _logFilePath =
        Path.Combine(AppContext.BaseDirectory, "Logs", "personal.log");

    private static Process? _consoleProcess;
    private static bool _initialized;

    public static void Initialize(string? logFilePath = null)
    {
        if (_initialized) return;

        if (!string.IsNullOrWhiteSpace(logFilePath))
            _logFilePath = logFilePath;

        var logDir = Path.GetDirectoryName(_logFilePath)!;
        Directory.CreateDirectory(logDir);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            StartConsole();
        }

        _initialized = true;
        Log("PersonalLogger inicializado.");
    }

    public static void Log(string message)
    {
        if (!_initialized)
            Initialize();

        var logMessage = $"[PERSONAL] {DateTime.Now:HH:mm:ss} {message}";

        File.AppendAllText(
            _logFilePath,
            logMessage + Environment.NewLine + Environment.NewLine,
            Encoding.UTF8
        );

        Console.WriteLine(logMessage);
    }


    private static void StartConsole()
    {
        if (_consoleProcess != null && !_consoleProcess.HasExited)
            return;

        string args =
            "-Command " +
            "\"chcp 65001; " + // UTF-8
            "$Host.UI.RawUI.BackgroundColor='Black'; " +
            "$Host.UI.RawUI.ForegroundColor='Green'; " +
            "Clear-Host; " +
            "$OutputEncoding = [System.Text.UTF8Encoding]::new(); " +
            $"Get-Content -Path '{_logFilePath}' -Wait -Tail 0\"";

        try
        {
            _consoleProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = args,
                UseShellExecute = true,
                CreateNoWindow = false
            });

            // Evento que se ejecuta cuando el programa principal termina
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                try
                {
                    if (_consoleProcess != null && !_consoleProcess.HasExited)
                        _consoleProcess.Kill();
                }
                catch { }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ No se pudo abrir consola externa: {ex.Message}");
        }
    }
}