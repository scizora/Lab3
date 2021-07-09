using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Text score;
	private int playerScore = 0;
	
	public void increaseScoreGoomba(){
		playerScore += 1;
		score.text = "SCORE: " + playerScore.ToString();
		OnGoombaDeath();
	}

	public void increaseScoreKoopa(){
		playerScore += 2;
		score.text = "SCORE: " + playerScore.ToString();
		OnKoopaDeath();
	}

	public void increaseScoreBrick(){
		playerScore += 1;
		score.text = "SCORE: " + playerScore.ToString();
		OnBrickCoinBreak();
	}

    public void damagePlayer(){
        OnPlayerDeath();
    }

    public delegate void gameEvent();

    public static event gameEvent OnPlayerDeath;
	public static event gameEvent OnGoombaDeath;
	public static event gameEvent OnKoopaDeath;
	public static event gameEvent OnBrickCoinBreak;
}