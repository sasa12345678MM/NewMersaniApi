using Mersani.models.Finance;
using Mersani.Oracle;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mersani.Utility
{
    public static class SerializeEntity
    {
        public static T MapEntity<T>(T entity)
        {
            //object obj = null;
            Dictionary<string, dynamic> props = PropertiesFromType(entity);
            for (int i = 0; i < props.Count; i++)
            {
                //props[i] = FormatStingToStandard(props[i]);
                string key = props.Keys.ElementAt(i);
                var value = props[props.Keys.ElementAt(i)];
                key = FormatStingToStandard(key);
            }

            return entity;
        }

        private static Dictionary<string, dynamic> PropertiesFromType(object atype)
        {
            if (atype == null) return new Dictionary<string, dynamic>(); //string[] { };
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> propNames = new List<string>();
            Dictionary<string, dynamic> propsData = new Dictionary<string, dynamic>();
            foreach (PropertyInfo prp in props)
            {
                propNames.Add(prp.Name);
                propsData.Add(prp.Name, prp.GetValue(t, null));
            }
            return propsData;//propNames.ToArray();
        }
        private static string FormatStingToStandard(string prop)
        {
            string finalValue = "";
            string[] keys = prop.Split("_");
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                key = key.ToLower();
                key = char.ToUpper(key[0]) + key.Substring(1);
                finalValue += key;
            }
            return finalValue;
        }

        public static T NormalizeKeys<T>(T originalObject)
        {
            var original = JObject.FromObject(originalObject);
            var newObject = new JObject();
            foreach (var kvp in original)
                newObject.Add(FormatStingToStandard(kvp.Key), kvp.Value);//.ToLower()
            return newObject.ToObject<T>();
        }

        /// <summary>
        /// entity to xml serializer
        /// </summary>

        public static string dateFormat = "yyyy/MM/dd HH:mm:ss";
        public static string Encode(List<dynamic> entities)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<PARAMETER_LIST>");
            foreach (var entity in entities)
            {
                Type type = entity.GetType(); 

                //Skipped  properties that  have ignoreJson attribute
                PropertyInfo[] propertyInfos = type.GetProperties()
                                               .Where(prop => !prop.IsDefined(typeof(JsonIgnoreAttribute), false)).ToArray();

                string entityName = GetTableName(entity.GetType());
                sb.AppendFormat("<PARAMETER>");

                foreach (var prop in propertyInfos)
                {
                    if (prop.Name == "JsonObject")
                    {
                        if (prop.GetValue(entity) != null)
                        {
                            dynamic propertyValue = JsonStringToXmlString(prop.GetValue(entity, null));
                            sb.Append(propertyValue);
                        }
                    }
                    else
                    {
                        dynamic propertyValue = FormatDateTime(prop.PropertyType, prop.GetValue(entity, null));
                        sb.AppendFormat("<{0}>{1}</{0}>", prop.Name, propertyValue);
                    }
                }

                sb.AppendFormat("</PARAMETER>");
            }
            sb.AppendFormat("</PARAMETER_LIST>").Append(Environment.NewLine);
            return sb.ToString();
        }

       
        public static string EncodeWithExcludeNull(List<dynamic> entities, OperationType? operation = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            foreach (var entity in entities)
            {
                Type type = entity.GetType();

                //Skipped  properties that  have ignoreJson attribute
                PropertyInfo[] propertyInfos = type.GetProperties()
                                               .Where(prop => !prop.IsDefined(typeof(JsonIgnoreAttribute), false)).ToArray();

                string entityName = GetTableName(entity.GetType());
                sb.AppendFormat("<{0}>", entityName);
                sb.AppendFormat("<{0}_row>", entityName);

                foreach (var prop in propertyInfos)
                {
                    if (prop.GetValue(entity, null) == null) continue;
                    dynamic propertyValue = FormatDateTime(prop.PropertyType, prop.GetValue(entity, null));
                    sb.AppendFormat("<{0}>{1}</{0}>", prop.Name, propertyValue);
                }
                if (operation.HasValue)
                {
                    sb.AppendFormat("<DataStatus>{0}</DataStatus>", (int)operation);
                }
                sb.AppendFormat("</{0}_row>", entityName);
                sb.AppendFormat("</{0}>", entityName);
            }
            sb.Append("</Root>");
            return sb.ToString();
        }
        public static dynamic FormatDateTime(dynamic o, dynamic value)
        {
            //if the property is dateTime or nullable dateTime and have value format it
            if (typeof(DateTime).GetTypeInfo().IsAssignableFrom(o) || typeof(DateTime?).GetTypeInfo().IsAssignableFrom(o))
            {
                if (value != null)
                {
                    DateTime dd = Convert.ToDateTime(value.ToString(dateFormat));
                    DateTime now = DateTime.Now;

                    string mth = dd.Month < 10 ? $"0{dd.Month}" : dd.Month.ToString();
                    if (dd.Hour == 0 && dd.Minute == 0 && dd.Second == 0)
                    {
                        dd.AddHours(now.Hour);
                        dd.AddMinutes(now.Minute);
                        dd.AddSeconds(now.Second);
                        return dd.Day + "-" + mth  + "-" + dd.Year; //value != null ? value.ToString(dateFormat) : null;
                    }
                    else
                    {
                        return dd.Day + "-" + mth + "-" + dd.Year;
                    }
                }
                return value != null ? value.ToString(dateFormat) : null;
            }
            return value;
        }
        private static string GetTableName(Type entityType)
        {
            var att = entityType.GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
            if (att != null)
            {
                return att.Name;
            }
            return entityType.Name;
        }
        public static string JsonStringToXmlString(string jsonString)
        {
            DateTime dateValue;
            float floatValue;
            string dynamicProp = "";
            if (jsonString != null)
            {
                var d = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
                foreach (var key in d.Keys)
                {
                    string dd = Convert.ToString(d[key]);


                    if (!float.TryParse(dd, out floatValue) && DateTime.TryParse(Convert.ToString(d[key]), out dateValue))
                    {
                        if (key.EndsWith("Str"))
                        {
                            dd = Convert.ToString(d[key]);
                        }
                        else
                            dd = d[key].ToString(SerializeEntity.dateFormat);

                    }


                    string fisrtUpper = "";
                    fisrtUpper = FirstCharToUpper(key);
                    dynamicProp = $"{dynamicProp}<{fisrtUpper}>{dd}</{fisrtUpper}>";
                }

            }
            return dynamicProp;
        }
        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }



        public static dynamic ConvertDateToDbDate(dynamic value)
        {
            if (value != null)
            {
                DateTime dd = Convert.ToDateTime(value.ToString(dateFormat));
                DateTime now = DateTime.Now;

                string mth = dd.Month < 10 ? $"0{dd.Month}" : dd.Month.ToString();
                if (dd.Hour == 0 && dd.Minute == 0 && dd.Second == 0)
                {
                    dd.AddHours(now.Hour);
                    dd.AddMinutes(now.Minute);
                    dd.AddSeconds(now.Second);
                    return dd.Day + "-" + mth + "-" + dd.Year; //value != null ? value.ToString(dateFormat) : null;
                }
                else
                {
                    return dd.Day + "-" + mth + "-" + dd.Year;
                }
            }
            return value != null ? value.ToString(dateFormat) : null;
        }



        public static string QueryStringToXML(List<dynamic> entities)
        {
            dynamic entity = entities.First();
            string encodedXml = "<PARAMETER_LIST><PARAMETER>";

            //var d = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(entity);

            StringBuilder sb = new StringBuilder("");

            // Iterate through the Dictionary.
            foreach (string key in entity.Keys)
            {
                // check if the value is not null or empty.
                if (!string.IsNullOrEmpty(key))
                {
                    string tag = key;
                    var val = entity[key];
                    sb.Append("<" + tag + ">" + val + "</" + tag + ">");
                }
            }
            
            // Write the result to a label.
            encodedXml = encodedXml + sb.ToString();
            encodedXml = encodedXml + "</PARAMETER></PARAMETER_LIST>";
            return encodedXml;
        }

        public static string UpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

    }
}
