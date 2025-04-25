using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // read file
using System.Text.RegularExpressions; // regex
using UnityEngine.UI; // text

public class global : MonoBehaviour
{

    public Transform target; // 要跟随的目标（通常是玩家角色）
    public float smoothSpeed = 0.125f; // 相机移动的平滑速度

    public static string[] eng; // 英文
    public static string[] chi; // 中文
    public static int answer_i; // 答案的序列
    public GameObject answer_; // 问题的引用
    public GameObject answer_t; // 答案的引用
    public bool isBirth; // 是否进入协程

    public Text chiText;
    void Start()
    {   
        answer_i = 0;
        isBirth = true;
        chiText = GetComponentInChildren<Text>(); // 将要浮现的文字

        // 导入词源 Application.dataPath
        // string filePath = Application.dataPath + "/out.txt";
        // TextAsset filePath = Resources.Load<TextAsset>("out");
        // Debug.Log(filePath);
        // if (File.Exists(filePath)) {
        //     string[] lines = File.ReadAllLines(filePath);
        //     eng = new string[lines.Length];
        //     chi = new string[lines.Length];
        //     for (int i = 0; i < lines.Length; i++)
        //     {
        //         string text = lines[i];
        //         string pattern_e = @"[a-z]+"; // match eng
        //         string pattern_c = @"[\u4e00-\u9fbb]+"; // match chi
        //         Match match_c = Regex.Match(lines[i], pattern_c);
        //         Match match_e = Regex.Match(lines[i], pattern_e);
        //         eng[i] = match_e.Success ? match_e.Value : string.Empty;
        //         chi[i] = match_c.Success ? match_c.Value : string.Empty;
        //     }
        // }

        TextAsset textAsset = Resources.Load<TextAsset>("out");
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n'); // 使用换行符分割文本为行
            eng = new string[lines.Length];
            chi = new string[lines.Length];
 
            for (int i = 0; i < lines.Length; i++)
            {
                string text = lines[i].Trim(); // 去除行首尾的空白字符
                string pattern_e = @"[a-z]+"; // 匹配英文
                string pattern_c = @"[\u4e00-\u9fff]+"; // 匹配中文
 
                Match match_c = Regex.Match(text, pattern_c);
                Match match_e = Regex.Match(text, pattern_e);
 
                eng[i] = match_e.Success ? match_e.Value : string.Empty;
                chi[i] = match_c.Success ? match_c.Value : string.Empty;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        // 相机跟随运动
        Vector3 desiredPosition = target.position + new Vector3(0,0,-10f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (isBirth == true) {
            StartCoroutine(birth());
        }
    }

    // 如果进入下一次选择, 更新答案并创建answer
    private System.Collections.IEnumerator birth() {
        
        if (move.next_scene) {
            // 产生新实例
            isBirth = false;
            yield return new WaitForSeconds(1.2f);
            answer_i = Random.Range(0, chi.Length);
            StartCoroutine(text_chi());
            move.next_scene = false;
            //  这个是答案
            Instantiate(answer_t, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);

            // 可能包含答案
            for (int i=0; i<Random.Range(1, 4); i++) {
                float x = Random.Range(-8, 8) + target.position.x + Random.Range(-10, 10);
                float y = Random.Range(-5, 5)+ target.position.y + Random.Range(-10, 10);
                Instantiate(answer_, new Vector3(x, y, 0), Quaternion.identity);
            }
            
        }
    }

    // 调整文本透明度
    private System.Collections.IEnumerator text_chi() {
        chiText.text = chi[answer_i];
        float elapsedTime = 0f;                    // 经过的时间
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime; // 增加经过的时间
            Color currentColor = chiText.color;
            
            // 修改 alpha 值，例如设置为 0.5f（50% 透明）
            currentColor.a = Mathf.Lerp(1, 0, elapsedTime);
            // 将修改后的颜色重新赋值给 Text 组件
            chiText.color = currentColor;

            yield return null; // 等待下一帧
        }
        isBirth = true;
    }

}
