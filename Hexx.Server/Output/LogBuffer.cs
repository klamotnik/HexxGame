using System;
using System.Collections.Generic;
using System.Linq;
using Hexx.Types;

namespace Hexx.Server.Output
{
    public class LogBuffer : Singleton<LogBuffer>
    {
        private class Record
        {
            public DateTime Date { get; private set; }
            public string Text { get; private set; }

            public Record(string text)
            {
                Date = DateTime.Now;
                Text = text;
            }

            public override string ToString()
            {
                return Date.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + Text;
            }
        }
        
        private List<Record> buffer = new List<Record>();
        
        public void Push(string text)
        {
            lock (buffer)
                buffer.Add(new Record(text));
        }

        public void WriteBuffer()
        {
            if (buffer.Count == 0)
                return;
            List<Record> linesToWrite = null; 
            lock (buffer)
            {
                linesToWrite = buffer.ToList();
                buffer.Clear();
            }
            foreach (Record record in linesToWrite)
                Console.WriteLine(record);
        }
    }
}
