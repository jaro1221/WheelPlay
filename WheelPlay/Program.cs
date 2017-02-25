using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace WheelPlay
{
    public class SerialData
    {
        public struct AxisStruct
        {
            public uint X { get; set; }
            public uint Y { get; set; }
            public uint Z { get; set; }
            public uint Rx { get; set; }
            public uint Ry { get; set; }
            public uint Rz { get; set; }
        }

        public struct ButtonsStruct
        {
            public bool B1 { get; set; }
            public bool B2 { get; set; }
            public bool B3 { get; set; }
            public bool B4 { get; set; }
            public bool B5 { get; set; }
            public bool B6 { get; set; }
            public bool B7 { get; set; }
            public bool B8 { get; set; }
            public bool B9 { get; set; }
            public bool B10 { get; set; }


        }
        public AxisStruct Axis = new AxisStruct();
        public ButtonsStruct Buttons = new ButtonsStruct();

        public void GetData(string[] data)
        {
            // DATA

        }
    }


    class Program
    {


        static public SerialData Data;
        static public vJoy joystick;
        static public vJoy.JoystickState jState;
        static public int id = 1;
        static System.IO.Ports.SerialPort serialPort;

        static void Main(string[] args)
        {
            Data = new SerialData();
            serialPort = new System.IO.Ports.SerialPort("COM1", 9600);
            joystick = new vJoy();
            jState = new vJoy.JoystickState();

            Console.WriteLine("WheelPlay alpha 0.1");
            Console.WriteLine("Coded by imn1oy");
            Console.WriteLine("===================\n");

            Initialize();
            Console.WriteLine("Press any key to start a program...");
            Console.ReadKey();

            joystick.ResetVJD((uint)id);
            serialPort.Open();
            Console.Clear();
            Console.WriteLine("Press 'X' to end session.");

            while (true)
            {
                GetDataFromSerial();
                SetJoystickData();
                if (Console.ReadKey() == new ConsoleKeyInfo('x', ConsoleKey.X, false, false, false))
                {
                    serialPort.Close();
                    joystick.ResetVJD((uint)id);
                    joystick.ResetButtons((uint)id);
                    Environment.Exit(0);
                }
            }


        }

        private static void SetJoystickData()
        {
            bool res;

            res = joystick.SetAxis((int)Data.Axis.X, (uint)id, HID_USAGES.HID_USAGE_X);
            res = joystick.SetAxis((int)Data.Axis.Y, (uint)id, HID_USAGES.HID_USAGE_Y);
            res = joystick.SetAxis((int)Data.Axis.Z, (uint)id, HID_USAGES.HID_USAGE_Z);
            res = joystick.SetAxis((int)Data.Axis.Rx, (uint)id, HID_USAGES.HID_USAGE_RX);
            res = joystick.SetAxis((int)Data.Axis.Ry, (uint)id, HID_USAGES.HID_USAGE_RY);
            res = joystick.SetAxis((int)Data.Axis.Rz, (uint)id, HID_USAGES.HID_USAGE_RZ);

            res = joystick.SetBtn(Data.Buttons.B1, (uint)id, 1);
            // and other buttons...

        }

        private static void GetDataFromSerial()
        {
            string serialString;
            string[] serialData;

            if (serialPort.IsOpen)
                serialString = serialPort.ReadLine();
            else
            {
                Console.WriteLine("Error! Serial port is not open. Trying to open...");
                serialPort.Open();
                return;
            }

            serialData = serialString.Split(';');
            Data.GetData(serialData);

        }

        private static void Initialize()
        {
            Console.Write("Joystick (ID: {0}) state: ", id);
            if (joystick.vJoyEnabled())
            {
                Console.Write("Enabled\n");
                Console.WriteLine("Vendor: {0}", joystick.GetvJoyManufacturerString());
                Console.WriteLine("Product: {0}", joystick.GetvJoyProductString());
                Console.WriteLine("Version: {0}\n", joystick.GetvJoyVersion());
            }

            else
            {
                Console.Write("Disabled");
                Console.Write("\nERROR!");
                return;
            }
            VjdStat status = joystick.GetVJDStatus((uint)id);

            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", id);
                    break;
                case VjdStat.VJD_STAT_FREE:
                    Console.WriteLine("vJoy Device {0} is free\n", id);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Console.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id);
                    return;
                case VjdStat.VJD_STAT_MISS:
                    Console.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", id);
                    return;
                default:
                    Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", id);
                    return;
            };

            bool AxisX = joystick.GetVJDAxisExist((uint)id, HID_USAGES.HID_USAGE_X);
            bool AxisY = joystick.GetVJDAxisExist((uint)id, HID_USAGES.HID_USAGE_Y);
            bool AxisZ = joystick.GetVJDAxisExist((uint)id, HID_USAGES.HID_USAGE_Z);
            bool AxisRX = joystick.GetVJDAxisExist((uint)id, HID_USAGES.HID_USAGE_RX);
            bool AxisRY = joystick.GetVJDAxisExist((uint)id, HID_USAGES.HID_USAGE_RY);
            bool AxisRZ = joystick.GetVJDAxisExist((uint)id, HID_USAGES.HID_USAGE_RZ);
            int nButtons = joystick.GetVJDButtonNumber((uint)id);
            int ContPovNumber = joystick.GetVJDContPovNumber((uint)id);
            int DiscPovNumber = joystick.GetVJDDiscPovNumber((uint)id);

            Console.WriteLine("vJoy Device {0} capabilities:", id);
            Console.WriteLine("Numner of buttons: {0}", nButtons);
            Console.WriteLine("Numner of Continuous POVs: {0}", ContPovNumber);
            Console.WriteLine("Numner of Descrete POVs: {0}", DiscPovNumber);
            Console.WriteLine("Axis X: {0}", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Y: {0}", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Z: {0}", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Rx: {0}", AxisRX ? "Yes" : "No");
            Console.WriteLine("Axis Ry: {0}", AxisRY ? "Yes" : "No");
            Console.WriteLine("Axis Rz: {0}", AxisRZ ? "Yes" : "No");

            UInt32 DllVer = 0, DrvVer = 0;
            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
                Console.WriteLine("Version of Driver Matches DLL Version ({0:X})", DllVer);
            else
                Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})", DrvVer, DllVer);

            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD((uint)id))))
            {
                Console.WriteLine("\nFailed to acquire vJoy device number {0}.", id);
                return;
            }
            else
                Console.WriteLine("\nAcquired: vJoy device number {0}.", id);

            return;
        }
    }
}
