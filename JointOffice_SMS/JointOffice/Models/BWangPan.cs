using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BWangPan : IWangPan
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public BWangPan(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 访问文件
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge FangWenWenJian(FangWenWenJianPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";

            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            else
            {
                //string[] b = para.url.Split(new Char[] { '?' });
                //var url = b[0];
                var OldJiLu = _JointOfficeContext.WangPan_ZuiJin.Where(t => t.MemberId == memberid && t.WenJianId == para.wenJianJiaId).FirstOrDefault();
                if (OldJiLu != null)
                {
                    OldJiLu.SeeDate = DateTime.Now;
                }
                else
                {
                    var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                    if (WangPan_WenJian != null)
                    {
                        WangPan_ZuiJin WangPan_ZuiJin = new WangPan_ZuiJin();
                        WangPan_ZuiJin.Id = Guid.NewGuid().ToString();
                        WangPan_ZuiJin.MemberId = memberid;
                        WangPan_ZuiJin.SeeDate = DateTime.Now;
                        WangPan_ZuiJin.Length = para.length;
                        WangPan_ZuiJin.type = WangPan_WenJian.type;
                        WangPan_ZuiJin.WenJianId = para.wenJianJiaId;
                        WangPan_ZuiJin.CreateDate = DateTime.Now;
                        WangPan_ZuiJin.Url = WangPan_WenJian.url;
                        WangPan_ZuiJin.Name = WangPan_WenJian.FileName;
                        var dizhi = WangPan_WenJian.FileName;
                        var id = WangPan_WenJian.MenuId;
                        while (true)
                        {
                            var One = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == id && t.ParentId != "0").FirstOrDefault();
                            if (One != null)
                            {
                                dizhi = One.Name + "/" + dizhi;
                                id = One.ParentId;
                            }
                            else
                            {
                                dizhi = "我的文件/" + dizhi;
                                break;
                            }
                        }
                        WangPan_ZuiJin.DiZhi = dizhi;
                        _JointOfficeContext.WangPan_ZuiJin.Add(WangPan_ZuiJin);
                    }
                    var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                    if (WangPan_GongXiangWenJian != null)
                    {
                        WangPan_ZuiJin WangPan_ZuiJin = new WangPan_ZuiJin();
                        WangPan_ZuiJin.Id = Guid.NewGuid().ToString();
                        WangPan_ZuiJin.MemberId = memberid;
                        WangPan_ZuiJin.SeeDate = DateTime.Now;
                        WangPan_ZuiJin.Length = para.length;
                        WangPan_ZuiJin.WenJianId = para.wenJianJiaId;
                        WangPan_ZuiJin.CreateDate = DateTime.Now;
                        WangPan_ZuiJin.type = WangPan_GongXiangWenJian.type;
                        WangPan_ZuiJin.Url = WangPan_GongXiangWenJian.url;
                        WangPan_ZuiJin.Name = WangPan_GongXiangWenJian.FileName;
                        var dizhi = WangPan_GongXiangWenJian.FileName;
                        var id = WangPan_GongXiangWenJian.MenuId;
                        while (true)
                        {
                            var One = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == id).FirstOrDefault();
                            if (One != null)
                            {
                                dizhi = One.Name + "/" + dizhi;
                                id = One.ParentId;
                            }
                            else
                            {
                                dizhi = "共享文件/" + dizhi;
                                break;
                            }
                        }
                        WangPan_ZuiJin.DiZhi = dizhi + WangPan_GongXiangWenJian.FileName;
                        _JointOfficeContext.WangPan_ZuiJin.Add(WangPan_ZuiJin);
                    }
                    var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                    if (WangPan_QiYeWenJian != null)
                    {
                        WangPan_ZuiJin WangPan_ZuiJin = new WangPan_ZuiJin();
                        WangPan_ZuiJin.Id = Guid.NewGuid().ToString();
                        WangPan_ZuiJin.MemberId = memberid;
                        WangPan_ZuiJin.SeeDate = DateTime.Now;
                        WangPan_ZuiJin.WenJianId = para.wenJianJiaId;
                        WangPan_ZuiJin.CreateDate = DateTime.Now;
                        WangPan_ZuiJin.Length = para.length;
                        WangPan_ZuiJin.type = WangPan_QiYeWenJian.type;
                        WangPan_ZuiJin.Url = WangPan_QiYeWenJian.url;
                        WangPan_ZuiJin.Name = WangPan_QiYeWenJian.FileName;
                        var dizhi = WangPan_QiYeWenJian.FileName;
                        var id = WangPan_QiYeWenJian.MenuId;
                        var teamid = "";
                        while (true)
                        {
                            var One = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == id).FirstOrDefault();
                            if (One != null)
                            {
                                dizhi = One.Name + "/" + dizhi;
                                id = One.ParentId;
                                teamid = One.TeamId;
                            }
                            else
                            {
                                var Team = _JointOfficeContext.Member_Team.Where(t => t.Id == teamid).FirstOrDefault();
                                dizhi = Team.Name + "/" + dizhi;
                                break;
                            }
                        }
                        WangPan_ZuiJin.DiZhi = dizhi + WangPan_QiYeWenJian.FileName;
                        _JointOfficeContext.WangPan_ZuiJin.Add(WangPan_ZuiJin);
                    }
                }
                _JointOfficeContext.SaveChanges();
                return Message.SuccessMeaasge("访问成功");
            }
        }
        /// <summary>
        /// 最近文件
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<ZuiJinWenJianList> GetZuiJinWenJianList()
        {
            Showapi_Res_List<ZuiJinWenJianList> res = new Showapi_Res_List<ZuiJinWenJianList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<ZuiJinWenJianList>();
                return Return.Return();
            }
            else
            {
                //string sql = @"select  WenJianUrl,SeeDate  from WangPan_ZuiJin where MemberId='" + memberid + "' order by seedate desc";
                List<ZuiJinWenJianList> List = new List<ZuiJinWenJianList>();
                //using (SqlConnection conText = new SqlConnection(constr))
                //{
                //    List = conText.Query<ZuiJinWenJianList>(sql).ToList();
                //}
                //foreach (var item in List)
                //{
                //    item.url = item.url + SasKey;
                //}

                var list1 = _JointOfficeContext.WangPan_ZuiJin.Where(t => t.MemberId == memberid).OrderByDescending(t => t.SeeDate).ToList();
                foreach (var item in list1)
                {
                    ZuiJinWenJianList ZuiJinWenJianList = new ZuiJinWenJianList();
                    ZuiJinWenJianList.dizhi = item.DiZhi;
                    ZuiJinWenJianList.url = item.Url + SasKey;
                    ZuiJinWenJianList.name = item.Name;
                    ZuiJinWenJianList.type = item.type;
                    ZuiJinWenJianList.length = item.Length;
                    ZuiJinWenJianList.id = item.WenJianId;
                    ZuiJinWenJianList.date = item.SeeDate.ToString("yyyy-MM-dd HH:mm:ss");
                    List.Add(ZuiJinWenJianList);
                }

                res.showapi_res_code = "200";
                res.showapi_res_body = new Showapi_res_body_list<ZuiJinWenJianList>();
                res.showapi_res_body.contentlist = List;
                return res;
            }
        }
        /// <summary>
        /// 我的网盘
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<WoDeWangPanList> WoDeWangPan()
        {
            Showapi_Res_Single<WoDeWangPanList> res = new Showapi_Res_Single<WoDeWangPanList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<WoDeWangPanList>();
                return ReturnSingle.Return();
            }
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.MemberId == memberid && t.ParentId == "0").FirstOrDefault();
            var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.UId == WangPan_Menu.Id).ToList();
            WoDeWangPanList WoDeWangPanList = new WoDeWangPanList();
            WoDeWangPanList.wenJianJiaId = WangPan_Menu.Id;
            WoDeWangPanList.length = WangPan_WenJian.Sum(t => t.length).ToString();
            List<QiYeWenJian> list = new List<QiYeWenJian>();
            var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.TeamPerson.Contains(memberid) || t.MemberId == memberid).ToList();
            foreach (var item in Member_Team)
            {
                var QiYeWenJian = new QiYeWenJian();
                QiYeWenJian.name = item.Name;
                QiYeWenJian.wenJianJiaId = item.Id;
                long length = 0;
                var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.TeamId == item.Id).Select(t => t.Id).ToList();
                //var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.TeamPerson.Contains(memberid)).Select(t => t.Id).ToList();
                foreach (var One in WangPan_QiYeMenu)
                {
                    var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.UId == One).ToList();
                    length = WangPan_QiYeWenJian.Sum(t => t.length) + length;
                }
                QiYeWenJian.length = BusinessHelper.ConvertBytes(length);
                list.Add(QiYeWenJian);
            }
            WoDeWangPanList.qiyelist = list;
            res.showapi_res_code = "200";
            res.showapi_res_body = WoDeWangPanList;
            return res;
        }
        /// <summary>
        /// 文件动态
        /// </summary>
        /// <param name="页数，总数"></param>
        /// <returns></returns>
        public Showapi_Res_List<WenJianDongTaiList> GetWenJianDongTaiList(GetWenJianDongTaiListPara para)
        {
            Showapi_Res_List<WenJianDongTaiList> res = new Showapi_Res_List<WenJianDongTaiList>();
            List<WenJianDongTaiList> List = new List<WenJianDongTaiList>();

            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WenJianDongTaiList>();
                return Return.Return();
            }
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            var JiLulist1 = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 1).OrderByDescending(t => t.CreateDate).Select(t => new { t.Tid, t.CreateDate }).ToList();
            List<ZuiJinWenJianInfo> list = new List<ZuiJinWenJianInfo>();
            foreach (var item in JiLulist1)
            {
                ZuiJinWenJianInfo ZuiJinWenJianInfo = new ZuiJinWenJianInfo();
                ZuiJinWenJianInfo.tid = item.Tid;
                ZuiJinWenJianInfo.date = item.CreateDate.ToString("yyyy-MM-dd HH");
                list.Add(ZuiJinWenJianInfo);
            }
            var shujuList = list.GroupBy(t => new { t.tid, t.date }).ToList();
            var JiLulist = shujuList.Skip(para.page * para.count).Take(para.count).ToList();
            foreach (var item in JiLulist)
            {
                var begindate = Convert.ToDateTime(item.Key.date + ":00:00");
                var enddate = Convert.ToDateTime(item.Key.date + ":59:59");
                var JiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.Tid == item.Key.tid && t.CreateDate>= begindate && t.CreateDate <= enddate).ToList();
                WenJianDongTaiList onedate = new WenJianDongTaiList();
                var OneJilu = JiLu.OrderByDescending(t => t.CreateDate).FirstOrDefault();
                var Info = JiLu.Where(t => t.BlobType != 3).FirstOrDefault();
                if (Info!=null)
                {
                    onedate.type = 2;
                }
                else
                {
                    onedate.type = 1;
                }
                onedate.date = OneJilu.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
               
                onedate.name = OneJilu.Name;
                onedate.person = "我";
                onedate.personurl = info.Picture + SasKey;
                onedate.qita = 0;
                List<WenJianInfo> infolist = new List<WenJianInfo>();
                foreach (var One in JiLu)
                {
                    WenJianInfo WenJianInfo = new WenJianInfo();
                    WenJianInfo.filename = One.FileName;
                    WenJianInfo.url = One.Url + SasKey;
                    WenJianInfo.isdelete = One.IsDelete;
                    WenJianInfo.blobType = One.BlobType;
                    WenJianInfo.length = BusinessHelper.ConvertBytes(One.Length); ;
                    infolist.Add(WenJianInfo);
                    if (One.Url != null)
                    {
                        string[] b = One.Url.Split(new Char[] { '.' });
                        var url = b[1];
                        if (url != "gif" || url != "bmp" || url != "jpg" || url != "jpeg" || url != "png")
                        {
                            onedate.qita = 1;
                        }
                    }
                    else
                    {
                        onedate.qita = 2;
                    }

                }
                onedate.list = infolist;
                onedate.count = infolist.Count();
                List.Add(onedate);
            }
            int allPages = shujuList.Count / para.count;
            if (list.Count % para.count != 0)
            {
                allPages += 1;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WenJianDongTaiList>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.allNum = shujuList.Count;
            res.showapi_res_body.contentlist = List;
            return res;
        }
        /// <summary>
        /// 修改共享文件夹权限
        /// </summary>
        /// <param name="文件夹ID，文件类型，个人IDList"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateGongXiangQuanXian(UpdateGongXiangQuanXianPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var PeopleList = new List<People>();
            var str = "";
            List<People> list = new List<People>();
            if (para.shiFouDelete == 1)
            {
                if (para.type == 1)
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.GuanLi);
                    foreach (var item in para.memberidlist)
                    {
                        var one = list.Where(t => t.memberid == item).FirstOrDefault();
                        list.Remove(one);
                    }
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    WangPan_GongXiangMenu.GuanLi = str;
                }
                else if (para.type == 2)
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ShangChuan);
                    foreach (var item in para.memberidlist)
                    {
                        var one = list.Where(t => t.memberid == item).FirstOrDefault();
                        list.Remove(one);
                    }
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    WangPan_GongXiangMenu.ShangChuan = str;
                }
                else if (para.type == 3)
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ChaKan);
                    foreach (var item in para.memberidlist)
                    {
                        var one = list.Where(t => t.memberid == item).FirstOrDefault();
                        list.Remove(one);
                    }
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    WangPan_GongXiangMenu.ChaKan = str;
                }
            }
            else
            {
                int i = 0;
                if (para.memberidlist.Contains(memberid))
                {
                    para.memberidlist.Remove(memberid);
                }
                if (para.type == 1)
                {
                    if (WangPan_GongXiangMenu.GuanLi != null)
                    {
                        list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.GuanLi);

                    }
                    foreach (var item in para.memberidlist)
                    {
                        People People = new People();
                        People.memberid = item;
                        list.Add(People);
                        if (i != 1)
                        {
                            if (WangPan_GongXiangMenu.ShangChuan != null)
                            {
                                if (WangPan_GongXiangMenu.ShangChuan.Contains(item))
                                {
                                    var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ShangChuan);
                                    foreach (var OneDate in para.memberidlist)
                                    {
                                        var one = list1.Where(t => t.memberid == OneDate).FirstOrDefault();
                                        list1.Remove(one);
                                    }
                                    var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                                    WangPan_GongXiangMenu.ShangChuan = str1;
                                    i = 1;
                                }
                            }
                        }
                        if (i != 2)
                        {
                            if (WangPan_GongXiangMenu.ChaKan != null)
                            {
                                if (WangPan_GongXiangMenu.ChaKan.Contains(item))
                                {
                                    var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ChaKan);
                                    foreach (var OneDate in para.memberidlist)
                                    {
                                        var one = list1.Where(t => t.memberid == OneDate).FirstOrDefault();
                                        list1.Remove(one);
                                    }
                                    var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                                    WangPan_GongXiangMenu.ChaKan = str2;
                                    i = 2;
                                }
                            }
                        }
                    }
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    WangPan_GongXiangMenu.GuanLi = str;
                }
                else if (para.type == 2)
                {
                    if (WangPan_GongXiangMenu.ShangChuan != null)
                    {
                        list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ShangChuan);
                    }
                    foreach (var item in para.memberidlist)
                    {
                        People People = new People();
                        People.memberid = item;
                        list.Add(People);
                        if (i != 1)
                        {
                            if (WangPan_GongXiangMenu.GuanLi != null)
                            {
                                if (WangPan_GongXiangMenu.GuanLi.Contains(item))
                                {
                                    var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.GuanLi);
                                    foreach (var OneDate in para.memberidlist)
                                    {
                                        var one = list1.Where(t => t.memberid == OneDate).FirstOrDefault();
                                        list1.Remove(one);
                                    }
                                    var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                                    WangPan_GongXiangMenu.GuanLi = str1;
                                    i = 1;
                                }
                            }
                        }
                        if (i != 2)
                        {
                            if (WangPan_GongXiangMenu.ChaKan != null)
                            {
                                if (WangPan_GongXiangMenu.ChaKan.Contains(item))
                                {
                                    var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ChaKan);
                                    foreach (var OneDate in para.memberidlist)
                                    {
                                        var one = list1.Where(t => t.memberid == OneDate).FirstOrDefault();
                                        list1.Remove(one);
                                    }
                                    var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                                    WangPan_GongXiangMenu.ChaKan = str2;
                                    i = 2;
                                }
                            }
                        }
                    }

                    str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    WangPan_GongXiangMenu.ShangChuan = str;
                }
                else if (para.type == 3)
                {
                    if (WangPan_GongXiangMenu.ChaKan != null)
                    {
                        list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ChaKan);
                    }
                    foreach (var item in para.memberidlist)
                    {
                        People People = new People();
                        People.memberid = item;
                        list.Add(People);
                        if (i != 1)
                        {
                            if (WangPan_GongXiangMenu.GuanLi != null)
                            {
                                if (WangPan_GongXiangMenu.GuanLi.Contains(item))
                                {
                                    var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.GuanLi);
                                    foreach (var OneDate in para.memberidlist)
                                    {
                                        var one = list1.Where(t => t.memberid == OneDate).FirstOrDefault();
                                        list1.Remove(one);
                                    }
                                    var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                                    WangPan_GongXiangMenu.GuanLi = str1;
                                    i = 1;
                                }
                            }
                        }
                        if (i != 2)
                        {
                            if (WangPan_GongXiangMenu.ShangChuan != null)
                            {
                                if (WangPan_GongXiangMenu.ShangChuan.Contains(item))
                                {
                                    var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ShangChuan);
                                    foreach (var OneDate in para.memberidlist)
                                    {
                                        var one = list1.Where(t => t.memberid == OneDate).FirstOrDefault();
                                        list1.Remove(one);
                                    }
                                    var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                                    WangPan_GongXiangMenu.ShangChuan = str2;
                                    i = 2;
                                }
                            }
                        }
                    }
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    WangPan_GongXiangMenu.ChaKan = str;
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 查看共享文件夹的权限人数
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GongXiangQuanXian> ChaKanGongXiangQuanXian(ChaKanGongXiangQuanXianPara para)
        {
            Showapi_Res_Single<GongXiangQuanXian> res = new Showapi_Res_Single<GongXiangQuanXian>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<GongXiangQuanXian>();
                return ReturnSingle.Return();
            }
            var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var guanlilist = new List<People>();
            var shangchuanlist = new List<People>();
            var chakanlist = new List<People>();
            if (WangPan_GongXiangMenu.GuanLi != null && WangPan_GongXiangMenu.GuanLi != "")
            {
                guanlilist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.GuanLi);
            }
            if (WangPan_GongXiangMenu.ShangChuan != null && WangPan_GongXiangMenu.ShangChuan != "")
            {
                shangchuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ShangChuan);
            }
            if (WangPan_GongXiangMenu.ChaKan != null && WangPan_GongXiangMenu.ChaKan != "")
            {
                chakanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ChaKan);
            }
            var GongXiangQuanXian = new GongXiangQuanXian();
            GongXiangQuanXian.guanli = guanlilist.Count() + 1;
            GongXiangQuanXian.shangchuan = shangchuanlist.Count();
            GongXiangQuanXian.chakan = chakanlist.Count();
            res.showapi_res_body = GongXiangQuanXian;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 查看共享文件夹的权限人员
        /// </summary>
        /// <param name="文件夹ID，文件类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<GongXiangQuanXianRenYuanList> GongXiangQuanXianRenYuan(GongXiangQuanXianRenYuanPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();

            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GongXiangQuanXianRenYuanList>();
                return Return.Return();
            }
            Showapi_Res_List<GongXiangQuanXianRenYuanList> res = new Showapi_Res_List<GongXiangQuanXianRenYuanList>();
            List<GongXiangQuanXianRenYuanList> list = new List<GongXiangQuanXianRenYuanList>();
            var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var renyuanlist = new List<People>();
            if (para.type == 1)
            {
                if (WangPan_GongXiangMenu.GuanLi != null && WangPan_GongXiangMenu.GuanLi != "")
                {
                    renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.GuanLi);
                }
                People People = new People();
                People.memberid = WangPan_GongXiangMenu.ChuanJian;
                renyuanlist.Add(People);
            }
            else if (para.type == 2)
            {
                if (WangPan_GongXiangMenu.ShangChuan != null && WangPan_GongXiangMenu.ShangChuan != "")
                {
                    renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ShangChuan);
                }
            }
            else if (para.type == 3)
            {
                if (WangPan_GongXiangMenu.ChaKan != null && WangPan_GongXiangMenu.ChaKan != "")
                {
                    renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(WangPan_GongXiangMenu.ChaKan);
                }
            }
            foreach (var item in renyuanlist)
            {
                var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.memberid).FirstOrDefault();
                GongXiangQuanXianRenYuanList GongXiangQuanXianRenYuanList = new GongXiangQuanXianRenYuanList();

                GongXiangQuanXianRenYuanList.membrid = item.memberid;
                GongXiangQuanXianRenYuanList.name = Member_Info.Name;
                GongXiangQuanXianRenYuanList.url = Member_Info.Picture + SasKey;
                list.Add(GongXiangQuanXianRenYuanList);
            }
            res.showapi_res_body = new Showapi_res_body_list<GongXiangQuanXianRenYuanList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取共享文件列表
        /// </summary>
        /// <param name="顺序"></param>
        /// <returns></returns>
        public Showapi_Res_List<GongXiangWenJianList> GetGongXiangWenJianList(GetGongXiangWenJianListPara para)
        {
            Showapi_Res_List<GongXiangWenJianList> res = new Showapi_Res_List<GongXiangWenJianList>();
            List<GongXiangWenJianList> list = new List<GongXiangWenJianList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GongXiangWenJianList>();
                return Return.Return();
            }
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == "0" && (t.ChuanJian == memberid || t.GuanLi.Contains(memberid))).ToList();
            foreach (var item in WangPan_GongXiangMenu)
            {
                var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id).ToList();
                var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault();
                var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == item.TeamId).FirstOrDefault();
                var length = WangPan_GongXiangWenJian.Sum(t => t.length);
                GongXiangWenJianList GongXiangWenJianList = new GongXiangWenJianList();
                GongXiangWenJianList.length = length.ToString();
                GongXiangWenJianList.name = item.Name;
                GongXiangWenJianList.renyuanname = Member_Info.Name;
                GongXiangWenJianList.qunzuname = Member_Team.Name;
                GongXiangWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                GongXiangWenJianList.wenJianJiaId = item.Id;
                GongXiangWenJianList.type = 1;
                GongXiangWenJianList.blobtype = "1";
                list.Add(GongXiangWenJianList);
            }
            var WangPan_GongXiangMenu2 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ShangChuan.Contains(memberid)).ToList();
            foreach (var item in WangPan_GongXiangMenu2)
            {
                var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id).ToList();
                var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault();
                var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == item.TeamId).FirstOrDefault();
                var length = WangPan_GongXiangWenJian.Sum(t => t.length);
                GongXiangWenJianList GongXiangWenJianList = new GongXiangWenJianList();
                GongXiangWenJianList.length = length.ToString();
                GongXiangWenJianList.name = item.Name;
                GongXiangWenJianList.renyuanname = Member_Info.Name;
                GongXiangWenJianList.qunzuname = Member_Team.Name;
                GongXiangWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                GongXiangWenJianList.wenJianJiaId = item.Id;
                GongXiangWenJianList.type = 2;
                GongXiangWenJianList.blobtype = "1";
                list.Add(GongXiangWenJianList);
            }
            var WangPan_GongXiangMenu3 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t =>t.ChaKan.Contains(memberid)).ToList();
            foreach (var item in WangPan_GongXiangMenu3)
            {
                var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id).ToList();
                var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault();
                var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == item.TeamId).FirstOrDefault();
                var length = WangPan_GongXiangWenJian.Sum(t => t.length);
                GongXiangWenJianList GongXiangWenJianList = new GongXiangWenJianList();
                GongXiangWenJianList.length = length.ToString();
                GongXiangWenJianList.name = item.Name;
                GongXiangWenJianList.renyuanname = Member_Info.Name;
                GongXiangWenJianList.qunzuname = Member_Team.Name;
                GongXiangWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                GongXiangWenJianList.wenJianJiaId = item.Id;
                GongXiangWenJianList.type = 3;
                GongXiangWenJianList.blobtype = "1";
                list.Add(GongXiangWenJianList);
            }
            if (list.Count() > 0)
            {
                if (para.shunxu == "name")
                {
                    list = list.OrderBy(t => t.name).ToList();
                }
                else
                {
                    list = list.OrderByDescending(t => t.date).ToList();
                }
            }
            res.showapi_res_body = new Showapi_res_body_list<GongXiangWenJianList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取企业文件列表
        /// </summary>
        /// <param name="文件夹ID，顺序"></param>
        /// <returns></returns>
        public Showapi_Res_List<QiYeWenJianList> GetQiYeWenJianList(GetQiYeWenJianListPara para)
        {
            Showapi_Res_List<QiYeWenJianList> res = new Showapi_Res_List<QiYeWenJianList>();
            List<QiYeWenJianList> list = new List<QiYeWenJianList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<QiYeWenJianList>();
                return Return.Return();
            }
            //var Id = _JointOfficeContext.Member_Team.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.TeamId == para.wenJianJiaId).ToList();
            foreach (var item in WangPan_QiYeMenu)
            {
                QiYeWenJianList QiYeWenJianList = new QiYeWenJianList();
                QiYeWenJianList.name = item.Name;
                QiYeWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                QiYeWenJianList.wenJianJiaId = item.Id;
                var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.UId == para.wenJianJiaId).ToList();
                QiYeWenJianList.length = WangPan_QiYeWenJian.Sum(t => t.length).ToString();
                if (item.ParentId == "0")
                {
                    QiYeWenJianList.type = 1;
                }
                else
                {
                    QiYeWenJianList.type = 2;
                }
                list.Add(QiYeWenJianList);
            }
            res.showapi_res_body = new Showapi_res_body_list<QiYeWenJianList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<BuMenList> GetBuMenList()
        {
            Showapi_Res_List<BuMenList> res = new Showapi_Res_List<BuMenList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<BuMenList>();
                return Return.Return();
            }
            var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.MemberId == memberid || t.TeamPerson.Contains(memberid)).ToList();
            List<BuMenList> list = new List<BuMenList>();
            foreach (var item in Member_Team)
            {
                BuMenList QiYeList = new BuMenList();
                QiYeList.teamid = item.Id;
                QiYeList.name = item.Name;
                list.Add(QiYeList);
            }
            res.showapi_res_body = new Showapi_res_body_list<BuMenList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;

        }
        /// <summary>
        ///  全局搜索
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_List<AllList> GetAllList(GetAllListPara para)
        {
            Showapi_Res_List<AllList> res = new Showapi_Res_List<AllList>();
            List<AllList> list = new List<AllList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<AllList>();
                return Return.Return();
            }
            var gongxiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ChuanJian == memberid || t.ChaKan.Contains(memberid) || t.GuanLi.Contains(memberid) || t.ShangChuan.Contains(memberid)).ToList();
            var gongxiang = new List<WangPan_GongXiangWenJian>();
            foreach (var item in gongxiangMenu)
            {
                var gongxiangList = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id && t.FileName.Contains(para.name)).ToList();
                gongxiang.AddRange(gongxiang);
            }
            var team = _JointOfficeContext.Member_Team.Where(t => t.MemberId == memberid || t.TeamPerson.Contains(memberid)).ToList();
            var qiyemenu = new List<WangPan_QiYeMenu>();
            foreach (var item in team)
            {
                var qiyemenulist = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.TeamId == item.Id).ToList();
                qiyemenu.AddRange(qiyemenulist);
            }
            var qiye = new List<WangPan_QiYeWenJian>();
            foreach (var item in qiyemenu)
            {
                var qiyeList = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == item.Id && t.FileName.Contains(para.name)).ToList();
                qiye.AddRange(qiyeList);
            }
            var wenjian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MemberId == memberid && t.FileName.Contains(para.name)).ToList();
            //var list1 = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.FileName.Contains(para.name)).ToList();
            //var list2 = _JointOfficeContext.WangPan_WenJian.Where(t => t.FileName.Contains(para.name)).ToList();
            //var list3 = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.FileName.Contains(para.name)).ToList();
            if (gongxiang.Count != 0)
            {
                foreach (var item in gongxiang)
                {
                    var dizhi = item.FileName;
                    var id = item.MenuId;
                    while (true)
                    {
                        var One = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == id).FirstOrDefault();
                        if (One != null)
                        {
                            dizhi = One.Name + "/" + dizhi;
                            id = One.ParentId;
                        }
                        else
                        {
                            dizhi = "共享文件/" + dizhi;
                            break;
                        }
                    }
                    AllList AllList = new AllList();
                    AllList.name = item.FileName;
                    AllList.wenJianId = item.Id;
                    AllList.length = BusinessHelper.ConvertBytes(item.length);
                    AllList.dizhi = dizhi;
                    AllList.url = item.url + SasKey;
                    AllList.type = item.type.ToString();
                    list.Add(AllList);
                }
            }
            if (qiye.Count != 0)
            {
                foreach (var item in qiye)
                {
                    var dizhi = item.FileName;
                    var id = item.MenuId;
                    var teamid = "";
                    while (true)
                    {
                        var One = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == id).FirstOrDefault();
                        if (One != null)
                        {
                            dizhi = One.Name + "/" + dizhi;
                            id = One.ParentId;
                            teamid = One.TeamId;
                        }
                        else
                        {
                            var Team = _JointOfficeContext.Member_Team.Where(t => t.Id == teamid).FirstOrDefault();
                            dizhi = Team.Name + "/" + dizhi;
                            break;
                        }
                    }
                    AllList AllList = new AllList();
                    AllList.name = item.FileName;
                    AllList.wenJianId = item.Id;
                    AllList.dizhi = dizhi;
                    AllList.length = BusinessHelper.ConvertBytes(item.length);
                    AllList.url = item.url + SasKey;
                    AllList.type = item.type.ToString();
                    list.Add(AllList);
                }
            }
            if (wenjian.Count != 0)
            {
                foreach (var item in wenjian)
                {
                    var dizhi = item.FileName;
                    var id = item.MenuId;
                    while (true)
                    {
                        var One = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == id && t.ParentId != "0").FirstOrDefault();
                        if (One != null)
                        {
                            dizhi = One.Name + "/" + dizhi;
                            id = One.ParentId;
                        }
                        else
                        {
                            dizhi = "我的文件/" + dizhi;
                            break;
                        }
                    }
                    AllList AllList = new AllList();
                    AllList.name = item.FileName;
                    AllList.wenJianId = item.Id;
                    AllList.length = BusinessHelper.ConvertBytes(item.length);
                    AllList.dizhi = dizhi;
                    AllList.url = item.url + SasKey;
                    AllList.type = item.type.ToString();
                    list.Add(AllList);
                }
            }
            list = list.Skip(para.page * para.count).Take(para.count).ToList();
            res.showapi_res_body = new Showapi_res_body_list<AllList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge DownloadBlob(List<DownloadBlobPara> para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";

            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            else
            {
                foreach (var item in para)
                {
                    WangPan_Download WangPan_Download = new WangPan_Download();
                    WangPan_Download.Id = Guid.NewGuid().ToString();
                    WangPan_Download.MemberId = memberid;
                    WangPan_Download.Url = item.url.Replace(SasKey, "");
                    WangPan_Download.FileName = item.name;
                    WangPan_Download.Type = item.type;
                    WangPan_Download.Length = item.length;
                    WangPan_Download.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_Download.Add(WangPan_Download);
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("下载成功");

        }
        /// <summary>
        /// 下载列表
        /// </summary>
        public Showapi_Res_List<GetDownloadBlobList> GetDownloadBlobList(GetDownloadBlobListPara para)
        {
            Showapi_Res_List<GetDownloadBlobList> res = new Showapi_Res_List<GetDownloadBlobList>();
            List<GetDownloadBlobList> list = new List<GetDownloadBlobList>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GetDownloadBlobList>();
                return Return.Return();
            }
            var list1 = _JointOfficeContext.WangPan_Download.Where(t => t.MemberId == memberid).OrderByDescending(t => t.CreateDate).Skip(para.page * para.count).Take(para.count).ToList();
            foreach (var item in list1)
            {
                GetDownloadBlobList GetDownloadBlobList = new GetDownloadBlobList();
                GetDownloadBlobList.id = item.Id;
                GetDownloadBlobList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                GetDownloadBlobList.name = item.FileName;
                GetDownloadBlobList.url = item.Url + SasKey;
                GetDownloadBlobList.type = item.Type;
                GetDownloadBlobList.length = item.Length;
                list.Add(GetDownloadBlobList);
            }
            res.showapi_res_body = new Showapi_res_body_list<GetDownloadBlobList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;

        }
        /// <summary>
        /// 下载列表删除记录
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteDownloadBlobList(List<DeleteDownloadBlobListPara> para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";

            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            else
            {
                foreach (var item in para)
                {
                    var One = _JointOfficeContext.WangPan_Download.Where(t => t.Id == item.id).FirstOrDefault();
                    if (One != null)
                    {
                        _JointOfficeContext.WangPan_Download.Remove(One);
                    }
                    else
                    {
                        throw new BusinessException("无此条记录.");
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 重新保存共享文件夹权限
        /// </summary>
        /// <param name="文件夹ID，文件类型，个人IDList"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge SaveGongXiangQuanXian(SaveGongXiangQuanXianPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var PeopleList = new List<People>();
            var str = "";
            List<People> list = new List<People>();

            if (para.type == 1)
            {
                foreach (var item in para.memberidlist)
                {
                    People People = new People();
                    if (item.membrid != null && item.membrid != memberid)
                    {
                        People.memberid = item.membrid;
                        list.Add(People);
                    }
                }
                var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                WangPan_GongXiangMenu.GuanLi = str1;
            }
            else if (para.type == 2)
            {
                foreach (var item in para.memberidlist)
                {
                    People People = new People();
                    if (item.membrid != null && item.membrid != memberid)
                    {
                        People.memberid = item.membrid;
                        list.Add(People);
                    }
                }
                var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                WangPan_GongXiangMenu.ShangChuan = str1;
            }
            else if (para.type == 3)
            {
                foreach (var item in para.memberidlist)
                {
                    People People = new People();
                    if (item.membrid != null && item.membrid != memberid)
                    {
                        People.memberid = item.membrid;
                        list.Add(People);
                    }
                }
                var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                WangPan_GongXiangMenu.ChaKan = str1;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
    }
}
