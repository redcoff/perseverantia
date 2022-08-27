using UnityEngine;

namespace Player
{
    public class Sanity : MonoBehaviour
    {
        private LevelSettings _levelSettings;
        
        private int currentSanity = 0;
        private GameUIController _gameUIController; 
        
        public int CurrentSanity => currentSanity;
        
        private void Awake()
        {
            _levelSettings = FindObjectOfType<LevelSettings>();
            currentSanity = _levelSettings.StartingSanity;
            _gameUIController = FindObjectOfType<GameUIController>(); 
        }

        public void TakeSanity(int value)
        {
            currentSanity -= value;
            
            if (currentSanity < 0)
            {
                currentSanity = 0;
                _gameUIController.GameOver();
            }
            
            _gameUIController.UpdateSanity(CurrentSanity);
        }
    }
}