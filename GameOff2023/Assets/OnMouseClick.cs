using UnityEngine;
using UnityEngine.Events;

public class OnMouseClick : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMouseClick;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _onMouseClick?.Invoke();
        }
    }
}
