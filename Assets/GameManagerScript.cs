using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
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
        map = new int[,]{
            {0, 0, 0, 0, 0 },
            {0, 0, 1, 0, 0 },
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
            }
            debugText += "\n";//���s
        }
        Debug.Log(debugText);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        //Box�^�O�������Ă�����ċA����
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box"){
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
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
        //GameObject�̍��W(position)���ړ������Ă���C���f�b�N�X�̓���ւ�
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    
    

    


    //// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
           //���\�b�h�������������g�p
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(tag, playerIndex, playerIndex);
            //PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //���\�b�h�������������g�p
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(tag, playerIndex, playerIndex);
           //PrintArray();
        }
    }
}
