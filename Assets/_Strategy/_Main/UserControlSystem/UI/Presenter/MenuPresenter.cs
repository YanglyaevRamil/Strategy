using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    public sealed class MenuPresenter : MonoBehaviour
    {

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _exitButton;


        private void Start()
        {
            _backButton.OnClickAsObservable()
                .Subscribe(_ => gameObject.SetActive(false));

            _exitButton.OnClickAsObservable()
                .Subscribe(_ => QuitApplication());
        }


        private void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        
        
    }
}