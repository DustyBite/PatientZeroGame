using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public static int xCord = 17;
    public static int yCord = 10;

    int xP = 0;
    int yP = 0;

    public int radOne = 0;
    public int radTwo = 0;
    public int radThree = 0;
    public int radFour = 0;

    Transform[,] coords = new Transform[yCord, xCord];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountAndSetSlots(this.transform);
        PickPoint();
        GetNeighborsTest(radOne, Color.green);
        GetNeighborsTest(radTwo, Color.yellow);
        GetNeighborsTest(radThree, Color.orange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountAndSetSlots(Transform parent)
    {
        int x = 0;
        int y = 0;
        Color TagColor = Color.green;
        foreach (Transform child in parent)
        {
            if (child.name.StartsWith("point"))
            {
                coords[y, x] = child;
                //Debug.Log(child);
                //TagPoint(child, TagColor);
            }
            y++;

            if (y >= yCord)
            {
                y = 0;
                x++;
            }
        }
    }

    public void TagPoint(Transform point, Color taggedColor)
    {
        // Find the cube child
        Transform cube = point.Find("Cube"); // assumes the child is named "Cube"
        if (cube != null)
        {
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Change the material color
                renderer.material.color = taggedColor;
            }
        }
        else
        {
            Debug.LogWarning("No Cube child found under " + point.name);
        }
    }

    public void PickPoint()
    {
        xP = Random.Range(0, xCord);
        yP = Random.Range(0, yCord);

        Transform child = coords[yP, xP];

        TagPoint(child, Color.red);

    }

    public void GetNeighborsNew(int radius, Color taggedColor)
    {
        int resolution = 100;
        int r = radius;

        for (int i = 0; i < resolution; i++)
        {
            float angle = i * 2f * Mathf.PI / resolution;

            int x = xP + Mathf.RoundToInt(Mathf.Cos(angle) * r);
            int y = yP + Mathf.RoundToInt(Mathf.Sin(angle) * r);

            if (!(x < 0 || x >= xCord || y < 0 || y >= yCord))
            {
                Transform child = coords[y, x];
                TagPoint(child, taggedColor);
            }

            //do your extra stuff here based on x and y
        }
    }

    public void GetNeighborsTest(int radius, Color taggedColor)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int x = xP + dx;
                int y = yP + dy;

                // Check bounds
                if (x < 0 || x >= xCord || y < 0 || y >= yCord)
                    continue;

                // Check if point is within circle radius
                if (dx * dx + dy * dy <= radius * radius)
                {
                    Transform child = coords[y, x];
                    TagPoint(child, taggedColor);

                    // Extra stuff here if needed
                }
            }
        }


        Transform origin = coords[yP, xP];
        TagPoint(origin, Color.red);

    }

}
