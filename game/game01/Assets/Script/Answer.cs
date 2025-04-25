using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text

public class Answer : MonoBehaviour
{
    public Text myText;
    

    void Start()
    {
        myText = GetComponentInChildren<Text>();
        int len = global.eng.Length; // 词源长度
        string eng_ = global.eng[Random.Range(0, len)];
        myText.text = eng_;
        

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
