using UnityEngine;
using UnityEngine.UI;

public class StamineBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxStamine(int stamine)
    {
        slider.maxValue = stamine;
        slider.value = stamine;
    }

    public void SetStamine(int stamine)
    {
        slider.value = stamine;
    }

}
