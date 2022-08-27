using UnityEngine;
using UnityEngine.UIElements;

namespace AlpaSunFade
{
	internal class TransitionPanel : MonoBehaviour
	{
		[Header("Properties")]
		[SerializeField] private Color transitionColor;
		[SerializeField] private PanelSettings settings;

		private VisualElement _transitionPanel;

		private void OnEnable()
		{
			_transitionPanel = GetComponent<UIDocument>().rootVisualElement.Q("Container");

			_transitionPanel.style.opacity = 0;

			// Color is set manually to make sure the alpha value is always 1
			_transitionPanel.style.backgroundColor = new Color(transitionColor.r, transitionColor.g, transitionColor.b, 1);
		}

		/// <summary>
		/// Start the fade transition
		/// </summary>
		/// <param name="fadeToDark">If true, the panel will darken to the chosen color. If false, the panel will start at the color
		/// and fade away.</param>
		/// <param name="waitDuration">Duration in seconds to wait before the fade starts</param>
		/// <param name="fadeDuration">Duration in seconds of the fade</param>
		internal void StartTransition(bool fadeToDark, float waitDuration, float fadeDuration)
		{
			settings.sortingOrder = 1;
			if(fadeToDark)
			{
				_transitionPanel.style.opacity = 0;
				StartCoroutine(StaticCoroutines.LerpVisualElementOpacity(_transitionPanel, 0, 1, fadeDuration, waitDuration, settings));
			}
			else
			{
				_transitionPanel.style.opacity = 1;
				StartCoroutine(StaticCoroutines.LerpVisualElementOpacity(_transitionPanel, 1, 0, fadeDuration, waitDuration, settings));
			}
		}
	}
}