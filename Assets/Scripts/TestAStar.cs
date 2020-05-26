using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{

    public int beginX = -3;
    public int beginY = 5;

    public int offsetX = -2;
    public int offsetY = 2;

    public int mapW = 5;
    public int mapH = 5;

    public Material red;

    public Material yellow;

    public Material green;

    public Material white;
    private Vector2 beginPos = Vector2.right * -1;

    private List<AStarNode> list;
    private Dictionary<string, GameObject> Cubes = new Dictionary<string, GameObject>();
    // Use this for initialization
    void Start()
    {
        AStarMgr.Instance.InitMapInfo(mapW, mapH);
        for (int i = 0; i < mapW; ++i)
        {
            for (int j = 0; j < mapH; ++j)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);
                obj.name = i + "_" + j;
                Cubes.Add(obj.name, obj);
                AStarNode node = AStarMgr.Instance.nodes[i, j];
                if (node.type == E_Node_Type.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out info, 1000))
            {
                if (beginPos == Vector2.right * -1)
                {
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            Cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = white;
                        }

                    }
                    string[] strs = info.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                }
                else
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));

                    list = AStarMgr.Instance.FindPath(beginPos, endPos);

                    Cubes[beginPos.x + "_" + beginPos.y].GetComponent<MeshRenderer>().material = white;

                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            Cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }


                    beginPos = Vector2.right * -1;

                }
            }
        }
    }
}
