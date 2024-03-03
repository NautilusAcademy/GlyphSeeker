using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEditor;

[CreateAssetMenu(menuName = "Scriptable Objects/Options (S.O.)", fileName = "Options_SO")]
public class OptionsSO_Script : ScriptableObject
{
    //Menu Principale
    #region Cambia scena


    public void LoadChosenScene(int sceneNum)
    {
        SceneManager.LoadSceneAsync(sceneNum);
    }
    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void LoadAdditiveScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Additive);
    }
    public void NextScene()
    {
        int sceneNow = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(++sceneNow);
    }
    public void PreviousScene()
    {
        int scenaNow = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(--scenaNow);
    }

    #endregion


    #region Esci

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion


    //Opzioni
    #region Sensibilita'

    const float MIN_SENSIT = 1f,
                MAX_SENSIT = 10;

    [Space(15)]
    [Range(MIN_SENSIT, MAX_SENSIT)]
    [SerializeField] float sensitivity;

    public void ChangeSensitivity(float s)
    {
        sensitivity = Mathf.Clamp(s, MIN_SENSIT, MAX_SENSIT);
    }

    public float GetSensitivity() => sensitivity;

    #endregion


    #region Volume e Audio

    [Space(15)]
    [SerializeField] AudioMixer generalMixer;
    #region Funzioni della variabile (con tasto dx)
    [ContextMenuItem("–Reset curve to default settings–", nameof(ResetAudioCurve))] 
    #endregion
    [SerializeField] AnimationCurve audioCurve;

    [Range(0, 110)]
    [SerializeField] float musicVolume = 0f;
    [Range(0, 110)]
    [SerializeField] float soundVolume = 0f;


    ///<summary></summary>
    /// <param name="vM"> new volume, in range [0; 1.1]</param>
    public void ChangeMusicVolume(float vM)
    {
        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("musVol", audioCurve.Evaluate(vM));

        musicVolume = vM * 100;
    }
    ///<summary></summary>
    /// <param name="vS"> new volume, in range [0; 1.1]</param>
    public void ChangeSoundVolume(float vS)
    {
        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("sfxVol", audioCurve.Evaluate(vS));

        soundVolume = vS * 100;
    }

    ///<summary></summary>
    /// <param name="vM"> new volume, in range [0; 11]</param>
    public void ChangeMusicVolumeTen(float vM)
    {
        vM /= 10;

        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("musVol", audioCurve.Evaluate(vM));

        musicVolume = vM * 100;
    }
    ///<summary></summary>
    /// <param name="vS"> new volume, in range [0; 11]</param>
    public void ChangeSoundVolumeTen(float vS)
    {
        vS /= 10;

        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("sfxVol", audioCurve.Evaluate(vS));

        soundVolume = vS * 100;
    }

    public AnimationCurve GetVolumeCurve() => audioCurve;

    public float GetMusicVolume() => audioCurve.Evaluate(musicVolume);
    public float GetMusicVolume_Percent() => musicVolume / 100;
    public float GetSoundVolume() => audioCurve.Evaluate(soundVolume);
    public float GetSoundVolume_Percent() => soundVolume / 100;



    public void ResetAudioCurve()
    {
        //Cancella tutti i keyframe (se ce ne sono)
        if (audioCurve.keys.Length >= 0)
        {
            audioCurve.keys = null;
        }

        //Ne crea di nuovi con i parametri usati dall'audio
        Keyframe[] newKeys = new Keyframe[]
        {
            new Keyframe(0, -60,
                         0, 172.1225f),
            new Keyframe(1, 0),
            new Keyframe(1.1f, 5)
        };

        audioCurve.keys = newKeys;   //Li inserisce nella curva

        //Sistema le tangenti del secondo e terzo keyframe
        //per "adattarli" meglio ai decibel dell'audio
        AnimationUtility.SetKeyBroken(audioCurve, 1, true);
        AnimationUtility.SetKeyRightTangentMode(audioCurve, 1, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyLeftTangentMode(audioCurve, 2, AnimationUtility.TangentMode.Linear);
    }

    #endregion


    #region Selezione Rune

    [Space(15)]
    [SerializeField] RuneSelectionType runeSelection;

    public void ChangeRuneSelect(RuneSelectionType rS)
    {
        runeSelection = rS;
    }
    public void ChangeRuneSelect(int i)
    {
        runeSelection = (RuneSelectionType)i;
    }

    public RuneSelectionType GetRuneSelect() => runeSelection;

    #endregion


    #region Fullscreen

    [Space(15)]
    [SerializeField] bool fullscreen = true;

    public void ToggleFullscreen(bool yn)
    {
        Screen.fullScreen = yn;

        fullscreen = yn;
    }

    public bool GetIsFullscreen() => fullscreen;

    #endregion


    //Altro
    #region Altre funzioni

    //Enum della selezione delle Rune
    public enum RuneSelectionType
    {
        MouseWheel,
        HoldAndSelect
    }


    //Funzione di Reset di ogni variabile
    #region Funzione di script (con tasto dx)
    [ContextMenu("–Reset all variables to default–")]
    #endregion
    void ResetAllVariables()
    {
        sensitivity = 1;

        ResetAudioCurve();
        musicVolume = 50;
        soundVolume = 60;

        fullscreen = false;

        runeSelection = RuneSelectionType.MouseWheel;
    }

    #endregion
}
