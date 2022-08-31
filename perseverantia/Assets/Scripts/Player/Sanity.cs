using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class Sanity : MonoBehaviour
    {
        private LevelSettings _levelSettings;
        private LevelController _levelController;
        
        private int currentSanity = 0;
        private GameUIController _gameUIController; 
        
        public int CurrentSanity => currentSanity;

        [SerializeField] private Camera _camera;
        
        private void Awake()
        {
            _levelSettings = FindObjectOfType<LevelSettings>();
            _levelController = FindObjectOfType<LevelController>();
            currentSanity = _levelSettings.StartingSanity;
            _gameUIController = FindObjectOfType<GameUIController>(); 
        }

        public void TakeSanity(int value)
        {
            currentSanity -= value;
            
            if (currentSanity <= 0)
            {
                currentSanity = 0;
                _levelController.GameOver();
            }
            
            _gameUIController.UpdateSanity(CurrentSanity);
            _camera.transform.DOShakePosition(1f, 2);
        }
    }
}