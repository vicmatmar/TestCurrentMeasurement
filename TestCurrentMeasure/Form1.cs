using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;

namespace PowerCalibration
{
    public partial class Form1 : Form
    {
        MultiMeter _meter = null; // The multimeter controller

        delegate void setControlPropertyValueCallback(Control control, object value, string property_name);  // Set object text

        Task _measure_task;
        CancellationTokenSource _cancel;

        public Form1()
        {
            InitializeComponent();

            button1.Text = "Start";
            create_task();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            labelVoltage.Text = "";
            labelCurrent.Text = "";
            labelPower.Text = "";


            autoDetectMeterCOMPort();
            _meter.Init();

            _meter.ConfDisplay("CURR:DC 50", 1);
            _meter.ConfDisplay("VOLT:DC 5", 2);

            start_task();
        }

        void measure_error(Task task)
        {
            string text = textBoxOutputStatus.Text;
            text += "\r\n" + task.Exception.InnerException.Message;

            controlSetPropertiValue(textBoxOutputStatus, text);
            controlSetPropertiValue(button1, "Start");
        }

        /// <summary>
        /// Detects whether the meter is ON and connected to one of the COM ports
        /// If one is found, the serial port setting is changed automatically
        /// </summary>
        /// <returns>Whether a meter was detected connected to the system</returns>
        bool autoDetectMeterCOMPort()
        {
            bool detected = false;
            string[] ports = SerialPort.GetPortNames();
            foreach (string portname in ports)
            {
                _meter = new MultiMeter(portname);
                try
                {
                    _meter.WaitForDsrHolding = false;
                    _meter.OpenComPort();
                    string idn = _meter.IDN();
                    _meter.CloseSerialPort();

                    if (idn.StartsWith("GWInstek,GDM8341"))
                    {
                        detected = true;
                        string msg = string.Format("Multimeter '{0}' communications port auto detected at {1}", idn.TrimEnd('\n'),
                           portname);
                        updateOutputStatus(msg);

                        break;
                    }

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

            }
            if (!detected)
            {
                string msg = string.Format("Unable to detect Multimeter communications port");
                updateOutputStatus(msg);
            }

            return detected;

        }

        /// <summary>
        /// Updates the output status text box and log file
        /// </summary>
        /// <param name="text"></param>
        void updateOutputStatus(string text)
        {
            string line = string.Format("{0:G}: {1}", DateTime.Now, text);
            line = string.Format("{0}\r\n", line);
            controlSetPropertiValue(this.textBoxOutputStatus, line);
        }

        /// <summary>
        /// Sets the text property of any control as long as it has one
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        void controlSetPropertiValue(Control control, object value, string property_name = "Text")
        {
            if (control.InvokeRequired)
            {
                setControlPropertyValueCallback d = new setControlPropertyValueCallback(controlSetPropertiValue);
                this.Invoke(d, new object[] { control, value, property_name });
            }
            else
            {
                var property = control.GetType().GetProperty(property_name);
                if (property != null)
                {
                    property.SetValue(control, value);
                }
            }
        }


        /// <summary>
        /// Process cmd key events
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                cancel_task();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Starts the task if not running
        /// </summary>
        void start_task()
        {
            if (_measure_task.Status != TaskStatus.Running)
            {
                if (_measure_task.IsCompleted)
                    create_task();
                _measure_task.Start();
            }

            button1.Text = "Stop";
        }

        void create_task()
        {
            _cancel = new CancellationTokenSource();
            _measure_task = new Task(measure, _cancel.Token);
            _measure_task.ContinueWith(measure_error, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Cancels the task
        /// </summary>
        void cancel_task()
        {
            if (_cancel != null && _cancel.Token.CanBeCanceled)
                _cancel.Cancel();
            Task.WaitAll();
            button1.Text = "Start";
        }

        /// <summary>
        /// Takes meter measurements until cancel
        /// </summary>
        void measure()
        {
            while (true)
            {
                if (_cancel.Token.IsCancellationRequested)
                    return;

                _meter.Triger();

                double[] values = _meter.Read();

                controlSetPropertiValue(labelVoltage, string.Format("{0:F4}V", values[0]));
                controlSetPropertiValue(labelCurrent, string.Format("{0:F3}mA", values[1]));
                controlSetPropertiValue(labelPower, string.Format("{0:F4}mW", values[0] * values[1]));
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancel_task();
            _meter.CloseSerialPort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                start_task();
            }
            else 
            { 
                cancel_task();
            }

        }

    }
}
