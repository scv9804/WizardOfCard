using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;

using TMPro;

using UnityEngine.UI;

public class Sorry : MonoBehaviour
{
	public Image Fade;

    public TMP_Text Messege;
    public Button ReturnMainMenu;

    public GameObject Object;

    private void Awake()
    {
        StartCoroutine(Display());
    }

    private IEnumerator Display()
    {
        yield return new WaitUntil(() =>
        {
            return Input.anyKeyDown;
        });

        Fade.DOFade(0.5f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        Object.gameObject.SetActive(true);
    }

    public void GoToMain()
	{
        StartCoroutine(Main());

        IEnumerator Main()
        {
            Destroy(Messege.gameObject);
            Destroy(ReturnMainMenu.gameObject);

            Fade.DOFade(1.0f, 2.5f);

            yield return new WaitForSeconds(2.5f);

            //BETA.GameManager.Instance.Loading("IntroScene", BETA.GameManager.Instance.GameEnd);

            BETA.GameManager.Instance.GameEnd();

            SceneManager.LoadScene("IntroScene");

            yield return null;
        }
	}
}
