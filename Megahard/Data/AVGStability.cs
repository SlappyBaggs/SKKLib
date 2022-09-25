using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Megahard.Data
{
    public class AVGStability
    {
        public delegate void AVGTickHandler(object o, EventArgs e);
        public event AVGTickHandler AVGStabilityTick;
        public event AVGTickHandler AVGTick;

        public AVGStability()
        {
            Tolerance = 0.01;
            AvgTime = 5;

            running_ = false;
            worker = new BackgroundWorker();
            worker.DoWork += AvgWork;
            worker.RunWorkerCompleted += AvgDone;
            
            Reset();
            values = new List<double>();
        }

        private bool running_;
        public void Start()
        {
            Reset();
            OnTick();

            timerSpan_ = DateTime.Now;
            running_ = true;
            worker.RunWorkerAsync();
        }

        public void Stop()
        {
            running_ = false;
        }

        private void Reset()
        {
            _stable = false;
            _currentAverage = 0.0;
            avgCount = AvgTime * 1000;
        }

        private void OnTick()
        {
            if (running_ && AVGTick != null)
                AVGTick(this, EventArgs.Empty);
        }

        private void OnAvg()
        {
            if (running_ && AVGStabilityTick != null)
                AVGStabilityTick(this, EventArgs.Empty);
        }

        private int timerInterval_;
        public int TimerInterval
        {
            get { return timerInterval_; }
            set { timerInterval_ = value; }
        }

        private DateTime timerSpan_;
        private void AvgWork(object o, DoWorkEventArgs args)
        {
            while (running_)
            {
                // If we have a poll function, call it...
                Thread.Sleep(timerInterval_);
                if (PollFunc != null)
                    AddValue(PollFunc());
                avgCount -= (int)((DateTime.Now - timerSpan_).TotalMilliseconds);
                timerSpan_ = DateTime.Now;
                if (avgCount <= 0)
                {
                    avgCount = AvgTime * 1000;
                    if (values.Count == 0)
                    {
                        _currentAverage = 0.0;
                        OnTick();
                        OnAvg();
                        return;
                    }
                    double tot = 0.0;
                    double min = 0xffffffff;
                    double max = -0xffffffff;

                    foreach (double d in values)
                    {
                        tot += d;
                        if (d < min) min = d;
                        if (d > max) max = d;
                    }
                    _currentAverage = tot / values.Count;
                    values.Clear();

                    _currentDiff = max - min;
                    _stable = CurrentDiff <= Tolerance;
                    OnAvg();
                }
                OnTick();
            }
        }

        private void AvgDone(object o, RunWorkerCompletedEventArgs args)
        {
            Reset();
            OnTick();
        }
        
        
        
        
        Megahard.Threading.SyncLock curValLock_ = new Megahard.Threading.SyncLock();
        public void AddValue(double d)
        {
            using(curValLock_.Lock())
                values.Add(_currentValue = d);
        }


        public Func<double> PollFunc;

        public double Tolerance { get; set; }
        public int AvgTime { get; set; }

        private bool _stable;
        public bool Stable { get { return _stable; } }

        private double _currentAverage;
        public double CurrentAverage { get { return _currentAverage; } }

        private double _currentValue;
        public double CurrentValue 
        { 
            get 
            { 
                using(curValLock_.Lock())
                    return _currentValue; 
            } 
        }

        private double _currentDiff;
        public double CurrentDiff { get { return _currentDiff; } }

        private int avgCount;
        public int UpdateTime
        {
            get
            {
                return (int)(((double)avgCount + 999.0) / 1000.0);
            }
        }

        //private System.Windows.Forms.Timer myTimer;
        private BackgroundWorker worker;
        private List<double> values;
    }
}
