﻿using System;
using System.Collections.Generic;

namespace VertigoGamesCase.Game.Scripts
{
    public static class EventManager
    {
        private static Dictionary<string, Action> eventDictionary = new();
        private static Dictionary<string, Action<object>> eventDictionaryParam = new();

        public static void Subscribe(string eventName, Action listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
                eventDictionary.Add(eventName, listener);
            else
                eventDictionary[eventName] += listener;
        }

        public static void Unsubscribe(string eventName, Action listener)
        {
            if (eventDictionary.ContainsKey(eventName))
                eventDictionary[eventName] -= listener;
        }

        public static void TriggerEvent(string eventName)
        {
            if (eventDictionary.ContainsKey(eventName))
                eventDictionary[eventName]?.Invoke();
        }

        public static void Subscribe<T>(string eventName, Action<T> listener)
        {
            if (!eventDictionaryParam.ContainsKey(eventName))
                eventDictionaryParam.Add(eventName, o => listener((T)o));
            else
                eventDictionaryParam[eventName] += o => listener((T)o);
        }

        public static void Unsubscribe<T>(string eventName, Action<T> listener)
        {
            if (eventDictionaryParam.ContainsKey(eventName))
                eventDictionaryParam[eventName] -= o => listener((T)o);
        }

        public static void TriggerEvent<T>(string eventName, object obj)
        {
            if (eventDictionaryParam.ContainsKey(eventName))
                eventDictionaryParam[eventName]?.Invoke(obj);
        }
    }
}