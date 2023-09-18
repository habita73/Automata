using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Automata.Controls;

public partial class ControlBase : ContentView
{
    IMqttClient MqttClient;    
    public string Title { get; set; }
    public string Topic { get; set; }
    public MqttQualityOfServiceLevel MqttQualityOfServiceLevel { get; set; }

    public ControlBase(IMqttClient mqttClient, string topic, MqttQualityOfServiceLevel mqttQualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce, string title="new Control")
	{
		InitializeComponent();
        
        MqttClient = mqttClient;
        MqttQualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce;
        Topic = topic;
        Title = title;

        entry_Topic.Text = Topic;
        entry_Title.Text = Title;
        pik_QoS.SelectedIndex = (int)(MqttQualityOfServiceLevel);

        MqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
        
        swh_OnOff.Toggled += SwhOnOff_Toggled;
        mqttClient.ConnectedAsync += MqttClient_ConnectedAsync;
    }

    private Task MqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
    {
        if (MqttClient.IsConnected) MqttClient.SubscribeAsync(Topic, MqttQualityOfServiceLevel);
        return Task.CompletedTask;
    }

    private Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        if(arg.ApplicationMessage.Topic == Topic)
        Debug.WriteLine("-------------------------------------------------- " + arg.ApplicationMessage.ConvertPayloadToString());
        return Task.CompletedTask;
    }

    private void SwhOnOff_Toggled(object sender, ToggledEventArgs e)
    {
        IEnumerable<byte> byteEnumerable = new List<byte>();
        byteEnumerable = Encoding.GetEncoding("iso-8859-1").GetBytes("0");
        if (MqttClient.IsConnected) MqttClient.PublishBinaryAsync(Topic, byteEnumerable,MqttQualityOfServiceLevel.AtLeastOnce);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        SettingsPanel.IsVisible = true;
    }

    private void ButtonOk_Clicked(object sender, EventArgs e)
    {
        string oldTopic = Topic;
        Topic = entry_Topic.Text;
        Title = entry_Title.Text;
        MqttQualityOfServiceLevel = (MqttQualityOfServiceLevel)(pik_QoS.SelectedIndex);

        txt_Title.Text = entry_Title.Text;
        if (MqttClient.IsConnected)
        {
            MqttClient.UnsubscribeAsync(oldTopic);
            MqttClient.SubscribeAsync(Topic, MqttQualityOfServiceLevel);
        }
        SettingsPanel.IsVisible = false;
    }

    private void ButtonCancel_Clicked(object sender, EventArgs e)
    {
        entry_Topic.Text=Topic;
        entry_Title.Text=Title;
        pik_QoS.SelectedIndex = (int)(MqttQualityOfServiceLevel);
        SettingsPanel.IsVisible = false;
    }
}