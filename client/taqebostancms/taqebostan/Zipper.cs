using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

/// <summary>
/// Summary description for Zipper
/// </summary>
public class Zipper
{
	public Zipper()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static byte[] Compress(string data)
    {
        return Compress(System.Text.Encoding.Unicode.GetBytes(data));
    }

    public static string DecompressToStrng(byte[] data)
    {
        return System.Text.Encoding.Unicode.GetString(Decompress(data));
    }

    public static byte[] Compress(byte[] data)
    {
        try
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

            MemoryStream ms = new MemoryStream();
            Stream s = new DeflateStream(ms, CompressionMode.Compress);

            s.Write(data, 0, data.Length);
            s.Close();

            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;

            return ms.ToArray();
        }
        catch
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
            return null;
        }
    }

    public static byte[] Decompress(byte[] data)
    {
        try
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

            string result = string.Empty;
            byte[] buffer = { };

            MemoryStream ms = new MemoryStream(data);
            Stream s = new DeflateStream(ms, CompressionMode.Decompress);

            int len = 4096;

            while (true)
            {
                int oldLen = buffer.Length;
                Array.Resize(ref buffer, oldLen + len);
                int size = s.Read(buffer, oldLen, len);
                if (size != len)
                {
                    Array.Resize(ref buffer, buffer.Length - (len - size));
                    break;
                }
                if (size <= 0)
                    break;
            }
            s.Close();

            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;

            return buffer;
        }
        catch
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
            return null;
        }
    }
}
