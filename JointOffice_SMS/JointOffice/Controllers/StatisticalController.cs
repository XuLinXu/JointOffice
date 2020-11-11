using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.Configuration;
using Microsoft.Extensions.Options;
using JointOffice.DbModel;
using JointOffice.Models;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class StatisticalController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IStatistical _IStatistical;
        ExceptionMessage em;
        IOptions<Root> config;
        public StatisticalController(IOptions<Root> config, IStatistical IStatistical, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IStatistical = IStatistical;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 区域列表
        /// </summary>
        [HttpPost("GetAreaList")]
        public Showapi_Res_List<Area> GetAreaList()
        {
            Showapi_Res_List<Area> res = new Showapi_Res_List<Area>();
            try
            {
                return _IStatistical.GetAreaList();
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
        /// 客户转换
        /// </summary>
        [HttpPost("GetKeHuChange")]
        public Showapi_Res_Single<KeHuChange> GetKeHuChange([FromBody]KeHuChangeInPara para)
        {
            Showapi_Res_Single<KeHuChange> res = new Showapi_Res_Single<KeHuChange>();
            try
            {
                return _IStatistical.GetKeHuChange(para);
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
        /// 订单追踪
        /// </summary>
        [HttpPost("GetOrderTrack")]
        public Showapi_Res_Single<OrderTrack> GetOrderTrack([FromBody]KeHuChangeInPara para)
        {
            Showapi_Res_Single<OrderTrack> res = new Showapi_Res_Single<OrderTrack>();
            try
            {
                return _IStatistical.GetOrderTrack(para);
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
        /// 订单列表
        /// </summary>
        [HttpPost("GetOrderList")]
        public Showapi_Res_Single<OrderInfo> GetOrderList([FromBody]OrderListInpara para)
        {
            Showapi_Res_Single<OrderInfo> res = new Showapi_Res_Single<OrderInfo>();
            try
            {
                return _IStatistical.GetOrderList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.ReturnMeaasge(ex);
                return res;
            }
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        [HttpPost("GetOrderDetails")]
        public Showapi_Res_Single<OrderDetails> GetOrderDetails([FromBody]DetailsID para)
        {
            Showapi_Res_Single<OrderDetails> res = new Showapi_Res_Single<OrderDetails>();
            try
            {
                return _IStatistical.GetOrderDetails(para);
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
        /// 销售订单
        /// </summary>
        [HttpPost("GetSalesOrder")]
        public Showapi_Res_Single<SalesOrder> GetSalesOrder([FromBody]KeHuChangeInPara para)
        {
            Showapi_Res_Single<SalesOrder> res = new Showapi_Res_Single<SalesOrder>();
            try
            {
                return _IStatistical.GetSalesOrder(para);
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
        /// 应收款统计
        /// </summary>
        [HttpPost("GetReceiptCount")]
        public Showapi_Res_Single<ReceiptCount> GetReceiptCount([FromBody]KeHuChangeInPara para)
        {
            Showapi_Res_Single<ReceiptCount> res = new Showapi_Res_Single<ReceiptCount>();
            try
            {
                return _IStatistical.GetReceiptCount(para);
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
        /// 回款统计
        /// </summary>
        [HttpPost("GetBackMoney")]
        public Showapi_Res_Single<BackMoney> GetBackMoney([FromBody]KeHuChangeInPara para)
        {
            Showapi_Res_Single<BackMoney> res = new Showapi_Res_Single<BackMoney>();
            try
            {
                return _IStatistical.GetBackMoney(para);
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
        /// 物流信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetWuliuXinXi")]
        public Showapi_Res_Single<WuliuXinXi> GetWuliuXinXi([FromBody]DetailsID para)
        {
            Showapi_Res_Single<WuliuXinXi> res = new Showapi_Res_Single<WuliuXinXi>();
            try
            {
                return _IStatistical.GetWuliuXinXi(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        ///// <summary>
        ///// 目标管理
        ///// </summary>
        //[HttpPost("GetTargetManage")]
        //public Showapi_Res_Single<TargetManage> GetTargetManage([FromBody]TargetManageInPara para)
        //{
        //    Showapi_Res_Single<TargetManage> res = new Showapi_Res_Single<TargetManage>();
        //    try
        //    {
        //        return _IStatistical.GetTargetManage(para);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.showapi_res_code = "508";
        //        res.showapi_res_error = ex.Message;
        //        em.XieLogs(ex);
        //        return res;
        //    }
        //}

        ///// <summary>
        ///// 片区销售任务额
        ///// </summary>
        //[HttpPost("GetAreaSalesMoney")]
        //public Showapi_Res_List<AreaSalesMoney> GetAreaSalesMoney([FromBody]KeHuChangeInPara para)
        //{
        //    Showapi_Res_List<AreaSalesMoney> res = new Showapi_Res_List<AreaSalesMoney>();
        //    try
        //    {
        //        return _IStatistical.GetAreaSalesMoney(para);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.showapi_res_code = "508";
        //        res.showapi_res_error = ex.Message;
        //        em.XieLogs(ex);
        //        return res;
        //    }
        //}
        /// <summary>
        /// 片区详情
        /// </summary>
        [HttpPost("GetAreaDetails")]
        public Showapi_Res_Single<AreaDetails> GetAreaDetails([FromBody]DetailsID para)
        {
            Showapi_Res_Single<AreaDetails> res = new Showapi_Res_Single<AreaDetails>();
            try
            {
                return _IStatistical.GetAreaDetails(para);
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
        /// 销售人员详情
        /// </summary>
        [HttpPost("GetSalesPersonDetails")]
        public Showapi_Res_Single<SalesPersonDetails> GetSalesPersonDetails([FromBody]DetailsID para)
        {
            Showapi_Res_Single<SalesPersonDetails> res = new Showapi_Res_Single<SalesPersonDetails>();
            try
            {
                return _IStatistical.GetSalesPersonDetails(para);
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
        /// 个人信息
        /// </summary>
        [HttpPost("GetMemberInfo")]
        public Showapi_Res_Single<MemberInfo> GetMemberInfo([FromBody]DetailsID para)
        {
            Showapi_Res_Single<MemberInfo> res = new Showapi_Res_Single<MemberInfo>();
            try
            {
                return _IStatistical.GetMemberInfo(para);
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
        /// 个人信息  Odoo
        /// </summary>
        [HttpPost("GetMemberInfoOdoo")]
        public Showapi_Res_Single<MemberInfoOdoo> GetMemberInfoOdoo([FromBody]GetInfoOdooAPI para)
        {
            Showapi_Res_Single<MemberInfoOdoo> res = new Showapi_Res_Single<MemberInfoOdoo>();
            try
            {
                return _IStatistical.GetMemberInfoOdoo(para);
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
        /// HR设置
        /// </summary>
        [HttpPost("GetHRSet")]
        public Showapi_Res_Single<HRSet> GetHRSet([FromBody]DetailsID para)
        {
            Showapi_Res_Single<HRSet> res = new Showapi_Res_Single<HRSet>();
            try
            {
                return _IStatistical.GetHRSet(para);
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
        /// 业务情况
        /// </summary>
        [HttpPost("GetBusinessDetails")]
        public Showapi_Res_Single<BusinessDetails> GetBusinessDetails([FromBody]DetailsID para)
        {
            Showapi_Res_Single<BusinessDetails> res = new Showapi_Res_Single<BusinessDetails>();
            try
            {
                return _IStatistical.GetBusinessDetails(para);
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
        /// 关联销售 销售总监
        /// </summary>
        [HttpPost("GetAreaAllList")]
        public Showapi_Res_Single<GetAreaAllListInfo> GetAreaAllList([FromBody]DetailsID para)
        {
            Showapi_Res_Single<GetAreaAllListInfo> res = new Showapi_Res_Single<GetAreaAllListInfo>();
            try
            {
                return _IStatistical.GetAreaAllList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.ReturnMeaasge(ex);
                return res;
            }
        }
        /// <summary>
        /// 关联销售 销售总监 查看片区
        /// </summary>
        [HttpPost("GetExtensionPersonByAreaList")]
        public Showapi_Res_List<GetExtensionPersonByOneList> GetExtensionPersonByAreaList(GetExtensionPersonListPara para)
        {
            Showapi_Res_List<GetExtensionPersonByOneList> res = new Showapi_Res_List<GetExtensionPersonByOneList>();
            try
            {
                return _IStatistical.GetExtensionPersonByAreaList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.ReturnMeaasge(ex);
                return res;
            }
        }
        /// <summary>
        /// 关联销售 销售经理
        /// </summary>
        [HttpPost("GetExtensionPersonList")]
        public Showapi_Res_List<GetExtensionPersonList> GetExtensionPersonList([FromBody]GetExtensionPersonListPara para)
        {
            Showapi_Res_List<GetExtensionPersonList> res = new Showapi_Res_List<GetExtensionPersonList>();
            try
            {
                return _IStatistical.GetExtensionPersonList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.ReturnMeaasge(ex);
                return res;
            }
        }
        /// <summary>
        /// 关联销售 销售人员
        /// </summary>
        [HttpPost("GetExtensionPersonByOneList")]
        public Showapi_Res_List<GetExtensionPersonByOneList> GetExtensionPersonByOneList([FromBody]GetExtensionPersonByOneListPara para)
        {
            Showapi_Res_List<GetExtensionPersonByOneList> res = new Showapi_Res_List<GetExtensionPersonByOneList>();
            try
            {
                return _IStatistical.GetExtensionPersonByOneList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.ReturnMeaasge(ex);
                return res;
            }
        }
        /// <summary>
        /// 查看投诉订单
        /// </summary>
        [HttpPost("GetComplaintList")]
        public Showapi_Res_List<GetComplaintListInfo> GetComplaintList([FromBody]GetComplaintListPara para)
        {
            Showapi_Res_List<GetComplaintListInfo> res = new Showapi_Res_List<GetComplaintListInfo>();
            try
            {
                return _IStatistical.GetComplaintList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.ReturnMeaasge(ex);
                return res;
            }
        }
    }
}
