using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Skrypt odpowiada za usatwienie rozdzielczosci kemerze
/// </summary>
public class CameraResolution : MonoBehaviour
{


    #region Pola
    private int ScreenSizeX = 1080;
    private int ScreenSizeY = 1920;
    #endregion

    #region metody

    #region rescale camera
    private void RescaleCamera()
    {

        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;

        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = Camera.main;

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }
    #endregion

    #endregion

    #region metody unity

    void OnGUI()
    {
        // 화면 비율에 맞추어 검정색 박스를 그립니다.
        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        if (scaleheight < 1.0f)
        {
            float barThickness = (1.0f - scaleheight) * Screen.height / 2.0f;
            // 위와 아래에 검정색 박스를 그립니다.
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, barThickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(0, Screen.height - barThickness, Screen.width, barThickness), Texture2D.whiteTexture);
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
            float barThickness = (1.0f - scalewidth) * Screen.width / 2.0f;
            // 좌우에 검정색 박스를 그립니다.
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(0, 0, barThickness, Screen.height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(Screen.width - barThickness, 0, barThickness, Screen.height), Texture2D.whiteTexture);
        }
    }

    // Use this for initialization
    void Awake()
    {
        RescaleCamera();
    }

    #endregion
}