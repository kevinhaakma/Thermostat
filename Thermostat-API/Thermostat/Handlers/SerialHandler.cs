using System;
using System.Diagnostics;
using System.IO.Ports;

namespace Thermostat.Handlers
{
    public static class SerialHandler
    {
        private static SerialPort COMPort = new SerialPort("/dev/ttyACM0", 9600, Parity.None, 8, StopBits.One);

        public static void OpenPort()
        {
            try
            {
                COMPort.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Forcing COM Port Closed");
                COMPort.Close();
            }
        }
        public static void ResetTimer()
        {
            if (!COMPort.IsOpen) OpenPort();
            if (COMPort.IsOpen)
            {
                COMPort.DiscardOutBuffer();
                COMPort.WriteLine(2.ToString());
            }
        }

        public static void TurnOn()
        {
            if (!COMPort.IsOpen) OpenPort();
            if (COMPort.IsOpen)
            {
                COMPort.DiscardOutBuffer();
                COMPort.WriteLine(1.ToString());
            }
        }
        public static void TurnOff()
        {
            if (!COMPort.IsOpen) OpenPort();
            if (COMPort.IsOpen)
            {
                COMPort.DiscardOutBuffer();
                COMPort.WriteLine(0.ToString());
            }
        }
        public static double GetLastValue()
        {
            try
            {
                if (!COMPort.IsOpen) OpenPort();
                string lastValue = string.Empty;
                if (COMPort.IsOpen)
                {
                    COMPort.DiscardInBuffer();
                    lastValue = COMPort.ReadLine();
                }
                try
                {
                    return Convert.ToDouble(lastValue);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

        }
    }
}
