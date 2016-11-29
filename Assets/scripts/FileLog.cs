using UnityEngine;
using System.IO;
using System;
public class FileLog : MonoBehaviour, ILog {

    string logFile = @"C:\Users\Zivile\Documents\Unity projects\Tetrisv2\log.txt";
    public void Log(string message)
    {
        using (StreamWriter sw = File.AppendText(logFile))
        {
            sw.Write("\r\nLog Entry : ");
            sw.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            sw.WriteLine("  :");
            sw.WriteLine("  :{0}", message);
            sw.WriteLine("-------------------------------");
        }
    }
}
