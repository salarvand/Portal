﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.Services;

namespace Orchard.Layouts.Controllers {
    public class LayoutController : Controller {
        private readonly IContentManager _contentManager;
        private readonly ILayoutManager _layoutManager;
        private readonly ILayoutModelMapper _mapper;

        public LayoutController(IContentManager contentManager, ILayoutManager layoutManager, ILayoutModelMapper mapper) {

            _contentManager = contentManager;
            _layoutManager = layoutManager;
            _mapper = mapper;
        }

        [HttpPost, ValidateInput(enableValidation: false)]
        public JsonResult ApplyTemplate(int? templateId = null, string layoutData = null, int? contentId = null, string contentType = null) {
            var template = templateId != null ? _layoutManager.GetLayout(templateId.Value) : null;
            var templateElements = template != null ? _layoutManager.LoadElements(template).ToList() : default(IEnumerable<IElement>);
            var describeContext = CreateDescribeElementsContext(contentId, contentType);
            var elementInstances = _mapper.ToLayoutModel(layoutData, describeContext).ToList();
            var updatedLayout = templateElements != null
                ? _layoutManager.ApplyTemplate(elementInstances, templateElements)
                : _layoutManager.DetachTemplate(elementInstances);

            var editorModel = _mapper.ToEditorModel(updatedLayout, describeContext);
            return Json(editorModel);
        }

        private DescribeElementsContext CreateDescribeElementsContext(int? contentId, string contentType) {
            var content = contentId != null && contentId != 0
                ? _contentManager.Get(contentId.Value, VersionOptions.Latest) ?? _contentManager.New(contentType)
                : _contentManager.New(contentType);

            return new DescribeElementsContext { Content = content };
        }
    }
}