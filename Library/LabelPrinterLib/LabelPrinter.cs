using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPrinterLib
{
    public class LabelPrinter
    {
        SerialPort serialPort;
        List<byte> bufferSend = new List<byte>();
        public LabelPrinter(string Serialport)
        {
            serialPort = new SerialPort(Serialport);
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.Open();
        }
        ~LabelPrinter()
        {
            serialPort.Close();
        }
        private void sendSerialData(byte[] data)
        {

            serialPort.Write(data, 0, data.Length);
        }

        private byte[] int16ToByte(UInt16 i)
        {
            byte[] data = new byte[2];
            data[0] = (byte)(i & 0xFF);
            data[1] = (byte)((i >> 8) & 0xFF);
            return data;
        }
        private void flush()
        {
            sendSerialData(bufferSend.ToArray());
            bufferSend.Clear();
        }
        public void createPage(UInt16 x,UInt16 y,UInt16 w,UInt16 h,byte r)
        {
            byte[] header = { 0x1A, 0x5B , 0x01 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(x));
            bufferSend.AddRange(int16ToByte(y));
            bufferSend.AddRange(int16ToByte(w));
            bufferSend.AddRange(int16ToByte(h));
            bufferSend.Add(r);
            flush();
        }
        public void drawRectangleBlock(UInt16 x, UInt16 y, UInt16 w, UInt16 h, byte c)
        {
            byte[] header = { 0x1A, 0x2A, 0x00 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(x));
            bufferSend.AddRange(int16ToByte(y));
            bufferSend.AddRange(int16ToByte(w));
            bufferSend.AddRange(int16ToByte(h));
            bufferSend.Add(c);
            flush();
        }
        public void drawRectangleBox(UInt16 l, UInt16 t, UInt16 r, UInt16 b,UInt16 width, byte c)
        {
            byte[] header = { 0x1A, 0x26, 0x01 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(l));
            bufferSend.AddRange(int16ToByte(t));
            bufferSend.AddRange(int16ToByte(r));
            bufferSend.AddRange(int16ToByte(b));
            bufferSend.AddRange(int16ToByte(width));
            bufferSend.Add(c);
            flush();
        }
        public void drawLine(UInt16 sx, UInt16 sy, UInt16 ex, UInt16 ey, UInt16 width, byte c)
        {
            byte[] header = { 0x1A, 0x5C, 0x01 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(sx));
            bufferSend.AddRange(int16ToByte(sy));
            bufferSend.AddRange(int16ToByte(ex));
            bufferSend.AddRange(int16ToByte(ey));
            bufferSend.AddRange(int16ToByte(width));
            bufferSend.Add(c);
            flush();
        }
        public void drawText(byte[] s,UInt16 x,UInt16 y,UInt16 FontHeight,bool bold,bool underline ,bool colorReverse,bool rotate,byte resizeX,byte resizeY)
        {
            if (resizeX >= 16 || resizeY >= 16)
            {
                return;
            }
            byte[] header = { 0x1A, 0x54, 0x01 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(x));
            bufferSend.AddRange(int16ToByte(y));
            bufferSend.AddRange(int16ToByte(FontHeight));
            byte FontTypeL = 0;
            byte FontTypeH = 0;
            FontTypeH |= (byte)((resizeY & 0b00001111) << 4);
            FontTypeH |= (byte)((resizeX & 0b00001111) << 0);
            FontTypeL |= (byte)(Convert.ToByte(rotate) << 4);
            FontTypeL |= (byte)(Convert.ToByte(colorReverse) << 2);
            FontTypeL |= (byte)(Convert.ToByte(underline) << 1);
            FontTypeL |= (byte)(Convert.ToByte(bold) << 0);
            Console.WriteLine(Convert.ToString(FontTypeH, 2).PadLeft(8, '0'));
            bufferSend.Add(FontTypeL);
            bufferSend.Add(FontTypeH);
            bufferSend.AddRange(s);
            bufferSend.Add(0x00);
            flush();
        }
        public void endPage()
        {
            byte[] header = { 0x1A, 0x5D, 0x00 };
            bufferSend.AddRange(header);
            flush();
        }
        public void startPrint()
        {
            byte[] header = { 0x1A, 0x4F, 0x00 };
            bufferSend.AddRange(header);
            flush();
        }
    }
}
