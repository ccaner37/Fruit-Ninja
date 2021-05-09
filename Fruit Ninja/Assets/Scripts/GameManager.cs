using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Slice;
	public AudioSource slash;
	public int score;
	public Text scoreText;
	public Spawner spawner;

	private float currentTime;
	private float slowMotionFactor = 0.10f;

	private static GameManager instance = null;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("GameManager").AddComponent<GameManager>();
			}

			return instance;
		}
	}

	private void OnEnable()
	{
		instance = this;
	}


	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Slice.SetActive(true);
        }

	}

	public void SlashSound()
    {
		slash.Play();
    }

	public void IncreaseScore()
    {
		score += 1;
		scoreText.text = score.ToString();

		int tmp = score % 30;
		if (tmp == 0 || tmp == 30) //could also be written as " if(tmp % 100 == 0) "
		{
			spawner.LaunchBomb();
		}

		//increase game speed
		int faster = score % 50;
		if (faster == 0 || faster == 50) //could also be written as " if(tmp % 100 == 0) "
		{
			spawner.FasterFasterFaster();
		}
	}

	public void GameEnd()
    {
		GameOverEffect();
		spawner.EffectSpawner();
    }

	public void GameOverEffect()
	{
		Time.timeScale = slowMotionFactor;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		StartCoroutine(NoMoreGameOverEffect());
	}

	private IEnumerator NoMoreGameOverEffect()
	{
		yield return new WaitForSeconds(0.45f);
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}
}
