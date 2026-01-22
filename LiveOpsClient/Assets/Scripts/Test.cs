using System;
using CunningFox.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace CunningFox
{
    public class Test : MonoBehaviour
    {
        private async void OnEnable()
        {
            var request = UnityWebRequest.Get("https://localhost:7158/active-liveops");
            await request.SendWebRequest();
            var calendar = JsonConvert.DeserializeObject<LiveOpCalendarDto>(request.downloadHandler.text);
            var timeDifference = calendar.ServerTime - DateTime.UtcNow.Ticks;
            var timeDifferenceString = TimeSpan.FromTicks(timeDifference);
            Debug.Log(calendar);
        }
    }
}