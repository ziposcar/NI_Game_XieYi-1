using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour
{
    public GameObject explosion;
    public GameObject came;
    public CameraMoveWithPlayer cmwp;
    public bool ifeventonce = true;
    public GameObject player;
    public ResPutUp rpu;
    public PlayerControl pc;
    public GameObject dumplings;
    public GameObject Fuzi;
    public float rotatePerFrame = 3f;
    public float NowRotation;
    public string script = "";
    public bool iffade = false;
    public bool ifemerge = false;
    public bool iftriggered = false;
    public bool ifshowed = false;
    public GameObject scenedoor;
    public GameObject WholeMap;
    public GameObject tm;//对话框文字
    public Text tmt;
    public GameObject tkm;//对话框本身
    //public SpriteRenderer tkms;
    public Image tkms;
    private Color duihuakuangColor;
    private Color wenziColor;
    // Use this for initialization
    void Start()
    {
        came = GameObject.Find("Main Camera");
        player = GameObject.Find("NewHero");
        rpu = player.GetComponent<ResPutUp>();
        WholeMap = GameObject.Find("BackGround");
        tkm = GameObject.Find("duihuakuang");
        tm = GameObject.Find("TextMind");
        scenedoor = GameObject.Find("SceneDoor");
        tkms = tkm.GetComponent<Image>();
        tmt = tm.GetComponent<Text>();
        cmwp = came.GetComponent<CameraMoveWithPlayer>();
        pc = player.GetComponent<PlayerControl>();
        //duihuakuangmaterial = tkm.GetComponent<SpriteRenderer>().material;
        //GetComponent<SpriteRenderer>().color = new Color(256, 256, 256, 0);//检测体本身透明
    }

    // Update is called once per frame
    //public Material duihuakuangmaterial;

    float minAlpha = 0f;
    float maxAlpha = 1f;
    float varifySpeed = 1f;
    public float curAlpha = 0f;
    // Use this for initialization


    void Update()
    {
        if (iftriggered)
        {
            if (ifemerge && !ifshowed)
            {
                ScenarioEmerge();
            }
            if (iffade)
            {
                ScenarioFade();
            }
            if (name == "RotationEventPoint")
            {
                pc.ifUpSideDown = true;
                /*
                Destroy(GameObject.Find("NPCs"));
                Destroy(GameObject.Find("Monster"));
                Destroy(GameObject.Find("fuzi(Clone)"));
                Destroy(GameObject.Find("Dumplings(Clone)"));*/
                NowRotation = player.transform.localEulerAngles.z;
                if (NowRotation < 180f)
                {
                    //rotation += Time.deltaTime;
                    came.transform.Rotate(0, 0, rotatePerFrame);
                    player.transform.Rotate(0, 0, rotatePerFrame);
                }
                else if (NowRotation >= 180f && ifshowed)
                {
                    iftriggered = false;
                }
                Physics2D.gravity = new Vector3(0, 9.81F, 0);
               
            }
            else if (name == "SeeTheMonsterEventPoint")
            {
                GameObject.Find("CannonEventPoint").GetComponent<BoxCollider2D>().enabled = true;
            }
            else if (name == "ThrowDumplingsEventPoint")
            {
#if UNITY_ANDROID
                script= "在这里把饺子丢下去吸引它(按'■'键)";
#endif
                if (ifeventonce)
                {
                    if (ifshowed)
                    {
                        if ((Input.GetKeyDown("e") || rpu.if_E_Pressed)&& !pc.facingRight)
                        {
                            Invoke("ThrowDumplings", 0.1f);
                            ifeventonce = false;
                        }
                    }
                }
            }
            else if (name == "SeeChunlianEventPoint")
            {
                if (ifeventonce)
                {
                    
                    if (pc.grounded)
                    {
                        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        pc.enabled = false;
                    }
                    if (ifshowed&&pc.enabled==false)
                    {
                        GameObject chunlian = GameObject.Find("chunlian");
                        came.GetComponent<Camera>().orthographicSize = 3f;
                        cmwp.changeToSolidPoint(chunlian.transform.position, true);
                        ifeventonce = false;
                        pc.enabled = true;
                    }
                }
            }
            else if (name== "PutFuEventPoint")
            {
#if UNITY_ANDROID
                script = "把福字贴回去吧(按'■'键)";
#endif
                if (ifeventonce)
                {
                    if (ifshowed)
                    {
                        if ((Input.GetKeyDown("e") || rpu.if_E_Pressed) && pc.facingRight)
                        {
                            StartCoroutine(FuziFunc());
                            ifeventonce = false;
                        }
                    }
                }
            }
            else if(name=="SeeFireEventPoint")
            {
                if (ifeventonce)
                {
                    
                    if(pc.grounded)
                    {
                        player.GetComponent<Rigidbody2D>().velocity=new Vector2(0,0);
                        pc.enabled = false;
                    }
                    if (ifshowed&& pc.enabled==false)
                    {
                        //player.GetComponent()
                        
                        GameObject Fire = GameObject.Find("FireCrakerBomb");
                        cmwp.changeToSolidPoint(Fire.transform.position, true);
                        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                        explosion.transform.localScale = new Vector3(0.15f,0.15f,1f);
                        StartCoroutine(rpu.Emerge(GameObject.Find("NPC1 (2)")));
                        // Instantiate the explosion where the rocket is with the random rotation.
                        Instantiate(explosion, Fire.transform.position, randomRotation);
                        ifeventonce = false;
                        pc.enabled = true;
                    }
                }
            }
            else if(name=="InChapter1EventPoint")
            {
                GameObject.Find("GameManager").GetComponent<Manager2>().co = ChapterOrder.Chapter1;
                Destroy(this);
            }


        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&collision.name=="NewHero" && !ifshowed&&!iftriggered&&!ifemerge&&!iffade)
        {
            iftriggered = true;
            ifemerge = true;
        }
    }
    void ScenarioFade()
    {
        if (curAlpha > minAlpha)
        {
            curAlpha -= Time.deltaTime * varifySpeed;
            //if (curAlpha < maxAlpha)
            //  varifySpeed *= -1;

            //对话框和文字渐透明
            curAlpha = Mathf.Clamp(curAlpha, minAlpha, maxAlpha);
            duihuakuangColor = tkms.color;
            wenziColor = tmt.color;

            duihuakuangColor.a = curAlpha;
            wenziColor.a = curAlpha;

            tkms.color = duihuakuangColor;
            tmt.color = wenziColor;
            
            
            
        }
        else if (curAlpha <= minAlpha)
        {
            iffade = false;
            ifshowed = true;
        }
    }
    void ScenarioEmerge()
    {
        tm.GetComponent<Text>().text = script;
        if (script != "")
        {
            if (curAlpha < maxAlpha)
            {
                curAlpha += Time.deltaTime * varifySpeed;
                //if (curAlpha < maxAlpha)
                //  varifySpeed *= -1;

                //对话框和文字渐显示
                curAlpha = Mathf.Clamp(curAlpha, minAlpha, maxAlpha);
                duihuakuangColor = tkms.color;
                wenziColor = tmt.color;

                duihuakuangColor.a = curAlpha;
                wenziColor.a = curAlpha;

                tkms.color = duihuakuangColor;
                tmt.color = wenziColor;
            }
            else if (curAlpha >= maxAlpha)
            {
                Invoke("LastAndReadyToFade", 0.5f);
            }
        }
    }
    void LastAndReadyToFade()
    {
        ifemerge = false;
        iffade = true;
    }
    void ThrowDumplings()
    {
        Vector3 targetposition = new Vector3(player.transform.position.x - 0.05f, player.transform.position.y, 0);
        player.GetComponent<ResPutUp>().Reses.Dumplings.clear();
        GameObject dump = Instantiate(dumplings, targetposition, player.transform.rotation);
        dump.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 11);
        cmwp.changeFollowObject(dump);
    }


    IEnumerator FuziFunc()
    {
        GameObject fuzi = Instantiate(Fuzi, player.transform.position, player.transform.rotation);
        cmwp.changeFollowObject(fuzi);

        while (fuzi.transform.localPosition != new Vector3(23.7708f, 0.4588f, 0f)|| fuzi.transform.localScale.x < 1f)
         {
            if(fuzi.transform.localScale.x < 1f)
                fuzi.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
            if(fuzi.transform.localPosition != new Vector3(23.7708f, 0.4588f, 0f))
                fuzi.transform.localPosition = Vector3.MoveTowards(fuzi.transform.localPosition, new Vector3(23.7708f, 0.4588f, 0f), Time.deltaTime);
            yield return 0;
         }
        cmwp.stopAllCoroutine();
        GameObject npc = GameObject.Find("NPC3");
        npc.transform.position = new Vector3(npc.transform.position.x-0.5f,npc.transform.position.y,npc.transform.position.z);
        npc.GetComponentInChildren<TextMesh>().text = "太感谢你了,春联\n的力量回来了！";
        GameObject.Find("Door1collider").GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Find("DrinkWineEventPoint").GetComponent<Scenario>().script = "现在可以好好喝一碗了";
    }
   
}