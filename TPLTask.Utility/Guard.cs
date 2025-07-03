using JetBrains.Annotations;

namespace TPLTask.Utility;

/// <summary>
/// Утилитарный класс для валидации параметров с выбрасыванием исключений при ошибках
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class Guard
{
    private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars();

    /// <summary>
    /// Проверяет, что объект не равен <c>null</c>. 
    /// В случае <c>null</c> выбрасывает <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="data"> Объект для проверки </param>
    public static void NotNull(string paramName, object data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(paramName, $"Parameter '{paramName}' is null!");
        }
    }

    /// <summary>
    /// Проверяет, что строка не равна <c>null</c> и не пуста. 
    /// В противном случае выбрасывает <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="text"> Строка для проверки </param>
    public static void NotNullOrEmpty(string paramName, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException(paramName, $"Parameter '{paramName}' is null or empty!");
        }
    }

    /// <summary>
    /// Проверяет, что строка не содержит ни одного символа из указанного массива.
    /// В противном случае выбрасывает <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="text"> Строка для проверки </param>
    /// <param name="invalidSymbols"> Массив недопустимых символов </param>
    public static void NotHasAnySymbol(string paramName, string text, char[] invalidSymbols)
    {
        var indexOfInvalidChar = text.IndexOfAny(invalidSymbols);

        if (indexOfInvalidChar != -1)
        {
            throw new ArgumentException($"Incorrect symbol '{text[indexOfInvalidChar]}' into '{paramName}'!", nameof(paramName));
        }
    }
        
    /// <summary>
    /// Проверяет, что строка не содержит указанный недопустимый символ.
    /// В противном случае выбрасывает <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="text"> Строка для проверки </param>
    /// <param name="invalidSymbol"> Недопустимый символ </param>
    public static void NotHasSymbol(string paramName, string text, char invalidSymbol)
    {
        var indexOfInvalidChar = text.IndexOf(invalidSymbol);

        if (indexOfInvalidChar != -1)
        {
            throw new ArgumentException($"Incorrect symbol '{text[indexOfInvalidChar]}' into '{paramName}'!", nameof(paramName));
        }
    }

    /// <summary>
    /// Проверяет, что строка является допустимым именем файла, не содержащим недопустимых символов.
    /// В противном случае выбрасывает <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="text"> Строка, представляющая путь к файлу </param>
    public static void NotInvalidFilePath(string paramName, string text)
    {
        NotHasAnySymbol(paramName, text, Path.GetInvalidFileNameChars());
    }

    /// <summary>
    /// Проверяет строку имени файла и заменяет все недопустимые символы на символ подчёркивания '_'.
    /// Изменения применяются напрямую к переданному по ссылке параметру fileName.
    /// </summary>
    /// <param name="fileName"> Ссылка на строку, представляющую имя файла, которую требуется очистить от недопустимых символов. </param>
    public static void DoFileNameValid(ref string fileName)
    {
        if (fileName.IndexOfAny(_invalidFileNameChars) != -1)
        {
            fileName = new(fileName.Select(c => _invalidFileNameChars.Contains(c) ? '_' : c).ToArray());
        }
    }

    /// <summary>
    /// Проверяет, что объект можно привести к типу <typeparamref name="T"/>. 
    /// В случае неудачи выбрасывает <see cref="ArgumentException"/>.
    /// </summary>
    /// <typeparam name="T"> Ожидаемый тип объекта </typeparam>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="data"> Объект для проверки </param>
    /// <param name="tData"> Результат приведения типа при успешной проверке </param>
    public static void Is<T>(string paramName, object data, out T tData)
    {
        if (!(data is T otherData))
        {
            throw new ArgumentException($"Parameter '{paramName}' is not '{nameof(T)}'!", paramName);
        }

        tData = otherData;
    }

    /// <summary>
    /// Проверяет, что путь до файла указывает на файл, который имеет указанное расширение.
    /// В противном случае выбрасывает <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="paramName"> Имя параметра </param>
    /// <param name="filePath"> Путь к файлу </param>
    /// <param name="expectedExtension"> Ожидаемое расширение (без точки) </param>
    public static void NotInvalidFileExtension(string paramName, string filePath, string expectedExtension)
    {
        NotNullOrEmpty(paramName, filePath);
        NotNullOrEmpty(nameof(expectedExtension), expectedExtension);

        var actualExtension = Path.GetExtension(filePath).TrimStart('.').ToLowerInvariant();
        var normalizedExpected = expectedExtension.ToLowerInvariant();

        if (actualExtension != normalizedExpected)
        {
            throw new ArgumentException($"Parameter '{paramName}' must have extension '.{expectedExtension}', but was '.{actualExtension}'.", paramName);
        }
    }
}