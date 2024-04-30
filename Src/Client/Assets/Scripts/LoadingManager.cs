using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using SkillBridge.Message;
using ProtoBuf;
using Common;
using Services;

public class LoadingManager : MonoBehaviour {

    public GameObject UITips;
    public GameObject UIBackground;
    public GameObject UIloadBG;
    public GameObject UILoading;
    public GameObject UILogin;
    public GameObject UIregister;

    public Text barText;
    public Slider progressBar;
    public Sprite[] sprites;

    // Use this for initialization
    IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        UITips.SetActive(true);
        UILoading.SetActive(false);
        UILogin.SetActive(false);
        yield return new WaitForSeconds(2f);
        UILoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);

        yield return DataManager.Instance.LoadData();

        //Init basic services
        MapService.Instance.Init();
        UserService.Instance.Init();


        // Fake Loading Simulate
        for (float i = 50; i < 100;)
        {
            i += Random.Range(0.1f, 1.5f);
            progressBar.value = i;
            barText.text = ((int)i).ToString() +  "%";
            yield return new WaitForEndOfFrame();
        }

        UILoading.SetActive(false);
        UILogin.SetActive(true);
        UIloadBG.SetActive(true);
        UIBackground.GetComponent<Image>().sprite = sprites[0];

        yield return null;
    }

    public void changeBG(int index)
    {
        UIBackground.GetComponent<Image>().sprite = sprites[index];
    }

    public void RegisterSuccess()
    {
        UILogin.SetActive(true);
        UIregister.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

    }
}
