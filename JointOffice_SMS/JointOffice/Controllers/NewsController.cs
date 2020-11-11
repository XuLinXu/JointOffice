using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure; 
using Microsoft.Extensions.Options;
using JointOffice.Models;
using System.IO;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using JointOffice.Core;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        INews _INews;
        ExceptionMessage em;
        IOptions<Root> config;
        string SasKey;
        public NewsController(IOptions<Root> config, INews INews, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _INews = INews;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="群名称,群组人员ID"></param>
        /// <returns></returns>
        [HttpPost("CreateGroup")]
        public Showapi_Res_Meaasge CreateGroup([FromBody]CreateGroupPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.memberidlist.Count().ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.CreateGroup(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改群组人员
        /// </summary>
        /// <param name="群组ID,群组类型,群组人员ID"></param>
        /// <returns></returns>
        [HttpPost("UpdateGroup")]
        public Showapi_Res_Meaasge UpdateGroup([FromBody]UpdateGroupPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.groupid) || string.IsNullOrEmpty(para.memberidlist.Count().ToString()) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.UpdateGroup(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改群头像
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateGroupPicture")]
        public Showapi_Res_Meaasge UpdateGroupPicture()
        {
            try
            {
                UpdateGroupPicturePara para = new UpdateGroupPicturePara();

                //if (string.IsNullOrEmpty(para.name) || string.IsNullOrEmpty(para.id))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var id = string.IsNullOrEmpty(Request.Form["id"]) ? "" : Request.Form["id"].ToString();
                //var name = string.IsNullOrEmpty(Request.Form["name"]) ? "" : Request.Form["name"].ToString();

                para.id = id;
                //para.name = name;

                var files = Request.Form.Files;
                if (files != null && files.Count != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();

                    if (Picture.Count != 0)
                    {
                        //List<DaiDianPingDeRiZhi_url> list2 = new List<DaiDianPingDeRiZhi_url>();
                        //foreach (var item in Picture)
                        //{
                        //    DaiDianPingDeRiZhi_url DaiDianPingDeRiZhi_url = new DaiDianPingDeRiZhi_url();
                        //    DaiDianPingDeRiZhi_url.url = item.fileurl;
                        //    list2.Add(DaiDianPingDeRiZhi_url);
                        //}

                        para.picture = Picture.FirstOrDefault().fileurl;
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _INews.UpdateGroupPicture(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateGroupName")]
        public Showapi_Res_Meaasge UpdateGroupName([FromBody]UpdateGroupNamePara para)
        {
            try
            {
                return _INews.UpdateGroupName(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 存储会话信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveConversation")]
        public Showapi_Res_Meaasge SaveConversation()
        {
            try
            {
                News_News News_News = new News_News();
                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var id = string.IsNullOrEmpty(Request.Form["id"]) ? "" : Request.Form["id"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var infoType = string.IsNullOrEmpty(Request.Form["infoType"]) ? "" : Request.Form["infoType"].ToString();
                var length = string.IsNullOrEmpty(Request.Form["length"]) ? "" : Request.Form["length"].ToString();
                var name = string.IsNullOrEmpty(Request.Form["name"]) ? "" : Request.Form["name"].ToString();
                var time = string.IsNullOrEmpty(Request.Form["time"]) ? "" : Request.Form["time"].ToString();
                var baseurl = string.IsNullOrEmpty(Request.Form["baseurl"]) ? "" : Request.Form["baseurl"].ToString();

                //var travelAll = string.IsNullOrEmpty(Request.Form["travelAll"]) ? "" : Request.Form["travelAll"].ToString();

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(infoType))
                {
                    throw new BusinessException("参数不正确。");
                }

                if (infoType!="1")
                {
                    var files = Request.Form.Files;
                    if (files.Count() != 0)
                    {
                        var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                        CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                        var Picture = blobFiles.FirstOrDefault();
                        body = Picture.fileurl;
                        //length = BusinessHelper.ConvertBytes(Picture.filelength);
                        name = Picture.fileYName;
                    }
                }
                else
                {
                    length = "0";
                    name = "";
                }

                News_News.Time = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString(); ;
                if (body.Contains(SasKey))
                {
                    body = body.Replace(SasKey, "");
                }
                News_News.Body = body;
                News_News.Map = map;
                News_News.Address = address;
                News_News.InfoType = infoType;
                News_News.Type = Convert.ToInt32(type);
                News_News.GroupId = id;
                News_News.BaseUrl = baseurl;
                News_News.FileName = name;
                News_News.Length = length;
                return _INews.SaveConversation(News_News);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除会话消息
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteConversation")]
        public Showapi_Res_Meaasge DeleteConversation([FromBody]DeleteConversationInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.DeleteConversation(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 清空会话消息
        /// </summary>
        /// <returns></returns>
        [HttpPost("CleanConversation")]
        public Showapi_Res_Meaasge CleanConversation([FromBody]CleanConversationInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.CleanConversation(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 显示   消息列表
        /// </summary>
        [HttpPost("GetConversationList")]
        public Showapi_Res_List<ConversationList> GetConversationList([FromBody]GetConversationList para)
        {
            Showapi_Res_List<ConversationList> res = new Showapi_Res_List<ConversationList>();
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetConversationList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 发群通知
        /// </summary>
        /// <returns></returns>
        [HttpPost("SendGroupNotice")]
        public Showapi_Res_Meaasge SendGroupNotice()
        {
            try
            {
                News_GroupNotice para = new News_GroupNotice();

                //if (string.IsNullOrEmpty(para.Range) || string.IsNullOrEmpty(para.Title) || string.IsNullOrEmpty(para.Body) || string.IsNullOrEmpty(para.IsConfirm))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var range = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var title = string.IsNullOrEmpty(Request.Form["title"]) ? "" : Request.Form["title"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var isConfirm = string.IsNullOrEmpty(Request.Form["isConfirm"]) ? "" : Request.Form["isConfirm"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();

                para.Range = range;
                para.Title = title;
                para.Body = body;
                para.IsConfirm = isConfirm;
                para.AtPerson = _person;
                para.VoiceLength = voiceLength;
                para.PhoneModel = phoneModel;

                var files = Request.Form.Files;
                if (files != null && files.Count() != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Voice = blobFiles.Where(t => t.filetype == 2).FirstOrDefault();
                    var Annex = blobFiles.Where(t => t.filetype == 3).ToList();
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();

                    if (Voice != null)
                    {
                        para.Voice = Voice.fileurl;
                    }

                    if (Annex.Count != 0)
                    {
                        List<Work_File> list1 = new List<Work_File>();
                        foreach (var item in Annex)
                        {
                            Work_File Work_File = new Work_File();
                            Work_File.url = item.fileurl;
                            Work_File.length = item.filelength;
                            Work_File.name = item.fileYName;
                            Work_File.fileType = item.annexfiletype.ToString();
                            list1.Add(Work_File);
                        }
                        para.Annex = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                    }

                    if (Picture.Count != 0)
                    {
                        List<DaiDianPingDeRiZhi_url> list2 = new List<DaiDianPingDeRiZhi_url>();
                        foreach (var item in Picture)
                        {
                            DaiDianPingDeRiZhi_url DaiDianPingDeRiZhi_url = new DaiDianPingDeRiZhi_url();
                            DaiDianPingDeRiZhi_url.url = item.fileurl;
                            list2.Add(DaiDianPingDeRiZhi_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }

                return _INews.SendGroupNotice(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除群通知
        /// </summary>
        [HttpPost("DeleteGroupNotice")]
        public Showapi_Res_Meaasge DeleteGroupNotice([FromBody]DeleteGroupNoticeInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.DeleteGroupNotice(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 确认读取群通知
        /// </summary>
        /// <param name="群通知ID"></param>
        /// <returns></returns>
        [HttpPost("ConfirmReadGroupNotice")]
        public Showapi_Res_Meaasge ConfirmReadGroupNotice([FromBody]ConfirmReadGroupNoticeInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.mark))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.ConfirmReadGroupNotice(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 群通知列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetGroupNoticeList")]
        public Showapi_Res_List<GroupNoticeList> GetGroupNoticeList([FromBody]GroupNoticeListInPara para)
        {
            Showapi_Res_List<GroupNoticeList> res = new Showapi_Res_List<GroupNoticeList>();
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetGroupNoticeList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 群通知详情
        /// </summary>
        [HttpPost("GetGroupNoticeBodyList")]
        public Showapi_Res_Single<GroupNoticeBody> GetGroupNoticeBodyList([FromBody]GroupNoticeBodyInPara para)
        {
            Showapi_Res_Single<GroupNoticeBody> res = new Showapi_Res_Single<GroupNoticeBody>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetGroupNoticeBodyList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 群通知读取/确认人数
        /// </summary>
        [HttpPost("GetGroupNoticeConfirmNumList")]
        public Showapi_Res_List<GroupNoticeConfirmNum> GetGroupNoticeConfirmNumList([FromBody]GroupNoticeBodyInPara para)
        {
            Showapi_Res_List<GroupNoticeConfirmNum> res = new Showapi_Res_List<GroupNoticeConfirmNum>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetGroupNoticeConfirmNumList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 收藏
        /// </summary>
        [HttpPost("Collection")]
        public Showapi_Res_Meaasge Collection([FromBody]CollectionInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.otherMemberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.Collection(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改收藏标签
        /// </summary>
        [HttpPost("UpdateCollection")]
        public Showapi_Res_Meaasge UpdateCollection([FromBody]UpdateCollectionInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.addMarkInfo.Count().ToString()) || string.IsNullOrEmpty(para.deleteMarkInfo.Count().ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.UpdateCollection(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 收藏列表
        /// </summary>
        [HttpPost("GetCollectionList")]
        public Showapi_Res_List<CollectionList> GetCollectionList()
        {
            Showapi_Res_List<CollectionList> res = new Showapi_Res_List<CollectionList>();
            try
            {
                return _INews.GetCollectionList();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 我的收藏 Web
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMyCollectionList")]
        public Showapi_Res_List<PersonDynamic_info> GetMyCollectionList([FromBody]GetMyFocusInPara para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetMyCollectionList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("NoCollection")]
        public Showapi_Res_Meaasge NoCollection([FromBody]NoCollectionInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.NoCollection(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        [HttpPost("InsertMark")]
        public Showapi_Res_Meaasge InsertMark([FromBody]InsertMarkInPara para)
        {
            try
            {
                return _INews.InsertMark(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="ID，name"></param>
        /// <returns></returns>
        [HttpPost("UpdateMark")]
        public Showapi_Res_Meaasge UpdateMark([FromBody]UpdateMarkInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.name))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.UpdateMark(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteMark")]
        public Showapi_Res_Meaasge DeleteMark([FromBody]DeleteMarkInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.DeleteMark(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 标签列表
        /// </summary>
        [HttpPost("GetMarkList")]
        public Showapi_Res_List<MarkList> GetMarkList()
        {
            Showapi_Res_List<MarkList> res = new Showapi_Res_List<MarkList>();
            try
            {
                return _INews.GetMarkList();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="类型,ID"></param>
        /// <returns></returns>
        [HttpPost("Focus")]
        public Showapi_Res_Meaasge Focus([FromBody]FocusInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.Focus(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("NoFocus")]
        public Showapi_Res_Meaasge NoFocus([FromBody]NoFocusInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.NoFocus(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 我关注的
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMyFocusList")]
        public Showapi_Res_List<PersonDynamic_info> GetMyFocusList([FromBody]GetMyFocusInPara para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetMyFocusList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// @我的回复
        /// </summary>
        [HttpPost("GetMyReplyList")]
        public Showapi_Res_List<WorkReply> GetMyReplyList([FromBody]GetATMyInPara para)
        {
            Showapi_Res_List<WorkReply> res = new Showapi_Res_List<WorkReply>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetMyReplyList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 工作回复  Web
        /// </summary>
        [HttpPost("GetWorkReplyWebList")]
        public Showapi_Res_List<WorkReply> GetWorkReplyWebList([FromBody]AllSearch para)
        {
            Showapi_Res_List<WorkReply> res = new Showapi_Res_List<WorkReply>();
            try
            {
                return _INews.GetWorkReplyWebList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// @我的工作
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetATMyWorkList")]
        public Showapi_Res_List<PersonDynamic_info> GetATMyWorkList([FromBody]GetATMyInPara para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetATMyWorkList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 我收到的赞
        /// </summary>
        [HttpPost("GetMyZanList")]
        public Showapi_Res_List<MyZan> GetMyZanList([FromBody]MyZanInPara para)
        {
            Showapi_Res_List<MyZan> res = new Showapi_Res_List<MyZan>();
            try
            {
                return _INews.GetMyZanList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 我的赞  收到+发出
        /// </summary>
        [HttpPost("GetMyZanWebList")]
        public Showapi_Res_List<MyZan> GetMyZanWebList([FromBody]AllSearch para)
        {
            Showapi_Res_List<MyZan> res = new Showapi_Res_List<MyZan>();
            try
            {
                return _INews.GetMyZanWebList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 我的回执
        /// </summary>
        [HttpPost("GetMyReceiptList")]
        public Showapi_Res_List<PersonDynamic_info> GetMyReceiptList([FromBody]AllSearch para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                return _INews.GetMyReceiptList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 各种提醒的数量
        /// </summary>
        [HttpPost("GetNewsRemindNumList")]
        public Showapi_Res_List<NewsRemindNum> GetNewsRemindNumList()
        {
            Showapi_Res_List<NewsRemindNum> res = new Showapi_Res_List<NewsRemindNum>();
            try
            {
                return _INews.GetNewsRemindNumList();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 审批提醒
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetApprovalRemindList")]
        public Showapi_Res_List<DaiShenPi> GetApprovalRemindList([FromBody]ApprovalRemindInPara para)
        {
            Showapi_Res_List<DaiShenPi> res = new Showapi_Res_List<DaiShenPi>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetApprovalRemindList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 日志提醒
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        [HttpPost("GetLogRemindList")]
        public Showapi_Res_List<DaiDianPingDeRiZhi> GetLogRemindList([FromBody]LogRemindInPara para)
        {
            Showapi_Res_List<DaiDianPingDeRiZhi> res = new Showapi_Res_List<DaiDianPingDeRiZhi>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetLogRemindList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 我执行的任务
        /// </summary>
        [HttpPost("GetIDoTaskList")]
        public Showapi_Res_List<DaiZhiXingDeRenWu> GetIDoTaskList([FromBody]IDoTaskInPara para)
        {
            Showapi_Res_List<DaiZhiXingDeRenWu> res = new Showapi_Res_List<DaiZhiXingDeRenWu>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetIDoTaskList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 我发出的任务
        /// </summary>
        [HttpPost("GetIPublishTaskList")]
        public Showapi_Res_List<DaiZhiXingDeRenWu> GetIPublishTaskList([FromBody]IPublishTaskInPara para)
        {
            Showapi_Res_List<DaiZhiXingDeRenWu> res = new Showapi_Res_List<DaiZhiXingDeRenWu>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetIPublishTaskList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 日程列表
        /// </summary>
        /// <param name="年月"></param>
        /// <returns></returns>
        [HttpPost("GetProgramList")]
        public Showapi_Res_Single<ProgramList> GetProgramList([FromBody]ProgramListInPara para)
        {
            Showapi_Res_Single<ProgramList> res = new Showapi_Res_Single<ProgramList>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetProgramList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 日程通知
        /// </summary>
        /// <param name="日程状态"></param>
        /// <returns></returns>
        [HttpPost("GetProgramNoticeList")]
        public Showapi_Res_List<RiChengDetail> GetProgramNoticeList([FromBody]ProgramNoticeInPara para)
        {
            Showapi_Res_List<RiChengDetail> res = new Showapi_Res_List<RiChengDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetProgramNoticeList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 指令提醒
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        [HttpPost("GetOrderRemindList")]
        public Showapi_Res_List<ZhiLingDetail> GetOrderRemindList([FromBody]OrderRemindInPara para)
        {
            Showapi_Res_List<ZhiLingDetail> res = new Showapi_Res_List<ZhiLingDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetOrderRemindList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 创建人取消(审批,任务，指令)
        /// </summary>
        [HttpPost("Cancel")]
        public Showapi_Res_Meaasge Cancel([FromBody]CancelInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.phoneModel))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.Cancel(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除(1审批 2日志 3任务 4日程 5指令)
        /// </summary>
        [HttpPost("Delete")]
        public Showapi_Res_Meaasge Delete([FromBody]DeleteInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.Delete(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 群文档
        /// </summary>
        [HttpPost("GetGroupDocumentList")]
        public Showapi_Res_List<GroupDocument> GetGroupDocumentList([FromBody]GroupDocumentInPara para)
        {
            Showapi_Res_List<GroupDocument> res = new Showapi_Res_List<GroupDocument>();
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetGroupDocumentList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 全图片
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllPictureList")]
        public Showapi_Res_List<AllPicture> GetAllPictureList([FromBody]GroupDocumentInPara para)
        {
            Showapi_Res_List<AllPicture> res = new Showapi_Res_List<AllPicture>();
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetAllPictureList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetNewsList")]
        public Showapi_Res_List<NewsList> GetNewsList([FromBody]GetNewsListPara para)
        {
            Showapi_Res_List<NewsList> res = new Showapi_Res_List<NewsList>();
            try
            {
                return _INews.GetNewsList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 群组信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetNewsInfo")]
        public Showapi_Res_Single<NewsList> GetNewsInfo([FromBody]CleanConversationInPara para)
        {
            Showapi_Res_Single<NewsList> res = new Showapi_Res_Single<NewsList>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetNewsInfo(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取聊天信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetNewsInfoList")]
        public Showapi_Res_List<NewsInfoList> GetNewsInfoList([FromBody]GetNewsInfoListPara para)
        {
            Showapi_Res_List<NewsInfoList> res = new Showapi_Res_List<NewsInfoList>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetNewsInfoList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 删除会话窗口
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteNewsList")]
        public Showapi_Res_Meaasge DeleteNewsList([FromBody]DeleteNewsListPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.DeleteNewsList(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取人员和权限信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetNewsPersonInfo")]
        public Showapi_Res_Single<GetNewsPersonInfoList> GetNewsPersonInfo([FromBody]CleanConversationInPara para)
        {
            Showapi_Res_Single<GetNewsPersonInfoList> res = new Showapi_Res_Single<GetNewsPersonInfoList>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.GetNewsPersonInfo(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        [HttpPost("DissolutionGroup")]
        public Showapi_Res_Meaasge DissolutionGroup([FromBody]CleanConversationInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.DissolutionGroup(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 搜索聊天记录
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("SearchNewsInfoList")]
        public Showapi_Res_List<SearchNewsInfoList> SearchNewsInfoList([FromBody]SearchNewsInfoListPara para)
        {
            Showapi_Res_List<SearchNewsInfoList> res = new Showapi_Res_List<SearchNewsInfoList>();
            try
            {
                if (string.IsNullOrEmpty(para.id)|| string.IsNullOrEmpty(para.body))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.SearchNewsInfoList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 根据memberid获取头像名称
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetNamePicture")]
        public Showapi_Res_Single<GetNamePictureInfo> GetNamePicture([FromBody]GetNamePicturePara para)
        {
            Showapi_Res_Single<GetNamePictureInfo> res = new Showapi_Res_Single<GetNamePictureInfo>();
            try
            {
                return _INews.GetNamePicture(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 聊天上传图片
        /// </summary>
        /// <param name="群名称,群组人员ID"></param>
        /// <returns></returns>
        [HttpPost("ShangChuanImage")]
        public Showapi_Res_Meaasge ShangChuanImage()
        {
            try
            {
                Showapi_Res_Meaasge Showapi_Res_Meaasge = new Showapi_Res_Meaasge();
                var files = Request.Form.Files;
                if (files != null && files.Count != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Picture = blobFiles.FirstOrDefault();

                    if (Picture != null)
                    {
                        Showapi_Res_Meaasge.showapi_res_body = new ReturnMessage();
                        Showapi_Res_Meaasge.showapi_res_body.memberid = Picture.fileurl + SasKey;
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                    Showapi_Res_Meaasge.showapi_res_code = "200";
                    Showapi_Res_Meaasge.showapi_res_body.Oprationflag = true;
                    return Showapi_Res_Meaasge;
                }
                else
                {
                    throw new BusinessException("请上传文件。");
                }
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="群名称,群组人员ID"></param>
        /// <returns></returns>
        [HttpPost("NewCreateGroup")]
        public Showapi_Res_Meaasge NewCreateGroup([FromBody]CreateGroupPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.memberidlist.Count().ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _INews.NewCreateGroup(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 分公司显示各种提醒的数量
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetNewsRemindNumListCompany")]
        public Showapi_Res_List<GetNewsRemindNumListCompany> GetNewsRemindNumListCompany([FromBody]GetNewsRemindNumListCompanyInPara para)
        {
            Showapi_Res_List<GetNewsRemindNumListCompany> res = new Showapi_Res_List<GetNewsRemindNumListCompany>();
            try
            {
                return _INews.GetNewsRemindNumListCompany(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取当前用户某聊天未读数量
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPersonNewsNotReadNum")]
        public Showapi_Res_List<GetPersonNewsNotReadNum> GetPersonNewsNotReadNum([FromBody]GetPersonNewsNotReadNumInPara para)
        {
            Showapi_Res_List<GetPersonNewsNotReadNum> res = new Showapi_Res_List<GetPersonNewsNotReadNum>();
            try
            {
                return _INews.GetPersonNewsNotReadNum(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取当前用户全部聊天未读数量
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPersonNewsNotReadAllNum")]
        public Showapi_Res_Single<GetPersonNewsNotReadAllNum> GetPersonNewsNotReadAllNum()
        {
            Showapi_Res_Single<GetPersonNewsNotReadAllNum> res = new Showapi_Res_Single<GetPersonNewsNotReadAllNum>();
            try
            {
                return _INews.GetPersonNewsNotReadAllNum();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 各种提醒的数量  web
        /// </summary>
        [HttpPost("GetNewsRemindNumListCompanyWeb")]
        public Showapi_Res_List<NewsRemindNum> GetNewsRemindNumListCompanyWeb([FromBody]UpdateMember_info para)
        {
            Showapi_Res_List<NewsRemindNum> res = new Showapi_Res_List<NewsRemindNum>();
            try
            {
                return _INews.GetNewsRemindNumListCompanyWeb(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
    }
}
