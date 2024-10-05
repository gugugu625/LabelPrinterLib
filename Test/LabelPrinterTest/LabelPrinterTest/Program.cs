using LabelPrinterLib;using System.Text;LabelPrinter printer = new LabelPrinter("COM3");

/*void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e){    List<byte> data = new List<byte>();    data.Add((byte)serialPort.ReadByte());}*/

/*byte[] sendData = { 0x1F, 0x2D, 0x52, 0x00 };sendSerialData(sendData);byte[] sendData1 = { 0x1A, 0x5B, 0x01, 0x00, 0x00, 0x00, 0x00, 0xC8, 0x00, 0x78, 0x00, 0x00 };printer.sendSerialData(sendData1);*/

printer.createPage(0, 0, 400, 120, 0);
System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//printer.drawQRCode(System.Text.Encoding.GetEncoding("gb2312").GetBytes("12"), 0, 8,1,3,4,0);
//printer.drawLine(0, 0, 400, 120, 2, 1);
printer.drawLine(0, 40, 160, 40, 2, 1);printer.drawLine(0, 80, 160, 80, 2, 1);printer.drawLine(80, 40, 80, 80, 2, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("测试电阻电容"), 10, 8, PrinterFontSize.Height24, false, false, false, false, 1, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("AAA-000"), 2, 48, PrinterFontSize.Height22, false, false, false, false, 1, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("220uF"), 85, 48, PrinterFontSize.Height22, false, false, false, false, 1, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("25v/65536"), 10, 88, PrinterFontSize.Height24, false, false, false, false, 1, 1);

printer.drawLine(0 + 220, 40, 160 + 220, 40, 2, 1);printer.drawLine(0 + 220, 80, 160 + 220, 80, 2, 1);printer.drawLine(80 + 220, 40, 80 + 220, 80, 2, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("测试电阻电容"), 10 + 220, 8, PrinterFontSize.Height24, false, false, false, false, 1, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("AAA-000"), 2 + 220, 48, PrinterFontSize.Height22, false, false, false, false, 1, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("220uF"), 85 + 220, 48, PrinterFontSize.Height22, false, false, false, false, 1, 1);printer.drawText(System.Text.Encoding.GetEncoding("gb2312").GetBytes("25v/65536"), 10 + 220, 88, PrinterFontSize.Height24, false, false, false, false, 1, 1);printer.endPage();printer.startPrint();Console.WriteLine("f");while (true) ;
//serialPort.Close();