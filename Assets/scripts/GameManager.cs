using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform text1;
    public Transform text2;
    public SpriteRenderer image1;
    public SpriteRenderer image2;

    private float initialText1X, initialText2X;
    private bool isSwapped;

    void Start()
    {
        initialText1X = text1.position.x;
        initialText2X = text2.position.x;
        UpdateImages();
    }

    public void TextsPossiblySwapped()
    {
        bool originalOrder = initialText1X < initialText2X;
        bool currentOrder = text1.position.x < text2.position.x;

        bool nowSwapped = (currentOrder != originalOrder);

        if (nowSwapped != isSwapped)
        {
            isSwapped = nowSwapped;
            UpdateImages();
        }
    }

    private void UpdateImages()
    {
        image1.enabled = !isSwapped;
        image2.enabled = isSwapped;
    }
}
