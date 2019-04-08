using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Tutorial : MonoBehaviour
{
    private static GameManager_Tutorial instance;
    public static GameManager_Tutorial Instance
    {
        get { return instance; }
    }

    public enum MakingProcess { Recipe = 0, Moving, Bellowing, Heating, Hammering, HammeringComplete, Cooling, Handling, Completing }
    public MakingProcess _makingProcess = MakingProcess.Recipe;

    public bool isPlaying = false;

    public GameObject[] recipeProcessObject; 
    public GameObject[] movingProcessObject;
    public GameObject[] bellowingProcessObjects;
    public GameObject[] hammeringProcessObjects;
    public GameObject[] coolingProcessObjects;
    public GameObject[] handlingProcessObject;
    private List<GameObject> stoppedObj = new List<GameObject>();

    public bool recipeBoxCheck = false;
    public bool movingBoxCheck = false;
    public bool bellowingBoxCheck = false;
    public bool tempCheck = false;
    public bool magneticCheck = false;
    public bool hammeringCompleteCheck = false;
    public bool coolingCheck = false;
    public bool handlingBoxCheck = false;

    public GameObject[] instructions;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        stoppedObj.AddRange(recipeProcessObject);
        stoppedObj.AddRange(bellowingProcessObjects);
        stoppedObj.AddRange(movingProcessObject);
        stoppedObj.AddRange(hammeringProcessObjects);
        stoppedObj.AddRange(coolingProcessObjects);
        stoppedObj.AddRange(handlingProcessObject);
    }

    void Update()
    {
        if (isPlaying) return;
        StateTransition();
    }

    private void StateTransition()
    {
        switch (_makingProcess)
        {
            case MakingProcess.Recipe:
                StopShining();
                StartShining(recipeProcessObject);
                instructions[0].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Moving:
                StopShining();
                StartShining(movingProcessObject);
                instructions[0].SetActive(false);
                instructions[1].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Bellowing:
                StopShining();
                StartShining(bellowingProcessObjects);
                instructions[1].SetActive(false);
                instructions[2].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Heating:
                StopShining();
                isPlaying = true;
                instructions[2].SetActive(false);
                instructions[3].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Hammering:
                StopShining();
                StartShining(hammeringProcessObjects);
                instructions[3].SetActive(false);
                instructions[4].SetActive(true);
                instructions[5].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.HammeringComplete:
                StopShining();
                isPlaying = true;
                instructions[4].SetActive(false);
                instructions[5].SetActive(false);
                instructions[6].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Cooling:
                StopShining();
                StartShining(coolingProcessObjects);
                instructions[6].SetActive(false);
                instructions[7].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Handling:
                StopShining();
                StartShining(handlingProcessObject);
                instructions[7].SetActive(false);
                instructions[8].SetActive(true);
                StartCoroutine(StateTransitionChecker());
                break;

            case MakingProcess.Completing:
                StopShining();
                isPlaying = true;
                instructions[8].SetActive(false);
                break;
        }
    }

    public void StopShining()
    {
        foreach (var item in stoppedObj)
        {
            item.GetComponent<Animation>().Stop();
            var _renderer = item.GetComponent<MeshRenderer>();

            if (_renderer.materials.Length >= 2)
                foreach (var item_sec in _renderer.materials)
                {
                    item_sec.DisableKeyword("_EMISSION");
                }
            else
                _renderer.material.DisableKeyword("_EMISSION");
        }
    }

    private void StartShining(GameObject[] gameObjects)
    {
        foreach (var item in gameObjects)
        {
            var _renderer = item.GetComponent<MeshRenderer>();

            if (_renderer.materials.Length >= 2)
            {
                foreach (var item_sec in _renderer.materials)
                {
                    item_sec.EnableKeyword("_EMISSION");
                }
            }
            else _renderer.material.EnableKeyword("_EMISSION");

            item.GetComponent<Animation>().Play();
        }
        isPlaying = true;
    }

    public void QuitTutorial()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private IEnumerator StateTransitionChecker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1F);
            switch (_makingProcess)
            {
                case MakingProcess.Recipe:
                    if (recipeBoxCheck)
                    {
                        _makingProcess = MakingProcess.Moving;
                        isPlaying = false;
                        recipeBoxCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Moving:
                    if (movingBoxCheck)
                    {
                        _makingProcess = MakingProcess.Bellowing;
                        isPlaying = false;
                        movingBoxCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Bellowing:
                    if (bellowingBoxCheck)
                    {
                        _makingProcess = MakingProcess.Heating;
                        isPlaying = false;
                        bellowingBoxCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Heating:
                    if (tempCheck)
                    {
                        _makingProcess = MakingProcess.Hammering;
                        isPlaying = false;
                        tempCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Hammering:
                    if (magneticCheck)
                    {
                        _makingProcess = MakingProcess.HammeringComplete;
                        isPlaying = false;
                        magneticCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.HammeringComplete:
                    if (hammeringCompleteCheck)
                    {
                        _makingProcess = MakingProcess.Cooling;
                        isPlaying = false;
                        hammeringCompleteCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Cooling:
                    if (coolingCheck)
                    {
                        _makingProcess = MakingProcess.Handling;
                        isPlaying = false;
                        coolingCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Handling:
                    if (handlingBoxCheck)
                    {
                        _makingProcess = MakingProcess.Completing;
                        isPlaying = false;
                        handlingBoxCheck = false;
                        yield break;
                    }
                    break;

                case MakingProcess.Completing:
                    break;
            }
        }
    }
}
