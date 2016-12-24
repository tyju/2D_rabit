using UnityEngine;
using System.Collections;

public class Player : Character {
  public float walk;
  public float jump;
  [Range(0, 1)]
  public float sliding;

  void Awake() {
    AwakeCharactor();
  }

  // Use this for initialization
  void Start() {
    StartCharactor();
    force.walk    = walk;
    force.jump    = jump;
    force.sliding = sliding;
  }

  void FixedUpdate() {
    ActPlayer();
  }

  // Update is called once per frame
  void Update() {
    GetCollision();
    GetInput();
    ChangeSprite();
    ChangeAction();
  }

  void GetInput() {
    state.walk = Input.GetAxis("Horizontal");
    if (state.isGround) {
      state.isOnceJump = Input.GetButtonDown("Jump");
    }
  }

  void ChangeAction() {
    if (state.isEnemy) {
      state.isDown = true;
      controller.SendMessage("Reset");
    }
  }

  protected override void ChangeSprite() {
    GetComponent<Animator>().SetBool("IsGround", state.isGround);
    GetComponent<Animator>().SetBool("IsWalk"  , IsAxis(state.walk));
  }

  void ActPlayer() {
    // 衝突中に横移動すると重力に勝ってしまう
    if (IsAxis(state.walk) && state.isAtack == false) {
      ActWalk();
    } else {
      ActStand();
    }

    if (state.isOnceJump) {
      ActJump();
    }
  }
}