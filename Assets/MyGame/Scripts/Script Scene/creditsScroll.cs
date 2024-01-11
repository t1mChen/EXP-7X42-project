using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class creditsScroll : MonoBehaviour
{

    [SerializeField] private GameObject scrollBar;
    private bool scrolling = false;
    [SerializeField] UnityEvent onFinish;
    // Start is called before the first frame update
    void Start()
    {
        scrollBar.GetComponent<Scrollbar>().value = 1f;
        scrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        // scroll the text in credits scene
        if (scrolling && scrollBar.GetComponent<Scrollbar>().value > 0f)
        {
            scrollBar.GetComponent<Scrollbar>().value = scrollBar.GetComponent<Scrollbar>().value - Time.deltaTime * 0.02f;
        }
        else {
            onFinish.Invoke();
        }
    }
}
