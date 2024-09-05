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
        // ī�޶� �����ϴ��� Ȯ��
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Main camera is not found.");
            return; // ī�޶� ������ ����
        }

        // 9:16 ����
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // ī�޶� Orthographic�� ��� ó��
        if (camera.orthographic)
        {
            // �⺻ Orthographic Size ���� (�ʿ信 ���� ���� ����)
            float defaultOrthoSize = 5.0f;

            if (scaleHeight < 1.0f)
            {
                // ȭ�� ���̿� ���� ����
                camera.orthographicSize = defaultOrthoSize / scaleHeight;
            }
            else
            {
                // ȭ�� ���� ���� ���� (���͹ڽ��� �¿쿡 ������)
                camera.orthographicSize = defaultOrthoSize;
            }
        }

        // ���� ȭ�� ũ�� ����
        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }


    #endregion

    #endregion

    #region metody unity

    void OnGUI()
    {
        // ȭ�� ������ ���߾� ������ �ڽ��� �׸��ϴ�.
        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        if (scaleheight < 1.0f)
        {
            float barThickness = (1.0f - scaleheight) * Screen.height / 2.0f;
            // ���� �Ʒ��� ������ �ڽ��� �׸��ϴ�.
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, barThickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(0, Screen.height - barThickness, Screen.width, barThickness), Texture2D.whiteTexture);
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
            float barThickness = (1.0f - scalewidth) * Screen.width / 2.0f;
            // �¿쿡 ������ �ڽ��� �׸��ϴ�.
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
        // �ػ� ��ȭ ����
        RescaleCamera();
    }

    #endregion
}