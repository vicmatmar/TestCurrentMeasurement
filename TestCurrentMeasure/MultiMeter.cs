﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace PowerCalibration
{
    class MultiMeter : IDisposable
    {
        public bool WaitForDsrHolding
        {
            get { return _waitForDsrHolding; }
            set { _waitForDsrHolding = value; }
        }

        public bool IsSerialPortOpen
        {
            get { return this._serialPort.IsOpen; }
        }

        private bool _waitForDsrHolding = true;
        private string _portName;
        private SerialPort _serialPort;
        private string _value_txt = "";

        public enum Models { NONE, HP34401A, GDM8341 };
        private Models _model = Models.NONE;
        public Models Model { get { return _model; } }

        int _read_delay = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portName"></param>
        public MultiMeter(string portName)
        {
            this._portName = portName;

            _serialPort = new SerialPort();
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }

        /// <summary>
        /// Open serial port
        /// </summary>
        /// <returns></returns>
        public SerialPort OpenComPort()
        {
            //if (_serialPort != null && _serialPort.IsOpen)
            //{
            //    _serialPort.Close();
            //}
            //_serialPort = new SerialPort(_portName, 600, Parity.None, 8, StopBits.One);
            _serialPort.PortName = _portName;
            //_serialPort.BaudRate = 9600;
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.DtrEnable = true;

            // Calculated using GDM8341 and reading two displays
            // May need to adjust for different meter, computer or more data coming across the port
            _read_delay = (_serialPort.BaudRate - 131446) / -1624;

            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }

            return _serialPort;
        }

        /// <summary>
        /// Handle data received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //lock (_value_txt)
            _value_txt += _serialPort.ReadExisting();
        }

        /// <summary>
        /// Waits for data
        /// </summary>
        /// <returns></returns>
        string waitForData()
        {
            int n = 0;
            while (_value_txt == "")
            {
                Thread.Sleep(_read_delay);
                if (n++ > 10)
                {
                    break;
                }
            }
            n = 0;
            while (_serialPort.BytesToRead > 0)
            {
                Thread.Sleep(_read_delay);
                if (n++ > 10)
                {
                    break;
                }
            }

            return _value_txt;
        }

        /// <summary>
        /// Clears our data holder
        /// </summary>
        void clearData()
        {
            //lock (_value_txt)
            _serialPort.ReadExisting();
            _value_txt = "";
        }

        /// <summary>
        /// Writes to meter
        /// </summary>
        /// <param name="cmd"></param>
        public void writeLine(string cmd)
        {
            int n;
            if (_waitForDsrHolding)
            {
                n = 0;
                while (!_serialPort.DsrHolding)
                {
                    Thread.Sleep(250);
                    if (n++ > 20)
                    {
                        string msg = "Multimeter not responding to serial commands.";
                        msg += "  Make sure multi-meter is on and serial cable connected.";
                        msg += "  DSR holding not set";
                        throw new Exception(msg);
                    }
                }
            }

            _serialPort.WriteLine(cmd);
            Thread.Sleep(250);

            n = 0;
            while (_serialPort.BytesToWrite > 0)
            {
                Thread.Sleep(100);
                if (n++ > 20)
                    throw new Exception("Multimeter write buffer did not empty");
            }
        }

        /// <summary>
        /// Clears the meters error status
        /// </summary>
        public void ClearError()
        {
            writeLine("*CLS");
        }

        /// <summary>
        /// Sets meter to remote mode
        /// </summary>
        public void SetToRemote()
        {
            writeLine("SYST:REM");
        }

        /// <summary>
        /// Gets the meter id and sets the Model if id is recognized
        /// </summary>
        /// <returns>meter id string</returns>
        public string IDN()
        {
            clearData();
            writeLine("*IDN?");
            string data = waitForData();

            if (data.StartsWith("HEWLETT-PACKARD,34401A"))
                _model = Models.HP34401A;
            else if (data.StartsWith("GWInstek,GDM8341"))
                _model = Models.GDM8341;

            return data;
        }

        /// <summary>
        /// Call this function to open the com port and
        /// This will also detect meter model and setup WaitForDsrHolding
        /// among other things
        /// </summary>
        public void Init()
        {
            OpenComPort();

            if (Model == Models.NONE)
            {
                WaitForDsrHolding = false;
                string data = IDN();
            }

            switch (Model)
            {
                case Models.HP34401A:
                    WaitForDsrHolding = true; //use hardware handshake
                    break;
                case Models.GDM8341:
                    WaitForDsrHolding = false;  // don't use hardware handshake
                    writeLine("SYST:SCP:MODE COMP");  //Compatible to GDM8246
                    writeLine("TRIG:SOUR EXT");  // Set trigger to be external
                    writeLine("TRIG:COUN 1");  // Set trigger COUNT
                    writeLine("TRIG:AUTO OFF");  // Turn auto trigger off
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sets up the meter for V AC measurement
        /// </summary>
        public void SetupForVAC()
        {
            switch (Model)
            {
                case Models.HP34401A:
                    writeLine(":CONF:VOLT:AC 1000,0.01");
                    break;
                case Models.GDM8341:
                    writeLine("CONF:VOLT:AC 500");
                    Thread.Sleep(1000);
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sets up the meter for V DC measurement
        /// </summary>
        public void SetupForVDC()
        {
            switch (Model)
            {
                case Models.HP34401A:
                    writeLine(":CONF:VOLT:DC 10,0.01");
                    break;
                case Models.GDM8341:
                    writeLine("CONF:VOLT:DC 5");
                    Thread.Sleep(1000);
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sets up the meter for I AC measurement
        /// </summary>
        public void SetupForIAC()
        {
            switch (Model)
            {
                case Models.HP34401A:
                    writeLine(":CONF:CURR:AC 1,0.000001");
                    break;
                case Models.GDM8341:
                    // Note that input should be on white 0.5 A terminal
                    // Make sure COM is set to COMMun
                    writeLine("CONF:CURR:AC 500");
                    Thread.Sleep(1000);
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sets up for dc amps on specified display
        /// Only applies to GDM8341
        /// </summary>
        /// <param name="range"></param>
        public void SetupForIDC(string range)
        {
            switch (Model)
            {
                case Models.GDM8341:
                    // Note that input should be on white 0.5 A terminal
                    // Make sure COM is set to COMMun
                    writeLine("CONF:CURR:DC " + range);
                    break;
            }
        }

        /// <summary>
        /// Sets up specific display
        /// Only applies to GDM8341
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="display"></param>
        public void ConfDisplay(string setting, uint display)
        {
            switch (Model)
            {
                case Models.GDM8341:
                    string conf = "CONF";
                    if (display == 2)
                        conf = "CONF2";

                    string cmd = string.Format("{0}:{1}", conf, setting);
                    writeLine(cmd);
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Send Read? command and returns values
        /// </summary>
        /// <returns></returns>
        public double[] Read()
        {
            double[] values = new double[] { 0.0, 0.0 };
            clearData();
            switch (Model)
            {
                case Models.GDM8341:
                    //writeLine("READ?");
                    _serialPort.WriteLine("READ?");

                    string data = waitForData();
                    try
                    {
                        string[] valstr = data.Split(',');
                        values[0] = Convert.ToDouble(valstr[0]);
                        values[1] = Convert.ToDouble(valstr[1]);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        throw;
                    }
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }

            return values;
        }

        /// <summary>
        /// Sets up the meter for Resistance measurement
        /// Only GDM8341
        /// </summary>
        /// <param name="range"></param>
        public void SetupForResistance(string range)
        {
            switch (Model)
            {
                case Models.GDM8341:
                    string conf = "CONF:RES " + range;
                    writeLine(conf);
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sets up the meter for Continuity measurement
        /// Only GDM8341
        /// </summary>
        public void SetupForContinuity()
        {
            switch (Model)
            {
                case Models.GDM8341:
                    writeLine("CONF:CONT");
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sets up the meter for Capacitance measurement
        /// Only GDM8341
        /// </summary>
        /// <param name="range"></param>
        public void SetupForCapacitance(int range)
        {
            switch (Model)
            {
                case Models.GDM8341:
                    string conf = string.Format("CONF:CAP {0}", range);
                    writeLine(conf);
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }
        }

        /// <summary>
        /// Sends TRG command
        /// </summary>
        public void Triger()
        {
            if (Model == Models.HP34401A)
            {
                writeLine("TRIG:SOUR BUS");
                writeLine("INIT");
                writeLine("*TRG");
            }
            else
            {
                _serialPort.WriteLine("*TRG");
            }
        }

        /// <summary>
        /// Triggers meter and returns measurement
        /// </summary>
        /// <returns>measurement</returns>
        public string Measure()
        {
            clearData();

            Triger();

            switch (Model)
            {
                case Models.HP34401A:
                    writeLine(":FETC?");
                    break;
                case Models.GDM8341:
                    writeLine("VAL1?");
                    break;
                default:
                    throw new Exception("Unsupported model: " + Model);
            }

            string data = waitForData();
            //data = data.TrimEnd(new char[] { '\r', '\n' });

            return data;
        }

        /// <summary>
        /// Closes the serial port
        /// </summary>
        public void CloseSerialPort()
        {
            _serialPort.Close();
        }
    }
}
