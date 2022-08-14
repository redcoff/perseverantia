using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    private UIDocument _doc;
    private Label _sanityValue;
    private Label _happinessValue;

    [SerializeField] private GameObject gameOverObject;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();

        _sanityValue = _doc.rootVisualElement.Q<Label>("SanityValue");
        _happinessValue = _doc.rootVisualElement.Q<Label>("HappinessValue");
    }

    public void UpdateSanity(int sanityValue)
    {
        _sanityValue.text = string.Concat(sanityValue.ToString(), " %");
    }

    public void UpdateHappiness(int happinessValue)
    {
        _happinessValue.text = happinessValue.ToString();
    }

    public void GameOver()
    {
        gameOverObject.SetActive(true);
    }

}
