using BallController;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BallSimilationInterface
{
    public class SimiInterface : IBallController
    {
        Socket client;
        EndPoint server;

        public SimiInterface()
        {
            server = new IPEndPoint(IPAddress.Loopback, 65531);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Loopback, 65532));
            new Thread(new ThreadStart(() =>
            {
                __RecvLoop();
            })).Start();
        }

        public void RecvPack(out string cmd, out string[] args)
        {
            byte[] buffer = new byte[128];
            client.ReceiveFrom(buffer, ref server);
            string[] str = Encoding.UTF8.GetString(buffer).Split('!');
            cmd = str[0].ToUpper();
            args = str[1].Split(',');
        }

        public void SendPack(string cmd, params float[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cmd.ToUpper());
            sb.Append('!');
            foreach (var i in args)
            {
                sb.Append(i);
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);
            client.SendTo(Encoding.UTF8.GetBytes(sb.ToString()), server);
        }

        public bool Ping()
        {
            SendPack("ping", 0);
            RecvPack(out string cmd, out string[] args);
            return cmd.Equals("PONG");
        }

        public float f(string dt)
        {
            return float.Parse(dt);
        }
        public short s(string dt)
        {
            return short.Parse(dt);
        }

        public void __RecvLoop()
        {
            while (true)
            {
                RecvPack(out string cmd, out string[] args);
                switch (cmd)
                {
                    case "S_BL"://小球位置
                        x = f(args[0]);
                        y = f(args[2]);
                        break;
                    case "S_BR"://板子倾斜角
                        rx = f(args[0]);
                        ry = f(args[2]);
                        break;
                    case "ERR":
                        Console.WriteLine("Similator error:" + args[1]);
                        break;
                    case "OB":
                        BallFallOutEvent?.Invoke(this);
                        break;
                    case "EN":
                        BallTouchCheckpointEvent?.Invoke(this, s(args[0]));
                        break;
                    case "IN":
                        BallEnterCheckpointEvent?.Invoke(this, s(args[0]));
                        break;
                    case "LV":
                        BallLeaveCheckpointEvent?.Invoke(this, s(args[0]));
                        break;
                    case "EX":
                        BallDetouchCheckpointEvent?.Invoke(this, s(args[0]));
                        break;
                }
            }
        }

        private float x, y, rx, ry;

        public float XRotation { get => rx; set { rx = value; SendPack("S_RA", value, 0, YRotation); } }
        public float YRotation { get => ry; set { ry = value; SendPack("S_RA", XRotation, 0, value); } }

        public Vector2 BallLocation => new Vector2 { X = x, Y = y };

        public event Action<IBallController, short> BallTouchCheckpointEvent;
        public event Action<IBallController, short> BallEnterCheckpointEvent;
        public event Action<IBallController, short> BallLeaveCheckpointEvent;
        public event Action<IBallController, short> BallDetouchCheckpointEvent;
        public event Action<IBallController> BallFallOutEvent;
    }
}
