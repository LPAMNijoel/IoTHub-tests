using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using System.Net.Security;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Security.Cryptography;


namespace M2MQTTClient
{
    class Program
    {

        private static readonly long UtcReference = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks;
        private static int MESSAGE_COUNT = 3;
        private static byte QoS_AT_MOST_ONCE = 0x01;


        static void Main(string[] args)
        {
            MqttClient client = new MqttClient("RPIdemo.azure-devices.net", 8883, true, MqttSslProtocols.TLSv1_0, null, null);

            //clientID - it's the device ID I used to register my device
            String clientID = "MQTTDevice";
            //username - it's my IoT Hub URI / ClientID
            String username = "<your IoT Hub URI/ClientID>";
            //Password - SAS Token taken from Device Explorer, removing the initial part (before SharedAccessSignature sr=...)
            String password = "SharedAccessSignature sr=******&sig=*********";

            Console.WriteLine("M2MQTTClient Device\n");
           

            try
            {
                //M2MQTT Connection
                var returnValue = client.Connect(clientID, username, password);
                Console.WriteLine($"connection return value: {returnValue}\n");

                //send messages to IoT Hub
                String msg = "Hello from M2MQTT device - ";   
                             
                for (int i=0; i<MESSAGE_COUNT; i++)
                {
                    Random rand = new Random();
                    String current = msg + DateTime.Now.ToLocalTime() + " - " + rand.Next().ToString();
                    byte[] message = Encoding.ASCII.GetBytes(current);

                    client.Publish($"devices/{clientID}/messages/events/", message, QoS_AT_MOST_ONCE, false);

                    Console.WriteLine($"message {i} sent to IoT Hub : {current}");

                }    
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }


        }

    }
}
