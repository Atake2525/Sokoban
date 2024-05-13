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
    int[,] map; //�ύX�B�񎟌��z��Ő錾  ���x���f�U�C���p�̔z��
    GameObject[,] field; //�Q�[���Ǘ��p�̔z��


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
        //�ǉ� �m�F������폜
        //GameObject intstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
        map = new int[,]{  // 1���v���C���[ 2�� 3���i�[�ꏊ�Ƃ���
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


        //�ύX�B��dfor���œ񎟌��z��̏����o��
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
            debugText += "\n";//���s
        }
        Debug.Log(debugText);
    }

    // moveFrom�����moveTo��Vector2Int�^�Ŏ󂯎��
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        playerPrefab.GetComponent<GameObject>();

        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        //Box�^�O�������Ă�����ċA����
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box"){
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        /*if (field[moveTo.y, moveTo.x] == 2)
        {
            //�ǂ̕����ֈړ����邩���Z�o
            int velocity = moveTo - moveFrom;
            //�v���Cy�|�̈ړ��悩��A����ɐ��2(��)���ړ�������B
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //���������ړ����s������A�v���C���[�̈ړ������s
            if (!success) { return false; }
        }*/

        Vector3 moveToPosition = new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().moveTo(moveToPosition);

        //GameObject�̍��W(position)���ړ������Ă���C���f�b�N�X�̓���ւ�
                    field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
                    field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
                    field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        // Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++){
            for (int x = 0; x < map.GetLength(1); x++){

                // �i�[�ꏊ���ۂ��𔻒f
                if (map[y, x] == 3)
                {
                    // �i�[�ꏊ�̃C���f�b�N�X���T���Ă���
                    goals.Add(new Vector2Int(x, y));
                }

            }

        }

        // �v�f����goals.Count�Ŏ擾
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                // ��ł������Ȃ�������������B��
                return false;
            }
        }
        // �������B���łȂ���Ώ����B��
        return true;
    }

    


    //// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
           ////���\�b�h�������������g�p
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
           // //PrintArray();

            // �����N���A���Ă�����
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //���\�b�h�������������g�p
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(-1, 0));
            //PrintArray();

            // �����N���A���Ă�����
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            //���\�b�h�������������g�p
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, -1));
            //PrintArray();

            // �����N���A���Ă�����
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //���\�b�h�������������g�p
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));
            //PrintArray();

            // �����N���A���Ă�����
            if (IsCleard())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }
        }
    }
}
