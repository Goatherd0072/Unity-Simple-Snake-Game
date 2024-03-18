using System;
using UnityEngine;

namespace Snake
{
    public class DataManager : Singleton<DataManager>
    {
        public Vector4 mapAABB = new();
        public Vector2 foodPos = new();
        public ParfabsConfig parfabsConfig;
        float score = 0f;
        float foodMuti = 25f;

        protected override void OnInitializing()
        {
            base.OnInitializing();
            parfabsConfig = Resources.Load<ParfabsConfig>("Config/ParfabsConfig");
            score = 0f;

            InitEvent();
        }

        void InitEvent()
        {
            EventManager.Instance.GetFood += AddScore;
        }

        void AddScore(int add)
        {
            score += add * foodMuti;
            EventManager.Instance.SetScore?.Invoke(score);
        }
    }
}