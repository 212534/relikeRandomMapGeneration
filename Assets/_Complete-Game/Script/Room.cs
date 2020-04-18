using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour {

    public int X;
    public int Y;
    public bool ROOM;
    public bool CORRIDOR;
    public int flag;
    public List<GameObject> RoomElement = new List<GameObject>();//存放一个房间所有块
    public List<Vector2> postionList = new List<Vector2>();//存放所有块的坐标
    private List<string> direction = new List<string>();//存放已经在哪些方向开洞
    private List<List<Vector2>> directionPostion = new List<List<Vector2>>();//存放开洞洞口的坐标
    private List<List<GameObject>> directionGameObject = new List<List<GameObject>>();//存放开洞洞口的坐标对应的方块
    public List<GameObject> EDGE= new List<GameObject>();
    public List<List<Vector2>> EDGEXY = new List<List<Vector2>>();
    public Vector2 nextStartXY;
    public Vector2 nextPassagewayXY;
    public int nextI;
    public Vector2 StartFillRoom;//填充房间的 起始种子点
    public List<GameObject> Fill = new List<GameObject>();//填充的陆地
    // Use this for initialization
    void Start () {


        
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}
    public void createMAP()
    {
        
    }
    public void FillmainROOM()
    {
        if (ROOM == true)
        {
            for (int x = 1; x < map.Instance.initialRoomX - 1; x++)
            {
                for (int y = 1; y < map.Instance.initialRoomY - 1; y++)
                {
                    int index = Random.Range(0, 4);
                    GameObject go = GameObject.Instantiate(map.Instance.floor[index], new Vector3(x + RoomElement[0].transform.position.x, y + RoomElement[0].transform.position.y, 0), Quaternion.identity);
                    go.transform.SetParent(transform);
                    Fill.Add(go);
                }
                
            }

            //用来填补开洞留下的缺口
            for (int i = 0; i < direction.Count; i++)
            {
                if (direction[i] == "up")
                {
                    for (int x = (int)(directionPostion[i][0].x) + 1; x < directionPostion[i][1].x; x++)
                    {
                        int index = Random.Range(0, 4);
                        GameObject go = GameObject.Instantiate(map.Instance.floor[index], new Vector3(x + RoomElement[0].transform.position.x, directionPostion[i][0].y + RoomElement[0].transform.position.y, 0), Quaternion.identity);
                        go.transform.SetParent(transform);
                        Fill.Add(go);
                    }
                }
                else if (direction[i] == "down")
                {
                    for (int x = (int)(directionPostion[i][0].x) + 1; x < directionPostion[i][1].x; x++)
                    {
                        int index = Random.Range(0, 4);
                        GameObject go = GameObject.Instantiate(map.Instance.floor[index], new Vector3(x + RoomElement[0].transform.position.x, directionPostion[i][0].y + RoomElement[0].transform.position.y, 0), Quaternion.identity);
                        go.transform.SetParent(transform);
                        Fill.Add(go);
                    }
                }
                else if (direction[i] == "left")
                {
                    for (int y = (int)(directionPostion[i][0].y) + 1; y < directionPostion[i][1].y; y++)
                    {
                        int index = Random.Range(0, 4);
                        GameObject go = GameObject.Instantiate(map.Instance.floor[index], new Vector3(directionPostion[i][0].x+ RoomElement[0].transform.position.x, y+ RoomElement[0].transform.position.y, 0), Quaternion.identity);
                        go.transform.SetParent(transform);
                        Fill.Add(go);
                    }
                }
                else if (direction[i] == "right")
                {
                    for (int y = (int)(directionPostion[i][0].y) + 1; y < directionPostion[i][1].y; y++)
                    {
                        int index = Random.Range(0, 4);
                        GameObject go = GameObject.Instantiate(map.Instance.floor[index], new Vector3(directionPostion[i][0].x + RoomElement[0].transform.position.x, y + RoomElement[0].transform.position.y, 0), Quaternion.identity);
                        go.transform.SetParent(transform);
                        Fill.Add(go);
                    }
                }
            }

        }
    }

    public List<Vector2> direction_8(Vector2 xy)//生成XY的八向坐标
    {
        List<Vector2> dire = new List<Vector2>();
        List<Vector2> temporaryDire = new List<Vector2>();
        dire.Add(new Vector2(xy.x, xy.y + 1));
        dire.Add(new Vector2(xy.x, xy.y - 1));
        dire.Add(new Vector2(xy.x + 1, xy.y));
        dire.Add(new Vector2(xy.x - 1, xy.y));
       // dire.Add(new Vector2(xy.x - 1, xy.y + 1));   //八向坐标不好用   这里改用四向坐标 
       // dire.Add(new Vector2(xy.x + 1, xy.y + 1));
        //dire.Add(new Vector2(xy.x - 1, xy.y - 1));
       // dire.Add(new Vector2(xy.x + 1, xy.y - 1));
        for(int i =0;i<4;i++)//对超出范围的坐标进行删除操作
        {
            if (dire[i].x < 0 || dire[i].x > map.Instance.initialRoomX-1)
            {
                temporaryDire.Add(dire[i]);
            }
            if (dire[i].y < 0 || dire[i].y > map.Instance.initialRoomY-1)
            {
                temporaryDire.Add(dire[i]);
            }
        }
        foreach (var t in temporaryDire)
        {
            if (dire.Contains(t) == true)
            {
                dire.Remove(t);
            }
        }

        return dire;
    }
    public void FillRoom(Vector2 xy)//填充房间
    {
        if(ROOM == false)
        {
            foreach (var t in EDGE)
            {
                if (xy == findPostion(t))//如果这个点是墙 那么终止函数
                {
                    return;
                }
            }
            foreach (var t in Fill)
            {
                if (xy == findPostion(t))//如果这个位置已经生成过陆地了 则退出函数
                {
                    return;
                }
            }

            List<Vector2> dire = new List<Vector2>();
            dire = direction_8(xy);//生成这个点的八向坐标

            int index = Random.Range(0, 4);
            GameObject go = GameObject.Instantiate(map.Instance.floor[index], new Vector3(xy.x + RoomElement[0].transform.position.x, xy.y + RoomElement[0].transform.position.y, 0), Quaternion.identity);
            go.transform.SetParent(transform);
            Fill.Add(go);

            foreach (var T in dire)
            {
                FillRoom(T);//递归
            }
        }
       
    }
    public void findStartFillRoom()//取得起始种子点
    {
        if (ROOM == false)
        {
            if (directionPostion[0][0].x == directionPostion[0][1].x)
            {
                StartFillRoom = new Vector2(directionPostion[0][0].x, directionPostion[0][0].y + 1);//存储一个种子点
            }
            else if (directionPostion[0][0].y == directionPostion[0][1].y)
            {
                StartFillRoom = new Vector2(directionPostion[0][0].x + 1, directionPostion[0][0].y);
            }
        }
        
    }

    public void repairRoom()
    {
        if (ROOM == false)//如果是走廊的话则执行这个函数
        {
            if (direction.Count == 1)
            {
                return;
            }
            if (direction.Count >= 2)
            {
                for (int i = 0; i < direction.Count; i++)
                {
                                      
                    EDGE.Add(findOBJ(directionPostion[i][0]));
                    EDGE.Add(findOBJ(directionPostion[i][1]));
                    EDGEXY.Add(directionPostion[i]);

                }

                foreach (var t in RoomElement)
                {
                    if (EDGE.Contains(t) == false)
                    {
                        Destroy(t);
                    }
                }
            }
        }


    }
    public GameObject findOBJ(Vector2 xy)//按坐标寻找一个物体
    {
        GameObject obj;
        foreach (var t in RoomElement)
        {
            if (t.transform.position.x == xy.x + RoomElement[0].transform.position.x && t.transform.position.y == xy.y + RoomElement[0].transform.position.y)
            {
                return t;
            }
        }
        return null;
    }
    public Vector2 findPostion(GameObject obj)//返回给定物体的相对坐标
    {
        Vector2 pos;
        pos = new Vector2(obj.transform.position.x - RoomElement[0].transform.position.x, obj.transform.position.y - RoomElement[0].transform.position.y);
        return pos;
    }

    public void edgeRemove(List<Vector2> delXY,int i)//用来删除没用的方块
    {
        List<Vector2> tes = new List<Vector2>();
        for (int n = 0; n < EDGEXY[i].Count; n++)
        {
            if (delXY.Contains(EDGEXY[i][n]))
            {
                tes.Add(EDGEXY[i][n]);
            }
        }
        foreach (var te in tes)
        {
            EDGEXY[i].Remove(te);
        }
        // print(EDGEXY[i].Count+"     "+i);

    }

    public List<Vector2> edgeDel(List<Vector2> EDGExy,int flage,int direction)
    {
        
        List<Vector2> delXY = new List<Vector2>();
        if (flage == 0)
        {
            foreach (var t in EDGExy)
            {
                if (direction == 0)
                {
                    for (int i = 0; i < t.y; i++)
                    {
                        delXY.Add(new Vector2(t.x, i));//需要被删除的
                    }
                }
                else if(direction == 1)
                {
                    for (int i = (int)(t.y + 1); i < map.Instance.initialRoomY -1; i++)
                    {
                        delXY.Add(new Vector2(t.x, i));//需要被删除的
                    }
                }
            }
        }
        else if(flage == 1)
        {
            foreach (var t in EDGExy)
            {
                if (direction == 0)
                {
                    for (int i = 0; i < t.x; i++)
                    {
                        delXY.Add(new Vector2(i, t.y));//需要被删除的
                    }
                }
                else if (direction == 1)
                {
                    for (int i = (int)(t.x + 1); i < map.Instance.initialRoomX - 1; i++)
                    {
                        delXY.Add(new Vector2(i, t.y));//需要被删除的
                    }
                }
                
            }
        }
        return delXY;
        
    }

    public void createCorridor()//生成通道
    {
        if (ROOM == true)//如果这是一个房间 那么不执行本函数
        {
            return;
        }
        if (direction.Count == 1)
        {
            return;
        } 
        if (direction.Count == 2)//如果已经在两个方向开洞
        {
            if (direction.Contains("left") && direction.Contains("right"))//如果是左右开洞
            {
                Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
            }
            else if (direction.Contains("up") && direction.Contains("down"))//如果是上下开洞
            {
                Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数
            }
            else if (direction.Contains("up") && direction.Contains("left")) //如果是在上 左方向开洞
            {
                passageway("down", 0);//补全成四个洞 
                passageway("right", 0);//补全成四个洞 
                Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
                Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数

                List<Vector2> EDGDEL1 = edgeDel(EDGEXY[0], 0, 0);
                List<Vector2> EDGDEL2 = edgeDel(EDGEXY[3], 1, 1);
                List<Vector2> EDGDEL3 = edgeDel(EDGEXY[2], 1, 1);
                List<Vector2> EDGDEL4 = edgeDel(EDGEXY[1], 0, 0);

                edgeRemove(EDGDEL1, 3);//删去多余的边
                edgeRemove(EDGDEL2, 0);//删去多余的边  
                edgeRemove(EDGDEL3, 1);//删去多余的边
                edgeRemove(EDGDEL4, 2);//删去多余的边

                direction.Remove("down");
                direction.Remove("right");
            }
            else if (direction.Contains("up") && direction.Contains("right"))
            {
                passageway("down", 0);//补全成四个洞 
                passageway("left", 0);//补全成四个洞 
                Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
                Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数

                List<Vector2> EDGDEL1 = edgeDel(EDGEXY[2], 1, 0);
                List<Vector2> EDGDEL2 = edgeDel(EDGEXY[0], 0, 0);
                List<Vector2> EDGDEL3 = edgeDel(EDGEXY[3], 1, 0);
                List<Vector2> EDGDEL4 = edgeDel(EDGEXY[1], 0, 0);

                edgeRemove(EDGDEL1, 0);//删去多余的边
                edgeRemove(EDGDEL2, 2);//删去多余的边  
                edgeRemove(EDGDEL3, 1);//删去多余的边 
                edgeRemove(EDGDEL4, 3);//删去多余的边 

                direction.Remove("down");
                direction.Remove("left");
            }
            else if (direction.Contains("down") && direction.Contains("left"))
            {
                passageway("up", 0);//补全成四个洞 
                passageway("right", 0);//补全成四个洞 
                Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
                Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数

                List<Vector2> EDGDEL1 = edgeDel(EDGEXY[3], 1, 1);
                List<Vector2> EDGDEL2 = edgeDel(EDGEXY[1], 0, 1);
                List<Vector2> EDGDEL3 = edgeDel(EDGEXY[0], 0, 1);
                List<Vector2> EDGDEL4 = edgeDel(EDGEXY[2], 1, 1);

                edgeRemove(EDGDEL1, 1);//删去多余的边
                edgeRemove(EDGDEL2, 3);//删去多余的边
                edgeRemove(EDGDEL3, 2);//删去多余的边
                edgeRemove(EDGDEL4, 0);//删去多余的边

                direction.Remove("up");
                direction.Remove("right");
            }
            else if (direction.Contains("down") && direction.Contains("right"))
            {
                passageway("up", 0);//补全成四个洞 
                passageway("left", 0);//补全成四个洞 
                Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
                Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数

                List<Vector2> EDGDEL1 = edgeDel(EDGEXY[1], 0, 1);
                List<Vector2> EDGDEL2 = edgeDel(EDGEXY[2], 1, 0);
                List<Vector2> EDGDEL3 = edgeDel(EDGEXY[0], 0, 1);
                List<Vector2> EDGDEL4 = edgeDel(EDGEXY[3], 1, 0);

                edgeRemove(EDGDEL1, 2);//删去多余的边
                edgeRemove(EDGDEL2, 1);//删去多余的边
                edgeRemove(EDGDEL3, 3);//删去多余的边
                edgeRemove(EDGDEL4, 0);//删去多余的边

                direction.Remove("up");
                direction.Remove("left");
            }
        }
        if (direction.Count == 3)
        {
            if (direction.Contains("left") && direction.Contains("right"))//如果是左右开洞
            {
                Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
                if (direction.Contains("up"))//两边有洞 顶边有洞
                {
                    passageway("down", 0);//补全成四个洞 
                    Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");

                    List<Vector2> delXY = new List<Vector2>();
                    foreach (var t in EDGEXY[1])
                    {
                        for (int i = 0; i < t.y; i++)
                        {
                            delXY.Add(new Vector2(t.x, i));//需要被删除的
                        }
                    }
                    edgeRemove(delXY,2);//删去多余的边
                    edgeRemove(delXY, 3);

                    List<Vector2> delXY2 = new List<Vector2>();
                    foreach (var t in EDGEXY[1])
                    {
                        if (t.x < EDGEXY[3][0].x && t.x > EDGEXY[2][0].x)
                        {
                            delXY2.Add(t);
                        }
                    }
                    edgeRemove(delXY2, 1);//开个口子
                    direction.Remove("down");
                }
                else if (direction.Contains("down"))//两边有洞底边有洞
                {
                    passageway("up", 0);//补全成四个洞 
                    Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");
                    List<Vector2> delXY = new List<Vector2>();
                    foreach (var t in EDGEXY[0])
                    {
                        for (int i = (int)(t.y); i< map.Instance.initialRoomY-1; i++)
                        {
                            delXY.Add(new Vector2(t.x, i));//需要被删除的
                        }
                    }
                    edgeRemove(delXY, 2);
                    edgeRemove(delXY, 3);
                    List<Vector2> delXY2 = new List<Vector2>();
                    foreach (var t in EDGEXY[0])
                    {
                        if (t.x < EDGEXY[3][EDGEXY[3].Count- 1].x && t.x > EDGEXY[2][EDGEXY[2].Count - 1].x)
                        {
                            delXY2.Add(t);
                        }
                    }
                    edgeRemove(delXY2, 0);
                    direction.Remove("up");
                }
            }
            else if (direction.Contains("up") && direction.Contains("down"))//如果是上下开洞
            {
                Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数

                if (direction.Contains("left"))
                {
                    passageway("right", 0);//补全成四个洞 
                    Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");

                    List<Vector2> delXY = new List<Vector2>();
                    foreach (var t in EDGEXY[0])
                    {
                        for (int i = (int)(t.x); i < map.Instance.initialRoomX - 1; i++)
                        {
                            delXY.Add(new Vector2(i, t.y));//需要被删除的
                        }
                    }
                    edgeRemove(delXY, 2);
                    edgeRemove(delXY, 3);
                    List<Vector2> delXY2 = new List<Vector2>();
                    foreach (var t in EDGEXY[0])
                    {
                        if (t.y < EDGEXY[3][EDGEXY[3].Count- 1].y && t.y > EDGEXY[2][EDGEXY[2].Count - 1].y)
                        {
                            delXY2.Add(t);
                        }
                    }
                    edgeRemove(delXY2, 0);//开个口子
                    direction.Remove("right");
                }
                else if(direction.Contains("right"))
                {
                    passageway("left", 0);//补全成四个洞 
                    Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");

                    List<Vector2> delXY = new List<Vector2>();
                    foreach (var t2 in EDGEXY[1])
                    {
                        for (int i = 0; i <= t2.x; i++)
                        {
                            delXY.Add(new Vector2(i, t2.y));//需要被删除的
                        }
                    }
                    edgeRemove(delXY, 2);
                    edgeRemove(delXY, 3);
                    List<Vector2> delXY2 = new List<Vector2>();
                    foreach (var t in EDGEXY[1])
                    {
                        if (t.y < EDGEXY[3][0].y && t.y > EDGEXY[2][0].y)
                        {
                            delXY2.Add(t);
                        }
                    }
                    edgeRemove(delXY2, 1);//开个口子
                    direction.Remove("left");

                }
            }
        }
        if (direction.Count == 4)
        {
            Boundary(directionPostion[direction.IndexOf("left")], directionPostion[direction.IndexOf("right")], "LR");//把左右两个洞的上下边缘放进函数
            Boundary(directionPostion[direction.IndexOf("up")], directionPostion[direction.IndexOf("down")], "UD");//把上下两个洞的左右边缘放进函数
            

            List<Vector2> EDGDEL1 = new List<Vector2>();
            for (int i = 0; i < map.Instance.initialRoomX -2 ; i++)
            {
                for (int y = (int)(EDGEXY[0][i].y+1); y < EDGEXY[1][i].y; y++)
                {
                    EDGDEL1.Add(new Vector2(EDGEXY[1][i].x, y));
                }
            }
            List<Vector2> EDGDEL2 = new List<Vector2>();
            for (int i = 0; i < map.Instance.initialRoomY - 2; i++)
            {
                for (int x = (int)(EDGEXY[2][i].x+1); x < EDGEXY[3][i].x; x++)
                {
                    EDGDEL2.Add(new Vector2(x, EDGEXY[3][i].y));
                }
            }
            edgeRemove(EDGDEL1, 2);//删去多余的边
            edgeRemove(EDGDEL1, 3);//删去多余的边
            edgeRemove(EDGDEL2, 1);//删去多余的边
            edgeRemove(EDGDEL2, 0);//删去多余的边
        }

    }


    public void Draw()
    {
        Vector2 XYJIZHUN = new Vector2(RoomElement[0].transform.position.x, RoomElement[0].transform.position.y);
        foreach (var a in EDGEXY)
        {
            foreach(var b in a)
            {
                int index = Random.Range(0, 4);
                GameObject go = GameObject.Instantiate(map.Instance.wall[index], new Vector3(b.x + XYJIZHUN.x, b.y + XYJIZHUN.y, 0), Quaternion.identity);
                go.transform.SetParent(transform);
                EDGE.Add(go); //把边存进去
            }
        }

    }


    public void Boundary(List<Vector2> startXY, List<Vector2> endXY,string str)
    {
        
        
        List<List<Vector2>> edgeLR = new List<List<Vector2>>();//储存边
        List<Vector2> Road1 = new List<Vector2>();//储存通道


        List<List<Vector2>> edgeUD = new List<List<Vector2>>();//储存边
        List<Vector2> Road2 = new List<Vector2>();//储存通道
        


        if (str == "UD")//如果是左右开口的类型
        {
            for (int i = 0; i < 2; i++)
            {
                edgeUD.Add(equation(startXY, endXY, i,str));//这样就可以把这个支线经过的方块坐标放入edgeUD中 
                
            }
            EDGEXY.Add(edgeUD[0]);
            EDGEXY.Add(edgeUD[1]);
        }

        if (str == "LR")//如果是左右开口的类型
        {
            for (int i = 0; i < 2; i++) //为0时为下边 为1时为上边
            {
                edgeLR.Add(equation(startXY, endXY, i,str));//这样就可以把这个支线经过的方块坐标放入edgeUD中 
                
            }
            EDGEXY.Add(edgeLR[0]);
            EDGEXY.Add(edgeLR[1]);
        }
        

    }


    public List<Vector2> equation(List<Vector2> startXY, List<Vector2> endXY, int i,string STR)
    {
        List<Vector2> EDFEXY = new List<Vector2>();

        float K;
        float B;
        if (endXY[i].x - startXY[i].x == 0)//为一条竖线的时候 斜率为无穷
        {
            K = Mathf.Infinity;//获得直线方程的K
            B = endXY[i].x;
        }
        else
        {
            K = (endXY[i].y - startXY[i].y) / (endXY[i].x - startXY[i].x);//获得直线方程的K
            B = startXY[i].y - K * startXY[i].x;//获得支线方程的B
        }
        

        if (STR == "LR")
        {
            for (int n = 1; n < map.Instance.initialRoomX - 1; n++)
            {

                float y = n * K + B;//求得Y轴的坐标
               // print(y + "  " + (int)Mathf.Round(y));

                EDFEXY.Add(new Vector2(n, (int)Mathf.Round(y)));
              //  print(EDFEXY[n - 1]);

            }
        }
        if (STR == "UD")
        {
            for (int n = 1; n < map.Instance.initialRoomY - 1; n++)
            {
                float x = 0;
                if (K == Mathf.Infinity)//斜率为无穷时
                {
                    x = B;

                }
                else
                {
                    x = (n - B) / K;//求得Y轴的坐标
                }
                
                EDFEXY.Add(new Vector2((int)Mathf.Round(x),n ));
            }
        }


        return EDFEXY;
    }

    public void passageway(string directionSTR, int flag)
    {
        foreach (var str in direction)
        {
            if (str == directionSTR)
            {
                return;
            }
        }

        direction.Add(directionSTR);//用来表示这个房间那些方向已经开洞 

        createPassageway0(directionSTR);

    }
    public void passageway(string directionSTR, int flag,Vector2 startXY, Vector2 PassagewayXY, int i)//重载方法  被动开洞时使用
    {
        foreach (var str in direction)
        {
            if (str == directionSTR)
            {
                return;
            }
        }
        direction.Add(directionSTR);

        createPassageway1(directionSTR,startXY, PassagewayXY, i);
    }


    public void createPassageway0(string str)
    {
        int i = 0;//这个开口的大小
        int PassagewayY = -1;
        int PassagewayX = -1;
        int startX = 0;
        int endX = 0;
        int startY = 0;
        int endY = 0;
        List<int> Index = new List<int>();

        if (str == "up" || str == "down")
        {
            i = Random.Range(2, (int)map.Instance.initialRoomX / 2);//随机洞口大小
            PassagewayY = str == "up" ? map.Instance.initialRoomY - 1 : 0;//检测上下

            startX = Random.Range(3, map.Instance.initialRoomX - 3 - i);//随机起点  位于X轴时
            endX = startX + i;

            List<Vector2> startORendXY = new List<Vector2>();//用来存储起点和终点的坐标
            startORendXY.Add(new Vector2(startX - 1, PassagewayY));
            startORendXY.Add(new Vector2(endX + 1, PassagewayY));
           // print(startORendXY[0] + "  " + startORendXY[1]);
            directionPostion.Add(startORendXY);
        }
        else
        {
            i = Random.Range(2, (int)map.Instance.initialRoomY / 2);//随机洞口大小
            PassagewayX = str == "right" ? map.Instance.initialRoomX - 1 : 0;//检测左右


            startY = Random.Range(3, map.Instance.initialRoomY - 3 - i);//随机起点 位于Y轴时
            endY = startY + i;


            List<Vector2> startORendXY = new List<Vector2>();//用来存储起点和终点的坐标
            startORendXY.Add(new Vector2(PassagewayX, startY - 1));
            startORendXY.Add(new Vector2(PassagewayX,endY + 1));
            //print(startORendXY[0] + "  " + startORendXY[1]);
            directionPostion.Add(startORendXY);
        }

        
        
        for (int n = 0; n < postionList.Count; n++)//记录下需要被删除的格子
        {
            if (postionList[n].y == PassagewayY && postionList[n].x <= endX && postionList[n].x >= startX)
            {
                Index.Add(n);
            }
            else if (postionList[n].x == PassagewayX && postionList[n].y <= endY && postionList[n].y >= startY)
            {
                Index.Add(n);
            }
        }

        nextStartXY = new Vector2(startX, startY);
        nextPassagewayXY = new Vector2(PassagewayX, PassagewayY); //用于保存上下左右
        nextI = i;

        for (int m = 0; m < Index.Count; m++)//开洞
        {
            Destroy(RoomElement[Index[m]]);
        }
    }


    public void createPassageway1(string str,Vector2 startXY, Vector2 PassagewayXY, int i)
    {
        int PassagewayY = -1;
        int PassagewayX = -1;
        int startX = 0;
        int endX = 0;
        int startY = 0;
        int endY = 0;
        List<int> Index = new List<int>();

        startX = (int)startXY.x;
        endX = startX + i;
        startY = (int)startXY.y;
        endY = startY + i;


        if (str == "up" || str == "down")
        {       
            PassagewayY = str == "up" ? map.Instance.initialRoomY - 1 : 0;//检测上下


            List<Vector2> startORendXY = new List<Vector2>();//用来存储起点和终点的坐标
            startORendXY.Add(new Vector2(startX - 1, PassagewayY));
            startORendXY.Add(new Vector2(endX + 1, PassagewayY));
            //print(startORendXY[0] + "  " + startORendXY[1]);
            directionPostion.Add(startORendXY);
        }
        else
        {
            PassagewayX = str == "right" ? map.Instance.initialRoomX - 1 : 0;//检测左右

            List<Vector2> startORendXY = new List<Vector2>();//用来存储起点和终点的坐标
            startORendXY.Add(new Vector2(PassagewayX, startY - 1));
            startORendXY.Add(new Vector2(PassagewayX, endY + 1));
           // print(startORendXY[0] + "  " + startORendXY[1]);
            directionPostion.Add(startORendXY);
        }


        
        for (int n = 0; n < postionList.Count; n++)//记录下需要被删除的格子
        {
            if (postionList[n].y == PassagewayY && postionList[n].x <= endX && postionList[n].x >= startX)
            {
                Index.Add(n);
            }
            else if (postionList[n].x == PassagewayX && postionList[n].y <= endY && postionList[n].y >= startY)
            {
                Index.Add(n);
            }
        }

        for (int m = 0; m < Index.Count; m++)//开洞
        {
            Destroy(RoomElement[Index[m]]);
        }
    }

}
