using RealEstate.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

namespace Presentation.Areas.Company.Models.PropertyVM
{
    public class PropertyVM
    {
        public Property Property { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TransactionList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UsersList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> PropertyTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Cities { get; set; }
        [ValidateNever]
        [DisplayName("Choose the cover Image of your property")]
        public IFormFile CoverImage { get; set; }
        [ValidateNever]
        //public List<PropertyImage> PopertyImage { get; set; }
        [DisplayName("Choose the Images of your Property")]
        public IFormFileCollection PropertyImages { get; set; }
        [ValidateNever]
        [BindNever]
        public int[] DeleteImageIdArr { get; set; } = new int[0];
        public string[] DeleteImageAws { get; set; }= new string[0];
        //[DisplayName("Choose the Images of your property")]
        //public List<PropertyImageVM> Images { get; set; }

    }
}
