﻿using System.Collections.Generic;

namespace DNI.Shared.Web.ViewModels
{
    public class PageViewComponentViewModel
    {
        public IEnumerable<SectionViewComponentModel> Sections { get; set; }
        public IEnumerable<StyleSheetViewComponentModel> StyleSheets { get; set; }
    }
}