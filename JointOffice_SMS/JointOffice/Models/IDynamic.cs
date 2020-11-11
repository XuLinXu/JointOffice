using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IDynamic
    {
        /// <summary>
        /// 获取个人动态
        /// </summary>
        /// <param name="页数，总数，类型"></param>
        /// <returns></returns>
        Personjob GetPersonjobList(Persondynamic para);
    }
    public class Persondynamic
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }
    }
    public class Personjob
    {
        /// <summary>
        /// 职务
        /// </summary>
        public string jobname { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public string suozaibumen { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 动态内容List
        /// </summary>
        public List<Personinfo> zhuyaoneirong { get; set; }

    }
    public class Personinfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string createdate { get; set; }
        /// <summary>
        /// 点评人
        /// </summary>
        public string commentperson { get; set; }
        /// <summary>
        /// 来自哪
        /// </summary>
        public string from { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int dengji { get; set; }
        /// <summary>
        /// 范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 点评状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 总结
        /// </summary>
        public string summary { get; set; }
        /// <summary>
        /// 计划
        /// </summary>
        public string plan { get; set; }
        /// <summary>
        /// 体会
        /// </summary>
        public string experience { get; set; }
        /// <summary>
        /// 点评人点评
        /// </summary>
        public string commentcontent { get; set; }
        /// <summary>
        /// 点评时间
        /// </summary>
        public string commentdate { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public string zanNum { get; set; }
        /// <summary>
        /// 是否点赞
        /// </summary>
        public int shifouzan { get; set; }
        /// <summary>
        /// 回复
        /// </summary>
        public string commentNum { get; set; }
    }
    
}
