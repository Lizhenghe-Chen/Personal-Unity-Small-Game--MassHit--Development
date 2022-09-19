using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using System.Text;
using UnityEngine.UI;

namespace UIElements
{
    public class MissionTextCrtl : GlobalUIFunctions
    {
        [Header("Mission Manager")]
        public int Mission_Text_Progress;
        public Animator missionAnimator;
        [SerializeField] LocalizeStringEvent MissionTextEvent;
        public float StylizedSetStringTotalTime = 2f;
        public int Mission_Text_Progress_UpperLimit, Mission_Text_Progress_LowerLimit;
        public List<LocalizedString> MissionTextList;
        public TextMeshProUGUI TMP_MissionText;
        public WaitForSeconds StylizedSetStrigWaitTime;
        public AudioSource soundEffect;
        public Button previousButton, NextButton;
        private void Start()
        {
            // missionAnimator.GetComponent<Animator>();
            // MissionTextEvent.GetComponent<LocalizeStringEvent>();
            // TMP_MissionText = this.GetComponent<TextMeshProUGUI>();
            // soundEffect = this.GetComponent<AudioSource>();

            Mission_Text_Progress = 0;

            MissionTextEvent.StringReference = MissionTextList[Mission_Text_Progress];




            // Invoke("SetEnable", 2);
            //Debug.Log("SetEnable");
        }

        //private void OnEnable()
        //{

        //    Debug.Log(MissionTextList[Mission_Text_Progress].GetLocalizedString());

        //}

        // Update is called once per frame
        void Update()
        {
            // Debug.Log(missionAnimator.GetCurrentAnimatorStateInfo(0).length);
            if (Input.GetKeyDown(GlobalRules.instance.MoveUp))
            {
                NextMissionText();
            }

            if (Input.GetKeyDown(GlobalRules.instance.MoveDown))
            {
                PreviousMissionText();
            }
        }
        public void SwitchBoolParameter()
        {
            missionAnimator.SetBool("isActive", true);
        }
        public void NextMissionText()
        {
            if (!CheckNextAvailability()) return;
            missionAnimator.Play("FadeAway", 0, 0);
            Mission_Text_Progress++;
            MissionTextEvent.StringReference = MissionTextList[Mission_Text_Progress];
        }
        public void PreviousMissionText()
        {
            if (!CheckPreviousAvailability()) return;
            missionAnimator.Play("FadeAway", 0, 0);
            Mission_Text_Progress--;
            MissionTextEvent.StringReference = MissionTextList[Mission_Text_Progress];
        }
        public void NextMissionTextNow()
        {
             //if (!CheckNextAvailability()) return;
            missionAnimator.Play("FadeAway", 0, 0);
            Mission_Text_Progress++;
            try { MissionTextEvent.StringReference = MissionTextList[Mission_Text_Progress];CheckNextAvailability(); }
            catch (System.Exception)
            {
                Debug.LogWarning("MissionTextList out of range");
            }

        }
        bool CheckNextAvailability()
        {
            NextButton.interactable = true;
            previousButton.interactable = true;
            var currentAnimatorInfo = missionAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimatorInfo.IsName("FadeAway"))
            {
                if (currentAnimatorInfo.normalizedTime >= 0.8)// if animator finnished
                {
                    missionAnimator.Play("ShowUp", 0, 0); return false;
                }
            }
            if (Mission_Text_Progress + 1 >= MissionTextList.Count - 1 || Mission_Text_Progress + 1 >= Mission_Text_Progress_UpperLimit)
            {
                NextButton.interactable = false;
            }
            if (Mission_Text_Progress >= MissionTextList.Count - 1 || Mission_Text_Progress >= Mission_Text_Progress_UpperLimit)
            {
                return NextButton.interactable = false;
            }
            return true;
            // Debug.Log("Enable");
        }
        bool CheckPreviousAvailability()
        {
            NextButton.interactable = true;
            previousButton.interactable = true;
            var currentAnimatorInfo = missionAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimatorInfo.IsName("FadeAway"))
            {
                if (currentAnimatorInfo.normalizedTime >= 0.8)
                {
                    missionAnimator.Play("ShowUp", 0, 0); return false;
                }
            }
            if (Mission_Text_Progress - 1 <= 0 || Mission_Text_Progress - 1 <= Mission_Text_Progress_LowerLimit)
            {
                previousButton.interactable = false;
            }
            if (Mission_Text_Progress <= 0 || Mission_Text_Progress <= Mission_Text_Progress_LowerLimit)
            {
                return previousButton.interactable = false;
            }
            return true;
        }
        private IEnumerator coroutine;

        public void StartStylizedSetString(string _)
        {
            TMP_MissionText.text = _; TMP_MissionText.maxVisibleCharacters = 0;
            // TMP_MissionText.text = string.Empty;
            if (coroutine != null) StopCoroutine(coroutine);
            //StartCoroutine(StylizedSetString(content));
            Invoke(nameof(DelayCoroutine), 0.1f);//this is strange that Locolaz event need time to compile...so wait it for a while


        }
        void DelayCoroutine() { StartCoroutine(coroutine = StylizedSetTMPString()); }
        //IEnumerator StylizedSetString(string content)
        //{
        //    StringBuilder Buffer = new();
        //    int index = 0;
        //    while (index < content.Length)
        //    {
        //        Buffer.Append(content[index]);
        //        soundEffect.pitch = Random.Range(1f, 3f);
        //        soundEffect.Play();
        //        TMP_MissionText.text = Buffer.ToString();
        //        yield return StylizedSetStrigWaitTime;
        //        index++;
        //    }
        //}

        IEnumerator StylizedSetTMPString()
        {
            int index = 0;
            while (index < TMP_MissionText.GetParsedText().Length)
            {
                StylizedSetStrigWaitTime = new WaitForSeconds(StylizedSetStringTotalTime / TMP_MissionText.GetParsedText().Length);
                // Debug.Log(TMP_MissionText.text + "<>" + TMP_MissionText.GetParsedText().Length);
                // Debug.Log(TMP_MissionText.textInfo.characterInfo[index].character);
                soundEffect.pitch = Random.Range(1f, 3f);
                soundEffect.Play();

                TMP_MissionText.maxVisibleCharacters = index + 1;
                yield return StylizedSetStrigWaitTime;
                index++;
            }
        }
        //void SetEnable()
        //{
        //    TMP_MissionText.text = string.Empty;
        //    MissionTextEvent.enabled = true;
        //    Debug.Log("SetEnable");
        //    //gameObject.SetActive(true);
        //}
    }
}

