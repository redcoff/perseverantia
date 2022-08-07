using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument _doc;
    private VisualElement _buttonsWrapper;
    private Button _playButton;
    private Button _settingsButton;
    private Button _exitButton;
    private Button _muteButton;

    [Header("MuteButton")]
    [SerializeField] private Sprite _mutedSprite;

    [SerializeField] private Sprite _unmutedSprite;
    private bool _muted;

    [SerializeField] private VisualTreeAsset _settingsButtonsTemplate;
    private VisualElement _settingsButtons;
    
    
    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _playButton = _doc.rootVisualElement.Q<Button>("PlayButton");
        _settingsButton = _doc.rootVisualElement.Q<Button>("SettingsButton");
        _exitButton = _doc.rootVisualElement.Q<Button>("ExitButton");
        _muteButton = _doc.rootVisualElement.Q<Button>("MuteButton");

        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");
        _settingsButtons = _settingsButtonsTemplate.CloneTree();

        _playButton.clicked += PlayButtonOnClicked;
        _exitButton.clicked += ExitButtonOnClicked;
        _muteButton.clicked += MuteButtonOnClicked;
        _settingsButton.clicked += SettingsButtonOnClicked;

        var backButton = _settingsButtons.Q<Button>("BackButton");
        backButton.clicked += BackButtonOnClicked;
    }

    private void PlayButtonOnClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void ExitButtonOnClicked()
    {
        Application.Quit();
    }

    private void MuteButtonOnClicked()
    {
        _muted = !_muted;
        var image = _muteButton.style.backgroundImage;
        image.value = Background.FromSprite(_muted ? _mutedSprite : _unmutedSprite);
        _muteButton.style.backgroundImage = image;

        AudioListener.volume = _muted ? 0 : 1;
    }
    
    private void SettingsButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_settingsButtons);
    }

    private void BackButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_playButton);
        _buttonsWrapper.Add(_settingsButton);
        _buttonsWrapper.Add(_exitButton);
    }
    
}
