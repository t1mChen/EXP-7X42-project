using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class noteDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tipscanvas;
    [SerializeField] private GameObject content;
    private bool hintDisplay = false;
    private bool onDisplay = false;
    [SerializeField]  private int storyId;
    private bool buffGiven = false;
    private GameObject notesManagerObj;

    // Start is called before the first frame update
    void Start()
    {
        tipscanvas.GetComponent<Canvas>().enabled = false;
        canvas.GetComponent<Canvas>().enabled = false;
        notesManagerObj = GameObject.Find("NotesManager");
    }
    // Update is called once per frame
    void Update()
    {
        // display the notes
        if (Input.GetKeyDown(KeyCode.E) && hintDisplay == true) {
            canvas.GetComponent<Canvas>().enabled = true;
            onDisplay = true;
            content.GetComponent<TextMeshProUGUI>().text =
                notesManagerObj.GetComponent<notesManager>().getNote(storyId);
            tipscanvas.GetComponent<Canvas>().enabled = false;
        }

        // destory notes after being collectec
        if (onDisplay && Input.GetKeyDown(KeyCode.F)) {
            if (!buffGiven) {
                notesManagerObj.GetComponent<notesManager>().executeBuff();
                buffGiven = true;
                Destroy(gameObject);
            }
            onDisplay=false;
            canvas.GetComponent<Canvas>().enabled = false;
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        // display note hint
        if (other.gameObject.tag == "Player"&&!onDisplay) {
            hintDisplay = true;
            tipscanvas.GetComponent<Canvas>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // undisplay note hint
        if (other.gameObject.tag == "Player")
        {
            hintDisplay = false;
            tipscanvas.GetComponent<Canvas>().enabled = false;
        }
    }

}
