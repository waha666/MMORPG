using Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuetService : Singleton<QuetService>, IDisposable{

	public QuetService() 
	{
        MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnQuestAccept);
        MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnQuesSubmit);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(this.OnQuestAccept);
        MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(this.OnQuesSubmit);
    }

    public bool SendQuestAccept(Quest quest) 
    {
        Debug.Log("sendQuestAccept");
        NetMessage msg = new NetMessage();
        msg.Request = new NetMessageRequest();
        msg.Request.questAccept = new QuestAcceptRequest();
        msg.Request.questAccept.QuestId = quest.Define.ID;
        NetClient.Instance.SendMessage(msg);
        return true;
    }

    public bool SendQuestSubmit(Quest quest)
    {
        Debug.Log("SendQuestSubmit");
        NetMessage msg = new NetMessage();
        msg.Request = new NetMessageRequest();
        msg.Request.questSubmit = new QuestSubmitRequest();
        msg.Request.questSubmit.QuestId = quest.Define.ID;
        NetClient.Instance.SendMessage(msg);
        return true;
    }

    private void OnQuestAccept(object sender, QuestAcceptResponse message)
    {
        Debug.LogFormat("OnQuestAccept:{0},err{1}", message.Result, message.Errormsg);
        if (message.Result == Result.Success)
        {
            QuestManager.Instance.OnQuestAccepted(message.Quest);
        }
        else 
        {
            MessageBox.Show("任务接受失败", "错误", MessageBoxType.Error);
        }
    }

    private void OnQuesSubmit(object sender, QuestSubmitResponse message)
    {
        Debug.LogFormat("OnQuesSubmit:{0},err{1}", message.Result, message.Errormsg);
        if (message.Result == Result.Success)
        {
            QuestManager.Instance.OnQuestSubmit(message.Quest);
        }
        else
        {
            MessageBox.Show("任务完成失败", "错误", MessageBoxType.Error);
        }
    }

}
