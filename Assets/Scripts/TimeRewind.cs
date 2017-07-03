using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TimeRewind : MonoBehaviour
{
    private PilhaEncadeada<Vector3> _posicoes;
    private PilhaEncadeada<Quaternion> _rotacoes;
    private Vector3 _ultimaPosicao;
    private Quaternion _ultimaRotacao;

    public bool IsRewinding = false;
    public float MRepeatMultiplier = 15f;
    public float RRepeatMultiplier = 15f;
    private FirstPersonController _pController;
    private Kino.AnalogGlitch _glitchScript;
    public AudioSource[] Sources;
    public AudioClip Base, RewindSound;

    // Use this for initialization
    void Awake()
    {
        Sources = GetComponents<AudioSource>();
        _glitchScript = Camera.main.GetComponent<Kino.AnalogGlitch>();
        _pController = GetComponent<FirstPersonController>();
        _posicoes = new PilhaEncadeada<Vector3>();
        _rotacoes = new PilhaEncadeada<Quaternion>();
        _ultimaPosicao = transform.position;
        _ultimaRotacao = transform.rotation;
        _posicoes.Inserir(_ultimaPosicao);
        _rotacoes.Inserir(_ultimaRotacao);
        Sources[1].clip = Base;
        Sources[1].Play();



    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        InvokeRepeating("Recorder", 0, 0.2f);

    }
    // Update is called once per frame
    void Update()
    {
        if (!IsRewinding)
        {
            if (Input.GetMouseButton(1) && !_posicoes.Vazia())
            {
                _glitchScript.enabled = true;
                _pController.enabled = false;
                IsRewinding = true;
                Sources[1].clip = RewindSound;
                Sources[1].Play();
                Sources[1].pitch = -2f;


            }
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                if (!_posicoes.Vazia())
                {
                    Sources[1].pitch = Mathf.Lerp(Sources[1].pitch, 0, Time.deltaTime*2);
                    if (Vector3.Distance(transform.position, _ultimaPosicao) < 1f)
                    {
                        _posicoes.Retirar(out _ultimaPosicao);
                        _rotacoes.Retirar(out _ultimaRotacao);

                        Debug.Log("Sobrepondo");

                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, _ultimaPosicao, Time.smoothDeltaTime * MRepeatMultiplier);
                        transform.rotation = Quaternion.Lerp(transform.rotation, _ultimaRotacao, Time.smoothDeltaTime * RRepeatMultiplier);
                    }
                }
                else
                {
                    _pController.enabled = true;
                    _glitchScript.enabled = false;
                    _pController.ReturnFromRewind();
                    Sources[1].clip = Base;
                    
                    Sources[1].pitch = 1.45f;
                }
            }
            else
            {
                _pController.enabled = true;
                _glitchScript.enabled = false;
                _pController.ReturnFromRewind();
                _posicoes.Esvaziar();
                _rotacoes.Esvaziar();
                IsRewinding = false;
                Sources[1].clip = Base;
                Sources[1].Play();
                Sources[1].pitch = 1.45f;


            }
        }


    }
    void Recorder()
    {
        if (IsRewinding) return;
        _ultimaPosicao = transform.position;
        _ultimaRotacao = transform.rotation;
        _posicoes.Inserir(_ultimaPosicao);
        _rotacoes.Inserir(_ultimaRotacao);
        Debug.Log("NovapOSICAO");
    }

}
