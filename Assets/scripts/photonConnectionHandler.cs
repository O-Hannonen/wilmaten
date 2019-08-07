using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class photonConnectionHandler : MonoBehaviour {

    private string versionName = "0.1";
    public GameObject mainMenu;
    public GameObject joinRoom;
    public static string errorMessage = "";
    public TextMeshProUGUI error;
    public GameObject errorObj;
    public static bool inRoom, connected, inLobby;
    public TMP_InputField roomNameInput;
    public string roomName;
    public GameObject loadingAnimation;

    public List<Button> buttons;
    public Button playButton;
    public int buttonIndex;

    private void Awake()
    {
        mainMenu.SetActive(true);
        joinRoom.SetActive(false);
        errorObj.SetActive(false);
        loadingAnimation.SetActive(false);
    }
    public Color32 getColor(int hexval)
    {
        byte R = (byte)((hexval >> 16) & 0xFF);
        byte G = (byte)((hexval >> 8) & 0xFF);
        byte B = (byte)((hexval) & 0xFF);

        return new Color32(R, G, B, 255);
    }


    private void Update()
    {
        inRoom = PhotonNetwork.inRoom;
        inLobby = PhotonNetwork.insideLobby;
        connected = PhotonNetwork.connected;

        if(mainMenu.activeInHierarchy == true)
        {
            if (Input.GetKeyDown("up"))
            {
                if (buttonIndex >= 1)
                {
                    buttonIndex--;
                }
            }
            else if (Input.GetKeyDown("down"))
            {
                if (buttonIndex < buttons.Count - 1)
                {
                    buttonIndex++;
                }
            }
            buttons[buttonIndex].Select();

            foreach (Button btn in buttons)
            {
                Image img = btn.GetComponentInParent<Image>();
                if (buttons.IndexOf(btn) == buttonIndex)
                {
                    img.color = getColor(5080064);

                }
                else
                {
                    img.color = getColor(9810539);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(roomNameInput.text == "")
                {
                    roomNameInput.Select();
                }
                else if(roomNameInput.text != "")
                {
                    playButton.Select();
                }
            }
        }
        

        

        if (errorMessage == "")
        {
            errorObj.SetActive(false);
        }
        else
        {
            error.text = errorMessage;
            errorObj.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "mainMenu")
        {
            foreach (char letter in Input.inputString)
            {
                if (letter.ToString() == "2")
                {
                    connectToPhoton();
                }
                
            }
        }
    }


    public void connectToPhoton()
    {
        mainMenu.SetActive(false);
        loadingAnimation.SetActive(true);
        //jos pelaaja on jo huoneessa, kiinni serverissä tai lobbyssä, ja koittaa uudellleen päästä, se ei onnistu
        //siksi valikon auetessa pitää aina poistaa vanhat connectionit
        if (inRoom)
        {
            PhotonNetwork.LeaveRoom();
            inRoom = false;
        }

        if (connected)
        {
            PhotonNetwork.Disconnect();
            connected = false;
        }

        if (inLobby)
        {
            PhotonNetwork.LeaveLobby();
            inLobby = false;
        }
        
        PhotonNetwork.ConnectUsingSettings(versionName);
        errorMessage = "";

    }

    public void back()
    {
        if (inLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        if (connected)
        {
            PhotonNetwork.Disconnect();
        }
        if (inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        mainMenu.SetActive(true);
        joinRoom.SetActive(false);
        errorObj.SetActive(false);
        loadingAnimation.SetActive(false);
    }

    private void OnConnectedToMaster()
    {
        //tätä kutsutaan automaattisesti kun yhteys photon servereille on saatu
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        errorMessage = "";
    }

    private void OnJoinedLobby()
    {
        //kaikki yhteydet ovat kunnossa, eli pelaaja voi luoda tai liittyä huoneeseen
        mainMenu.SetActive(false);
        loadingAnimation.SetActive(false);
        joinRoom.SetActive(true);
        errorMessage = "";
    }

    private void OnDisconnectedFromPhoton()
    {
        //yhteys katkee kesken kaiken
        //errorMessage = "Yhteys katkesi. Varmista internetyhteytesi.";
    }

    private void OnFailedToConnectToPhoton()
    {
        //netti ongelma yhdistäessä
        errorMessage = "Ei internetyhteyttä. Sinun tulee olle yhteydessä internettiin pelataksesi moninpeliä.";
    }


    public void joinRoomButton()
    {
        if (roomName.Length >= 1)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        else
        {
            errorMessage = "Syötä huoneen nimi syöttökenttään";
        }
    }

    private void OnJoinedRoom()
    {
        errorMessage = "";
        PhotonNetwork.LoadLevel("Multiplayer");

    }

    public void onTextFieldValueChanged(string value)
    {
        roomName = value;   
    }
}
