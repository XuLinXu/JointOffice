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
    public class WorkController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IWork _IWork;
        ExceptionMessage em;
        IOptions<Root> config;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public WorkController(IOptions<Root> config, IWork IWork, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            _IWork = IWork;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            em = new ExceptionMessage(_JointOfficeContext);
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 待审批
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDaiShenPiList")]
        public Showapi_Res_List<DaiShenPi> GetDaiShenPiList([FromBody]GetDaiDianPingDeRiZhiListPara para)
        {
            Showapi_Res_List<DaiShenPi> res = new Showapi_Res_List<DaiShenPi>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetDaiShenPiList(para);
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
        /// 待点评的日志
        /// </summary>
        /// <param name="页数，页大小"></param>
        /// <returns></returns>
        [HttpPost("GetDaiDianPingDeRiZhiList")]
        public Showapi_Res_List<DaiDianPingDeRiZhi> GetDaiDianPingDeRiZhiList([FromBody]GetDaiDianPingDeRiZhiListPara para)
        {
            Showapi_Res_List<DaiDianPingDeRiZhi> res = new Showapi_Res_List<DaiDianPingDeRiZhi>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetDaiDianPingDeRiZhiList(para);
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
        /// 待执行的任务
        /// </summary>
        /// <param name="页数，页大小"></param>
        /// <returns></returns>
        [HttpPost("GetDaiZhiXingDeRenWuList")]
        public Showapi_Res_List<DaiZhiXingDeRenWu> GetDaiZhiXingDeRenWuList([FromBody]GetDaiDianPingDeRiZhiListPara para)
        {
            Showapi_Res_List<DaiZhiXingDeRenWu> res = new Showapi_Res_List<DaiZhiXingDeRenWu>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetDaiZhiXingDeRenWuList(para);
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
        /// 待执行的指令
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetZhiLingDetailList")]
        public Showapi_Res_List<ZhiLingDetail> GetZhiLingDetailList([FromBody]GetDaiDianPingDeRiZhiListPara para)
        {
            Showapi_Res_List<ZhiLingDetail> res = new Showapi_Res_List<ZhiLingDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetZhiLingDetailList(para);
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
        /// 待回执的公告
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetGongGaoDetailList")]
        public Showapi_Res_List<GongGaoDetail> GetGongGaoDetailList([FromBody]GetDaiDianPingDeRiZhiListPara para)
        {
            Showapi_Res_List<GongGaoDetail> res = new Showapi_Res_List<GongGaoDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetGongGaoDetailList(para);
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
        /// 待回执的分享
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFenXiangDetailList")]
        public Showapi_Res_List<FenXiangDetail> GetFenXiangDetailList([FromBody]GetDaiDianPingDeRiZhiListPara para)
        {
            Showapi_Res_List<FenXiangDetail> res = new Showapi_Res_List<FenXiangDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetFenXiangDetailList(para);
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
        /// 个人动态主页
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPersonDynamic_infoList")]
        public Showapi_Res_List<PersonDynamic_info> GetPersonDynamic_infoList([FromBody]GetPersonDynamic_infoListPara para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetPersonDynamic_infoList(para);
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
        /// 部门信息中个人动态主页
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDept_PersonDynamic_infoList")]
        public Showapi_Res_List<PersonDynamic_info> GetDept_PersonDynamic_infoList([FromBody]GetPersonDynamic_infoListPara para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetDept_PersonDynamic_infoList(para);
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
        /// 工作回复  回复我的
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetWorkReplyList")]
        public Showapi_Res_List<WorkReply> GetWorkReplyList([FromBody]GetReplyListPara para)
        {
            Showapi_Res_List<WorkReply> res = new Showapi_Res_List<WorkReply>();
            try
            {
                if (string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetWorkReplyList(para);
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
        /// 创建审批
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkApproval")]
        public Showapi_Res_Meaasge CreateWorkApproval()
        {
            try
            {
                Work_Approval para = new Work_Approval();
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //if (string.IsNullOrEmpty(para.Type) || string.IsNullOrEmpty(para.Body) || string.IsNullOrEmpty(para.ApprovalPerson) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var approvalPerson = string.IsNullOrEmpty(Request.Form["approvalPerson"]) ? "" : Request.Form["approvalPerson"].ToString();
                var rangenew = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var workDuration = string.IsNullOrEmpty(Request.Form["workDuration"]) ? "" : Request.Form["workDuration"].ToString();
                var overTime = string.IsNullOrEmpty(Request.Form["overTime"]) ? "" : Request.Form["overTime"].ToString();
                var travelAll = string.IsNullOrEmpty(Request.Form["travelAll"]) ? "" : Request.Form["travelAll"].ToString();
                var travel = string.IsNullOrEmpty(Request.Form["travel"]) ? "" : Request.Form["travel"].ToString();
                var leave = string.IsNullOrEmpty(Request.Form["leave"]) ? "" : Request.Form["leave"].ToString();
                var travelReb = string.IsNullOrEmpty(Request.Form["travelReb"]) ? "" : Request.Form["travelReb"].ToString();
                var reb = string.IsNullOrEmpty(Request.Form["reb"]) ? "" : Request.Form["reb"].ToString();
                var leaveDuration = string.IsNullOrEmpty(Request.Form["leaveDuration"]) ? "" : Request.Form["leaveDuration"].ToString();
                var travelMoney = string.IsNullOrEmpty(Request.Form["travelMoney"]) ? "" : Request.Form["travelMoney"].ToString();
                var rebMoney = string.IsNullOrEmpty(Request.Form["rebMoney"]) ? "" : Request.Form["rebMoney"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.Type = type;
                para.Body = body;
                para.ApprovalPerson = approvalPerson;

                var rangelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangenew);
                List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
                foreach (var item in rangelist)
                {
                    if (item.type == "2")
                    {
                        rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                    }
                    if (item.type == "1")
                    {
                        var checkRange = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                        if (!checkRange.Contains(item.id))
                        {
                            rangelistnew.Add(item);
                        }
                    }
                }
                var rangestrnew = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                para.Range = rangestrnew;
                para.RangeNew = rangenew;

                para.Map = map;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }
                para.WorkDuration = workDuration;
                para.OverTime = overTime;
                para.TravelAll = travelAll;
                para.Travel = travel;
                para.Leave = leave;
                para.LeaveDuration = leaveDuration;
                para.TravelMoney = travelMoney;
                para.RebMoney = rebMoney;
                para.IsDraft = Convert.ToInt32(isDraft);
                para.WangPanJson = wangPanJson;
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.CompanyId = companyId;

                var files = Request.Form.Files;
                if (files != null && files.Count() != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Voice = blobFiles.Where(t => t.filetype == 2).FirstOrDefault();
                    var Annex = blobFiles.Where(t => t.filetype == 3).ToList();
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();
                    var anotherPicture = blobFiles.Where(t => t.filetype == 0).ToList();
                    //出差报销
                    if (travelReb != null && travelReb != "")
                    {
                        var travelRebFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TravelReb>>(travelReb);
                        foreach (var item in travelRebFiles)
                        {
                            List<DaiZhiXingDeRenWu_url> list1 = new List<DaiZhiXingDeRenWu_url>();
                            var list2 = anotherPicture.Where(t => t.fileMName == item.proPicture).ToList();
                            foreach (var item2 in list2)
                            {
                                DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                                DaiZhiXingDeRenWu_url.url = item2.fileurl;
                                list1.Add(DaiZhiXingDeRenWu_url);
                            }
                            item.proPicture = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        }
                        para.TravelReb = Newtonsoft.Json.JsonConvert.SerializeObject(travelRebFiles);
                    }
                    
                    //普通报销
                    if (reb != null && reb != "")
                    {
                        var rebFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reb>>(reb);
                        foreach (var item in rebFiles)
                        {
                            List<DaiZhiXingDeRenWu_url> list1 = new List<DaiZhiXingDeRenWu_url>();
                            var list2 = anotherPicture.Where(t => t.fileMName == item.proPicture).ToList();
                            foreach (var item2 in list2)
                            {
                                DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                                DaiZhiXingDeRenWu_url.url = item2.fileurl;
                                list1.Add(DaiZhiXingDeRenWu_url);
                            }
                            item.proPicture = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        }
                        para.Reb = Newtonsoft.Json.JsonConvert.SerializeObject(rebFiles);
                    }

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
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }
                    
                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                else
                {
                    if (travelReb != null && travelReb != "")
                    {
                        var travelRebFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TravelReb>>(travelReb);
                        foreach (var item in travelRebFiles)
                        {
                            item.proPicture = "";
                        }
                        para.TravelReb = Newtonsoft.Json.JsonConvert.SerializeObject(travelRebFiles);
                    }
                    if (reb != null && reb != "")
                    {
                        var rebFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reb>>(reb);
                        foreach (var item in rebFiles)
                        {
                            item.proPicture = "";
                        }
                        para.Reb = Newtonsoft.Json.JsonConvert.SerializeObject(rebFiles);
                    }
                }
                return _IWork.CreateWorkApproval(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建日志
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkLog")]
        public Showapi_Res_Meaasge CreateWorkLog()
        {
            try
            {
                Work_Log para = new Work_Log();
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //if (string.IsNullOrEmpty(para.MoBan) || string.IsNullOrEmpty(para.WorkSummary) || string.IsNullOrEmpty(para.WorkPlan) || string.IsNullOrEmpty(para.Experience))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.ReviewPersonName) || string.IsNullOrEmpty(para.ReviewPersonId) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var moBan = string.IsNullOrEmpty(Request.Form["moBan"]) ? "" : Request.Form["moBan"].ToString();
                var moBanTime = string.IsNullOrEmpty(Request.Form["moBanTime"]) ? "" : Request.Form["moBanTime"].ToString();
                var workSummary = string.IsNullOrEmpty(Request.Form["workSummary"]) ? "" : Request.Form["workSummary"].ToString();
                var workPlan = string.IsNullOrEmpty(Request.Form["workPlan"]) ? "" : Request.Form["workPlan"].ToString();
                var experience = string.IsNullOrEmpty(Request.Form["experience"]) ? "" : Request.Form["experience"].ToString();
                var rangenew = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var receipt = string.IsNullOrEmpty(Request.Form["receipt"]) ? "" : Request.Form["receipt"].ToString();
                var reviewPersonName = string.IsNullOrEmpty(Request.Form["reviewPersonName"]) ? "" : Request.Form["reviewPersonName"].ToString();
                var reviewPersonId = string.IsNullOrEmpty(Request.Form["reviewPersonId"]) ? "" : Request.Form["reviewPersonId"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var moneyInfo = string.IsNullOrEmpty(Request.Form["moneyInfo"]) ? "" : Request.Form["moneyInfo"].ToString();
                var money = string.IsNullOrEmpty(Request.Form["money"]) ? "" : Request.Form["money"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.MoBan = moBan;
                para.MoBanTime = moBanTime;
                para.WorkPlan = workPlan;
                para.WorkSummary = workSummary;
                para.Experience = experience;

                var rangelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangenew);
                List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
                foreach (var item in rangelist)
                {
                    if (item.type == "2")
                    {
                        rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                    }
                    if (item.type == "1")
                    {
                        var checkRange = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                        if (!checkRange.Contains(item.id))
                        {
                            rangelistnew.Add(item);
                        }
                    }
                }
                var rangestrnew = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                para.Range = rangestrnew;
                para.RangeNew = rangenew;

                para.Receipt = receipt;
                para.ReviewPersonName = reviewPersonName;
                para.ReviewPersonId = reviewPersonId;
                para.Map = map;
                para.WangPanJson = wangPanJson;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }
                para.IsDraft = Convert.ToInt32(isDraft);
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.MoneyInfo = moneyInfo;
                para.Money = money;
                para.TMSstate = 0;
                para.CompanyId = companyId;

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
                return _IWork.CreateWorkLog(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkTask")]
        public Showapi_Res_Meaasge CreateWorkTask()
        {
            try
            {
                Work_Task para = new Work_Task();
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //if (string.IsNullOrEmpty(para.TaskTitle) || string.IsNullOrEmpty(para.Remarks) || string.IsNullOrEmpty(para.StopTime) || string.IsNullOrEmpty(para.RemindTime))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.Executor) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var taskTitle = string.IsNullOrEmpty(Request.Form["taskTitle"]) ? "" : Request.Form["taskTitle"].ToString();
                var remarks = string.IsNullOrEmpty(Request.Form["remarks"]) ? "" : Request.Form["remarks"].ToString();
                var stopTime = string.IsNullOrEmpty(Request.Form["stopTime"]) ? "" : Request.Form["stopTime"].ToString();
                var remindTime = string.IsNullOrEmpty(Request.Form["remindTime"]) ? "" : Request.Form["remindTime"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var rangenew = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var executor = string.IsNullOrEmpty(Request.Form["executor"]) ? "" : Request.Form["executor"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.TaskTitle = taskTitle;
                para.Remarks = remarks;
                para.StopTime = stopTime;
                List<RemindTime> remindTimeList = new List<RemindTime>();
                var strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RemindTime>>(remindTime);
                if (strList != null && strList.Count != 0)
                {
                    foreach (var item in strList)
                    {
                        RemindTime RemindTime = new RemindTime();
                        RemindTime.timeWord = item.timeWord;
                        RemindTime.time = item.time;
                        remindTimeList.Add(RemindTime);
                    }
                    para.RemindTime = Newtonsoft.Json.JsonConvert.SerializeObject(remindTimeList);
                }
                else
                {
                    para.RemindTime = "";
                }
                para.Map = map;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }

                var rangelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangenew);
                List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
                foreach (var item in rangelist)
                {
                    if (item.type == "2")
                    {
                        rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                    }
                    if (item.type == "1")
                    {
                        var checkRange = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                        if (!checkRange.Contains(item.id))
                        {
                            rangelistnew.Add(item);
                        }
                    }
                }
                var rangestrnew = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                para.Range = rangestrnew;
                para.RangeNew = rangenew;

                para.IsDraft = Convert.ToInt32(isDraft);
                para.WangPanJson = wangPanJson;
                para.Executor = executor;
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.CompanyId = companyId;

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
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }
                    
                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.CreateWorkTask(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建日程
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkProgram")]
        public Showapi_Res_Meaasge CreateWorkProgram()
        {
            try
            {
                Work_Program para = new Work_Program();

                //if (string.IsNullOrEmpty(para.Body) || string.IsNullOrEmpty(para.Year) || string.IsNullOrEmpty(para.Hour) || string.IsNullOrEmpty(para.JoinPerson))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.RemindTime) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var year = string.IsNullOrEmpty(Request.Form["year"]) ? "" : Request.Form["year"].ToString();
                var hour = string.IsNullOrEmpty(Request.Form["hour"]) ? "" : Request.Form["hour"].ToString();
                var joinPerson = string.IsNullOrEmpty(Request.Form["joinPerson"]) ? "" : Request.Form["joinPerson"].ToString();
                var receipt = string.IsNullOrEmpty(Request.Form["receipt"]) ? "" : Request.Form["receipt"].ToString();
                var remindTime = string.IsNullOrEmpty(Request.Form["remindTime"]) ? "" : Request.Form["remindTime"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.Body = body;
                para.Year = year;
                para.Hour = hour;
                para.JoinPerson = joinPerson;
                para.Receipt = receipt;
                List<RemindTime> remindTimeList = new List<RemindTime>();
                var strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RemindTime>>(remindTime);
                if (strList != null && strList.Count != 0)
                {
                    foreach (var item in strList)
                    {
                        RemindTime RemindTime = new RemindTime();
                        RemindTime.timeWord = item.timeWord;
                        RemindTime.time = item.time;
                        remindTimeList.Add(RemindTime);
                    }
                    para.RemindTime = Newtonsoft.Json.JsonConvert.SerializeObject(remindTimeList);
                }
                else
                {
                    para.RemindTime = "";
                }
                para.Range = joinPerson;
                para.Map = map;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }
                para.IsDraft = Convert.ToInt32(isDraft);
                para.WangPanJson = wangPanJson;
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.CompanyId = companyId;

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
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }
                    
                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.CreateWorkProgram(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建指令
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkOrder")]
        public Showapi_Res_Meaasge CreateWorkOrder()
        {
            try
            {
                Work_Order para = new Work_Order();
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //if (string.IsNullOrEmpty(para.Body) || string.IsNullOrEmpty(para.StopTime) || string.IsNullOrEmpty(para.Executor) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var stopTime = string.IsNullOrEmpty(Request.Form["stopTime"]) ? "" : Request.Form["stopTime"].ToString();
                var rangenew = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var executor = string.IsNullOrEmpty(Request.Form["executor"]) ? "" : Request.Form["executor"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.Body = body;
                para.StopTime = stopTime;

                var rangelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangenew);
                List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
                foreach (var item in rangelist)
                {
                    if (item.type == "2")
                    {
                        rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                    }
                    if (item.type == "1")
                    {
                        var checkRange = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                        if (!checkRange.Contains(item.id))
                        {
                            rangelistnew.Add(item);
                        }
                    }
                }
                var rangestrnew = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                para.Range = rangestrnew;
                para.RangeNew = rangenew;

                para.Map = map;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }
                para.IsDraft = Convert.ToInt32(isDraft);
                para.WangPanJson = wangPanJson;
                para.Executor = executor;
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.CompanyId = companyId;

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
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }
                    
                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.CreateWorkOrder(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建公告
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkAnnouncement")]
        public Showapi_Res_Meaasge CreateWorkAnnouncement()
        {
            try
            {
                Work_Announcement para = new Work_Announcement();
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //if (string.IsNullOrEmpty(para.Body) || string.IsNullOrEmpty(para.StopTime) || string.IsNullOrEmpty(para.Executor) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var title = string.IsNullOrEmpty(Request.Form["title"]) ? "" : Request.Form["title"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var beginTime = string.IsNullOrEmpty(Request.Form["beginTime"]) ? "" : Request.Form["beginTime"].ToString();
                var stopTime = string.IsNullOrEmpty(Request.Form["stopTime"]) ? "" : Request.Form["stopTime"].ToString();
                var rangenew = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var receipt = string.IsNullOrEmpty(Request.Form["receipt"]) ? "" : Request.Form["receipt"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.Title = title;
                para.Body = body;
                para.BeginTime = Convert.ToDateTime(beginTime);
                para.StopTime = Convert.ToDateTime(stopTime);

                var rangelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangenew);
                List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
                foreach (var item in rangelist)
                {
                    if (item.type == "2")
                    {
                        rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                    }
                    if (item.type == "1")
                    {
                        var checkRange = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                        if (!checkRange.Contains(item.id))
                        {
                            rangelistnew.Add(item);
                        }
                    }
                }
                var rangestrnew = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                para.Range = rangestrnew;
                para.RangeNew = rangenew;

                para.Receipt = receipt;
                para.Map = map;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }
                para.IsDraft = Convert.ToInt32(isDraft);
                para.WangPanJson = wangPanJson;
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.CompanyId = companyId;

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
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.CreateWorkAnnouncement(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 创建分享
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkShare")]
        public Showapi_Res_Meaasge CreateWorkShare()
        {
            try
            {
                Work_Share para = new Work_Share();
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //if (string.IsNullOrEmpty(para.Body) || string.IsNullOrEmpty(para.StopTime) || string.IsNullOrEmpty(para.Executor) || string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var rangenew = string.IsNullOrEmpty(Request.Form["range"]) ? "" : Request.Form["range"].ToString();
                var receipt = string.IsNullOrEmpty(Request.Form["receipt"]) ? "" : Request.Form["receipt"].ToString();
                var map = string.IsNullOrEmpty(Request.Form["map"]) ? "" : Request.Form["map"].ToString();
                var address = string.IsNullOrEmpty(Request.Form["address"]) ? "" : Request.Form["address"].ToString();
                var isDraft = string.IsNullOrEmpty(Request.Form["isDraft"]) ? "" : Request.Form["isDraft"].ToString();
                var wangPanJson = string.IsNullOrEmpty(Request.Form["wangPanJson"]) ? "" : Request.Form["wangPanJson"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var companyId = string.IsNullOrEmpty(Request.Form["companyId"]) ? "" : Request.Form["companyId"].ToString();

                para.Body = body;

                var rangelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangenew);
                List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
                foreach (var item in rangelist)
                {
                    if (item.type == "2")
                    {
                        rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                    }
                    if (item.type == "1")
                    {
                        var checkRange = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                        if (!checkRange.Contains(item.id))
                        {
                            rangelistnew.Add(item);
                        }
                    }
                }
                var rangestrnew = Newtonsoft.Json.JsonConvert.SerializeObject(rangelistnew);
                para.Range = rangestrnew;
                para.RangeNew = rangenew;

                para.Receipt = receipt;
                para.Map = map;
                if (address == "")
                {
                    para.Address = null;
                }
                else
                {
                    para.Address = address;
                }
                para.IsDraft = Convert.ToInt32(isDraft);
                para.WangPanJson = wangPanJson;
                para.ATPerson = _person;
                para.VoiceLength = voiceLength;
                para.State = 0;
                para.PhoneModel = phoneModel;
                para.CompanyId = companyId;

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
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.Picture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.CreateWorkShare(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 审批详情
        /// </summary>
        /// <param name="审批ID"></param>
        /// <returns></returns>
        [HttpPost("GetShenPiDetail")]
        public Showapi_Res_Single<DaiShenPi> GetShenPiDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<DaiShenPi> res = new Showapi_Res_Single<DaiShenPi>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetShenPiDetail(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 日志详情
        /// </summary>
        /// <param name="日志ID"></param>
        /// <returns></returns>
        [HttpPost("GetRiZhiDetail")]
        public Showapi_Res_Single<DaiDianPingDeRiZhi> GetRiZhiDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<DaiDianPingDeRiZhi> res = new Showapi_Res_Single<DaiDianPingDeRiZhi>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetRiZhiDetail(para);
            }
            catch (Exception ex)
            {
                DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                res.showapi_res_body = DaiDianPingDeRiZhi;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 任务详情
        /// </summary>
        /// <param name="任务ID"></param>
        /// <returns></returns>
        [HttpPost("GetRenWuDetail")]
        public Showapi_Res_Single<DaiZhiXingDeRenWu> GetRenWuDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<DaiZhiXingDeRenWu> res = new Showapi_Res_Single<DaiZhiXingDeRenWu>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetRenWuDetail(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 日程详情
        /// </summary>
        /// <param name="日程ID"></param>
        /// <returns></returns>
        [HttpPost("GetRiChengDetail")]
        public Showapi_Res_Single<RiChengDetail> GetRiChengDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<RiChengDetail> res = new Showapi_Res_Single<RiChengDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetRiChengDetail(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 指令详情
        /// </summary>
        /// <param name="指令ID"></param>
        /// <returns></returns>
        [HttpPost("GetZhiLingDetail")]
        public Showapi_Res_Single<ZhiLingDetail> GetZhiLingDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<ZhiLingDetail> res = new Showapi_Res_Single<ZhiLingDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetZhiLingDetail(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 公告详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetGongGaoDetail")]
        public Showapi_Res_Single<GongGaoDetail> GetGongGaoDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<GongGaoDetail> res = new Showapi_Res_Single<GongGaoDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetGongGaoDetail(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 分享详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFenXiangDetail")]
        public Showapi_Res_Single<FenXiangDetail> GetFenXiangDetail([FromBody]DetailID para)
        {
            Showapi_Res_Single<FenXiangDetail> res = new Showapi_Res_Single<FenXiangDetail>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetFenXiangDetail(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 工作列表中每个工作的详情页
        /// </summary>
        [HttpPost("GetAllDetails")]
        public Showapi_Res_Single<AllDetails> GetAllDetails([FromBody]FocusInPara para)
        {
            Showapi_Res_Single<AllDetails> res = new Showapi_Res_Single<AllDetails>();
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetAllDetails(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 工作附件
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        [HttpPost("GetWorkDoc_AnnexList")]
        public Showapi_Res_List<WorkDoc_Annex> GetWorkDoc_AnnexList([FromBody]WorkDoc_Para para)
        {
            Showapi_Res_List<WorkDoc_Annex> res = new Showapi_Res_List<WorkDoc_Annex>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetWorkDoc_AnnexList(para);
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
        /// 工作图片
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        [HttpPost("GetWorkDoc_PictureList")]
        public Showapi_Res_List<WorkDoc_Picture> GetWorkDoc_PictureList([FromBody]WorkDoc_Para para)
        {
            Showapi_Res_List<WorkDoc_Picture> res = new Showapi_Res_List<WorkDoc_Picture>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetWorkDoc_PictureList(para);
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
        /// 工作录音
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        [HttpPost("GetWorkDoc_VoiceList")]
        public Showapi_Res_List<WorkDoc_Voice> GetWorkDoc_VoiceList([FromBody]WorkDoc_Para para)
        {
            Showapi_Res_List<WorkDoc_Voice> res = new Showapi_Res_List<WorkDoc_Voice>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetWorkDoc_VoiceList(para);
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
        /// 工作列表
        /// </summary>
        [HttpPost("WorkListAll")]
        public Showapi_Res_Single<WorkListAll> GetWorkListAll([FromBody]WorkListAllInPara para)
        {
            Showapi_Res_Single<WorkListAll> res = new Showapi_Res_Single<WorkListAll>();
            try
            {
                if (string.IsNullOrEmpty(para.type.ToString()) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.page.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetWorkListAll(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 工作列表分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetWorkList")]
        public Showapi_Res_List<WorkList> GetWorkList()
        {
            Showapi_Res_List<WorkList> res = new Showapi_Res_List<WorkList>();
            try
            {
                return _IWork.GetWorkList();
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
        /// 审批  同意/不同意
        /// </summary>
        /// <returns></returns>
        [HttpPost("Approval")]
        public Showapi_Res_Meaasge Approval()
        {
            try
            {
                ApprovalInPara para = new ApprovalInPara();

                //if (string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.body) || string.IsNullOrEmpty(para.isAgree.ToString()) || string.IsNullOrEmpty(para.phoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var uid = string.IsNullOrEmpty(Request.Form["uid"]) ? "" : Request.Form["uid"].ToString();
                var memberid = string.IsNullOrEmpty(Request.Form["memberid"]) ? "" : Request.Form["memberid"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var isAgree = string.IsNullOrEmpty(Request.Form["isAgree"]) ? "" : Request.Form["isAgree"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();

                para.uid = uid;
                para.memberid = memberid;
                para.body = body;
                para.isAgree = Convert.ToInt32(isAgree);
                para.phoneModel = phoneModel;
                para.voiceLength = voiceLength;
                para._person = _person;

                var files = Request.Form.Files;
                if (files != null && files.Count() != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Voice = blobFiles.Where(t => t.filetype == 2).FirstOrDefault();
                    var Annex = blobFiles.Where(t => t.filetype == 3).ToList();
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();
                    var anotherPicture = blobFiles.Where(t => t.filetype == 0).FirstOrDefault();

                    if (Voice != null)
                    {
                        para.voice = Voice.fileurl;
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
                        para.annex = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                    }

                    if (Picture.Count != 0)
                    {
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.appendPicture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    if (anotherPicture != null)
                    {
                        para.picture = anotherPicture.fileurl;
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.Approval(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 审批人  取消审批
        /// </summary>
        /// <returns></returns>
        [HttpPost("EscApproval")]
        public Showapi_Res_Meaasge EscApproval([FromBody]EscApprovalInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.phoneModel))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.EscApproval(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 重新选择审批人
        /// </summary>
        [HttpPost("AgainChooseApprovalPerson")]
        public Showapi_Res_Meaasge AgainChooseApprovalPerson([FromBody]AgainChooseApprovalPersonInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.newApprovalPerson))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.AgainChooseApprovalPerson(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 执行人  同意执行
        /// </summary>
        /// <returns></returns>
        [HttpPost("Exe")]
        public Showapi_Res_Meaasge Exe()
        {
            try
            {
                ExeInPara para = new ExeInPara();

                //if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.body))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.phoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var uid = string.IsNullOrEmpty(Request.Form["uid"]) ? "" : Request.Form["uid"].ToString();
                var memberid = string.IsNullOrEmpty(Request.Form["memberid"]) ? "" : Request.Form["memberid"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();

                para.type = type;
                para.uid = uid;
                para.memberid = memberid;
                para.body = body;
                para.phoneModel = phoneModel;
                para.voiceLength = voiceLength;
                para._person = _person;

                var files = Request.Form.Files;
                if (files != null && files.Count() != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Voice = blobFiles.Where(t => t.filetype == 2).FirstOrDefault();
                    var Annex = blobFiles.Where(t => t.filetype == 3).ToList();
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();

                    if (Voice != null)
                    {
                        para.voice = Voice.fileurl;
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
                        para.annex = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                    }

                    if (Picture.Count != 0)
                    {
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.appendPicture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.Exe(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 执行人  取消任务/取消指令    参与人  取消日程
        /// </summary>
        /// <returns></returns>
        [HttpPost("NoExe")]
        public Showapi_Res_Meaasge NoExe([FromBody]NoExeInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.phoneModel))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.NoExe(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 指令  继续执行
        /// </summary>
        /// <returns></returns>
        [HttpPost("ContinueExe")]
        public Showapi_Res_Meaasge ContinueExe()
        {
            try
            {
                ExeInPara para = new ExeInPara();

                //if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.body))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.phoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var uid = string.IsNullOrEmpty(Request.Form["uid"]) ? "" : Request.Form["uid"].ToString();
                var memberid = string.IsNullOrEmpty(Request.Form["memberid"]) ? "" : Request.Form["memberid"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();

                para.type = type;
                para.uid = uid;
                para.memberid = memberid;
                para.body = body;
                para.phoneModel = phoneModel;
                para.voiceLength = voiceLength;
                para._person = _person;

                var files = Request.Form.Files;
                if (files != null && files.Count() != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Voice = blobFiles.Where(t => t.filetype == 2).FirstOrDefault();
                    var Annex = blobFiles.Where(t => t.filetype == 3).ToList();
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();

                    if (Voice != null)
                    {
                        para.voice = Voice.fileurl;
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
                        para.annex = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                    }

                    if (Picture.Count != 0)
                    {
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.appendPicture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.ContinueExe(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        [HttpPost("PingLun")]
        public Showapi_Res_Meaasge PingLun()
        {
            try
            {
                Comment_Body para = new Comment_Body();

                //if (string.IsNullOrEmpty(para.UId) || string.IsNullOrEmpty(para.MemberId) || string.IsNullOrEmpty(para.Type) || string.IsNullOrEmpty(para.Body))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.PhoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var uid = string.IsNullOrEmpty(Request.Form["uid"]) ? "" : Request.Form["uid"].ToString();
                var memberId = string.IsNullOrEmpty(Request.Form["memberId"]) ? "" : Request.Form["memberId"].ToString();
                var pid = string.IsNullOrEmpty(Request.Form["pid"]) ? "" : Request.Form["pid"].ToString();
                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();

                para.UId = uid;
                para.MemberId = memberId;
                para.PId = pid;
                para.Type = type;
                para.Body = body;
                para.VoiceLength = voiceLength;
                para.PhoneModel = phoneModel;
                para.ATPerson = _person;

                var files = Request.Form.Files;
                if (files != null)
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
                        para.PictureList = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }
                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.PingLun(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 点评
        /// </summary>
        /// <returns></returns>
        [HttpPost("DianPing")]
        public Showapi_Res_Meaasge DianPing()
        {
            try
            {
                DianPing_Body para = new DianPing_Body();

                //if (string.IsNullOrEmpty(para.UId) || string.IsNullOrEmpty(para.MemberId) || string.IsNullOrEmpty(para.Type) || string.IsNullOrEmpty(para.Body))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.PhoneModel) || string.IsNullOrEmpty(para.Grade))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var uid = string.IsNullOrEmpty(Request.Form["uid"]) ? "" : Request.Form["uid"].ToString();
                var memberId = string.IsNullOrEmpty(Request.Form["memberId"]) ? "" : Request.Form["memberId"].ToString();
                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var grade = string.IsNullOrEmpty(Request.Form["grade"]) ? "" : Request.Form["grade"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();

                para.UId = uid;
                para.MemberId = memberId;
                para.Type = type;
                para.Grade = grade;
                para.Body = body;
                para.VoiceLength = voiceLength;
                para.PhoneModel = phoneModel;
                para.ATPerson = _person;

                var files = Request.Form.Files;
                if (files != null)
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
                        para.PictureList = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }
                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.DianPing(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 回执
        /// </summary>
        /// <returns></returns>
        [HttpPost("Rece")]
        public Showapi_Res_Meaasge Rece()
        {
            try
            {
                ReceInPara para = new ReceInPara();

                //if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.memberid) || string.IsNullOrEmpty(para.body))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.phoneModel))
                //{
                //    throw new BusinessException("参数不正确。");
                //}

                var type = string.IsNullOrEmpty(Request.Form["type"]) ? "" : Request.Form["type"].ToString();
                var uid = string.IsNullOrEmpty(Request.Form["uid"]) ? "" : Request.Form["uid"].ToString();
                var memberid = string.IsNullOrEmpty(Request.Form["memberid"]) ? "" : Request.Form["memberid"].ToString();
                var body = string.IsNullOrEmpty(Request.Form["body"]) ? "" : Request.Form["body"].ToString();
                var phoneModel = string.IsNullOrEmpty(Request.Form["phoneModel"]) ? "" : Request.Form["phoneModel"].ToString();
                var voiceLength = string.IsNullOrEmpty(Request.Form["voiceLength"]) ? "" : Request.Form["voiceLength"].ToString();
                var _person = string.IsNullOrEmpty(Request.Form["_person"]) ? "" : Request.Form["_person"].ToString();

                para.type = type;
                para.uid = uid;
                para.memberid = memberid;
                para.body = body;
                para.phoneModel = phoneModel;
                para.voiceLength = voiceLength;
                para._person = _person;

                var files = Request.Form.Files;
                if (files != null && files.Count() != 0)
                {
                    var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                    var Voice = blobFiles.Where(t => t.filetype == 2).FirstOrDefault();
                    var Annex = blobFiles.Where(t => t.filetype == 3).ToList();
                    var Picture = blobFiles.Where(t => t.filetype == 1).ToList();

                    if (Voice != null)
                    {
                        para.voice = Voice.fileurl;
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
                        para.annex = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                    }

                    if (Picture.Count != 0)
                    {
                        List<DaiZhiXingDeRenWu_url> list2 = new List<DaiZhiXingDeRenWu_url>();
                        foreach (var item in Picture)
                        {
                            DaiZhiXingDeRenWu_url DaiZhiXingDeRenWu_url = new DaiZhiXingDeRenWu_url();
                            DaiZhiXingDeRenWu_url.url = item.fileurl;
                            list2.Add(DaiZhiXingDeRenWu_url);
                        }
                        para.appendPicture = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                    }

                    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                }
                return _IWork.Rece(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllSearchList")]
        public Showapi_Res_List<PersonDynamic_info> GetAllSearchList([FromBody]AllSearch para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                return _IWork.GetAllSearchList(para);
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
        /// 普通搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetSearchList")]
        public Showapi_Res_List<PersonDynamic_info> GetSearchList([FromBody]Search para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                if (string.IsNullOrEmpty(para.body) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetSearchList(para);
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
        /// 高级搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHighSearchList")]
        public Showapi_Res_List<PersonDynamic_info> GetHighSearchList([FromBody]HighSearch para)
        {
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            try
            {
                //if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.beginTime) || string.IsNullOrEmpty(para.stopTime))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                //if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IWork.GetHighSearchList(para);
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
        /// 高级搜索   列表
        /// </summary>
        [HttpPost("GetHighSearchTypeList")]
        public Showapi_Res_List<HighSearchType> GetHighSearchTypeList()
        {
            Showapi_Res_List<HighSearchType> res = new Showapi_Res_List<HighSearchType>();
            try
            {
                return _IWork.GetHighSearchTypeList();
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
        /// 日志周计划  周列表
        /// </summary>
        [HttpPost("GetOneDay")]
        public Showapi_Res_List<GetOneDay> GetOneDay([FromBody]GetOneDayInPara para)
        {
            Showapi_Res_List<GetOneDay> res = new Showapi_Res_List<GetOneDay>();
            try
            {
                if (string.IsNullOrEmpty(para.oneDay))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetOneDay(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 点击更多返回的状态值
        /// </summary>
        [HttpPost("GetFocusCollectionDeleteState")]
        public Showapi_Res_Single<FocusCollectionDeleteState> GetFocusCollectionDeleteState([FromBody]FocusCollectionDeleteStateInPara para)
        {
            Showapi_Res_Single<FocusCollectionDeleteState> res = new Showapi_Res_Single<FocusCollectionDeleteState>();
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.uid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetFocusCollectionDeleteState(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取中国城市列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetChinaCity")]
        public Showapi_Res_List<ChinaCity> GetChinaCity()
        {
            Showapi_Res_List<ChinaCity> res = new Showapi_Res_List<ChinaCity>();
            try
            {
                return _IWork.GetChinaCity();
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取中国城市列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetChinaCityNew")]
        public Showapi_Res_List<ChinaCity_Province> GetChinaCityNew()
        {
            Showapi_Res_List<ChinaCity_Province> res = new Showapi_Res_List<ChinaCity_Province>();
            try
            {
                return _IWork.GetChinaCityNew();
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 搜索城市
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetChinaCitySearch")]
        public Showapi_Res_List<ChinaCitySearch> GetChinaCitySearch([FromBody]ChinaCitySearchInPara para)
        {
            Showapi_Res_List<ChinaCitySearch> res = new Showapi_Res_List<ChinaCitySearch>();
            try
            {
                if (string.IsNullOrEmpty(para.body))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetChinaCitySearch(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取抄送范围/参与人详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetRangeInfo")]
        public Showapi_Res_List<RangeInfo> GetRangeInfo([FromBody]FocusInPara para)
        {
            Showapi_Res_List<RangeInfo> res = new Showapi_Res_List<RangeInfo>();
            try
            {
                if (string.IsNullOrEmpty(para.type) || string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IWork.GetRangeInfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// WorkTest
        /// </summary>
        /// <returns></returns>
        [HttpPost("WorkTest")]
        public Showapi_Res_Meaasge WorkTest([FromBody]WorkTestInPara para)
        {
            try
            {
                return _IWork.WorkTest(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 工作列表标签顺序  写入
        /// </summary>
        /// <returns></returns>
        [HttpPost("WorkListTagDES")]
        public Showapi_Res_Meaasge WorkListTagDES([FromBody]WorkListTagDESInPara para)
        {
            try
            {
                return _IWork.WorkListTagDES(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 工作列表标签顺序  获取
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetWorkListTagDES")]
        public Showapi_Res_List<WorkListTagDES> GetWorkListTagDES()
        {
            Showapi_Res_List<WorkListTagDES> res = new Showapi_Res_List<WorkListTagDES>();
            try
            {
                return _IWork.GetWorkListTagDES();
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 单个工作刷新
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOneWorkInfo")]
        public Showapi_Res_Single<PersonDynamic_info> GetOneWorkInfo([FromBody]GetOneWorkInfopara para)
        {
            Showapi_Res_Single<PersonDynamic_info> res = new Showapi_Res_Single<PersonDynamic_info>();
            try
            {
                return _IWork.GetOneWorkInfo(para);
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
        /// 获取签到类型
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetQianDaoType")]
        public Showapi_Res_List<QianDaoType> GetQianDaoType()
        {
            Showapi_Res_List<QianDaoType> res = new Showapi_Res_List<QianDaoType>();
            try
            {
                return _IWork.GetQianDaoType();
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 修改抄送范围
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateWorkRange")]
        public Showapi_Res_Meaasge UpdateWorkRange([FromBody]UpdateWorkRangeInPara para)
        {
            try
            {
                return _IWork.UpdateWorkRange(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
    }
}