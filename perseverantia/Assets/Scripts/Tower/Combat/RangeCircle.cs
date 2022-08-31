    using UnityEngine;
    
    public class RangeCircle : MonoBehaviour
    {
        public enum Axis { X, Y, Z };
     
        [SerializeField]
        [Tooltip("The number of lines that will be used to draw the circle. The more lines, the more the circle will be \"flexible\".")]
        [Range(0, 1000)]
        private int _segments = 60;
        
        private float _horizRadius = 10;
        private float _vertRadius = 10;
     
        [SerializeField]
        [Tooltip("The offset will be applied in the direction of the axis.")]
        private float _offset = 0;
     
        [SerializeField]
        [Tooltip("The axis about which the circle is drawn.")]
        private Axis _axis = Axis.Z;

        private int _previousSegmentsValue;

        [SerializeField] private LineRenderer _line;
        [SerializeField] private TargetLocator _targetLocator;
     
        public void Draw()
        {
            _horizRadius = _targetLocator.Range;
            _vertRadius = _targetLocator.Range;
            
            _line.SetVertexCount(_segments + 1);
            _line.useWorldSpace = false;

            CreatePoints();
            _line.gameObject.SetActive(false);
        }

        public void EnableLine()
        {
            _line.gameObject.SetActive(true);
        }

        public void DisableLine()
        {
            _line.gameObject.SetActive(false);
        }

        void CreatePoints()
        {
     
            if (_previousSegmentsValue != _segments)
            {
                _line.SetVertexCount(_segments + 1);
            }
     
            float x;
            float y;
            float z = _offset;
     
            float angle = 0f;
     
            for (int i = 0; i < (_segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * _horizRadius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * _vertRadius;
     
                switch(_axis)
                {
                    case Axis.X: _line.SetPosition(i, new Vector3(z, y, x));
                        break;
                    case Axis.Y: _line.SetPosition(i, new Vector3(y, z, x));
                        break;
                    case Axis.Z: _line.SetPosition(i, new Vector3(x, y, z));
                        break;
                    default:
                        break;
                }
     
                angle += (360f / _segments);
            }
        }
    }
