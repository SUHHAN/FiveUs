using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

public class NpcPersuade : MonoBehaviour
{
    public GameObject persuadeUI; // ���� UI �г�
    public GameObject resultUI; // ��� UI �г�
    public TextMeshProUGUI persuadeText; // ���� �ؽ�Ʈ UI ����
    public TextMeshProUGUI resultText; // ��� �ؽ�Ʈ UI ����
    public Button yesButton; // �� ��ư ����
    public Button noButton; // �ƴϿ� ��ư ����
    public int affection = 50; // ȣ����
    private int remainingAttempts = 3; // ���� ���� �õ� Ƚ��
    public bool success = false; // ���� ���� ����
    private NpcScript npcScript; // NpcScript ��ũ��Ʈ ����

    void Start()
    {
        npcScript = GetComponent<NpcScript>(); // NpcScript ��ũ��Ʈ ���� ���
        persuadeUI.SetActive(false); // ������ �� ���� UI ��Ȱ��ȭ
        resultUI.SetActive(false); // ������ �� ��� UI ��Ȱ��ȭ
        yesButton.onClick.AddListener(OnYesButtonClick); // �� ��ư Ŭ�� �̺�Ʈ ����
        noButton.onClick.AddListener(OnNoButtonClick); // �ƴϿ� ��ư Ŭ�� �̺�Ʈ ����
    }

    public void ShowPersuadeUI()
    {
        persuadeUI.SetActive(true); // ���� UI ǥ��
        persuadeText.text = $"�����Ͻðڽ��ϱ�? ���� ��ȸ: {remainingAttempts}";
        yesButton.gameObject.SetActive(true); // �� ��ư ǥ��
        noButton.gameObject.SetActive(true); // �ƴϿ� ��ư ǥ��
    }

    public void OnYesButtonClick()
    {
        AttemptPersuasion(); // ���� �õ�
    }

    public void OnNoButtonClick()
    {
        persuadeUI.SetActive(false); // ���� UI �����
    }

    public void AttemptPersuasion()
    {
        if (remainingAttempts > 0)
        {
            int successChance = affection; // ȣ������ ����� ���� Ȯ��
            int randomValue = Random.Range(0, 100); // 0���� 100 ������ ���� �� ����
            if (randomValue < successChance)
            {
                // ���� ����
                resultText.text = "�����߽��ϴ�!";
                success = true;
                npcScript.HidePersuadeAndGiftButtons(); // ���� ���� �� �����ϱ�� �����ϱ� ��ư �����
            }
            else
            {
                // ���� ����
                remainingAttempts--; // ���� �õ� Ƚ�� 1 ����
                resultText.text = $"�����߽��ϴ�! ���� ��ȸ: {remainingAttempts}";
                success = false; // ���� ���� ���� ����
            }
            persuadeUI.SetActive(false); // ���� UI �����
            resultUI.SetActive(true); // ��� UI ǥ��
            StartCoroutine(HideResultUIAfterDelay(2f)); // 2�� �� ��� UI �����

            if (remainingAttempts == 0 && !success)
            {
                npcScript.HidePersuadeAndGiftButtons(); // ���� �õ� ��ȸ ��� ���� �� ��ư �����
            }
        }
        else
        {
            resultText.text = "���� �õ� ��ȸ�� ��� ����߽��ϴ�.";
            persuadeUI.SetActive(false); // ���� UI �����
            resultUI.SetActive(true); // ��� UI ǥ��
            StartCoroutine(HideResultUIAfterDelay(3f)); // 3�� �� ��� UI �����
            npcScript.HidePersuadeAndGiftButtons(); // ���� �õ� ��ȸ ��� ���� �� ��ư �����
        }
    }

    private IEnumerator HideResultUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // delay ��ŭ ���
        resultUI.SetActive(false); // ��� UI �����
    }
}