using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {

	public delegate void CloseHandler(UIBase sender, BaseResult result);
	public event CloseHandler OnClose;
	public virtual System.Type Type { get { return this.GetType(); } }

	public enum BaseResult 
	{
		None = 0,
		Yes,
		No,
	}

	public void Close(BaseResult result = BaseResult.None) 
	{
		UIManager.Instance.Close(this.Type);
		if (this.OnClose != null)
			this.OnClose(this, result);
		this.OnClose = null;
    }

	public virtual void OnCloseClick() 
	{
		this.Close();
	}

	public virtual void OnYesClick() 
	{
        this.Close(BaseResult.Yes);
    }

    public virtual void OnNoClick()
    {
        this.Close(BaseResult.No);
    }
}
