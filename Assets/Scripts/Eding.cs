using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class Eding : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private CanvasGroup endText;
    [SerializeField] private float gravityChangeSpeed;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float endTextSpeed;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Ending());
        }
    }

    public IEnumerator Ending()
    {
        float target = playerRB.gravityScale *= -1f;
        while (playerRB.gravityScale != target)
        {
            playerRB.gravityScale = Mathf.MoveTowards(playerRB.gravityScale, target, Time.deltaTime * gravityChangeSpeed);
            yield return null;
        }

        while (fade.alpha != 1)
        {
            fade.alpha = Mathf.MoveTowards(fade.alpha, 1.0f, Time.deltaTime * fadeSpeed);
            yield return null;
        }

        while (endText.alpha != 1)
        {
            endText.alpha = Mathf.MoveTowards(endText.alpha, 1.0f, Time.deltaTime * endTextSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("Title");
    }
}
