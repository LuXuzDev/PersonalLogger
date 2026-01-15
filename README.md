# PersonalLogger

**PersonalLogger** es un logger ligero y simple para aplicaciones .NET, diseñado para ofrecer:
- Registro en consola principal
- Persistencia en archivo de logs UTF-8
- Seguimiento en tiempo real mediante una consola PowerShell independiente (Windows)

Ideal para proyectos de consola, herramientas internas, scripts y utilidades donde se necesita un logging claro sin dependencias pesadas.

---

## ✨ Características

- ✔️ Logger estático y fácil de usar
- ✔️ Escritura en archivo de logs (UTF-8)
- ✔️ Consola PowerShell independiente para monitoreo en tiempo real
- ✔️ Soporte para múltiples tipos de log
- ✔️ Sin dependencias externas
- ✔️ Compatible con .NET 8 / .NET 9

---

## 📦 Instalación

Desde la terminal:

```bash
dotnet add package LuxuzDev.PersonalLogger
```
---

# 🚀 Uso básico

``` c#
using LuxuzDev.PersonalLogger;

class Program
{
    static void Main()
    {
        PersonalLogger.Initialize(); //Importante comando para iniciar el logger

        PersonalLogger.Log("Aplicación iniciada");
        PersonalLogger.Log("Proceso exitoso", LogType.Success);
        PersonalLogger.Log("Advertencia detectada", LogType.Warning);
        PersonalLogger.Log("Error crítico", LogType.Error);
        PersonalLogger.Log("Mensaje de depuración", LogType.Debug);
    }
}

```
---

# 🧾 Tipos de log disponibles

``` c#
public enum LogType
{
    Info,
    Success,
    Warning,
    Error,
    Debug
}

```
# 🖥️ Consola PowerShell (Windows)

En Windows, el logger abre automáticamente una consola PowerShell independiente que permite:
 - Ver logs en tiempo real
 - Mantener el seguimiento separado de la consola principal
 - No interferir con la entrada del usuario
 - En otros sistemas operativos, el logger funciona sin la consola extra.

---

# 📂 Archivo de logs

Por defecto, los logs se guardan en:
``` bash
<directorio de la app>/Logs/personal.log
```

También puedes definir una ruta personalizada:
``` c#
PersonalLogger.Initialize("C:/logs/mi_aplicacion.log");
```
