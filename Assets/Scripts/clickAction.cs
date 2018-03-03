using UnityEngine;
using UnityEngine.EventSystems;

public class clickAction : MonoBehaviour, IPointerClickHandler
{
  	bool clicked;

	void Start () 
	{
		clicked = false;
	}
	
	public bool isClicked()
	{
		return clicked;    
	}

	public void setClicked(bool value)
	{
		clicked = value;
	}

	public void OnPointerClick(PointerEventData data)
	{
		clicked = true;
	}
}
