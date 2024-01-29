using LabelPrinterLib;using System.Collections.Generic;using System.IO.Ports;using System.Text;LabelPrinter printer = new LabelPrinter("COM6");

/*void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e){    List<byte> data = new List<byte>();    data.Add((byte)serialPort.ReadByte());}*/

/*byte[] sendData = { 0x1F, 0x2D, 0x52, 0x00 };sendSerialData(sendData);byte[] sendData1 = { 0x1A, 0x5B, 0x01, 0x00, 0x00, 0x00, 0x00, 0xC8, 0x00, 0x78, 0x00, 0x00 };printer.sendSerialData(sendData1);*/

printer.createPage(0, 0, 200, 120, 0);System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("AB测试"), 8, 8, 32, false, false, false, false, 1, 1);printer.endPage();printer.startPrint();Console.WriteLine("f");while (true) ;
//serialPort.Close();