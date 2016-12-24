using UnityEngine;
using System.Collections;

public class AnimController : MonoBehaviour {
	
  public bool IsState(Animator anim, string state) {
    AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
    Debug.Log(Animator.StringToHash(state));
    return stateInfo.fullPathHash == Animator.StringToHash(state);
  }
}
