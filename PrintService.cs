using System;
using System.Collections.Generic;
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
        List<IDevice> devices;

        public PrintService()
        {
            devices = new List<IDevice>();
        }

        public async void Connect()
        {
            var ble = CrossBluetoothLE.Current;
            var bluetoothStatus = ble.State;

            if (bluetoothStatus == BluetoothState.On)
            {
                var adapter = CrossBluetoothLE.Current.Adapter;

                var devices = adapter.GetSystemConnectedOrPairedDevices();

                return;
            }
        }
    }
}
