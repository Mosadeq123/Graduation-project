using Microsoft.Extensions.Configuration;
using Store.G04.Core;
using Store.G04.Core.Dtos.Wishlist;
using Store.G04.Core.Entities;
using Store.G04.Core.Entities.Order;
using Store.G04.Core.Repositories.Contract;
using Store.G04.Core.Services.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.G04.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IWishlistRepository wishlistRepository, IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _wishlistRepository = wishlistRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<customerWishlistDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];
            // Retrieve the wishlist
            var basket = await _wishlistRepository.GetWishlistAsync(basketId);
            if (basket == null) return null;

            // Calculate shipping price
            decimal shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod != null)
                {
                    shippingPrice = deliveryMethod.Cost;
                }
            }

            // Update item prices if necessary
            if (basket.Items.Any())
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<RawMaterial, int>().GetAsync(item.Id);
                    if (product != null && item.Price != product.UnitPrice)
                    {
                        item.Price = product.UnitPrice;
                    }
                }
            }

            // Calculate the subtotal
            var subTotal = basket.Items.Sum(item => item.Price * item.Quantity);

            // Initialize Stripe PaymentIntent service
            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create a new PaymentIntent
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = (long)((subTotal + shippingPrice) * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(createOptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update existing PaymentIntent
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((subTotal + shippingPrice) * 100)
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            // Update the wishlist
            var updatedBasket = await _wishlistRepository.UpdateWishlistAsync(basket);

            // Map to DTO before returning
            return new customerWishlistDto
            {
                Id = updatedBasket.Id,
                Items = updatedBasket.Items.Select(item => new WishlistItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList(),
                DeliveryMethodId = updatedBasket.DeliveryMethodId,
                PaymentIntentId = updatedBasket.PaymentIntentId,
                ClientSecret = updatedBasket.ClientSecret
            };
        }
    }
}
