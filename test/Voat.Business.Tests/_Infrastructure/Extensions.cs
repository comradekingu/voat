using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voat.Common;

namespace Voat.Business.Tests.Infrastructure
{
    public static class TestExtensions
    {
        public static string Repeat(this string s, int n)
        {
            return new String(Enumerable.Range(0, n).SelectMany(x => s).ToArray());
        }
        public static string RepeatUntil(this string s, int n)
        {
            var times = n / s.Length + 1;
            var result = new String(Enumerable.Range(0, times).SelectMany(x => s).ToArray());
            return result.Substring(0, n);
        }
        public static string GetUnitTestUserPassword(this string userName)
        {
            var pwd = userName;
            while (pwd.Length < 6)
            {
                pwd += userName;
            }
            return pwd;
        }
        /// <summary>
        /// Compares a flat object against provided Json
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="json"></param>
        /// <param name="excludedProperties"></param>
        public static object AssertObjectEqualsJson(this object instance, string json, JsonSerializerSettings settings, params string[] excludedProperties)
        {
            settings = settings ?? new JsonSerializerSettings();

            var newObject = Activator.CreateInstance(instance.GetType());

            JsonConvert.PopulateObject(json, newObject, settings);

            string[] includeProperties = null;

            //Try to limit the properties to only what is included in json but this might deserialize into correctly typed object if the appropriate json is provided so we check
            var d = JsonConvert.DeserializeObject(json, settings);
            if (d is JObject jobject)
            {
                var tokens = jobject.Children().ToList();
                includeProperties = tokens.Select(t => t.Path).ToArray();
            }

            excludedProperties = excludedProperties ?? new string[] { };

            var props = instance.GetType().GetProperties().Where(x => (includeProperties == null || includeProperties.Any(e => e.IsEqual(x.Name))) && !excludedProperties.Any(e => e.IsEqual(x.Name)));

            foreach (var prop in props)
            {
                Assert.AreEqual(prop.GetValue(newObject), prop.GetValue(instance), prop.Name);
            }

            return newObject;
        }
    }
}
