using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class ChickenManager : MonoBehaviour
{
    Action<string> _createChickensCallback;


    // Start is called before the first frame update
    void Start()
    {
        _createChickensCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateChickensRoutine(jsonArrayString));
        };

        CreateChickens();
    }

    public void CreateChickens()
    {
        string userID = Main.Instance.UserInfo.UserID;
        StartCoroutine(Main.Instance.Web.GetChickensIDs(userID, _createChickensCallback));
    }

    public IEnumerator CreateChickensRoutine(string jsonArrayString)
    {
        //Parsing json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;    // are we done downloading?
            string chickenId = jsonArray[i].AsObject["chickenID"];

            string id = jsonArray[i].AsObject["Id"];

            JSONObject chickenInfoJson = new JSONObject();

            //Create a callback to get the information from Web.cs
            Action<string> getChickenInfoCallback = (chickenInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(chickenInfo) as JSONArray;
                chickenInfoJson = tempArray[0].AsObject;
            };

            //Wait until Web.cs calls the callback we passed as parameter
            StartCoroutine(Main.Instance.Web.GetChicken(chickenId, getChickenInfoCallback));

            //Wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            //Instantiate GameObject (chicken prefab)
            GameObject chickGo = Instantiate(Resources.Load("Prefabs/Chicken") as GameObject);
            Chicken chicken = chickGo.AddComponent<Chicken>();

            chicken.Id = id;
            chicken.chickenID = chickenId;

            chickGo.transform.SetParent(this.transform);
            chickGo.transform.localScale = Vector3.one;
            chickGo.transform.localPosition = Vector3.zero;

            //Fill information
            chickGo.transform.Find("Type").GetComponent<Text>().text = chickenInfoJson["type"];
            chickGo.transform.Find("Color").GetComponent<Text>().text = chickenInfoJson["color"];
            chickGo.transform.Find("Price").GetComponent<Text>().text = chickenInfoJson["price"];
            chickGo.transform.Find("Personality").GetComponent<Text>().text = chickenInfoJson["personality"];

            //Create a callback to get the sprite from Web.cs
            Action<Sprite> getChickenIconCallback = (downloadedSprite) =>
            {
                chickGo.transform.Find("Image").GetComponent<Image>().sprite = downloadedSprite;
            };

            StartCoroutine(Main.Instance.Web.GetChickenIcon(chickenId, getChickenIconCallback));

            //Set sell button
            chickGo.transform.Find("Buy").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject chickGo = Instantiate(Resources.Load("Prefabs/Chick_1") as GameObject);
                GameObject.Find("Chick 2 icon").SetActive(true);
            });

            //Set buy button
           

            //continue to the next item

        }
    }
}
