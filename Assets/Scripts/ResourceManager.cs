using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public int color = 0;
    public bool isCollectable = false;

    public Cube() { }
}
public class ResourceManager : MonoBehaviour
{
    public GameObject normalCube;

    [Header("Materials")]
    public Material noneMaterial;
    public Material goldMaterial;
    public Material sliverMaterial;
    public Material bronzeMaterial;

    [Header("Minigame Size")]
    public int row = 32;
    public int column = 32;

    [Header("Mineral Price")]
    public int bronzeVal = 500;
    public int sliverVal = 1000;
    public int goldenVal = 2000;

    public GameObject[][] cubeHolder;
    public Cube[,] cubeArray;

    // Start is called before the first frame update
    void Start()
    {
        cubeArray = new Cube[32,32];
        GameManager.instance.ScanMineralEvent += ScanResource;
        GameManager.instance.CollectMineralEvent += CollectResource;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEverything()
    {
        SpawnCube();
        SpawnSeedMineral();
    }
    void SpawnCube()
    {
    
        cubeHolder = new GameObject[row][];
        for (int i = 0 ; i < row ; i++)
        {
        
            cubeHolder[i] = new GameObject[column];
            for (int j = 0; j < column; j++)
            {
                cubeArray[i, j] = new Cube();
                cubeHolder[i][j] = (GameObject)Instantiate(normalCube, new Vector3(transform.position.x + i * normalCube.transform.localScale.x, transform.position.y + j * normalCube.transform.localScale.y, 0), Quaternion.identity);
  
            }
        }
    }
    void SpawnSeedMineral()
    {
        for (int i = 2; i < row - 2; i++)
        {
            for (int j = 2; j < column - 2; j++)
            {
                int seed = Random.Range(0, 100);
                if (seed > 97)
                {
                    cubeArray[i,j].color = 3;
                    SpawnBoundaryMineral(i, j);
                }
            }
        }
    }
    void SpawnBoundaryMineral(int x,int y)
    {
        for (int i = x - 2; i <= x + 2 ; i++)
        {
            for (int j = y - 2; j <= y + 2; j++)
            {
                if (i != x || j != y)
                {
                     cubeArray[i,j].color = 1;
                }
            }
        }
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i != x || j != y)
                {
                     cubeArray[i,j].color = 2;
                }
            }
        }
    }
    void ScanResource(int x,int y)
    {
        if (GameManager.instance.currentState == GameState.SCAN)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i >= 0 && i < row)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (j >= 0 && j < column)
                        {
                            switch (cubeArray[i,j].color)
                            {
                                case 1:
                                    cubeHolder[i][j].GetComponent<Renderer>().material = bronzeMaterial;
                                    cubeArray[i, j].isCollectable = true;
                                    break;
                                case 2:
                                    cubeHolder[i][j].GetComponent<Renderer>().material = sliverMaterial;
                                    cubeArray[i, j].isCollectable = true;
                                    break;
                                case 3:
                                    cubeHolder[i][j].GetComponent<Renderer>().material = goldMaterial;
                                    cubeArray[i, j].isCollectable = true;
                                    break;
                                default:
                                    cubeHolder[i][j].GetComponent<Renderer>().material = noneMaterial;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        GameManager.instance.UpdateLimit();
    }

    void CollectResource(int x,int y)
    {
        int total = 0;
        if (GameManager.instance.currentState == GameState.COLLECT)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i >= 0 && i < row)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (j >= 0 && j < column)
                        {
                            if (cubeArray[i,j].isCollectable)
                            {
                                switch (cubeArray[i, j].color)
                                {
                                    case 1:
                                        total += bronzeVal;
                                        cubeHolder[i][j].GetComponent<Renderer>().material = noneMaterial;
                                        cubeArray[i, j].color = 0;
                                        break;
                                    case 2:
                                        total += sliverVal;
                                        cubeHolder[i][j].GetComponent<Renderer>().material = bronzeMaterial;
                                        cubeArray[i, j].color = 1;
                                        break;
                                    case 3:
                                        total += goldenVal;
                                        cubeHolder[i][j].GetComponent<Renderer>().material = sliverMaterial;
                                        cubeArray[i, j].color = 2;
                                        break;
                                    default:
                                        cubeHolder[i][j].GetComponent<Renderer>().material = noneMaterial;
                                        cubeArray[i, j].color = 0;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            GameManager.instance.SetScore(total);
        }
        GameManager.instance.UpdateLimit();
    }
}
