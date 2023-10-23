using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameMenuManager : MonoBehaviour
{
    public GameObject inGameMenu;
    public GameObject _player;
    private InputManager _inputManager;
    private GameObject pauseMenu;
    private GameObject resumeButton;
    private GameObject settings;
    private GameObject controls;
    private GameObject mainMenu;
    private bool _isMenuOpen;
    private bool _isInSettings;
    private Slider _slider;


    private void Start()
    {
        _isMenuOpen = false;
        inGameMenu.SetActive(true);
        _inputManager = _player.GetComponent<InputManager>();
        pauseMenu = inGameMenu.transform.GetChild(0).GetChild(0).gameObject;
        resumeButton = pauseMenu.transform.GetChild(1).gameObject;
        resumeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            pauseMenu.SetActive(false);
            _inputManager.DisplayMenu = false;
            
        });
        settings = pauseMenu.transform.GetChild(2).gameObject;
        settings.GetComponent<Button>().onClick.AddListener(() =>
        {
            pauseMenu.gameObject.SetActive(false);
            _isInSettings = true;
            inGameMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            controls = inGameMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            _slider = controls.transform.GetChild(8).GetComponent<Slider>();
            _slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
            _slider.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                System.MathF.Round(_slider.value, 2).ToString("F2");

            _slider.onValueChanged
                .AddListener(OnSensitivityChange); // add listener to slider to change mouse sensitivity

            controls.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() =>// back button to pause menu
            {
                controls.transform.parent.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(true);
                _isInSettings = false;
                PlayerPrefs.Save();
            });
        });
        mainMenu = pauseMenu.transform.GetChild(3).gameObject;
        mainMenu.GetComponent<Button>().onClick.AddListener(() =>// main menu button
        {
            Player player = _player.GetComponent<Player>();
            SaveManager.SavePlayer(player);
            if (GetComponent<MazeGenerator>() != null) SaveManager.SaveMaze(GetComponent<MazeGenerator>());
            SceneManager.LoadScene(0);
        });
    }

    private void OnSensitivityChange(float value)
    {
        _slider.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            System.MathF.Round(value, 2).ToString("F2");
        _player.GetComponent<PlayerController>().ChangeMouseSensitivity(value);
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputManager.DisplayMenu)
        {
            Time.timeScale = 0; // stop time
            inGameMenu.SetActive(true);
            if (_isInSettings == false) pauseMenu.SetActive(true); // activate pause menu if not in settings menu
        }
        else
        {
            inGameMenu.gameObject.gameObject.SetActive(false); // deactivate menu
            Time.timeScale = 1; // resume time
        }
    }
}