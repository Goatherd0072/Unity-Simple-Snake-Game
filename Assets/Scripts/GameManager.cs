using System.Collections;
using System.Collections.Generic;
using Snake;
using UnityEngine;

namespace Snake
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        List<ISingleton> systemList;

        protected override void OnInitializing()
        {
            base.OnInitializing();
            systemList = new List<ISingleton>
            {
                // SnakeController.Instance,
                // MapSystem.Instance,
            };

            EventManager.Instance.StartGame += StartGame;
            EventManager.Instance.MainMenu += QuitGame;
        }

#if UNITY_EDITOR
        void Update()
        {
            // Debug.Log("Build To Delete");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                QuitGame();
            }
        }
#endif

        void StartGame()
        {
            systemList = new List<ISingleton>
        {
            SnakeController.Instance,
            MapSystem.Instance,
        };
        }

        void QuitGame()
        {
            foreach (var system in systemList)
            {
                system.ClearSingleton();
            }

            systemList = new List<ISingleton>
            {

            };
        }
    }
}