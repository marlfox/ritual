﻿using System.Collections;

using UnityEngine;

public class GamePlayState : FSMState {
	private CharacterController _player;
	private AudioSource _playerSource;
	private float _choirZ;

	private float SPEED = 0.2f;

    public GamePlayState()
        : base(GameState.GamePlay) {

    }

    public override void InitState(FSMTransition transition) {
        base.InitState(transition);

    }

    public override void EnterState(FSMTransition transition) {
        base.EnterState(transition);

		_player = GameData.Player.GetComponent<CharacterController>();
		_playerSource = _player.GetComponent<AudioSource>();

		_choirZ = GameData.Monks[0].transform.position.z;
    }

	public override void ExitState(FSMTransition transition) {
		_player = null;
		_playerSource = null;

		_choirZ = -1.0f;

		base.ExitState(transition);
	}

	public override void Update() {
		MovePlayer();
		PlayPlayerNote();

		if(HasJoinedChorus()) {
			if(HasCorrectPlacement()) {
				ExitState(new FSMTransition(GameState.GameWin));
			} else {
				ExitState(new FSMTransition(GameState.GameLose));
			}
		}
	}

	private void MovePlayer() {
		int h = 0;
		int v = 0;

		if(Input.GetKey(KeyCode.LeftArrow)) {
			h -= 1;
		}

		if(Input.GetKey(KeyCode.RightArrow)) {
			h += 1;
		}

		if(Input.GetKey(KeyCode.UpArrow)) {
			v += 1;
		}

		if(Input.GetKey(KeyCode.DownArrow)) {
			v -= 1;
		}

		_player.Move(new Vector3(h * SPEED, 0, v * SPEED));
	}

	private void PlayPlayerNote() {
		if(Input.GetKey(KeyCode.Space)) {
			if(!_playerSource.isPlaying) {
				_playerSource.volume = 1;
				_playerSource.Play();
			}
		} else {
			_playerSource.volume = 0;
			_playerSource.Stop();
		}
	}

	private bool HasJoinedChorus() {
		return _player.transform.position.z >= _choirZ;
	}

	private bool HasCorrectPlacement() {
		GameObject leftMonk = GameData.Monks[GameData.PlayerNote - 1];
		GameObject rightMonk = GameData.Monks[GameData.PlayerNote];

		float xMin = leftMonk.transform.position.x;
		float xMax = rightMonk.transform.position.x;
		float playerX = _player.transform.position.x;

		return playerX > xMin && playerX < xMax;
	}
}
