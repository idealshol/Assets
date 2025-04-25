using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text

public class answer2 : MonoBehaviour
{
    public Text myText;
    public AudioSource pu; // 吃豆音效
    void Start()
    {
        if (global.eng != null) {
            myText = GetComponentInChildren<Text>();
            int len = global.answer_i;
            myText.text = global.eng[len];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (move.next_scene == true) Destroy(gameObject); // 如果结束一轮,强制销毁
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
