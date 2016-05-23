namespace PowerCalibration
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.textBoxOutputStatus = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSamples = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelVoltage = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.labelAve = new System.Windows.Forms.Label();
            this.labelPower = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxOutputStatus
            // 
            this.textBoxOutputStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOutputStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxOutputStatus.Location = new System.Drawing.Point(12, 412);
            this.textBoxOutputStatus.Multiline = true;
            this.textBoxOutputStatus.Name = "textBoxOutputStatus";
            this.textBoxOutputStatus.ReadOnly = true;
            this.textBoxOutputStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOutputStatus.Size = new System.Drawing.Size(664, 66);
            this.textBoxOutputStatus.TabIndex = 3;
            this.textBoxOutputStatus.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(500, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.labelSamples);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.labelVoltage);
            this.panel1.Controls.Add(this.labelCurrent);
            this.panel1.Controls.Add(this.labelAve);
            this.panel1.Controls.Add(this.labelPower);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelMax);
            this.panel1.Controls.Add(this.labelMin);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(663, 131);
            this.panel1.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(144, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 16);
            this.label7.TabIndex = 26;
            this.label7.Text = "Power:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 16);
            this.label6.TabIndex = 25;
            this.label6.Text = "Current:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "Voltage:";
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.BackColor = System.Drawing.SystemColors.Control;
            this.labelSamples.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSamples.Location = new System.Drawing.Point(212, 66);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(64, 18);
            this.labelSamples.TabIndex = 22;
            this.labelSamples.Tag = "v";
            this.labelSamples.Text = "                  ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(144, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Samples:";
            // 
            // labelVoltage
            // 
            this.labelVoltage.AutoSize = true;
            this.labelVoltage.BackColor = System.Drawing.SystemColors.Control;
            this.labelVoltage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVoltage.Location = new System.Drawing.Point(67, 37);
            this.labelVoltage.Name = "labelVoltage";
            this.labelVoltage.Size = new System.Drawing.Size(64, 18);
            this.labelVoltage.TabIndex = 5;
            this.labelVoltage.Tag = "v";
            this.labelVoltage.Text = "                  ";
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.labelCurrent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrent.Location = new System.Drawing.Point(67, 66);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(64, 18);
            this.labelCurrent.TabIndex = 4;
            this.labelCurrent.Tag = "v";
            this.labelCurrent.Text = "                  ";
            // 
            // labelAve
            // 
            this.labelAve.AutoSize = true;
            this.labelAve.BackColor = System.Drawing.SystemColors.Control;
            this.labelAve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAve.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAve.Location = new System.Drawing.Point(342, 75);
            this.labelAve.Name = "labelAve";
            this.labelAve.Size = new System.Drawing.Size(64, 18);
            this.labelAve.TabIndex = 21;
            this.labelAve.Tag = "v";
            this.labelAve.Text = "                  ";
            // 
            // labelPower
            // 
            this.labelPower.AutoSize = true;
            this.labelPower.BackColor = System.Drawing.SystemColors.Control;
            this.labelPower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPower.Location = new System.Drawing.Point(212, 44);
            this.labelPower.Name = "labelPower";
            this.labelPower.Size = new System.Drawing.Size(64, 18);
            this.labelPower.TabIndex = 13;
            this.labelPower.Tag = "v";
            this.labelPower.Text = "                  ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(304, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Ave:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(303, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 19;
            this.label2.Text = "Min:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(303, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Max:";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.BackColor = System.Drawing.SystemColors.Control;
            this.labelMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMax.Location = new System.Drawing.Point(342, 37);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(64, 18);
            this.labelMax.TabIndex = 16;
            this.labelMax.Tag = "v";
            this.labelMax.Text = "                  ";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.SystemColors.Control;
            this.labelMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMin.Location = new System.Drawing.Point(342, 56);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(64, 18);
            this.labelMin.TabIndex = 17;
            this.labelMin.Tag = "v";
            this.labelMin.Text = "                  ";
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(13, 149);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(663, 257);
            this.chart1.TabIndex = 16;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 490);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxOutputStatus);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOutputStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelVoltage;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label labelPower;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelAve;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

