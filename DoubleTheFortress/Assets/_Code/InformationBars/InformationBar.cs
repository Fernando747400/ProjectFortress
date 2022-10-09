using UnityEngine;
using UnityEngine.UI;

public class InformationBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;

    public void UpdateBar(float currentValue, float maxValue)
    {
        _slider.value = currentValue / maxValue;
        ChangeColor();
    }

    public void ChangeColor()
    {
        _fillImage.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
