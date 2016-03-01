using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading; 

namespace SimulatedDevice1
{
    class Program
    {
        private static int MESSAGE_COUNT = 3;
        static DeviceClient deviceClient;
        private static string iotHubUri = "<Your IoT Hub URI>";
        private static string deviceKey = "<your device key>";
        private static string deviceID = "AMQPDevice";

        private static async void SendDeviceToCloudMessagesAsync()
        {
            String msg = "Hello from AMQP Device - "; 

            for(int i=0; i<MESSAGE_COUNT; i++)
            {
                Random rand = new Random();
                string current = msg + " " + DateTime.Now.ToLocalTime() + " - " + rand.Next().ToString();
                var message = new Message(Encoding.ASCII.GetBytes(current));

                await deviceClient.SendEventAsync(message);

                Console.WriteLine($"message {i} sent to IoT Hub : {current}");

                Thread.Sleep(1000);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("AMQP Device");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceID, deviceKey));

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
