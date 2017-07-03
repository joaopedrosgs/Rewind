using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

	private float Score = 0;
	public GameObject Player;
	public PlayerStatus status;
	public Text ScoreText;

	private void Awake()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		status = Player.GetComponent<PlayerStatus>();
	}
	


	public void IncrementScore()
	{
		
		StartCoroutine(IncScore(10));
	}

	private void Update()
	{
		if (status.IsDead())
		{
			if(Input.GetKey(KeyCode.Return))
				Restart();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			SceneManager.LoadScene(0);
		}
	}

	public IEnumerator IncScore(float quantity)
	{
		float newScore = Score + quantity;
		for (float timer = 0; timer < 0.5f; timer += Time.deltaTime)
		{
			float progress = timer / 0.5f;
			Score = (int) Mathf.Lerp(Score, newScore, progress);
			ScoreText.text = Score.ToString("0");
			yield return null;
		}
		ScoreText.text = newScore.ToString("0");
		Score = newScore;
	}

	public void Restart()
	{
		Score = 0;
		ScoreText.text = "0";
		Player.GetComponent<PlayerStatus>().Revive();
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (var enemy in enemies)
		{
			Destroy(enemy);
		}


	}
}
