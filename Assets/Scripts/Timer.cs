using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public void SetTime(float currentTime)
    {
        slider.value = currentTime;

    }

    public void SetMaxTime(float time)
    {
        slider.maxValue = time;
        slider.value = time;

    }
    public void ChangeColorPrepare()
    {
        fillImage.color = Color.white;
    }

    public void ChangeColorAttack()
    {
        fillImage.color = Color.gray;
    }
    public void ChangeColorWeapon()
    {
        fillImage.color = Color.black;
    }
}
