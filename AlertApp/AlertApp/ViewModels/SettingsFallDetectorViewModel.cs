using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class SettingsFallDetectorViewModel : BaseViewModel
    {

        #region Services        
        readonly ILocalSettingsService _localSettingsService;
        #endregion


        public double? FALLING_WAIST_SV_TOT { get; set; }
        public double? IMPACT_WAIST_SV_TOT { get; set; }
        public double? IMPACT_WAIST_SV_D { get; set; }
        public double? IMPACT_WAIST_SV_MAXMIN { get; set; }
        public double? IMPACT_WAIST_Z_2 { get; set; }
        public int? INTERVAL_MS { get; set; }
        public double? FILTER_LPF_GAIN { get; set; }


        public double? FILTER_HPF_GAIN { get; set; }
        public double? FILTER_FACTOR_0 { get; set; }
        public double? FILTER_FACTOR_1 { get; set; }
        public double? LYING_AVERAGE_Z_LPF { get; set; }


        public SettingsFallDetectorViewModel(ILocalSettingsService localSettingsService)
        {
            this._localSettingsService = localSettingsService;
            FALLING_WAIST_SV_TOT = Preferences.Get("FALLING_WAIST_SV_TOT", 0.6);
            IMPACT_WAIST_SV_TOT = Preferences.Get("IMPACT_WAIST_SV_TOT", 2.0);
            IMPACT_WAIST_SV_D = Preferences.Get("IMPACT_WAIST_SV_D", 1.7);
            IMPACT_WAIST_SV_MAXMIN = Preferences.Get("IMPACT_WAIST_SV_MAXMIN", 2.0);
            IMPACT_WAIST_Z_2 = Preferences.Get("IMPACT_WAIST_Z_2", 1.5);
            INTERVAL_MS = Preferences.Get("INTERVAL_MS", 20);
            FILTER_LPF_GAIN = Preferences.Get("FILTER_LPF_GAIN", 4.143204922e+03);
            FILTER_HPF_GAIN = Preferences.Get("FILTER_HPF_GAIN", 1.022463023e+00);
            FILTER_FACTOR_0 = Preferences.Get("FILTER_FACTOR_0", -0.9565436765);
            FILTER_FACTOR_1 = Preferences.Get("FILTER_FACTOR_1", +1.9555782403);
            LYING_AVERAGE_Z_LPF = Preferences.Get("LYING_AVERAGE_Z_LPF", 0.5);
        }

        public void SaveSettings()
        {
            Preferences.Set("FALLING_WAIST_SV_TOT", FALLING_WAIST_SV_TOT.Value);
            Preferences.Set("IMPACT_WAIST_SV_TOT", IMPACT_WAIST_SV_TOT.Value);
            Preferences.Set("IMPACT_WAIST_SV_D", IMPACT_WAIST_SV_D.Value);
            Preferences.Set("IMPACT_WAIST_SV_MAXMIN", IMPACT_WAIST_SV_MAXMIN.Value);
            Preferences.Set("IMPACT_WAIST_Z_2", IMPACT_WAIST_Z_2.Value);
            Preferences.Set("INTERVAL_MS", INTERVAL_MS.Value);
            Preferences.Set("FILTER_LPF_GAIN", FILTER_LPF_GAIN.Value);
            Preferences.Set("FILTER_HPF_GAIN", FILTER_HPF_GAIN.Value);
            Preferences.Set("FILTER_FACTOR_0", FILTER_FACTOR_0.Value);
            Preferences.Set("FILTER_FACTOR_1", FILTER_FACTOR_1.Value);
            Preferences.Set("LYING_AVERAGE_Z_LPF", LYING_AVERAGE_Z_LPF.Value);

            if (_localSettingsService.GetFallDetecion())
            {
                RestartFallDetection();
            }
            else
            {
                DisableFallDetection();
            }

        }

        public void RestartFallDetection()
        {
            _localSettingsService.SetFallDetection(true);
            MessagingCenter.Send((BaseViewModel)this, StartStopFallDetectionEvent.Event, new StartStopFallDetectionEvent { Restart = true }) ;

        }

        public void EnableFallDetection()
        {
            _localSettingsService.SetFallDetection(true);
            MessagingCenter.Send((BaseViewModel)this, StartStopFallDetectionEvent.Event, new StartStopFallDetectionEvent { Start = true });

        }
        public void DisableFallDetection()
        {
            MessagingCenter.Send((BaseViewModel)this, StartStopFallDetectionEvent.Event, new StartStopFallDetectionEvent { Stop = true });
            _localSettingsService.SetFallDetection(false);
        }

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }


        internal bool IsValid()
        {
            return FALLING_WAIST_SV_TOT.HasValue &&
            IMPACT_WAIST_SV_TOT.HasValue &&
            IMPACT_WAIST_SV_D.HasValue &&
            IMPACT_WAIST_SV_MAXMIN.HasValue &&
            IMPACT_WAIST_Z_2.HasValue &&
            INTERVAL_MS.HasValue &&
            FILTER_LPF_GAIN.HasValue &&
            FILTER_HPF_GAIN.HasValue &&
            FILTER_FACTOR_0.HasValue &&
            FILTER_FACTOR_1.HasValue &&
            LYING_AVERAGE_Z_LPF.HasValue;
        }


    }
}
