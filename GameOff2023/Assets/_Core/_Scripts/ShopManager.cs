using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Panel;
  public void OpenPanel()
    {
        if(Panel != null)
        {
            bool isActive = Panel.activeSelf;

            Panel.SetActive(!isActive);
        }
    }
}
