using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace interpreterv2
{
    public class ExecutableParser
    {
        private byte[] header = Encoding.ASCII.GetBytes("SkendaOS");
        private string file_path;
        private byte[] TheKey = { 0x2B, 0x53, 0x43, 0x21, 0x74, 0x23, 0x9A, 0x18 };
        private byte[] Vector = { 0xC3, 0x7E, 0x34, 0x79, 0x8F, 0x9F, 0x47, 0x54 };
        public ExecutableParser(string path)
        {
            file_path = path;
        }
        private byte[] Encrypt(string message)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            byte[] in_buf, out_buf;
            in_buf = Encoding.ASCII.GetBytes(message);
            try
            {
                CryptoStream crStream = new CryptoStream(ms, des.CreateEncryptor(TheKey, Vector), CryptoStreamMode.Write);
                crStream.Write(in_buf, 0, in_buf.Length);
                crStream.FlushFinalBlock();
                out_buf = ms.ToArray();
                ms.Close();
                crStream.Close();
            }
            catch (CryptographicException ex)
            {
                return null;
            }
            return out_buf;
        }
        public string Decrypt(byte[] message)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            byte[] in_buf, out_buf;
            try
            {
                in_buf = message;
            }
            catch (FormatException ex)
            {
                return "";
            }

            try
            {
                CryptoStream crStream = new CryptoStream(ms, des.CreateDecryptor(TheKey, Vector), CryptoStreamMode.Write);
                crStream.Write(in_buf, 0, in_buf.Length);
                crStream.FlushFinalBlock();
                out_buf = ms.ToArray();
                ms.Close();
                crStream.Close();
            }
            catch (CryptographicException ex)
            {
                return "";
            }
            return Encoding.ASCII.GetString(out_buf);
        }
        public T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public string Read()
        {
            try
            {
                FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] data = new byte[stream.Length - header.Length];
                int actualRead = 0;
                stream.Position = header.Length;
                do
                {
                    actualRead = stream.Read(data, actualRead, int.Parse(stream.Length.ToString()) - header.Length);
                } while (stream.Position < stream.Length);

                return Decrypt(data);
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public bool Write(String content)
        {
            try
            {
                Byte[] data = Encrypt(content);
                FileStream stream = new FileStream(file_path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                stream.Write(header, 0, header.Length);
                stream.Close();
                stream = new FileStream(file_path, FileMode.Append, FileAccess.Write, FileShare.Read);
                stream.Write(data, 0, data.Length);
                stream.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public string DecodeBase64(string base64)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            } catch (Exception)
            {
                return "SKENDAOS_PARSING_ERROR";
            }
        }
    }
}
