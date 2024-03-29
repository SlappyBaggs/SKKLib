﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
    public partial class AVGStabilityControl : UserControl
    {
        internal AVGStability avgStab = new AVGStability();
        public event AVGStability.AVGTickHandler StabTick
        {
            add { avgStab.AVGStabilityTick += value; }
            remove { avgStab.AVGStabilityTick -= value; }
        }

        public AVGStabilityControl()
        {
            InitializeComponent();

            avgStab.AVGTick += new AVGStability.AVGTickHandler(OnChange);
            textBoxStable.Visible = false;
            NumDecimals = 2;
        }

        private void numUpDownAvgTime_ValueChanged(object sender, EventArgs e)
        {
            avgStab.AvgTime = Convert.ToInt32(numUpDownAvgTime.Value);
        }

        private void numUpDownTolerance_ValueChanged(object sender, EventArgs e)
        {
            avgStab.Tolerance = Convert.ToDouble(numUpDownTolerance.Value);
        }

        private void OnChange(object o, EventArgs e)
        {
            if (textBoxAvg.InvokeRequired)
            {
                textBoxAvg.BeginInvoke((Action)delegate { OnChange(o, e); } );
            }
            else
            {
                if (avgStab.Stable)
                    textBoxStable.StateCommon.Back.Color1 = System.Drawing.Color.Green;
                else
                    textBoxStable.StateCommon.Back.Color1 = System.Drawing.Color.Red;

                textBoxAvg.Text = CurrentAverage.ToString("F" + NumDecimals.ToString());
                textBoxCur.Text = CurrentValue.ToString("F" + NumDecimals.ToString());
                textBoxDiff.Text = CurrentDiff.ToString("F" + NumDecimals.ToString());
                textBoxUpdateTime.Text = UpdateTime.ToString();
            }
        }

        public void AddValue(double d)
        {
            avgStab.AddValue(d);
        }

        [Category("AvgStability")]
        [DefaultValue(2)]
        private int numDecimals_ = 2;
        public int NumDecimals
        {
            get { return numDecimals_; }
            set
            {
                numDecimals_ = value;
                numUpDownTolerance.DecimalPlaces = numDecimals_;
            }
        }

        [Category("AvgStability")]
        [DefaultValue(5)]
        public int AvgTime
        {
            get { return avgStab.AvgTime; }
            set
            {
                avgStab.AvgTime = value;
                numUpDownAvgTime.Value = (decimal)value;
            }
        }

        [Browsable(false)]
        public double Tolerance
        {
            get { return avgStab.Tolerance; }
            set
            {
                avgStab.Tolerance = value;
                numUpDownTolerance.Value = (decimal)value;
            }
        }

        [Category("AvgStability")]
        [DefaultValue(1)]
        public decimal ToleranceIncrement
        {
            get { return numUpDownTolerance.Increment;  }
            set { numUpDownTolerance.Increment = value; }
        }

        [Category("AvgStability")]
        [DefaultValue(0)]
        public decimal ToleranceMin
        {
            get { return numUpDownTolerance.Minimum; }
            set { numUpDownTolerance.Minimum = value; }
        }

        [Category("AvgStability")]
        [DefaultValue(100)]
        public decimal ToleranceMax
        {
            get { return numUpDownTolerance.Maximum; }
            set { numUpDownTolerance.Maximum = value; }
        }

        [Category("AvgStability")]
        [DefaultValue(500)]
        public int TimerInterval
        {
            get { return avgStab.TimerInterval; }
            set
            {
                avgStab.TimerInterval = value;
                numUpDownAvgTime.Value = (decimal)value;
            }
        }

        [Browsable(false)]
        public Func<double> PollFunc
        {
            get { return avgStab.PollFunc; }
            set { avgStab.PollFunc = value; }
        }

        [Browsable(false)]
        public bool Stable
        {
            get { return avgStab.Stable; }
        }

        [Browsable(false)]
        public double CurrentAverage
        {
            get { return avgStab.CurrentAverage; }
        }

        [Browsable(false)]
        public double CurrentValue
        {
            get { return avgStab.CurrentValue; }
        }

        [Browsable(false)]
        public double CurrentDiff
        {
            get { return avgStab.CurrentDiff;  }
        }

        [Browsable(false)]
        public int UpdateTime
        {
            get { return avgStab.UpdateTime; }
        }

        [Category("AvgStability")]
        [DefaultValue("AVG / Stability")]
        public string Title
        {
            get { return this.headerGroup_.ValuesPrimary.Heading; }
            set { this.headerGroup_.ValuesPrimary.Heading = value; }
        }

        public void Start()
        {
            if (textBoxStable.InvokeRequired)
                textBoxStable.BeginInvoke((Action)delegate { Start(); });
            else
            {
                avgStab.Start();
                textBoxStable.Visible = true;
            }
        }
        
        public void Stop()
        {
            if (textBoxCur.InvokeRequired)
                textBoxCur.BeginInvoke((Action)delegate { Stop(); });
            else
            {
                avgStab.Stop();
                TrueStop();
            }
        }
    
        private void TrueStop()
        {
            textBoxCur.Text = String.Empty;
            textBoxAvg.Text = String.Empty;
            textBoxUpdateTime.Text = String.Empty;
            textBoxStable.Visible = false;
        }
    }
}
