using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class noteMove : MonoBehaviour
{

    private Scrollbar scrollbar;
    [SerializeField] private float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        // the user is able to scroll the long notes 
        float scroll = Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        scrollbar.value += scroll;
        if (scrollbar.value > 1) {
            scrollbar.value = 1;
        }
        if (scrollbar.value < 0) {
            scrollbar.value = 0;
        }
    }
}
