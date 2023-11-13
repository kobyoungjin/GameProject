using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimationManager : MonoBehaviour
{
    private Image fader;

    private void Start()
    {
        fader = GameObject.Find("GameManager/SceneAnimation").transform.GetChild(0).GetComponent<Image>();

        fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
        fader.gameObject.SetActive(false);
    }

    // fade in out ���� �Լ�
    private IEnumerator FadeScene(string nextScene, float duration)
    {
        fader.gameObject.SetActive(true); //UI Image On

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t)); //Image(fader) ���� ����
            yield return null;
        }

        if(nextScene != "")  // �� �̸��� ������ �ٷ� ����
            SceneManager.LoadScene(nextScene); //��ȯ

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t)); //Image(fader) ���� ����
            yield return null;
        }
        fader.gameObject.SetActive(false);// UI Image Off

        //SaveScene();
    }
    //private IEnumerator InformNoSave()
    //{
    //    GameObject text = GameObject.Find("Main Canvas").transform.Find("NoSaveText").gameObject;
    //    if (text.active == false)
    //    {
    //        text.SetActive(true);
    //        yield return new WaitForSeconds(2);
    //        text.SetActive(false);
    //    }
    //}

    // ���� ������� ����
    public void SetFadeScene(string nextScene, float duration)
    {
        StartCoroutine(FadeScene(nextScene, duration));
    }
 
    //����� ������
    //public static void LoadSavedScene()
    //{
    //    if (PlayerPrefs.HasKey("SavedScene"))
    //    {
    //        int savedScene = PlayerPrefs.GetInt("SavedScene");
    //        GoScene(savedScene);
    //    }
    //    else
    //        instance.StartCoroutine(instance.InformNoSave());
    //}
    //private void saveScene() //���� �� ����
    //{
    //    int savedScene = SceneManager.GetActiveScene().buildIndex;
    //    if (savedScene > 0 && savedScene <= 2)
    //        PlayerPrefs.SetInt("SavedScene", savedScene);
    //}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            PlayerPrefs.DeleteKey("SavedScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))  // esc ������ Ÿ��Ʋ ������ (�ӽ�)
        {
            SetFadeScene("Title", 1);
        }
    }
}
