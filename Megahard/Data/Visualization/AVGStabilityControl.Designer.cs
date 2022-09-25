﻿using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Navigator;

namespace Megahard.Data.Visualization
{
    partial class AVGStabilityControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxAvg = new KryptonTextBox();
            this.numUpDownAvgTime = new KryptonNumericUpDown();
            this.numUpDownTolerance = new KryptonNumericUpDown();
            this.label_ = new KryptonLabel();
            this.textBoxUpdateTime = new KryptonTextBox();
            this.label1_ = new KryptonLabel();
            this.kryptonPage2 = new KryptonPage();
            this.headerGroup_ = new KryptonHeaderGroup();
            this.labelDiff = new KryptonLabel();
            this.textBoxDiff = new KryptonTextBox();
            this.textBoxCur = new KryptonTextBox();
            this.textBoxStable = new KryptonTextBox();
            this.labelCur = new KryptonLabel();
            this.labelAvg = new KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerGroup_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerGroup_.Panel)).BeginInit();
            this.headerGroup_.Panel.SuspendLayout();
            this.headerGroup_.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxAvg
            // 
            this.textBoxAvg.Location = new System.Drawing.Point(87, 30);
            this.textBoxAvg.Name = "textBoxAvg";
            this.textBoxAvg.ReadOnly = true;
            this.textBoxAvg.Size = new System.Drawing.Size(78, 20);
            this.textBoxAvg.TabIndex = 0;
            // 
            // numUpDownAvgTime
            // 
            this.numUpDownAvgTime.Location = new System.Drawing.Point(123, 55);
            this.numUpDownAvgTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownAvgTime.Name = "numUpDownAvgTime";
            this.numUpDownAvgTime.ReadOnly = true;
            this.numUpDownAvgTime.Size = new System.Drawing.Size(42, 22);
            this.numUpDownAvgTime.TabIndex = 1;
            this.numUpDownAvgTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownAvgTime.ValueChanged += new System.EventHandler(this.numUpDownAvgTime_ValueChanged);
            // 
            // numUpDownTolerance
            // 
            this.numUpDownTolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numUpDownTolerance.Location = new System.Drawing.Point(73, 83);
            this.numUpDownTolerance.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numUpDownTolerance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numUpDownTolerance.Name = "numUpDownTolerance";
            this.numUpDownTolerance.Size = new System.Drawing.Size(92, 22);
            this.numUpDownTolerance.TabIndex = 2;
            this.numUpDownTolerance.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numUpDownTolerance.ValueChanged += new System.EventHandler(this.numUpDownTolerance_ValueChanged);
            // 
            // label_
            // 
            this.label_.Location = new System.Drawing.Point(3, 56);
            this.label_.Name = "label_";
            this.label_.Size = new System.Drawing.Size(64, 20);
            this.label_.TabIndex = 3;
            this.label_.Values.Text = "Update in";
            // 
            // textBoxUpdateTime
            // 
            this.textBoxUpdateTime.Location = new System.Drawing.Point(73, 56);
            this.textBoxUpdateTime.Name = "textBoxUpdateTime";
            this.textBoxUpdateTime.ReadOnly = true;
            this.textBoxUpdateTime.Size = new System.Drawing.Size(44, 20);
            this.textBoxUpdateTime.TabIndex = 4;
            // 
            // label1_
            // 
            this.label1_.Location = new System.Drawing.Point(25, 83);
            this.label1_.Name = "label1_";
            this.label1_.Size = new System.Drawing.Size(44, 20);
            this.label1_.TabIndex = 5;
            this.label1_.Values.Text = "Stable";
            // 
            // kryptonPage2
            // 
            this.kryptonPage2.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage2.Flags = 65535;
            this.kryptonPage2.LastVisibleSet = true;
            this.kryptonPage2.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage2.Name = "kryptonPage2";
            this.kryptonPage2.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage2.Text = "kryptonPage2";
            this.kryptonPage2.ToolTipTitle = "Page ToolTip";
            this.kryptonPage2.UniqueName = "2201E77AE10B4EA22201E77AE10B4EA2";
            // 
            // headerGroup_
            // 
            this.headerGroup_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerGroup_.HeaderVisibleSecondary = false;
            this.headerGroup_.Location = new System.Drawing.Point(0, 0);
            this.headerGroup_.Name = "headerGroup_";
            // 
            // headerGroup_.Panel
            // 
            this.headerGroup_.Panel.Controls.Add(this.labelAvg);
            this.headerGroup_.Panel.Controls.Add(this.labelCur);
            this.headerGroup_.Panel.Controls.Add(this.labelDiff);
            this.headerGroup_.Panel.Controls.Add(this.textBoxDiff);
            this.headerGroup_.Panel.Controls.Add(this.textBoxCur);
            this.headerGroup_.Panel.Controls.Add(this.textBoxStable);
            this.headerGroup_.Panel.Controls.Add(this.textBoxAvg);
            this.headerGroup_.Panel.Controls.Add(this.label_);
            this.headerGroup_.Panel.Controls.Add(this.numUpDownAvgTime);
            this.headerGroup_.Panel.Controls.Add(this.textBoxUpdateTime);
            this.headerGroup_.Panel.Controls.Add(this.label1_);
            this.headerGroup_.Panel.Controls.Add(this.numUpDownTolerance);
            this.headerGroup_.Size = new System.Drawing.Size(175, 180);
            this.headerGroup_.TabIndex = 8;
            this.headerGroup_.ValuesPrimary.Heading = "Avg / Stability";
            this.headerGroup_.ValuesPrimary.Image = null;
            // 
            // labelDiff
            // 
            this.labelDiff.Location = new System.Drawing.Point(37, 112);
            this.labelDiff.Name = "labelDiff";
            this.labelDiff.Size = new System.Drawing.Size(30, 20);
            this.labelDiff.TabIndex = 9;
            this.labelDiff.Values.Text = "Diff";
            // 
            // textBoxDiff
            // 
            this.textBoxDiff.Location = new System.Drawing.Point(73, 112);
            this.textBoxDiff.Name = "textBoxDiff";
            this.textBoxDiff.Size = new System.Drawing.Size(92, 20);
            this.textBoxDiff.TabIndex = 8;
            // 
            // textBoxCur
            // 
            this.textBoxCur.Location = new System.Drawing.Point(3, 30);
            this.textBoxCur.Name = "textBoxCur";
            this.textBoxCur.ReadOnly = true;
            this.textBoxCur.Size = new System.Drawing.Size(78, 20);
            this.textBoxCur.TabIndex = 7;
            // 
            // textBoxStable
            // 
            this.textBoxStable.Location = new System.Drawing.Point(3, 82);
            this.textBoxStable.Name = "textBoxStable";
            this.textBoxStable.ReadOnly = true;
            this.textBoxStable.Size = new System.Drawing.Size(20, 20);
            this.textBoxStable.StateCommon.Back.Color1 = System.Drawing.Color.Red;
            this.textBoxStable.TabIndex = 6;
            // 
            // labelCur
            // 
            this.labelCur.Location = new System.Drawing.Point(3, 4);
            this.labelCur.Name = "labelCur";
            this.labelCur.Size = new System.Drawing.Size(51, 20);
            this.labelCur.TabIndex = 10;
            this.labelCur.Values.Text = "Current";
            // 
            // labelAvg
            // 
            this.labelAvg.Location = new System.Drawing.Point(87, 4);
            this.labelAvg.Name = "labelAvg";
            this.labelAvg.Size = new System.Drawing.Size(55, 20);
            this.labelAvg.TabIndex = 11;
            this.labelAvg.Values.Text = "Average";
            // 
            // AVGStabilityControl
            // 
            this.Controls.Add(this.headerGroup_);
            this.Name = "AVGStabilityControl";
            this.Size = new System.Drawing.Size(175, 180);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerGroup_.Panel)).EndInit();
            this.headerGroup_.Panel.ResumeLayout(false);
            this.headerGroup_.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerGroup_)).EndInit();
            this.headerGroup_.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private KryptonTextBox textBoxAvg;
        private KryptonNumericUpDown numUpDownAvgTime;
        private KryptonNumericUpDown numUpDownTolerance;
        private KryptonLabel label_;
        private KryptonTextBox textBoxUpdateTime;
        private KryptonLabel label1_;
        private KryptonPage kryptonPage2;
        private KryptonHeaderGroup headerGroup_;
        private KryptonTextBox textBoxStable;
        private KryptonTextBox textBoxCur;
        private KryptonLabel labelDiff;
        private KryptonTextBox textBoxDiff;
        private KryptonLabel labelAvg;
        private KryptonLabel labelCur;
    }
}
