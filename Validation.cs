using System;
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
        // Проверка фамилии
        if (string.IsNullOrWhiteSpace(surname))
            return (false, "Фамилия не может быть пустой");

        if (!IsRussianLettersOnly(surname))
            return (false, "Фамилия должна содержать только буквы русского алфавита");

        // Проверка имени
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Имя не может быть пустым");

        if (!IsRussianLettersOnly(name))
            return (false, "Имя должно содержать только буквы русского алфавита");

        // Проверка отчества (может быть пустым)
        if (!string.IsNullOrWhiteSpace(middlename) && !IsRussianLettersOnly(middlename))
            return (false, "Отчество должно содержать только буквы русского алфавита");

        // Проверка местоположения
        if (string.IsNullOrWhiteSpace(location))
            return (false, "Местоположение не может быть пустым");

        // Проверка телефона
        if (string.IsNullOrWhiteSpace(phone))
            return (false, "Телефон не может быть пустым");

        if (!IsDigitsOnly(phone))
            return (false, "Телефон должен содержать только цифры");

        // Проверка email
        if (string.IsNullOrWhiteSpace(email))
            return (false, "Email не может быть пустым");

        if (!IsValidEmail(email))
            return (false, "Некорректный формат email");

        return (true, "Все данные корректны");
    }

    // Проверка, что строка содержит только русские буквы (большие и маленькие)
    private static bool IsRussianLettersOnly(string input)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[а-яёА-ЯЁ]+$");
    }

    // Проверка, что строка содержит только цифры
    private static bool IsDigitsOnly(string input)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d+$");
    }

    // Проверка корректности email (наличие @)
    private static bool IsValidEmail(string email)
    {
        try
        {
            // Простая проверка на наличие @
            if (!email.Contains("@"))
                return false;

            // Более строгая проверка с использованием MailAddress
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
