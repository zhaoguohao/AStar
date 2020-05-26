using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A星寻路管理器 单例模式
/// </summary>
public class AStarMgr
{
    //单例
    private static AStarMgr instace;

    public static AStarMgr Instance
    {
        get
        {
            if (instace == null)
                instace = new AStarMgr();
            return instace;

        }
    }

    //地图的宽高
    private int mapW;
    private int mapH;

    /// <summary>
    /// 地图相关所有的格子对象容器
    /// </summary>
    public AStarNode[,] nodes;

    /// <summary>
    /// 开启列表
    /// </summary>
    private List<AStarNode> openList = new List<AStarNode>();

    /// <summary>
    /// 关闭列表
    /// </summary>
    private List<AStarNode> closeList = new List<AStarNode>();

    /// <summary>
    /// 初始化地图
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMapInfo(int w, int h)
    {
        this.mapW = w;
        this.mapH = h;

        nodes = new AStarNode[w, h];
        for (int i = 0; i < w; ++i)
        {
            for (int j = 0; j < h; ++j)
            {
                AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 30 ? E_Node_Type.Stop : E_Node_Type.Walk);
                nodes[i, j] = node;
            }
        }
    }

    /// <summary>
    /// 寻路方法 提供给外部
    /// </summary>
    /// <param name="starPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public List<AStarNode> FindPath(Vector2 starPos, Vector2 endPos)
    {
        if (starPos.x < 0 || starPos.x >= mapW ||
            starPos.y < 0 || starPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("开始或者结束点在地图格子范围之外！！！");
            return null;
        }

        AStarNode start = nodes[(int)starPos.x, (int)starPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];

        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)

        {
            Debug.Log("开始或者结束点是阻挡！！！");
            return null;
        }

        closeList.Clear();
        openList.Clear();

        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        while (true)
        {
            FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);


            if (openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }

            openList.Sort(SortOpneList);

            closeList.Add(openList[0]);
            start = openList[0];
            openList.RemoveAt(0);

            if (start == end)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //列表翻转
                path.Reverse();
                return path;
            }
        }
    }

    /// <summary>
    /// 排序函数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortOpneList(AStarNode a, AStarNode b)
    {
        if (a.f > b.f)
        {
            return 1;
        }
        else if(a.f==b.f)
        {
            return 1;
        }
        else
        {
            return -1;
        }

    }

    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode father, AStarNode end)
    {
        if (x < 0 || x >= mapW || y < 0 || y >= mapH)
            return;

        AStarNode node = nodes[x, y];

        if (node == null || node.type == E_Node_Type.Stop ||
         closeList.Contains(node) || openList.Contains(node))
            return;
        node.father = father;

        node.g = father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;

        openList.Add(node);
    }
}
