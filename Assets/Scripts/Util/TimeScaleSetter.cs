using UnityEngine.UI;
using UnityEngine;

public class TimeScaleSetter : MonoBehaviour
{
    [SerializeField]
    private Slider timeScaleSlider;

    public void SetTimeScale()
    {
        Time.timeScale = timeScaleSlider.value;
    }
}
