using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrid : MonoBehaviour
{
    public Transform currentFallGroup;
    public Transform[,] grid = new Transform[20, 10]; //20*10=200个格子
    public float destoryIntervalTime;  //消除格子的间隔时间

    public void SetGrid()
    {
        int childCount = currentFallGroup.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = currentFallGroup.GetChild(0);
            if (child == null) return;
            SetOneGrid(child);
            child.SetParent(transform);
        }
    }

    private void SetOneGrid(Transform block)
    {
        Vector3 pos = block.position;
        int x = (int)(pos.x + 0.1);
        int y = (int)(pos.y + 0.1);

        if(y>19)
        {
            GameManager.Instance.GameOver();
            return;
        }
        grid[y, x] = block;
    }

    public void ToUpdateGrid()
    {
        StartCoroutine(UpdateGrid());
    }

    IEnumerator UpdateGrid()
    {
        for (int i = 0; i < 20; i++)
        {
            int j = 0;
            for (; j < 10; j++)
            {
                if (grid[i, j] == null)
                {
                    break;
                }
            }

            //如果存在一行全是格子
            if (j == 10)
            {
                GameManager.Instance.score+=10;
                currentFallGroup.GetComponent<CurrentFallGroup>().fallTimeInterval *= 0.99f;
                GameManager.Instance.audioSource.PlayOneShot(GameManager.Instance.audioClip_DestoryOneRow);

                //播放格子消失动画
                for (int u = 0; u < 15; u++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Transform block = grid[i, k];
                        if (block == null)
                        {
                        }
                        SpriteRenderer sr = block.gameObject.GetComponent<SpriteRenderer>();
                        sr.color = new Vector4(1, 1, 1, Mathf.Abs(u - 7) * 0.1f + 0.3f);
                    }
                    yield return new WaitForSeconds(destoryIntervalTime);
                }

                yield return null;


                //销毁该行
                for (int v = 0; v < 10; v++)
                {
                    Transform block = grid[i, v];
                    grid[i, v] = null;
                    Destroy(block.gameObject);
                }

                //更新格子
                for (int k = i; k < 19; k++)
                {
                    for (int u = 0; u < 10; u++)
                    {
                        grid[k, u] = grid[k + 1, u];

                        if (grid[k, u] != null)
                        {
                            //注意 grid[k,u] 的位置是 (u,k);
                            grid[k, u].position = new Vector3(u, k, 0);
                        }
                    }
                }

                //重新计算
                i--;
                yield return null;
            }
        }
    }


}
