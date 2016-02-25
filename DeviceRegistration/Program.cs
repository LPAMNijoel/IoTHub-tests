using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace DeviceRegistration
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=RPIdemo.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=O2ZQGk40/o8vUxUCCl82UV7c7kq1akXNJLPSg6FFQ0c=";

        private async static Task AddDeviceAsync()
        {
            //deviceId is the string I wanto to use to register to my device
            string deviceId = "MQTTDevice";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }

        static void Main(string[] args)
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                AddDeviceAsync().Wait();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            
            }
           
        }
    }
}
