using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class ItemManager : MonoBehaviour
{
    Action<string> _createItemsCallback;


    // Start is called before the first frame update
    void Start()
    {
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };

        CreateItems();
    }

    public void CreateItems()
    {
        string userID = Main.Instance.UserInfo.UserID;
        StartCoroutine(Main.Instance.Web.GetItemsIDs(userID, _createItemsCallback));
    }

    public IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        //Parsing json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;    // are we done downloading?
            string itemId = jsonArray[i].AsObject["itemID"];
            
            string id = jsonArray[i].AsObject["ID"];

            JSONObject itemInfoJson = new JSONObject();

            //Create a callback to get the information from Web.cs
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            //Wait until Web.cs calls the callback we passed as parameter
            StartCoroutine(Main.Instance.Web.GetItem(itemId, getItemInfoCallback));

            //Wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            //Instantiate GameObject (item prefab)
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;

            itemGo.transform.SetParent(this.transform);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;

            //Fill information
            itemGo.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            itemGo.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            itemGo.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];

            //Create a callback to get the sprite from Web.cs
            Action<Sprite> getItemIconCallback = (downloadedSprite) =>
            {
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = downloadedSprite;
            };

            StartCoroutine(Main.Instance.Web.GetItemIcon(itemId, getItemIconCallback));

            //Set sell button
            itemGo.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                string idInInventory = id;
                string iId = itemId;
                string userId = Main.Instance.UserInfo.UserID; ;

                StartCoroutine(Main.Instance.Web.SellItem(idInInventory, itemId, userId));
            });

            //Set buy button
            itemGo.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                string idInInventory = id;
                string iId = itemId;
                string userId = Main.Instance.UserInfo.UserID; ;

                StartCoroutine(Main.Instance.Web.BuyItem(idInInventory, itemId, userId));
            });

            //continue to the next item

        }
    }
}