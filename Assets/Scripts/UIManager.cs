using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snake.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private List<UIPanel> _panelList;
        private Dictionary<Type,UIPanel> _panelDic = new Dictionary<Type, UIPanel>();
        private GameObject _canvas;

        protected override void OnInitializing()
        {
            base.OnInitializing();
            _canvas = GameObject.Find("Canvas");

            var arr = _canvas.GetComponentsInChildren<UIPanel>();
            _panelDic = arr.ToDictionary(panel => panel.GetType());

            _panelList = arr.ToList();

            foreach (var panel in _panelList)
            {
                panel.Init();
                panel.Hide();
            }

            InitEvent();
        }

        void InitEvent()
        {
            EventManager.Instance.MainMenu += EnterMainMenu;
            EventManager.Instance.StartGame += EnterGame;
        }

        void EnterMainMenu()
        {
            // 进入主菜单
            _panelDic[typeof(GamePanel)].Hide();
            _panelDic[typeof(MainMenuPanel)].Show();
        }
        void EnterGame()
        {
            // 进入游戏
            _panelDic[typeof(MainMenuPanel)].Hide();
            _panelDic[typeof(GamePanel)].Show();
        }
    }
}