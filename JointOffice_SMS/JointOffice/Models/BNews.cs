using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BNews : INews
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        string appkey;
        string appsecret;
        string ImConnection;
        private readonly IPrincipalBase _PrincipalBase;
        IMemoryCache _memoryCache;
        public BNews(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase, IMemoryCache memoryCache)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
            appkey = this.config.Value.ConnectionStrings.appkey;
            appsecret = this.config.Value.ConnectionStrings.appsecret;
            ImConnection = this.config.Value.ConnectionStrings.ImConnection;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="群名称,群组人员ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateGroup(CreateGroupPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            Message Message = new Message();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var id = "";
            var memberList = para.memberidlist;
            if (!memberList.Contains(memberid))
            {
                memberList.Add(memberid);
            }
            memberList.Sort();
            var qunzuid = "";
            var guid = Guid.NewGuid().ToString();
            if (memberList.Count() == 2)
            {

                foreach (var item in memberList)
                {
                    qunzuid = qunzuid + item;
                }
                var OneInfo = _JointOfficeContext.Member_Group.Where(t => t.Id == qunzuid).FirstOrDefault();
                if (OneInfo == null)
                {
                    var PeopleList = new List<People>();
                    foreach (var item in memberList)
                    {
                        var People = new People();
                        People.memberid = item;
                        PeopleList.Add(People);
                    }
                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
                    Member_Group Member_Group = new Member_Group();
                    Member_Group.Id = qunzuid;
                    Member_Group.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png";
                    Member_Group.MemberId = memberid;
                    Member_Group.GroupKingMemberId = memberid;
                    Member_Group.GroupPersonId = str;
                    Member_Group.Name = para.name;
                    Member_Group.CreateDate = DateTime.Now;
                    Member_Group.State = 1;
                    _JointOfficeContext.Member_Group.Add(Member_Group);
                    id = qunzuid.Replace(memberid, "");

                }
                else
                {
                    id = OneInfo.Id.Replace(memberid, "");

                }

            }
            else
            {
                string postUrl = "http://api.cn.ronghub.com/group/create.json";
                string postStr = "";
                postStr += "groupName=" + para.name;
                foreach (var item in para.memberidlist)
                {
                    postStr += "&userId=" + item;
                }
                postStr += "&groupId=" + guid;
                var postRes = WebApiHelper.PostAsynctMethod<RongYun>(postUrl, postStr, appkey, appsecret);
                if (postRes.code != "200")
                {
                    throw new BusinessTureException("创建失败.");
                }
                else
                {
                    var PeopleList = new List<People>();
                    foreach (var item in para.memberidlist)
                    {
                        var People = new People();
                        People.memberid = item;
                        PeopleList.Add(People);
                    }
                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
                    Member_Group Member_Group = new Member_Group();
                    Member_Group.Id = guid;
                    Member_Group.MemberId = memberid;
                    Member_Group.GroupKingMemberId = memberid;
                    Member_Group.GroupPersonId = str;
                    Member_Group.Name = para.name;
                    Member_Group.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/avatar_group_chat.png";
                    Member_Group.CreateDate = DateTime.Now;
                    Member_Group.State = 1;
                    _JointOfficeContext.Member_Group.Add(Member_Group);

                    var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();

                    News_Member News_Member = new News_Member();
                    News_Member.Id = Guid.NewGuid().ToString();
                    News_Member.MemberId = memberid;
                    News_Member.GroupId = guid;
                    News_Member.GroupPersonId = str;
                    News_Member.DeleteGroupPersonId = "";
                    News_Member.CreateDate = DateTime.Now;
                    List<WeiDuInfo> list = new List<WeiDuInfo>();
                    foreach (var item in PeopleList)
                    {
                        WeiDuInfo WeiDuInfo = new WeiDuInfo();
                        WeiDuInfo.memberId = item.memberid;
                        WeiDuInfo.count = 0;
                        list.Add(WeiDuInfo);
                    }
                    var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    News_Member.WeiDuGroupPersonId = weiduInfo;
                    _JointOfficeContext.News_Member.Add(News_Member);

                    News_News News_News = new News_News();
                    News_News.Id = Guid.NewGuid().ToString();
                    News_News.Body = info.Name + "创建群组";
                    News_News.NewsSenderId = memberid;
                    News_News.Type = 2;
                    News_News.InfoType = "7";
                    TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
                    News_News.Time = ts.TotalMilliseconds.ToString();
                    News_News.GroupId = guid;
                    News_News.NoSeePerson = "";
                    News_News.CreateTime = DateTime.Now;
                    _JointOfficeContext.News_News.Add(News_News);
                    var url2 = "http://api.cn.ronghub.com/message/group/publish.json";
                    var parastr = "";

                    //MessagePara MessagePara = new MessagePara();
                    //MessagePara.message = info.Name+"创建了群组";
                    //MessagePara.extra = "";
                    GroupMessagePara GroupMessagePara = new GroupMessagePara();
                    GroupMessagePara.operatorUserId = memberid;
                    GroupMessagePara.operation = "Create";
                    GroupMessagePara.message = "创建群组";
                    GroupMessagePara.extra = "ChuangJian";
                    dataPara data = new dataPara();
                    data.operatorNickname = info.Name;
                    data.targetGroupName = para.name;
                    GroupMessagePara.operation = "Create";
                    GroupMessagePara.data = data;
                    var message = Newtonsoft.Json.JsonConvert.SerializeObject(GroupMessagePara);
                    parastr = parastr + "fromUserId=" + memberid + "&toGroupId=" + guid + "&objectName=RC:GrpNtf&content=" + message + "&isIncludeSender=1";
                    var postRes2 = WebApiHelper.PostAsynctMethod<RongYun>(url2, parastr, appkey, appsecret);
                    if (postRes.code != "200")
                    {
                        throw new BusinessTureException("创建失败.");
                    }
                    id = guid;
                }
            }
            _JointOfficeContext.SaveChanges();
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_error = "创建成功";
            res.showapi_res_code = "200";
            ReturnMessage mes = new ReturnMessage();
            mes.Oprationflag = true;
            mes.Message = "创建成功";
            mes.memberid = id;
            if (memberList.Count() == 2)
            {
                mes.token = qunzuid;
            }
            else
            {
                mes.token = id;
            }

            res.showapi_res_body = mes;
            return res;
            //return Message.SuccessMeaasgeCode("创建成功", id);
        }
        /// <summary>
        /// 修改群组人员
        /// </summary>
        /// <param name="群组ID,群组类型,群组人员ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateGroup(UpdateGroupPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            //memberid = "aa9f0ccb-3a28-4ca5-9f0e-28ec37b23aaa";
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var Member_Group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.groupid).FirstOrDefault();
            if (Member_Group == null)
            {
                #region
                //if (para.type == 1)
                //{
                //    string postUrl = "http://api.cn.ronghub.com/group/join.json";
                //    string postStr = "";
                //    postStr += "groupName=" + Member_Group.Name;
                //    foreach (var item in para.memberidlist)
                //    {
                //        postStr += "&userId=" + item;
                //    }
                //    postStr += "&groupId=" + para.groupid;
                //    var postRes = WebApiHelper.PostAsynctMethod<RongYun>(postUrl, postStr, appkey, appsecret);
                //    if (postRes.code != "200")
                //    {
                //        throw new BusinessTureException("添加失败.");
                //    }
                //    var PeopleList = new List<People>();
                //    foreach (var item in para.memberidlist)
                //    {
                //        var People = new People();
                //        People.memberid = item;
                //        PeopleList.Add(People);
                //    }
                //    var str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
                //    Member_Group Member_Group1 = new Member_Group();
                //    Member_Group1.Id = Guid.NewGuid().ToString();
                //    Member_Group1.MemberId = memberid;
                //    Member_Group1.GroupKingMemberId = memberid;
                //    Member_Group1.GroupPersonId = str;
                //    Member_Group1.Name = "";
                //    Member_Group1.CreateDate = DateTime.Now;
                //    _JointOfficeContext.Member_Group.Add(Member_Group1);
                //}
                #endregion
                throw new BusinessTureException("不存在此群组.");
            }
            else
            {
                var NewsMember = _JointOfficeContext.News_Member.Where(t => t.GroupId == para.groupid).FirstOrDefault();

                var renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(Member_Group.GroupPersonId);
                //var News_Member = _JointOfficeContext.News_Member.Where(t => t.GroupId == para.groupid).FirstOrDefault();

                var sql = "";
                if (para.type == 1)
                {
                    foreach (var item in para.memberidlist)
                    {
                        if (!Member_Group.GroupPersonId.Contains(item))
                        {
                            sql += "insert into ofGroupUser (groupName,username,administrator) values('" + para.groupid + "','" + item + "','0');";
                        }
                    }
                }
                else
                {
                    foreach (var item in para.memberidlist)
                    {
                        sql += "delete ofGroupUser where groupName='" + para.groupid + "' and username = '" + item + "';";
                    }
                }
                using (SqlConnection conText = new SqlConnection(ImConnection))
                {
                    conText.Open();
                    SqlTransaction transaction;
                    using (SqlCommand cmd = conText.CreateCommand())
                    {
                        //启动事务
                        transaction = conText.BeginTransaction();
                        cmd.Connection = conText;
                        cmd.Transaction = transaction;
                        try
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            //完成提交
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            //数据回滚
                            transaction.Rollback();
                            throw new BusinessTureException(ex.Message);
                        }
                    }
                }
                if (para.type == 1)
                {
                    foreach (var item in para.memberidlist)
                    {
                        if (!Member_Group.GroupPersonId.Contains(item))
                        {
                            var People = new People();
                            People.memberid = item;
                            renyuanlist.Add(People);
                        }
                    }
                    if (NewsMember != null)
                    {

                        var personlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(NewsMember.WeiDuGroupPersonId);
                        List<WeiDuInfo> list1 = new List<WeiDuInfo>();
                        foreach (var one in para.memberidlist)
                        {
                            if (!NewsMember.WeiDuGroupPersonId.Contains(one))
                            {
                                WeiDuInfo WeiDuInfo = new WeiDuInfo();
                                WeiDuInfo.memberId = one;
                                WeiDuInfo.count = 0;
                                personlist.Add(WeiDuInfo);
                            }
                        }
                        var Newrenyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(NewsMember.GroupPersonId);

                        foreach (var item in para.memberidlist)
                        {
                            if (!NewsMember.GroupPersonId.Contains(item))
                            {
                                var People = new People();
                                People.memberid = item;
                                Newrenyuanlist.Add(People);
                            }
                        }
                        var newweiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(personlist);
                        NewsMember.WeiDuGroupPersonId = newweiduInfo;
                        NewsMember.GroupPersonId = Newtonsoft.Json.JsonConvert.SerializeObject(Newrenyuanlist);

                    }
                }
                else
                {
                    foreach (var item in para.memberidlist)
                    {
                        var one = renyuanlist.Where(t => t.memberid == item).FirstOrDefault();
                        if (one == null)
                        {
                            throw new BusinessTureException("该群组不存在该人员.");
                        }
                        renyuanlist.Remove(one);

                    }
                    if (NewsMember != null)
                    {
                        var personlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(NewsMember.WeiDuGroupPersonId);
                        foreach (var one in para.memberidlist)
                        {
                            var info = personlist.Where(t => t.memberId == one).FirstOrDefault();
                            if (info != null)
                            {
                                info.count = 0;
                            }
                        }
                        var newweiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(personlist);
                        NewsMember.WeiDuGroupPersonId = newweiduInfo;
                    }
                }
                Member_Group.GroupPersonId = Newtonsoft.Json.JsonConvert.SerializeObject(renyuanlist);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改群头像
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateGroupPicture(UpdateGroupPicturePara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.id).FirstOrDefault();
            group.Picture = para.picture;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasgeCode("修改成功", para.picture + SasKey);
        }
        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateGroupName(UpdateGroupNamePara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.id).FirstOrDefault();
            group.Name = para.name;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 存储会话信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge SaveConversation(News_News News_News)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            News_News.Id = Guid.NewGuid().ToString();
            News_News.NewsSenderId = memberid;
            News_News.NoSeePerson = "";
            News_News.CreateTime = DateTime.Now;

            if (!string.IsNullOrEmpty(News_News.GroupId))
            {
                var memoften = _JointOfficeContext.Member_Often.Where(t => t.MemberId == memberid && t.OtherMemberId == News_News.GroupId).FirstOrDefault();
                if (memoften == null)
                {
                    Member_Often Member_Often = new Member_Often();
                    Member_Often.Id = Guid.NewGuid().ToString();
                    Member_Often.MemberId = memberid;
                    Member_Often.OtherMemberId = News_News.GroupId;
                    Member_Often.CreateDate = DateTime.Now;
                    Member_Often.WriteDate = DateTime.Now;
                    _JointOfficeContext.Member_Often.Add(Member_Often);
                }
                else
                {
                    memoften.WriteDate = DateTime.Now;
                }
            }

            var othermemberid = News_News.GroupId;
            if (News_News.Type == 1)
            {
                List<string> list = new List<string>();
                list.Add(News_News.GroupId);
                list.Add(memberid);
                list.Sort();
                var qunzuid = "";
                foreach (var item in list)
                {
                    qunzuid = qunzuid + item;
                }
                News_News.GroupId = qunzuid;
            }

            var str = "";
            var info = _JointOfficeContext.Member_Group.Where(t => t.Id == News_News.GroupId).FirstOrDefault();
            if (info == null)
            {
                List<string> list1 = new List<string>();
                list1.Add(othermemberid);
                list1.Add(memberid);
                var PeopleList = new List<People>();
                foreach (var item in list1)
                {
                    var People = new People();
                    People.memberid = item;
                    PeopleList.Add(People);
                }

                str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
                Member_Group Member_Group = new Member_Group();
                if (News_News.Type == 1)
                {
                    Member_Group.Id = News_News.GroupId;
                    Member_Group.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png";
                }
                else
                {
                    Member_Group.Id = othermemberid;
                    Member_Group.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/avatar_group_chat.png";
                }

                Member_Group.MemberId = memberid;
                Member_Group.GroupKingMemberId = memberid;
                Member_Group.GroupPersonId = str;
                Member_Group.Name = "";
                Member_Group.State = 1;
                Member_Group.CreateDate = DateTime.Now;
                _JointOfficeContext.Member_Group.Add(Member_Group);
                News_News.SeePerson = Member_Group.GroupPersonId;
                info = Member_Group;
            }
            else
            {
                News_News.SeePerson = info.GroupPersonId;
            }
            _JointOfficeContext.News_News.Add(News_News);
            var NewsMember = _JointOfficeContext.News_Member.Where(t => t.GroupId == News_News.GroupId).FirstOrDefault();

            if (NewsMember == null)
            {
                List<string> list3 = new List<string>();
                list3.Add(othermemberid);
                list3.Add(memberid);
                var PeopleList = new List<People>();
                foreach (var item in list3)
                {
                    var People = new People();
                    People.memberid = item;
                    PeopleList.Add(People);
                }

                str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
                News_Member News_Member = new News_Member();
                News_Member.Id = Guid.NewGuid().ToString();
                News_Member.MemberId = memberid;
                News_Member.GroupId = News_News.GroupId;
                News_Member.DeleteGroupPersonId = "";
                News_Member.GroupPersonId = str;
                News_Member.CreateDate = DateTime.Now;
                List<WeiDuInfo> list1 = new List<WeiDuInfo>();

                WeiDuInfo WeiDuInfo = new WeiDuInfo();
                WeiDuInfo.memberId = memberid;
                WeiDuInfo.count = 0;
                list1.Add(WeiDuInfo);

                WeiDuInfo WeiDuInfo1 = new WeiDuInfo();
                WeiDuInfo1.memberId = othermemberid;
                WeiDuInfo1.count = 1;
                list1.Add(WeiDuInfo1);

                var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                News_Member.WeiDuGroupPersonId = weiduInfo;
                _JointOfficeContext.News_Member.Add(News_Member);
            }
            else
            {
                NewsMember.DeleteGroupPersonId = "";
                var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(NewsMember.WeiDuGroupPersonId);
                var personlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(info.GroupPersonId);
                foreach (var item in personlist)
                {
                    if (item.memberid!=memberid)
                    {
                        var one = objectList.Where(t => t.memberId == item.memberid).FirstOrDefault();
                        if (one != null)
                        {
                            one.count = one.count + 1;
                        }
                    }                  
                }
                //foreach (var item in objectList)
                //{
                //    if (item.memberId != memberid)
                //    {
                //        item.count += 1;
                //    }
                //}
                var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(objectList);
                NewsMember.WeiDuGroupPersonId = weiduInfo;
                //NewsMember.GroupPersonId = info.GroupPersonId;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasgeCode("存储成功", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        }
        /// <summary>
        /// 删除会话消息
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteConversation(DeleteConversationInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var newsInfo = _JointOfficeContext.News_News.Where(t => t.Id == para.id).FirstOrDefault();
            var result = "";
            if (newsInfo.NoSeePerson != "" && newsInfo.NoSeePerson != null)
            {
                var list = JsonConvert.DeserializeObject<List<string>>(newsInfo.NoSeePerson);
                list.Add(memberid);
                result = JsonConvert.SerializeObject(list);
            }
            else
            {
                List<string> list = new List<string>();
                list.Add(memberid);
                result = JsonConvert.SerializeObject(list);
            }
            newsInfo.NoSeePerson = result;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 清空会话消息
        /// </summary>
        /// <param name="会话ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge CleanConversation(CleanConversationInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var newsInfo = _JointOfficeContext.News_News.Where(t => t.GroupId == para.id).ToList();
            foreach (var item in newsInfo)
            {
                var result = "";

                if (item.NoSeePerson != "" && item.NoSeePerson != null)
                {
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(item.NoSeePerson);
                    list.Add(memberid);
                    result = JsonConvert.SerializeObject(list);
                }
                else
                {
                    List<string> list = new List<string>();
                    list.Add(memberid);
                    result = JsonConvert.SerializeObject(list);
                }
                item.NoSeePerson = result;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("清除成功");
        }
        /// <summary>
        /// 显示   消息列表
        /// </summary>
        public Showapi_Res_List<ConversationList> GetConversationList(GetConversationList para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<ConversationList>();
                return Return.Return();
            }
            Showapi_Res_List<ConversationList> res = new Showapi_Res_List<ConversationList>();
            List<ConversationList> list = new List<ConversationList>();
            var newsInfo = _JointOfficeContext.News_News.Where(t => t.GroupId == para.id).OrderBy(t => t.CreateTime).ToList();
            foreach (var item in newsInfo)
            {
                ConversationList ConversationList = new ConversationList();
                ConversationList.id = item.Id;
                ConversationList.date = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                if (item.InfoType == "1")
                {
                    ConversationList.body = item.Body;
                }
                else
                {
                    ConversationList.body = item.Body + SasKey;
                }
                list.Add(ConversationList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ConversationList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 发群通知
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge SendGroupNotice(News_GroupNotice para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            _JointOfficeContext.News_GroupNotice.Add(para);

            var list = JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Range);
            List<string> memberInfo = new List<string>();
            foreach (var item1 in list)
            {
                if (item1.type == "1")
                {
                    var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                    memberInfo.AddRange(member_Info);
                }
                if (item1.type == "2")
                {
                    WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                    memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                }
                if (item1.type == "3")
                {
                    var member_Info1 = _JointOfficeContext.Member_Group.Where(t => t.Id == item1.id).FirstOrDefault();
                    var list2 = JsonConvert.DeserializeObject<List<PeoPleInfo2>>(member_Info1.GroupPersonId);
                    List<string> member_Info = new List<string>();
                    foreach (var item in list2)
                    {
                        var exeMember = item.memberid;
                        member_Info.Add(exeMember);
                    }
                    memberInfo.AddRange(member_Info);
                }
            }
            var memberInfoset = new HashSet<string>(memberInfo);

            foreach (var item1 in memberInfoset)
            {
                News_GroupNotice_Content News_GroupNotice_Content = new News_GroupNotice_Content();
                News_GroupNotice_Content.Id = Guid.NewGuid().ToString();
                News_GroupNotice_Content.MemberId = item1;
                News_GroupNotice_Content.GroupNoticeId = para.Id;
                News_GroupNotice_Content.IsConfirm = para.IsConfirm;
                News_GroupNotice_Content.IfYesIsConfirm = "";
                News_GroupNotice_Content.ConfirmTime = "";
                _JointOfficeContext.News_GroupNotice_Content.Add(News_GroupNotice_Content);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("发送成功");
        }
        /// <summary>
        /// 删除群通知
        /// </summary>
        public Showapi_Res_Meaasge DeleteGroupNotice(DeleteGroupNoticeInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            News_GroupNotice News_GroupNotice = new News_GroupNotice();
            News_GroupNotice_Content News_GroupNotice_Content = new News_GroupNotice_Content();
            var groupNotice = _JointOfficeContext.News_GroupNotice.Where(t => t.MemberId == memberid && t.Id == para.id).FirstOrDefault();
            _JointOfficeContext.News_GroupNotice.Remove(groupNotice);

            var groupNoticeContent = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == para.id).ToList();
            foreach (var item in groupNoticeContent)
            {
                _JointOfficeContext.News_GroupNotice_Content.Remove(item);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 确认读取群通知
        /// </summary>
        /// <param name="群通知ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge ConfirmReadGroupNotice(ConfirmReadGroupNoticeInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            News_GroupNotice_Content News_GroupNews_Content = new News_GroupNotice_Content();
            var groupNotice = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == para.id && t.MemberId == memberid).FirstOrDefault();
            if (groupNotice.IsConfirm == "1")
            {
                if (para.mark == "1")
                {
                    News_GroupNews_Content.IfYesIsConfirm = para.mark;
                    News_GroupNews_Content.ConfirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                if (para.mark == "2")
                {
                    News_GroupNews_Content.IfYesIsConfirm = para.mark;
                    News_GroupNews_Content.ConfirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
            }
            else
            {
                News_GroupNews_Content.IfYesIsConfirm = para.mark;
                News_GroupNews_Content.ConfirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 群通知列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GroupNoticeList> GetGroupNoticeList(GroupNoticeListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GroupNoticeList>();
                return Return.Return();
            }
            Showapi_Res_List<GroupNoticeList> res = new Showapi_Res_List<GroupNoticeList>();
            List<GroupNoticeList> list = new List<GroupNoticeList>();
            if (para.type == "1")
            {
                var groupNoticeList = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.MemberId == memberid).OrderByDescending(t => t.ConfirmTime).Skip(para.count * para.page).Take(para.count).ToList();
                if (groupNoticeList != null)
                {
                    foreach (var item in groupNoticeList)
                    {
                        var groupNotice = _JointOfficeContext.News_GroupNotice.Where(t => t.Id == item.GroupNoticeId).FirstOrDefault();
                        var groupNoticeMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == groupNotice.MemberId).FirstOrDefault();
                        GroupNoticeList GroupNoticeList = new GroupNoticeList();
                        GroupNoticeList.memberid = groupNotice.MemberId;
                        GroupNoticeList.name = groupNoticeMember.Name;
                        GroupNoticeList.picture = groupNoticeMember.Picture + SasKey;
                        GroupNoticeList.id = groupNotice.Id;
                        GroupNoticeList.title = groupNotice.Title;
                        GroupNoticeList.content = groupNotice.Body;
                        GroupNoticeList.createTime = groupNotice.CreateDate.ToString("yyyy-MM-dd HH:mm");
                        if (item.IsConfirm == "1")
                        {
                            var isConfirmList = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == groupNotice.Id).ToList();
                            var ifYesIsConfirm = isConfirmList.Where(t => t.IfYesIsConfirm == "2").ToList();
                            GroupNoticeList.confirmed = "已确认(" + ifYesIsConfirm.Count().ToString() + "/" + isConfirmList.Count().ToString() + ")";
                        }
                        else
                        {
                            var isConfirmMemberid = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == groupNotice.Id && t.MemberId == memberid).FirstOrDefault();
                            if (isConfirmMemberid.IfYesIsConfirm == "1")
                            {
                                GroupNoticeList.confirmed = "已读";
                            }
                            else
                            {
                                GroupNoticeList.confirmed = "";
                            }
                        }
                        list.Add(GroupNoticeList);
                    }
                }
            }
            if (para.type == "2")
            {
                var groupNoticeList = _JointOfficeContext.News_GroupNotice.Where(t => t.MemberId == memberid).OrderByDescending(t => t.CreateDate).Skip(para.count * para.page).Take(para.count).ToList();
                if (groupNoticeList != null)
                {
                    var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                    foreach (var item in groupNoticeList)
                    {
                        GroupNoticeList GroupNoticeList = new GroupNoticeList();
                        GroupNoticeList.memberid = memberid;
                        GroupNoticeList.name = memberInfo.Name;
                        GroupNoticeList.picture = memberInfo.Picture + SasKey;
                        GroupNoticeList.id = item.Id;
                        GroupNoticeList.title = item.Title;
                        GroupNoticeList.content = item.Body;
                        GroupNoticeList.createTime = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
                        if (item.IsConfirm == "1")
                        {
                            var isConfirmList = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == item.Id).ToList();
                            var ifYesIsConfirm = isConfirmList.Where(t => t.IfYesIsConfirm == "2").ToList();
                            GroupNoticeList.confirmed = "已确认(" + ifYesIsConfirm.Count().ToString() + "/" + isConfirmList.Count().ToString() + ")";
                        }
                        else
                        {
                            var isConfirmMemberid = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == item.Id && t.MemberId == memberid).FirstOrDefault();
                            if (isConfirmMemberid.IfYesIsConfirm == "1")
                            {
                                GroupNoticeList.confirmed = "已读";
                            }
                            else
                            {
                                GroupNoticeList.confirmed = "";
                            }
                        }
                        list.Add(GroupNoticeList);
                    }
                }
            }
            if (para.type == "3")
            {
                var groupNoticeList = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.MemberId == memberid && t.IfYesIsConfirm == "").OrderByDescending(t => t.ConfirmTime).Skip(para.count * para.page).Take(para.count).ToList();
                if (groupNoticeList != null)
                {
                    foreach (var item in groupNoticeList)
                    {
                        var groupNotice = _JointOfficeContext.News_GroupNotice.Where(t => t.Id == item.GroupNoticeId).FirstOrDefault();
                        var groupNoticeMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == groupNotice.MemberId).FirstOrDefault();
                        GroupNoticeList GroupNoticeList = new GroupNoticeList();
                        GroupNoticeList.memberid = groupNotice.MemberId;
                        GroupNoticeList.name = groupNoticeMember.Name;
                        GroupNoticeList.picture = groupNoticeMember.Picture + SasKey;
                        GroupNoticeList.id = groupNotice.Id;
                        GroupNoticeList.title = groupNotice.Title;
                        GroupNoticeList.content = groupNotice.Body;
                        GroupNoticeList.createTime = groupNotice.CreateDate.ToString("yyyy-MM-dd HH:mm");
                        GroupNoticeList.confirmed = "未读";
                        list.Add(GroupNoticeList);
                    }
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GroupNoticeList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 群通知详情
        /// </summary>
        public Showapi_Res_Single<GroupNoticeBody> GetGroupNoticeBodyList(GroupNoticeBodyInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<GroupNoticeBody>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<GroupNoticeBody> res = new Showapi_Res_Single<GroupNoticeBody>();
            GroupNoticeBody GroupNoticeBody = new GroupNoticeBody();
            var groupNotice = _JointOfficeContext.News_GroupNotice.Where(t => t.Id == para.id).FirstOrDefault();
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == groupNotice.MemberId).FirstOrDefault();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.id).FirstOrDefault();
            var agree = _JointOfficeContext.Agree.Where(t => t.MemberId == memberid && t.UId == para.id).FirstOrDefault();
            GroupNoticeBody.memberid = memberInfo.MemberId;
            GroupNoticeBody.name = memberInfo.Name;
            GroupNoticeBody.picture = memberInfo.Picture + SasKey;
            GroupNoticeBody.createTime = groupNotice.CreateDate.ToString("yyyy-MM-dd HH:mm");
            GroupNoticeBody.phoneModel = groupNotice.PhoneModel;
            GroupNoticeBody.isConfirm = groupNotice.IsConfirm;
            if (groupNotice.IsConfirm == "1")
            {
                var groupNoticeContent = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == para.id).ToList();
                var groupNoticeContent1 = groupNoticeContent.Where(t => t.IfYesIsConfirm == "2").ToList();
                GroupNoticeBody.confirmed = "已确认(" + groupNoticeContent1.Count().ToString() + "/" + groupNoticeContent.Count().ToString() + ")";
            }
            else
            {
                var groupNoticeContentMemberid = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == para.id && t.MemberId == memberid).FirstOrDefault();
                if (groupNoticeContentMemberid.IfYesIsConfirm == "1")
                {
                    GroupNoticeBody.confirmed = "已读";
                }
                else
                {
                    GroupNoticeBody.confirmed = "";
                }
            }
            GroupNoticeBody.body = groupNotice.Body;
            GroupNoticeBody.title = groupNotice.Title;
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    GroupNoticeBody.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    GroupNoticeBody.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    GroupNoticeBody.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    GroupNoticeBody.dianZanNum = 0;
                }
            }
            else
            {
                GroupNoticeBody.pingLunNum = 0;
                GroupNoticeBody.dianZanNum = 0;
            }
            if (agree == null)
            {
                GroupNoticeBody.isZan = 0;
            }
            else
            {
                GroupNoticeBody.isZan = 1;
            }
            if (groupNotice.Range != null && groupNotice.Range != "" && groupNotice.Range != "[]")
            {
                var list01 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(groupNotice.Range);
                var list11 = list01.Where(t => t.type == "1").ToList();
                var list12 = list01.Where(t => t.type == "2").ToList();
                var list13 = list01.Where(t => t.type == "3").ToList();
                var range1 = "";
                var range2 = "";
                var range3 = "";
                if (list11.Count != 0)
                {
                    range1 = list11.Count() + "个同事";
                }
                if ((list11.Count != 0 && list12.Count != 0) || (list11.Count != 0 && list13.Count != 0 && list12.Count == 0))
                {
                    range1 = range1 + "、";
                }
                if (list12.Count != 0)
                {
                    range2 = list12.Count() + "个部门";
                }
                if (list12.Count != 0 && list13.Count != 0)
                {
                    range2 = range2 + "、";
                }
                if (list13.Count != 0)
                {
                    range3 = list13.Count() + "个群组";
                }
                GroupNoticeBody.range = range1 + range2 + range3;
            }
            else
            {
                GroupNoticeBody.range = "私密";
            }
            if (groupNotice.Picture != null && groupNotice.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(groupNotice.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                GroupNoticeBody.appendPicture = listPicture;
            }
            if (groupNotice.Annex != null && groupNotice.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(groupNotice.Annex);
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                }
                GroupNoticeBody.annex = listAnnex;
            }
            if (groupNotice.Voice != null && groupNotice.Voice != "")
            {
                GroupNoticeBody.voice = groupNotice.Voice + SasKey;
            }
            GroupNoticeBody.voiceLength = groupNotice.VoiceLength;

            res.showapi_res_code = "200";
            res.showapi_res_body = GroupNoticeBody;
            return res;
        }
        /// <summary>
        /// 群通知读取/确认人数
        /// </summary>
        public Showapi_Res_List<GroupNoticeConfirmNum> GetGroupNoticeConfirmNumList(GroupNoticeBodyInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GroupNoticeConfirmNum>();
                return Return.Return();
            }
            Showapi_Res_List<GroupNoticeConfirmNum> res = new Showapi_Res_List<GroupNoticeConfirmNum>();
            List<GroupNoticeConfirmNum> list = new List<GroupNoticeConfirmNum>();
            var groupNoticeContent = _JointOfficeContext.News_GroupNotice_Content.Where(t => t.GroupNoticeId == para.id).ToList();
            foreach (var item in groupNoticeContent)
            {
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
                GroupNoticeConfirmNum GroupNoticeConfirmNum = new GroupNoticeConfirmNum();
                GroupNoticeConfirmNum.id = memberInfo.Id;
                GroupNoticeConfirmNum.name = memberInfo.Name;
                GroupNoticeConfirmNum.picture = memberInfo.Picture + SasKey;
                GroupNoticeConfirmNum.confirmTime = item.ConfirmTime;
                switch (item.IfYesIsConfirm)
                {
                    case "":
                        GroupNoticeConfirmNum.confirmMark = "未读";
                        break;
                    case "1":
                        GroupNoticeConfirmNum.confirmMark = "已读";
                        break;
                    case "2":
                        GroupNoticeConfirmNum.confirmMark = "已确认";
                        break;
                }
                list.Add(GroupNoticeConfirmNum);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GroupNoticeConfirmNum>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 收藏
        /// </summary>
        public Showapi_Res_Meaasge Collection(CollectionInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            News_Collection News_Collection = new News_Collection();
            News_Collection.Id = Guid.NewGuid().ToString();
            News_Collection.MemberId = memberid;
            News_Collection.UId = para.id;
            News_Collection.OtherMemberID = para.otherMemberid;
            News_Collection.Type = para.type;
            switch (para.type)
            {
                case "1":
                    var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workApproval.Body;
                    break;
                case "2":
                    var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workLog.WorkSummary;
                    break;
                case "3":
                    var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workTask.TaskTitle;
                    break;
                case "4":
                    var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workProgram.Body;
                    break;
                case "5":
                    var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workOrder.Body;
                    break;
                case "8":
                    var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workAnnouncement.Title;
                    break;
                case "9":
                    var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == para.id).FirstOrDefault();
                    News_Collection.Body = workShare.Body;
                    break;
            }
            News_Collection.CreateDate = DateTime.Now;
            News_Collection.MarkInfo = "";

            //foreach (var item in para.markInfo)
            //{
            //    if (item.markId == null || item.markId == "")
            //    {
            //        News_Mark News_Mark = new News_Mark();
            //        News_Mark.Id = Guid.NewGuid().ToString();
            //        News_Mark.MemberId = memberid;
            //        News_Mark.Name = item.markName;
            //        List<MarkUId> markUIdList = new List<MarkUId>();
            //        MarkUId MarkUId = new MarkUId();
            //        MarkUId.id = para.id;
            //        MarkUId.type = para.type;
            //        markUIdList.Add(MarkUId);
            //        var str = JsonConvert.SerializeObject(markUIdList);
            //        News_Mark.MarkUId = str;
            //        _JointOfficeContext.News_Mark.Add(News_Mark);
            //        item.markId = News_Mark.Id;
            //    }
            //    else
            //    {
            //        var markInfo = _JointOfficeContext.News_Mark.Where(t => t.Id == item.markId).FirstOrDefault();
            //        var markUIdList = JsonConvert.DeserializeObject<List<MarkUId>>(markInfo.MarkUId);
            //        MarkUId MarkUId = new MarkUId();
            //        MarkUId.id = para.id;
            //        MarkUId.type = para.type;
            //        markUIdList.Add(MarkUId);
            //        var str = JsonConvert.SerializeObject(markUIdList);
            //        markInfo.MarkUId = str;
            //    }
            //}
            //News_Collection.MarkInfo = JsonConvert.SerializeObject(para.markInfo);

            _JointOfficeContext.News_Collection.Add(News_Collection);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("收藏成功");
        }
        /// <summary>
        /// 修改收藏的标签
        /// </summary>
        public Showapi_Res_Meaasge UpdateCollection(UpdateCollectionInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var Info = _JointOfficeContext.News_Collection.Where(t => t.UId == para.id && t.MemberId == memberid).FirstOrDefault();
            var markInfoList = JsonConvert.DeserializeObject<List<MarkInfoPara>>(Info.MarkInfo);
            if (para.type == "1")
            {
                foreach (var item in para.addMarkInfo)
                {
                    if (item.markId == null || item.markId == "")
                    {
                        News_Mark News_Mark = new News_Mark();
                        News_Mark.Id = Guid.NewGuid().ToString();
                        News_Mark.MemberId = memberid;
                        News_Mark.Name = item.markName;
                        List<MarkUId> markUIdList = new List<MarkUId>();
                        MarkUId MarkUId = new MarkUId();
                        MarkUId.id = para.id;
                        MarkUId.type = para.type;
                        markUIdList.Add(MarkUId);
                        var str = JsonConvert.SerializeObject(markUIdList);
                        News_Mark.MarkUId = str;
                        _JointOfficeContext.News_Mark.Add(News_Mark);
                        item.markId = News_Mark.Id;
                    }
                    markInfoList.Add(item);
                }
            }
            if (para.type == "2")
            {
                foreach (var item in para.deleteMarkInfo)
                {
                    var xinxi = markInfoList.Where(t => t.markId == item.markId).FirstOrDefault();
                    if (xinxi != null)
                    {
                        markInfoList.Remove(xinxi);
                    }
                    var markInfo = _JointOfficeContext.News_Mark.Where(t => t.Id == item.markId).FirstOrDefault();
                    if (markInfo != null)
                    {
                        var markUIdList = JsonConvert.DeserializeObject<List<MarkUId>>(markInfo.MarkUId);
                        if (markUIdList != null && markUIdList.Count != 0)
                        {
                            var markUId = markUIdList.Where(t => t.id == para.id).FirstOrDefault();
                            if (markUId != null)
                            {
                                markUIdList.Remove(markUId);
                                var str = JsonConvert.SerializeObject(markUIdList);
                                markInfo.MarkUId = str;
                            }
                        }
                    }
                }
            }
            var markInfoListStr = JsonConvert.SerializeObject(markInfoList);
            Info.MarkInfo = markInfoListStr;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 收藏列表
        /// </summary>
        public Showapi_Res_List<CollectionList> GetCollectionList()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CollectionList>();
                return Return.Return();
            }
            Showapi_Res_List<CollectionList> res = new Showapi_Res_List<CollectionList>();
            List<CollectionList> list = new List<CollectionList>();
            var collection = _JointOfficeContext.News_Collection.Where(t => t.MemberId == memberid).ToList();
            foreach (var item in collection)
            {
                CollectionList CollectionList = new CollectionList();
                CollectionList.type = item.Type;
                CollectionList.id = item.UId;
                CollectionList.body = item.Body;
                CollectionList.otherMemberId = item.OtherMemberID;
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.OtherMemberID).FirstOrDefault();
                CollectionList.picture = memberInfo.Picture + SasKey;
                CollectionList.name = memberInfo.Name;
                CollectionList.time = item.CreateDate.ToString("yyyy-MM-dd HH:mm");

                var markInfoList = JsonConvert.DeserializeObject<List<MarkInfoPara>>(item.MarkInfo);
                if (markInfoList != null && markInfoList.Count != 0)
                {
                    List<MarkInfoPara> MarkInfoList = new List<MarkInfoPara>();
                    foreach (var one in markInfoList)
                    {
                        MarkInfoPara MarkInfoPara = new MarkInfoPara();
                        MarkInfoPara.markId = one.markId;
                        MarkInfoPara.markName = one.markName;
                        MarkInfoList.Add(MarkInfoPara);
                    }
                    CollectionList.markInfo = MarkInfoList;
                }
                list.Add(CollectionList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CollectionList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 我的收藏 Web
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetMyCollectionList(GetMyFocusInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            var sql = @"exec WoDeShouCang '" + memberid + "'," + begin + "," + end + ",'" + para.companyId + "'";
            var sql1 = @"exec WoDeShouCangCount '" + memberid + "','" + para.companyId + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            //大类  list  处理
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge NoCollection(NoCollectionInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var collectionOne = _JointOfficeContext.News_Collection.Where(t => t.UId == para.id && t.MemberId == memberid).FirstOrDefault();
            if (collectionOne != null)
            {
                var markInfoList = JsonConvert.DeserializeObject<List<MarkInfoPara>>(collectionOne.MarkInfo);
                if (markInfoList != null && markInfoList.Count != 0)
                {
                    foreach (var item in markInfoList)
                    {
                        var markInfo = _JointOfficeContext.News_Mark.Where(t => t.Id == item.markId).FirstOrDefault();
                        if (markInfo != null)
                        {
                            var markUIdList = JsonConvert.DeserializeObject<List<MarkUId>>(markInfo.MarkUId);
                            if (markUIdList != null && markUIdList.Count != 0)
                            {
                                var markUIdOne = markUIdList.Where(t => t.id == para.id).FirstOrDefault();
                                if (markUIdOne != null)
                                {
                                    markUIdList.Remove(markUIdOne);
                                    markInfo.MarkUId = JsonConvert.SerializeObject(markUIdList);
                                }
                            }
                        }
                    }
                }
                _JointOfficeContext.News_Collection.Remove(collectionOne);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("取消收藏成功");
        }
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge InsertMark(InsertMarkInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            News_Mark News_Mark = new News_Mark();
            News_Mark.Id = Guid.NewGuid().ToString();
            News_Mark.MemberId = memberid;
            News_Mark.Name = para.name;
            _JointOfficeContext.News_Mark.Add(News_Mark);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("添加成功");
        }
        /// <summary>
        /// 修改标签名称
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateMark(UpdateMarkInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var markInfo = _JointOfficeContext.News_Mark.Where(t => t.Id == para.id).FirstOrDefault();
            if (markInfo != null)
            {
                markInfo.Name = para.name;
            }

            var markUIdList = JsonConvert.DeserializeObject<List<MarkUId>>(markInfo.MarkUId);
            if (markUIdList != null && markUIdList.Count != 0)
            {
                foreach (var item in markUIdList)
                {
                    var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.id && t.Type == item.type).FirstOrDefault();
                    if (collection != null)
                    {
                        var markInfoList = JsonConvert.DeserializeObject<List<MarkInfoPara>>(collection.MarkInfo);
                        if (markInfoList != null && markInfoList.Count != 0)
                        {
                            var markInfoOne = markInfoList.Where(t => t.markId == para.id).FirstOrDefault();
                            if (markInfoOne != null)
                            {
                                markInfoOne.markName = para.name;
                                var str = JsonConvert.SerializeObject(markInfoList);
                                collection.MarkInfo = str;
                            }
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteMark(DeleteMarkInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var markOne = _JointOfficeContext.News_Mark.Where(t => t.Id == para.id).FirstOrDefault();
            if (markOne != null)
            {
                var collectionMark = _JointOfficeContext.News_Collection.Where(t => t.MarkInfo.Contains(para.id)).ToList();
                if (collectionMark != null && collectionMark.Count != 0)
                {
                    foreach (var item in collectionMark)
                    {
                        var markInfoList = JsonConvert.DeserializeObject<List<MarkInfoPara>>(item.MarkInfo);
                        if (markInfoList != null && markInfoList.Count != 0)
                        {
                            var markInfoOne = markInfoList.Where(t => t.markId == para.id).FirstOrDefault();
                            if (markInfoOne != null)
                            {
                                markInfoList.Remove(markInfoOne);
                                var str = JsonConvert.SerializeObject(markInfoList);
                                item.MarkInfo = str;
                            }
                        }
                    }
                }
                _JointOfficeContext.News_Mark.Remove(markOne);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 标签列表
        /// </summary>
        public Showapi_Res_List<MarkList> GetMarkList()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<MarkList>();
                return Return.Return();
            }
            Showapi_Res_List<MarkList> res = new Showapi_Res_List<MarkList>();
            List<MarkList> list = new List<MarkList>();
            var markList = _JointOfficeContext.News_Mark.Where(t => t.MemberId == memberid).ToList();
            foreach (var item in markList)
            {
                MarkList MarkList = new MarkList();
                MarkList.id = item.Id;
                MarkList.name = item.Name;
                list.Add(MarkList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<MarkList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="类型,ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge Focus(FocusInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            News_Focus News_Focus = new News_Focus();
            News_Focus.Id = Guid.NewGuid().ToString();
            News_Focus.MemberId = memberid;
            News_Focus.UId = para.id;
            News_Focus.Type = para.type;
            News_Focus.CreateDate = DateTime.Now;
            _JointOfficeContext.News_Focus.Add(News_Focus);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("关注成功");
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge NoFocus(NoFocusInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var focusOne = _JointOfficeContext.News_Focus.Where(t => t.UId == para.id).FirstOrDefault();
            if (focusOne != null)
            {
                _JointOfficeContext.News_Focus.Remove(focusOne);
                _JointOfficeContext.SaveChanges();
            }
            return Message.SuccessMeaasge("取消关注成功");
        }
        /// <summary>
        /// 我关注的
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetMyFocusList(GetMyFocusInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            var sql = "";
            var sql1 = "";
            if (string.IsNullOrEmpty(para.companyId))
            {
                sql = @"exec WoGuanZhuDe '" + memberid + "'," + begin + "," + end;
                sql1 = @"exec WoGuanZhuDeCount '" + memberid + "'";
            }
            else
            {
                sql = @"exec WoGuanZhuDeCompany '" + memberid + "','" + para.companyId + "'," + begin + "," + end;
                sql1 = @"exec WoGuanZhuDeCompanyCount '" + memberid + "','" + para.companyId + "'";
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            //大类  list  处理
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// @我的回复
        /// </summary>
        public Showapi_Res_List<WorkReply> GetMyReplyList(GetATMyInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkReply>();
                return Return.Return();
            }
            Showapi_Res_List<WorkReply> res = new Showapi_Res_List<WorkReply>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<WorkReply> list = new List<WorkReply>();
            List<string> idList = new List<string>();
            var allPage = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (para.memberid == null)
            {
                para.memberid = "";
            }
            var sql = @"exec WoDeHuiFu '" + para.memberid + "','" + memberid + "'," + begin + "," + end;
            var sql1 = @"exec WoDeHuiFuCount '" + para.memberid + "','" + memberid + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                idList = conText.Query<string>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                var allPage1 = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPage1 += 1;
                }
                allPage = allPage1;
            }
            foreach (var item in idList)
            {
                List<WorkReply> list11 = new List<WorkReply>();
                list11 = WorkDetails.GetWorkReply(item, memberid);
                list.AddRange(list11);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkReply>();
            res.showapi_res_body.allPages = allPage;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 工作回复  Web
        /// </summary>
        public Showapi_Res_List<WorkReply> GetWorkReplyWebList(AllSearch para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkReply>();
                return Return.Return();
            }
            Showapi_Res_List<WorkReply> res = new Showapi_Res_List<WorkReply>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<WorkReply> list = new List<WorkReply>();
            List<string> idList = new List<string>();
            var allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            //处理日期入参 规范年月日格式
            if (para.beginTime == "" || para.beginTime == null)
            {
                para.beginTime = "1901-01-01";
            }
            if (para.stopTime == "" || para.stopTime == null)
            {
                para.stopTime = "2200-12-31";
            }
            para.beginTime = WorkDetails.GetYMD(para.beginTime) + " 00:00:00";
            para.stopTime = WorkDetails.GetYMD(para.stopTime) + " 23:59:59";
            //我收到的回复
            if (para.state == "1")
            {
                var sql = @"exec WoGetHuiFu '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'," + begin + "," + end;
                var sql1 = @"exec WoGetHuiFuCount '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    idList = conText.Query<string>(sql).ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                }
            }
            //我发出的回复
            if (para.state == "2")
            {
                var sql = @"exec WoSetHuiFu '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'," + begin + "," + end;
                var sql1 = @"exec WoSetHuiFuCount '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    idList = conText.Query<string>(sql).ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                }
            }
            foreach (var item in idList)
            {
                List<WorkReply> list11 = new List<WorkReply>();
                list11 = WorkDetails.GetWorkReply(item, memberid);
                list.AddRange(list11);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkReply>();
            res.showapi_res_body.allNum = allNum;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// @我的工作
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetATMyWorkList(GetATMyInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (para.memberid == null)
            {
                para.memberid = "";
            }
            var sql = @"exec WoDeGongZuo '" + para.memberid + "','" + memberid + "'," + begin + "," + end;
            var sql1 = @"exec WoDeGongZuoCount '" + para.memberid + "','" + memberid + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            //大类  list  处理
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 我收到的赞
        /// </summary>
        public Showapi_Res_List<MyZan> GetMyZanList(MyZanInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<MyZan>();
                return Return.Return();
            }
            Showapi_Res_List<MyZan> res = new Showapi_Res_List<MyZan>();
            List<MyZan> list1 = new List<MyZan>();
            if (string.IsNullOrEmpty(para.companyId))
            {
                var agree = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid && t.IsRead == false).OrderByDescending(t => t.CreateTime).ToList();
                foreach (var item in agree)
                {
                    item.IsRead = true;
                }
            }
            else
            {
                //我收到的赞  未读
                var sql1 = @"select a.* from Agree a
                            left join Work_Approval b on b.Id=a.UId
                            left join Work_Log c on c.Id=a.UId
                            left join Work_Task d on d.Id=a.UId
                            left join Work_Program e on e.Id=a.UId
                            left join Work_Order f on f.Id=a.UId
                            left join Work_Announcement g on g.Id=a.UId
                            left join Work_Share h on h.Id=a.UId
                            where a.OtherMemberId='" + memberid + @"' and
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and
                            (c.CompanyId='" + para.companyId + @"' or c.CompanyId is null or c.CompanyId='') and
                            (d.CompanyId='" + para.companyId + @"' or d.CompanyId is null or d.CompanyId='') and
                            (e.CompanyId='" + para.companyId + @"' or e.CompanyId is null or e.CompanyId='') and
                            (f.CompanyId='" + para.companyId + @"' or f.CompanyId is null or f.CompanyId='') and
                            (g.CompanyId='" + para.companyId + @"' or g.CompanyId is null or g.CompanyId='') and
                            (h.CompanyId='" + para.companyId + @"' or h.CompanyId is null or h.CompanyId='')";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    var qqq = conText.Query<Agree>(sql1).Where(t => t.IsRead == false).ToList();
                    var agreeIdList = "";
                    foreach (var item in qqq)
                    {
                        agreeIdList += "'" + item.Id + "',";
                    }
                    if (agreeIdList != "")
                    {
                        agreeIdList = agreeIdList.Remove(agreeIdList.LastIndexOf(","));
                        var sql2 = "update Agree set IsRead=1 where Id in (" + agreeIdList + ")";
                        using (SqlConnection conText1 = new SqlConnection(constr))
                        {
                            var www = conText1.Query<bool>(sql2);
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();

            List<Agree> list2 = new List<Agree>();
            if (string.IsNullOrEmpty(para.companyId))
            {
                list2 = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid).OrderByDescending(t => t.CreateTime).ToList();
            }
            else
            {
                var sql1 = @"select a.* from Agree a
                            left join Work_Approval b on b.Id=a.UId
                            left join Work_Log c on c.Id=a.UId
                            left join Work_Task d on d.Id=a.UId
                            left join Work_Program e on e.Id=a.UId
                            left join Work_Order f on f.Id=a.UId
                            left join Work_Announcement g on g.Id=a.UId
                            left join Work_Share h on h.Id=a.UId
                            where a.OtherMemberId='" + memberid + @"' and
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and
                            (c.CompanyId='" + para.companyId + @"' or c.CompanyId is null or c.CompanyId='') and
                            (d.CompanyId='" + para.companyId + @"' or d.CompanyId is null or d.CompanyId='') and
                            (e.CompanyId='" + para.companyId + @"' or e.CompanyId is null or e.CompanyId='') and
                            (f.CompanyId='" + para.companyId + @"' or f.CompanyId is null or f.CompanyId='') and
                            (g.CompanyId='" + para.companyId + @"' or g.CompanyId is null or g.CompanyId='') and
                            (h.CompanyId='" + para.companyId + @"' or h.CompanyId is null or h.CompanyId='')
                            order by a.CreateTime desc";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list2 = conText.Query<Agree>(sql1).ToList();
                }
            }
            foreach (var item in list2)
            {
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
                MyZan MyZan = new MyZan();
                MyZan.zannerId = item.MemberId;
                MyZan.zannerName = memberInfo.Name;
                MyZan.zannerPicture = memberInfo.Picture + SasKey;
                MyZan.zanTypeNum = item.Type;
                switch (item.Type)
                {
                    case "1":
                        MyZan.zanType = "审批";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "2":
                        MyZan.zanType = "日志";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "3":
                        MyZan.zanType = "任务";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "4":
                        MyZan.zanType = "日程";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "5":
                        MyZan.zanType = "指令";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "6":
                        MyZan.zanType = "群通知";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "8":
                        MyZan.zanType = "公告";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "9":
                        MyZan.zanType = "分享";
                        MyZan.zanTypeId = item.UId;
                        break;
                }
                if (item.Type.Contains("+"))
                {
                    switch (item.Type.Substring(2, 1))
                    {
                        case "1":
                            MyZan.zanType = "审批的回复";
                            break;
                        case "2":
                            MyZan.zanType = "日志的回复";
                            break;
                        case "3":
                            MyZan.zanType = "任务的回复";
                            break;
                        case "4":
                            MyZan.zanType = "日程的回复";
                            break;
                        case "5":
                            MyZan.zanType = "指令的回复";
                            break;
                        case "8":
                            MyZan.zanType = "公告的回复";
                            break;
                        case "9":
                            MyZan.zanType = "分享的回复";
                            break;
                    }
                    MyZan.zanTypeId = item.P_UId;
                }
                MyZan.zanTypeBody = item.Body;
                MyZan.phoneModel = item.PhoneModel;
                MyZan.time = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                list1.Add(MyZan);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<MyZan>();
            res.showapi_res_body.contentlist = list1;
            return res;
        }
        /// <summary>
        /// 我的赞  收到+发出
        /// </summary>
        public Showapi_Res_List<MyZan> GetMyZanWebList(AllSearch para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<MyZan>();
                return Return.Return();
            }
            Showapi_Res_List<MyZan> res = new Showapi_Res_List<MyZan>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<MyZan> list1 = new List<MyZan>();
            List<Agree> list2 = new List<Agree>();
            var allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            //处理日期入参 规范年月日格式
            if (para.beginTime == "" || para.beginTime == null)
            {
                para.beginTime = "1901-01-01";
            }
            if (para.stopTime == "" || para.stopTime == null)
            {
                para.stopTime = "2200-12-31";
            }
            para.beginTime = WorkDetails.GetYMD(para.beginTime) + " 00:00:00";
            para.stopTime = WorkDetails.GetYMD(para.stopTime) + " 23:59:59";
            //我收到的赞
            if (para.state == "1")
            {
                string sql = @"select a.* from Agree a
                            left join Work_Approval b on b.Id=a.UId
                            left join Work_Log c on c.Id=a.UId
                            left join Work_Task d on d.Id=a.UId
                            left join Work_Program e on e.Id=a.UId
                            left join Work_Order f on f.Id=a.UId
                            left join Work_Announcement g on g.Id=a.UId
                            left join Work_Share h on h.Id=a.UId
                            where a.OtherMemberId='" + memberid + @"' and a.Body like '%" + para.body + @"%' and 
                            (CONVERT(varchar(100), a.CreateTime, 21) between '" + para.beginTime + @"' and '" + para.stopTime + @"') and 
                            (a.Type like case when " + para.type + @"<>0 then '%" + para.type + @"%' else a.Type end) and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and
                            (c.CompanyId='" + para.companyId + @"' or c.CompanyId is null or c.CompanyId='') and
                            (d.CompanyId='" + para.companyId + @"' or d.CompanyId is null or d.CompanyId='') and
                            (e.CompanyId='" + para.companyId + @"' or e.CompanyId is null or e.CompanyId='') and
                            (f.CompanyId='" + para.companyId + @"' or f.CompanyId is null or f.CompanyId='') and
                            (g.CompanyId='" + para.companyId + @"' or g.CompanyId is null or g.CompanyId='') and
                            (h.CompanyId='" + para.companyId + @"' or h.CompanyId is null or h.CompanyId='') 
                            order by a.CreateTime desc";

                string sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateTime desc) row,* from
                            (select a.* from Agree a
                            left join Work_Approval b on b.Id=a.UId
                            left join Work_Log c on c.Id=a.UId
                            left join Work_Task d on d.Id=a.UId
                            left join Work_Program e on e.Id=a.UId
                            left join Work_Order f on f.Id=a.UId
                            left join Work_Announcement g on g.Id=a.UId
                            left join Work_Share h on h.Id=a.UId
                            where a.OtherMemberId='" + memberid + @"' and a.Body like '%" + para.body + @"%' and 
                            (CONVERT(varchar(100), a.CreateTime, 21) between '" + para.beginTime + @"' and '" + para.stopTime + @"') and 
                            (a.Type like case when " + para.type + @"<>0 then '%" + para.type + @"%' else a.Type end) and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and
                            (c.CompanyId='" + para.companyId + @"' or c.CompanyId is null or c.CompanyId='') and
                            (d.CompanyId='" + para.companyId + @"' or d.CompanyId is null or d.CompanyId='') and
                            (e.CompanyId='" + para.companyId + @"' or e.CompanyId is null or e.CompanyId='') and
                            (f.CompanyId='" + para.companyId + @"' or f.CompanyId is null or f.CompanyId='') and
                            (g.CompanyId='" + para.companyId + @"' or g.CompanyId is null or g.CompanyId='') and
                            (h.CompanyId='" + para.companyId + @"' or h.CompanyId is null or h.CompanyId='')
                            ) b) c 
                            where row between " + begin + @" and " + end;

                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Agree>(sql).ToList().Count();
                    list2 = conText.Query<Agree>(sql1).ToList();
                }

                //if (para.type == "0")
                //{
                //    allNum = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid && t.Body.Contains(para.body) && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).ToList().Count();
                //    list2 = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid && t.Body.Contains(para.body) && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();
                //}
                //else
                //{
                //    allNum = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid && t.Body.Contains(para.body) && t.Type == para.type && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).ToList().Count();
                //    list2 = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid && t.Body.Contains(para.body) && t.Type == para.type && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();
                //}
            }
            //我发出的赞
            if (para.state == "2")
            {
                string sql = @"select a.* from Agree a
                            left join Work_Approval b on b.Id=a.UId
                            left join Work_Log c on c.Id=a.UId
                            left join Work_Task d on d.Id=a.UId
                            left join Work_Program e on e.Id=a.UId
                            left join Work_Order f on f.Id=a.UId
                            left join Work_Announcement g on g.Id=a.UId
                            left join Work_Share h on h.Id=a.UId
                            where a.MemberId='" + memberid + @"' and a.Body like '%" + para.body + @"%' and 
                            (CONVERT(varchar(100), a.CreateTime, 21) between '" + para.beginTime + @"' and '" + para.stopTime + @"') and 
                            (a.Type like case when " + para.type + @"<>0 then '%" + para.type + @"%' else a.Type end) and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and
                            (c.CompanyId='" + para.companyId + @"' or c.CompanyId is null or c.CompanyId='') and
                            (d.CompanyId='" + para.companyId + @"' or d.CompanyId is null or d.CompanyId='') and
                            (e.CompanyId='" + para.companyId + @"' or e.CompanyId is null or e.CompanyId='') and
                            (f.CompanyId='" + para.companyId + @"' or f.CompanyId is null or f.CompanyId='') and
                            (g.CompanyId='" + para.companyId + @"' or g.CompanyId is null or g.CompanyId='') and
                            (h.CompanyId='" + para.companyId + @"' or h.CompanyId is null or h.CompanyId='') 
                            order by a.CreateTime desc";

                string sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateTime desc) row,* from
                            (select a.* from Agree a
                            left join Work_Approval b on b.Id=a.UId
                            left join Work_Log c on c.Id=a.UId
                            left join Work_Task d on d.Id=a.UId
                            left join Work_Program e on e.Id=a.UId
                            left join Work_Order f on f.Id=a.UId
                            left join Work_Announcement g on g.Id=a.UId
                            left join Work_Share h on h.Id=a.UId
                            where a.MemberId='" + memberid + @"' and a.Body like '%" + para.body + @"%' and 
                            (CONVERT(varchar(100), a.CreateTime, 21) between '" + para.beginTime + @"' and '" + para.stopTime + @"') and 
                            (a.Type like case when " + para.type + @"<>0 then '%" + para.type + @"%' else a.Type end) and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and
                            (c.CompanyId='" + para.companyId + @"' or c.CompanyId is null or c.CompanyId='') and
                            (d.CompanyId='" + para.companyId + @"' or d.CompanyId is null or d.CompanyId='') and
                            (e.CompanyId='" + para.companyId + @"' or e.CompanyId is null or e.CompanyId='') and
                            (f.CompanyId='" + para.companyId + @"' or f.CompanyId is null or f.CompanyId='') and
                            (g.CompanyId='" + para.companyId + @"' or g.CompanyId is null or g.CompanyId='') and
                            (h.CompanyId='" + para.companyId + @"' or h.CompanyId is null or h.CompanyId='')
                            ) b) c 
                            where row between " + begin + @" and " + end;

                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Agree>(sql).ToList().Count();
                    list2 = conText.Query<Agree>(sql1).ToList();
                }

                //if (para.type == "0")
                //{
                //    allNum = _JointOfficeContext.Agree.Where(t => t.MemberId == memberid && t.Body.Contains(para.body) && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).ToList().Count();
                //    list2 = _JointOfficeContext.Agree.Where(t => t.MemberId == memberid && t.Body.Contains(para.body) && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();
                //}
                //else
                //{
                //    allNum = _JointOfficeContext.Agree.Where(t => t.MemberId == memberid && t.Body.Contains(para.body) && t.Type == para.type && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).ToList().Count();
                //    list2 = _JointOfficeContext.Agree.Where(t => t.MemberId == memberid && t.Body.Contains(para.body) && t.Type == para.type && t.CreateTime > Convert.ToDateTime(para.beginTime) && t.CreateTime < Convert.ToDateTime(para.stopTime)).OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();
                //}
            }
            foreach (var item in list2)
            {
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
                var memberInfo1 = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.OtherMemberId).FirstOrDefault();
                MyZan MyZan = new MyZan();
                MyZan.zannerId = item.MemberId;
                MyZan.zannerName = memberInfo.Name;
                MyZan.zannerPicture = memberInfo.Picture + SasKey;
                MyZan.beizannerName = memberInfo1.Name;
                MyZan.zanTypeNum = item.Type;
                switch (item.Type)
                {
                    case "1":
                        MyZan.zanType = "审批";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "2":
                        MyZan.zanType = "日志";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "3":
                        MyZan.zanType = "任务";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "4":
                        MyZan.zanType = "日程";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "5":
                        MyZan.zanType = "指令";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "6":
                        MyZan.zanType = "群通知";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "8":
                        MyZan.zanType = "公告";
                        MyZan.zanTypeId = item.UId;
                        break;
                    case "9":
                        MyZan.zanType = "分享";
                        MyZan.zanTypeId = item.UId;
                        break;
                }
                if (item.Type.Contains("+"))
                {
                    switch (item.Type.Substring(2, 1))
                    {
                        case "1":
                            MyZan.zanType = "审批的回复";
                            break;
                        case "2":
                            MyZan.zanType = "日志的回复";
                            break;
                        case "3":
                            MyZan.zanType = "任务的回复";
                            break;
                        case "4":
                            MyZan.zanType = "日程的回复";
                            break;
                        case "5":
                            MyZan.zanType = "指令的回复";
                            break;
                        case "8":
                            MyZan.zanType = "公告的回复";
                            break;
                        case "9":
                            MyZan.zanType = "分享的回复";
                            break;
                    }
                    MyZan.zanTypeId = item.P_UId;
                }
                MyZan.zanTypeBody = item.Body;
                MyZan.phoneModel = item.PhoneModel;
                MyZan.time = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                list1.Add(MyZan);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<MyZan>();
            res.showapi_res_body.contentlist = list1;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 我的回执
        /// </summary>
        public Showapi_Res_List<PersonDynamic_info> GetMyReceiptList(AllSearch para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            //处理日期入参 规范年月日格式
            if (para.beginTime == "" || para.beginTime == null)
            {
                para.beginTime = "1901-01-01";
            }
            if (para.stopTime == "" || para.stopTime == null)
            {
                para.stopTime = "2200-12-31";
            }
            //if (para.beginTime.Contains("T"))
            //{
            //    para.beginTime = string.Format("{0:d}", Convert.ToDateTime(para.beginTime));
            //}
            //if (para.stopTime.Contains("T"))
            //{
            //    para.stopTime = string.Format("{0:d}", Convert.ToDateTime(para.stopTime));
            //}
            para.beginTime = WorkDetails.GetYMD(para.beginTime) + " 00:00:00";
            para.stopTime = WorkDetails.GetYMD(para.stopTime) + " 23:59:59";
            //if (para.beginTime == para.stopTime)
            //{
            //    para.stopTime = string.Format("{0:d}", Convert.ToDateTime(para.stopTime).AddDays(1));
            //}
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            para.body = para.body.Replace("'", "");
            //未回执
            if (para.state == "1")
            {
                var sql = @"exec MyReceiptsNo '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'," + begin + "," + end;
                var sql1 = @"exec MyReceiptsNoCount '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list = conText.Query<PersonDynamic_info>(sql).ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    allPages = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPages += 1;
                    }
                }
            }
            //已回执
            if (para.state == "2")
            {
                var sql = @"exec MyReceiptsYes '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'," + begin + "," + end;
                var sql1 = @"exec MyReceiptsYesCount '" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + para.companyId + "'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list = conText.Query<PersonDynamic_info>(sql).ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    allPages = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPages += 1;
                    }
                }
            }
            //大类  list  处理
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 各种提醒的数量
        /// </summary>
        public Showapi_Res_List<NewsRemindNum> GetNewsRemindNumList()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<NewsRemindNum>();
                return Return.Return();
            }
            Showapi_Res_List<NewsRemindNum> res = new Showapi_Res_List<NewsRemindNum>();
            List<NewsRemindNum> list = new List<NewsRemindNum>();
            string[] array = new string[] { "1", "2", "3", "4", "5", "21" };
            //审批待处理
            var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.OtherMemberId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            //日志待点评
            var workLog = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            //我执行的任务
            var workTask1 = _JointOfficeContext.Execute_Content.Where(t => t.OtherMemberId == memberid && t.Type == 3 && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            //我发出的任务
            var workTask2 = _JointOfficeContext.Work_Task.Where(t => t.MemberId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            //日程未开始
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var workProgram = _JointOfficeContext.Work_Program.Where(t => (t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(memberInfo.ZhuBuMen)) && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            //指令待处理
            var workOrder = _JointOfficeContext.Execute_Content.Where(t => t.OtherMemberId == memberid && t.Type == 5 && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            //回复我的  未读
            var replyMe = 0;
            var sql = @"exec ReplyMeIsRead '" + memberid + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                var idList = conText.Query<ReplyMeIsRead>(sql).Where(t => t.isRead == false).ToList();
                replyMe = idList.Count();
            }
            //我收到的赞  未读
            var agree = _JointOfficeContext.Agree.Where(t => t.OtherMemberId == memberid && t.IsRead == false).OrderByDescending(t => t.CreateTime).ToList();
            for (int i = 0; i < array.Length; i++)
            {
                NewsRemindNum NewsRemindNum = new NewsRemindNum();
                switch (array[i])
                {
                    case "1":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = approvalContent.Count();
                        break;
                    case "2":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = workLog.Count();
                        break;
                    case "3":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = workTask1.Count();
                        //NewsRemindNum.num2 = workTask2.Count();
                        break;
                    case "4":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = workProgram.Count();
                        break;
                    case "5":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = workOrder.Count();
                        break;
                    case "21":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = replyMe;
                        NewsRemindNum.num2 = agree.Count();
                        break;
                }
                list.Add(NewsRemindNum);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<NewsRemindNum>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 审批提醒
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<DaiShenPi> GetApprovalRemindList(ApprovalRemindInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiShenPi>();
                return Return.Return();
            }
            Showapi_Res_List<DaiShenPi> res = new Showapi_Res_List<DaiShenPi>();
            List<DaiShenPi> list = new List<DaiShenPi>();
            List<Work_Approval> list1 = new List<Work_Approval>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (para.type == "1")
            {
                var sql = @"select b.* from Approval_Content a
                            inner join Work_Approval b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=0
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Approval_Content a
                            inner join Work_Approval b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=0
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Approval>(sql).ToList().Count();
                    list1 = conText.Query<Work_Approval>(sql1).ToList();
                }
            }
            if (para.type == "2")
            {
                var sql = @"select b.* from Approval_Content a
                            inner join Work_Approval b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            (a.State=1 or a.State=2 or a.State=3)
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Approval_Content a
                            inner join Work_Approval b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            (a.State=1 or a.State=2 or a.State=3)
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Approval>(sql).ToList().Count();
                    list1 = conText.Query<Work_Approval>(sql1).ToList();
                }
            }
            foreach (var item in list1)
            {
                DaiShenPi DaiShenPi = new DaiShenPi();
                DaiShenPi = WorkDetails.GetDaiShenPiOne(item, memberid);
                list.Add(DaiShenPi);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiShenPi>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 日志提醒
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<DaiDianPingDeRiZhi> GetLogRemindList(LogRemindInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiDianPingDeRiZhi>();
                return Return.Return();
            }
            Showapi_Res_List<DaiDianPingDeRiZhi> res = new Showapi_Res_List<DaiDianPingDeRiZhi>();
            List<DaiDianPingDeRiZhi> list = new List<DaiDianPingDeRiZhi>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<Work_Log> list1 = new List<Work_Log>();
            var allNum = 0;
            if (string.IsNullOrEmpty(para.companyId))
            {
                if (para.type == "1")
                {
                    allNum = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).ToList().Count();
                    if (para.count == 0)
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
                    }
                    else
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).Skip(para.page * para.count).Take(para.count).ToList();
                    }
                }
                if (para.type == "2")
                {
                    allNum = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 1).OrderByDescending(t => t.CreateDate).ToList().Count();
                    if (para.count == 0)
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 1).OrderByDescending(t => t.CreateDate).ToList();
                    }
                    else
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 1).OrderByDescending(t => t.CreateDate).Skip(para.page * para.count).Take(para.count).ToList();
                    }
                }
            }
            else
            {
                if (para.type == "1")
                {
                    allNum = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0 && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList().Count();
                    if (para.count == 0)
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0 && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                    }
                    else
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0 && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).Skip(para.page * para.count).Take(para.count).ToList();
                    }
                }
                if (para.type == "2")
                {
                    allNum = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 1 && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList().Count();
                    if (para.count == 0)
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 1 && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                    }
                    else
                    {
                        list1 = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 1 && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).Skip(para.page * para.count).Take(para.count).ToList();
                    }
                }
            }
            foreach (var item in list1)
            {
                DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
                DaiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(item, memberid);
                list.Add(DaiDianPingDeRiZhi);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiDianPingDeRiZhi>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 我执行的任务
        /// </summary>
        public Showapi_Res_List<DaiZhiXingDeRenWu> GetIDoTaskList(IDoTaskInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiZhiXingDeRenWu>();
                return Return.Return();
            }
            Showapi_Res_List<DaiZhiXingDeRenWu> res = new Showapi_Res_List<DaiZhiXingDeRenWu>();
            List<DaiZhiXingDeRenWu> list = new List<DaiZhiXingDeRenWu>();
            List<Work_Task> list1 = new List<Work_Task>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (para.type == "1")
            {
                var sql = @"select b.* from Execute_Content a
                            inner join Work_Task b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 3 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=0
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Execute_Content a
                            inner join Work_Task b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 3 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=0
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Task>(sql).ToList().Count();
                    list1 = conText.Query<Work_Task>(sql1).ToList();
                }
            }
            if (para.type == "2")
            {
                var sql = @"select b.* from Execute_Content a
                            inner join Work_Task b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 3 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=1
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Execute_Content a
                            inner join Work_Task b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 3 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=1
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Task>(sql).ToList().Count();
                    list1 = conText.Query<Work_Task>(sql1).ToList();
                }
            }
            if (para.type == "3")
            {
                var sql = @"select b.* from Execute_Content a
                            inner join Work_Task b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 3 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=2
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Execute_Content a
                            inner join Work_Task b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 3 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=2
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Task>(sql).ToList().Count();
                    list1 = conText.Query<Work_Task>(sql1).ToList();
                }
            }
            foreach (var item in list1)
            {
                DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
                DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(item, memberid);
                list.Add(DaiZhiXingDeRenWu);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiZhiXingDeRenWu>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 我发出的任务
        /// </summary>
        public Showapi_Res_List<DaiZhiXingDeRenWu> GetIPublishTaskList(IPublishTaskInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiZhiXingDeRenWu>();
                return Return.Return();
            }
            Showapi_Res_List<DaiZhiXingDeRenWu> res = new Showapi_Res_List<DaiZhiXingDeRenWu>();
            List<DaiZhiXingDeRenWu> list = new List<DaiZhiXingDeRenWu>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var allNum = 0;
            List<Work_Task> list1 = new List<Work_Task>();
            if (string.IsNullOrEmpty(para.companyId))
            {
                list1 = _JointOfficeContext.Work_Task.Where(t => t.MemberId == memberid).OrderByDescending(t => t.CreateDate).ToList();
            }
            else
            {
                list1 = _JointOfficeContext.Work_Task.Where(t => t.MemberId == memberid && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
            }
            List<Work_Task> list2 = new List<Work_Task>();
            if (para.type == "1")
            {
                allNum = list1.Where(t => t.State == 0).ToList().Count();
                if (para.count == 0)
                {
                    list2 = list1.Where(t => t.State == 0).ToList();
                }
                else
                {
                    list2 = list1.Where(t => t.State == 0).Skip(para.page * para.count).Take(para.count).ToList();
                }
            }
            if (para.type == "2")
            {
                allNum = list1.Where(t => t.State == 1).ToList().Count();
                if (para.count == 0)
                {
                    list2 = list1.Where(t => t.State == 1).ToList();
                }
                else
                {
                    list2 = list1.Where(t => t.State == 1).Skip(para.page * para.count).Take(para.count).ToList();
                }
            }
            if (para.type == "3")
            {
                allNum = list1.Where(t => t.State == 2).ToList().Count();
                if (para.count == 0)
                {
                    list2 = list1.Where(t => t.State == 2).ToList();
                }
                else
                {
                    list2 = list1.Where(t => t.State == 2).Skip(para.page * para.count).Take(para.count).ToList();
                }
            }
            foreach (var item in list2)
            {
                DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
                DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(item, memberid);
                list.Add(DaiZhiXingDeRenWu);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiZhiXingDeRenWu>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 日程列表
        /// </summary>
        /// <param name="年月"></param>
        /// <returns></returns>
        public Showapi_Res_Single<ProgramList> GetProgramList(ProgramListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<ProgramList>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<ProgramList> res = new Showapi_Res_Single<ProgramList>();
            List<ProgramList> list = new List<ProgramList>();
            List<ProgramListDetail> list1 = new List<ProgramListDetail>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            ProgramList ProgramList = new ProgramList();
            List<Work_Program> programList1 = new List<Work_Program>();
            if (string.IsNullOrEmpty(para.companyId))
            {
                programList1 = _JointOfficeContext.Work_Program.Where(t => t.MemberId == memberid || t.JoinPerson.Contains(memberid)).ToList();
            }
            else
            {
                programList1 = _JointOfficeContext.Work_Program.Where(t => t.MemberId == memberid || t.JoinPerson.Contains(memberid) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).ToList();
            }
            if (para.type == "1")
            {
                if (para.yy_mm.Length == 6)
                {
                    var timeY = para.yy_mm.Substring(0, 5);
                    var timeM = "0" + para.yy_mm.Substring(5, 1);
                    para.yy_mm = timeY + timeM;
                }

                if (para.yy_mm != null && para.yy_mm != "")
                {
                    var programList2 = programList1.Where(t => t.Year.Substring(0, 7) == para.yy_mm).OrderBy(t => t.Year).ToList();
                    foreach (var item in programList2)
                    {
                        var days = item.Year.Substring(8, 2);
                        list2.Add(days);
                    }
                    var programListSet = new HashSet<string>(list2);
                    foreach (var item in programListSet)
                    {
                        var days = item;
                        list3.Add(days);
                    }
                }
            }
            if (para.type == "2")
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                para.yy_mm_dd = WorkDetails.GetYMD(para.yy_mm_dd);

                if (para.yy_mm_dd != null && para.yy_mm_dd != "")
                {
                    var programList3 = programList1.Where(t => t.Year == para.yy_mm_dd).OrderBy(t => t.CreateDate).ToList();
                    foreach (var item in programList3)
                    {
                        ProgramListDetail ProgramListDetail = new ProgramListDetail();
                        ProgramListDetail.hh_mm = item.Hour;
                        ProgramListDetail.id = item.Id;
                        ProgramListDetail.body = item.Body;
                        list1.Add(ProgramListDetail);
                    }
                }
            }
            ProgramList.programList = list3;
            ProgramList.programListDetail = list1;
            res.showapi_res_body = ProgramList;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 日程通知
        /// </summary>
        /// <param name="日程状态"></param>
        /// <returns></returns>
        public Showapi_Res_List<RiChengDetail> GetProgramNoticeList(ProgramNoticeInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<RiChengDetail>();
                return Return.Return();
            }
            Showapi_Res_List<RiChengDetail> res = new Showapi_Res_List<RiChengDetail>();
            List<RiChengDetail> list = new List<RiChengDetail>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var allNum = 0;
            List<Work_Program> list1 = new List<Work_Program>();
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (string.IsNullOrEmpty(para.companyId))
            {
                list1 = _JointOfficeContext.Work_Program.Where(t => t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(memberInfo.ZhuBuMen)).OrderByDescending(t => t.CreateDate).ToList();
            }
            else
            {
                list1 = _JointOfficeContext.Work_Program.Where(t => (t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(memberInfo.ZhuBuMen)) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
            }
            List<Work_Program> list2 = new List<Work_Program>();
            if (para.type == "1")
            {
                allNum = list1.Where(t => t.State == 1).ToList().Count();
                if (para.count == 0)
                {
                    list2 = list1.Where(t => t.State == 1).ToList();
                }
                else
                {
                    list2 = list1.Where(t => t.State == 1).Skip(para.page * para.count).Take(para.count).ToList();
                }
            }
            if (para.type == "2")
            {
                allNum = list1.Where(t => t.State == 0).ToList().Count();
                if (para.count == 0)
                {
                    list2 = list1.Where(t => t.State == 0).ToList();
                }
                else
                {
                    list2 = list1.Where(t => t.State == 0).Skip(para.page * para.count).Take(para.count).ToList();
                }
            }
            foreach (var item in list2)
            {
                RiChengDetail RiChengDetail = new RiChengDetail();
                RiChengDetail = WorkDetails.GetRiChengDetailOne(item, memberid);
                list.Add(RiChengDetail);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<RiChengDetail>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 指令提醒
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<ZhiLingDetail> GetOrderRemindList(OrderRemindInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<ZhiLingDetail>();
                return Return.Return();
            }
            Showapi_Res_List<ZhiLingDetail> res = new Showapi_Res_List<ZhiLingDetail>();
            List<ZhiLingDetail> list = new List<ZhiLingDetail>();
            List<Work_Order> list1 = new List<Work_Order>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (para.type == "1")
            {
                var sql = @"select b.* from Execute_Content a
                            inner join Work_Order b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 5 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=0
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Execute_Content a
                            inner join Work_Order b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 5 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            a.State=0
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Order>(sql).ToList().Count();
                    list1 = conText.Query<Work_Order>(sql1).ToList();
                }
            }
            if (para.type == "2")
            {
                var sql = @"select b.* from Execute_Content a
                            inner join Work_Order b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 5 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            (a.State=1 or a.State=2)
                            order by b.CreateDate desc";
                var sql1 = @"select * from
                            (select ROW_NUMBER() OVER (order by CreateDate desc) row,* from
                            (select b.* from Execute_Content a
                            inner join Work_Order b on a.UId=b.Id
                            where a.OtherMemberId='" + memberid + @"' and a.Type = 5 and 
                            (b.CompanyId='" + para.companyId + @"' or b.CompanyId is null or b.CompanyId='') and 
                            (a.State=1 or a.State=2)
                            ) b) c
                            where row between " + begin + @" and " + end;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    allNum = conText.Query<Work_Order>(sql).ToList().Count();
                    list1 = conText.Query<Work_Order>(sql1).ToList();
                }
            }
            foreach (var item in list1)
            {
                ZhiLingDetail ZhiLingDetail = new ZhiLingDetail();
                ZhiLingDetail = WorkDetails.GetZhiLingDetailOne(item, memberid);
                list.Add(ZhiLingDetail);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ZhiLingDetail>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 创建人取消(审批,任务,指令)
        /// </summary>
        public Showapi_Res_Meaasge Cancel(CancelInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            Comment_Body Comment_Body = new Comment_Body();
            TotalNum TotalNum = new TotalNum();
            Work_Approval approval = null;
            Work_Task task = null;
            Work_Order order = null;
            if (para.type == "1")
            {
                approval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
                if (approval == null)
                {
                    throw new BusinessTureException("此审批已删除");
                }
            }
            if (para.type == "3")
            {
                task = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
                if (task == null)
                {
                    throw new BusinessTureException("此任务已删除");
                }
            }
            if (para.type == "5")
            {
                order = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
                if (order == null)
                {
                    throw new BusinessTureException("此指令已删除");
                }
            }
            if (para.type == "1")
            {
                var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.id && (t.State == 0 || t.State == 5)).ToList();
                if (approval != null)
                {
                    approval.State = 2;
                    approval.ApprovalPersonNum = 0;

                    if (approvalContent != null && approvalContent.Count != 0)
                    {
                        foreach (var item in approvalContent)
                        {
                            item.State = 6;
                            item.IsMeApproval = "0";
                            item.Content = "";
                            item.ApprovalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            item.PhoneModel = para.phoneModel;
                        }
                    }

                    Comment_Body.Body = "取消审批。";
                    TotalNum.Type = "7+1";
                }
            }
            if (para.type == "3")
            {
                var execute_Content = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.id && t.State == 0).ToList();
                if (task != null)
                {
                    task.State = 2;

                    if (execute_Content.Count != 0 && execute_Content != null)
                    {
                        foreach (var item in execute_Content)
                        {
                            item.State = 3;
                            item.Content = "";
                            item.ExecuteDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            item.PhoneModel = para.phoneModel;
                        }
                    }

                    Comment_Body.Body = "取消任务。";
                    TotalNum.Type = "7+3";
                }
            }
            if (para.type == "5")
            {
                var execute_Content = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.id && t.State == 0).FirstOrDefault();
                if (order != null)
                {
                    order.State = 2;

                    if (execute_Content != null)
                    {
                        execute_Content.State = 3;
                        execute_Content.Content = "";
                        execute_Content.ExecuteDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        execute_Content.PhoneModel = para.phoneModel;
                    }

                    Comment_Body.Body = "取消指令。";
                    TotalNum.Type = "7+5";
                }
            }

            Comment_Body.Id = Guid.NewGuid().ToString();
            Comment_Body.PingLunMemberId = memberid;
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Comment_Body.Name = memberInfo.Name;
            Comment_Body.Picture = memberInfo.Picture;
            Comment_Body.PingLunTime = DateTime.Now;
            Comment_Body.UId = para.id;
            Comment_Body.MemberId = memberid;
            Comment_Body.PId = "";
            Comment_Body.OtherBody = "";
            Comment_Body.PersonId = "";
            Comment_Body.PersonName = "";
            Comment_Body.Type = para.type;
            Comment_Body.IsExeComment = 1;
            Comment_Body.PictureList = "";
            Comment_Body.Voice = "";
            Comment_Body.VoiceLength = "";
            Comment_Body.Annex = "";
            Comment_Body.PhoneModel = para.phoneModel;
            Comment_Body.ATPerson = "";
            _JointOfficeContext.Comment_Body.Add(Comment_Body);

            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.UId = "";
            TotalNum.PId = Comment_Body.Id;
            TotalNum.P_UId = para.id;
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);

            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.id).FirstOrDefault();
            if (totalNum != null)
            {
                totalNum.PingLunNum += 1;
            }

            _JointOfficeContext.SaveChanges();
            if (para.type == "1")
            {
                SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                var parems = SendHelper.getXiaoXiParams(para.id, "1");
                var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(approval.ApprovalPerson);
                foreach (var item in list1)
                {
                    SendHelper.SendXiaoXi("你待审批的审批已取消", item.id, parems);
                }
            }
            else if (para.type == "3")
            {
                SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                var parems = SendHelper.getXiaoXiParams(para.id, "3");
                var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(task.Executor);
                foreach (var item in list1)
                {
                    SendHelper.SendXiaoXi("你待执行的任务已取消", item.id, parems);
                }
            }
            else if (para.type == "5")
            {
                SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                var parems = SendHelper.getXiaoXiParams(para.id, "5");
                var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(order.Executor);
                foreach (var item in list1)
                {
                    SendHelper.SendXiaoXi("你待执行的指令已取消", item.id, parems);
                }
            }
            return Message.SuccessMeaasge("取消成功");
        }
        /// <summary>
        /// 删除(1审批 2日志 3任务 4日程 5指令)
        /// </summary>
        public Showapi_Res_Meaasge Delete(DeleteInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (para.type == "1")
            {
                var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
                if (workApproval == null)
                {
                    throw new BusinessTureException("此审批已删除");
                }
                _JointOfficeContext.Work_Approval.Remove(workApproval);
                var approval = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.id).ToList();
                foreach (var item in approval)
                {
                    _JointOfficeContext.Approval_Content.Remove(item);
                }
            }
            if (para.type == "2")
            {
                var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
                if (workLog == null)
                {
                    throw new BusinessTureException("此日志已删除");
                }
                _JointOfficeContext.Work_Log.Remove(workLog);
            }
            if (para.type == "3")
            {
                var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
                if (workTask == null)
                {
                    throw new BusinessTureException("此任务已删除");
                }
                _JointOfficeContext.Work_Task.Remove(workTask);
                var executor = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.id).ToList();
                foreach (var item in executor)
                {
                    _JointOfficeContext.Execute_Content.Remove(item);
                }
            }
            if (para.type == "4")
            {
                var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
                if (workProgram == null)
                {
                    throw new BusinessTureException("此日程已删除");
                }
                _JointOfficeContext.Work_Program.Remove(workProgram);
            }
            if (para.type == "5")
            {
                var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
                if (workOrder == null)
                {
                    throw new BusinessTureException("此指令已删除");
                }
                _JointOfficeContext.Work_Order.Remove(workOrder);
                var executor = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.id).FirstOrDefault();
                _JointOfficeContext.Execute_Content.Remove(executor);
            }
            if (para.type == "8")
            {
                var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.id).FirstOrDefault();
                if (workAnnouncement == null)
                {
                    throw new BusinessTureException("此公告已删除");
                }
                _JointOfficeContext.Work_Announcement.Remove(workAnnouncement);
            }
            if (para.type == "9")
            {
                var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == para.id).FirstOrDefault();
                if (workShare == null)
                {
                    throw new BusinessTureException("此分享已删除");
                }
                _JointOfficeContext.Work_Share.Remove(workShare);
            }

            var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == para.id).ToList();
            foreach (var item in comment)
            {
                _JointOfficeContext.Comment_Body.Remove(item);
            }
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.id || t.P_UId == para.id).ToList();
            foreach (var item in totalNum)
            {
                _JointOfficeContext.TotalNum.Remove(item);
            }
            var agree = _JointOfficeContext.Agree.Where(t => t.UId == para.id || t.P_UId == para.id).ToList();
            foreach (var item in agree)
            {
                _JointOfficeContext.Agree.Remove(item);
            }
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == para.id).ToList();
            foreach (var item in focus)
            {
                _JointOfficeContext.News_Focus.Remove(item);
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == para.id).ToList();
            foreach (var item in collection)
            {
                _JointOfficeContext.News_Collection.Remove(item);
            }
            var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == para.id).ToList();
            foreach (var item in receipts)
            {
                _JointOfficeContext.Receipts.Remove(item);
            }

            if (para.type == "2")
            {
                SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                var parems = SendHelper.getXiaoXiParams(para.id, "2");
                var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
                SendHelper.SendXiaoXi("你待点评的日志已删除", workLog.ReviewPersonId, parems);
            }
            else if (para.type == "4")
            {
                SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                var parems = SendHelper.getXiaoXiParams(para.id, "4");
                var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
                var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(workProgram.JoinPerson);
                foreach (var item in list1)
                {
                    SendHelper.SendXiaoXi("你待参与的日程已删除", item.id, parems);
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 群文档
        /// </summary>
        public Showapi_Res_List<GroupDocument> GetGroupDocumentList(GroupDocumentInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GroupDocument>();
                return Return.Return();
            }
            Showapi_Res_List<GroupDocument> res = new Showapi_Res_List<GroupDocument>();
            List<GroupDocument> list = new List<GroupDocument>();
            var NoteList = _JointOfficeContext.News_News.Where(t => t.GroupId == para.id && t.InfoType == "3").OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();
            foreach (var item in NoteList)
            {
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.NewsSenderId).FirstOrDefault();
                GroupDocument GroupDocument = new GroupDocument();
                GroupDocument.personName = memberInfo.Name;
                GroupDocument.name = item.FileName;
                GroupDocument.url = item.Body;
                GroupDocument.size = item.Length;
                GroupDocument.time = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                list.Add(GroupDocument);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GroupDocument>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 群图片
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<AllPicture> GetAllPictureList(GroupDocumentInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<AllPicture>();
                return Return.Return();
            }
            Showapi_Res_List<AllPicture> res = new Showapi_Res_List<AllPicture>();
            List<AllPicture> list = new List<AllPicture>();
            var NoteList = _JointOfficeContext.News_News.Where(t => t.GroupId == para.id && t.InfoType == "2").OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();
            foreach (var item in NoteList)
            {
                AllPicture AllPicture = new AllPicture();
                AllPicture.time = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                AllPicture.url = item.Body + SasKey;
                list.Add(AllPicture);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<AllPicture>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 消息列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<NewsList> GetNewsList(GetNewsListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            //memberid = "ccd2ed5a-8260-471c-8de5-3c6336993bd6";
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<NewsList>();
                return Return.Return();
            }

            Showapi_Res_List<NewsList> res = new Showapi_Res_List<NewsList>();
            List<NewsList> list = new List<NewsList>();
            List<NewsInfo> newlist = new List<NewsInfo>();
            //var sql = @"select a.GroupId id,b.type,b.infotype,b.body,c.Name name,c.picture,d.Name groupName,d.Picture groupPicture,convert(nvarchar,b.CreateTime,23) time
            //            from News_Member a
            //            left join  (select top 1 body,InfoType,GroupId,CreateTime,NewsSenderId,Type from News_News  order by CreateTime desc ) b on a.GroupId=b.GroupId
            //            left join Member_Info c on b.NewsSenderId=c.MemberId
            //            left join Member_Group d on a.GroupId=d.Id
            //            order by b.CreateTime desc";
            var sql = @"select a.GroupId id,b.type,b.infotype,b.body,c.Name name,c.picture,d.Name groupName,d.Picture groupPicture,b.time,a.WeiDuGroupPersonId weidu,a.grouppersonid,d.State
                        from News_Member a
                        left join (select a.body,a.InfoType,a.GroupId,a.CreateTime,a.NewsSenderId,a.Type,a.Time
						from News_News a inner join  (select  max(CreateTime) CreateTime,GroupId  from News_News where SeePerson like '%"+memberid+@"%' group by GroupId  ) b on a.GroupId=b.GroupId and a.CreateTime=b.CreateTime) b on a.GroupId=b.GroupId
                        left join Member_Info c on b.NewsSenderId=c.MemberId
                        left join Member_Group d on a.GroupId=d.Id
						where a.GroupPersonId like '%" + memberid + "%' and a.DeleteGroupPersonId not like '%" + memberid + "%' and b.time>'" + para.time + "' order by b.CreateTime desc";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                newlist = conText.Query<NewsInfo>(sql).ToList();
            }
            var update = false;
            //newlist= newlist.Skip(para.page * para.count).Take(para.count).ToList();
            foreach (var item in newlist)
            {
                NewsList NewsList = new NewsList();
                if (item.type != "" && item.type != "null")
                {
                    if (item.weidu == null)
                    {
                        var personlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(item.grouppersonid);
                        List<WeiDuInfo> list1 = new List<WeiDuInfo>();
                        foreach (var one in personlist)
                        {
                            WeiDuInfo WeiDuInfo = new WeiDuInfo();
                            WeiDuInfo.memberId = one.memberid;
                            WeiDuInfo.count = 0;
                            list1.Add(WeiDuInfo);
                        }
                        var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(list1);

                        var News_Member = _JointOfficeContext.News_Member.Where(t => t.GroupId == item.id).FirstOrDefault();
                        if (News_Member != null)
                        {
                            News_Member.WeiDuGroupPersonId = weiduInfo;
                            update = true;
                            item.weidu = weiduInfo;
                        }

                    }
                    var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(item.weidu);
                    var info = objectList.Where(t => t.memberId == memberid).FirstOrDefault();
                    if (info != null)
                    {
                        NewsList.typeNum = info.count;
                    }
                    else
                    {
                        NewsList.typeNum = 0;
                    }
                    NewsList.type = item.type;
                    NewsList.groupId = item.id;
                    var memGroup = _JointOfficeContext.Member_Group.Where(t => t.Id == item.id).FirstOrDefault();
                    var personNum = "";
                    if (memGroup != null && !string.IsNullOrEmpty(memGroup.GroupPersonId))
                    {
                        var strList = JsonConvert.DeserializeObject<List<MemberID>>(memGroup.GroupPersonId);
                        personNum = strList.Count() + "人";
                    }
                    NewsList.personNum = personNum;
                    if (item.type == "1")
                    {
                        NewsList.id = item.id.Replace(memberid, "");
                    }
                    else
                    {
                        NewsList.id = item.id;
                    }
                    NewsList.time = item.time;

                    //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
                    //DateTime dt = startTime.AddMilliseconds(Convert.ToDouble(item.time));
                    var newsnews = _JointOfficeContext.News_News.Where(t => t.GroupId == item.id).OrderByDescending(t => t.CreateTime).FirstOrDefault();
                    if (newsnews != null)
                    {
                        DateTime dtNow = DateTime.Now;
                        var qqq = dtNow - newsnews.CreateTime;
                        var www = qqq.Days;
                        if (www < 1)
                        {
                            NewsList.time1 = newsnews.CreateTime.Hour.ToString() + ":" + newsnews.CreateTime.Minute.ToString();
                        }
                        else
                        {
                            NewsList.time1 = www + "天前";
                        }
                    }
                    else
                    {
                        NewsList.time1 = "";
                    }
                    NewsList.isGroup = item.State;
                    if (item.infotype == "2")
                    {
                        NewsList.body = "[图片]";
                    }
                    else if (item.infotype == "3")
                    {
                        NewsList.body = "[文档]";
                    }
                    else if (item.infotype == "4")
                    {
                        NewsList.body = "[录音]";
                    }
                    else if (item.infotype == "5")
                    {
                        NewsList.body = "[视频]";
                    }
                    else if (item.infotype == "6")
                    {
                        NewsList.body = "[位置]";
                    }
                    else
                    {
                        NewsList.body = item.body;
                    }
                    if (item.type == "2")
                    {
                        NewsList.name = item.groupName;
                        NewsList.url = item.groupPicture + SasKey;
                    }
                    else
                    {
                        var id = item.id.Replace(memberid, "");
                        var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == id).FirstOrDefault();
                        NewsList.name = memberInfo.Name;
                        NewsList.url = memberInfo.Picture + SasKey;
                    }

                }

                list.Add(NewsList);
            }
            if (update)
            {
                _JointOfficeContext.SaveChanges();
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<NewsList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<NewsList> GetNewsInfo(CleanConversationInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<NewsList>();
                return Return.Return();
            }
            Showapi_Res_Single<NewsList> res = new Showapi_Res_Single<NewsList>();
            NewsList NewsList = new NewsList();
            NewsList.id = para.id;
            var Group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.id).FirstOrDefault();


            var News = _JointOfficeContext.News_News.Where(t => t.GroupId == para.id).OrderByDescending(t => t.CreateTime).FirstOrDefault();
            if (News != null)
            {
                if (News.InfoType == "2")
                {
                    NewsList.body = "[图片]";
                }
                else
                {
                    NewsList.body = News.Body;
                }
                NewsList.time = News.CreateTime.ToString();
            }
            //if (News.Type == 1)
            //{
            //    var id = Group.Id.Replace(memberid, "");
            //    var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == id).FirstOrDefault();
            //    NewsList.name = memberInfo.Name;
            //    NewsList.url = memberInfo.Picture + SasKey;
            //}
            //else
            //{
            NewsList.name = Group.Name;
            NewsList.url = Group.Picture + SasKey;
            //}

            res.showapi_res_body = NewsList;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 消息列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<NewsInfoList> GetNewsInfoList(GetNewsInfoListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            //memberid = "9bf1d5cd-4ffc-4a8d-8a2a-35855da2d18f";
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<NewsInfoList>();
                return Return.Return();
            }
            var id = para.id;
            if (para.type == 1)
            {
                List<string> list1 = new List<string>();
                list1.Add(para.id);
                list1.Add(memberid);
                list1.Sort();
                var qunzuid = "";
                foreach (var item in list1)
                {
                    qunzuid = qunzuid + item;
                }
                id = qunzuid;
            }
            Showapi_Res_List<NewsInfoList> res = new Showapi_Res_List<NewsInfoList>();
            List<NewsInfoList> list = new List<NewsInfoList>();
            //var newlist = new List<News_News>();

            var newlist = _JointOfficeContext.News_News.Where(t => t.GroupId == id && t.NewsSenderId != null && t.NewsSenderId != "" &&t.SeePerson.Contains(memberid)).OrderByDescending(t => t.CreateTime).Skip(para.page * para.count).Take(para.count).ToList();

            var group = _JointOfficeContext.Member_Group.Where(t => t.Id == id).FirstOrDefault();
            var ingroup = 0;
            if (group != null)
            {
                if (group.GroupPersonId.Contains(memberid) && group.State == 1)
                {
                    ingroup = 1;
                }
            }
            foreach (var item in newlist)
            {
                NewsInfoList NewsInfoList = new NewsInfoList();
                //NewsInfoList.id = item.GroupId;
                NewsInfoList.type = item.Type.ToString();
                NewsInfoList.fileType = item.InfoType;
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.NewsSenderId).FirstOrDefault();
                NewsInfoList.url = info.Picture + SasKey;
                NewsInfoList.name = info.Name;
                if (NewsInfoList.fileType != "1"&& NewsInfoList.fileType != "7")
                {
                    NewsInfoList.body = item.Body + SasKey;
                }
                else
                {
                    NewsInfoList.body = item.Body;
                }

                NewsInfoList.map = item.Map;
                NewsInfoList.time = item.Time;
                NewsInfoList.map = item.Map;
                NewsInfoList.length = item.Length;
                NewsInfoList.address = item.Address;
                NewsInfoList.filename = item.FileName;
                NewsInfoList.newSenderId = item.NewsSenderId;
                NewsInfoList.baseurl = item.BaseUrl;
                if (para.type == 1)
                {
                    NewsInfoList.newReceiveId = id.Replace(item.NewsSenderId, "");
                }
                else
                {
                    NewsInfoList.newReceiveId = id;
                }
                if (item.NewsSenderId == memberid)
                {
                    NewsInfoList.newType = "1";
                }
                else
                {
                    NewsInfoList.newType = "2";
                }
                list.Add(NewsInfoList);
            }
            var NewsMember = _JointOfficeContext.News_Member.Where(t => t.GroupId == id).FirstOrDefault();
            if (NewsMember != null)
            {
                if (NewsMember.WeiDuGroupPersonId == null)
                {
                    var personlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(NewsMember.GroupPersonId);
                    List<WeiDuInfo> list1 = new List<WeiDuInfo>();
                    foreach (var one in personlist)
                    {
                        WeiDuInfo WeiDuInfo = new WeiDuInfo();
                        WeiDuInfo.memberId = one.memberid;
                        WeiDuInfo.count = 0;
                        list1.Add(WeiDuInfo);
                    }
                    var newweiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                    NewsMember.WeiDuGroupPersonId = newweiduInfo;
                    _JointOfficeContext.SaveChanges();
                }
                var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(NewsMember.WeiDuGroupPersonId);
                foreach (var item in objectList)
                {
                    if (item.memberId == memberid)
                    {
                        item.count = 0;
                    }
                }
                var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(objectList);
                NewsMember.WeiDuGroupPersonId = weiduInfo;
                _JointOfficeContext.SaveChanges();
            }


            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<NewsInfoList>();
            res.showapi_res_body.contentlist = list.OrderBy(t => t.time).ToList();
            res.showapi_res_body.unread = ingroup;
            return res;
        }
        /// <summary>
        /// 删除会话窗口
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteNewsList(DeleteNewsListPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var id = para.id;
            if (para.type == "1")
            {
                List<string> list1 = new List<string>();
                list1.Add(para.id);
                list1.Add(memberid);
                list1.Sort();
                var qunzuid = "";
                foreach (var item in list1)
                {
                    qunzuid = qunzuid + item;
                }
                id = qunzuid;
            }
            var info = _JointOfficeContext.News_Member.Where(t => t.GroupId == id).FirstOrDefault();
            //People People = new People();
            //People.memberid = memberid;

            if (info.GroupPersonId != "" && info.GroupPersonId != null)
            {
                if (!info.DeleteGroupPersonId.Contains(memberid))
                {
                    //var renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(info.GroupPersonId);
                    //var one = renyuanlist.Where(t => t.memberid == memberid).FirstOrDefault();
                    //renyuanlist.Remove(one);
                    //info.GroupPersonId = Newtonsoft.Json.JsonConvert.SerializeObject(renyuanlist);
                    info.DeleteGroupPersonId = info.DeleteGroupPersonId + memberid;

                    var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(info.WeiDuGroupPersonId);
                    foreach (var item in objectList)
                    {
                        if (item.memberId == memberid)
                        {
                            item.count = 0;
                        }
                    }
                    var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(objectList);
                    info.WeiDuGroupPersonId = weiduInfo;
                    var group = _JointOfficeContext.Member_Group.Where(t => t.Id == id).FirstOrDefault();
                    if (group!=null)
                    {
                        if (!group.GroupPersonId.Contains(memberid))
                        {
                            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(info.GroupPersonId);
                            var one = list.Where(t => t.memberid == memberid).FirstOrDefault();
                            if (one!=null)
                            {
                                list.Remove(one);
                                info.GroupPersonId = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                            }
                            
                        }
                    }


                    _JointOfficeContext.SaveChanges();
                }
            }
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 获取人员和权限信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GetNewsPersonInfoList> GetNewsPersonInfo(CleanConversationInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<GetNewsPersonInfoList>();
                return Return.Return();
            }
            Showapi_Res_Single<GetNewsPersonInfoList> res = new Showapi_Res_Single<GetNewsPersonInfoList>();
            GetNewsPersonInfoList NewsList = new GetNewsPersonInfoList();
            List<PersonInfo> list = new List<PersonInfo>();
            NewsList.id = para.id;
            var Group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.id).FirstOrDefault();
            NewsList.isGroup = Group.State;
            var renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(Group.GroupPersonId);
            foreach (var item in renyuanlist)
            {
                PersonInfo PersonInfo = new PersonInfo();
                PersonInfo.memberid = item.memberid;
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.memberid).FirstOrDefault();
                PersonInfo.name = info.Name;
                PersonInfo.jobName = "";
                var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == info.JobID).FirstOrDefault();
                if (memJob != null)
                {
                    PersonInfo.jobName = memJob.Name;
                }
                if (Group.GroupKingMemberId == item.memberid)
                {
                    PersonInfo.type = "1";
                }
                else
                {
                    PersonInfo.type = "0";
                }
                PersonInfo.picture = info.Picture + SasKey;
                list.Add(PersonInfo);
            }
            list = list.OrderByDescending(t => t.type).ToList();
            NewsList.personList = list;
            NewsList.name = Group.Name;
            NewsList.picture = Group.Picture + SasKey;
            NewsList.number = renyuanlist.Count().ToString();
            if (Group.GroupKingMemberId == memberid)
            {
                NewsList.type = "1";
            }
            else
            {
                NewsList.type = "0";
            }
            res.showapi_res_body = NewsList;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge DissolutionGroup(CleanConversationInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var Member_Group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.id).FirstOrDefault();
            if (Member_Group == null)
            {

                throw new BusinessTureException("不存在此群组.");
            }
            else
            {
                //var url2 = "http://api.cn.ronghub.com/message/group/publish.json";
                //var parastr = "";

                //GroupMessagePersonPara GroupMessagePersonPara = new GroupMessagePersonPara();
                //GroupMessagePersonPara.operatorUserId = memberid;
                //GroupMessagePersonPara.operation = "Dismiss";
                //GroupMessagePersonPara.message = "解散群组";
                //GroupMessagePersonPara.extra = "JieSan";
                //dataPersonPara data = new dataPersonPara();
                //var name = "";
                //List<string> list2 = new List<string>();
                //List<string> list1 = new List<string>();

                //var Member_info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                //data.operatorNickname = Member_info.Name;
                //GroupMessagePersonPara.data = data;
                //var message = Newtonsoft.Json.JsonConvert.SerializeObject(GroupMessagePersonPara);
                //parastr = parastr + "fromUserId=" + memberid + "&toGroupId=" + Member_Group.Id + "&objectName=RC:GrpNtf&content=" + message + "&isIncludeSender=1";
                //var postRes2 = WebApiHelper.PostAsynctMethod<RongYun>(url2, parastr, appkey, appsecret);
                //if (postRes2.code != "200")
                //{
                //    //throw new BusinessTureException("创建失败.");
                //}
                //string postUrl = "http://api.cn.ronghub.com/group/dismiss.json";
                //string postStr = "";

                //postStr += "userId=" + memberid;

                //postStr += "&groupId=" + Member_Group.Id;
                //var postRes = WebApiHelper.PostAsynctMethod<RongYun>(postUrl, postStr, appkey, appsecret);
                //if (postRes.code != "200")
                //{
                //    throw new BusinessTureException("解散失败.");
                //}
                //Member_Group.State = 0;
                var sql = "";
                Member_Group.State = 0;
                sql += "delete ofGroup where groupName='" + Member_Group.Id + "';";
                sql += "delete ofGroupUser where groupName='" + Member_Group.Id + "';";
                sql += "delete ofGroupProp where groupName='" + Member_Group.Id + "';";
                using (SqlConnection conText = new SqlConnection(ImConnection))
                {
                    conText.Open();
                    SqlTransaction transaction;
                    using (SqlCommand cmd = conText.CreateCommand())
                    {
                        //启动事务
                        transaction = conText.BeginTransaction();
                        cmd.Connection = conText;
                        cmd.Transaction = transaction;
                        try
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            //完成提交
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            //数据回滚
                            transaction.Rollback();
                            throw new BusinessTureException(ex.Message);
                        }
                    }
                }
                var NewsMember = _JointOfficeContext.News_Member.Where(t => t.GroupId == para.id).FirstOrDefault();
                if (NewsMember != null)
                {
                    if (NewsMember.WeiDuGroupPersonId == null)
                    {
                        var personlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(NewsMember.WeiDuGroupPersonId);
                        foreach (var one in personlist)
                        {
                            one.count = 0;
                        }
                        var newweiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(personlist);
                        NewsMember.WeiDuGroupPersonId = newweiduInfo;
                    }
                }
                _JointOfficeContext.SaveChanges();

            }
            return Message.SuccessMeaasge("解散成功");
        }
        /// <summary>
        /// 搜索聊天记录
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<SearchNewsInfoList> SearchNewsInfoList(SearchNewsInfoListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<SearchNewsInfoList>();
                return Return.Return();
            }
            var id = para.id;
            if (para.type == "1")
            {
                List<string> list1 = new List<string>();
                list1.Add(para.id);
                list1.Add(memberid);
                list1.Sort();
                var qunzuid = "";
                foreach (var item in list1)
                {
                    qunzuid = qunzuid + item;
                }
                id = qunzuid;
            }
            Showapi_Res_List<SearchNewsInfoList> res = new Showapi_Res_List<SearchNewsInfoList>();
            List<SearchNewsInfoList> list = new List<SearchNewsInfoList>();
            var newlist = _JointOfficeContext.News_News.Where(t => t.GroupId == id && t.InfoType == "1" && t.Body.Contains(para.body)).OrderByDescending(t => t.CreateTime).ToList();
            foreach (var item in newlist)
            {
                SearchNewsInfoList SearchNewsInfoList = new SearchNewsInfoList();
                //NewsInfoList.id = item.GroupId;
                SearchNewsInfoList.body = item.Body;
                SearchNewsInfoList.time = item.CreateTime.ToString();
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.NewsSenderId).FirstOrDefault();
                SearchNewsInfoList.url = info.Picture + SasKey;
                SearchNewsInfoList.name = info.Name;
                SearchNewsInfoList.id = item.NewsSenderId;
                list.Add(SearchNewsInfoList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<SearchNewsInfoList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 根据memberid获取头像名称
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GetNamePictureInfo> GetNamePicture(GetNamePicturePara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            var count = 0;
            //memberid = "9bf1d5cd-4ffc-4a8d-8a2a-35855da2d18f";
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<GetNamePictureInfo>();
                return Return.Return();
            }
            Showapi_Res_Single<GetNamePictureInfo> res = new Showapi_Res_Single<GetNamePictureInfo>();

            GetNamePictureInfo GetNamePictureInfo = new GetNamePictureInfo();
            if (para.type == 2)
            {
                var member_group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.groupId).FirstOrDefault();
                if (member_group != null)
                {
                    GetNamePictureInfo.name = member_group.Name;
                    GetNamePictureInfo.picture = member_group.Picture + SasKey;
                    if(member_group.GroupPersonId.Contains(memberid))
                    {
                        GetNamePictureInfo.inGroup = 1;
                    }
                    else
                    {
                        GetNamePictureInfo.inGroup = 0;
                    }
                }
            }
            else
            {
                if (para.type == 3)
                {
                    var member_group = _JointOfficeContext.Member_Group.Where(t => t.Id == para.groupId).FirstOrDefault();
                    if (member_group != null)
                    {
                        if (member_group.GroupPersonId.Contains(memberid))
                        {
                            GetNamePictureInfo.inGroup = 1;
                        }
                        else
                        {
                            GetNamePictureInfo.inGroup = 0;
                        }
                    }
                }
                var member_info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberId).FirstOrDefault();
                GetNamePictureInfo.name = member_info.Name;
                GetNamePictureInfo.picture = member_info.Picture + SasKey;
            }
            if (para.state)
            {
                var id = para.groupId;
                if (para.type == 1)
                {
                    List<string> list1 = new List<string>();
                    list1.Add(para.memberId);
                    list1.Add(memberid);
                    list1.Sort();
                    var qunzuid = "";
                    foreach (var item in list1)
                    {
                        qunzuid = qunzuid + item;
                    }
                    id = qunzuid;
                }

                var NewsMember = _JointOfficeContext.News_Member.Where(t => t.GroupId == id).FirstOrDefault();
                if (NewsMember != null)
                {
                    var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeiDuInfo>>(NewsMember.WeiDuGroupPersonId);
                    var one = objectList.Where(t => t.memberId == memberid).FirstOrDefault();
                    if (one!=null)
                    {
                        count = one.count;
                        one.count = 0;
                    }

                    var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(objectList);
                    NewsMember.WeiDuGroupPersonId = weiduInfo;
                    _JointOfficeContext.SaveChanges();
                }
            }

            res.showapi_res_body = GetNamePictureInfo;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="群名称,群组人员ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge NewCreateGroup(CreateGroupPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            //memberid = "2de62cc3-cc32-48b8-9511-cee872f79534";
            Message Message = new Message();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var PeopleList = new List<People>();
            foreach (var item in para.memberidlist)
            {
                var People = new People();
                People.memberid = item;
                PeopleList.Add(People);
            }
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var guid = Guid.NewGuid().ToString();
            var sql = @" insert  into ofGroup(groupName,description ) values('" + guid + "',N'" + info.Name + "创建群组[" + para.name + "]') ;" +
                " insert into ofGroupProp (groupName,name,propValue) values('" + guid + "','sharedRoster.displayName','" + guid + "');" +
                "insert into ofGroupProp (groupName,name,propValue) values('" + guid + "','sharedRoster.groupList','');" +
                " insert into ofGroupProp (groupName,name,propValue) values('" + guid + "','sharedRoster.showInRoster','onlyGroup');" +
                " insert into ofGroupUser (groupName,username,administrator) values('" + guid + "','" + memberid + "','0');";
            foreach (var item in para.memberidlist)
            {
                if (item != memberid)
                {
                    sql += "insert into ofGroupUser (groupName,username,administrator) values('" + guid + "','" + item + "','0');";
                }
            }

            using (SqlConnection conText = new SqlConnection(ImConnection))
            {
                conText.Open();
                SqlTransaction transaction;
                using (SqlCommand cmd = conText.CreateCommand())
                {
                    //启动事务
                    transaction = conText.BeginTransaction();
                    cmd.Connection = conText;
                    cmd.Transaction = transaction;
                    try
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        //完成提交
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //数据回滚
                        transaction.Rollback();
                        throw new BusinessTureException(ex.Message);
                    }
                }
            }

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
            Member_Group Member_Group = new Member_Group();
            Member_Group.Id = guid;
            Member_Group.MemberId = memberid;
            Member_Group.GroupKingMemberId = memberid;
            Member_Group.GroupPersonId = str;
            Member_Group.Name = para.name;
            Member_Group.State = 1;
            Member_Group.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/avatar_group_chat.png";
            Member_Group.CreateDate = DateTime.Now;
            _JointOfficeContext.Member_Group.Add(Member_Group);



            News_Member News_Member = new News_Member();
            News_Member.Id = Guid.NewGuid().ToString();
            News_Member.MemberId = memberid;
            News_Member.GroupId = guid;
            News_Member.GroupPersonId = str;
            News_Member.DeleteGroupPersonId = "";
            News_Member.CreateDate = DateTime.Now;
            List<WeiDuInfo> list = new List<WeiDuInfo>();
            foreach (var item in PeopleList)
            {
                WeiDuInfo WeiDuInfo = new WeiDuInfo();
                WeiDuInfo.memberId = item.memberid;
                WeiDuInfo.count = 0;
                list.Add(WeiDuInfo);
            }
            var weiduInfo = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            News_Member.WeiDuGroupPersonId = weiduInfo;
            _JointOfficeContext.News_Member.Add(News_Member);

            //News_News News_News = new News_News();
            //News_News.Id = Guid.NewGuid().ToString();
            //News_News.Body = info.Name + "创建群组";
            //News_News.NewsSenderId = memberid;
            //News_News.Type = 2;
            //News_News.InfoType = "7";
            //TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            //News_News.Time = ts.TotalMilliseconds.ToString();
            //News_News.GroupId = guid;
            //News_News.NoSeePerson = "";
            //News_News.CreateTime = DateTime.Now;
            //_JointOfficeContext.News_News.Add(News_News);


            _JointOfficeContext.SaveChanges();
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_error = "创建成功";
            res.showapi_res_code = "200";
            ReturnMessage mes = new ReturnMessage();
            mes.Oprationflag = true;
            mes.Message = "创建成功";
            mes.memberid = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/avatar_group_chat.png" + SasKey;
            mes.token = guid;
            res.showapi_res_body = mes;
            return res;
            //return Message.SuccessMeaasgeCode("创建成功", id);
        }
        /// <summary>
        /// 分公司显示各种提醒的数量
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GetNewsRemindNumListCompany> GetNewsRemindNumListCompany(GetNewsRemindNumListCompanyInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GetNewsRemindNumListCompany>();
                return Return.Return();
            }
            Showapi_Res_List<GetNewsRemindNumListCompany> res = new Showapi_Res_List<GetNewsRemindNumListCompany>();
            List<GetNewsRemindNumListCompany> list = new List<GetNewsRemindNumListCompany>();
            var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (!string.IsNullOrEmpty(memInfo.CompanyIDS))
            {
                var comList = memInfo.CompanyIDS.Split(",");
                foreach (var item in comList)
                {
                    GetNewsRemindNumListCompany GetNewsRemindNumListCompany = new GetNewsRemindNumListCompany();
                    var com = _JointOfficeContext.Member_Company.Where(t => t.Id == item).FirstOrDefault();
                    GetNewsRemindNumListCompany.companyId = item;
                    GetNewsRemindNumListCompany.companyName = "";
                    if (com != null)
                    {
                        GetNewsRemindNumListCompany.companyName = com.Name;
                    }
                    var num1 = 0;
                    var num2 = 0;
                    if (para.type == "1")
                    {
                        //审批待处理
                        var sql = @"select count(*) from Approval_Content a
                                    inner join Work_Approval b on b.Id=a.UId
                                    where a.OtherMemberId='" + memberid + @"' 
                                    and a.State=0 and (b.CompanyId='" + item + @"' or b.CompanyId is null or b.CompanyId='')";
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            num1 = conText.Query<int>(sql).FirstOrDefault();
                        }
                    }
                    if (para.type == "2")
                    {
                        //日志待点评
                        var workLog = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0 && (t.CompanyId == item || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                        num1 = workLog.Count();
                    }
                    if (para.type == "3")
                    {
                        //我执行的任务
                        var sql = @"select count(*) from Execute_Content a
                                    inner join Work_Task b on b.Id=a.UId
                                    where a.OtherMemberId='" + memberid + @"' 
                                    and a.Type=3 and a.State=0 and (b.CompanyId='" + item + @"' or b.CompanyId is null or b.CompanyId='')";
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            num1 = conText.Query<int>(sql).FirstOrDefault();
                        }
                    }
                    if (para.type == "4")
                    {
                        //日程未开始
                        var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                        var workProgram = _JointOfficeContext.Work_Program.Where(t => (t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(memberInfo.ZhuBuMen)) && t.State == 0 && (t.CompanyId == item || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                        num1 = workProgram.Count();
                    }
                    if (para.type == "5")
                    {
                        //指令待处理
                        var sql = @"select count(*) from Execute_Content a
                                    inner join Work_Order b on b.Id=a.UId
                                    where a.OtherMemberId='" + memberid + @"' 
                                    and a.Type=5 and a.State=0 and (b.CompanyId='" + item + @"' or b.CompanyId is null or b.CompanyId='')";
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            num1 = conText.Query<int>(sql).FirstOrDefault();
                        }
                    }
                    if (para.type == "21")
                    {
                        //回复我的  未读
                        var sql = @"exec ReplyMeIsReadCompany '" + memberid + "','" + item + "'";
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            var idList = conText.Query<ReplyMeIsRead>(sql).Where(t => t.isRead == false).ToList();
                            num1 = idList.Count();
                        }
                        //我收到的赞  未读
                        var sql1 = @"select a.* from Agree a
                                    left join Work_Approval b on b.Id=a.UId
                                    left join Work_Log c on c.Id=a.UId
                                    left join Work_Task d on d.Id=a.UId
                                    left join Work_Program e on e.Id=a.UId
                                    left join Work_Order f on f.Id=a.UId
                                    left join Work_Announcement g on g.Id=a.UId
                                    left join Work_Share h on h.Id=a.UId
                                    where a.OtherMemberId='" + memberid + @"' and
                                    (b.CompanyId='" + item + @"' or b.CompanyId is null or b.CompanyId='') and
                                    (c.CompanyId='" + item + @"' or c.CompanyId is null or c.CompanyId='') and
                                    (d.CompanyId='" + item + @"' or d.CompanyId is null or d.CompanyId='') and
                                    (e.CompanyId='" + item + @"' or e.CompanyId is null or e.CompanyId='') and
                                    (f.CompanyId='" + item + @"' or f.CompanyId is null or f.CompanyId='') and
                                    (g.CompanyId='" + item + @"' or g.CompanyId is null or g.CompanyId='') and
                                    (h.CompanyId='" + item + @"' or h.CompanyId is null or h.CompanyId='')";
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            var qqq = conText.Query<Agree>(sql1).Where(t => t.IsRead == false).ToList();
                            num2 = qqq.Count();
                        }
                    }
                    if (para.type == "22")
                    {
                        //我关注的
                        //var sql = @"select count(*) from News_Focus a
                        //            left join Work_Approval b on b.Id=a.UId
                        //            left join Work_Log c on c.Id=a.UId
                        //            left join Work_Task d on d.Id=a.UId
                        //            left join Work_Program e on e.Id=a.UId
                        //            left join Work_Order f on f.Id=a.UId
                        //            left join Work_Announcement g on g.Id=a.UId
                        //            left join Work_Share h on h.Id=a.UId
                        //            where a.MemberId='" + memberid + @"' and
                        //            (b.CompanyId='" + item + @"' or b.CompanyId is null or b.CompanyId='') and
                        //            (c.CompanyId='" + item + @"' or c.CompanyId is null or c.CompanyId='') and
                        //            (d.CompanyId='" + item + @"' or d.CompanyId is null or d.CompanyId='') and
                        //            (e.CompanyId='" + item + @"' or e.CompanyId is null or e.CompanyId='') and
                        //            (f.CompanyId='" + item + @"' or f.CompanyId is null or f.CompanyId='') and
                        //            (g.CompanyId='" + item + @"' or g.CompanyId is null or g.CompanyId='') and
                        //            (h.CompanyId='" + item + @"' or h.CompanyId is null or h.CompanyId='')";
                        //using (SqlConnection conText = new SqlConnection(constr))
                        //{
                        //    num1 = conText.Query<int>(sql).FirstOrDefault();
                        //}
                        num1 = 0;
                    }
                    GetNewsRemindNumListCompany.num1 = num1;
                    GetNewsRemindNumListCompany.num2 = num2;
                    list.Add(GetNewsRemindNumListCompany);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GetNewsRemindNumListCompany>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取当前用户某聊天未读数量
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GetPersonNewsNotReadNum> GetPersonNewsNotReadNum(GetPersonNewsNotReadNumInPara para)
        {
            var member = _JointOfficeContext.Member_Token.Where(t => t.Token == para.token && t.Type == "web" && t.Effective == 1).FirstOrDefault();
            if (member == null)
            {
                var Return = new ReturnList<GetPersonNewsNotReadNum>();
                return Return.Return();
            }
            var memberid = member.MemberId;
            //var memberid = _PrincipalBase.GetMemberId();
            //if (memberid == null || memberid == "")
            //{
            //    var Return = new ReturnList<GetPersonNewsNotReadNum>();
            //    return Return.Return();
            //}
            Showapi_Res_List<GetPersonNewsNotReadNum> res = new Showapi_Res_List<GetPersonNewsNotReadNum>();
            List<GetPersonNewsNotReadNum> list = new List<GetPersonNewsNotReadNum>();
            var newsmems = _JointOfficeContext.News_Member.Where(t => t.WeiDuGroupPersonId.Contains(memberid)).ToList();
            foreach (var item in newsmems)
            {
                var strList = JsonConvert.DeserializeObject<List<WeiDuInfo>>(item.WeiDuGroupPersonId);
                if (strList != null && strList.Count != 0)
                {
                    var one = strList.Where(t => t.memberId == memberid).FirstOrDefault();
                    if (one != null && one.count != 0)
                    {
                        GetPersonNewsNotReadNum GetPersonNewsNotReadNum = new GetPersonNewsNotReadNum();
                        GetPersonNewsNotReadNum.id = item.GroupId;
                        GetPersonNewsNotReadNum.name = "";
                        GetPersonNewsNotReadNum.picture = "";
                        var group = _JointOfficeContext.Member_Group.Where(t => t.Id == item.GroupId).FirstOrDefault();
                        if (group != null)
                        {
                            GetPersonNewsNotReadNum.name = group.Name;
                            GetPersonNewsNotReadNum.picture = group.Picture + SasKey;
                        }
                        GetPersonNewsNotReadNum.num = one.count;
                        if (item.GroupId.Length > 36)
                        {
                            GetPersonNewsNotReadNum.newsType = 1;
                        }
                        else
                        {
                            GetPersonNewsNotReadNum.newsType = 2;
                        }
                        list.Add(GetPersonNewsNotReadNum);
                    }
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GetPersonNewsNotReadNum>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取当前用户全部聊天未读数量
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<GetPersonNewsNotReadAllNum> GetPersonNewsNotReadAllNum()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<GetPersonNewsNotReadAllNum>();
                return Return.Return();
            }
            Showapi_Res_Single<GetPersonNewsNotReadAllNum> res = new Showapi_Res_Single<GetPersonNewsNotReadAllNum>();
            GetPersonNewsNotReadAllNum GetPersonNewsNotReadAllNum = new GetPersonNewsNotReadAllNum();
            GetPersonNewsNotReadAllNum.num = 0;
            var newsmems = _JointOfficeContext.News_Member.Where(t => t.WeiDuGroupPersonId.Contains(memberid)).ToList();
            foreach (var item in newsmems)
            {
                var strList = JsonConvert.DeserializeObject<List<WeiDuInfo>>(item.WeiDuGroupPersonId);
                if (strList != null && strList.Count != 0)
                {
                    var one = strList.Where(t => t.memberId == memberid).FirstOrDefault();
                    if (one != null && one.count != 0)
                    {
                        GetPersonNewsNotReadAllNum.num += one.count;
                    }
                }
            }
            res.showapi_res_body = GetPersonNewsNotReadAllNum;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 各种提醒的数量  web
        /// </summary>
        public Showapi_Res_List<NewsRemindNum> GetNewsRemindNumListCompanyWeb(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<NewsRemindNum>();
                return Return.Return();
            }
            Showapi_Res_List<NewsRemindNum> res = new Showapi_Res_List<NewsRemindNum>();
            List<NewsRemindNum> list = new List<NewsRemindNum>();
            string[] array = new string[] { "1", "2", "3", "4", "5" };
            //审批待处理
            var num1 = 0;
            var sql1 = @"select count(*) from Approval_Content a
                        inner join Work_Approval b on b.Id=a.UId
                        where a.OtherMemberId='" + memberid + @"' 
                        and a.State=0 and (b.CompanyId='" + para.id + @"' or b.CompanyId is null or b.CompanyId='')";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num1 = conText.Query<int>(sql1).FirstOrDefault();
            }
            //日志待点评
            var workLog = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0 && (t.CompanyId == para.id || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
            var num2 = workLog.Count();
            //我执行的任务
            var num3 = 0;
            var sql3 = @"select count(*) from Execute_Content a
                        inner join Work_Task b on b.Id=a.UId
                        where a.OtherMemberId='" + memberid + @"' 
                        and a.Type=3 and a.State=0 and (b.CompanyId='" + para.id + @"' or b.CompanyId is null or b.CompanyId='')";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num3 = conText.Query<int>(sql3).FirstOrDefault();
            }
            //日程未开始
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var workProgram = _JointOfficeContext.Work_Program.Where(t => (t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(memberInfo.ZhuBuMen)) && t.State == 0 && (t.CompanyId == para.id || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
            var num4 = workProgram.Count();
            //指令待处理
            var num5 = 0;
            var sql5 = @"select count(*) from Execute_Content a
                        inner join Work_Order b on b.Id=a.UId
                        where a.OtherMemberId='" + memberid + @"' 
                        and a.Type=5 and a.State=0 and (b.CompanyId='" + para.id + @"' or b.CompanyId is null or b.CompanyId='')";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num5 = conText.Query<int>(sql5).FirstOrDefault();
            }
            for (int i = 0; i < array.Length; i++)
            {
                NewsRemindNum NewsRemindNum = new NewsRemindNum();
                switch (array[i])
                {
                    case "1":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num1;
                        break;
                    case "2":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num2;
                        break;
                    case "3":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num3;
                        break;
                    case "4":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num4;
                        break;
                    case "5":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num5;
                        break;
                }
                list.Add(NewsRemindNum);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<NewsRemindNum>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
    }
}
