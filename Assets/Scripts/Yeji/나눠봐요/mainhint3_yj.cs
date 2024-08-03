using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting; // TextMeshPro 네임스페이스 추가
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

// 어디에나 있는거(단서3)

public class mainhint3_yj : MonoBehaviour
{
    private bool isbasicdial_yj = false; // 대사 치고 있는지 여부

    public GameObject choiceUI3_yj; // 기본활동3 UI 패널

    public GameObject Dial_changyj; // 기본대사 띄울 대화창
    public TextMeshProUGUI dialoguename_yj; // name text
    public TextMeshProUGUI dialogueText_yj; // line text
    public float interactionRange = 3f; // 상호작용 거리

    public GameObject player; // 플레이어 오브젝트
    private GameObject currentNPC; // 현재 상호작용하는 NPC 저장 변수

    public GameObject npc3_yj; // 힌트

    public Button noButton3; // 아니오 버튼 연결3
    public Button noButton5; // 결과창 닫기 버튼 연결5

    public Button findhintButton_yj; // 3. 단서 보기 버튼 연결
    public Button gotobedButton_yj; //5. 침대 이동 버튼 연결

    public GameObject resultUI_yj; // 결과 UI 패널
    public GameObject resultUI2_yj;
    public TextMeshProUGUI resuedit_yj; // result edit text
    public TextMeshProUGUI resuedit2_yj; // result edit text

    // 기본활동 몇번 진행했는지 세야 하니..플레이어를 부르자
    // public PlayerNow_yj nowplayer_yj;
    public PlayerManager_yj playermanager_yj; // 체력관리용
    public TimeManager timemanager_yj; // 날짜 관리 + 기본활동 덧뺄셈용

    private static mainhint3_yj _instance;

    public static mainhint3_yj Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<mainhint3_yj>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("talkwithjjang_yj");
                    _instance = obj.AddComponent<mainhint3_yj>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        findhintButton_yj.onClick.AddListener(OnhintButtonClick);
        gotobedButton_yj.onClick.AddListener(OngobedButtonClick);

        noButton3.onClick.AddListener(OnNo3ButtonClick);
        noButton5.onClick.AddListener(OnNo5ButtonClick);

        isbasicdial_yj = false;
        playermanager_yj.playerNow.howtoday_py = 0;
        playermanager_yj.playerNow.howtrain_py = 0;
        player = GameObject.FindGameObjectWithTag("Player"); // 태그가 "Player"인 오브젝트 찾기
        HideUI(); // 시작할 때 UI 숨기기
    }

    // Update is called once per frame
    void Update()
    {
        CheckNPCInteraction();
        HandleUserInput();
        //timeManager_yj.UpdateDateAndTimeDisplay(); 
    }

    void HandleUserInput()
    {
        if (currentNPC != null)
        {
            HandleNPCDialogue_yj(currentNPC); // npc한테 가까이 가면 대화창이 뜬다

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isbasicdial_yj)
                    HandleNPCchoice_yj(currentNPC);
            }
        }
    }

    void CheckNPCInteraction()
    {
        if (npc3_yj != null)
        {
            float distanceNPC3 = Vector3.Distance(player.transform.position, npc3_yj.transform.position);

            if (distanceNPC3 <= interactionRange)
            {
                currentNPC = npc3_yj;
            }
            else
            {
                currentNPC = null;
            }
        }
    }

    void HandleNPCDialogue_yj(GameObject npc_yjyj)
    {
        if (isbasicdial_yj == false)
            Dial_changyj.SetActive(true);
        else if (resultUI_yj.activeSelf || choiceUI3_yj.activeSelf)
            Dial_changyj.SetActive(false);
        // 현재 NPC에 따라 대화 처리
        if (npc_yjyj == npc3_yj) // 단서
        {
            dialoguename_yj.text = "단서"; // 단서 이름 출력 
            dialogueText_yj.text = "단서를 찾았다.\n인벤토리에서 내용을 확인해 보자.\n[스페이스바를 누르세요]"; // 단서 기본 대사 출력                                      
            isbasicdial_yj = true; // 기본대사 치고 있는 중
        }
    }

    void HandleNPCchoice_yj(GameObject npc_yjyj)
    {
        // 현재 NPC에 따라 선택지처리
        if (npc_yjyj == npc3_yj) // 단서
        {
            Dial_changyj.SetActive(false);
            isbasicdial_yj = false;
            choiceUI3_yj.SetActive(true);
        }

    }
    // 기본활동3 : 단서 보겠다 했을 때
    public void OnhintButtonClick()
    {
        Debug.Log("단서 클릭");
        choiceUI3_yj.SetActive(false);
        timemanager_yj.CompleteActivity(); // 하루 기본 활동 수행 횟수 1 증가
        resuedit_yj.text = $"기본활동 횟수 : {timemanager_yj.activityCount / 2} / 3";
        if (timemanager_yj.activityCount >= 5)
        {
            resuedit2_yj.text = "하루치 기본 활동 3개를 모두 완수하셨습니다!\n[주인공 집]의 [침대]로 돌아가 휴식을 취해주세요!";
            resultUI2_yj.SetActive(true);
        }

        resultUI_yj.SetActive(true);
        Invoke("HideResultPanel()", 2f);

        // 인벤토리에 힌트 랜덤 추가
        ItemManager.instance.GetHint_inv();

        SceneManager.LoadScene("InventoryMain"); // 인벤토리 씬으로 이동
        // 찾은 단서 개수를 한 개 늘림. 이건 인벤토리랑 연관 후에 생각해야 할듯
    }

    public void OngobedButtonClick()
    {
        SceneManager.LoadScene("main_house");
    }
    public void OnNo3ButtonClick()
    {
        choiceUI3_yj.SetActive(false); // 단서 UI 선택창 비활성화
        isbasicdial_yj = false;
    }

    public void OnNo5ButtonClick()
    {
        resultUI_yj.SetActive(false); // 결과 UI 선택창 비활성화
        resultUI2_yj.SetActive(false); // 결과2 UI 선택창 비활성화
        isbasicdial_yj = false;
    }


    public void HideUI()
    {
        choiceUI3_yj.SetActive(false);
        Dial_changyj.SetActive(false);
        resultUI_yj.SetActive(false);
    }
    void HideResultPanel()
    {
        resultUI_yj.SetActive(false);
    }
}