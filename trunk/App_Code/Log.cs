using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.IO;
using MTBScout;

/// <summary>
/// Summary description for Log
/// </summary>
public class Log
{
   
    private static string logFile;
    public enum MsgType { info, warning, error }
    public static void Add(MsgType type, string message)
    {
        new Thread((ThreadStart)delegate
            {
                try
                {
                    lock (typeof(Log))
                    {
                        string file = GetLogFile();
                        using (StreamWriter sw = new StreamWriter(file, true))
                        {
                            sw.WriteLine(string.Concat(DateTime.Now.ToString(), " - ", GetType(type), " - ", message));
                            sw.WriteLine();
                        }
                    }
                }
                catch
                {

                    throw;
                }
            }).Start();
    }

    private static string GetType(MsgType type)
    {
        switch (type)
        {
            case MsgType.info:
                return "Information";
            case MsgType.warning:
                return "Warning";
            case MsgType.error:
                return "Error";
            default:
                return "Error";
        }
    }

    private static string GetLogFile()
    {
        if (string.IsNullOrEmpty(logFile))
        {
            logFile = Path.Combine(PathFunctions.LogPath, "log1.log");
        }
        while (!IsValid(logFile))
        {
            string number = Path.GetFileName(logFile).Replace("log", "").Replace(".", "");
            int next = int.Parse(number);
            next++;
            logFile = Path.Combine(PathFunctions.LogPath, string.Format("log{0}.log", next));
        }
        return logFile;

    }

    private static bool IsValid(string logFile)
    {
        if (!File.Exists(logFile))
            return true;
        FileInfo fi = new FileInfo(logFile);
        return fi.Length < 32768;
    }
    public static void Add(MsgType type, string message, params object[] args)
    {
        Add(type, string.Format(message, args));
    }
    public static void Add(string message, params object[] args)
    {
        Add(MsgType.error, string.Format(message, args));
    }
    public static void Add(string message)
    {
        Add(MsgType.error, message);
    }
}
