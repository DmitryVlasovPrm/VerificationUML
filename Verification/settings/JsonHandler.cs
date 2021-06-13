using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Verification.settings {
    /// <summary>
    /// Класс для дессериал\сериал json
    /// </summary>
    class JsonHandler<T> {
        public T desserialize(string fileName, object output, Func<object, Boolean> checker) {
            var jsonString = File.ReadAllText(fileName);
            T settings;
            try {
                settings = JsonSerializer.Deserialize<T>(jsonString);
                checker(settings);
            } catch (InvalidCastException) {
                throw new Exception("Проблема с индексом для комбобокса");
            } catch (Exception e) when (e is ArgumentNullException || e is JsonException || e is NotSupportedException) {
                throw new Exception("Проблема с чтением файла");
            } catch (Exception e) when (e is InvalidCastException || e is FormatException) {
                throw new Exception("Значения настроек не соответствуют требуемым. Проверьте, что используются только действительные и целые числа");
            }
            return settings;
        }
    }
}
