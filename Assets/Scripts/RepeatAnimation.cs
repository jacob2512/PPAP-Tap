using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RepeatAnimation : MonoBehaviour 
{
  [SerializeField] private Sprite _initialSprite;
  [SerializeField] private Sprite _alternateSprite;

  [SerializeField] private float _startTime;

  [SerializeField] private float _repeatInterval;

  private SpriteRenderer _spriteRenderer;
  private bool hasInitialSprite;
 
  private void Start()
  {
    CancelInvoke("Animate");
    _spriteRenderer = GetComponent<SpriteRenderer>();

	  hasInitialSprite = true;    
    RepeatAnim(_startTime, _repeatInterval);
  }

  public void RepeatAnim(float startTime, float repeatInterval)
  {
    //print("repeat anim "+ startTime+" "+repeatInterval);
    InvokeRepeating("Animate", startTime, repeatInterval);
  }

  private void Animate()
  {
    if(hasInitialSprite) 
	{
	  _spriteRenderer.sprite = _alternateSprite;
	  hasInitialSprite = false;
	}
   else
   {
	  _spriteRenderer.sprite = _initialSprite;
	  hasInitialSprite = true;
   }
  }

}