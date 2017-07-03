using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerStatus : MonoBehaviour {
	private enum States
	{
		Playing, Menu, Dead
	}
	// Use this for initialization
	public float maxHp;
	private float _hp;
	private float _targetVignette = 0;
	public PostProcessingBehaviour PostScript;
	private VignetteModel.Settings _settings;
	private DateTime _lastHit;
	public float RecoveryDelay = 2;
	private States _state;
	public GameObject DeadMenu;
	public AudioSource Source;

	public float MaxHp
	{
		get { return maxHp; }
		set { maxHp = value; }
	}

	public bool IsDead()
	{
		return _state == States.Dead;
	}

	public float Hp
	{
		get { return _hp; }
		set
		{
			_hp = value;
			
		}
	}

	private void Awake()
	{
		Revive();
	}

	public void TakeBullet()
	{
		
		Hp--;
		if (Hp <= 0)
			return;
		Debug.Log(Hp);
		_targetVignette = 1-(Hp / MaxHp);
		_lastHit = DateTime.Now;
		Source.Play();


	}

	private void Update()
	{
		switch (_state)
		{
			case States.Playing:
			{

				_settings.intensity = Mathf.Lerp(_settings.intensity, _targetVignette, Time.deltaTime);
				PostScript.profile.vignette.settings = _settings;
				if (Hp <= 0)
				{
					_state = States.Dead;
					Time.timeScale = 0.02f;
					DeadMenu.SetActive(true);
				}
				else
				{
					if (_lastHit.AddSeconds(RecoveryDelay) < DateTime.Now && Hp < MaxHp)
					{
						_targetVignette = 1 - (Hp / MaxHp);
						Hp = Mathf.Lerp(Hp, MaxHp, Time.deltaTime * 25);

					}
				}
			}
				break;
			case States.Dead:
			{
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
				AudioListener.pause = true;


			}
				break;
		}
	}

	public void Revive()
	{
		Hp = MaxHp;
		_settings = PostScript.profile.vignette.settings;
		_settings.intensity = 0;
		PostScript.profile.vignette.settings = _settings;
		_state = States.Playing;
		Source = GetComponents<AudioSource>()[2];
		Time.timeScale = 1f;
		DeadMenu.SetActive(false);
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		AudioListener.pause = false;

	}

	
}
