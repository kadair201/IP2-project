﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerSelector : MonoBehaviour
{
    public bool isHovered = false;
    public bool isSelected = false;
    public int selectedPlayer = -1;

    public Animator animator;
    public Image highlighter;
    public Text playerText;

    PlayerSelectionData playerSelectionData;

    List<CursorScript> cursorsHovering = new List<CursorScript>();

    public PlayerType playerType;

    public void Start()
    {
        playerSelectionData = GameObject.Find("PlayerSelectionData").GetComponent<PlayerSelectionData>();
    }

    public void Hover()
    {
        if (isSelected) return;

        if (!isHovered)
        {
            isHovered = true;
            animator.Play("hover");
        }
    }

    public void Unhover()
    {
        if (isSelected) return;

        if (isHovered)
        {
            isHovered = false;
            animator.Play("unhover");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Hover();
        cursorsHovering.Add(collision.GetComponent<CursorScript>());
        collision.GetComponent<CursorScript>().currentlyHoveredSelector = this;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        cursorsHovering.Remove(collision.GetComponent<CursorScript>());
        collision.GetComponent<CursorScript>().currentlyHoveredSelector = null;

        // only unhover if there are no cursors sitting on this boi
        if (cursorsHovering.Count == 0) Unhover();
    }

    public void Unselect()
    {
        if (isSelected)
        {
            isSelected = false;
            animator.Play("unselect");
        }
    }

    public void Select(int playerNumber)
    {
        selectedPlayer = playerNumber;

        if (!isSelected)
        {
            isHovered = false;
            isSelected = true;
            highlighter.color = playerSelectionData.playerColors[selectedPlayer];
            playerText.text = "P" + (selectedPlayer + 1);
            animator.Play("select");
        } 
    }
}
