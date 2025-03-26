using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;


    public GameObject selfCorpse;
    public GameObject ObstacleCorpse;

    public Transform spawnPoint;

    public bool hasItem = false;
    public GameObject item;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }


    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);

        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.5f, rigid.linearVelocity.y);
        }

        //���� ��ȯ
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //�ȴ� �ִϸ��̼�
        if (Mathf.Abs(rigid.linearVelocity.x) < 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        //EŰ ������ ������ ���
        if (Input.GetKeyDown(KeyCode.E) && hasItem)
        {
            UseItem();
        }

        //RŰ ������ ����۰� ��ü ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            SelfDie();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.linearVelocity.x > maxSpeed) //Right Max Speed
        {
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        }
        else if (rigid.linearVelocity.x < maxSpeed * (-1)) //Left Max Speed
        {
            rigid.linearVelocity = new Vector2(maxSpeed * (-1), rigid.linearVelocity.y);
        }

        //������ ���� ���� ����
        if (rigid.linearVelocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down * 0.7f, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.2f, LayerMask.GetMask("Ground"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.3f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    //���Ƿ� ���(���ึ�ñ�&RŰ������
    public void SelfDie()
    {
        GameObject corpse = Instantiate(selfCorpse, transform.position, Quaternion.identity); //��ü ����

        //��ü�� CorpseManager�� �ڽ����� ����
        GameObject corpseManager = GameObject.Find("Corpse Manager");
        if (corpseManager != null)
        {
            corpse.transform.parent = corpseManager.transform;
        }

        //�÷��̾� ������
        transform.position = spawnPoint.position;
    }

    //��ֹ��� ���(����,ȭ��)
    public void ObstacleDie()
    {
        Instantiate(ObstacleCorpse, transform.position, Quaternion.identity); //��ü ����
        DontDestroyOnLoad (ObstacleCorpse);
        transform.position = spawnPoint.position;
    }

    //�������� �Ծ��� ��
    public void PickUpItem(GameObject newItem)
    {
        if (!hasItem)
        {
            item = newItem;
            hasItem = true;
            item.SetActive(false);
        }
    }

    //������ ���
    void UseItem()
    {
        SelfDie();
        hasItem = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
        }
    }

    //�ӵ� 0 ����
    public void VelocityZero()
    {
        rigid.linearVelocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJumping", false);
        }
        else if (collision.gameObject.CompareTag("Corpse"))
        {
            anim.SetBool("isJumping", false);
        }
    }
}