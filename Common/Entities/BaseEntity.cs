﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class BaseEntity
    {
       
        public int Id {  get; set; }
        public DateTime? CreatedDate { get; set; }= DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
