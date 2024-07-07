using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Cutscene1 : MonoBehaviour
{
    public PlayableDirector director;
    public TimelineAsset clip;
    public Animator cameraAnimator;

    public GameObject playerRef;
    List<Component> playerComponents;
    
    PlayerAnimation playerAnimation;
    PlayerCamera cam;
    Player1 player1;
    PlayerInteract playerInteract;
    PlayerTask playerTask;
    Inventory inventory;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        DisablePlayer();
        StartCoroutine(Cutscene01());
    }

    void DisablePlayer() 
    {
        playerAnimation = playerRef.GetComponent<PlayerAnimation>();
        if (playerAnimation != null) playerAnimation.enabled = false;

        cam = playerRef.GetComponentInChildren<PlayerCamera>();
        if (cam != null)
        {
            cam.enabled = false;
            playerRef.GetComponentInChildren<Camera>().enabled = false;
        }

        player1 = playerRef.GetComponent<Player1>();
        if (player1 != null) player1.enabled = false;

        playerInteract = playerRef.GetComponent<PlayerInteract>();
        if (playerInteract != null) playerInteract.enabled = false;

        playerTask = playerRef.GetComponent<PlayerTask>();
        if (playerTask != null) playerTask.enabled = false;

        inventory = playerRef.GetComponent<Inventory>();
        if (inventory != null) inventory.enabled = false;

    }

    void EnablePLayer()
    {
         if (playerAnimation != null) playerAnimation.enabled = true;
        if (cam != null)
        {
            cam.enabled = true;
            playerRef.GetComponentInChildren<Camera>().enabled = true;
        }
        if (player1 != null) player1.enabled = true;
        if (playerInteract != null) playerInteract.enabled = true;
        if (playerTask != null) playerTask.enabled = true;
        if (inventory != null) inventory.enabled = true;
        
    }

    IEnumerator Cutscene01()
    {
        GameObject camera = GameObject.Find("Cutscene01");
        director.Play(clip);
        yield return new WaitForSeconds(5.6f);

        EnablePLayer();
        camera.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
