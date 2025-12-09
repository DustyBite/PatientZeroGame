using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool checkFlagCorrect = false;

    Transform[,] coords = new Transform[yCord, xCord];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountAndSetSlots(this.transform);
        PickPoint();
        GetNeighborsTest(radOne, Color.green, 1);
        GetNeighborsTest(radTwo, Color.yellow, 2);
        GetNeighborsTest(radThree, Color.orange, 3);
    }

    // Update is called once per frame
    void Update()
    {
        checkSlot();
    }

    public Transform getRandomPos()
    {
        int x = Random.Range(0, xCord);
        int y = Random.Range(0, yCord);

        Transform slot = coords[y, x];

        return slot;   // now valid
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

    public void TagPoint(Transform point, Color taggedColor, int radi)
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
                assignSlotData(radi, point);
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

        TagPoint(child, Color.red, 0);

    }

    /*
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
    */

    public void GetNeighborsTest(int radius, Color taggedColor, int radi)
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
                    TagPoint(child, taggedColor, radi);

                    // Extra stuff here if needed
                }
            }
        }


        Transform origin = coords[yP, xP];
        TagPoint(origin, Color.red, 0);

    }

    public void assignSlotData(int radi, Transform point)
    {
        SpotScript spot = point.GetComponent<SpotScript>();
        if (radi == 1)
        {
            spot.radiOne = true;
        }
        else if (radi == 2)
        {
            spot.radiTwo = true;
        }
        else if(radi == 3)
        {
            spot.radiThree = true;
        }
    }



    public void checkSlot()
    {
        Transform slot = coords[yP, xP];

        Transform child = slot.Find("checkFlag");
        if (child != null)
        {
            checkFlagCorrect = true;
            Debug.Log("Flag found as child!");

            // Restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


}
