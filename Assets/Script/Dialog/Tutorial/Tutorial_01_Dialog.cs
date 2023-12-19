using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_01_Dialog : DialogSystem
{
	public override bool UpdateDialog()
	{

		if (isFirst == true)
		{
			Setup();

			// �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
			if (isAutoStart) SetNextDialog();

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
				SetNextDialog();
			}
			else
			{
				// ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
				for (int i = 0; i < speakers.Length; ++i)
				{
					SetActiveObjects(speakers[i], false);
					// SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
					speakers[i].spriteRenderer.gameObject.SetActive(false);
					speakers[i].highlight.gameObject.SetActive(false);
				}

				return true;
			}
		}

		return false;

	}

	protected override void SetNextDialog()
	{   // ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
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


	protected override void SetActiveObjects(Speaker speaker, bool visible)
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

		if (dialogs[currentSpeakerIndex].highlight == null)
			speaker.highlight.gameObject.SetActive(false);
		else
			speaker.highlight.gameObject.SetActive(true);


		// ĳ���� ���� �� ����
		Color color = speaker.spriteRenderer.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.spriteRenderer.color = color;
	}

}
