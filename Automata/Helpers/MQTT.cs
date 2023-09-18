using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.Helpers
{
    internal class MQTT
    {
        MqttFactory mqttFactory = new MqttFactory();
        IMqttClient mqttClient;

        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("ebc1b0bf4344480c9ea1a08673fb1628.s1.eu.hivemq.cloud", 8883)
            .WithCredentials("habita73", "@Hh25051973")
            .WithClientId("client-13")
            .WithTls()
            .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithWillRetain(true)
            .WithWillTopic("onlinestate/")
            .WithWillPayload("I'm Disconnected")
            //.WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
            .WithCleanSession(false)
            .WithCleanStart(false)
            .Build();
        void Initialize()
        {
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClient.DisconnectedAsync += async e =>
            {
                if (e.Reason == MqttClientDisconnectReason.AdministrativeAction) return;
                if (e.ClientWasConnected) await mqttClient.ConnectAsync(mqttClient.Options); //else Connect();               
            };

            mqttClient.ApplicationMessageReceivedAsync += delegate (MqttApplicationMessageReceivedEventArgs args)
            {
                //Thread t = new Thread(() => { Clipboard.SetText(Newtonsoft.Json.JsonConvert.SerializeObject(mqttClient.Options)); });
                //t.SetApartmentState(ApartmentState.STA);
                //t.Start();
                //t.Join();
                //MessageBox.Show(args.ApplicationMessage.Topic + "\n" + Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment.Array, args.ApplicationMessage.PayloadSegment.Offset, args.ApplicationMessage.PayloadSegment.Count));
                return Task.CompletedTask;
            };

            Connect();
        }
        private async void Connect()
        {
            var connectResult = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(f =>
                {
                    f.WithTopic("test2/");
                    f.WithExactlyOnceQoS();
                    //f.WithRetainHandling(MqttRetainHandling.SendAtSubscribe);
                    //f.WithRetainAsPublished(false);
                    //f.WithNoLocal(false);
                }).Build();
                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
                await mqttClient.PingAsync();
                await mqttClient.TryPingAsync();
            }
            async void Subscribe()
            {
                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(f =>
                {
                    f.WithTopic("test2/");
                    f.WithExactlyOnceQoS();
                    //f.WithRetainHandling(MqttRetainHandling.SendAtSubscribe);
                    //f.WithRetainAsPublished(false);
                    //f.WithNoLocal(false);
                }).Build();
                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            }

            async void Publish()
            {
                var applicationMessage = new MqttApplicationMessageBuilder().WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                    .WithTopic("test/").WithPayload("My Message to send").Build();
                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }
        }
    }
}
