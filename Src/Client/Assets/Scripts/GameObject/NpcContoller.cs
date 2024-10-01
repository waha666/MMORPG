using Common.Data;
using Managers;
using Models;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class NpcContoller : MonoBehaviour {

	public int npcID;
    Animator anim;
	SkinnedMeshRenderer npcRenderer;
	Color orignColor;

	NpcDefine npcDefine;
    private bool inInteractive;

	NpcQuestStatus questStatus;

    // Use this for initialization
    void Start () {
		anim = this.GetComponent<Animator>();
		npcDefine = NpcManager.Instance.GetNpcDefine(npcID);
        npcRenderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		orignColor = npcRenderer.sharedMaterial.color;
		this.StartCoroutine(Actions());

		this.RefreshNpcStatus();
		QuestManager.Instance.onQuestStatusChanged += onQuestStatusChanged;
    }

    private void RefreshNpcStatus()
    {
		this.questStatus = QuestManager.Instance.GetNpcQuestStatusByNpc(this.npcID);
		UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

	void onDestroy() 
	{
        QuestManager.Instance.onQuestStatusChanged -= onQuestStatusChanged;
		if (UIWorldElementManager.Instance != null) UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    private void onQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    IEnumerator Actions()
    {
		while (true) 
		{
			if (inInteractive)
			{
				yield return new WaitForSeconds(2f);
			}
			else 
			{
				yield return new WaitForSeconds(Random.Range(5f, 10f));
			}
			this.Relax();
		}
    }

    private void Relax()
    {
		anim.SetTrigger("Relax");
    }

    // Update is called once per frame
    void Update () {
		
	}

	void OnMouseDown() 
	{
		Debug.LogFormat("当前点击NPC：{0}", npcDefine.Name);
		Interactive();
	}

    private void Interactive()
    {
		if (!inInteractive) 
		{
			inInteractive = true;
			StartCoroutine(DoInteractive());

        }
    }

    IEnumerator DoInteractive()
    {
		yield return FaceToPlayer();
		if (NpcManager.Instance.Interactive(npcID)) 
		{
			anim.SetTrigger("Talk");
		}
		yield return new WaitForSeconds(3f);
		inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
		Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.gameObject.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo))>5)
        {
			this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
			yield return null;
        }
    }

	private void OnMouseOver() 
	{
		this.Highlighter(true);
	}
    private void OnMouseEnter()
    {
        this.Highlighter(true);
    }
    private void OnMouseExit()
    {
        this.Highlighter(false);
    }

    private void Highlighter(bool highlight)
    {
		if (highlight)
		{
			if (npcRenderer.sharedMaterial.color != Color.white) 
			{
                npcRenderer.sharedMaterial.color = Color.white;
            }
		}
		else 
		{
            if (npcRenderer.sharedMaterial.color != orignColor)
            {
                npcRenderer.sharedMaterial.color = orignColor;
            }
        }
    }
}
