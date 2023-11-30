using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="PosX">‚»‚Ì“G‚ÌxÀ•W‚Ìî•ñ</param>
    /// <param name="PosZ">‚»‚Ì“G‚ÌzÀ•W‚Ìî•ñ</param>
    public void PlayerCheck(int PosX, int PosZ)
    {
        for (int i = PosX - 5; i <= PosX + 5; i++)
        {
            for (int j = PosZ - 5; j <= PosZ + 5; j++)
            {
                var areaController = MapManager._areas[i, j].GetComponent<AreaController>();

                if (areaController._onPlayer == true)
                {
                    Move.GoMove(i, j);

                    break;
                }
            }
        }
    }
}


class Move : MonoBehaviour
{
    public static void GoMove(int x, int z)
    {

    }
}