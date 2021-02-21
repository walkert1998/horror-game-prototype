using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerLogin : MonoBehaviour
{
    public string password;
    public TMP_InputField passwordField;
    public Button enterButton;
    // Start is called before the first frame update
    void Start()
    {
        passwordField.text = "";
        enterButton.onClick.AddListener(
            ()=>
            {
                Debug.Log(password + " " + passwordField.text);
                EnterPassword(passwordField.text);
            }
        );
    }

    public void EnterPassword(string value)
    {
        if (value.Equals(password))
        {
            gameObject.SetActive(false);
        }
        else
        {
            passwordField.text = "";
        }
    }
}
