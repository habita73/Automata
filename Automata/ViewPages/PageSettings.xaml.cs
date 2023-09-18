using System.Xml.Linq;

namespace Automata.ViewPages;

public partial class PageSettings : ContentPage
{
    public PageSettings()
	{
		InitializeComponent();
        EntryHost.Text = Preferences.Get("Host", "ebc1b0bf4344480c9ea1a08673fb1628.s1.eu.hivemq.cloud");
        EntryPort.Text = Preferences.Get("Port", "8883");
        EntryUsername.Text = Preferences.Get("Username", "habita73");
        EntryPassword.Text = Preferences.Get("Password", "@Hh25051973");
        EntryClientId.Text = Preferences.Get("ClientId", "Client-001");
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("Host",EntryHost.Text);
        Preferences.Set("Port", EntryPort.Text);
        Preferences.Set("Username", EntryUsername.Text);
        Preferences.Set("Password", EntryPassword.Text);
        Preferences.Set("ClientId", EntryClientId.Text);
    }
}