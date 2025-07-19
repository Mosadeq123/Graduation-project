namespace Store.G04.Core.Entities.Order
{
    public class ProductItemOrder
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } // تعديل النوع ليكون string
        public string PictureUrl { get; set; } // تعديل النوع ليكون string

        // إضافة منشئ لقبول القيم
        public ProductItemOrder(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
    }
}