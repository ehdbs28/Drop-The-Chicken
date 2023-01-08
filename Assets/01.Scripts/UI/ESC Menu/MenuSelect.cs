using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelect : MonoBehaviour
{
    private MenuObject[] _objects;
    private Transform _cursor;

    private int _index = 0;

    private void Awake() {
        _objects = GetComponentsInChildren<MenuObject>();
        _cursor = transform.Find("Cursor");
    }

    private void Update() {
        SelectObject();
        ClickObject();
    }

    private void SelectObject(){
        float input = Input.GetAxisRaw("Vertical");

        switch(input){
            case 1: // Press Up Key
                if(_index - 1 > 0){
                    _index--;
                }
                break;
            case -1: // Press Down Key
                if(_index + 1 < _objects.Length){
                    _index++;
                }
                break;
        }

        _cursor.position = new Vector2(_cursor.position.x, _objects[_index].ObjectPosition);
    }

    private void ClickObject(){
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
            _objects[_index].ObjectEvent();
        }
    }
}
