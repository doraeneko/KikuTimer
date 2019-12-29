//
// (C) 2019, Andreas Gaiser.
// 

using System;
using Xamarin.Forms;
using XamarinDevice = Xamarin.Forms.Device;

namespace KikuTimer
{
    public partial class MainPage : ContentPage
    {
        AudioTimer _audioTimer;
        bool _isRunning = false;

        public MainPage()
        {
            InitializeComponent();
            _audioTimer = new AudioTimer();
            _isRunning = false;
            _audioTimer.OnTimeChanged += TimerChanged;

        }

        private void RefreshClockDisplay()
        {
            XamarinDevice.BeginInvokeOnMainThread(()
                => LabelTimeDisplay.Text = _audioTimer?.TheTime);
        }
        
        private void SetStartStopButtonCaption(string caption)
        {
            XamarinDevice.BeginInvokeOnMainThread(()
    => ButtonStartPause.Text = caption);
        }

        private void TimerChanged(object sender,
           System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimeTick")
            {
                RefreshClockDisplay();
            }
        }

        private void ButtonStartPause_Clicked(object sender, EventArgs e)
        {
            var theButton = sender as Button;
            if (_isRunning)
            {
                _audioTimer.Pause();
                SetStartStopButtonCaption("Continue");
            }
            else
            {
                _audioTimer.StartOrContinue();
                SetStartStopButtonCaption("Pause");
            }
            _isRunning = !_isRunning;
        }

        private void ButtonReset_Clicked(object sender, EventArgs e)
        {
            _audioTimer.Reset();
            RefreshClockDisplay();
            SetStartStopButtonCaption("Start");
        }
    }
}
