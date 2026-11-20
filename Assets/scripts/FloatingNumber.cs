using TMPro;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{
    public TMP_Text label;
    public float lifetime = 1.2f;
    public float riseSpeed = 1.5f;

    private float t;

    public void Show(string text, Color color, Vector3 worldPos)
    {
        label.text = text;
        label.color = color;
        label.fontStyle = FontStyles.Bold;
        label.fontSize = 6;
        transform.position = worldPos;
        t = 0f;
    }

    void Update()
    {
        t += Time.deltaTime;
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        var c = label.color;
        c.a = Mathf.Lerp(1f, 0f, t / lifetime);
        label.color = c;
        if (t >= lifetime) Destroy(gameObject);
    }
}