using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        int x = Mathf.FloorToInt((transform.position.x + 65) / transform.localScale.x);
        int y = Mathf.FloorToInt((transform.position.y + 65) / transform.localScale.y);
        if (GameManager.instance.currentState == GameState.SCAN && GameManager.instance.playerScan > 0)
        {
            GameManager.instance.OnScanMineral(x, y);
        }
        if (GameManager.instance.currentState == GameState.COLLECT && GameManager.instance.playerCollect > 0)
        {
            GameManager.instance.OnCollectMineral(x, y);
        }
    }
}
