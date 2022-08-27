using System.Collections;
using AlpaSunFade;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;

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
    [SerializeField] private AudioSource _clickSound;
    private bool _muted;

    [SerializeField] private VisualTreeAsset _settingsButtonsTemplate;
    
    [SerializeField] private TransitionPanel _transitionPanel;
    [SerializeField] private AudioSource _music;
    private VisualElement _settingsButtons;
    

    private void Awake()
    {
        _music.DOFade(AudioListener.volume, 2);
        _transitionPanel.StartTransition(false, 1, 1);
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
        var soundsSlider = _settingsButtons.Q<Slider>("Sounds");
        
        backButton.clicked += BackButtonOnClicked;
 
        soundsSlider.RegisterValueChangedCallback(v =>
        {
            AudioListener.volume = v.newValue / 100;
        });
    }
    

    private void PlayButtonOnClicked()
    {
        _music.DOFade(0, 2);
        _transitionPanel.StartTransition(true, 0, 2);
        _clickSound.Play();

        StartCoroutine(Waiter());
        
        
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Tutorial");
    }

    private void ExitButtonOnClicked()
    {
        _clickSound.Play();
        Application.Quit();
    }

    private void MuteButtonOnClicked()
    {
        _clickSound.Play();
        _muted = !_muted;
        var image = _muteButton.style.backgroundImage;
        image.value = Background.FromSprite(_muted ? _mutedSprite : _unmutedSprite);
        _muteButton.style.backgroundImage = image;

        AudioListener.volume = _muted ? 0 : 1;
    }
    
    private void SettingsButtonOnClicked()
    {
        _clickSound.Play();
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_settingsButtons);
    }

    private void BackButtonOnClicked()
    {
        _clickSound.Play();
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_playButton);
        _buttonsWrapper.Add(_settingsButton);
        _buttonsWrapper.Add(_exitButton);
    }
    
}
