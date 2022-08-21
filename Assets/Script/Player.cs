using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] Weapon;
    public bool[] hasWeapon;

    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;

    bool iDown;

    bool isJump;
    bool isDodge;

    bool sDown1;
    bool sDown2;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearOj;
    GameObject equipWeapon;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Getinput();
        Move();
        Turn();
        Jump();
        Dodge();
        Interation();
        Swap();
    }

    void Getinput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //Axis 값을 정수로 반환
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");

    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge)
        {
            moveVec = dodgeVec;
        }
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);//이름, 조건
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); // lookat -> 지정된 벡터를 향해서 회전시켜주는 함수
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJump /*!isDodge*/)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            // impulse는 즉각적인 힘을 줌.

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero&&!isJump)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");

            isDodge = true;

            //시간차를 줘서 닷지를 푸는 함수를 만듦

            Invoke("DodgeOut", 0.3f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        int index = -1;
        if (sDown1) index = 0;
        if (sDown2) index = 1;

        if((sDown1 || sDown2) && !isJump && !isDodge)
        {
            if(equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            equipWeapon = Weapon[index];
            equipWeapon.SetActive(true);
        }
    }

    void Interation()
    {
        if(iDown == true && nearOj != null && !isJump && !isDodge)
        {
            if (nearOj.tag == "Weapon")
            {
                Item item = nearOj.GetComponent<Item>();
                int weaponIndex = item.index;
                hasWeapon[weaponIndex] = true;

                Destroy(nearOj);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearOj = other.gameObject;

            Debug.Log(nearOj.name);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearOj = null;
        }
    }

}
