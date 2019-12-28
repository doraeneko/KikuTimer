//
// (C) 2019, Andreas Gaiser.
// 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using Xamarin.Essentials;

namespace KikuTimer
{
    class AudioTimer
    {
        System.Timers.Timer _timer;
        int _miliseconds;
        const int _oneTickInMs = 100;
        const int _milisecondsPerMinute = 60000;

        protected void TimeTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            _miliseconds += 100;
            NotifyPropertyChanged();
            if (_miliseconds % _milisecondsPerMinute == 0)
            {
                var timeSpan = new TimeSpan(0, 0, 0, 0, _miliseconds);
                if (timeSpan.Hours > 0)
                {
                    TextToSpeech.SpeakAsync(
                        $"{timeSpan.Hours.ToString("D", CultureInfo.InvariantCulture)} hours, " +
                        $"{timeSpan.Minutes.ToString("D", CultureInfo.InvariantCulture)} minutes.");
                }
                else
                {
                    TextToSpeech.SpeakAsync(
                        $"{timeSpan.Minutes.ToString("D", CultureInfo.InvariantCulture)} minutes.");
                }
            }
        }

        public event PropertyChangedEventHandler OnTimeChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            OnTimeChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal String TheTime
        {
            get
            {
                var timeSpan = new TimeSpan(0,0,0,0,_miliseconds);
                return $"{timeSpan.Hours.ToString("00", CultureInfo.InvariantCulture)}:" +
                       $"{timeSpan.Minutes.ToString("00", CultureInfo.InvariantCulture)}:" +
                       $"{timeSpan.Seconds.ToString("00", CultureInfo.InvariantCulture)}"; 
            }
        }

        public AudioTimer()
        {
            _miliseconds = 0;
            _timer = new System.Timers.Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += TimeTick;
            _timer.Interval = _oneTickInMs;
        }

        public void StartOrContinue()
        {
            _timer.Enabled = true;
        }

        public void Pause()
        {
            _timer.Enabled = false;
        }

        public void Reset()
        {
            _miliseconds = 0;
            NotifyPropertyChanged();
        }
    }
}
