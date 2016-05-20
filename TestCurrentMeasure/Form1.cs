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
using System.IO;

using System.IO.Ports;

namespace PowerCalibration
{
    public partial class Form1 : Form
    {
        MultiMeter _meter = null; // The multimeter controller

        delegate void setControlPropertyValueCallback(Control control, object value, string property_name);  // Set object text
        delegate void updateGUICallback(power_data data);

        Task _meter_task;
        CancellationTokenSource _cancel;

        double _max_val = double.MinValue, _min_val = double.MaxValue;
        double _average = 0.0;
        uint _read_count = 0;

        struct power_data
        {
            public double current;
            public double voltage;
        }

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Init GUI, meter and start to take measurements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Setup meter
            autoDetectMeterCOMPort();
            _meter.Init();

            _meter.ConfDisplay("CURR:DC 50", 1);
            _meter.ConfDisplay("VOLT:DC 5", 2);

        }

        /// <summary>
        /// Called when the measure task throws an exception
        /// </summary>
        /// <param name="task"></param>
        void meter_error(Task task)
        {
            string text = textBoxOutputStatus.Text;
            text += "\r\n" + task.Exception.InnerException.Message;

            controlSetPropertyValue(textBoxOutputStatus, text);
            controlSetPropertyValue(button1, "Start");
        }

        /// <summary>
        /// Detects whether the meter is ON and connected to one of the COM ports
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
        /// Updates the output status text box
        /// </summary>
        /// <param name="text"></param>
        void updateOutputStatus(string text)
        {
            string line = string.Format("{0:G}: {1}", DateTime.Now, text);
            line = string.Format("{0}\r\n", line);
            controlSetPropertyValue(this.textBoxOutputStatus, line);
        }

        /// <summary>
        /// Sets the text property of any control as long as it has one
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        void controlSetPropertyValue(Control control, object value, string property_name = "Text")
        {
            if (control.InvokeRequired)
            {
                setControlPropertyValueCallback d = new setControlPropertyValueCallback(controlSetPropertyValue);
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
        /// Starts the task
        /// </summary>
        void start_task()
        {
            button1.Text = "Stop";

            create_task();
            _meter_task.Start();
        }

        /// <summary>
        /// Creates the task and cancelation source
        /// </summary>
        void create_task()
        {
            _cancel = new CancellationTokenSource();
            _meter_task = new Task(meter_run, _cancel.Token);
            _meter_task.ContinueWith(meter_completed, TaskContinuationOptions.OnlyOnRanToCompletion);
            _meter_task.ContinueWith(meter_error, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Cancels the task
        /// </summary>
        void cancel_task()
        {

            if (_cancel != null && _cancel.Token.CanBeCanceled)
                _cancel.Cancel();

            button1.Text = "Start";

        }

        void meter_completed(Task task)
        {
        }

        void update_gui(power_data data)
        {

            if (this.InvokeRequired)
            {
                updateGUICallback d = new updateGUICallback(update_gui);
                this.Invoke(d, new object[] { data });
            }
            else
            {

                double power = data.voltage * data.current;

                if (power > _max_val)
                    _max_val = power;
                if (power < _min_val)
                    _min_val = power;

                _read_count++;

                _average = (_average * (_read_count - 1) + power) / _read_count;

                labelVoltage.Text = string.Format("{0:F4} V", data.voltage);
                labelCurrent.Text = string.Format("{0:F3} mA", data.current);
                labelPower.Text = string.Format("{0:F6} mW", power);
                labelMax.Text = string.Format("{0:F6} mW", _max_val);
                labelMin.Text = string.Format("{0:F6} mW", _min_val);
                labelSamples.Text = string.Format("{0}", _read_count);
                labelAve.Text = string.Format("{0:F6} mW", _average);
            }
        }

        /// <summary>
        /// Takes meter measurements until cancel
        /// </summary>
        void meter_run()
        {
            while (true)
            {
                if (_cancel.Token.IsCancellationRequested)
                    return;

                _meter.Triger();

                double[] values = _meter.Read();

                power_data data = new power_data();
                data.voltage = values[0];
                data.current = values[1];

                update_gui(data);

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
