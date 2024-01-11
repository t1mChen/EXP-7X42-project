using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

// The following content is modified from a video tutorial
// original link is：
// https://www.bilibili.com/video/BV1Wr4y147nD/?spm_id_from=333.337.search-card.all.click
// edited by Tianxi Chen

public enum effect {
    typewriter =0,
}

public class gameWordDisplay : MonoBehaviour
{
    public UnityEvent onFinish;

    public TMP_Text message;
    [SerializeField] private float speed;

    private void Awake()
    {
        gameObject.TryGetComponent<TMP_Text>(out message);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Type());
    }

    private IEnumerator Type() {
        // get the text component and update each character one by one
        message.ForceMeshUpdate();
        TMP_TextInfo info = message.textInfo;
        int numChar = info.characterCount;
        bool done = false;
        int i = 0;
        while (!done) {
            if (i > numChar) {
                i = numChar;
                yield return new WaitForSecondsRealtime(1);
                done = true;
            }
            message.maxVisibleCharacters = i;
            i++;
            // controlling time output each character
            yield return new WaitForSecondsRealtime(speed);
        }
        onFinish.Invoke();
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
