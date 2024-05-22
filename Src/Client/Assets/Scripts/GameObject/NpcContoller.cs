using Common.Data;
using Managers;
using Models;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class NpcContoller : MonoBehaviour {

	public int npcID;
    Animator anim;
	SkinnedMeshRenderer renderer;
	Color orignColor;

	NpcDefine npcDefine;
    private bool inInteractive;

    // Use this for initialization
    void Start () {
		anim = this.GetComponent<Animator>();
		npcDefine = NpcManager.Instance.GetNpcDefine(npcID);
		renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		orignColor = renderer.sharedMaterial.color;
		this.StartCoroutine(Actions());
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
			if (renderer.sharedMaterial.color != Color.white) 
			{
				renderer.sharedMaterial.color = Color.white;
            }
		}
		else 
		{
            if (renderer.sharedMaterial.color != orignColor)
            {
                renderer.sharedMaterial.color = orignColor;
            }
        }
    }
}
