﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.InputModel
{
    public class StoryDeleteInputModel
    {
        public Project Project { get; set; }
        public string ProjectId { get; set; }
        public Sprint Sprint { get; set; }
        public string Id { get; set; }

        [DisplayName("説明")]
        public string Description { get; set; }
    }
}