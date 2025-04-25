using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class direct : MonoBehaviour
{
    public Transform target; // 目标物体
    public Camera mainCamera; // 主摄像机
    public GameObject arrow_; // 箭头
    public int n; // 距离摄像机比例
    private Vector2 t_p, c_p, a_p;
    private GameObject arrow;
    public float ang; // angle test
    public float clip;
    public float x_dis;
    void Start()
    {
        clip = 1f;
        target = GetComponent<Transform>();
        mainCamera = Camera.main;
        n = 3;
    }

    void Update()
    {
        
        if (!IsTargetInView())
        {   
            if (arrow == null) {
                
                arrow = Instantiate(arrow_,Vector3.zero, Quaternion.identity);
                arrow.transform.SetParent(target, false);
            }
            
            
            // 计算位置(两点和*1/n)
            t_p = new Vector2(target.position.x, target.position.y);
            c_p = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);

            // 箭头方向
             // 计算方向向量
            Vector2 dirAB = (t_p - c_p).normalized;
 
            // 假设主对象（如果有的话）在两点连线上，或者你可以选择一个固定的比例点
            // 这里我们简单地选择在两点连线上移动箭头
            float t = Mathf.Clamp01(Time.time * clip * 0.01f); // 使用时间来控制箭头移动，0.01f 是调整速度用的因子
            Vector2 targetPosition = c_p + dirAB * Vector2.Distance(c_p, t_p) * t;

            // Vector2 currentPosition2D = new Vector2(arrow.transform.position.x, arrow.transform.position.y);
            // a_p = Vector2.Lerp(currentPosition2D, (t_p + c_p) / n, clip * Time.deltaTime);
            arrow.transform.position = new Vector3(targetPosition.x + x_dis, targetPosition.y, 0);

            // 箭头角度
            Vector2 direction = (t_p - (Vector2)arrow.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, -angle+ang); // Unity 中 Y 轴向上，所以可能需要 -angle
        }
        else {
            if (arrow != null) {
                Destroy(arrow);
                arrow = null;
            }
        }
    }
 
    bool IsTargetInView()
    {
        if (target == null || mainCamera == null)
            return false;
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(target.position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0; // z > 0 确保物体在摄像机前方
    }
 
}
