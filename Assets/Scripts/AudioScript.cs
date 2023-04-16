using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private AudioSource _harpoonShot;
    [SerializeField] private InputActionReference _shotPres;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_shotPres.action.IsPressed())
        {
            PlayAudio();
        }
    }

    void PlayAudio()
    {
        
    }
}
