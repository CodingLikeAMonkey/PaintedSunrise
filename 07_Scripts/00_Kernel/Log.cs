using System;
namespace Kernel;
#nullable enable
public static class Log
{
    public static Action<string>? Info;
    public static Action<string>? Warn;
    public static Action<string>? Error;

    public static void PrintInfo(string message) => Info?.Invoke(message);
    public static void PrintWarn(string message) => Warn?.Invoke(message);
    public static void PrintError(string message) => Error?.Invoke(message);
}
