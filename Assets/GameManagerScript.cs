using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[] map;
    void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ", ";
        }
        Debug.Log(debugText);
    }

    int GetPlayerIndex()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }
    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };
        PrintArray();
    }

    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo < 0 || moveTo >= map.Length) { return false; }
        //�ړ����2(��)��������
        if (map[moveTo] == 2)
        {
            //�ǂ̕����ֈړ����邩���Z�o
            int velocity = moveTo - moveFrom;
            //�v���Cy�|�̈ړ��悩��A����ɐ��2(��)���ړ�������B
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //���������ړ����s������A�v���C���[�̈ړ������s
            if (!success) { return false; }
        }
        //�v���C���[�E���ւ�炸�̈ړ�����
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //���\�b�h�������������g�p
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //���\�b�h�������������g�p
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
    }
}
