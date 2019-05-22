using FluentValidation;
using InterceptorDemo.Core.Models;

namespace InterceptorDemo.Application.ValidationRules.FluentValidation
{
	public class ProductValidator : AbstractValidator<Product>
	{
		public ProductValidator()
		{
			RuleFor(p => p.CategoryId).NotEmpty();
			RuleFor(p => p.ProductName).NotEmpty().Length(2, 20);
			RuleFor(p => p.UnitPrice).GreaterThan(0);
			RuleFor(p => p.QuantityPerUnit).NotEmpty();
		}
	}
}
