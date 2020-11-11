using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace JointOffice.Models
{
    public class BStatistical : IStatistical
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public BStatistical(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 区域列表
        /// </summary>
        public Showapi_Res_List<Area> GetAreaList()
        {
            Showapi_Res_List<Area> res = new Showapi_Res_List<Area>();
            List<Area> list = new List<Area>();
            string[] array1 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
            string[] array2 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
            for (int i = 0; i < array1.Length; i++)
            {
                Area Area = new Area();
                Area.areaID = array1[i];
                Area.areaName = array2[i];
                list.Add(Area);
            }
            res.showapi_res_body = new Showapi_res_body_list<Area>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 客户转换
        /// </summary>
        public Showapi_Res_Single<KeHuChange> GetKeHuChange(KeHuChangeInPara para)
        {
            Showapi_Res_Single<KeHuChange> res = new Showapi_Res_Single<KeHuChange>();
            KeHuChange KeHuChange = new KeHuChange();

            KeHuChange.chartOnlineKeHuNum = "7000";
            KeHuChange.chartOfflineKeHuNum = "3000";
            KeHuChange.chartKeHuNum = "10000";
            KeHuChange.chartRate = "70%";
            var memberid = _PrincipalBase.GetMemberId();
            var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
            List<ColumnCount> list2 = new List<ColumnCount>();
            List<Area> list3 = new List<Area>();
            if (jobid == "销售总监")
            {
                if (para.order == "1")
                {
                    string[] array1 = new string[] { "华南区", "华北区", "华中区", "华东区", "华西区" };
                    string[] array2 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "50", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };

                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array2[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list3.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list3.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }

                }
                else
                {
                    string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                    string[] array2 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array2[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list3.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list3.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }
            }
            else if (jobid == "片区经理")
            {
                string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                string[] array2 = new string[] { "100", "300", "240", "180", "200" };
                //string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                for (int i = 0; i < array1.Length; i++)
                {
                    ColumnCount ColumnCount = new ColumnCount();
                    ColumnCount.area = array1[i];
                    ColumnCount.keHuNum = array2[i];
                    //ColumnCount.onlineKeHuNum = array3[i];
                    //ColumnCount.completeKeHuNum = array4[i];
                    ColumnCount.rate = array5[i];
                    ColumnCount.yesChangeKeHuNum = array6[i];
                    ColumnCount.noChangeKeHuNum = array7[i];
                    list2.Add(ColumnCount);
                }
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list3.Add(Area);
                }
                if (para.area != "0")
                {
                    var one = list3.Where(t => t.areaID == para.area).FirstOrDefault();
                    list2 = list2.Where(t => t.area == one.areaName).ToList();
                }
            }
            else
            {
                ColumnCount ColumnCount = new ColumnCount();
                ColumnCount.area = "张三";
                ColumnCount.keHuNum = "100";
                //ColumnCount.onlineKeHuNum = array3[i];
                //ColumnCount.completeKeHuNum = array4[i];
                ColumnCount.rate = "70%";
                ColumnCount.yesChangeKeHuNum = "70";
                ColumnCount.noChangeKeHuNum = "30";
                list2.Add(ColumnCount);
            }

            KeHuChange.cloumnCount = list2;
            KeHuChange.areaList = list3;
            res.showapi_res_body = KeHuChange;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 订单追踪
        /// </summary>
        public Showapi_Res_Single<OrderTrack> GetOrderTrack(KeHuChangeInPara para)
        {
            Showapi_Res_Single<OrderTrack> res = new Showapi_Res_Single<OrderTrack>();
            OrderTrack OrderTrack = new OrderTrack();
            List<Area> list = new List<Area>();
            var memberid = _PrincipalBase.GetMemberId();
            var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
            if (jobid == "销售总监")
            {
                string[] array01 = new string[] { "0000", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list.Add(Area);
                }
            }
            else if (jobid == "片区经理")
            {
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list.Add(Area);
                }
            }
            OrderTrack.area = list;

            List<OrderProfile> list1 = new List<OrderProfile>();
            string[] array1 = new string[] { "1", "2", "3", "4" };
            string[] array2 = new string[] { "1100", "210000", "10000", "200000" };
            string[] array3 = new string[] { "1600", "170000", "31000", "60000" };
            for (int i = 0; i < array1.Length; i++)
            {
                OrderProfile OrderProfile = new OrderProfile();
                OrderProfile.type = array1[i];
                OrderProfile.target = array2[i];
                OrderProfile.real = array3[i];
                list1.Add(OrderProfile);
            }
            OrderTrack.orderProfile = list1;
            res.showapi_res_body = OrderTrack;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        public Showapi_Res_Single<OrderInfo> GetOrderList(OrderListInpara para)
        {
            Showapi_Res_Single<OrderInfo> res = new Showapi_Res_Single<OrderInfo>();
            OrderInfo OrderInfo = new OrderInfo();
            List<OrderList> list = new List<OrderList>();
            List<Area> list3 = new List<Area>();

            if (para.type == "1")
            {
                string[] array1 = new string[] { "2017.09.01", "2017.09.09", "2017.09.11", "2017.09.15" };
                string[] array2 = new string[] { "优质客户", "优质客户", "重要客户", "重要客户" };
                string[] array3 = new string[] { "¥ 10000", "¥ 9000", "¥ 8000", "¥ 7000" };
                string[] array4 = new string[] { "货到付款", "在线支付", "货到付款", "在线支付" };
                string[] array5 = new string[] { "", "退货订单", "", "投诉订单" };
                string[] array7 = new string[] { "0", "1", "", "2" };
                string[] array6 = new string[] { "", "货物有损坏", "", "物流人员态度恶劣" };
                for (int i = 0; i < array1.Length; i++)
                {
                    OrderList OrderList = new OrderList();
                    OrderList.id = Guid.NewGuid().ToString();
                    OrderList.name = "华东区";
                    OrderList.orderCode = "XS000" + (i + 1);
                    OrderList.keHuName = "海尔集团";
                    OrderList.orderDate = array1[i];
                    OrderList.keHuProperty = array2[i];
                    OrderList.payMoney = array3[i];
                    OrderList.payMethod = array4[i];
                    OrderList.orderState = array5[i];
                    OrderList.reason = array6[i];
                    OrderList.state = array7[i];
                    list.Add(OrderList);
                }
            }
            if (para.type == "2")
            {
                string[] array1 = new string[] { "2017.09.01", "2017.09.09" };
                string[] array2 = new string[] { "优质客户", "优质客户" };
                string[] array3 = new string[] { "¥ 10000", "¥ 9000" };
                string[] array4 = new string[] { "货到付款", "在线支付" };
                string[] array5 = new string[] { "退货订单", "退货订单" };
                string[] array7 = new string[] { "1", "1" };
                string[] array6 = new string[] { "货物有损坏", "货物有损坏" };
                for (int i = 0; i < array1.Length; i++)
                {
                    OrderList OrderList = new OrderList();
                    OrderList.id = Guid.NewGuid().ToString();
                    OrderList.name = "华北区";
                    OrderList.orderCode = "XS000" + (i + 1);
                    OrderList.keHuName = "海尔集团";
                    OrderList.orderDate = array1[i];
                    OrderList.keHuProperty = array2[i];
                    OrderList.payMoney = array3[i];
                    OrderList.payMethod = array4[i];
                    OrderList.orderState = array5[i];
                    OrderList.reason = array6[i];
                    OrderList.state = array7[i];
                    list.Add(OrderList);
                }
            }
            if (para.type == "3")
            {
                string[] array1 = new string[] { "2017.09.11", "2017.09.15" };
                string[] array2 = new string[] { "重要客户", "重要客户" };
                string[] array3 = new string[] { "¥ 8000", "¥ 7000" };
                string[] array4 = new string[] { "货到付款", "在线支付" };
                string[] array5 = new string[] { "投诉订单", "投诉订单" };
                string[] array7 = new string[] { "2", "2" };
                string[] array6 = new string[] { "物流人员态度恶劣", "物流人员态度恶劣" };
                for (int i = 0; i < array1.Length; i++)
                {
                    OrderList OrderList = new OrderList();
                    OrderList.id = Guid.NewGuid().ToString();
                    OrderList.name = "华南区";
                    OrderList.orderCode = "XS000" + (i + 1);
                    OrderList.keHuName = "海尔集团";
                    OrderList.orderDate = array1[i];
                    OrderList.keHuProperty = array2[i];
                    OrderList.payMoney = array3[i];
                    OrderList.payMethod = array4[i];
                    OrderList.orderState = array5[i];
                    OrderList.reason = array6[i];
                    OrderList.state = array7[i];
                    list.Add(OrderList);
                }
            }
            if (para.body != null && para.body != "")
            {
                list = list.Where(t => t.orderCode.Contains(para.body) || t.keHuName.Contains(para.body)).ToList();
            }
            var memberid = _PrincipalBase.GetMemberId();
            var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
            if (jobid == "销售总监")
            {
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list3.Add(Area);
                }
            }
            else if (jobid == "片区经理")
            {
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list3.Add(Area);
                }
            }
            OrderInfo.orderList = list;
            OrderInfo.area = list3;
            //res.showapi_res_body = new Showapi_Res_Single<OrderInfo>();
            res.showapi_res_body = OrderInfo;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        public Showapi_Res_Single<OrderDetails> GetOrderDetails(DetailsID para)
        {
            Showapi_Res_Single<OrderDetails> res = new Showapi_Res_Single<OrderDetails>();
            OrderDetails OrderDetails = new OrderDetails();
            OrderDetails.orderCode = "XS0001";
            OrderDetails.companyName = "NSUS公司";
            OrderDetails.companyLogo = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            OrderDetails.companyAddress = "山东省青岛市XX区XX路XX号";
            OrderDetails.companyPhone = "0532-89096788";
            OrderDetails.area = "华东片区";
            OrderDetails.salesPerson = "张XX";
            OrderDetails.delivery = "交货(1)";
            OrderDetails.invoice = "发票(1)";
            OrderDetails.invoiceAddress = "广东省深圳市福田区xxx路xxx号";
            OrderDetails.orderAddress = "广东省深圳市福田区xxx路xxx号";
            OrderDetails.sureDate = "2017-09-10";
            OrderDetails.payTerms = "立即付费/30天授权/60天授权";
            OrderDetails.deliveryMethod = "付费/免费";
            OrderDetails.deliveryMoney = "￥200.00";
            OrderDetails.payDate = "2017-09-11";
            OrderDetails.noGeldMoney = "¥ 5860.80";
            OrderDetails.zongGeld = "¥ 0.00";
            OrderDetails.payMoney = "¥ 5860.80";
            OrderDetails.payMethod = "在线现金付款";

            List<CommodityDetails> list = new List<CommodityDetails>();
            string[] array1 = new string[] { "亿乐PET预涂膜（亮膜）", "上海牡丹05型快干亮光胶印油墨印刷机油墨", "压印系列胶辊", "AMPHOT阿霍特勒口机锋钢刀片" };
            string[] array2 = new string[] { "厚: 5.4 mm 宽: 310mm", "型号：05-32天蓝", "（类型）标准系列 （包胶外径）20cm（钢芯直径）15cm（包胶长度）100cm（钢芯长度）", "（规格）400x100x12mm" };
            string[] array3 = new string[] { "2480.00", "560.00", "2430.00", "2480.00" };
            string[] array4 = new string[] { "2580.00", "580.00", "2530.00", "2530.00" };
            for (int i = 0; i < array1.Length; i++)
            {
                CommodityDetails CommodityDetails = new CommodityDetails();
                CommodityDetails.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
                CommodityDetails.name = array1[i];
                CommodityDetails.property = array2[i];
                CommodityDetails.nowPrice = array3[i];
                CommodityDetails.formerPrice = array4[i];
                CommodityDetails.buyNum = "1";
                CommodityDetails.giftNum = "1";
                CommodityDetails.geld = "0.00";
                CommodityDetails.rebate = "2.0%";
                CommodityDetails.doInvoice = "1";
                CommodityDetails.subtotal = array3[i];
                list.Add(CommodityDetails);
            }
            OrderDetails.commodityDetails = list;

            res.showapi_res_body = OrderDetails;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 销售订单
        /// </summary>
        public Showapi_Res_Single<SalesOrder> GetSalesOrder(KeHuChangeInPara para)
        {
            Showapi_Res_Single<SalesOrder> res = new Showapi_Res_Single<SalesOrder>();
            SalesOrder SalesOrder = new SalesOrder();
            List<Area> list = new List<Area>();
            List<ColumnCount> list2 = new List<ColumnCount>();
            var memberid = _PrincipalBase.GetMemberId();
            var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
            if (jobid == "销售总监")
            {
                if (para.order == "1")
                {
                    string[] array1 = new string[] { "华南区", "华北区", "华中区", "华东区", "华西区" };
                    string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array3[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }
                else
                {
                    string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                    string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array3[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }
            }
            else if (jobid == "片区经理")
            {

                string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                for (int i = 0; i < array1.Length; i++)
                {
                    ColumnCount ColumnCount = new ColumnCount();
                    ColumnCount.area = array1[i];
                    ColumnCount.keHuNum = array3[i];
                    //ColumnCount.onlineKeHuNum = array3[i];
                    //ColumnCount.completeKeHuNum = array4[i];
                    ColumnCount.rate = array5[i];
                    ColumnCount.yesChangeKeHuNum = array6[i];
                    ColumnCount.noChangeKeHuNum = array7[i];
                    list2.Add(ColumnCount);
                }
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list.Add(Area);
                }
                if (para.area != "0")
                {
                    var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                    list2 = list2.Where(t => t.area == one.areaName).ToList();
                }
            }
            else
            {
                ColumnCount ColumnCount = new ColumnCount();
                ColumnCount.area = "张三";
                ColumnCount.keHuNum = "100";
                //ColumnCount.onlineKeHuNum = array3[i];
                //ColumnCount.completeKeHuNum = array4[i];
                ColumnCount.rate = "70%";
                ColumnCount.yesChangeKeHuNum = "70";
                ColumnCount.noChangeKeHuNum = "30";
                list2.Add(ColumnCount);
            }
            SalesOrder.areaList = list;
            SalesOrder.cloumnCount = list2;

            res.showapi_res_body = SalesOrder;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 应收款统计
        /// </summary>
        public Showapi_Res_Single<ReceiptCount> GetReceiptCount(KeHuChangeInPara para)
        {
            Showapi_Res_Single<ReceiptCount> res = new Showapi_Res_Single<ReceiptCount>();
            ReceiptCount ReceiptCount = new ReceiptCount();
            List<Area> list = new List<Area>();
            List<ColumnCount> list2 = new List<ColumnCount>();
            var memberid = _PrincipalBase.GetMemberId();
            var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
            if (jobid == "销售总监")
            {
                if (para.order == "1")
                {
                    string[] array1 = new string[] { "华南区", "华北区", "华中区", "华东区", "华西区" };
                    string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array3[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }
                else
                {
                    string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                    string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array3[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }
            }
            else if (jobid == "片区经理")
            {
                string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                for (int i = 0; i < array1.Length; i++)
                {
                    ColumnCount ColumnCount = new ColumnCount();
                    ColumnCount.area = array1[i];
                    ColumnCount.keHuNum = array3[i];
                    //ColumnCount.onlineKeHuNum = array3[i];
                    //ColumnCount.completeKeHuNum = array4[i];
                    ColumnCount.rate = array5[i];
                    ColumnCount.yesChangeKeHuNum = array6[i];
                    ColumnCount.noChangeKeHuNum = array7[i];
                    list2.Add(ColumnCount);
                }
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list.Add(Area);
                }
                if (para.area != "0")
                {
                    var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                    list2 = list2.Where(t => t.area == one.areaName).ToList();
                }
            }
            else
            {
                ColumnCount ColumnCount = new ColumnCount();
                ColumnCount.area = "张三";
                ColumnCount.keHuNum = "100";
                //ColumnCount.onlineKeHuNum = array3[i];
                //ColumnCount.completeKeHuNum = array4[i];
                ColumnCount.rate = "70%";
                ColumnCount.yesChangeKeHuNum = "70";
                ColumnCount.noChangeKeHuNum = "30";
                list2.Add(ColumnCount);
            }
            ReceiptCount.areaList = list;
            ReceiptCount.cloumnCount = list2;

            res.showapi_res_body = ReceiptCount;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 回款统计
        /// </summary>
        public Showapi_Res_Single<BackMoney> GetBackMoney(KeHuChangeInPara para)
        {
            Showapi_Res_Single<BackMoney> res = new Showapi_Res_Single<BackMoney>();
            BackMoney BackMoney = new BackMoney();
            List<Area> list = new List<Area>();
            List<ColumnCount> list2 = new List<ColumnCount>();
            var memberid = _PrincipalBase.GetMemberId();
            var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
            if (jobid == "销售总监")
            {
                if (para.order == "1")
                {
                    string[] array1 = new string[] { "华南区", "华北区", "华中区", "华东区", "华西区" };
                    string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array3[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }
                else
                {
                    string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                    string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                    //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                    string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                    string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                    for (int i = 0; i < array1.Length; i++)
                    {
                        ColumnCount ColumnCount = new ColumnCount();
                        ColumnCount.area = array1[i];
                        ColumnCount.keHuNum = array3[i];
                        //ColumnCount.onlineKeHuNum = array3[i];
                        //ColumnCount.completeKeHuNum = array4[i];
                        ColumnCount.rate = array5[i];
                        ColumnCount.yesChangeKeHuNum = array6[i];
                        ColumnCount.noChangeKeHuNum = array7[i];
                        list2.Add(ColumnCount);
                    }
                    string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                    string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                    for (int i = 0; i < array01.Length; i++)
                    {
                        Area Area = new Area();
                        Area.areaID = array01[i];
                        Area.areaName = array02[i];
                        list.Add(Area);
                    }
                    if (para.area != "0")
                    {
                        var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                        list2 = list2.Where(t => t.area == one.areaName).ToList();
                    }
                }

            }
            else if (jobid == "片区经理")
            {

                string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
                string[] array3 = new string[] { "100", "300", "240", "180", "200" };
                //string[] array4 = new string[] { "70", "240", "200", "100", "120" };
                string[] array5 = new string[] { "70%", "80%", "83%", "56%", "60%" };
                string[] array6 = new string[] { "70", "240", "200", "100", "120" };
                string[] array7 = new string[] { "30", "60", "40", "80", "80" };
                for (int i = 0; i < array1.Length; i++)
                {
                    ColumnCount ColumnCount = new ColumnCount();
                    ColumnCount.area = array1[i];
                    ColumnCount.keHuNum = array3[i];
                    //ColumnCount.onlineKeHuNum = array3[i];
                    //ColumnCount.completeKeHuNum = array4[i];
                    ColumnCount.rate = array5[i];
                    ColumnCount.yesChangeKeHuNum = array6[i];
                    ColumnCount.noChangeKeHuNum = array7[i];
                    list2.Add(ColumnCount);
                }
                string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
                string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
                for (int i = 0; i < array01.Length; i++)
                {
                    Area Area = new Area();
                    Area.areaID = array01[i];
                    Area.areaName = array02[i];
                    list.Add(Area);
                }
                if (para.area != "0")
                {
                    var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
                    list2 = list2.Where(t => t.area == one.areaName).ToList();
                }
            }
            else
            {
                ColumnCount ColumnCount = new ColumnCount();
                ColumnCount.area = "张三";
                ColumnCount.keHuNum = "100";
                //ColumnCount.onlineKeHuNum = array3[i];
                //ColumnCount.completeKeHuNum = array4[i];
                ColumnCount.rate = "70%";
                ColumnCount.yesChangeKeHuNum = "70";
                ColumnCount.noChangeKeHuNum = "30";
                list2.Add(ColumnCount);

            }
            BackMoney.areaList = list;
            BackMoney.cloumnCount = list2;
            res.showapi_res_body = BackMoney;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 物流信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<WuliuXinXi> GetWuliuXinXi(DetailsID para)
        {
            Showapi_Res_Single<WuliuXinXi> res = new Showapi_Res_Single<WuliuXinXi>();
            WuliuXinXi WuliuXinXi = new WuliuXinXi();
            List<WuliuInfo> list1 = new List<WuliuInfo>();

            String host = "http://jisukdcx.market.alicloudapi.com";
            String path = "/express/query";
            String appcode = "e32efb009952406fb8187de10369a21b";
            String url = host + path;
            String querys = "number=453557452110&type=ZTO";
            url = url + "?" + querys;

            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "APPCODE " + appcode);
            //HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res1 = _client.GetAsync(url).Result;
            var resList = res1.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<WuliuPara>(resList);

            //WuliuXinXi.issign = objectList.result.issign;
            WuliuXinXi.deliverystatus = objectList.result.deliverystatus;
            switch (objectList.result.deliverystatus)
            {
                case 1:
                    WuliuXinXi.deliverystatusStr = "在途中";
                    break;
                case 2:
                    WuliuXinXi.deliverystatusStr = "派件中";
                    break;
                case 3:
                    WuliuXinXi.deliverystatusStr = "已签收";
                    break;
                case 4:
                    WuliuXinXi.deliverystatusStr = "派送失败";
                    break;
            }
            WuliuXinXi.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            WuliuXinXi.waybillNum = "718815973536";
            WuliuXinXi.infoSource = "中通快递";
            foreach (var item in objectList.result.list)
            {
                WuliuInfo WuliuInfo = new WuliuInfo();
                WuliuInfo.time = item.time;
                WuliuInfo.status = item.status;
                list1.Add(WuliuInfo);
            }
            WuliuXinXi.list = list1;

            res.showapi_res_code = "200";
            res.showapi_res_body = WuliuXinXi;
            return res;
        }
        ///// <summary>
        ///// 目标管理
        ///// </summary>
        //public Showapi_Res_Single<TargetManage> GetTargetManage(TargetManageInPara para)
        //{
        //    Showapi_Res_Single<TargetManage> res = new Showapi_Res_Single<TargetManage>();
        //    TargetManage TargetManage = new TargetManage();
        //    var memberid = _PrincipalBase.GetMemberId();
        //    var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
        //    List<TargetManageBody> list1 = new List<TargetManageBody>();
        //    List<Area> list = new List<Area>();
        //    if (jobid == "销售总监")
        //    {
        //        if (para.order == "1")
        //        {
        //            string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
        //            string[] array02 = new string[] { "全部", "华南区", "华北区", "华中区", "华东区", "华西区" };
        //            for (int i = 0; i < array01.Length; i++)
        //            {
        //                Area Area = new Area();
        //                Area.areaID = array01[i];
        //                Area.areaName = array02[i];
        //                list.Add(Area);
        //            }
        //            TargetManage.areaList = list;

        //            string[] array3 = new string[] { "0001", "0002", "0003", "0004", "0005" };
        //            string[] array1 = new string[] { "华南区", "华北区", "华中区", "华东区", "华西区" };
        //            string[] array2 = new string[] { "100000", "300000", "240000", "180000", "200000" };
        //            for (int i = 0; i < array1.Length; i++)
        //            {
        //                TargetManageBody TargetManageBody = new TargetManageBody();
        //                TargetManageBody.id = array3[i];
        //                TargetManageBody.area = array1[i];
        //                TargetManageBody.money = array2[i];
        //                list1.Add(TargetManageBody);
        //            }

        //            if (para.area != "0")
        //            {
        //                var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
        //                list1 = list1.Where(t => t.area == one.areaName).ToList();
        //            }
        //        }
        //        else
        //        {
        //            string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
        //            string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
        //            for (int i = 0; i < array01.Length; i++)
        //            {
        //                Area Area = new Area();
        //                Area.areaID = array01[i];
        //                Area.areaName = array02[i];
        //                list.Add(Area);
        //            }
        //            TargetManage.areaList = list;

        //            string[] array3 = new string[] { "0001", "0002", "0003", "0004", "0005" };
        //            string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
        //            string[] array2 = new string[] { "100000", "300000", "240000", "180000", "200000" };
        //            for (int i = 0; i < array1.Length; i++)
        //            {
        //                TargetManageBody TargetManageBody = new TargetManageBody();
        //                TargetManageBody.id = array3[i];
        //                TargetManageBody.area = array1[i];
        //                TargetManageBody.money = array2[i];
        //                list1.Add(TargetManageBody);
        //            }
        //            if (para.area != "0")
        //            {
        //                var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
        //                list1 = list1.Where(t => t.area == one.areaName).ToList();
        //            }
        //        }
        //    }
        //    else if (jobid == "片区经理")
        //    {
        //        string[] array01 = new string[] { "0", "0001", "0002", "0003", "0004", "0005" };
        //        string[] array02 = new string[] { "全部", "张三", "李四", "王五", "刘六", "小明" };
        //        for (int i = 0; i < array01.Length; i++)
        //        {
        //            Area Area = new Area();
        //            Area.areaID = array01[i];
        //            Area.areaName = array02[i];
        //            list.Add(Area);
        //        }
        //        TargetManage.areaList = list;

        //        string[] array3 = new string[] { "0001", "0002", "0003", "0004", "0005" };
        //        string[] array1 = new string[] { "张三", "李四", "王五", "刘六", "小明" };
        //        string[] array2 = new string[] { "100000", "300000", "240000", "180000", "200000" };
        //        for (int i = 0; i < array1.Length; i++)
        //        {
        //            TargetManageBody TargetManageBody = new TargetManageBody();
        //            TargetManageBody.id = array3[i];
        //            TargetManageBody.area = array1[i];
        //            TargetManageBody.money = array2[i];
        //            list1.Add(TargetManageBody);
        //        }
        //        if (para.area != "0")
        //        {
        //            var one = list.Where(t => t.areaID == para.area).FirstOrDefault();
        //            list1 = list1.Where(t => t.area == one.areaName).ToList();
        //        }
        //    }
        //    else
        //    {
        //        TargetManageBody TargetManageBody = new TargetManageBody();
        //        TargetManageBody.area = "张三";
        //        TargetManageBody.money = "10000";
        //        list1.Add(TargetManageBody);
        //    }
        //    TargetManage.targetManageBody = list1;

        //    res.showapi_res_code = "200";
        //    res.showapi_res_body = TargetManage;
        //    return res;
        //}
        ///// <summary>
        ///// 片区销售任务额
        ///// </summary>
        //public Showapi_Res_List<AreaSalesMoney> GetAreaSalesMoney(KeHuChangeInPara para)
        //{
        //    Showapi_Res_List<AreaSalesMoney> res = new Showapi_Res_List<AreaSalesMoney>();
        //    //var memberid = _PrincipalBase.GetMemberId();
        //    //var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
        //    List<AreaSalesMoney> list = new List<AreaSalesMoney>();
        //    //if (jobid == "销售总监")
        //    //{
        //    string[] array1 = new string[] { "第一季度", "第二季度", "第三季度", "第四季度" };
        //    string[] array4 = new string[] { "150000", "150000", "150000", "150000" };
        //    string[] array2 = new string[] { "1月份", "2月份", "3月份", "4月份", "5月份", "6月份", "7月份", "8月份", "9月份", "10月份", "11月份", "12月份" };
        //    string[] array3 = new string[] { "50000", "50000", "50000", "50000", "50000", "50000", "50000", "50000", "50000", "50000", "50000", "50000" };
        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        List<AreaSalesMoneyList> list1 = new List<AreaSalesMoneyList>();

        //        if (i == 0)
        //        {
        //            for (int x = 0; x < 3; x++)
        //            {
        //                AreaSalesMoneyList AreaSalesMoneyList = new AreaSalesMoneyList();
        //                AreaSalesMoneyList.month = array2[x];
        //                AreaSalesMoneyList.money = array3[x];
        //                list1.Add(AreaSalesMoneyList);
        //            }
        //        }
        //        if (i == 1)
        //        {
        //            for (int x = 3; x < 6; x++)
        //            {
        //                AreaSalesMoneyList AreaSalesMoneyList = new AreaSalesMoneyList();
        //                AreaSalesMoneyList.month = array2[x];
        //                AreaSalesMoneyList.money = array3[x];
        //                list1.Add(AreaSalesMoneyList);
        //            }
        //        }
        //        if (i == 2)
        //        {
        //            for (int x = 6; x < 9; x++)
        //            {
        //                AreaSalesMoneyList AreaSalesMoneyList = new AreaSalesMoneyList();
        //                AreaSalesMoneyList.month = array2[x];
        //                AreaSalesMoneyList.money = array3[x];
        //                list1.Add(AreaSalesMoneyList);
        //            }
        //        }
        //        if (i == 3)
        //        {
        //            for (int x = 9; x < 12; x++)
        //            {
        //                AreaSalesMoneyList AreaSalesMoneyList = new AreaSalesMoneyList();
        //                AreaSalesMoneyList.month = array2[x];
        //                AreaSalesMoneyList.money = array3[x];
        //                list1.Add(AreaSalesMoneyList);
        //            }
        //        }
        //        AreaSalesMoney AreaSalesMoney = new AreaSalesMoney();
        //        AreaSalesMoney.season = array1[i];
        //        AreaSalesMoney.money = array4[i];
        //        AreaSalesMoney.areaSalesMoneyList = list1;
        //        list.Add(AreaSalesMoney);
        //    }
        //    //}
        //    //else if (jobid == "片区经理")
        //    //{
        //    //    AreaSalesMoney.area = "张三";
        //    //    AreaSalesMoney.year = "2017";
        //    //    string[] array1 = new string[] { "第一季度", "1月份", "2月份", "3月份", "第二季度", "4月份", "5月份", "6月份", "第三季度", "7月份", "8月份", "9月份", "第四季度", "10月份", "11月份", "12月份" };
        //    //    string[] array2 = new string[] { "150000", "50000", "50000", "50000", "150000", "50000", "50000", "50000", "150000", "50000", "50000", "50000", "150000", "50000", "50000", "50000" };
        //    //    for (int i = 0; i < array1.Length; i++)
        //    //    {

        //    //    }

        //    //}
        //    res.showapi_res_code = "200";
        //    res.showapi_res_body = new Showapi_res_body_list<AreaSalesMoney>();
        //    res.showapi_res_body.contentlist = list;
        //    return res;
        //}
        /// <summary>
        /// 片区详情
        /// </summary>
        public Showapi_Res_Single<AreaDetails> GetAreaDetails(DetailsID para)
        {
            Showapi_Res_Single<AreaDetails> res = new Showapi_Res_Single<AreaDetails>();
            AreaDetails AreaDetails = new AreaDetails();
            List<MemberList> list = new List<MemberList>();
            AreaDetails.areaName = "华东区";
            AreaDetails.areaFuZeRen = "张XX";

            string[] array1 = new string[] { "张XX", "王XX", "李XX", "刘XX", "赵XX" };
            for (int i = 0; i < array1.Length; i++)
            {
                MemberList MemberList = new MemberList();
                MemberList.memberid = Guid.NewGuid().ToString();
                MemberList.name = array1[i];
                MemberList.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
                MemberList.jobName = "销售专员";
                list.Add(MemberList);
            }
            AreaDetails.memberList = list;

            res.showapi_res_code = "200";
            res.showapi_res_body = AreaDetails;
            return res;
        }
        /// <summary>
        /// 销售人员详情
        /// </summary>
        public Showapi_Res_Single<SalesPersonDetails> GetSalesPersonDetails(DetailsID para)
        {
            Showapi_Res_Single<SalesPersonDetails> res = new Showapi_Res_Single<SalesPersonDetails>();
            SalesPersonDetails SalesPersonDetails = new SalesPersonDetails();
            SalesPersonDetails.memberid = Guid.NewGuid().ToString();
            SalesPersonDetails.name = "张XX";
            SalesPersonDetails.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            SalesPersonDetails.workAddress = "山东省青岛市城阳区";
            SalesPersonDetails.workMobilePhone = "15625874563";
            SalesPersonDetails.address = "山东省青岛市市南区";
            SalesPersonDetails.email = "45236@163.com";
            SalesPersonDetails.workPhone = "0532-87563214";
            SalesPersonDetails.department = "销售部";
            SalesPersonDetails.jobName = "销售人员";
            SalesPersonDetails.admin = "李XX";
            SalesPersonDetails.master = "刘XX";
            SalesPersonDetails.workTime = "08:00-17:00";
            res.showapi_res_code = "200";
            res.showapi_res_body = SalesPersonDetails;
            return res;
        }
        /// <summary>
        /// 个人信息
        /// </summary>
        public Showapi_Res_Single<MemberInfo> GetMemberInfo(DetailsID para)
        {
            Showapi_Res_Single<MemberInfo> res = new Showapi_Res_Single<MemberInfo>();
            MemberInfo MemberInfo = new MemberInfo();
            MemberInfo.name = "张三";
            MemberInfo.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            MemberInfo.jobname = "销售人员";
            MemberInfo.gender = "男";
            MemberInfo.country = "中国";
            MemberInfo.birthday = "1981-11-11";
            MemberInfo.birthplace = "上海";
            MemberInfo.marital = "已婚";
            MemberInfo.sonNum = "10";
            MemberInfo.familyAddress = "上海市虹桥区";
            MemberInfo.idCard = "555666198111115412";
            MemberInfo.passportNum = "54566846FSD";
            MemberInfo.bankNum = "6214565775514286523";

            res.showapi_res_code = "200";
            res.showapi_res_body = MemberInfo;
            return res;
        }
        /// <summary>
        /// 个人信息  Odoo
        /// </summary>
        public Showapi_Res_Single<MemberInfoOdoo> GetMemberInfoOdoo(GetInfoOdooAPI para)
        {
            Showapi_Res_Single<MemberInfoOdoo> res = new Showapi_Res_Single<MemberInfoOdoo>();

            GetInfoOdooAPI GetInfoOdooAPI = new GetInfoOdooAPI();
            GetInfoOdooAPI.id = para.id;
            GetInfoOdooAPI.token = para.token;
            GetInfoOdooAPI.mark = para.mark;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(GetInfoOdooAPI);
            OdooMemberInfoAPIResult result = new OdooMemberInfoAPIResult();
            result = UseOdooAPI.GetAnyInfoOdoo<OdooMemberInfoAPIResult>("http://localhost:8088/info/employee", str);
            if (result.success)
            {
                var body = result.result;
                if (body != null && body.Count != 0)
                {
                    MemberInfoOdoo MemberInfoOdoo = new MemberInfoOdoo();
                    MemberInfoOdoo.id = body[0].id;
                    MemberInfoOdoo.name = body[0].name;
                    MemberInfoOdoo.image = body[0].image;
                    MemberInfoOdoo.jobID = body[0].jobID;
                    MemberInfoOdoo.gender = body[0].gender;
                    MemberInfoOdoo.country = body[0].country;
                    MemberInfoOdoo.birthday = body[0].birthday;
                    MemberInfoOdoo.city = body[0].city;
                    MemberInfoOdoo.home = body[0].home;
                    MemberInfoOdoo.idCard = body[0].idCard;
                    MemberInfoOdoo.bankID = body[0].bankID;

                    res.showapi_res_code = "200";
                    res.showapi_res_body = MemberInfoOdoo;
                }
            }
            else
            {
                res.showapi_res_code = "507";
                res.showapi_res_error = "数据异常";
            }
            return res;
        }
        /// <summary>
        /// HR设置
        /// </summary>
        public Showapi_Res_Single<HRSet> GetHRSet(DetailsID para)
        {
            Showapi_Res_Single<HRSet> res = new Showapi_Res_Single<HRSet>();
            HRSet HRSet = new HRSet();
            HRSet.workHourTable = "10000";
            HRSet.subject = "科目一";
            HRSet.relatedUser = "无";
            HRSet.badgeID = "65413654";
            HRSet.pin = "jkjjkj552";
            HRSet.attendance = "10";
            HRSet.experience = "无";
            HRSet.companyCar = "Maserati Levante";
            HRSet.distance = "1km";
            HRSet.number = "123456789";
            res.showapi_res_code = "200";
            res.showapi_res_body = HRSet;
            return res;
        }
        /// <summary>
        /// 业务情况
        /// </summary>
        public Showapi_Res_Single<BusinessDetails> GetBusinessDetails(DetailsID para)
        {
            Showapi_Res_Single<BusinessDetails> res = new Showapi_Res_Single<BusinessDetails>();
            BusinessDetails BusinessDetails = new BusinessDetails();
            BusinessDetails.keHuNum = "100";
            BusinessDetails.onlineKeHuNum = "70";
            BusinessDetails.offlineKeHuNum = "30";
            BusinessDetails.rate = "70%";
            BusinessDetails.yearOrderMoney = "500000";
            BusinessDetails.carryOrderMoney = "300000";
            BusinessDetails.noCarryOrderMoney = "200000";
            BusinessDetails.monthOrderMoney = "30000";
            BusinessDetails.monthCarryOrderMoney = "20000";
            BusinessDetails.keHuOweMoney = "5000";
            BusinessDetails.keHuOweOrder = "2";
            BusinessDetails.yearPlanMoney = "5000";
            BusinessDetails.monthPlanMoney = "3000";

            res.showapi_res_code = "200";
            res.showapi_res_body = BusinessDetails;
            return res;
        }
        /// <summary>
        /// 关联销售 销售总监
        /// </summary>
        public Showapi_Res_Single<GetAreaAllListInfo> GetAreaAllList(DetailsID para)
        {
            Showapi_Res_Single<GetAreaAllListInfo> res = new Showapi_Res_Single<GetAreaAllListInfo>();
            GetAreaAllListInfo GetAreaAllListInfo = new GetAreaAllListInfo();
            List<GetAreaAllList> list = new List<Models.GetAreaAllList>();

            string[] array4 = new string[] { "0001", "0002", "0003" };
            string[] array = new string[] { "推广片区1", "推广片区2", "推广片区3" };
            string[] array1 = new string[] { "片区1", "片区2", "片区3" };
            string[] array5 = new string[] { "0001", "0002", "0003" };
            string[] array2 = new string[] { "片区4", "片区5", "片区6" };
            string[] array6 = new string[] { "0001", "0002", "0003" };
            string[] array3 = new string[] { "片区7", "片区8", "片区9" };
            string[] array7 = new string[] { "0001", "0002", "0003" };
            //string[] array2 = new string[] { "推广片区1", "推广片区2", "推广片区3" };

            GetAreaAllList GetAreaAllList = new GetAreaAllList();
            GetAreaAllList.name = array[0];
            GetAreaAllList.id = Guid.NewGuid().ToString();
            List<Area> list1 = new List<Area>();
            for (int i = 0; i < array1.Length; i++)
            {
                Area Area = new Area();
                Area.areaID = array5[i];
                Area.areaName = array1[i];
                list1.Add(Area);
            }
            GetAreaAllList.area = list1;
            list.Add(GetAreaAllList);

            GetAreaAllList GetAreaAllList1 = new GetAreaAllList();
            GetAreaAllList1.name = array[1];
            GetAreaAllList1.id = Guid.NewGuid().ToString();
            List<Area> list2 = new List<Area>();
            for (int i = 0; i < array2.Length; i++)
            {
                Area Area = new Area();
                Area.areaID = array6[i];
                Area.areaName = array2[i];
                list2.Add(Area);
            }
            GetAreaAllList1.area = list2;
            list.Add(GetAreaAllList1);

            GetAreaAllList GetAreaAllList2 = new GetAreaAllList();
            GetAreaAllList2.name = array[2];
            GetAreaAllList2.id = Guid.NewGuid().ToString();
            List<Area> list3 = new List<Area>();
            for (int i = 0; i < array3.Length; i++)
            {
                Area Area = new Area();
                Area.areaID = array7[i];
                Area.areaName = array3[i];
                list3.Add(Area);
            }
            GetAreaAllList2.area = list3;
            list.Add(GetAreaAllList2);

            List<Area> list4 = new List<Area>();

            string[] array01 = new string[] { "0", "0001", "0002", "0003" };
            string[] array02 = new string[] { "全部", "推广片区1", "推广片区2", "推广片区3" };
            for (int i = 0; i < array01.Length; i++)
            {
                Area Area = new Area();
                Area.areaID = array01[i];
                Area.areaName = array02[i];
                list4.Add(Area);
            }
            if (para.id != "0")
            {
                var one = list4.Where(t => t.areaID == para.id).FirstOrDefault();
                list = list.Where(t => t.name == one.areaName).ToList();
            }
            GetAreaAllListInfo.info = list;
            GetAreaAllListInfo.area = list4;
            res.showapi_res_body = GetAreaAllListInfo;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 关联销售 销售总监 查看片区
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_List<GetExtensionPersonByOneList> GetExtensionPersonByAreaList(GetExtensionPersonListPara para)
        {
            Showapi_Res_List<GetExtensionPersonByOneList> res = new Showapi_Res_List<GetExtensionPersonByOneList>();
            List<GetExtensionPersonByOneList> list = new List<Models.GetExtensionPersonByOneList>();

            GetExtensionPersonByOneList GetExtensionPersonByOneList = new GetExtensionPersonByOneList();
            GetExtensionPersonByOneList.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            GetExtensionPersonByOneList.name = "张三";
            GetExtensionPersonByOneList.jobName = "片区推广专员";
            GetExtensionPersonByOneList.address = "青岛市城阳区";
            GetExtensionPersonByOneList.telephone = "13866886688";
            GetExtensionPersonByOneList.mobile = "88888888";
            GetExtensionPersonByOneList.memberId = Guid.NewGuid().ToString();
            list.Add(GetExtensionPersonByOneList);

            GetExtensionPersonByOneList GetExtensionPersonByOneList1 = new GetExtensionPersonByOneList();
            GetExtensionPersonByOneList1.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            GetExtensionPersonByOneList1.name = "李四";
            GetExtensionPersonByOneList1.jobName = "片区推广专员";
            GetExtensionPersonByOneList1.address = "青岛市城阳区";
            GetExtensionPersonByOneList1.telephone = "13866886688";
            GetExtensionPersonByOneList1.mobile = "88888888";
            GetExtensionPersonByOneList1.memberId = Guid.NewGuid().ToString();
            list.Add(GetExtensionPersonByOneList1);


            res.showapi_res_body = new Showapi_res_body_list<GetExtensionPersonByOneList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 关联销售 销售经理
        /// </summary>
        public Showapi_Res_List<GetExtensionPersonList> GetExtensionPersonList(GetExtensionPersonListPara para)
        {
            Showapi_Res_List<GetExtensionPersonList> res = new Showapi_Res_List<GetExtensionPersonList>();
            List<GetExtensionPersonList> list = new List<Models.GetExtensionPersonList>();
            GetExtensionPersonList GetExtensionPersonList = new GetExtensionPersonList();
            GetExtensionPersonList.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            GetExtensionPersonList.name = "张三";
            GetExtensionPersonList.jobName = "片区推广专员";
            GetExtensionPersonList.address = "青岛市城阳区";
            GetExtensionPersonList.telephone = "13866886688";
            GetExtensionPersonList.mobile = "88888888";
            GetExtensionPersonList.memberId = Guid.NewGuid().ToString();
            List<MemberList> list2 = new List<MemberList>();
            string[] array = new string[] { "李四", "王五", "刘六" };
            for (int i = 0; i < array.Length; i++)
            {
                MemberList memberList = new MemberList();
                memberList.memberid = Guid.NewGuid().ToString();
                memberList.name = array[i];
                list2.Add(memberList);
            }
            GetExtensionPersonList.personList = list2;
            list.Add(GetExtensionPersonList);

            //GetExtensionPersonList GetExtensionPersonList2 = new GetExtensionPersonList();
            //GetExtensionPersonList2.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            //GetExtensionPersonList2.name = "丁磊";
            //GetExtensionPersonList2.jobName = "片区推广专员";
            //GetExtensionPersonList2.address = "青岛市城阳区";
            //GetExtensionPersonList2.telephone = "13866886688";
            //GetExtensionPersonList2.mobile = "88888888";
            //GetExtensionPersonList2.memberId = Guid.NewGuid().ToString();
            //List<MemberList> list3 = new List<MemberList>();
            //string[] array1 = new string[] { "小明", "小红", "大头" };
            //for (int i = 0; i < array1.Length; i++)
            //{
            //    MemberList memberList1 = new MemberList();
            //    memberList1.memberid = Guid.NewGuid().ToString();
            //    memberList1.name = array1[i];
            //    list3.Add(memberList1);
            //}
            //GetExtensionPersonList2.personList = list3;
            //list.Add(GetExtensionPersonList2);



            res.showapi_res_body = new Showapi_res_body_list<GetExtensionPersonList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 关联销售 销售人员
        /// </summary>
        public Showapi_Res_List<GetExtensionPersonByOneList> GetExtensionPersonByOneList(GetExtensionPersonByOneListPara para)
        {
            Showapi_Res_List<GetExtensionPersonByOneList> res = new Showapi_Res_List<GetExtensionPersonByOneList>();
            List<GetExtensionPersonByOneList> list = new List<Models.GetExtensionPersonByOneList>();

            GetExtensionPersonByOneList GetExtensionPersonByOneList = new GetExtensionPersonByOneList();
            GetExtensionPersonByOneList.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            GetExtensionPersonByOneList.name = "张三";
            GetExtensionPersonByOneList.jobName = "片区推广专员";
            GetExtensionPersonByOneList.address = "青岛市城阳区";
            GetExtensionPersonByOneList.telephone = "13866886688";
            GetExtensionPersonByOneList.mobile = "88888888";
            GetExtensionPersonByOneList.memberId = Guid.NewGuid().ToString();
            list.Add(GetExtensionPersonByOneList);

            //GetExtensionPersonByOneList GetExtensionPersonByOneList1 = new GetExtensionPersonByOneList();
            //GetExtensionPersonByOneList1.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            //GetExtensionPersonByOneList1.name = "李四";
            //GetExtensionPersonByOneList1.jobName = "片区推广专员";
            //GetExtensionPersonByOneList1.address = "青岛市城阳区";
            //GetExtensionPersonByOneList1.telephone = "13866886688";
            //GetExtensionPersonByOneList1.mobile = "88888888";
            //GetExtensionPersonByOneList1.memberId = Guid.NewGuid().ToString();
            //list.Add(GetExtensionPersonByOneList1);


            res.showapi_res_body = new Showapi_res_body_list<GetExtensionPersonByOneList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 查看投诉订单
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_List<GetComplaintListInfo> GetComplaintList(GetComplaintListPara para)
        {
            Showapi_Res_List<GetComplaintListInfo> res = new Showapi_Res_List<GetComplaintListInfo>();
            List<GetComplaintListInfo> list = new List<GetComplaintListInfo>();
            var createdate = "";
            var enddate = "";
            if (para.season==null || para.season=="")
            {
                createdate = para.year + "-01-01 00:00:00";
                enddate =(Convert.ToInt32( para.year)+1).ToString() + "-01-01 00:00:00";
            }
            else
            {
                if (para.month == null || para.month == "")
                {
                    createdate = para.year + "-"+ (3*(Convert.ToInt32(para.season)-1)+1).ToString() +"-01 00:00:00";
                    enddate = para.year + "-" + (3 * Convert.ToInt32(para.season) + 1).ToString() + "-01 00:00:00";
                }
                else
                {
                    createdate = para.year + "-" + para.month + "-01 00:00:00";
                    if (para.month=="12")
                    {
                        enddate = (Convert.ToInt32(para.year)+1).ToString() + "-01-01 00:00:00";
                    }
                    
                }
            }
            GetComplaintListInfoPara GetComplaintListInfoPara = new GetComplaintListInfoPara();
            GetComplaintListInfoPara.type = "1";
            GetComplaintListInfoPara.createdate = createdate;
            GetComplaintListInfoPara.enddate = enddate;

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(GetComplaintListInfoPara);
            Showapi_Res_Single<OdooReturn<GetComplaintListInfo>> result = new Showapi_Res_Single<OdooReturn<GetComplaintListInfo>>();

            result = UseOdooAPI.GetAnyInfoOdoo<Showapi_Res_Single<OdooReturn<GetComplaintListInfo>>>("http://localhost:8088/api/v1.0/website/call/complaint_list", str);
            if (result.showapi_res_body.success)
            {
                list = result.showapi_res_body.result;
                foreach (var item in list)
                {
                    if (item.state== "pending")
                    {
                        item.state = "待处理";
                    }
                    else if (item.state == "handle")
                    {
                        item.state = "处理中";
                    }
                    else if (item.state == "complete")
                    {
                        item.state = "已处理";
                    }
                }
            }
            else
            {
                res.showapi_res_code = "507";
                res.showapi_res_error = "数据异常";
            }


            res.showapi_res_body = new Showapi_res_body_list<GetComplaintListInfo>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;

            //GetComplaintListInfo GetComplaintListInfo = new GetComplaintListInfo();
            //GetComplaintListInfo.complaintCode = "";
            //GetComplaintListInfo.complaintCompany = "";
            //GetComplaintListInfo.complaintDate = "";
            //GetComplaintListInfo.id = "";
            //GetComplaintListInfo.name = "";
            //GetComplaintListInfo.orderCode = "";
            //GetComplaintListInfo.state = "";



            //return res;
        }
    }
}
