using System.IO.Ports;

namespace LabelPrinterLib
{
    public interface ISerialPortService
    {
        void Open();
        void Close();
        void Write(byte[] buffer, int offset, int count);
        byte[] Read(int bufferSize); // 同步读取
        Task<byte[]> ReadAsync(int bufferSize); // 异步读取
        bool IsOpen { get; }
        //event EventHandler<byte[]> DataReceived;
    }
    public enum PrinterFontSize
    {
        Height16 = 16,
        Height20 = 20,
        Height22 = 22,
        Height24 = 24,
        Height32 = 32,
        Height48 = 48,
        Height64 = 64,
        Height80 = 80,
        Height96 = 96,
    }
    public enum PrinterBarcodeType
    {
        UPCA,
        UPCE,
        EAN13,
        EAN8,
        CODE39,
        I25,
        CODABAR,
        CODE93,
        CODE128,
        CODE11,
        MSI,
        T128M,
        EAN128,
        T25C,
        T39C,
        T39,
        EAN132,
        EAN135,
        EAN82,
        EAN85,
        POST,
        UPCA2,
        UPCA5,
        UPCE2,
        UPCE5,
        CPOST,
        PLESSEY,
        ITF14,
        EAN14

    }
    public class LabelPrinter
    {
        ISerialPortService serialPort;
        List<byte> bufferSend = new List<byte>();
        public LabelPrinter(ISerialPortService serialPortService)
        {
            serialPort = serialPortService;
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
        public void drawText(byte[] s,UInt16 x,UInt16 y, PrinterFontSize FontHeight,bool bold,bool underline ,bool colorReverse,bool rotate,byte resizeX,byte resizeY)
        {
            if (resizeX >= 16 || resizeY >= 16)
            {
                return;
            }
            byte[] header = { 0x1A, 0x54, 0x01 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(x));
            bufferSend.AddRange(int16ToByte(y));
            bufferSend.AddRange(int16ToByte((UInt16)FontHeight));
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
        public void drawBarcode(byte[] s, UInt16 x, UInt16 y, PrinterBarcodeType BarcodeType, byte Height, byte UnitWidth, byte rotate)
        {
            if ((byte)BarcodeType > 29)
            {
                return;
            }
            if (UnitWidth < 1 || UnitWidth > 4)
            {
                return;
            }
            if (rotate > 3)
            {
                return;
            }
            byte[] header = { 0x1A, 0x30, 0x00 };
            bufferSend.AddRange(header);

            bufferSend.AddRange(int16ToByte(x));
            bufferSend.AddRange(int16ToByte(y));
            bufferSend.Add((byte)BarcodeType);
            bufferSend.Add(Height);
            bufferSend.Add(UnitWidth);
            bufferSend.Add(rotate);
            bufferSend.AddRange(s);
            bufferSend.Add(0x00);
            flush();
        }
        public void drawQRCode(byte[] s, UInt16 x, UInt16 y, byte version, byte ECC, byte UnitWidth, byte rotate)
        {
            if (UnitWidth < 1 || UnitWidth > 4)
            {
                return;
            }
            if (rotate > 3)
            {
                return;
            }
            byte[] header = { 0x1A, 0x31, 0x00 };
            bufferSend.AddRange(header);

            bufferSend.Add(version);
            bufferSend.Add(ECC);
            bufferSend.AddRange(int16ToByte(x));
            bufferSend.AddRange(int16ToByte(y));
            
            bufferSend.Add(UnitWidth);
            bufferSend.Add(rotate);
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
