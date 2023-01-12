using UnityEngine;

[System.Serializable]
public class Skin : MonoBehaviour
{
    public int index;
    public bool achive = false;
    public Sprite[] skinImg;
    public string[] skinName;
    [TextArea] public string skinInfo;
}