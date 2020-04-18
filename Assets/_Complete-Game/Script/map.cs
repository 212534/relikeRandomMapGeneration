using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour {
    private static map instance;

    public GameObject[] floor;//地板
    public GameObject[] wall;//墙壁
    public GameObject[] BLACK;//占位
    public int roomsRadius;//最小值可以为0
    public int initialRoomX;//初始房间的宽
    public int initialRoomY;//初始房间的高
    public int Number_of_rooms;
    public int randomRoomCount;//随机选择几个房间  也就相当于有几个路径
    public GameObject centerRoom;//中心的那个房间
    public List<GameObject> AllRoom = new List<GameObject>();//存储所有房间块
    public List<GameObject> Room = new List<GameObject>();//存储所有房间
    public List<GameObject> corridor = new List<GameObject>();//存储所有走廊
    public List<GameObject> ALLcorridorAndRoomCube = new List<GameObject>();//所有的地板方块存起来  不包括围墙
    public List<List<GameObject>> Route = new List<List<GameObject>>();//储存所有路径
    private GameObject mapHolder;
    public GameObject mapCubePrfbe;
    private int ALLroomsXY;

    public float A;
    public float B;
    public float TIME;
    public bool AA;
    public bool BB;
    public bool CC;
    public bool DD;
    public bool EE;
    public bool FF;
    public bool GG;
    public bool HH;
    public bool II;
    public bool JJ;
    public bool KK;


    public static map Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {

        
          createRoom();
          centerRoom = FileRoom(new Vector2(roomsRadius, roomsRadius));//取得中心点那个房间
          randomRoom();//随机选出几个Room当作房间
          WalkLattice();//走格子
          clearRoom();//清除没用的格子
          OpenPassageway();//根据之前得到的路径 来开洞
          tseta();
          DRAW();//把通道的边缘画出来
          RepairRoom();//清除框线
          fill();//填充函数
          saveALLCube();//将所有的地板方块存起来  不包括围墙
        


        /*
        Invoke("createRoom", 2.0f);
        Invoke("qude", 3.0f);
        Invoke("randomRoom", 4.0f);
        Invoke("WalkLattice", 5.0f);
        Invoke("clearRoom", 6.0f);
        Invoke("OpenPassageway", 6.5f);
        Invoke("tseta", 6.6f);
        Invoke("DRAW", 6.7f);
        Invoke("RepairRoom", 6.7f);
        Invoke("fill", 6.9f);
        Invoke("saveALLCube", 19.0f);
        */
    }
	
	// Update is called once per frame
	void Update () {

        
    }
    public void qude()
    {
        centerRoom = FileRoom(new Vector2(roomsRadius, roomsRadius));//取得中心点那个房间
    }
    public void saveALLCube()
    {
        foreach (var a in AllRoom)
        {
            foreach(var b in a.GetComponent<Room>().Fill)
            {
                ALLcorridorAndRoomCube.Add(b);  //将所有的地板方块存起来  不包括围墙
            }    
        }
    }

    public void fill()
    {
        foreach (var a in AllRoom)
        {
            a.GetComponent<Room>().FillmainROOM();
            a.GetComponent<Room>().findStartFillRoom();
            a.GetComponent<Room>().FillRoom(a.GetComponent<Room>().StartFillRoom);
            
        }
    }

    public void RepairRoom()
    {
        foreach (var a in AllRoom)
        {
            a.GetComponent<Room>().repairRoom();
        }
    }
    public void tseta()
    {
        foreach (var a in AllRoom)
        {
            a.GetComponent<Room>().createCorridor();
        }
    }

    public void DRAW()
    {
        foreach (var a in AllRoom)
        {
            a.GetComponent<Room>().Draw();
        }
    }

    public void clearRoom()
    {
        List<GameObject> clearRoom = new List<GameObject>(AllRoom);//储存需要被删除的房间
        for (int n = 0; n < randomRoomCount; n++)
        {
            for (int i = 0; i < Route[n].Count; i++)
            {
                //print(Route[n][i].name);
                clearRoom.Remove(Route[n][i]);
            }
        }

        foreach (var arom in Route)
        {
            foreach (var rom in arom)
            {
                foreach (var crom in clearRoom)
                {
                    if (crom == rom)
                    {
                        clearRoom.Remove(crom);
                    }
                }
            }
        }
        for (int m = 0; m < clearRoom.Count; m++)
        {
            AllRoom.Remove(clearRoom[m]);
            Destroy(clearRoom[m]);
        }
    }

    public void OpenPassageway()//根据之前得到的路径 来开洞
    {
        foreach (var ro in Route)
        {
            for (int i = 0; i < ro.Count; i++)//开始按路径走  最后一个房间是不需要开洞的 所以
            {
                Vector2 nextStartXY;
                Vector2 nextPassagewayXY;
                int nextI;
                string FANG=null;

                if (i != ro.Count -1)
                {
                    int directionX;
                    int directionY;
                   
                    Vector2 directionXY;
                    directionX = ro[i].GetComponent<Room>().X - ro[i + 1].GetComponent<Room>().X;
                    directionY = ro[i].GetComponent<Room>().Y - ro[i + 1].GetComponent<Room>().Y;
                    directionXY = new Vector2(directionX, directionY);
                    if (directionXY == new Vector2(0, -1))//上
                    {
                        ro[i].GetComponent<Room>().passageway("up", 0);
                        FANG = "down";
                    }
                    else if (directionXY == new Vector2(0, 1))//下
                    {
                        ro[i].GetComponent<Room>().passageway("down", 0);
                        FANG = "up";
                    }
                    else if (directionXY == new Vector2(1, 0))//左
                    {
                        ro[i].GetComponent<Room>().passageway("left", 0);
                        FANG = "right";
                    }
                    else if (directionXY == new Vector2(-1, 0))//右
                    {
                        ro[i].GetComponent<Room>().passageway("right", 0);
                        FANG = "left";
                    }

                    
                    nextStartXY = ro[i].GetComponent<Room>().nextStartXY;
                    nextPassagewayXY = ro[i].GetComponent<Room>().nextPassagewayXY;
                    nextI = ro[i].GetComponent<Room>().nextI;

                    ro[i + 1].GetComponent<Room>().passageway(FANG, 0, nextStartXY, nextPassagewayXY, nextI);//让下一个方块被动开洞
                    

                }    
            }
        }
    }

    public void WalkLattice()//走格子
    {
        
        for (int i = 0; i < Room.Count; i++)
        {
            List<GameObject> route = new List<GameObject>();//用于储存单个路径
            Vector2 endXY = new Vector2();
            route.Add(centerRoom);//先把起点存进去
            Vector2 startXY = new Vector2(route[0].GetComponent<Room>().X, route[0].GetComponent<Room>().Y);//起点
            

            endXY = new Vector2(Room[i].GetComponent<Room>().X, Room[i].GetComponent<Room>().Y); //储存目标点的坐标
            while (true)
            {
                int FLAG = Random.Range(0, 2);
                if ( FLAG == 0 && endXY.x != startXY.x)
                {
                    if (endXY.x > startXY.x)
                    {
                        startXY.x = startXY.x + 1;//向右走一格
                        route.Add(FileRoom(new Vector2(startXY.x, startXY.y)));
                    }
                    else if (endXY.x < startXY.x)
                    {
                        startXY.x = startXY.x - 1;//向左走一格
                        route.Add(FileRoom(new Vector2(startXY.x, startXY.y)));

                    }
                }
                else if(FLAG == 1 && endXY.y != startXY.y)
                {
                    if (endXY.y > startXY.y)
                    {
                        startXY.y = startXY.y + 1;//向上走一格
                        route.Add(FileRoom(new Vector2(startXY.x, startXY.y)));
                    }
                    else if (endXY.y < startXY.y)
                    {
                        startXY.y = startXY.y - 1;//向下走一格子
                        route.Add(FileRoom(new Vector2(startXY.x, startXY.y)));
                    }
                }
                else if (endXY == startXY)//走到了  那么退出
                {
                    break;
                }

            }

            Route.Add(route);//将路径存进去
        } 
    }

    public void randomRoom()//随机选出几个Room当作房间
    {
        Vector2 XY = new Vector2();
        List<GameObject> room = new List<GameObject>(AllRoom);
        FileRoom(new Vector2(roomsRadius, roomsRadius)).GetComponent<Room>().ROOM = true;
        room.Remove(FileRoom(new Vector2(roomsRadius, roomsRadius)));//先把中间的点排除

        for (int i = 0; i < randomRoomCount; i++)
        {
            Room.Add(room[Random.Range(0, room.Count)]);
            Room[i].GetComponent<Room>().ROOM = true;
            room.Remove(Room[i]);
        }

       
    }

    public GameObject FileRoom(Vector2 xy)//寻找到一个房间 并使用他的passageway函数 
    {
        Vector2 XY = new Vector2();
        for (int i = 0; i < AllRoom.Count; i++)
        {
            XY = new Vector2(AllRoom[i].GetComponent<Room>().X, AllRoom[i].GetComponent<Room>().Y);
            if (XY == xy)
            {
                return AllRoom[i];
            }
        }
        return null;//如果没找到 那么返回一个空
    }


    public void createRoom()
    {
        ALLroomsXY = 2 * roomsRadius + 1;
        int count = 1;

        for (int y = 0; y < ALLroomsXY; y++)
        {
            for (int x = 0; x < ALLroomsXY; x++)
            {
                if (x == 0 && y == 0)
                {
                    FirstaddRoom(x, y);
                }
                else
                {
                    count++;
                    addRoom(x, y, count);
                }

            }
        }
    }

    public void FirstaddRoom(int X,int Y)
    {
        mapHolder = new GameObject("mapRoom");
        mapHolder.gameObject.AddComponent<Room>();//给每个房间加一个ROOM脚本
        mapHolder.transform.GetComponent<Room>().X = X;
        mapHolder.transform.GetComponent<Room>().Y = Y;
        for (int y = 0; y < initialRoomY; y++)
        {
            for (int x = 0; x < initialRoomX; x++)
            {
                if (x == 0 || y == 0 || y == initialRoomY - 1 || x == initialRoomX - 1) //生成围墙
                {
                    int index = Random.Range(0, wall.Length);
                    GameObject go = GameObject.Instantiate(wall[index], new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(mapHolder.transform);
                    mapHolder.gameObject.GetComponent<Room>().postionList.Add(new Vector2(x, y));//储存每个方块的坐标
                    mapHolder.gameObject.GetComponent<Room>().RoomElement.Add(go);//储存每个方块
                }
            }
        }
        AllRoom.Add(mapHolder);
        mapCubePrfbe = mapHolder;
    }

    public void addRoom(int X,int Y,int count)
    {
        GameObject go = Instantiate(mapCubePrfbe, new Vector3(initialRoomX * X, initialRoomY * Y, 0), Quaternion.identity);
        go.name = "room" + count;
        go.GetComponent<Room>().X = X;
        go.GetComponent<Room>().Y = Y;
        AllRoom.Add(go);
    }
}
