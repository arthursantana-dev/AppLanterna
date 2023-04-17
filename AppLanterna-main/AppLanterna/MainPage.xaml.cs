using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Battery.Abstractions;
using Plugin.Battery;

namespace AppLanterna
{
    public partial class MainPage : ContentPage
    {
        bool lanternaLigada = false;

        public MainPage()
        {
            InitializeComponent();

            buttonOnOff.Source = ("off");

            CarregaInfoBateria();
        }

        private void buttonOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!lanternaLigada)
                {
                    Flashlight.TurnOnAsync();
                    buttonOnOff.Source = ("on");
                    lanternaLigada=true;
                } else
                {
                    Flashlight.TurnOffAsync();
                    buttonOnOff.Source = ("off");
                    lanternaLigada = false;
                }
            } catch (Exception ex)
            {
                DisplayAlert("Ops...", "Não foi possível acessar a lanterna", "Ok");
                
            } finally
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(500));
            }
        }

        private void MudancaStatusBateria(object sender, BatteryChangedEventArgs e)
        {
            try
            {
                labelCargaRestante.Text = e.RemainingChargePercent.ToString() + "%";

                switch(e.Status)
                {
                    case BatteryStatus.Charging:
                        labelStatusBateria.Text = "Carregando";
                        break;

                    case BatteryStatus.Discharging:
                        labelStatusBateria.Text = "Descarregando";
                        break;

                    case BatteryStatus.Full:
                        labelStatusBateria.Text = "Carga Completa";
                        break;

                    case BatteryStatus.NotCharging:
                        labelStatusBateria.Text = "Sem Carregar";
                        break;

                    case BatteryStatus.Unknown: 
                        labelStatusBateria.Text= "Desconhecido";
                        break;

                }

                switch(e.PowerSource)
                {
                    case PowerSource.Ac:
                        labelFonteEnergia.Text = "Carregando";
                        break;

                    case PowerSource.Battery:
                        labelFonteEnergia.Text = "Bateria";
                        break;

                    case PowerSource.Other:
                        labelFonteEnergia.Text = "Outro";
                        break;

                    case PowerSource.Usb:
                        labelFonteEnergia.Text = "USB";
                        break;

                    case PowerSource.Wireless:
                        labelFonteEnergia.Text = "Sem Fio";
                        break;
                }
            } catch (Exception ex)
            {
                DisplayAlert("Ops...", "Não foi possível obter dados da bateria", "Ok");
            }
        }

        private void CarregaInfoBateria()
        {
            try
            {
                if(CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= MudancaStatusBateria;
                    CrossBattery.Current.BatteryChanged+= MudancaStatusBateria;
                } else
                {
                    throw new Exception("Não há suporte ao plugin de bateria");
                }
            }catch(Exception ex)
            {
                DisplayAlert("Ops...", "Não foi possível obter dados da bateria", "Ok");
            }
        }
    }
}
