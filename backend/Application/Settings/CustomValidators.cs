using FluentValidation;
using MongoDB.Bson;

namespace Application.Settings
{
    public static class CustomValidators 
    {
        public static IRuleBuilderOptions<T, string> MustBeObjectId<T>(this IRuleBuilder<T, string> ruleBuilder) {
            return ruleBuilder.Must(potentialId => ObjectId.TryParse(potentialId, out _)).WithMessage("The given ID was not an ObjectId.");
        }
    }
}