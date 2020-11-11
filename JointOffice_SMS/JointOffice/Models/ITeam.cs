using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface ITeam
    {
        Showapi_Res_Meaasge CreateTeam(CreateTeamPara para);
        Showapi_Res_Meaasge UpdateTeam(UpdateTeamPara para);
    }
    public class CreateTeamPara
    {
        public string name { get; set; }
        public List<string> memberidlist { get; set; }
    }
    public class UpdateTeamPara
    {
        public string teamid { get; set; }
        public int type { get; set; }
        public List<string> memberidlist { get; set; }
    }
}
