using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Managers
{
    public enum NpcQuestStatus
    {
        None = 0,//无任务
        Complete = 1,//已完成
        Available,//拥有可接受
        Incomplete,//拥有未完成
    }

    public class QuestManager : Singleton<QuestManager>
    {
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public delegate void onQuestStatusChangedHadler(Quest quest);
        public event onQuestStatusChangedHadler onQuestStatusChanged;

        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;

            this.allQuests.Clear();
            this.npcQuests.Clear();

            this.InitQuests();
        }

        void InitQuests()
        {
            //初始化可用任务
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                //this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                //this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.allQuests[quest.Info.QuestId] = quest;
            }

            this.CheckAvailableQuest();

            foreach (var kv in this.allQuests)
            {
                Debug.LogFormat("=========Quests:{0} {1}", kv.Key, kv.Value);
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
        }

        //初始化可接任务
        void CheckAvailableQuest() 
        {
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class) continue;
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level) continue;
                if (this.allQuests.ContainsKey(kv.Key)) continue;
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        if (preQuest.Info == null) continue;
                        if (preQuest.Info.Status != QuestStatus.Finished) continue;
                    }
                    else
                        continue;
                }
                Quest quest = new Quest(kv.Value);
                //this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                //this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.allQuests[kv.Key] = quest;
            }
        }

        private void AddNpcQuest(int npcID, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcID))
                this.npcQuests[npcID] = new Dictionary<NpcQuestStatus, List<Quest>>();

            List<Quest> availables;
            List<Quest> complates;
            List<Quest> incomplates;

            if (!this.npcQuests[npcID].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcID][NpcQuestStatus.Available] = availables;
            }
            if (!this.npcQuests[npcID].TryGetValue(NpcQuestStatus.Complete, out complates))
            {
                complates = new List<Quest>();
                this.npcQuests[npcID][NpcQuestStatus.Complete] = complates;
            }
            if (!this.npcQuests[npcID].TryGetValue(NpcQuestStatus.Incomplete, out incomplates))
            {
                incomplates = new List<Quest>();
                this.npcQuests[npcID][NpcQuestStatus.Incomplete] = incomplates;
            }

            if (quest.Info == null)
            {
                if (npcID == quest.Define.AcceptNPC && !this.npcQuests[npcID][NpcQuestStatus.Available].Contains(quest))
                {
                    this.npcQuests[npcID][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (npcID == quest.Define.SubmitNPC && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!this.npcQuests[npcID][NpcQuestStatus.Complete].Contains(quest))
                        this.npcQuests[npcID][NpcQuestStatus.Complete].Add(quest);
                }
                if (npcID == quest.Define.SubmitNPC && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!this.npcQuests[npcID][NpcQuestStatus.Incomplete].Contains(quest))
                        this.npcQuests[npcID][NpcQuestStatus.Incomplete].Add(quest);
                }
            }
        }

        public NpcQuestStatus GetNpcQuestStatusByNpc(int npcID)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (this.npcQuests.TryGetValue(npcID, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return NpcQuestStatus.Complete;
                if (status[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcID)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (this.npcQuests.TryGetValue(npcID, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return this.showQuestDialog(status[NpcQuestStatus.Complete].First());
                if (status[NpcQuestStatus.Available].Count > 0)
                    return this.showQuestDialog(status[NpcQuestStatus.Available].First());
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return this.showQuestDialog(status[NpcQuestStatus.Incomplete].First());
            }
            return false;
        }

        bool showQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dig = UIManager.Instance.Show<UIQuestDialog>();
                dig.setQuest(quest);
                dig.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.Complated) 
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);

            }
            return true;
        }

        void OnQuestDialogClose(UIBase sender, UIBase.BaseResult result) 
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIBase.BaseResult.Yes)
            {
                if (dlg.quest.Info == null)
                {
                    QuetService.Instance.SendQuestAccept(dlg.quest);
                }
                else 
                {
                    QuetService.Instance.SendQuestSubmit(dlg.quest);
                }
            }
            else if(result == UIBase.BaseResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }

        public void OnQuestAccepted(NQuestInfo info) 
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }

        internal void OnQuestSubmit(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }

        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            this.npcQuests.Clear();
            Quest result;
            if (this.allQuests.ContainsKey(quest.QuestId))
            {
                //更新新的任务状态
                this.allQuests[quest.QuestId].Info = quest;
                result = this.allQuests[quest.QuestId];
            }
            else 
            {
                result = new Quest(quest);
                this.allQuests[quest.QuestId] = result;
            }

            this.CheckAvailableQuest();

            foreach (var kv in this.allQuests)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }

            if (onQuestStatusChanged != null) 
            {
                onQuestStatusChanged(result);
            }
            return result;
        }
    }
}

