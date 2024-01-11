using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blink : MonoBehaviour
{
    private float buffer = 0f;
    private bool active = true;
    private TextMeshProUGUI text;
    [SerializeField] bool isInverted;

    // Start is called before the first frame update
    void Start()
    {
        if (isInverted) {
            active = false;
        }
        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        text.enabled = active;
    }

    // Update is called once per frame
    void Update()
    {
        buffer += Time.deltaTime;
        if (active == true && buffer >= 1.0f) {
            buffer = 0.0f;
            active = !active;
            text.enabled = active;
        }
        if (active == false && buffer >= 0.3f)
        {
            buffer = 0.0f;
            active = !active;
            text.enabled = active;
        }
    }
}
