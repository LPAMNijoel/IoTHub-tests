// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Client.Samples
{
    class Program
    {

        // String containing Hostname, Device Id & Device Key in one of the following formats:
        //  "HostName=<iothub_host_name>;DeviceId=<device_id>;SharedAccessKey=<device_key>"
        //  "HostName=<iothub_host_name>;CredentialType=SharedAccessSignature;DeviceId=<device_id>;SharedAccessSignature=SharedAccessSignature sr=<iot_host>/devices/<device_id>&sig=<token>&se=<expiry_time>";
        
        private const string DeviceConnectionString = "HostName=RPIdemo.azure-devices.net;DeviceId=MQTTDevice;SharedAccessKey=6QU3ehmRNMtI6XmxOzH9eRCq2Frixo0yB2XeIQh+8Ko=";
        private static int MESSAGE_COUNT = 3;

        static void Main(string[] args)
        {
            Console.WriteLine("MQTT Device");
            try
            {
                //Connection to IoT Hub using my device connection string
                DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);

                deviceClient.OpenAsync().Wait();
                SendEvent(deviceClient).Wait();
                ReceiveCommands(deviceClient).Wait();

                Console.WriteLine("Exited!");
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error in sample: {0}", exception);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        static async Task SendEvent(DeviceClient deviceClient)
        {    
            String msg = "Hello from MQTT Device - ";

            for (int count = 0; count < MESSAGE_COUNT; count++)
            {
                Random rand = new Random();
                string current = msg + " " + DateTime.Now.ToLocalTime() + " - " + rand.Next().ToString();
                var message = new Message(Encoding.UTF8.GetBytes(current));

                Console.WriteLine($"message {count} sent to IoT Hub : {current}");

                await deviceClient.SendEventAsync(message);
            }
        }

        //not used for now
        static async Task ReceiveCommands(DeviceClient deviceClient)
        {
            Console.WriteLine("\nDevice waiting for commands from IoTHub...\n");
            Message receivedMessage;
            string messageData;

            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync(TimeSpan.FromSeconds(1));

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }
    }
}
