using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Automata
{
    internal class MQTTBaseClient
    {
        MqttFactory mqttFactory = new MqttFactory();
        public IMqttClient mqttClient;
        MqttClientOptions mqttClientOptions;

        public MQTTBaseClient(string Host, Int32 Port,string Username,string Password, string ClientId,bool AutoConnect=true)
        {            
            mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(Host, Port)
            .WithCredentials(Username, Password)
            .WithClientId(ClientId)
            .WithTls(new MqttClientOptionsBuilderTlsParameters()
            {
                UseTls = true,
                SslProtocol = SslProtocols.Tls12,
                IgnoreCertificateChainErrors=true,
                IgnoreCertificateRevocationErrors=true
            })
            //.WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
            .WithCleanSession(false)
            .WithCleanStart(false)
            .Build();
            mqttClient = mqttFactory.CreateMqttClient();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            if (AutoConnect) Connect();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess==NetworkAccess.Internet)
                mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        }

        public async void Connect()
        {
            try
            { await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None); }
            catch 
            {}
        }
    }
}