using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class UserManager : MonoBehaviour
{
    Action<string> _createUserCallback;


    // Start is called before the first frame update
    void Start()
    {
        _createUserCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateUserRoutine(jsonArrayString));
        };

        CreateUser();
    }

    public void CreateUser()
    {
        string userID = Main.Instance.UserInfo.UserID;
        StartCoroutine(Main.Instance.Web.GetUsers(userID, _createUserCallback));
    }

    public IEnumerator CreateUserRoutine(string jsonArrayString)
    {
        //Parsing json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;    // are we done downloading?
            string userId = jsonArray[i].AsObject["id"];

            string username = jsonArray[i].AsObject["username"];

            JSONObject userInfoJson = new JSONObject();

            //Create a callback to get the information from Web.cs
            Action<string> getUserInfoCallback = (userInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(userInfo) as JSONArray;
                userInfoJson = tempArray[0].AsObject;
            };

            //Wait until Web.cs calls the callback we passed as parameter
            //StartCoroutine(Main.Instance.Web.GetUsers(userID, _createUserCallback));

            //Wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            //Instantiate GameObject (item prefab)
            GameObject userGo = Instantiate(Resources.Load("Prefabs/User") as GameObject);
            UserInfo user = userGo.AddComponent<UserInfo>();

            //user.UserID = id;
            //user.ItemID = itemId;

            userGo.transform.SetParent(this.transform);
            userGo.transform.localScale = Vector3.one;
            userGo.transform.localPosition = Vector3.zero;

            //Fill information
            userGo.transform.Find("Username").GetComponent<Text>().text = userInfoJson["username"];
            userGo.transform.Find("Level").GetComponent<Text>().text = userInfoJson["level"];
            userGo.transform.Find("Coins").GetComponent<Text>().text = userInfoJson["coins"];

        }
    }
}
