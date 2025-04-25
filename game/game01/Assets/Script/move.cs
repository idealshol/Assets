using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    public int speed; // 速度 4 6 8 10 15

    public GameObject particle; // 粒子
    public int particle_count; // 粒子数量
    public static bool next_scene; // 是否碰撞
    public AudioSource pu; // 吃豆音效
    private Vector3 startScale; // 初始缩放
    public Vector3 nowScale; // 每次缩放大小
    public GameObject gameOver; // 结束游戏框
    void Start()
    {
        startScale = transform.localScale;
        nowScale = startScale * (1/SceneConfig.Config.num);
        next_scene = false;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // 获得水平轴输入
        float y = Input.GetAxis("Vertical"); // 获得竖直方向输入
        Vector3 move_d = new Vector3(x, y, 0f); // 创建x,y,z分量
        if (x!=0 | y!=0) transform.Translate(move_d*speed *Time.deltaTime);

        if (Input.GetKey(KeyCode.R)) {
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(sceneIndex);
            }
        // #if UNITY_EDITOR 
        //     float x = Input.GetAxis("Horizontal"); // 获得水平轴输入
        //     float y = Input.GetAxis("Vertical"); // 获得竖直方向输入
        //     Vector3 move_d = new Vector3(x, y, 0f); // 创建x,y,z分量
        //     if (x!=0 | y!=0) transform.Translate(move_d*speed *Time.deltaTime);

        //     if (Input.GetKey(KeyCode.R)) {
        //         int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        //         SceneManager.LoadScene(sceneIndex);
        //     }
        // #else 
        //     Vector3 acceleration = Input.acceleration;
        //     float x = acceleration.x; // 左右移动
        //     float y = acceleration.y;    // 上下移动
        //     Vector2 move_d = new Vector2(x, y);
        //     if (x!=0 | y!=0) transform.Translate(move_d*speed *Time.deltaTime);
        // #endif

        
    }

    // 粒子效果
    private void rainBall() {
        particle_count = Random.Range(1,7); // 粒子数量
        Vector3 currentPosition = transform.position; // 当前位置

        for (int i=0; i<particle_count; i++) {
            // 生成粒子
            GameObject circle = Instantiate(particle, currentPosition, Quaternion.identity);
            circle.transform.localPosition = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
            ) + currentPosition;

        }
    }

    // 检测碰撞
    private void OnCollisionEnter2D(Collision2D collision) {
        next_scene = true;
        pu.Play();
        // 错误
        if (collision.gameObject.CompareTag("answer")) {
            // answer消失

        }

        // 正确
        if (collision.gameObject.CompareTag("answer1")) {
            rainBall();
            if (speed<15) speed++;


            // game over
            if (SceneConfig.Config.num<0) {
                
                Instantiate(gameOver,transform.position, Quaternion.identity);
                
                // // 退出游戏,判断是否在unity调试模式
                // #if UNITY_EDITOR
                //     UnityEditor.EditorApplication.isPlaying = false; // 退出播放器
                // #else
                //     Application.Quit(); // 构建后的退出
                // #endif
            }
            else {
                SceneConfig.Config.num -= 1;
                SetScaleRecursively(transform, startScale);
                startScale -= nowScale;
            }
        }
    }

    // 递归方法设置缩放
    void SetScaleRecursively(Transform parent, Vector3 scale)
    {
        // 设置当前对象的缩放
        parent.localScale = scale;
 
        // 遍历所有子对象并递归设置它们的缩放
        foreach (Transform child in parent)
        {
            SetScaleRecursively(child, scale);
        }
    }

}
