using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTool : MonoBehaviour
{
    private static LineRenderer GetLineRenderer(Transform t)
    {
        LineRenderer lr = t.GetComponent<LineRenderer>();
        if(lr == null)
        {
            lr = t.gameObject.AddComponent<LineRenderer>();
        }
        lr.SetWidth(0.3f, 0.3f);
        lr.loop = true;
        lr.sortingOrder = 1;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = Color.red;
        return lr;
    }

    public static void DrawLine(Transform t, Vector3 start, Vector3 end)
    {
        LineRenderer lr = GetLineRenderer(t);
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    //围绕z轴绘制空心圆
    public static void DrawCircle(Transform t, Vector3 center, float radius)
    {
        LineRenderer lr = GetLineRenderer(t);
        int pointAmount = 1000;//点的数目，值越大曲线越平滑
        float eachAngle = 360f / pointAmount;

        lr.positionCount = pointAmount;

        for(int i = 0; i < pointAmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, eachAngle * i) * new Vector3(1f,0f,0f) * radius + center;
            lr.SetPosition(i, pos);
        }
    }

    //围绕z轴绘制空心扇形
    public static void DrawSector(Transform t, Vector3 center, float angle, float radius)
    {
        LineRenderer lr = GetLineRenderer(t);
        int pointAmount = 100;//点的数目，值越大曲线越平滑
        float eachAngle = angle / pointAmount;
        Vector3 forward = t.forward;

        lr.positionCount = pointAmount;
        lr.SetPosition(0, center);
        lr.SetPosition(pointAmount - 1, center);

        for (int i = 1; i < pointAmount-1; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, -angle / 2 + eachAngle * (i-1)) * forward * radius + center;
            lr.SetPosition(i, pos);
        }
    }
}
