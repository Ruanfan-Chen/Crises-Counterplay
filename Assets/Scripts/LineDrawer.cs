using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer(float lineSize = 0.2f)
    {
        GameObject lineObj = new GameObject("LineObj");
        lineObj.tag = "Disposable";
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    private void init(float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineObj.tag = "Disposable";
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the position of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public static List<LineDrawer> DrawCircleInGameView(Vector3 center, float radius, Color color)
    {
        int interpolationDensity = 12;
        List<LineDrawer> drawers = new List<LineDrawer>();
        for (int i = 0; i < interpolationDensity; i++)
        {
            Vector3 startPos = center + Quaternion.Euler(0.0f, 0.0f, 360 * i / interpolationDensity) * Vector3.up * radius;
            Vector3 endPos = center + Quaternion.Euler(0.0f, 0.0f, 360 * (i + 1) / interpolationDensity) * Vector3.up * radius;
            LineDrawer lineDrawer = new();
            lineDrawer.DrawLineInGameView(startPos, endPos, color);
            drawers.Add(lineDrawer);
        }
        return drawers;
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }

    public void Destroy(float delay)
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject, delay);
        }
    }
}