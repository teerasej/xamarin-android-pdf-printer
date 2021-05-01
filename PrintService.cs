using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace PDFPrinter
{
    public class PrintService
    {
        IConnection connection;
        string btAddress = "48:A4:93:89:63:97";

        public PrintService()
        {
            
        }

        public async Task<bool> Print(Uri localFilePath)
        {

            connection = ConnectionBuilder.Current.Build("BT:" + btAddress);

            string zpl = "^XA^POI^MNN^LL90^PW400^FO20,20^A0N,50,50^FDTEST^FS^XZ";
            try
            {
                connection.Open();
                //if (!CheckPrinterLanguage(connection))
                //    return false;
                if (!PreCheckPrinterStatus(connection))
                    return false;
                connection.Write(Encoding.UTF8.GetBytes(zpl));
                PostPrintCheckStatus(connection);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
            }
            finally
            {
                if (connection.IsConnected)
                    connection.Close();
            }

            return true;
        }

        public bool CheckPrinterLanguage(IConnection connection)
        {
            //  Set the printer command languege
            connection.Write(Encoding.UTF8.GetBytes(@"! U1 setvar ""device.languages"" ""zpl\""\r\n"));
        
            byte[] response = connection.SendAndWaitForResponse(Encoding.UTF8.GetBytes("! U1 getvar \"device.languages\"\r\n"), 500, 100);
        
            string language = Encoding.UTF8.GetString(response, 0, response.Length);
            if (!language.Contains("zpl"))
            {
                return true;
            }
            return true;
        }

        public bool PreCheckPrinterStatus(IConnection connection)
        {
            // Check the printer status prior to printing
            IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
            IPrinterStatus status = printer.CurrentStatus;
            if (!status.IsReadyToPrint)
            {
                System.Diagnostics.Debug.WriteLine("Unable to print. Printer is " + status.Status);
                return false;
            }
            return true;
        }



        public async void Connect()
        {
            var ble = CrossBluetoothLE.Current;
            var bluetoothStatus = ble.State;

            if (bluetoothStatus == BluetoothState.On)
            {
                var adapter = CrossBluetoothLE.Current.Adapter;

                adapter.DeviceDiscovered += (sender, e) =>
                {
                    var foundDevice = e.Device;
                    //this.devices.Add(foundDevice);
                };

                await adapter.StartScanningForDevicesAsync();


                //var devices = adapter.GetSystemConnectedOrPairedDevices();

                return;
            }
        }
    }
}
