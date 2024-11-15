using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    [SerializeField] private bool _playOnAwake;


    private void Awake()
    {
        if( _playOnAwake)
        {
            Animator animator = GetComponent<Animator>();
            animator.Play("TransitionIn");
        }
    }
}
