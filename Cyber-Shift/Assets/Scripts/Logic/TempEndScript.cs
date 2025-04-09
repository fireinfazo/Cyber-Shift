using UnityEngine;

public class TempEndScript : MonoBehaviour
{
    [SerializeField] private GameObject panel; 
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (panel != null)
                panel.SetActive(true);

            if (WindowsManager.Layout != null)
            {
                WindowsManager.Layout.SetEndPanelActive(true);
            }
        }
    }
}