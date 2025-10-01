using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
public class ValidatorClient
{
    public static (bool isValid, string errorMessage) ValidateClientData(
        string surname,
        string name,
        string middlename,
        string location,
        string phone,
        string email,
        bool constClient)
    {
        if (string.IsNullOrWhiteSpace(surname))
            return (false, "Фамилия не может быть пустой");

        if (!IsRussianLettersOnly(surname))
            return (false, "Фамилия должна содержать только буквы русского алфавита");
        if (surname.Length > 20)
            return (false, "Фамилия не может быть длиннее 20 символов");

        if (string.IsNullOrWhiteSpace(name))
            return (false, "Имя не может быть пустым");

        if (!IsRussianLettersOnly(name))
            return (false, "Имя должно содержать только буквы русского алфавита");
        if (name.Length > 15)
            return (false, "Имя не может быть длиннее 15 символов");

        if (!IsRussianLettersOnly(middlename) && !string.IsNullOrWhiteSpace(middlename))
            return (false, "Отчество должно содержать только буквы русского алфавита");
        if (middlename.Length > 20)
            return (false, "Отчество не может быть длиннее 20 символов");
        if ((!IsDigitsOnly(phone) || phone.Length!=10 || !string.IsNullOrWhiteSpace(phone)) && phone != "10 цифр без +7 или 8")
            return (false, "Телефон должен содержать только 10 цифр");

        if (string.IsNullOrWhiteSpace(email))
            return (false, "Email не может быть пустым");

        if (!IsValidEmail(email))
            return (false, "Некорректный формат email");

        return (true, "Все данные корректны");
    }
    private static bool IsRussianLettersOnly(string input)
    {
        return Regex.IsMatch(input, @"^[а-яёА-ЯЁ]+$");
    }
    private static bool IsDigitsOnly(string input)
    {
        return Regex.IsMatch(input, @"^\d+$");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            if (!email.Contains("@"))
                return false;
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
