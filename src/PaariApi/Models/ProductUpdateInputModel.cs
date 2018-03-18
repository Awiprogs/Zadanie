using System;

namespace Paari.Models
{
    public class ProductUpdateInputModel : ProductCreateInputModel
    {
        public Guid Id { get; set; }
    }
}
