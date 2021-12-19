using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CurrentFallGroup : MonoBehaviour
{
    [SerializeField]
    public float fallTimeInterval;
    private float fallTimer;

    [SerializeField]
    private float horizontalMoveTimeInterval;
    private float horizontalTimer;

    [SerializeField]
    private float verticalMoveTimeInterval;
    private float verticalTimer;

    public BlockGrid blockGrid;
    public Spawner spawner;

    private void Update()
    {
        TransformGroup();
    }

    private void FixedUpdate()
    {
        NaturalFall();
        MoveMent();
    }

    //父物体自然下降代码
    private void NaturalFall()
    {
        fallTimer += Time.fixedDeltaTime;
        if (transform.childCount == 4 && fallTimer > fallTimeInterval)
        {
            fallTimer = 0;
            Vector3 offsetPos = new Vector3(0, -1, 0);
            Vector3 newPos = transform.position + offsetPos;
            if (!DetectChildIFOutBoundary(transform, newPos) && !DetectChildIFExistOtherBlocks(transform, newPos))
            {
                transform.position = newPos;
            }
            else
            {
                //下降失败，触碰到底部边缘或者其他已经存在的方块，格子系统开始处理
                blockGrid.SetGrid();
                blockGrid.ToUpdateGrid();
                //开始下一次下降回合
                spawner.StartNextFallGroup();
            }
        }
    }

    private void MoveMent()
    {
        horizontalTimer += Time.fixedDeltaTime;
        verticalTimer += Time.fixedDeltaTime;

        if (transform.childCount == 4)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && horizontalTimer > horizontalMoveTimeInterval)
            {
                Vector3 offsetPos = new Vector3(-1, 0, 0);
                Vector3 newPos = transform.position + offsetPos;
                if (!DetectChildIFOutBoundary(transform, newPos) && !DetectChildIFExistOtherBlocks(transform, newPos))
                {
                    transform.position = newPos;
                }
                horizontalTimer = 0;
            }

            if (Input.GetKey(KeyCode.RightArrow) && horizontalTimer > horizontalMoveTimeInterval)
            {
                Vector3 offsetPos = new Vector3(1, 0, 0);
                Vector3 newPos = transform.position + offsetPos;
                if (!DetectChildIFOutBoundary(transform, newPos) && !DetectChildIFExistOtherBlocks(transform, newPos))
                {
                    transform.position = newPos;
                }
                horizontalTimer = 0;
            }

            if (Input.GetKey(KeyCode.DownArrow) && verticalTimer > horizontalMoveTimeInterval)
            {
                Vector3 offsetPos = new Vector3(0, -1, 0);
                Vector3 newPos = transform.position + offsetPos;
                if (!DetectChildIFOutBoundary(transform, newPos)&& !DetectChildIFExistOtherBlocks(transform, newPos))
                {
                    transform.position =newPos;
                }
                else
                {
                    blockGrid.SetGrid();
                    //Check And Update Grid After Set the Grid
                    blockGrid.ToUpdateGrid();
                    spawner.StartNextFallGroup();
                }
                verticalTimer = 0;
            }
        }
    }

    //输入控制块组合是否变换是否旋转
    private void TransformGroup()
    {
        int childCount = transform.childCount;
        if (childCount == 4)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameManager.Instance.audioSource.PlayOneShot(GameManager.Instance.audioClip_Transform);
                RotateTheBlockGroup(transform);
            }
        }
    }

    private void RotateTheBlockGroup(Transform parent)
    {
        int childCount = parent.childCount;
        Vector3 pivot = parent.position;
        for (int i = 0; i < childCount; i++)
        {
            Vector3 originPos = parent.GetChild(i).position;
            Vector3 offset = originPos - pivot;
            //旋转后的位置是根据以下降父物体为中心，旋转得到的
            Vector3 newPos = pivot + new Vector3(-offset.y, offset.x, 0);
            if (DetectIFOutBoundary(newPos) || DetectIFExistOtherBlocks(newPos))
            {
                return;
            }
        }
        parent.Rotate(new Vector3(0, 0, 90), Space.Self);
    }
    private bool DetectIFOutBoundary(Vector3 pos)
    {
        //Why  can't x=(int)(pos.x+0.1) 
        float X = pos.x;
        float Y = pos.y;


        //aovid the error value
        if (X < -0.1 || X > 9.1 || Y < -0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool DetectIFExistOtherBlocks(Vector3 pos)
    {

        int X = (int)(pos.x + 0.1);
        int Y = (int)(pos.y + 0.1);

        Transform[,] grid = blockGrid.grid;

        //Judge if the index is outside the bounds of the grid.
        if (Y > 19)
        {
            return false;
        }
        //Y marks the row,X marks the Culumn. 

        if (grid[Y,X])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool DetectChildIFOutBoundary(Transform parent,Vector3 pos)
    {
        Vector3 offsetPos = pos - parent.position;
        int childCoun = parent.childCount;
        for(int i=0;i<childCoun;i++)
        {
            Vector3 newPos = parent.GetChild(i).position + offsetPos;
            if (DetectIFOutBoundary(newPos))
            {
                return true;
            }
        }
        return false;
    }
    private bool DetectChildIFExistOtherBlocks(Transform parent,Vector3 pos)
    {
        Vector3 offsetPos = pos - parent.position;
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Vector3 newPos = parent.GetChild(i).position + offsetPos;
            if (DetectIFExistOtherBlocks(newPos))
            {
                return true;
            }
        }
        return false;
    }
}
