using System.Text;
using _Strategy._Main.Abstractions;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    public class GameOverScreenPresenter : MonoBehaviour
    {

        [SerializeField] private GameObject _view;
        [SerializeField] private TextMeshProUGUI _text;

        [Inject] private IGameStatus _gameStatus;


        [Inject]
        private void Init()
        {
            _gameStatus.Status.ObserveOnMainThread().Subscribe(result =>
            {
                var sb = new StringBuilder($"Game Over!");
                if (result == 0)
                    sb.Append("Draw");
                else
                    sb.Append($"Win Fraction # {result} !");
                _view.SetActive(true);
                _text.text = sb.ToString();
                Time.timeScale = 0;
            });
        }
        

    }
}