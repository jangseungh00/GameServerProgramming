using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldId;
    [SerializeField] private TMP_InputField _inputFieldPw;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _joinButton;

    public static event Action<string, string> OnJoinButtonClick;
    public static event Action<string, string> OnLoginButtonClick;


    private string _id;
    private string _pw;

    private void Awake()
    {
       _inputFieldId = transform.Find("InputField (TMP)-id").GetComponent<TMP_InputField>();
       _inputFieldPw = transform.Find("InputField (TMP)-pw").GetComponent<TMP_InputField>();
        _loginButton = transform.Find("Button (Legacy)-Login").GetComponent<Button>();
        _joinButton = transform.Find("Button (Legacy)-join").GetComponent<Button>();
    }

    private void Start()
    {
        _joinButton.onClick.AddListener(HandleJoinButtonClick);
        _loginButton.onClick.AddListener(HandleLoginButtonClick);       
    }

    private void HandleJoinButtonClick()
    {
        string id = _inputFieldId.text;
        string pw = _inputFieldPw.text;

        OnJoinButtonClick?.Invoke(id, pw);
    }
    private void HandleLoginButtonClick()
    {
        string id = _inputFieldId.text;
        string pw = _inputFieldPw.text;

        OnLoginButtonClick?.Invoke(id, pw);
    }

}
