using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snake.UI
{
    public class GamePanel : UIPanel
    {
        Button _mainMenuBtn;
        TMP_Text _textScore;
        GameObject _goGameOver;

        public override void Init()
        {
            base.Init();
            _mainMenuBtn = transform.Find("btn_MainMenu").GetComponent<Button>();
            _textScore = transform.Find("text_score/text_score").GetComponent<TMP_Text>();
            _goGameOver = transform.Find("text_gameOver").gameObject;

            _mainMenuBtn.onClick.AddListener(() =>
            {
                EventManager.Instance.MainMenu?.Invoke();
            });
            _textScore.text = "0";

            InitEvent();
        }

        void InitEvent()
        {
            EventManager.Instance.SetScore += SetScore;
            EventManager.Instance.GameOver += ShowGameOver;
        }

        public override void Show()
        {
            base.Show();
            _goGameOver.SetActive(false);
        }

        public void ShowGameOver()
        {
            _goGameOver.SetActive(true);
        }

        public void SetScore(float score)
        {
            _textScore.text = Mathf.Ceil(score).ToString();
        }

    }
}