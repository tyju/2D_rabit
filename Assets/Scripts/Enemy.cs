using UnityEngine;
using System.Collections;

public class Enemy : Character {
  Character chara;
  public float walk;
  [Range(0, 1)] public float sliding;
  Animator  anim;

  void Awake () {
    AwakeCharactor();
    force.walk    = walk;
    force.sliding = sliding;
    force.jump    = 0f;
    state.walk    = 1.0f;
  }

  // Use this for initialization
  void Start () {
    StartCharactor();
    anim = GetComponent<Animator>();
	}

  void FixedUpdate () {
    ActEnemy();
  }
	
	// Update is called once per frame
	void Update () {
    GetCollision();
    GetSprite();
    ChangeSprite();
    ChangeAction();

    Debug.Log(controller.GetComponent<AnimController>().IsState(anim, "Enemy_Oven_Walk"));
	}

  void ChangeAction() {
    if (state.isPlayer) {
      state.isDown = true;
    }
  }

  void ActEnemy() {
    if (state.isAtack) {
      state.walk = -state.walk;
    }
    if (state.isPlayer) {
      state.isDown = true;
    }
  }

  void GetSprite() {
    Animator anim = GetComponent<Animator>();
  }

  protected override void ChangeSprite() {
    GetComponent<Animator>().SetBool("IsDown", state.isDown);
  }
}
