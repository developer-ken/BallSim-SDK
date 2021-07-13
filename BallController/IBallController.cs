using System;

namespace BallController
{
    public interface IBallController
    {
        /// <summary>
        /// X方向转动
        /// </summary>
        public float XRotation { get; set; }

        /// <summary>
        /// Y方向转动
        /// </summary>
        public float YRotation { get; set; }

        /// <summary>
        /// 球位置坐标获取
        /// </summary>
        public Vector2 BallLocation { get; }

        /// <summary>
        /// 球接触到检查点边缘
        /// </summary>
        public event Action<IBallController, short> BallTouchCheckpointEvent;

        /// <summary>
        /// 球完全进入检查点
        /// </summary>
        public event Action<IBallController, short> BallEnterCheckpointEvent;

        /// <summary>
        /// 球从检查点内离开触碰到边缘
        /// </summary>
        public event Action<IBallController, short> BallLeaveCheckpointEvent;

        /// <summary>
        /// 球完全离开检查点
        /// </summary>
        public event Action<IBallController, short> BallDetouchCheckpointEvent;

        /// <summary>
        /// 球离开板子范围
        /// </summary>
        public event Action<IBallController> BallFallOutEvent;
    }

    public struct Vector2
    {
        public float X, Y;
    }
}
