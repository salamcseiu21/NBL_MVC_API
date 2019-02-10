
namespace NBL.Models.Validators
{
    public  class Validator
    {
        public static bool ValidateProductBarCode(string barCode)
        {

            
            if (barCode?.Length == 13 && IsFirstThreeCharDigit(barCode))
            {
                return true;
            }
            return false;
        }

        private static bool IsFirstThreeCharDigit(string barCode)
        {
            var charArray = barCode.ToCharArray();
            if(char.IsDigit(charArray[0]) && char.IsDigit(charArray[1]) && char.IsDigit(charArray[2]))
            {
                return true;
            }
            return false;
        }
    }
}
