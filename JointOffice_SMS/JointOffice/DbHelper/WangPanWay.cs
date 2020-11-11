using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using JointOffice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace JointOffice.DbHelper
{
    public class WangPanWay
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public WangPanWay(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            _PrincipalBase = IPrincipalBase;
            this.config = config;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        public List<filelist> getfileList(List<filelist> list, string memberid)
        {
            foreach (var item in list)
            {
                var list1 = new List<filelist>();
                if (item.wenJianJiaId == "0")
                {
                    var info = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                    if (info != null)
                    {
                        var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == info.Id).ToList();
                        if (WangPan_Menu1.Count > 0)
                        {
                            foreach (var one in WangPan_Menu1)
                            {
                                filelist ListPara = new filelist();
                                ListPara.wenJianJiaId = one.Id;
                                ListPara.label = one.Name;
                                list1.Add(ListPara);
                            }
                            item.children = getfileList(list1, memberid);
                        }
                    }
                }
                else
                {
                    var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == item.wenJianJiaId).ToList();
                    if (WangPan_Menu1.Count > 0)
                    {
                        foreach (var one in WangPan_Menu1)
                        {
                            filelist ListPara = new filelist();
                            ListPara.wenJianJiaId = one.Id;
                            ListPara.label = one.Name;
                            list1.Add(ListPara);
                        }
                        item.children = getfileList(list1, memberid);
                    }
                }
            }
            return list;
        }
        public List<filelist> getgongxiangfileList(List<filelist> list, string memberid)
        {
            foreach (var item in list)
            {
                var list1 = new List<filelist>();
                var WangPan_Menu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == item.wenJianJiaId).ToList();
                if (WangPan_Menu1.Count > 0)
                {
                    foreach (var one in WangPan_Menu1)
                    {
                        filelist ListPara = new filelist();
                        ListPara.wenJianJiaId = one.Id;
                        ListPara.label = one.Name;
                        list1.Add(ListPara);
                    }
                    item.children = getgongxiangfileList(list1, memberid);
                }
            }
            return list;
        }
        public List<string> getfileIdList(string wenJianJiaId, List<string> wenJianJiaIdList)
        {

            var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == wenJianJiaId).ToList();
            if (WangPan_Menu1.Count > 0)
            {
                foreach (var one in WangPan_Menu1)
                {
                    wenJianJiaIdList.Add(one.Id);
                    getfileIdList(one.Id, wenJianJiaIdList);
                }
            }
            return wenJianJiaIdList;
        }
        public List<string> getgongxiangfileIdList(string wenJianJiaId, List<string> wenJianJiaIdList)
        {

            var WangPan_Menu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == wenJianJiaId).ToList();
            if (WangPan_Menu1.Count > 0)
            {
                foreach (var one in WangPan_Menu1)
                {
                    wenJianJiaIdList.Add(one.Id);
                    getgongxiangfileIdList(one.Id, wenJianJiaIdList);
                }
            }
            return wenJianJiaIdList;
        }
    }
}
