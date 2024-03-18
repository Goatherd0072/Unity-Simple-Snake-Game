using UnityEngine;
using UnityEngine.UI;

namespace Snake.UI
{
    public class MainMenuPanel : UIPanel
    {
        Button _startBtn;
        Button _exitBtn;

        public override void Init()
        {
            base.Init();
            _startBtn = transform.Find("btn_Start").GetComponent<Button>();
            _exitBtn = transform.Find("btn_Exit").GetComponent<Button>();

            _startBtn.onClick.AddListener(()=>
            {
                EventManager.Instance.StartGame?.Invoke();
            });

            _exitBtn.onClick.AddListener(()=>
            {
                EventManager.Instance.ExitApp?.Invoke();
            });
        }
    }
}