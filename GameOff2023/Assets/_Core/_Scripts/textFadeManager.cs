using TMPro;
using UnityEngine;


public class CurrencyGainText : MonoBehaviour
{
    public TextMeshPro currencyText;
    public SpriteRenderer currencyIcon;
    public float fadeSpeed = 1.0f;
    public float moveSpeed = 1.0f;
    public float displayTime = 2.0f;

    private float timer;

    void Start()
    {
        // Set initial values and make the text invisible
        currencyText.color = new Color(currencyText.color.r, currencyText.color.g, currencyText.color.b, 1f);
        currencyIcon.color = new Color(currencyIcon.color.r, currencyIcon.color.g, currencyIcon.color.b, 1f);
        timer = displayTime;
    }

    void Update()
    {
        // Move the text upwards
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade in
        float alpha = Mathf.Lerp(1f, 0f, (displayTime - timer) / displayTime);
        currencyText.color = new Color(currencyText.color.r, currencyText.color.g, currencyText.color.b, alpha);
        currencyIcon.color = new Color(currencyIcon.color.r, currencyIcon.color.g, currencyIcon.color.b, alpha);

        // Timer to control how long the text is displayed
        timer -= Time.deltaTime;
        if (timer <= 1f)
        {
            Destroy(gameObject);
        }
    }

    public void displayInfo(Sprite sprite, string text)
    {
        currencyIcon.sprite = sprite;
        currencyText.text = text;
    }
}
