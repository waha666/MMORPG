using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {

    public InputField account;
    public InputField password;
    public Toggle agree;
    public Button loginBtn;

    // Use this for initialization
    void Start () {
        UserService.Instance.OnLogin = this.OnLogin;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnLogin(SkillBridge.Message.Result result, string msg) {
        if (result == SkillBridge.Message.Result.Success)
        {
            SceneManager.Instance.LoadScene("charChoose");
        }
        else
        {
            MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
        }
    }

	public void OnClickLoginBtn() {
        if (string.IsNullOrEmpty(this.account.text)) 
		{
			MessageBox.Show("请输入账号");
            return;
		}
        else if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        else if (agree.isOn == false)
        {
            MessageBox.Show("请同意用户协议后继续");
            return;
        }

        UserService.Instance.SendLogin(this.account.text, this.password.text);
    }
}
