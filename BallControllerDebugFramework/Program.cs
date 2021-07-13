using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BallSimilationInterface;

namespace BallControllerDebugFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            SimiInterface ctl = new SimiInterface();
            Console.Write("Connecting to 3d scene...");
            if (ctl.Ping()) Console.WriteLine("OK");
            ctl.XRotation = 30;
            Thread.Sleep(3000);
            Random rnd = new Random();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ball :" + ctl.BallLocation.X + "," + ctl.BallLocation.Y);
                Console.WriteLine("Board :" + ctl.XRotation + "," + ctl.YRotation);
                ctl.XRotation = rnd.Next(-5, 5);
                ctl.YRotation = rnd.Next(-5, 5);
                Thread.Sleep(100);
            }
        }
    }
}
