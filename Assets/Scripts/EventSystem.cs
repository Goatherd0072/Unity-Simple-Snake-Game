using System;
using UnityEngine;

namespace Snake
{
    public class EventManager : Singleton<EventManager>
    {
        /// <summary>
        /// 增加snake的长度
        /// </summary>
        public Action<int> GetFood;
        public Action<Vector4> SetMapData;
        public Func<Vector2, bool> CheckSnakePos;
        public Action StartGame;
        public Action MainMenu;
        public Action ExitApp;
        public Action<float> SetScore;
        public Action GameOver;
    }
}
