using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JointOffice.WorkFlow
{
    [Serializable]
    [XmlInclude(typeof(WF_ArraySetp))]
    [XmlInclude(typeof(WF_Begin))]
    [XmlInclude(typeof(WF_ConditinDefaultPath))]
    [XmlInclude(typeof(WF_ConditionPath))]
    [XmlInclude(typeof(WF_ConditionSetp))]
    [XmlInclude(typeof(WF_End))]
    [XmlInclude(typeof(WF_Setp))]
    [XmlInclude(typeof(WF_SingleSetp))]
    [XmlInclude(typeof(WF_Setps))]
    public class WF_Flow : WF_ArraySetp
    {
        List<KeyValuePair<string, string>> para = new List<KeyValuePair<string, string>>();
        WF_Setps _WF_Setps = null;
        WF_Begin _WF_Begin = null;
        WF_End _WF_End = null;

        public WF_Flow()
        {
            this.SetpId = Guid.NewGuid().ToString();
            this.SetpName = "审批流";
            this.SetpDesc = "审批流";
            _WF_Setps = new WF_Setps();
            _WF_Begin = new WF_Begin();
            _WF_Begin.SetpName = "开始";
            _WF_Begin.SetpDesc = "开始";
            _WF_End = new WF_End();
            _WF_End.SetpName = "结束";
            _WF_End.SetpDesc = "结束";
            _WF_Begin.Next_SetpId = _WF_End.SetpId;
            _WF_End.Pervious_SetpId = _WF_Begin.SetpId;
            _WF_Begin.Parent_Id = this.SetpId;
            _WF_End.Parent_Id = this.SetpId;
        }
        public new List<KeyValuePair<string, string>> WF_Flow_Parameter
        {
            get { return para; }
            set { para = value; }
        }

        public WF_Begin WF_Begin
        {
            get
            {
                return _WF_Begin;
            }
            set
            {
                _WF_Begin = value;
            }
        }
        public override WF_Setps WF_Setps { get { return _WF_Setps; } }
        public WF_End WF_End
        {
            get { return _WF_End; }
            set { _WF_End = value; }
        }
        /// <summary>
        /// 反序列化为xml
        /// </summary>
        public static WF_Flow LoadFormXML(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(WF_Flow));
            System.IO.StringReader sr = new System.IO.StringReader(xml);
            WF_Flow flow = serializer.Deserialize(sr) as WF_Flow;
            return flow;
        }
        /// <summary>
        /// 序列化为xml
        /// </summary>
        public static string SaveToXML(WF_Flow flow)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(WF_Flow));
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            xmlSerializer.Serialize(sw, flow);
            return sb.ToString();
        }
        /// <summary>
        /// 反序列化为json
        /// </summary>
        public static WF_Flow LoadFormJson(string json)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
            var WF = Newtonsoft.Json.JsonConvert.DeserializeObject<WF_Flow>(json, jsonSerializerSettings);
            return WF;
        }
        /// <summary>
        /// 序列化为json
        /// </summary>
        public static string SaveToJson(WF_Flow flow)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(flow, jsonSerializerSettings);
            return str;
        }
        /// <summary>
        /// 反序列化为byte
        /// </summary>
        public static WF_Flow LoadFormByte(byte[] bytes)
        {
            //反序列化
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            var obj = formatter.Deserialize(ms);
            ms.Close();
            var WF = obj as WF_Flow;
            return WF;
        }
        /// <summary>
        /// 序列化为byte
        /// </summary>
        public static byte[] SaveToByte(WF_Flow flow)
        {
            MemoryStream ms = new MemoryStream();
            //用于序列化和反序列化的对象
            BinaryFormatter serializer = new BinaryFormatter();
            //开始序列化
            serializer.Serialize(ms, flow);
            ms.Position = 0;
            byte[] bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return bytes;
        }

        public override WF_Setp GetStartWF_Setp
        {
            get { return WF_Begin; }
        }
        static Dictionary<string, Dictionary<string, string>> _WF_Flow_Parameters = new Dictionary<string, Dictionary<string, string>>();

        //解决HttpContext.Current
        public static class MyHttpContext
        {
            public static IServiceProvider ServiceProvider;
            static MyHttpContext()
            { }
            public static HttpContext Current
            {
                get
                {
                    object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                    HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                    return context;
                }
            }
        }

        public static Dictionary<string, string> WF_Flow_Parameters_Get(string WF_InstanceId)
        {
            if (MyHttpContext.Current == null)
            {
                var find = _WF_Flow_Parameters.Where(t => string.Equals(t.Key, WF_InstanceId)).FirstOrDefault();
                if (string.IsNullOrEmpty(find.Key))
                {
                    return null;
                }
                return find.Value;
            }
            else
            {
                if (MyHttpContext.Current.Session != null)
                {
                    try
                    {
                        var dic1 = MyHttpContext.Current.Session.GetString("WF_Flow_Parameters");
                        Dictionary<string, Dictionary<string, string>> dic = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dic1);
                        //var dic = MyHttpContext.Current.Session["WF_Flow_Parameters"] as Dictionary<string, Dictionary<string, string>>;
                        var find = dic.Where(t => string.Equals(t.Key, WF_InstanceId)).FirstOrDefault();
                        if (string.IsNullOrEmpty(find.Key))
                        {
                            return null;
                        }
                        return find.Value;
                    }
                    catch (Exception e)
                    {

                    }
                }
                return null;
            }
        }

        public static void WF_Flow_Parameters_Set(string WF_InstanceId, Dictionary<string, string> parameter)
        {
            if (MyHttpContext.Current == null)
            {
                var find = _WF_Flow_Parameters.Where(t => string.Equals(t.Key, WF_InstanceId)).FirstOrDefault();
                if (string.IsNullOrEmpty(find.Key))
                {
                    _WF_Flow_Parameters.Add(WF_InstanceId, parameter);
                }
                else
                {
                    find.Value.Clear();
                    foreach (var m in parameter)
                    {
                        find.Value.Add(m.Key, m.Value);
                    }
                }
            }
            else
            {
                if (MyHttpContext.Current.Session != null)
                {
                    try
                    {
                        var dic1 = MyHttpContext.Current.Session.GetString("WF_Flow_Parameters");
                        Dictionary<string, Dictionary<string, string>> dic = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dic1);
                        //var dic = MyHttpContext.Current.Session["WF_Flow_Parameters"] as Dictionary<string, Dictionary<string, string>>;
                        var find = dic.Where(t => string.Equals(t.Key, WF_InstanceId)).FirstOrDefault();
                        if (string.IsNullOrEmpty(find.Key))
                        {
                            dic.Add(WF_InstanceId, parameter);
                        }
                        else
                        {
                            find.Value.Clear();
                            foreach (var m in parameter)
                            {
                                find.Value.Add(m.Key, m.Value);
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        #region 行为处理
        public override void Add(WF_Setp item)
        {
            if (_WF_Setps != null && item != null)
            {
                var find = _WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (find != null)
                {
                    throw new Exception(string.Format("Id是{0}的节点已经存在于节点集合中", item.SetpId));
                }
                if (_WF_Setps.Count == 0)  ////如果起始在开始和结束间插入审批节点
                {
                    if (item is WF_ArraySetp)
                    {
                        WF_ArraySetp setp = item as WF_ArraySetp;
                        var endSetp = setp.Get_EndSetp();

                        item.Parent_Id = this.SetpId;
                        _WF_Begin.Next_SetpId = item.SetpId;
                        item.Pervious_SetpId = _WF_Begin.SetpId;
                        item.Next_SetpId = "";//endSetp.SetpId;
                        endSetp.Next_SetpId = _WF_End.SetpId;
                        _WF_End.Pervious_SetpId = endSetp.SetpId;
                    }
                    else
                    {
                        item.Parent_Id = this.SetpId;
                        _WF_Begin.Next_SetpId = item.SetpId;
                        item.Pervious_SetpId = _WF_Begin.SetpId;
                        item.Next_SetpId = _WF_End.SetpId;
                        _WF_End.Pervious_SetpId = item.SetpId;
                    }
                }
                else  ////否则,添加到最后一个节点和WF_End节点之前
                {
                    var lastItem = _WF_Setps[_WF_Setps.Count - 1];
                    item.Parent_Id = this.SetpId;
                    if (item is WF_ArraySetp)
                    {
                        WF_ArraySetp setp = item as WF_ArraySetp;
                        var endSetp = setp.Get_EndSetp();

                        if (lastItem is WF_ArraySetp)
                        {
                            var temS = lastItem as WF_ArraySetp;
                            temS.Get_EndSetp().Next_SetpId = item.SetpId;
                        }
                        else
                        {
                            lastItem.Next_SetpId = item.SetpId;
                        }
                        item.Pervious_SetpId = lastItem.SetpId;
                        item.Next_SetpId = "";//endSetp.SetpId;
                        endSetp.Next_SetpId = _WF_End.SetpId;
                        _WF_End.Pervious_SetpId = endSetp.SetpId;
                    }
                    else
                    {
                        if (lastItem is WF_ArraySetp)
                        {
                            var temS = lastItem as WF_ArraySetp;
                            temS.Get_EndSetp().Next_SetpId = item.SetpId;
                        }
                        else
                        {
                            lastItem.Next_SetpId = item.SetpId;
                        }
                        item.Pervious_SetpId = lastItem.SetpId;
                        item.Next_SetpId = _WF_End.SetpId;
                        _WF_End.Pervious_SetpId = item.SetpId;
                    }
                }
                _WF_Setps.Add(item);
            }
        }

        public override void AddRange(IEnumerable<WF_Setp> collection)
        {
            if (_WF_Setps != null && collection != null)
            {
                foreach (var item in collection)
                {
                    Add(item);
                }
            }
        }

        public override void Insert(WF_Setp sourceItem, WF_Setp item)
        {
            if (_WF_Setps != null && item != null && sourceItem != null)
            {
                var find = _WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (find != null)
                {
                    throw new Exception(string.Format("Id是{0}的节点已经存在于节点集合中", item.SetpId));
                }

                var findItem = _WF_Setps.Where(t => t.SetpId == sourceItem.SetpId).FirstOrDefault();
                int index = _WF_Setps.IndexOf(findItem);
                if (index < _WF_Setps.Count)
                {
                    var PSetpId = findItem.Pervious_SetpId;
                    var PSetp = _WF_Setps.Where(t => t.SetpId == PSetpId).FirstOrDefault();/////寻找下一个节点
                    if (PSetp != null)
                    {
                        item.Parent_Id = this.SetpId;

                        PSetp.Next_SetpId = item.SetpId;
                        item.Pervious_SetpId = PSetp.SetpId;

                        item.Next_SetpId = item.SetpId;
                        findItem.Pervious_SetpId = item.SetpId;

                        _WF_Setps.Insert(index, item);
                    }
                    else
                    {
                        throw new Exception(string.Format("在审批节点集合中没有发现要在其后插入的Id是{0}的节点的后节点信息", item.SetpId));
                    }
                }
            }
        }

        //public override void InsertRange(int index, IEnumerable<WF_Setp> collection)
        //{
        //    throw new NotImplementedException();
        //} 
        public override void Delete(WF_Setp item)
        {
            if (_WF_Setps != null && item != null)
            {
                var findItem = _WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (findItem == null)
                {
                    throw new Exception(string.Format("在审批节点集合中没有发现要删除的Id是{0}的节点信息", item.SetpId));
                }
                else//////寻找此节点的上一个节点和下一个节点进行处理
                {
                    var PSetpId = findItem.Pervious_SetpId;
                    var NSetpId = "";
                    if (item is WF_ArraySetp)
                    {
                        var temp = item as WF_ArraySetp;
                        NSetpId = temp.Get_EndSetp().Next_SetpId;
                    }
                    else
                    {
                        NSetpId = findItem.Next_SetpId;
                    }

                    List<WF_Setp> setps = new List<WF_Setp>();
                    //setps.Add(this._WF_Begin );
                    //setps.Add(this._WF_End );
                    //var PSetp = _WF_Setps.Union(setps).Where(t => t.SetpId == PSetpId).FirstOrDefault();  /////寻找上一个节点
                    //var NSetp = _WF_Setps.Union(setps).Where(t => t.SetpId == NSetpId).FirstOrDefault();/////寻找下一个节点
                    var PSetp = GetInstanceSetp(PSetpId);  /////寻找上一个节点
                    var NSetp = GetInstanceSetp(NSetpId);/////寻找下一个节点
                    if (PSetp != null && NSetp != null)
                    {
                        if (PSetp is WF_ArraySetp)
                        {
                            var tempArray = PSetp as WF_ArraySetp;
                            tempArray.Get_EndSetp().Next_SetpId = NSetp.SetpId;
                        }
                        else
                        {
                            PSetp.Next_SetpId = NSetp.SetpId;
                        }
                        NSetp.Pervious_SetpId = PSetp.SetpId;
                        _WF_Setps.Remove(findItem);  ////从集合中移除.
                    }
                    else
                    {
                        throw new Exception(string.Format("在审批节点集合中没有发现要删除的Id是{0}的前节点和后节点信息", item.SetpId));
                    }
                }
            }
        }
        #endregion

        public override bool Container(WF_Setp item)
        {
            return this.WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault() == null ? false : true;
        }

        public override WF_Setp Get_EndSetp()
        {
            return this._WF_End;
        }

        public WF_Setp GetInstanceSetp(string SetpId)
        {
            var setpList = this.WF_Setps.Union(new List<WF_Setp>() { this.WF_End, this.WF_Begin, this }).ToList();
            WF_Setp setp = null;
            LoopFlowFindSetp(setpList, SetpId, ref setp);
            return setp;
        }
        protected void LoopFlowFindSetp(List<WF_Setp> setps, string SetpId, ref WF_Setp findSetp)
        {
            foreach (var m in setps)
            {
                if (string.Equals(m.SetpId, SetpId, StringComparison.OrdinalIgnoreCase))
                {
                    findSetp = m;
                    return;
                }
                else
                {
                    if (m is WF_ArraySetp)
                    {
                        var tm = m as WF_ArraySetp;
                        var tm2 = tm.Get_EndSetp();
                        if (string.Equals(tm2.SetpId, SetpId, StringComparison.OrdinalIgnoreCase))
                        {
                            findSetp = tm2;
                            return;
                        }
                        LoopFlowFindSetp(((WF_ArraySetp)m).WF_Setps, SetpId, ref findSetp);
                    }
                }
            }
        }

        public StringBuilder Check()
        {
            StringBuilder sb = new StringBuilder();
            var setpList = this.WF_Setps.Union(new List<WF_Setp>() { this.WF_End, this.WF_Begin, this }).ToList();

            foreach (var m in setpList)
            {
                if (m is WF_ArraySetp)
                {
                    var arraySetp = m as WF_ArraySetp;
                    if ((arraySetp.Get_EndSetp() == null || string.IsNullOrEmpty(arraySetp.Get_EndSetp().Next_SetpId)) && !(m is WF_Flow))
                    {
                        sb.Append(string.Format("异常!编码是{0}名称是{1}的集合节点的结束节点为Null,或结束节点的下一节点为空", m.SetpId, m.SetpName));
                    }
                    LoopFlowSetp(arraySetp.WF_Setps, sb);
                }
                else if (m is WF_Setp)
                {
                    var setp = m as WF_Setp;
                    if (string.IsNullOrEmpty(setp.Next_SetpId) && !(m is WF_End))
                    {
                        sb.Append(string.Format("异常!编码是{0}名称是{1}的节点下一节点为空", m.SetpId, m.SetpName));
                    }
                }
                else
                {

                }
            }
            return sb;
        }
        protected void LoopFlowSetp(List<WF_Setp> setps, StringBuilder sb)
        {
            foreach (var m in setps)
            {
                if (m is WF_ArraySetp)
                {
                    var arraySetp = m as WF_ArraySetp;
                    if (arraySetp.Get_EndSetp() == null || string.IsNullOrEmpty(arraySetp.Get_EndSetp().Next_SetpId))
                    {
                        sb.Append(string.Format("异常!编码是{0}名称是{1}的集合节点的结束节点为Null,或结束节点的下一节点为空", m.SetpId, m.SetpName));
                    }
                    LoopFlowSetp(arraySetp.WF_Setps, sb);
                }
                else if (m is WF_Setp)
                {
                    var setp = m as WF_Setp;
                    if (string.IsNullOrEmpty(setp.Next_SetpId))
                    {
                        sb.Append(string.Format("异常!编码是{0}名称是{1}的节点下一节点为空", m.SetpId, m.SetpName));
                    }
                }
                else
                {

                }
            }
        }
    }
}
