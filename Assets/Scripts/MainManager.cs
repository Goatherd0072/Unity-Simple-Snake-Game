using System.Collections;
using System.Collections.Generic;
using Snake.UI;
using UnityEngine;

namespace Snake
{
    public class MainManager : PersistentMonoSingleton<MainManager>
    {
        List<ISingleton> managerList;

        protected override void OnInitializing()
        {
            base.OnInitializing();

            // 非MonoBehaviour的单例
            managerList = new List<ISingleton>
            {
                EventManager.Instance,
                DataManager.Instance,
                UIManager.Instance,
                GameManager.Instance,
            };
            EventManager.Instance.ExitApp += ExitApplicaiton;
            EnterMainMenu();
        }

        void EnterMainMenu()
        {
            // 进入主菜单
            EventManager.Instance.MainMenu?.Invoke();
        }
        void EnterGame()
        {
            // 进入游戏
            EventManager.Instance.StartGame?.Invoke();
        }

        void ExitApplicaiton()
        {
            // 退出应用
            Application.Quit();
        }
    }
}
