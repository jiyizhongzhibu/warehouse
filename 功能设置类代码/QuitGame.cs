using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // �˳���Ϸ
    public void OnQuitGame()
    {
        Application.Quit(); // �˳���Ϸ

        // �ڱ༭����ֹͣ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
