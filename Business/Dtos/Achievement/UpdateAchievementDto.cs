using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.Achievement
{
    public class UpdateAchievementDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CertificateUrl { get; set; }
    }
}
