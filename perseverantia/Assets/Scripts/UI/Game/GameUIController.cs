using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Tower Tower1;
    [SerializeField] private Tower Tower2;
    [SerializeField] private Tower Tower3;
    [SerializeField] private GameObject PauseMenu;
    
    private LevelController _levelController;
    private LevelSettings _levelSettings;
    
    private UIDocument _doc;
    private Label _sanityValue;
    private Label _happinessValue;
    private Label _roundValue;
    
    private Button _playRoundButton;
    
    private Button _tower1Button;
    private Button _tower2Button;
    private Button _tower3Button;
    
    private Label _tower1Price;
    private Label _tower2Price;
    private Label _tower3Price;

    private Waypoint[] _waypoints;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _levelSettings = FindObjectOfType<LevelSettings>();
        
        _waypoints = FindObjectsOfType<Waypoint>();
        
        _doc = GetComponent<UIDocument>();

        _sanityValue = _doc.rootVisualElement.Q<Label>("SanityValue");
        _happinessValue = _doc.rootVisualElement.Q<Label>("HappinessValue");
        _playRoundButton = _doc.rootVisualElement.Q<Button>("StartRound");
        _roundValue = _doc.rootVisualElement.Q<Label>("CurrentRound");

        _sanityValue.text = string.Concat(_levelSettings.StartingSanity.ToString(), " %");
        _happinessValue.text = _levelSettings.StartingHappiness.ToString();
        
        _tower1Button = _doc.rootVisualElement.Q<Button>("SmallLamp");
        _tower2Button = _doc.rootVisualElement.Q<Button>("Lamp");
        _tower3Button = _doc.rootVisualElement.Q<Button>("Obelisk");
        
        _tower1Price = _doc.rootVisualElement.Q<Label>("SmallLampPrice");
        _tower2Price = _doc.rootVisualElement.Q<Label>("LampPrice");
        _tower3Price = _doc.rootVisualElement.Q<Label>("ObeliskPrice");

        SetTowerPrices();

        _playRoundButton.clicked += RunRound;
        
        _tower1Button.clicked += SetTower1;
        _tower2Button.clicked += SetTower2;
        _tower3Button.clicked += SetTower3;
    }

    private bool _isPauseMenuShown = false;

    public void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            PauseMenu.SetActive(!_isPauseMenuShown);
            _isPauseMenuShown = !_isPauseMenuShown;
        }
    }

    public void UpdateSanity(int sanityValue)
    {
        _sanityValue.text = string.Concat(sanityValue.ToString(), " %");
    }

    public void UpdateHappiness(int happinessValue)
    {
        _happinessValue.text = happinessValue.ToString();
    }

    public void UpdateRound(int currentRoundValue, int maxRoundValue)
    {
        _roundValue.text = string.Concat("Round " , currentRoundValue.ToString(), "/", maxRoundValue.ToString());
    }
    
    private void RunRound()
    {
        Debug.Log("Start round");
        _levelController.RunRound();
        _playRoundButton.visible = false;
    }

    public void PrepareBreak()
    {
        _playRoundButton.visible = true;
    }

    private void SetTowerPrices()
    {
        _tower1Price.text = _levelSettings.Tower1Price.ToString();
        _tower2Price.text = _levelSettings.Tower2Price.ToString();
        _tower3Price.text = _levelSettings.Tower3Price.ToString();
    }

    private void SetTower1()
    {
        foreach (var obj in _waypoints)
        {
            obj.tower = Tower1;
        }
    }
    
    private void SetTower2()
    {
        foreach (var obj in _waypoints)
        {
            obj.tower = Tower2;
        }
    }
    
    private void SetTower3()
    {
        foreach (var obj in _waypoints)
        {
            obj.tower = Tower3;
        }
    }

}
