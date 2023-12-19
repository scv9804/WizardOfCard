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

			// 자동 재생(isAutoStart=true)으로 설정되어 있으면 첫 번째 대사 재생
			if (isAutoStart) SetNextDialog();

			isFirst = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (isTypingEffect == true)
			{
				isTypingEffect = false;

				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("OnTypingText");
				speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
				// 대사가 완료되었을 때 출력되는 커서 활성화
				speakers[currentSpeakerIndex].objectArrow.SetActive(true);
				return false;
			}

			// 대사가 남아있을 경우 다음 대사 진행
			if (dialogs.Length > currentDialogIndex + 1)
			{
				SetNextDialog();
			}
			else
			{
				// 현재 대화에 참여했던 모든 캐릭터, 대화 관련 UI를 보이지 않게 비활성화
				for (int i = 0; i < speakers.Length; ++i)
				{
					SetActiveObjects(speakers[i], false);
					// SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기 때문에 별도로 호출
					speakers[i].spriteRenderer.gameObject.SetActive(false);
					speakers[i].highlight.gameObject.SetActive(false);
				}

				return true;
			}
		}

		return false;

	}

	protected override void SetNextDialog()
	{   // 이전 화자의 대화 관련 오브젝트 비활성화
		SetActiveObjects(speakers[currentSpeakerIndex], false);

		// 다음 대사를 진행하도록 
		currentDialogIndex++;

		// 현재 화자 순번 설정
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


		// 현재 화자의 대화 관련 오브젝트 활성화
		SetActiveObjects(speakers[currentSpeakerIndex], true);
		// 현재 화자 이름 텍스트 설정
		speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// 현재 화자의 대사 텍스트 설정
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");

	}


	protected override void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);
		speaker.backGround.gameObject.SetActive(visible);


		// 화살표는 대사가 종료되었을 때만 활성화하기 때문에 항상 false
		speaker.objectArrow.SetActive(false);

		if (dialogs[currentSpeakerIndex].Character == null)
			speaker.spriteRenderer.gameObject.SetActive(false);
		else
			speaker.spriteRenderer.gameObject.SetActive(true);

		if (dialogs[currentSpeakerIndex].highlight == null)
			speaker.highlight.gameObject.SetActive(false);
		else
			speaker.highlight.gameObject.SetActive(true);


		// 캐릭터 알파 값 변경
		Color color = speaker.spriteRenderer.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.spriteRenderer.color = color;
	}

}
