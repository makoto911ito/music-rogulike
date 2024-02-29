using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackObjController : MonoBehaviour
{
    List<GameObject> _attackObjList = new List<GameObject>();
    int _count;

    public void Generate(GameObject gameObject ,string direction , bool judg , PlayerPresenter playerPresenter , int posX , int posZ)
    {
        _count++;
        var _aobj = Instantiate(gameObject,new Vector3(posX,1,posZ),Quaternion.Euler(90,0,0));
        _aobj.name = _count.ToString();
        var _eAObjCs = _aobj.GetComponent<EnemyAttackObj>();
        if(playerPresenter == null)
        {
            Debug.Log("“ü‚Á‚Ä‚¢‚È‚¢");
        }
        _eAObjCs.ThisInit(direction, judg,posX,posZ,playerPresenter,this);
        _attackObjList.Add(_aobj);
    }

     public void GoAObj()
    {
        for (var i = 0; i < _attackObjList.Count; i++)
        {
            //Debug.Log("¶¬‚³‚ê‚Ä‚¢‚é“G‚É–½—ß‚ð‚µ‚Ä‚¢‚é");
            EnemyAttackObj _enemyAObj = _attackObjList[i].GetComponent<EnemyAttackObj>();
            _enemyAObj.GoObj();
        }
    }

    public void DestroyAObj(GameObject aobj)
    {
        for (var i = 0; i < _attackObjList.Count; i++)
        {
            if (_attackObjList[i].name == aobj.name)
            {
                Destroy(_attackObjList[i]);
                _attackObjList.Remove(_attackObjList[i]);
            }
        }
    }
}
