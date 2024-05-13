using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject clearText;
    int[,] map; //変更。二次元配列で宣言  レベルデザイン用の配列
    GameObject[,] field; //ゲーム管理用の配列


    /*void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ", ";
        }
        Debug.Log(debugText);
    }*/
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++){
            for (int x = 0; x< field.GetLength(1); x++){
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        
        return new Vector2Int(-1, -1);
    }
    // Start is called before the first frame update
    void Start()
    {
        //追加 確認したら削除
        //GameObject intstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
        map = new int[,]{  // 1をプレイヤー 2を箱 3を格納場所とする
            {0, 0, 0, 0, 0 },
            {0, 3, 1, 3, 0 },
            {0, 0, 2, 0, 0 },
            {0, 2, 3, 2, 0 },
            {0, 0, 0, 0, 0 },
        };
        string debugText = "";
        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)  
            ];


        //変更。二重for文で二次元配列の情報を出力
        for(int y = 0; y< map.GetLength(0); y++){
            for(int x = 0; x< map.GetLength(1); x++){
                debugText += map[y, x].ToString() + ",";
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0.01f), Quaternion.identity);
                }
            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);
    }

    // moveFromおよびmoveToをVector2Int型で受け取る
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        playerPrefab.GetComponent<GameObject>();

        //移動先が範囲外なら移動不可
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        //Boxタグを持っていたら再帰処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box"){
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        /*if (field[moveTo.y, moveTo.x] == 2)
        {
            //どの方向へ移動するかを算出
            int velocity = moveTo - moveFrom;
            //プレイy－の移動先から、さらに先へ2(箱)を移動させる。
            //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを呼び、処理が再起している。移動可不可をboolで記録
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success) { return false; }
        }*/

        Vector3 moveToPosition = new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().moveTo(moveToPosition);

        //GameObjectの座標(position)を移動させてからインデックスの入れ替え
                    field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
                    field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
                    field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        // Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++){
            for (int x = 0; x < map.GetLength(1); x++){

                // 格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    // 格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }

            }

        }

        // 要素数はgoals.Countで取得
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                // 一つでも箱がなかったら条件未達成
                return false;
            }
        }
        // 条件未達成でなければ条件達成
        return true;
    }

    


    //// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
           ////メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
           // //PrintArray();

            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(-1, 0));
            //PrintArray();

            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            //メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, -1));
            //PrintArray();

            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //メソッド化した処理を使用
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));
            //PrintArray();

            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
    }
}
