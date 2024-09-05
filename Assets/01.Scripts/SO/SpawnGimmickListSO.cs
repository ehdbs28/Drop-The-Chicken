using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SpawnGimmickList")]
public class SpawnGimmickListSO : ScriptableObject
{
    public List<Gimmick> GimmickList;

    private Dictionary<EGimmickType, Gimmick> GimmickDictionary;

    private void OnEnable()
    {
        if(GimmickDictionary == null) 
        {
            GimmickDictionary = new Dictionary<EGimmickType, Gimmick>();
               
            if(GimmickList.Count > 0)
            {
                GimmickList.ForEach((gimmick) =>
                {
                    GimmickDictionary.Add(gimmick.GimmickType, gimmick); 
                });
            }
        }
    }

    public string GetGimmickObjectName(in EGimmickType type)
    {
        Gimmick obj = null;
        Debug.Log(GimmickDictionary);
        if(GimmickDictionary.TryGetValue(type, out obj))
        {
            return obj.name;
        }
        return null;
    }
}
