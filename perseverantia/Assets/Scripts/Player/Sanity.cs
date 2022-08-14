using UnityEngine;

namespace Player
{
    public class Sanity : MonoBehaviour
    {
        [SerializeField] private int startingSanity = 100;
        
        private int currentSanity = 0;
        private GameUIController _gameUIController; 
        
        public int CurrentSanity => currentSanity;
        
        private void Awake()
        {
            currentSanity = startingSanity;
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