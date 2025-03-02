using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator Animator; // 角色动画控制器
    private Vector3 lastPosition; // 记录上一次角色的位置

    //在游戏开始时，记录角色的初始位置
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = transform.position - lastPosition;
        lastPosition = transform.position;

        if (movement.magnitude > 0.01f)
        {
            Animator.SetFloat("MoveX", movement.x);
            Animator.SetFloat("MoveY", movement.y);

            if (movement.x > 0)
                Animator.Play("Walk_Right");
            else if (movement.x < 0)
                Animator.Play("Walk_Left");
            else if (movement.y > 0)
                Animator.Play("Walk_Up");
            else if (movement.y < 0)
                Animator.Play("Walk_Down");

        }
        else //角色停止
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Up"))
                Animator.Play("up");
            else if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Down"))
                Animator.Play("down");
            else if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Left"))
                Animator.Play("left");
            else if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Right"))
                Animator.Play("right");

        }
    }
}
