using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class Character : MonoBehaviour {

  public struct State {
    // action
    public float walk;
    public bool isOnceJump;
    public bool isDown;
    // collision
    public bool isGround;
    public bool isAtack;
    public bool isPlayer;
    public bool isEnemy;
  }
  public struct Force {
    public float walk;
    public float jump;
    [Range(0, 1)]
    public float sliding; 
  }
  protected struct Mask {
    public LayerMask ground;
    public LayerMask player;
    public LayerMask enemy ;
  }
  public struct ObjData {
    public Vector2 size;
    public Vector2 b_offset;
    public Vector2 c_offset;
    public float   radius;
  }

  public    State       state = new State();
  public    Force       force = new Force();
  public    ObjData     data  = new ObjData();
  protected Mask        mask  = new Mask();
  public    GameObject  controller;
  private const float   PERMIT_AXIS_STATE = 0.2f;

  public void AwakeCharactor() {
    state.walk = 0f;
    state.isOnceJump = false;
    state.isDown = false;
    state.isGround = false;
    state.isAtack = false;
    state.isPlayer = false;
  }

  // Use this for initialization
  public void StartCharactor() {
    controller    = GameObject.FindWithTag("GameController");

    mask.ground   = LayerMask.GetMask("Land");
    mask.player   = LayerMask.GetMask("Player");
    mask.enemy    = LayerMask.GetMask("Enemy");

    data.size     = GetComponent<BoxCollider2D>().size;
    data.b_offset = GetComponent<BoxCollider2D>().offset;
    data.c_offset = GetComponent<CircleCollider2D>().offset;
    data.radius   = GetComponent<CircleCollider2D>().radius;
  }

  //void OnDrawGizmos() {
  //  Vector2 size = GetComponent<BoxCollider2D>().size;
  //  Vector2 b_offset = GetComponent<BoxCollider2D>().offset;
  //  Vector2 c_offset = GetComponent<CircleCollider2D>().offset;
  //  float radius = GetComponent<CircleCollider2D>().radius;

  //  Vector2 pos = transform.position;
  //  Vector2 scale = transform.localScale;

  //  Gizmos.color = new Color(1, 0, 0, 0.5F);
  //  Vector2 center = new Vector2(pos.x + b_offset.x, pos.y + ( c_offset.y - radius * 1.1f) * scale.y);
  //  Vector2 area = new Vector2(size.x * scale.x, radius * scale.y * 0.2f);
  //  Gizmos.DrawCube(center, area);
  //}

  public void GetCollision() {
    Vector2 pos = transform.position;
    Vector2 scale = transform.localScale;

    Vector2 upsideCenter = new Vector2(pos.x + data.b_offset.x, pos.y - (-data.c_offset.y - data.radius * 1.1f) * scale.y);
    Vector2 groundCenter = new Vector2(pos.x + data.b_offset.x, pos.y + (data.c_offset.y - data.radius * 1.1f) * scale.y);
    Vector2 groundArea   = new Vector2(data.size.x * scale.x * 0.5f, data.radius * scale.y * 0.1f);
    Vector2 leftCenter   = new Vector2(pos.x + (-data.c_offset.x - data.radius * 1.1f) * scale.x, pos.y + data.b_offset.y);
    Vector2 rightCenter  = new Vector2(pos.x - (data.c_offset.x - data.radius * 1.1f) * scale.x, pos.y + data.b_offset.y);
    Vector2 sideArea     = new Vector2(scale.x * data.radius * 0.1f, data.size.y * scale.y * 0.5f);

    state.isGround = Physics2D.OverlapArea(groundCenter + groundArea, groundCenter - groundArea, mask.ground);
    state.isPlayer = Physics2D.OverlapArea(upsideCenter + groundArea, upsideCenter - groundArea, mask.player);
    state.isEnemy  = Physics2D.OverlapArea(upsideCenter + groundArea, upsideCenter - groundArea, mask.enemy )
                  || Physics2D.OverlapArea(rightCenter  + sideArea  , rightCenter  - sideArea  , mask.enemy )
                  || Physics2D.OverlapArea(leftCenter   + sideArea  , leftCenter   - sideArea  , mask.enemy );
    state.isAtack  = Physics2D.OverlapArea(rightCenter  + sideArea  , rightCenter  - sideArea  , mask.ground)
                  || Physics2D.OverlapArea(leftCenter   + sideArea  , leftCenter   - sideArea  , mask.ground);
  }

  //--- ACTION ---//
  public void ActWalk() {
    Vector2 prev_v = GetComponent<Rigidbody2D>().velocity;
    Vector2 curr_v = prev_v;

    curr_v.x = state.walk * force.walk;
    transform.localScale = new Vector2(Mathf.Sign(state.walk) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

    GetComponent<Rigidbody2D>().velocity = curr_v;
  }

  public void ActStand() {
    Vector2 prev_v = GetComponent<Rigidbody2D>().velocity;
    Vector2 curr_v = prev_v;

    // slidingは物理演算でできそう
    curr_v.x = prev_v.x * force.sliding;
    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);

    GetComponent<Rigidbody2D>().velocity = curr_v;
  }

  public void ActJump() {
    GetComponent<Rigidbody2D>().AddForce(Vector2.up * force.jump);
    state.isOnceJump = false;
  }
  //------//

  //--- abstract関数 ---//
  protected abstract void ChangeSprite();
  //------//

  //--- static関数 ---//
  public static bool IsAxis(float value) {
      return Mathf.Abs(value) > PERMIT_AXIS_STATE;
  }
  //------//
}
