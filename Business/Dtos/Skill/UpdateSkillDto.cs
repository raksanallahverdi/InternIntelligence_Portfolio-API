using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.Skill
{
    public class UpdateSkillDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProficiencyLevel { get; set; }
    }
}
