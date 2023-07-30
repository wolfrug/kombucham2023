using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class SayDialogChatbox : SayDialog {
    [Tooltip ("This is the panel that will be copied - should basically be the Panel object in the default Say dialog.")]
    public GameObject m_chatBoxPanel;
    [Tooltip ("This is a scroll rect that contains all the 'saved' panels.")]
    public ScrollRect m_chatBoxParent;
    [Tooltip ("Do you want to scroll up or scroll down - this determines whether copies are copied on top or bottom")]
    public bool m_scrollDown = true;

    public override void Say (string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, bool stopVoiceover, bool waitForVO, AudioClip voiceOverClip, Action onComplete) {
        if (m_chatBoxParent != null && m_chatBoxPanel != null) { // null checks the necessary components - if these are null, it just acts like a normal say dialog
            onComplete = onComplete += delegate { CopyCurrentPanel (); }; // adds the script to the oncomplete delegate
            m_chatBoxPanel.transform.SetParent (m_chatBoxParent.content, true); // sets the chat panel to be parented to the content thingie
            if (m_scrollDown) {
                m_chatBoxPanel.transform.SetAsLastSibling (); // sets the panel as the first/last sibling depending on if we're scrolling up or down
            } else { // or the reverse if scrolling up;
                m_chatBoxPanel.transform.SetAsFirstSibling ();
            }
            UpdateLayout (m_chatBoxParent.transform); // a little update script to make sure everything is hunky dory
            m_chatBoxParent.verticalNormalizedPosition = m_scrollDown ? 0f : 1f; // sets it to scroll up or down depending on setting
        } else {
            Debug.LogWarning ("The Say Dialogue Chatbox requires the Scroll View and the Chat Box Panel to be designated to work!", gameObject);
        }
        base.Say (text, clearPrevious, waitForInput, fadeWhenDone, stopVoiceover, waitForVO, voiceOverClip, onComplete); // and all the base say stuff
    }
    public void CopyCurrentPanel () {
        // We have to do a little messing around to get the current portrait image to copy over properly, because of the use of the .overrideSprite property
        Sprite originalSprite = CharacterImage.sprite;
        CharacterImage.sprite = CharacterImage.overrideSprite;
        // Hides the Continue  button, if set, so it's not copied over
        if (continueButton != null) {
            continueButton.gameObject.SetActive (false);
        }
        GameObject copiedPanel = Instantiate (m_chatBoxPanel, m_chatBoxParent.content, false); // copies the panel as it is
        // Unhides the Continue  button, if set (on the original panel)
        if (continueButton != null) {
            continueButton.gameObject.SetActive (false);
        }
        // resets the characterimage on the original panel
        CharacterImage.sprite = originalSprite;
    
        if (m_scrollDown) {
            copiedPanel.transform.SetAsLastSibling (); // first sets the copied panel as last sibling
            m_chatBoxPanel.transform.SetAsLastSibling (); // then sets the parent panel as last sibling
        } else { // or the reverse if scrolling up;
            copiedPanel.transform.SetAsFirstSibling ();
            m_chatBoxPanel.transform.SetAsFirstSibling ();
        }
        // Return chatboxpanel to its proper parent
        m_chatBoxPanel.transform.SetParent (dialogCanvas.transform, true);
        // Update the layout
        UpdateLayout (m_chatBoxParent.transform);
        m_chatBoxParent.verticalNormalizedPosition = m_scrollDown ? 0f : 1f; // sets it to scroll up or down depending on setting
    }
    public void UpdateLayout (Transform xform) { // this basically handles various content size fitter/Layout group nonsense
        Canvas.ForceUpdateCanvases ();
        if (xform == null || xform.Equals (null)) {
            return;
        }

        // Update children first
        for (int x = 0; x < xform.childCount; ++x) {
            UpdateLayout (xform.GetChild (x));
        }

        // Update any components that might resize UI elements
        foreach (var layout in xform.GetComponents<LayoutGroup> ()) {
            layout.CalculateLayoutInputVertical ();
            layout.CalculateLayoutInputHorizontal ();
        }
        foreach (var fitter in xform.GetComponents<ContentSizeFitter> ()) {
            fitter.SetLayoutVertical ();
            fitter.SetLayoutHorizontal ();
        }
    }
}