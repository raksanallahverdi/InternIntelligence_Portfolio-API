using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.Project
{
    public class UpdateProjectDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string GitHubLink { get; set; }
        public string LiveDemoLink { get; set; }

    }
}
