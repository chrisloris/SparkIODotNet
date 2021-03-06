﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace SparkIO.WebServices.Internal
{
    internal class EventStreamReader
    {
        private Stream stream;
        private ConcurrentQueue<String> queue;
        private AutoResetEvent emitterWaitHandle;
        private int sleepTimeMs;
        
        private volatile bool _shouldStop = false;

        private EventStreamReader() { }

        public EventStreamReader(Stream _stream, ConcurrentQueue<String> _queue, AutoResetEvent _emitterWaitHandle, int _sleepTimeMs = 1000)
        {
            stream = _stream;
            queue = _queue;
            emitterWaitHandle = _emitterWaitHandle;
            sleepTimeMs = _sleepTimeMs;
        }

        public void DoWork()
        {
            StringBuilder orphanLine = new StringBuilder(2000);
            var encoder = new UTF8Encoding();
            var buffer = new byte[50];

            int len;
            bool endingN;
            int i;

            while (!_shouldStop & stream.CanRead)
            {
                len = stream.Read(buffer, 0, 50);
                if (len > 0)
                {
                    var text = encoder.GetString(buffer, 0, len);

                    endingN = text.Substring(text.Length - 1, 1) == "\n";

                    string[] lines = text.Trim().Split('\n');

                    for (i = 1; i <= lines.Length; i++)
                    {
                        if (i == 1) // special handling for first line
                        {
                            if (lines.Length == 1 && !endingN) // single line read with no /n - orphan
                            {  // store for later sending
                                orphanLine.Append(lines[i - 1]);
                            }
                            else
                            {  // 1st of many lines so we are sending it
                                if (orphanLine.Length > 0)
                                {  // if we have an oprhan from previous reads - prepend it to current line and send
                                    queue.Enqueue(orphanLine.Append(lines[i - 1]).ToString());
                                    orphanLine.Clear();
                                    emitterWaitHandle.Set();
                                }
                                else  // no previous orphans so just send the current line
                                {
                                    queue.Enqueue(lines[i - 1]);
                                    emitterWaitHandle.Set();
                                }
                            }
                        }
                        else if (i == lines.Length)  // special handling for last line when at last one line prior
                        {
                            if (!endingN)
                            {  // orphan last line - save it for the next lines
                                orphanLine.Append(lines[i - 1]);
                            }
                            else  // complete last line - send and signal
                            {
                                queue.Enqueue(lines[i - 1]);
                                emitterWaitHandle.Set();

                            }
                        }
                        else
                        {  // not the first or last line - just send and signal
                            queue.Enqueue(lines[i - 1]);
                            emitterWaitHandle.Set();
                        }
                    }
                }
                else // nothing in the stream
                {
                    // let's sleep a bit to give the CPU a break
                    Thread.Sleep(sleepTimeMs);
                }
            }

        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

    }
}
