using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float particle_liftTime; // 粒子持续时间
    public float particle_radius; // 粒子半径
    public float force; // 初始力
    
    // 定义颜色
    private Color[] particle_color = new Color[]
    {
        Color.red,        // 红色
        Color.yellow,     // 黄色
        Color.green,      // 绿色
        Color.cyan,       // 青色
        Color.blue,       // 蓝色
        Color.magenta     // 品红色
    };
    // Start is called before the first frame update
    void Start()
    {
        particle_liftTime = 1f;
        particle_radius = 2f;

        // 颜色
        Color particle_color_ = particle_color[Random.Range(0,6)];
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = particle_color_;

        // 给予力
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.Range(-1,1), 1).normalized;
        rig.AddForce(force*direction, ForceMode2D.Impulse);

        // 销毁
        Destroy(gameObject, particle_liftTime);

        StartCoroutine(ScaleDown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 缩小
    private System.Collections.IEnumerator ScaleDown() {
        Vector3 startScale = transform.localScale; // 初始缩放
        Vector3 endScale = Vector3.zero;           // 目标缩放
        float elapsedTime = 0f;                    // 经过的时间
 
        while (elapsedTime < particle_liftTime)
        {
            elapsedTime += Time.deltaTime; // 增加经过的时间
            float t = elapsedTime / particle_liftTime; // 计算归一化时间（0 到 1）
 
            // 使用 Lerp 平滑插值
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
 
            yield return null; // 等待下一帧
        }
 
        // 确保最终缩放精确为 (0, 0, 0)
        transform.localScale = endScale;
    }
}
