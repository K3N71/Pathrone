using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    private bool isNegative = false;
    [SerializeField] private float valueIncrement = 0;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0.00");
        });
        valueIncrement = ChangeIncrement();
    }

    // Update is called once per frame
    void Update()
    {

        // increment the value
        UpdateValue(valueIncrement);

        // update the colors
        ChangeColor(slider.value);

        // Last thing per update, check if the value is negative and update accordingly
        if(slider.value < 0)
        {
            isNegative = true;
        }
        else
        {
            isNegative = false;
        }
    }
    public float ReturnValue()
    {
        return slider.value;
    }
    private void UpdateValue(float increment)
    {
        // if you hit an edge, switch directions
        if (slider.value >= 1f || slider.value <= -1f)
        {
            valueIncrement = valueIncrement * -1f;
            if(slider.value <= -1f)
            {
                slider.value += 0.01f;
                valueIncrement = ChangeIncrement();
            }
            else if(slider.value >= 1f)
            {
                slider.value -= 0.01f;
                valueIncrement = (ChangeIncrement() * -1f);
            }
            
        }
        slider.value += increment;
    }
    private void ChangeColor(float value)
    {
        if (value >= -0.15f && value <= 0.15f)
        {
            text.color = Color.green;
        }
        else if (value >= -0.25f && value <= 0.25f)
        {
            text.color = Color.yellow;
        }
        else if (value >= -0.5f && value <= 0.5f)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.white;
        }
    }
    private float ChangeIncrement()
    {
        return Random.Range(0.001f, 0.003f);
    }
}
