using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private Button     _playBtn;
    [SerializeField] private Button     _exitBtn;

    private LevelManager _lm;

    private void Awake()
    {
        _playBtn.onClick.AddListener(PlayGame);
        _exitBtn.onClick.AddListener(ExitGame);

        _lm = GetComponent<LevelManager>();
    }

    private void PlayGame()
    {
        _gameUI.SetActive(true);
        _mainMenuUI.SetActive(false);

        _lm.LevelInit();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
