using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace TWC.Util
{
  public static class Toolbox
  {
    private static Random random = new Random(Guid.NewGuid().GetHashCode());
    private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static string root = Directory.GetCurrentDirectory() + "\\";

    public static void CopyStream(Stream readStream, Stream writeStream)
    {
      byte[] buffer = new byte[15360];
      while (true)
      {
        int count = readStream.Read(buffer, 0, buffer.Length);
        if (count != 0)
          writeStream.Write(buffer, 0, count);
        else
          break;
      }
    }

    public static void CopyStream(Stream readStream, Stream writeStream, long numBytes)
    {
      long num1 = 0;
      byte[] buffer = new byte[15360];
      do
      {
        long num2 = Math.Min((long) buffer.Length, numBytes - num1);
        int count = readStream.Read(buffer, 0, (int) num2);
        if (count == 0)
          break;
        writeStream.Write(buffer, 0, count);
        num1 += (long) count;
      }
      while (num1 != numBytes);
    }

    public static void SetRoot(string newRoot)
    {
      Toolbox.root = newRoot;
    }

    public static void RootFromAssembly()
    {
      Toolbox.root = Assembly.GetExecutingAssembly().Location;
      Toolbox.root = Path.GetDirectoryName(Toolbox.root) + "\\";
    }

    public static string GetFilePath(string fname)
    {
      return Path.IsPathRooted(fname) ? fname : Toolbox.root + fname;
    }

    public static string GetTempFileName(string prefix)
    {
      return Toolbox.GetTempFileName("Data\\Temp\\", prefix);
    }

    public static string GetTempFileName(string path, string prefix)
    {
      return Toolbox.GetTempFileName(path, prefix, (string) null);
    }

    public static string GetTempFileName(string path, string prefix, string suffix)
    {
      return Toolbox.GetTempFileName(path, prefix, 6, suffix);
    }

    public static string GetTempFileName(string path, string prefix, int numRandomChars)
    {
      return Toolbox.GetTempFileName(path, prefix, numRandomChars, (string) null);
    }

    public static string GetTempFileName(
      string path,
      string prefix,
      int numRandomChars,
      string suffix)
    {
      using (FileStream tempFileStream = Toolbox.GetTempFileStream(path, prefix, numRandomChars, suffix))
        return tempFileStream.Name;
    }

    public static FileStream GetTempFileStream(string prefix)
    {
      return Toolbox.GetTempFileStream("Data\\Temp\\", prefix);
    }

    public static FileStream GetTempFileStream(string path, string prefix)
    {
      return Toolbox.GetTempFileStream(path, prefix, (string) null);
    }

    public static FileStream GetTempFileStream(string path, string prefix, string suffix)
    {
      return Toolbox.GetTempFileStream(path, prefix, 6, suffix);
    }

    public static FileStream GetTempFileStream(
      string path,
      string prefix,
      int numRandomChars)
    {
      return Toolbox.GetTempFileStream(path, prefix, numRandomChars, (string) null);
    }

    public static FileStream GetTempFileStream(
      string path,
      string prefix,
      int numRandomChars,
      string suffix)
    {
      string str = Toolbox.GetFilePath(path) + (object) '\\' + prefix;
      StringBuilder stringBuilder = new StringBuilder(260);
label_1:
      stringBuilder.Remove(0, stringBuilder.Length);
      stringBuilder.Append(str);
      for (int index = 0; index < numRandomChars; ++index)
        stringBuilder.Append(Toolbox.chars[Toolbox.random.Next(Toolbox.chars.Length - 1)]);
      if (suffix != null)
        stringBuilder.Append(suffix);
      string path1 = stringBuilder.ToString();
      try
      {
        return new FileStream(path1, FileMode.CreateNew);
      }
      catch 
      {
        if (!File.Exists(path1))
          throw;
        else
          goto label_1;
      }
    }

    public static DirectoryInfo GetTempDirectory(string prefix)
    {
      return Toolbox.GetTempDirectory("Data\\Temp\\", prefix);
    }

    public static DirectoryInfo GetTempDirectory(string path, string prefix)
    {
      return Toolbox.GetTempDirectory(path, prefix, (string) null);
    }

    public static DirectoryInfo GetTempDirectory(
      string path,
      string prefix,
      string suffix)
    {
      return Toolbox.GetTempDirectory(path, prefix, 6, suffix);
    }

    public static DirectoryInfo GetTempDirectory(
      string path,
      string prefix,
      int numRandomChars)
    {
      return Toolbox.GetTempDirectory(path, prefix, numRandomChars, (string) null);
    }

    public static DirectoryInfo GetTempDirectory(
      string path,
      string prefix,
      int numRandomChars,
      string suffix)
    {
      string str = Toolbox.GetFilePath(path) + (object) '\\' + prefix;
      StringBuilder stringBuilder = new StringBuilder(260);
label_1:
      stringBuilder.Remove(0, stringBuilder.Length);
      stringBuilder.Append(str);
      for (int index = 0; index < numRandomChars; ++index)
        stringBuilder.Append(Toolbox.chars[Toolbox.random.Next(Toolbox.chars.Length - 1)]);
      if (suffix != null)
        stringBuilder.Append(suffix);
      string path1 = stringBuilder.ToString();
      try
      {
        return Directory.CreateDirectory(path1);
      }
      catch
      {
        if (!Directory.Exists(path1))
          throw;
        else
          goto label_1;
      }
    }

    public static string GetShortPathName(string longName)
    {
      StringBuilder shortPath = new StringBuilder(256);
      int capacity = shortPath.Capacity;
      Toolbox.GetShortPathName(longName, shortPath, capacity);
      return shortPath.ToString();
    }

    public static DateTime StringToDateTime(string t, string fmt, bool isUTC)
    {
      return !isUTC ? DateTime.ParseExact(t, fmt, (IFormatProvider) null) : DateTime.ParseExact(t, fmt, (IFormatProvider) null, DateTimeStyles.AssumeUniversal);
    }

    public static DateTime StringToDateTime(string t, string fmt, TimeZoneInfo tzi)
    {
      DateTime exact = DateTime.ParseExact(t, fmt, (IFormatProvider) null);
      return !tzi.Equals(TimeZoneInfo.Local) ? TimeZoneInfo.ConvertTime(exact, tzi, TimeZoneInfo.Local) : exact;
    }

    public static DateTime StringToDateTimeUTC(string t, string fmt, TimeZoneInfo tzi)
    {
      if (tzi.Equals(TimeZoneInfo.Utc) && fmt.ToLower() == "epoch")
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(t));
      DateTime exact = DateTime.ParseExact(t, fmt, (IFormatProvider) null);
      return !tzi.Equals(TimeZoneInfo.Utc) ? TimeZoneInfo.ConvertTime(exact, tzi, TimeZoneInfo.Utc) : exact;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetShortPathName(
      string longPath,
      [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
      [MarshalAs(UnmanagedType.U4)] int bufferLength);

    [DllImport("Kernel32")]
    public static extern bool SetConsoleCtrlHandler(Toolbox.HandlerRoutine Handler, bool Add);

    public delegate bool HandlerRoutine(Toolbox.CtrlTypes CtrlType);

    public enum CtrlTypes
    {
      CTRL_C_EVENT = 0,
      CTRL_BREAK_EVENT = 1,
      CTRL_CLOSE_EVENT = 2,
      CTRL_LOGOFF_EVENT = 5,
      CTRL_SHUTDOWN_EVENT = 6,
    }
  }
}
