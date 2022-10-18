using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Speaker[]		speakers;					// ��ȭ�� �����ϴ� ĳ���͵��� UI �迭
	[SerializeField]
	private	DialogData[]	dialogs;					// ���� �б��� ��� ��� �迭
	[SerializeField]
	private	bool			isAutoStart = true;			// �ڵ� ���� ����
	private	bool			isFirst = true;				// ���� 1ȸ�� ȣ���ϱ� ���� ����
	private	int				currentDialogIndex = -1;	// ���� ��� ����
	private	int				currentSpeakerIndex = 0;	// ���� ���� �ϴ� ȭ��(Speaker)�� speakers �迭 ����
	private	float			typingSpeed = 0.1f;			// �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
	private	bool			isTypingEffect = false;		// �ؽ�Ʈ Ÿ���� ȿ���� ���������

	private void Awake()
	{
		Setup();
	}

	private void Setup()
	{
		// ��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
		for ( int i = 0; i < speakers.Length; ++ i )
		{
			SetActiveObjects(speakers[i], false);
			// ĳ���� �̹����� ���̵��� ����
			speakers[i].spriteRenderer.gameObject.SetActive(false);
		}

	}

	public bool Update_CardTutorial()
	{   
		if (isFirst == true)
		{
			Setup();

			// �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
			if (isAutoStart) SetImageDialog();

			isFirst = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (isTypingEffect == true)
			{
				isTypingEffect = false;

				// Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
				StopCoroutine("OnTypingText");
				speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
				// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
				speakers[currentSpeakerIndex].objectArrow.SetActive(true);
				return false;
			}

			// ��簡 �������� ��� ���� ��� ����
			if (dialogs.Length > currentDialogIndex + 1)
			{
				SetImageDialog();
			}
			else
			{
				// ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
				for (int i = 0; i < speakers.Length; ++i)
				{
					SetActiveObjects_tutroial(speakers[i], false);
					// SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
					speakers[i].spriteRenderer.gameObject.SetActive(false);
					speakers[i].highlight.gameObject.SetActive(false);
				}

				return true;
			}
		}

		return false;

	}


	private void SetImageDialog()
	{
		// ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], false);

		// ���� ��縦 �����ϵ��� 
		currentDialogIndex++;

		// ���� ȭ�� ���� ����
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

		

		if (dialogs[currentDialogIndex].highlight != null)
		{
			speakers[currentSpeakerIndex].highlight.gameObject.SetActive(true);
			speakers[currentDialogIndex].highlight.sprite = dialogs[currentDialogIndex].highlight;
		}
		else
		{
			speakers[currentSpeakerIndex].highlight.gameObject.SetActive(false);
		}

		if (dialogs[currentDialogIndex].Character != null)
		{
			speakers[currentSpeakerIndex].spriteRenderer.enabled = true;
			speakers[currentDialogIndex].spriteRenderer.sprite = dialogs[currentDialogIndex].Character;
		}
		else
		{
			speakers[currentSpeakerIndex].spriteRenderer.enabled = false;

		}


		// ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], true);
		// ���� ȭ�� �̸� �ؽ�Ʈ ����
		speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// ���� ȭ���� ��� �ؽ�Ʈ ����
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects_tutroial(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);
		speaker.backGround.gameObject.SetActive(visible);
		

		// ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
		speaker.objectArrow.SetActive(false);

		if (dialogs[currentSpeakerIndex].Character == null)
			speaker.spriteRenderer.gameObject.SetActive(false);
		else
			speaker.spriteRenderer.gameObject.SetActive(true);

		if (dialogs[currentDialogIndex].highlight == null)
			speaker.highlight.gameObject.SetActive(false);
		else
			speaker.highlight.gameObject.SetActive(true);


		// ĳ���� ���� �� ����
		Color color = speaker.spriteRenderer.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.spriteRenderer.color = color;
	}



	public bool UpdateDialog()
	{
		// ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
		if ( isFirst == true )
		{
			// �ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
			Setup();

			// �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
			if ( isAutoStart ) SetNextDialog();

			isFirst = false;
		}

		if ( Input.GetMouseButtonDown(0) )
		{
			// �ؽ�Ʈ Ÿ���� ȿ���� ������϶� ���콺 ���� Ŭ���ϸ� Ÿ���� ȿ�� ����
			if ( isTypingEffect == true )
			{
				isTypingEffect = false;

				// Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
				StopCoroutine("OnTypingText");
				speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
				// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
				speakers[currentSpeakerIndex].objectArrow.SetActive(true);



				return false;
			}

			// ��簡 �������� ��� ���� ��� ����
			if ( dialogs.Length > currentDialogIndex + 1 )
			{
				SetNextDialog();
			}
			// ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
			else
			{
				// ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
				for ( int i = 0; i < speakers.Length; ++ i )
				{
					SetActiveObjects(speakers[i], false);
					// SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
					speakers[i].spriteRenderer.gameObject.SetActive(false);
				}

				return true;
			}
		}

		return false;
	}


	private void SetNextDialog()
	{
		// ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], false);

		// ���� ��縦 �����ϵ��� 
		currentDialogIndex ++;

		// ���� ȭ�� ���� ����
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

		speakers[currentDialogIndex].spriteRenderer.sprite = dialogs[currentDialogIndex].Character;

		// ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], true);
		// ���� ȭ�� �̸� �ؽ�Ʈ ����
		speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// ���� ȭ���� ��� �ؽ�Ʈ ����
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);
		speaker.backGround.gameObject.SetActive(visible);

		// ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
		speaker.objectArrow.SetActive(false);

		if (dialogs[currentSpeakerIndex].Character == null)
		{
			speaker.spriteRenderer.gameObject.SetActive(false);
		}
		else
		{
			speaker.spriteRenderer.gameObject.SetActive(true);
		}

		// ĳ���� ���� �� ����
		Color color = speaker.spriteRenderer.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.spriteRenderer.color = color;
	}

	private IEnumerator OnTypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
		while ( index < dialogs[currentDialogIndex].dialogue.Length )
		{
			speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);

			index ++;
		
			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);
	}
}

[System.Serializable]
public struct Speaker
{
	public GameObject backGround;
	public Image highlight;
	public	Image	spriteRenderer;		// ĳ���� �̹��� (û��/ȭ�� ���İ� ����)
	public	Image			imageDialog;		// ��ȭâ Image UI
	public	TextMeshProUGUI	textName;			// ���� ������� ĳ���� �̸� ��� Text UI
	public	TextMeshProUGUI	textDialogue;		// ���� ��� ��� Text UI
	public	GameObject		objectArrow;		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� ������Ʈ
}

[System.Serializable]
public struct DialogData
{
	public Sprite highlight;
	public Sprite Character;
	public	int		speakerIndex;	// �̸��� ��縦 ����� ���� DialogSystem�� speakers �迭 ����
	public	string	name;			// ĳ���� �̸�
	[TextArea(3, 5)]
	public	string	dialogue;		// ���
}

