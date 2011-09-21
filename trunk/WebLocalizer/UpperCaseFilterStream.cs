using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;

namespace WebLocalizer
{
    public class UpperCaseFilterStream : Stream
    // This filter changes all characters passed through it to uppercase.
    {
        private Stream strSink;
        private long lngPosition;
        private MemoryStream ms = new MemoryStream();

        public UpperCaseFilterStream(Stream sink)
        {
            strSink = sink;
        }

        // The following members of Stream must be overriden.
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return lngPosition; }
            set { lngPosition = value; }
        }

        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return strSink.Seek(offset, direction);
        }

        public override void SetLength(long length)
        {
            strSink.SetLength(length);
        }

        public override void Close()
        {
            strSink.Close();
        }

        public override void Flush()
        {
            try
            {
                
                byte[] data = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
                string inputstring = Encoding.UTF8.GetString(data);
                if (inputstring.IndexOf("<html", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    strSink.Write(data, 0, data.Length);
                }
                else
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes("html=" + HttpUtility.UrlEncode(inputstring));
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://web-localizer.appspot.com/translator");
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.UserAgent = ".NET Framework Example Client";
                    request.Method = "POST";
                    request.ContentLength = byteArray.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                    WebResponse response = request.GetResponse();
                    using (Stream s = response.GetResponseStream())
                    {
                        int sz = 1024;
                        int nRead = 0;
                        byte[] buff = new byte[sz];
                        while ((nRead = s.Read(buff, 0, sz)) > 0)
                            strSink.Write(buff, 0, nRead);
                    }
                }
            }
            catch (Exception ex)
            {
                byte[] buff = Encoding.UTF8.GetBytes(ex.Message);
                strSink.Write(buff, 0, buff.Length);
            }
            strSink.Flush();
            ms.SetLength(0);
        } 

        public override int Read(byte[] buffer, int offset, int count)
        {
            return strSink.Read(buffer, offset, count);
        }

        // The Write method actually does the filtering.
        public override void Write(byte[] buffer, int offset, int count)
        {
            ms.Write(buffer, offset, count);
            

        }
    }
}
