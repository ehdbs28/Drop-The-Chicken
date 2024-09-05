using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Skrypt odpowiada za usatwienie rozdzielczosci kemerze
/// </summary>
public class CameraResolution : MonoBehaviour
{


    #region Pola
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;
    #endregion

    #region metody

    #region rescale camera
    private void RescaleCamera()
    {
        // 카메라가 존재하는지 확인
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Main camera is not found.");
            return; // 카메라가 없으면 리턴
        }

        // 9:16 비율
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // 카메라가 Orthographic인 경우 처리
        if (camera.orthographic)
        {
            // 기본 Orthographic Size 설정 (필요에 따라 수정 가능)
            float defaultOrthoSize = 5.0f;

            if (scaleHeight < 1.0f)
            {
                // 화면 높이에 맞춰 조정
                camera.orthographicSize = defaultOrthoSize / scaleHeight;
            }
            else
            {
                // 화면 폭에 맞춰 조정 (레터박스가 좌우에 생성됨)
                camera.orthographicSize = defaultOrthoSize;
            }
        }

        // 현재 화면 크기 저장
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
    void Start()
    {
        RescaleCamera();
    }

    void Update()
    {
        // 해상도 변화 감지
        RescaleCamera();
    }

    #endregion
}