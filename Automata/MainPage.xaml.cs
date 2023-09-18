using Automata.Controls;
using Microsoft.Maui.Controls.Internals;
using MQTTnet.Client;
using MQTTnet.Server;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Automata
{
    public partial class MainPage : ContentPage
    {
        public bool IsConnected { get; set; } = false;
        public IMqttClient mqttClient = new MQTTBaseClient(Preferences.Get("Host", "ebc1b0bf4344480c9ea1a08673fb1628.s1.eu.hivemq.cloud"), int.Parse(Preferences.Get("Port", "8883")),
            Preferences.Get("Username","habita73"), Preferences.Get("Password", "@Hh25051973"),  Preferences.Get("ClientId", "Client-001"),true).mqttClient;        
        public MainPage()
        {            
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            //BindingContext = this;
            //mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
            ControlsPresenter.Add(new ControlBase(mqttClient,"habita73/1/1/"));
            ControlsPresenter.Add(new ControlBase(mqttClient,"habita73/1/2/"));
            
            ControlsPresenter.Add(new ControlBase(mqttClient, "habita73/1/state/", MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce));
            ControlsPresenter.Add(new ControlBase(mqttClient, "habita73/state/", MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce));
            
            mqttClient.ConnectedAsync += MqttClient_ConnectedAsync;
            mqttClient.DisconnectedAsync += MqttClient_DisconnectedAsync;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                //imgInternetIndicatorDisConnected.IsVisible = false;
                //imgInternetIndicatorConnected.IsVisible = true;
            }

            else
            {
                //imgInternetIndicatorDisConnected.IsVisible = false;
                //imgInternetIndicatorConnected.IsVisible = true;
            }
        }

        private async Task MqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            Application.Current.Dispatcher.Dispatch((Action)(() => { 
                ControlsPresenter.IsEnabled = false; 
                imgConnectionIndicatorConnected.IsVisible = false; 
                imgConnectionIndicatorDisConnected.IsVisible = true;
            }));
            if (arg.Reason != MqttClientDisconnectReason.AdministrativeAction)
            { Thread.Sleep(3000); await mqttClient.ReconnectAsync(CancellationToken.None); }
            return;
        }

        private async Task MqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            Application.Current.Dispatcher.Dispatch((Action)(() => { 
                ControlsPresenter.IsEnabled = true; 
                imgConnectionIndicatorConnected.IsVisible = true; 
                imgConnectionIndicatorDisConnected.IsVisible = false;
            }));
            return; 
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Dispatch((Action)(() => { }));
                //Newtonsoft.Json.JsonConvert.SerializeObject(ControlsPresenter.Children);
        }
    }
}
