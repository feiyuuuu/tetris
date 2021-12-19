using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Net.Http.Headers;

public class Spawner : MonoBehaviour
{
    public GameObject BlockPrefab;          
    public Transform nextFallGroup;
    public Transform currentFallGroup;
    private Vector2 showPos;
    public Vector2 spawnPos;

    private int styleIndex;//方块组合序号
    private int flipIndex;//方块翻转序号

    private void Start()
    {
        showPos = nextFallGroup.position;
        StartCoroutine(SpawnBlockGroup());
        StartNextFallGroup();
    }

    public void StartNextFallGroup()
    {
        StartCoroutine(StartNextFall());
    }

    IEnumerator StartNextFall()
    {
        //先让当前准备好的块组合开始下落
        yield return StartCoroutine(ToFallBlockGroup());
        //然后再生成新的块组合来准备下一回合
        StartCoroutine(SpawnBlockGroup());
    }

    //生成方块组合
    IEnumerator SpawnBlockGroup()
    {
        byte[] randomBytes = new byte[4];
        RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
        rngCrypto.GetBytes(randomBytes);
        int num = BitConverter.ToInt32(randomBytes, 0);//生成随机数
        flipIndex = Mathf.Abs(num) % 100;
        styleIndex = Mathf.Abs(num) % 5;

        //重置x轴翻转
        FlipTransformX(nextFallGroup, 1);

        if (styleIndex == 0)
        {
            nextFallGroup.position = showPos + new Vector2(0.5f, 0.5f);
            SpawnSquareBlockGroup();
        }
        if (styleIndex == 1)
        {
            nextFallGroup.position = showPos + new Vector2(1, 0);
            SpawnLineBlockGroup();
        }
        if (styleIndex == 2)
        {
            nextFallGroup.position = showPos + new Vector2(1, 0);
            SpawnTStyleBlockGroup();

        }
        if (styleIndex == 3)
        {

            nextFallGroup.position = showPos + new Vector2(1, 0);
            SpawnLStyleBlockGroup();

            if (flipIndex > 50)
            {
                FlipTransformX(nextFallGroup,-1);
            }
        }
        if (styleIndex == 4)
        {
            nextFallGroup.position = showPos + new Vector2(1, 0);
            SpawnSStyleBlockGroup();

            if (flipIndex > 50)
            {
                FlipTransformX(nextFallGroup,-1);
            }
        }

        yield return null;
    }

    //在组合下降前，处理一些必要的事务
    IEnumerator ToFallBlockGroup()
    {
        yield return new WaitForSeconds(0.4f);

        //重置x轴翻转
        FlipTransformX(currentFallGroup, 1);

        if (styleIndex == 0)
        {

            Transform chilid = nextFallGroup.GetChild(0);
            chilid.position = spawnPos;

            chilid = nextFallGroup.GetChild(1);
            chilid.position = spawnPos + new Vector2(1, 0);

            chilid = nextFallGroup.GetChild(2);
            chilid.position = spawnPos + new Vector2(0, 1);

            chilid = nextFallGroup.GetChild(3);
            chilid.position = spawnPos + new Vector2(1, 1);

            currentFallGroup.position = spawnPos + new Vector2(0.5f, 0.5f);
            //改变块组合的父物体到下降父物体
            ChangeParent(nextFallGroup, currentFallGroup);
        }
        if (styleIndex == 1)
        {

            Transform chilid = nextFallGroup.GetChild(0);
            chilid.position = spawnPos;

            chilid = nextFallGroup.GetChild(1);
            chilid.position = spawnPos + new Vector2(1, 0);

            chilid = nextFallGroup.GetChild(2);
            chilid.position = spawnPos + new Vector2(2, 0);

            chilid = nextFallGroup.GetChild(3);
            chilid.position = spawnPos + new Vector2(3, 0);

            currentFallGroup.position = spawnPos + new Vector2(1, 0);
            ChangeParent(nextFallGroup, currentFallGroup);
        }
        if (styleIndex == 2)
        {
            Transform chilid = nextFallGroup.GetChild(0);
            chilid.position = spawnPos;

            chilid = nextFallGroup.GetChild(1);
            chilid.position = spawnPos + new Vector2(1, 0);

            chilid = nextFallGroup.GetChild(2);
            chilid.position = spawnPos + new Vector2(2, 0);

            chilid = nextFallGroup.GetChild(3);
            chilid.position = spawnPos + new Vector2(1, 1);

            currentFallGroup.position = spawnPos + new Vector2(1, 0);
            ChangeParent(nextFallGroup, currentFallGroup);

        }
        if (styleIndex == 3)
        {
            Transform chilid = nextFallGroup.GetChild(0);
            chilid.position = spawnPos;

            chilid = nextFallGroup.GetChild(1);
            chilid.position = spawnPos + new Vector2(1, 0);

            chilid = nextFallGroup.GetChild(2);
            chilid.position = spawnPos + new Vector2(2, 0);

            chilid = nextFallGroup.GetChild(3);
            chilid.position = spawnPos + new Vector2(2, 1);

            currentFallGroup.position = spawnPos + new Vector2(1, 0);
            ChangeParent(nextFallGroup, currentFallGroup);

            if (flipIndex > 50)
            {
                FlipTransformX(currentFallGroup, -1);
            }
        }
        if (styleIndex == 4)
        {
            Transform chilid = nextFallGroup.GetChild(0);
            chilid.position = spawnPos;

            chilid = nextFallGroup.GetChild(1);
            chilid.position = spawnPos + new Vector2(1, 0);

            chilid = nextFallGroup.GetChild(2);
            chilid.position = spawnPos + new Vector2(1, 1);

            chilid = nextFallGroup.GetChild(3);
            chilid.position = spawnPos + new Vector2(2, 1);

            currentFallGroup.position = spawnPos + new Vector2(1, 0);
            ChangeParent(nextFallGroup, currentFallGroup);

            if (flipIndex > 50)
            {
                FlipTransformX(currentFallGroup, -1);
            }
        }
    }

    private void ChangeParent(Transform from,Transform to)
    {
        for(int i=0;i<4;i++)
        {
            Transform child = from.GetChild(0);
            child.SetParent(to);
        }
    }
    private void FlipTransformX(Transform t,int i)
    {
        t.localScale = new Vector3(i, 1, 1);
    }
    //以下五个函数分别生成五个不同的方块组合
    private void SpawnSquareBlockGroup()
    {
        Instantiate(BlockPrefab, showPos, Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 0), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(0, 1), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 1), Quaternion.identity, nextFallGroup);
    }
    private void SpawnLineBlockGroup()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(BlockPrefab, showPos + new Vector2(i, 0), Quaternion.identity, nextFallGroup);
        }
    }
    private void SpawnTStyleBlockGroup()
    {
        Instantiate(BlockPrefab, showPos, Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 0), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(2, 0), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 1), Quaternion.identity, nextFallGroup);
    }
    private void SpawnLStyleBlockGroup()
    {
        Instantiate(BlockPrefab, showPos, Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 0), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(2, 0), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(2, 1), Quaternion.identity, nextFallGroup);
    }
    private void SpawnSStyleBlockGroup()
    {
        Instantiate(BlockPrefab, showPos, Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 0), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(1, 1), Quaternion.identity, nextFallGroup);
        Instantiate(BlockPrefab, showPos + new Vector2(2, 1), Quaternion.identity, nextFallGroup);
    }
}