using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction_yj : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed_yj;
    public GameManager_yj manager_yj;

    Rigidbody2D rigid_yj;
    Animator play_anim_yj;
    Vector3 dirVec_yj;
    GameObject ScanObject_yj;

    float h_yj;
    float v_yj;
    bool isHorizonMove_yj;

    void Awake()
    {
        rigid_yj = GetComponent<Rigidbody2D>();
        play_anim_yj = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        h_yj = manager_yj.isAction_yj ? 0 : Input.GetAxisRaw("Horizontal");
        v_yj = manager_yj.isAction_yj ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown_yj = manager_yj.isAction_yj ? false : Input.GetButtonDown("Horizontal");
        bool vDown_yj = manager_yj.isAction_yj ? false : Input.GetButtonDown("Vertical");
        bool hUp_yj = manager_yj.isAction_yj ? false : Input.GetButtonUp("Horizontal");
        bool vUp_yj = manager_yj.isAction_yj ? false : Input.GetButtonUp("Vertical");

        if (hDown_yj)
            isHorizonMove_yj = true;
        else if (vDown_yj)
            isHorizonMove_yj = false;
        else if (hUp_yj || vUp_yj)
            isHorizonMove_yj = h_yj != 0;

        // �ִϸ��̼�
        if (play_anim_yj.GetInteger("hAxisRaw_yj") != h_yj) {
            play_anim_yj.SetBool("isChange_yj", true);
            play_anim_yj.SetInteger("hAxisRaw_yj",(int)h_yj);
        }
        else if (play_anim_yj.GetInteger("vAxisRaw_yj") != v_yj) {
            play_anim_yj.SetBool("isChange_yj", true);
            play_anim_yj.SetInteger("vAxisRaw_yj", (int)v_yj);
        }
        else
            play_anim_yj.SetBool("isChange_yj", false);

        // direction
        if(vDown_yj && v_yj == 1)
            dirVec_yj = Vector3.up;
        else if (vDown_yj && v_yj == -1)
            dirVec_yj = Vector3.down;
        else if (hDown_yj && h_yj == -1)
            dirVec_yj = Vector3.left;
        else if (hDown_yj && h_yj == 1)
            dirVec_yj = Vector3.right;

        // scan object
        if (Input.GetButtonDown("Jump") && ScanObject_yj != null)
            manager_yj.Action(ScanObject_yj);
    }

    void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove_yj ? new Vector2(h_yj,0) : new Vector2(0,v_yj);
        rigid_yj.velocity = moveVec* Speed_yj;

        //ray
        Debug.DrawRay(rigid_yj.position, dirVec_yj * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit_yj = Physics2D.Raycast(rigid_yj.position, dirVec_yj, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit_yj.collider != null)
            ScanObject_yj = rayHit_yj.collider.gameObject;
        else
            ScanObject_yj = null;
    }
}