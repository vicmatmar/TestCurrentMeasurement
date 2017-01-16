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

        double _power_max_val = double.MinValue, _power_min_val = double.MaxValue;
        double _power_average = 0.0;
        uint _read_count = 0;

        struct power_data
        {
            public double current;
            public double voltage;
            public DateTime time_stamp;
        }

        List<power_data> data_list = new List<power_data>();

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

            _meter.WriteLine("*CLS");
            _meter.WriteLine("SYST:SCP:MODE NORM");

            _meter.WriteLine("TRIG:AUTO OFF");
            _meter.WriteLine("TRIG:SOUR EXT");
            _meter.WriteLine("TRIG:COUN 1");

            _meter.ConfDisplay("CURR:DC 50E-03", 1);
            _meter.ConfDisplay("VOLT:DC 5", 2);


            //radioButtonFast.Checked = true;
            //radioButtonMedium.Checked = true;
            radioButtonSlow.Checked = true;

            init_chart();

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
            line = string.Format("{0}{1}\r\n", textBoxOutputStatus.Text, line);
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
            controlSetPropertyValue(button1, "Stop", "Text");

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

        }

        void meter_completed(Task task)
        {
            controlSetPropertyValue(button1, "Start", "Text");
        }

        void init_chart()
        {
            string name = "Power";
            chart1.Series.Add(name);
            chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            //chart1.ChartAreas[0].AxisY.ScaleView.Zoom(0, 10);

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;


            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;


            chart1.Series[name].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            //chart1.ChartAreas[0].AxisX.Interval = 1;

        }

        void update_chart(power_data data)
        {
            if (chart1.InvokeRequired)
            {
                updateGUICallback d = new updateGUICallback(update_chart);
                this.Invoke(d, new object[] { data });
            }
            else
            {
                try
                {
                    chart1.Series["Power"].Points.AddXY(data.time_stamp.ToLongTimeString(), data.current * data.voltage);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        void update_gui(power_data data)
        {
            double current_ma = data.current * 1000;
            controlSetPropertyValue(labelVoltage, string.Format("{0:F4} V", data.voltage));
            controlSetPropertyValue(labelCurrent, string.Format("{0:F3} mA", current_ma));
            controlSetPropertyValue(labelSamples, string.Format("{0}", _read_count++));

            update_chart(data);

            double power_mw = data.voltage * current_ma;

            if (power_mw > _power_max_val)
                _power_max_val = power_mw;
            if (power_mw < _power_min_val)
                _power_min_val = power_mw;

            _power_average = (_power_average * (_read_count - 1) + power_mw) / _read_count;

            controlSetPropertyValue(labelPower, string.Format("{0:F6} mW", power_mw));
            controlSetPropertyValue(labelMax, string.Format("{0:F6} mW", _power_max_val));
            controlSetPropertyValue(labelMin, string.Format("{0:F6} mW", _power_min_val));
            controlSetPropertyValue(labelAve, string.Format("{0:F6} mW", _power_average));
        }

        /// <summary>
        /// Takes meter measurements until cancel
        /// </summary>
        void meter_run()
        {
            _meter.ClearData();
            while (true)
            {
                if (_cancel.Token.IsCancellationRequested)
                    return;

                power_data data = new power_data();
                data.time_stamp = DateTime.Now;

                bool success = false;
                _meter.ClearData();
                _meter.WriteLine("*TRG;MEAS:CURR:DC? 50E-03");
                for (int i = 0; i < 3; i++)
                {
                    string meter_output_current = _meter.WaitForData(50);
                    try
                    {
                        data.current = Convert.ToDouble(meter_output_current);
                        success = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        updateOutputStatus("Current: " + ex.Message + " : " + meter_output_current.Replace("\r\n", ","));
                    }
                }

                if (success)
                {
                    success = false;
                    _meter.ClearData();
                    _meter.WriteLine("*TRG;MEAS2:VOLT:DC? 5");
                    for (int i = 0; i < 3; i++)
                    {
                        string meter_output_voltage = _meter.WaitForData(50);
                        try
                        {
                            data.voltage = Convert.ToDouble(meter_output_voltage);
                            success = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            updateOutputStatus("Voltage: " + ex.Message + " : " + meter_output_voltage.Replace("\r\n", ","));
                        }
                    }
                }

                if (success)
                {
                    data_list.Add(data);
                    update_gui(data);
                }

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

                /*
                StreamWriter sw = new StreamWriter("test.txt");
                foreach (power_data data in data_list)
                {
                    sw.WriteLine("{0},{1},{2}", data.time_stamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), data.voltage, data.current);
                }
                sw.Close();*/
            }

        }

        private void radioButtonSampleRate_CheckedChanged(object sender, EventArgs e)
        {

            RadioButton cb = sender as RadioButton;
            if (cb != null && cb.Checked)
            {
                if (radioButtonFast.Checked)
                {
                    _meter.SetSampleRate('F');
                }
                else if (radioButtonMedium.Checked)
                {
                    _meter.SetSampleRate('M');
                }
                else if (radioButtonSlow.Checked)
                {
                    _meter.SetSampleRate('S');
                }
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            _power_max_val = double.MinValue;
            _power_min_val = double.MaxValue;
            _read_count = 0;
            _power_average = 0.0;
        }


    }
}
