using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // scene
using UnityEngine.UI;

public class SceneConfig : MonoBehaviour
{
    public static SceneConfig Config; // 保留配置信息
    public float num; // 需要完成单次量
    public Slider slider;
    public Text text;
    void Start()
    {
        slider.value = 1;
        Config = this; // 
        DontDestroyOnLoad(gameObject);
        text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        num_conf();
        text.text = num.ToString();
    }

    // 切换场景 
    public void enter_game() {
        SceneManager.LoadScene(1);
    }
    public void over_game() {
        SceneManager.LoadScene(0);
    }
    public void quit_game() {
        // 退出游戏,判断是否在unity调试模式
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 退出播放器
        #else
            Application.Quit(); // 构建后的退出
        #endif
    }

    // 游戏内参数规定
    public void num_conf() {
        num = slider.value;
        if (num == 0) num = 1;
    }
}
