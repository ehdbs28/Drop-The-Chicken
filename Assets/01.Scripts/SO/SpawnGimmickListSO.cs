using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SpawnGimmickList")]
public class SpawnGimmickListSO : ScriptableObject
{
    public List<Gimmick> GimmickList;

    private Dictionary<EGimmickType, Gimmick> GimmickDictionary;

    private void Awake()
    {
        if(GimmickDictionary == null)
        {
            GimmickDictionary = new Dictionary<EGimmickType, Gimmick>();
                
            if(GimmickList != null)
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
        if(GimmickDictionary.TryGetValue(type, out obj))
        {
            return obj.name;
        }
        return null;
    }
}
