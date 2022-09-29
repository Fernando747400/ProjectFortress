using UnityEngine;
using UnityEngine.UI;

public class InformationBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;

    private void FixedUpdate()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        _fillImage.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
