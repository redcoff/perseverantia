using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private LevelController _levelController;
    
    private UIDocument _doc;
    private Label _sanityValue;
    private Label _happinessValue;
    private Button _playRoundButton;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _doc = GetComponent<UIDocument>();

        _sanityValue = _doc.rootVisualElement.Q<Label>("SanityValue");
        _happinessValue = _doc.rootVisualElement.Q<Label>("HappinessValue");
        _playRoundButton = _doc.rootVisualElement.Q<Button>("StartRound");

        Debug.Log(_playRoundButton.name);
        _playRoundButton.clicked += RunRound;
    }

    public void UpdateSanity(int sanityValue)
    {
        _sanityValue.text = string.Concat(sanityValue.ToString(), " %");
    }

    public void UpdateHappiness(int happinessValue)
    {
        _happinessValue.text = happinessValue.ToString();
    }

    private void RunRound()
    {
        Debug.Log("Start round");
        _levelController.RunRound();
        _playRoundButton.focusable = false;
    }

}
