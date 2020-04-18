using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoint //定义节点的数据结构
{
    public int x;
    public int y;
    public int G;//代价
    public int H;//曼哈顿距离
    public int F;
    public int cost;//代价
    public GameObject obj;
    public List<MyPoint> direction_8 = new List<MyPoint>();
   
    public MyPoint parent;//父节点
    public MyPoint()
    {

    }
    public MyPoint(int x0, int y0, int G0, int H0, MyPoint F)
    {
        x = x0;
        y = y0;
        G = G0;
        H = H0;
        parent = F;
    }
    public void seekH(GameObject target)//用于计算H  曼哈顿距离  当前点到终点
    {
        this.H = (Mathf.Abs((int)((int)(target.transform.position.x) - x)) + Mathf.Abs((int)((int)(target.transform.position.y) - y)))*cost;  
    }
    public void SEEKfgh(GameObject target)//把H G F计算好
    {
        seekH(target);
        seekG();
        seekF();
    }
    public void seekF()//计算F
    {
        this.F = this.H + this.G;
    }
    public void seekG()//用于计算G
    {
        int sum = 0;
        MyPoint mp = new MyPoint();
        mp = this;
        while (true)
        {
            sum = sum + mp.cost;
            if (mp.parent == null)//如果回到了初始节点
            {
                break;
            }
            mp = mp.parent;
        }
        this.G = sum;
    }
    public List<MyPoint> direction() //生成当前点的八向坐标
    {

        List<MyPoint> direction_8 = new List<MyPoint>();
        foreach (var t in map.Instance.ALLcorridorAndRoomCube)
        {
            MyPoint points = new MyPoint();
            if (t.transform.position == new Vector3(this.x, this.y + 1, 0))
            {
                points.x = this.x;
                points.y = this.y + 1;
                points.cost = 10;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x, this.y - 1, 0))
            {
                points.x = this.x;
                points.y = this.y - 1;
                points.cost = 10;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x + 1, this.y, 0))
            {
                points.x = this.x + 1;
                points.y = this.y;
                points.cost = 10;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x - 1, this.y, 0))
            {
                points.x = this.x - 1;
                points.y = this.y;
                points.cost = 10;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x + 1, this.y+1, 0))
            {
                points.x = this.x + 1;
                points.y = this.y + 1;
                points.cost = 14;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x - 1, this.y + 1, 0))
            {
                points.x = this.x - 1;
                points.y = this.y + 1;
                points.cost = 14;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x + 1, this.y - 1, 0))
            {
                points.x = this.x + 1;
                points.y = this.y - 1;
                points.cost = 14;
                direction_8.Add(points);
            }
            else if (t.transform.position == new Vector3(this.x - 1, this.y - 1, 0))
            {
                points.x = this.x - 1;
                points.y = this.y - 1;
                points.cost = 14;
                direction_8.Add(points);
            }
            
        }
        
        return direction_8;
     }
}


public class Astar : MonoBehaviour {

    public GameObject target;//目标点
    public bool flage;
    public List<MyPoint> Open_List = new List<MyPoint>();
    public List<MyPoint> Close_list = new List<MyPoint>();
    public List<GameObject> temmmp = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown("t"))
        {
            initPoint();
            huizhilujing();
           // TIANCHAONG();
        }
        if (Input.GetKeyDown("g"))
        {
            flage = true;
        }

        if (flage == true)
        {
            zougezi();
        }

    }
    void zougezi()
    {
        if (temmmp.Count < 1)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, temmmp[temmmp.Count - 1].transform.position, 0.5f);
        temmmp.Remove(temmmp[temmmp.Count - 1]);

    }

    void TIANCHAONG()
    {
        foreach (var T in Open_List)
        {
            int index = Random.Range(0, 4);
            GameObject go = GameObject.Instantiate(map.Instance.wall[index], new Vector3(T.x, T.y, 0), Quaternion.identity);
        }
    }
    void huizhilujing()
    {
        MyPoint a = new MyPoint();
        //a = Open_List[Open_List.Count - 1];
        a = Close_list[Close_list.Count - 1];
        while(true)
        {
            delCube(a);
            a = a.parent;
            if (a == null)
            {
                break;
            }
        }
    }
    void delCube(MyPoint point)
    {
        Vector3 xy = new Vector2();
        xy.x = point.x;
        xy.y = point.y;
        xy.z = 0;
        GameObject go = GameObject.Instantiate(map.Instance.BLACK[0], new Vector3(xy.x, xy.y, 0), Quaternion.identity);
        temmmp.Add(go);
    }
    void initPoint()
    {
        MyPoint point = new MyPoint();//生成起始节点
        point.x = (int)(transform.position.x);
        point.y = (int)(transform.position.y);

        point.SEEKfgh(target);//计算这些点的 F G H
        Open_List.Add(point);
        Close_list.Add(point);

        while (!(Close_list[Close_list.Count - 1].x == (int)(target.transform.position.x) && Close_list[Close_list.Count - 1].y == (int)(target.transform.position.y)))
        {
            startFind();
        }
       
    }
    bool CONTAINS(List<MyPoint> potits, MyPoint point)
    {
        foreach (var t in potits)
        {
            if (t.x == point.x && t.y == point.y)
            {
                return true;
            }
        }
        return false;
    }


    void startFind()
    {
        foreach (var t in Close_list[Close_list.Count - 1].direction())
        {
            if (CONTAINS(Close_list,t))
            {
                continue;
            }
            else
            {
                t.SEEKfgh(target);//计算这些点的 F G H
                if (CONTAINS(Open_List,t) == false)  //
                {
                    Open_List.Add(t);//把当前点加进OPEN表里  
                    print("asdadasdasdasdasd");
                    t.parent = Close_list[Close_list.Count - 1];// 设置父节点
                }
                else if (CONTAINS(Open_List,t))
                {
                    print("asdadasdasda234234234234234sdasd");
                    if ((Close_list[Close_list.Count - 2].G + t.cost) < Close_list[Close_list.Count - 1].G)
                    {
                        Close_list[Close_list.Count - 1] = t;
                        t.parent = Close_list[Close_list.Count - 2];
                        t.SEEKfgh(target);//计算这些点的 F G H
                    }
                }
            } 
        }
        //sortOpenList();//进行一次排序
        InsertSortOpenList();
        Close_list.Add(Open_List[1]);
        Open_List.Remove(Open_List[1]);//把F最小的点放近Close_list里
    }

    void InsertSortOpenList()//插入排序
    {
        for (int i = 1; i < Open_List.Count; i++)
        {
            MyPoint temp = Open_List[i];
            int j;
            for (j = i - 1; j >= 0 && temp.F < Open_List[j].F; j--)
            {
                Open_List[j + 1] = Open_List[j];   //将较大元素后移
        }

            Open_List[j + 1] = temp;        //temp插入正确的位置

        }
    }


    void sortOpenList()//冒泡排序
    {
        int isSorted;
        for (int i = 0; i < Open_List.Count - 1; i++)
        {
            isSorted = 1;//假设剩下的元素已经排序好
            for (int n = 0; n < Open_List.Count - 1 - i; n++)
            {
                if (Open_List[n].F > Open_List[n + 1].F)
                {
                    MyPoint point = new MyPoint();
                    point = Open_List[n];
                    Open_List[n] = Open_List[n + 1];
                    Open_List[n + 1] = point;
                    isSorted = 0;//发生交换的话就说明还没排序好
                    print(Open_List.Count);
                }
            }
            if (isSorted == 1)//如果已经排序好了 则直接交换
            {
                break;
            }
        }
    }
}
