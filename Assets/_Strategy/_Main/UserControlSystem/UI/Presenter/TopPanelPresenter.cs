using System;
using _Strategy._Main.Abstractions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    public class TopPanelPresenter : MonoBehaviour
    {

        [SerializeField] private TMP_InputField _timerInput;
        [SerializeField] private Button _menuButton;
        [SerializeField] private GameObject _goMenu;

        
        [Inject]
        private void Init(ITimeModel timeModel)
        {
            timeModel.GameTime.Subscribe(seconds =>
            {
                var t = TimeSpan.FromSeconds(seconds);
                _timerInput.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
            });

            _menuButton.OnClickAsObservable()
                .Subscribe(_ => _goMenu.SetActive(true));
        }
        
        

    }
}